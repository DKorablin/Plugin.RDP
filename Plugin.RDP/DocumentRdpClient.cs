using System;
using System.Drawing;
using System.Windows.Forms;
using MSTSCLib;
using Plugin.RDP.Bll;
using Plugin.RDP.Properties;
using Plugin.RDP.RDP;
using SAL.Flatbed;
using SAL.Windows;
using System.Diagnostics;

namespace Plugin.RDP
{
	internal partial class DocumentRdpClient : UserControl, IPluginSettings<DocumentRdpClientSettings>
	{
		private DocumentRdpClientSettings _settings;
		private Boolean _isControlCreated = false;
		private FormWindowState _lastWindowState;
		/// <summary>User logged in and display control channel is available</summary>
		private Boolean _isLoggedIn = false;
		/// <summary>Remote size was changed by full screen mode and must be restored even if client area is unchanged</summary>
		private Boolean _isResizeForced = false;
		/// <summary>Resize mode captured on connect. Changes in plugin settings are applied only to new connections</summary>
		private RdpClient.SessionResizeMode _resizeMode = RdpClient.SessionResizeMode.Auto;
		/// <summary>Remote session resize is delayed till user stops resizing the window</summary>
		private readonly Timer _resizeTimer = new Timer() { Interval = 500, };
		/// <summary>Fills Windows Security dialog when server policy requires to enter the password manually</summary>
		private CredentialPromptAutoFill _credentialPromptAutoFill;

		Object IPluginSettings.Settings => this.Settings;

