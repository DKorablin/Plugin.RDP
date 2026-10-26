using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using SAL.Flatbed;

namespace Plugin.RDP.RDP
{
	/// <summary>Fills the Windows Security credential dialog that is shown when the server policy requires the password to be entered manually</summary>
	/// <remarks>
	/// RDP ActiveX doesn't raise any event when it shows this dialog (Group Policy: "Always prompt for password upon connection").
	/// The dialog is hosted by CredentialUIBroker.exe, so it is detected by window class and filled using UI Automation.
	/// The password is filled only once per <see cref="Start"/> call to avoid an endless loop (and account lockout) when the password is wrong.
	/// </remarks>
	internal sealed class CredentialPromptAutoFill : IDisposable
	{
		private const String DialogClassName = "Credential Dialog Xaml Host";
		private const String OkButtonAutomationId = "OkButton";
		/// <summary>Time to wait till XAML content of the dialog is loaded</summary>
		private const Int32 ElementTimeout = 5000;

		private readonly ITraceSource _trace;
		/// <summary>Reference to the callback must be kept while the hook is registered</summary>
		private readonly Native.WinEventProc _winEventProc;
		private IntPtr _hook = IntPtr.Zero;
		private String _userName;
		private String _password;
		/// <summary>1 - waiting for the dialog, 0 - dialog already filled or watcher is stopped</summary>
		private Int32 _isArmed = 0;

		public CredentialPromptAutoFill(ITraceSource trace)
		{
			this._trace = trace ?? throw new ArgumentNullException(nameof(trace));
			this._winEventProc = new Native.WinEventProc(this.WinEventCallback);
		}

		/// <summary>Starts waiting for the credential dialog</summary>
		/// <remarks>Must be called from the UI thread. Hook callbacks are delivered through the message loop of the calling thread</remarks>
		/// <param name="userName">User name used to make sure that the dialog belongs to the current connection. Can be empty</param>
		/// <param name="password">Password to enter into the dialog</param>
		public void Start(String userName, String password)
		{
			if(String.IsNullOrEmpty(password))
				throw new ArgumentNullException(nameof(password));

			this.Stop();

			this._userName = userName;
			this._password = password;

			this._hook = Native.SetWinEventHook(Native.EVENT_OBJECT_SHOW, Native.EVENT_OBJECT_SHOW, IntPtr.Zero, this._winEventProc, 0, 0, Native.WINEVENT_OUTOFCONTEXT | Native.WINEVENT_SKIPOWNPROCESS);
			if(this._hook == IntPtr.Zero)
			{
				this._trace.TraceEvent(TraceEventType.Warning, 1, "Failed to register credential dialog hook. Password prompt will not be filled automatically");
				return;
			}

			Interlocked.Exchange(ref this._isArmed, 1);
		}

		/// <summary>Stops waiting for the credential dialog</summary>
		public void Stop()
		{
			Interlocked.Exchange(ref this._isArmed, 0);
			if(this._hook != IntPtr.Zero)
			{
				Native.UnhookWinEvent(this._hook);
				this._hook = IntPtr.Zero;
			}
			this._userName = null;
			this._password = null;
		}

		public void Dispose()
			=> this.Stop();

		private void WinEventCallback(IntPtr hWinEventHook, UInt32 eventType, IntPtr hWnd, Int32 idObject, Int32 idChild, UInt32 eventThread, UInt32 eventTime)
		{
			if(hWnd == IntPtr.Zero || idObject != Native.OBJID_WINDOW || idChild != 0)
				return;
			if(this._isArmed == 0 || !CredentialPromptAutoFill.IsCredentialDialog(hWnd))
				return;

			String userName = this._userName;
			String password = this._password;
			//UI Automation calls are slow and must not block the UI thread
			ThreadPool.QueueUserWorkItem(_ => this.FillDialog(hWnd, userName, password));
		}