		public DocumentRdpClientSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new DocumentRdpClientSettings();
					this._settings.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(this.Settings_PropertyChanged);
				}
				return this._settings;
			}
		}

		SettingsDataSet.TreeRow _treeRow;
		private SettingsDataSet.TreeRow TreeRow
			=> this._treeRow == null && this.Settings.TreeId != null
				? this._treeRow = this.Plugin.Settings.XmlSettings.GetTreeNode(this.Settings.TreeId.Value)
				: this._treeRow;

		SettingsDataSet.RdpClientRow _rdpClientRow;
		private SettingsDataSet.RdpClientRow RdpClientRow
			=> this._rdpClientRow ?? (this._rdpClientRow = this.Plugin.Settings.XmlSettings.GetClientRow(this.TreeRow.TreeID));

		/// <summary>Remote session follows the client area (or the screen in full screen mode). Explicitly specified desktop size is never changed</summary>
		private Boolean IsSessionResizable
			=> this.RdpClientRow.DesktopSizeI.SameAsClient || this.RdpClientRow.DesktopSizeI.FullScreen;

		private RdpClient _rdpClient;
		private RdpClient RdpClient
		{
			get
			{
				if(this._rdpClient == null)
				{
					RdpClient.Initialize(this.Plugin, this);
					this._rdpClient = RdpClient.AllocClient(this.Plugin, this);

					this._rdpClient.OnConnected += new EventHandler(this.RdpClient_OnConnected);
					this._rdpClient.OnLoginComplete += new EventHandler(this.RdpClient_OnLoginComplete);
					this._rdpClient.OnDisconnected += new AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEventHandler(this.RdpClient_OnDisconnected);
					this._rdpClient.OnRequestLeaveFullScreen += new EventHandler(this.RdpClient_OnRequestLeaveFullScreen);
					this._rdpClient.OnEnterFullScreenMode += new EventHandler(this.RdpClient_OnEnterFullScreenMode);
					this._rdpClient.OnLeaveFullScreenMode += new EventHandler(this.RdpClient_OnLeaveFullScreenMode);

					this._rdpClient.AdvancedSettings2.ContainerHandledFullScreen = 0;//The client automatically opens in full screen.
					this._rdpClient.AdvancedSettings2.SmartSizing = false;

					this._rdpClient.Control.Size = this.Size;
					this._rdpClient.Control.Show();
				}
				return this._rdpClient;
			}
		}

		private PluginWindows Plugin => (PluginWindows)this.Window.Plugin.Instance;
		private IWindow Window => (IWindow)base.Parent;

		public DocumentRdpClient()
		{
			this.InitializeComponent();
			this._resizeTimer.Tick += new EventHandler(this.ResizeTimer_Tick);
		}

		protected override void OnCreateControl()
		{
			if(_isControlCreated)
				return;

			if(this.Settings.TreeId == null)
			{
				this.Window.Close();
				return;
			}
			this.Window.Caption = $"{this.TreeRow.Name} - Terminal Client";
			this.Window.SetDockAreas(DockAreas.Document | DockAreas.Float);

			this.Window.Shown += new EventHandler(this.Window_Shown);
			this.Window.Closed += new EventHandler(this.Window_Closed);
			_lastWindowState = this.ParentForm?.WindowState ?? FormWindowState.Normal;//Exception if ParentForm is null
			if(this.ParentForm != null)
				this.ParentForm.ClientSizeChanged += this.ParentForm_ClientSizeChanged;

			this.Plugin.Settings.XmlSettings.RdpClientStateChange += new EventHandler<RdpStateEventArgs>(this.XmlSettings_RdpClientConnected);
			base.OnCreateControl();
			_isControlCreated = true;
			this.SetConnectingState(false);
		}

		private void ParentForm_ClientSizeChanged(Object sender, EventArgs e)
		{
			if(_lastWindowState != this.ParentForm.WindowState)
			{
				switch(_lastWindowState = this.ParentForm.WindowState)
				{
				case FormWindowState.Maximized:
					this.RdpClient.GoFullScreen();
					break;
				case FormWindowState.Normal:
					this.RdpClient.LeaveFullScreen();
					this.FitSessionAfterLeaveFullScreen();
					break;
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.ScheduleResizeSession();
		}

		/// <summary>Restarts resize timer so remote session will be resized after user stops resizing the window</summary>
		private void ScheduleResizeSession()
		{
			if(!this._isLoggedIn || this._rdpClient == null || !this.IsSessionResizable)
				return;

			this._resizeTimer.Stop();
			this._resizeTimer.Start();
		}

		private void ResizeTimer_Tick(Object sender, EventArgs e)
		{
			this._resizeTimer.Stop();

			if(!this._isLoggedIn || this._rdpClient == null || !this.IsSessionResizable)
				return;
			if(this.ParentForm?.WindowState == FormWindowState.Minimized || this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
				return;

			//In full screen mode remote session is fitted to the screen where the control is shown
			Boolean isFullScreen = this.RdpClient.MsRdpClient.FullScreen;
			Size size = isFullScreen
				? Screen.FromControl(this).Bounds.Size
				: this.ClientSize;

			Boolean allowReconnect = this.RdpClientRow.ConnectionTypeI != ConnectionType.VirtualMachineConsoleConnect;
			RdpClient.ResizeMethod method = this.RdpClient.ResizeSession(size, this._resizeMode, allowReconnect, this._isResizeForced);
			this._isResizeForced = isFullScreen;//Remote size differs from client area and must be restored after leaving full screen
			if(method != RdpClient.ResizeMethod.None)
				this.Plugin.Trace.TraceEvent(TraceEventType.Verbose, 1, "Session {0} resized to {1}x{2} using {3}", this.RdpClientRow.Server, size.Width, size.Height, method);
		}

		/// <summary>Fits remote session into the client area after leaving full screen mode</summary>
		private void FitSessionAfterLeaveFullScreen()
		{
			if(!this.IsSessionResizable)
				this.RdpClient.AdvancedSettings2.SmartSizing = true;//Remote desktop is bigger than client area
			else if(this._resizeMode != RdpClient.SessionResizeMode.None)
			{
				this._isResizeForced = true;
				this.ScheduleResizeSession();
			}
		}

		private void Settings_PropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch(e.PropertyName)
			{
			case nameof(DocumentRdpClientSettings.IsConnected):
				if(this.Settings.IsConnected)
					this.Connect();
				else
					this.Disconnect();
				break;
			}
		}

		private void Window_Shown(Object sender, EventArgs e)
			=> this.Connect();

		private void Window_Closed(Object sender, EventArgs e)
		{
			this.Plugin.Settings.XmlSettings.RdpClientStateChange -= new EventHandler<RdpStateEventArgs>(this.XmlSettings_RdpClientConnected);

			this._isLoggedIn = false;
			this._resizeTimer.Stop();
			this._resizeTimer.Dispose();
			this._credentialPromptAutoFill?.Dispose();
			this._credentialPromptAutoFill = null;

			this.RdpClient.OnConnected -= new EventHandler(this.RdpClient_OnConnected);
			this.RdpClient.OnLoginComplete -= new EventHandler(this.RdpClient_OnLoginComplete);
			this.RdpClient.OnDisconnected -= new AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEventHandler(this.RdpClient_OnDisconnected);
			this.RdpClient.OnRequestLeaveFullScreen -= new EventHandler(this.RdpClient_OnRequestLeaveFullScreen);
			this.RdpClient.OnEnterFullScreenMode -= new EventHandler(this.RdpClient_OnEnterFullScreenMode);
			this.RdpClient.OnLeaveFullScreenMode -= new EventHandler(this.RdpClient_OnLeaveFullScreenMode);

			if(this.RdpClient.ConnectionStatus == RdpClient.ConnectionState.Connected)
			{
				this.Disconnect();
				this.Plugin.Settings.XmlSettings.OnClientChangeState(this.Settings.TreeId.Value, RdpStateEventArgs.StateType.Disconnect);
			}
		}

		private void XmlSettings_RdpClientConnected(Object sender, RdpStateEventArgs e)
		{
			if(e.TreeId == this.Settings.TreeId)
			{
				switch(e.State){
				case RdpStateEventArgs.StateType.Connect:
					if(this.RdpClient.ConnectionStatus == RdpClient.ConnectionState.Disconnected)
						this.Connect();
					break;
				case RdpStateEventArgs.StateType.Disconnect:
					this.Disconnect();
					break;
				case RdpStateEventArgs.StateType.Focus:
					base.Focus();
					break;
				}
			}
		}

		private void Disconnect()
		{
			if(this.RdpClient.ConnectionStatus == RdpClient.ConnectionState.Connected)
			{//TODO: Test on older versions of RDP. Perhaps we need to switch completely to RequestClose.
				if(this.RdpClient.Version.Major < 10)
					this.RdpClient.MsRdpClient.Disconnect();
				else
					this.RdpClient.MsRdpClient.RequestClose();
			}
		}

		private void Connect()
		{
			if(this.RdpClient.ConnectionStatus == RDP.RdpClient.ConnectionState.Connecting || this.RdpClient.ConnectionStatus == RDP.RdpClient.ConnectionState.Connected)
			{//An error appears in SAL.EnvDTE when attempting to reopen the RDPClient window.
				this.Plugin.Trace.TraceEvent(TraceEventType.Information, 0, "Attempt to call Connect to {0} Ctrl. Server: {1}", this.RdpClient.ConnectionStatus, this.RdpClientRow.Server);
				return;
			}

			PluginSettings settings = this.Plugin.Settings;
			this._resizeMode = settings.ResizeMode;
			this.RdpClient.MsRdpClient.Server = String.IsNullOrEmpty(this.RdpClientRow.Server)
				? this.TreeRow.Name
				: this.RdpClientRow.Server;//If Server is empty then use TreeRow Name as Server Name

			if(!String.IsNullOrEmpty(this.RdpClientRow.UserName))
				this.RdpClient.MsRdpClient.UserName = this.RdpClientRow.UserName;

			if(!String.IsNullOrEmpty(this.RdpClientRow.PasswordI))
				this.RdpClient.AdvancedSettings2.ClearTextPassword = this.RdpClientRow.PasswordI;

			if(!String.IsNullOrEmpty(this.RdpClientRow.DomainI))
				this.RdpClient.MsRdpClient.Domain = this.RdpClientRow.DomainI;

			IMsRdpClientAdvancedSettings advanced = this.RdpClient.AdvancedSettings2;
			IMsRdpClientAdvancedSettings4 advanced4 = this.RdpClient.AdvancedSettings5;
			IMsRdpClientAdvancedSettings5 advanced5 = this.RdpClient.AdvancedSettings6;
			IMsRdpClientAdvancedSettings6 advanced6 = this.RdpClient.AdvancedSettings7;
			IMsRdpClientAdvancedSettings7 advanced7 = this.RdpClient.AdvancedSettings8;

			advanced.RDPPort = this.RdpClientRow.ConnectPortI;

			advanced.PerformanceFlags = (Int32)this.RdpClientRow.PerformanceFlagsI;
			advanced.GrabFocusOnConnect = false;

			advanced.EnableWindowsKey = 1;

			//Hot keys (virtual-key code only, modifiers are fixed by the RDP control)
			if(settings.AltTab != Keys.None)
				advanced.HotKeyAltTab = (Int32)settings.AltTab;
			if(settings.AltShiftTab != Keys.None)
				advanced.HotKeyAltShiftTab = (Int32)settings.AltShiftTab;
			if(settings.AltEsc != Keys.None)
				advanced.HotKeyAltEsc = (Int32)settings.AltEsc;
			if(settings.AltSpace != Keys.None)
				advanced.HotKeyAltSpace = (Int32)settings.AltSpace;
			if(settings.CtrlEsc != Keys.None)
				advanced.HotKeyCtrlEsc = (Int32)settings.CtrlEsc;
			if(settings.CtrlAltDel != Keys.None)
				advanced.HotKeyCtrlAltDel = (Int32)settings.CtrlAltDel;
			if(settings.FullScreen != Keys.None)
				advanced.HotKeyFullScreen = (Int32)settings.FullScreen;

			advanced.RedirectDrives = (this.RdpClientRow.RedirectI & RedirectFlags.Drives) == RedirectFlags.Drives;
			advanced.RedirectPorts = (this.RdpClientRow.RedirectI & RedirectFlags.Ports) == RedirectFlags.Ports;
			advanced.RedirectPrinters = (this.RdpClientRow.RedirectI & RedirectFlags.Printers) == RedirectFlags.Printers;
			advanced.RedirectSmartCards = (this.RdpClientRow.RedirectI & RedirectFlags.SmartCards) == RedirectFlags.SmartCards;

			advanced.MinutesToIdleTimeout = this.RdpClientRow.MinutesToIdle;

			this.RdpClient.SecuredSettings2.AudioRedirectionMode = this.RdpClientRow.RedirectAudioCapture ? 1 : 0;
			this.RdpClient.SecuredSettings2.KeyboardHookMode = (Int32)this.RdpClientRow.KeyboardHookI;

			advanced4.AuthenticationLevel = (UInt32)this.RdpClientRow.AuthenticationLevelI;

			if(advanced5 != null)
			{
				advanced5.EnableAutoReconnect = true;
				advanced5.AudioRedirectionMode = (UInt32)this.RdpClientRow.RedirectAudioI;
				advanced5.RedirectClipboard = (this.RdpClientRow.RedirectI & RedirectFlags.Clipboard) == RedirectFlags.Clipboard;
				advanced5.RedirectPOSDevices = (this.RdpClientRow.RedirectI & RedirectFlags.PointOfService) == RedirectFlags.PointOfService;
			}

			if(advanced6 != null)
				advanced6.EnableCredSspSupport = true;

			if(this.RdpClientRow.ConnectionTypeI == ConnectionType.VirtualMachineConsoleConnect)
			{
				advanced.RDPPort = RdpClient.DefaultVMConsoleConnectPort;
				advanced.ConnectToServerConsole = true;
				if(advanced6 != null)
				{
					advanced6.AuthenticationLevel = 0u;
					advanced6.AuthenticationServiceClass = "Microsoft Virtual Console Service";
				}
				if(advanced7 != null)
				{
					advanced7.PCB = this.RdpClientRow.VirtualMachineId;
					advanced7.NegotiateSecurityLayer = false;
				}
			} else
			{
				advanced.ConnectToServerConsole = this.RdpClientRow.ConnectConsole;
				advanced4.EnableAutoReconnect = true;
				advanced4.MaxReconnectAttempts = 20;
				if(advanced6 != null)
					advanced6.ConnectToAdministerServer = this.RdpClientRow.ConnectConsole;

				IMsRdpClientNonScriptable4 nonScriptable = (IMsRdpClientNonScriptable4)this.RdpClient.GetOcx();
				nonScriptable.PromptForCredentials = false;
				nonScriptable.NegotiateSecurityLayer = true;

				if(advanced7 != null)
					advanced7.AudioQualityMode = (UInt32)this.RdpClientRow.RedirectAudioQualityI;

				//Remote application
				if(this.RdpClientRow.RunApplication)
				{
					this.RdpClient.SecuredSettings2.StartProgram = this.RdpClientRow.RunApplicationPathI;
					this.RdpClient.SecuredSettings2.WorkDir = this.RdpClientRow.RunApplicationWorkingDirI;
					advanced.MaximizeShell = this.RdpClientRow.RunApplicationMaximize ? 1 : 0;
				}

				this.RdpClient.MsRdpClient.ColorDepth = this.RdpClientRow.ColorDepthI;

				//Desktop size
				if(this.RdpClientRow.DesktopSizeI.FullScreen)
				{
					Size size = Screen.FromControl(this).Bounds.Size;

					this.RdpClient.SetDesktopSize(size);

					this.RdpClient.GoFullScreen();
				} else if(this.RdpClientRow.DesktopSizeI.SameAsClient)
					this.RdpClient.SetDesktopSize(base.Size);
				else if(!this.RdpClientRow.DesktopSizeI.Size.IsEmpty)
					this.RdpClient.SetDesktopSize(this.RdpClientRow.DesktopSizeI.Size);
				else
					throw new InvalidOperationException("Unknown type of DesktopSize specified");
			}

			//Gateway
			IMsRdpClientTransportSettings transportSettings = this.RdpClient.TransportSettings;
			if(this.RdpClientRow.GatewayEnabled)
			{
				UInt32 gatewayUsageMethod = this.RdpClientRow.GatewayBypass ? 2u : 1u;
				transportSettings.GatewayProfileUsageMethod = 1u;
				transportSettings.GatewayUsageMethod = gatewayUsageMethod;
				UInt32 value = (UInt32)this.RdpClientRow.GatewayLogonMethodI;
				transportSettings.GatewayCredsSource = value;
				transportSettings.GatewayHostname =this.RdpClientRow.GatewayServer;
				IMsRdpClientTransportSettings2 transportSettings2 = this.RdpClient.TransportSettings2;
				if(transportSettings2 != null)
				{
					transportSettings2.GatewayCredSharing = this.RdpClientRow.GatewayShareAuth ? 1u : 0u;
					transportSettings2.GatewayUsername =this.RdpClientRow.GatewayUserName;
					transportSettings2.GatewayDomain = this.RdpClientRow.GatewayDomain;
					transportSettings2.GatewayPassword = this.RdpClientRow.GatewayPassword;
				}
			} else
			{
				transportSettings.GatewayProfileUsageMethod = 0u;
				transportSettings.GatewayUsageMethod = 0u;
			}

			this.RdpClient.MsRdpClient.ConnectingText = "Connecting...";
			this.StartCredentialPromptAutoFill();
			this.RdpClient.MsRdpClient.Connect();
		}

		/// <summary>Starts waiting for the Windows Security dialog if the feature is enabled and the password is stored</summary>
		private void StartCredentialPromptAutoFill()
		{
			this._credentialPromptAutoFill?.Stop();

			if(!this.Plugin.Settings.AutoFillPasswordPrompt || String.IsNullOrEmpty(this.RdpClientRow.PasswordI))
				return;

			if(this._credentialPromptAutoFill == null)
				this._credentialPromptAutoFill = new CredentialPromptAutoFill(this.Plugin.Trace);
			this._credentialPromptAutoFill.Start(this.RdpClientRow.UserName, this.RdpClientRow.PasswordI);
		}

		private void RdpClient_OnConnected(Object sender, EventArgs e)
		{
			this.SetConnectingState(true);
			this.Plugin.Settings.XmlSettings.OnClientChangeState(this.Settings.TreeId.Value, RdpStateEventArgs.StateType.Connect);
		}

		private void RdpClient_OnLoginComplete(Object sender, EventArgs e)
		{
			this._isLoggedIn = true;
			this._credentialPromptAutoFill?.Stop();
			this.ScheduleResizeSession();//Client area could be changed while user was logging in
		}

		System.Threading.Timer _xpHack;
		private void RdpClient_OnDisconnected(Object sender, AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEvent e)
		{
			this._isLoggedIn = false;
			this._isResizeForced = false;
			this._resizeTimer.Stop();
			this._credentialPromptAutoFill?.Stop();
			this.RdpClient.ResetResizeState();

			this.SetConnectingState(false);
			this.RdpClient.MsRdpClient.ConnectingText = "Disconnected";
			if(e != null)
			{//Event could be called from code
				String disconnectionReason = this.RdpClient.GetErrorDescription((UInt32)e.discReason);
				this.Plugin.Trace.TraceEvent(TraceEventType.Verbose, 1, disconnectionReason);
			}

			this.Plugin.Settings.XmlSettings.OnClientChangeState(this.Settings.TreeId.Value, RdpStateEventArgs.StateType.Disconnect);

			if(this.Plugin.Settings.CloseWindowAfterDisconnect)
			{
				if(this.RdpClient.Version.Build <= 6001)//An error occurs when attempting to close a window immediately after a disconnect. (This issue doesn't occur in Win7.)
					this._xpHack = new System.Threading.Timer(CloseWindowByTimer, this, 1000, System.Threading.Timeout.Infinite);
				else
					this.Window.Close();
			}
		}

		private void RdpClient_OnRequestLeaveFullScreen(Object sender, EventArgs e)
		{
			this.Plugin.Trace.TraceEvent(TraceEventType.Verbose, 1, "RDP requesting to leave full screen for: {0}", this.RdpClientRow.Server);

			// Force control to refresh and resize
			this.RdpClient.Control.Refresh();
			this.RdpClient.Control.Size = this.Size;
			this.RdpClient.Control.Invalidate();

			// Ensure parent form state is tracked correctly
			if(this.ParentForm != null)
				_lastWindowState = this.ParentForm.WindowState;

			this.FitSessionAfterLeaveFullScreen();
		}

		/// <summary>Full screen mode is entered by hot key, connection bar or from code</summary>
		private void RdpClient_OnEnterFullScreenMode(Object sender, EventArgs e)
		{
			this.Plugin.Trace.TraceEvent(TraceEventType.Verbose, 1, "RDP entered full screen for: {0}", this.RdpClientRow.Server);
			this.ScheduleResizeSession();//Fit remote session to the screen
		}

		/// <summary>Full screen mode is left by hot key, connection bar or from code</summary>
		private void RdpClient_OnLeaveFullScreenMode(Object sender, EventArgs e)
		{
			if(this.ParentForm != null)
				_lastWindowState = this.ParentForm.WindowState;

			this.FitSessionAfterLeaveFullScreen();
		}

		private static void CloseWindowByTimer(Object pThis)
		{
			DocumentRdpClient document = (DocumentRdpClient)pThis;
			document._xpHack.Dispose();
			document.Window.Close();
		}

		private void SetConnectingState(Boolean isConnected)
		{
			this.Settings.IsConnected = isConnected;
			switch(this.TreeRow.RdpClientRow.IconIdI)
			{
			case 0:
				if(isConnected)
					this.Window.SetTabPicture(Resources.iconClientConnected);
				else
					this.Window.SetTabPicture(Resources.iconClientDisconnected);
				break;
			case 1:
				if(isConnected)
					this.Window.SetTabPicture(Resources.iconRDP2Connected);
				else
					this.Window.SetTabPicture(Resources.iconRDP2Disconnected);
				break;
			}
		}
	}
}