		private void FillDialog(IntPtr hWnd, String userName, String password)
		{
			try
			{
				AutomationElement dialog = AutomationElement.FromHandle(hWnd);
				AutomationElement passwordBox = CredentialPromptAutoFill.WaitForElement(dialog,
					new AndCondition(
						new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit),
						new PropertyCondition(AutomationElement.IsPasswordProperty, true)));
				if(passwordBox == null)
				{
					this._trace.TraceEvent(TraceEventType.Warning, 1, "Credential dialog detected but password field not found");
					return;
				}

				if(!CredentialPromptAutoFill.IsDialogForUser(dialog, userName))
				{
					this._trace.TraceEvent(TraceEventType.Verbose, 1, "Credential dialog detected but it doesn't belong to user {0}", userName);
					return;
				}

				if(Interlocked.CompareExchange(ref this._isArmed, 0, 1) != 1)
					return;//Already filled or stopped

				CredentialPromptAutoFill.SetPassword(passwordBox, password);
				CredentialPromptAutoFill.Submit(dialog, passwordBox);
				this._trace.TraceEvent(TraceEventType.Information, 1, "Password prompt filled automatically for user {0}", userName);
			} catch(ElementNotAvailableException)
			{
				//Dialog was closed by user or by the system
			} catch(Exception exc)
			{
				this._trace.TraceData(TraceEventType.Error, 10, exc);
			}
		}

		private static Boolean IsCredentialDialog(IntPtr hWnd)
		{
			StringBuilder className = new StringBuilder(256);
			return Native.GetClassName(hWnd, className, className.Capacity) > 0
				&& String.Equals(className.ToString(), CredentialPromptAutoFill.DialogClassName, StringComparison.Ordinal);
		}

		/// <summary>Checks that the dialog shows the same user name as the current connection</summary>
		/// <remarks>Several connections can wait for the dialog at the same time, the user name is the only way to tell them apart</remarks>
		private static Boolean IsDialogForUser(AutomationElement dialog, String userName)
		{
			if(String.IsNullOrEmpty(userName))
				return true;

			//DOMAIN\user -> user; user@domain -> user
			Int32 index = userName.LastIndexOf('\\');
			if(index >= 0)
				userName = userName.Substring(index + 1);
			index = userName.IndexOf('@');
			if(index > 0)
				userName = userName.Substring(0, index);

			foreach(AutomationElement element in dialog.FindAll(TreeScope.Descendants, Condition.TrueCondition))
			{
				String name = element.Current.Name;
				if(name != null && name.IndexOf(userName, StringComparison.OrdinalIgnoreCase) >= 0)
					return true;
			}
			return false;
		}

		private static void SetPassword(AutomationElement passwordBox, String password)
		{
			if(passwordBox.TryGetCurrentPattern(ValuePattern.Pattern, out Object pattern) && !((ValuePattern)pattern).Current.IsReadOnly)
				((ValuePattern)pattern).SetValue(password);
			else
			{
				passwordBox.SetFocus();
				SendKeys.SendWait(CredentialPromptAutoFill.EscapeSendKeys(password));
			}
		}

		private static void Submit(AutomationElement dialog, AutomationElement passwordBox)
		{
			AutomationElement okButton = dialog.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, CredentialPromptAutoFill.OkButtonAutomationId));
			if(okButton != null && okButton.TryGetCurrentPattern(InvokePattern.Pattern, out Object pattern))
				((InvokePattern)pattern).Invoke();
			else
			{
				passwordBox.SetFocus();
				SendKeys.SendWait("{ENTER}");
			}
		}

		private static AutomationElement WaitForElement(AutomationElement root, Condition condition)
		{
			Stopwatch sw = Stopwatch.StartNew();
			do
			{
				AutomationElement result = root.FindFirst(TreeScope.Descendants, condition);
				if(result != null)
					return result;
				Thread.Sleep(100);
			} while(sw.ElapsedMilliseconds < CredentialPromptAutoFill.ElementTimeout);
			return null;
		}

		/// <summary>Escapes characters that have special meaning in <see cref="SendKeys"/></summary>
		private static String EscapeSendKeys(String value)
		{
			StringBuilder result = new StringBuilder(value.Length);
			foreach(Char c in value)
				switch(c)
				{
				case '+': case '^': case '%': case '~':
				case '(': case ')': case '{': case '}': case '[': case ']':
					result.Append('{').Append(c).Append('}');
					break;
				default:
					result.Append(c);
					break;
				}
			return result.ToString();
		}
	}
}
