using System;
using System.ComponentModel;
using System.Windows.Forms;
using Plugin.RDP.RDP;
using System.Diagnostics;

namespace Plugin.RDP.UI
{
	internal partial class RemoteSessionsDlg : Form,IDisposable
	{
		private const String Caption = "Remote Sessions";
		private readonly RemoteSessions _sessions;
		private readonly PluginWindows _plugin;
		private Boolean IsThreadsBusy => bgQuerySessions.IsBusy || bgDisconnectSession.IsBusy || bgLogoffSession.IsBusy;

		public RemoteSessionsDlg(PluginWindows plugin, String server)
		{
			InitializeComponent();
			this._sessions = new RemoteSessions(server);
			this._plugin = plugin;
			this.QuerySessions();
		}

		/// <summary>Log out the current user from the remote session</summary>
		/// <param name="serverName">Server name</param>
		/// <param name="userName">Connected client login</param>
		/// <returns>Client logout result</returns>
		internal static Boolean LogOffUser(String serverName, String userName)
		{
			RemoteSessions sessions = new RemoteSessions(serverName);
			try
			{
				sessions.OpenServer();
				Int32 sessionId = -1;
				foreach(RemoteSessions.RemoteSessionInfo info in sessions.QuerySessions())
					switch(info.State)
					{
					case Native.ConnectStateClass.Active:
					case Native.ConnectStateClass.Connected:
						if(info.UserName == userName)
						{
							if(sessionId != -1)
							{
								MessageBox.Show("Multiple instances activated. Can't determine which session to LogOff", RemoteSessionsDlg.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							sessionId = info.SessionId;
						}
						break;
					}
				if(sessionId>-1)
					sessions.LogOffSession(sessionId);
				return true;
			} catch(Win32Exception exc)
			{
				MessageBox.Show(exc.Message, RemoteSessionsDlg.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			finally
			{
				if(sessions.IsConnected)
					sessions.CloseServer();
			}
		}

		private void QuerySessions()
		{
			if(!this.IsThreadsBusy)
			{
				base.Text = String.Join(" - ", new String[] { RemoteSessionsDlg.Caption, "Querying...", });
				bgQuerySessions.RunWorkerAsync();
			}
		}

		protected override Boolean ProcessDialogKey(Keys keyData)
		{
			switch(keyData)
			{
			case Keys.Escape:
				base.Close();
				return true;
			default:
				return base.ProcessDialogKey(keyData);
			}
		}

		void IDisposable.Dispose()
		{
			if(this._sessions.IsConnected)
				this._sessions.CloseServer();
			base.Dispose();
		}

		private void lvSessions_KeyDown(Object sender, KeyEventArgs e)
		{
			switch(e.KeyData)
			{
				case Keys.F5:
					e.Handled = true;
					this.QuerySessions();
					break;
			}
		}

		private void cmsSession_Opening(Object sender, CancelEventArgs e)
		{
			e.Cancel = this.IsThreadsBusy;

			if(!e.Cancel)
			{
				tsmiLogoff.Visible = lvSessions.SelectedItems.Count > 0;
				tsmiDisconnect.Visible = lvSessions.SelectedItems.Count > 0;
				if(lvSessions.SelectedItems.Count > 0)
				{
					Boolean enableDisconnect = false;
					foreach(ListViewItem item in lvSessions.SelectedItems)
					{
						RemoteSessions.RemoteSessionInfo info = (RemoteSessions.RemoteSessionInfo)item.Tag;
						if(info.State != Native.ConnectStateClass.Disconnected)
						{
							enableDisconnect = true;
							break;
						}
					}
					tsmiDisconnect.Enabled = enableDisconnect;
				}
			}
		}

		private void cmsSession_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			cmsSession.Visible = false;
			if(e.ClickedItem == tsmiRefresh)
				this.QuerySessions();
			else if(e.ClickedItem == tsmiLogoff)
			{
				RemoteSessions.RemoteSessionInfo[] sessions = new RemoteSessions.RemoteSessionInfo[lvSessions.SelectedItems.Count];
				for(Int32 loop = 0; loop < sessions.Length; loop++)
					sessions[loop] = (RemoteSessions.RemoteSessionInfo)lvSessions.SelectedItems[loop].Tag;
				bgLogoffSession.RunWorkerAsync(sessions);
			} else if(e.ClickedItem == tsmiDisconnect)
			{
				RemoteSessions.RemoteSessionInfo[] sessions = new RemoteSessions.RemoteSessionInfo[lvSessions.SelectedItems.Count];
				for(Int32 loop = 0; loop < sessions.Length; loop++)
					sessions[loop] = (RemoteSessions.RemoteSessionInfo)lvSessions.SelectedItems[loop].Tag;
				bgDisconnectSession.RunWorkerAsync(sessions);
			}
		}

		private void bgQuerySessions_DoWork(Object sender, DoWorkEventArgs e)
		{
			if(!this._sessions.IsConnected)
				this._sessions.OpenServer();

			e.Result = this._sessions.QuerySessions();
		}

		private void bgQuerySessions_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			if(!base.IsDisposed)
			{
				if(e.Error != null)
				{
					tsslStatus.Text = e.Error.Message;
					base.Text = $"{RemoteSessionsDlg.Caption} - Error";
					this._plugin.Trace.TraceData(TraceEventType.Error, 10, e.Error);
				} else
				{
					RemoteSessions.RemoteSessionInfo[] list = (RemoteSessions.RemoteSessionInfo[])e.Result;
					lvSessions.CreateColumns(list);
					lvSessions.FillList(list, 0);
					lvSessions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
					lvSessions.EndUpdate();
					tsslStatus.Text = "Ready";
					base.Text = String.Format("{0:n0} - {0:n0} session{1}", lvSessions.Items.Count, lvSessions.Items.Count != 0 ? "s" : String.Empty);
				}
			}
		}
		private void bgLogoffSession_DoWork(Object sender, DoWorkEventArgs e)
		{
			RemoteSessions.RemoteSessionInfo[] sessions = (RemoteSessions.RemoteSessionInfo[])e.Argument;

			foreach(RemoteSessions.RemoteSessionInfo session in sessions)
			{
				tsslStatus.Text = "Logging off " + session.UserName;
				this._sessions.LogOffSession(session.SessionId);
			}
		}

		private void bgLogoffSession_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			if(!base.IsDisposed)
			{
				if(e.Error != null)
				{
					tsslStatus.Text = e.Error.Message;
					this._plugin.Trace.TraceData(TraceEventType.Error, 10, e.Error);
				} else
				{
					tsslStatus.Text = "Ready";
					this.QuerySessions();
				}
			}
		}

		private void bgDisconnectSession_DoWork(Object sender, DoWorkEventArgs e)
		{
			RemoteSessions.RemoteSessionInfo[] sessions = (RemoteSessions.RemoteSessionInfo[])e.Argument;

			foreach(RemoteSessions.RemoteSessionInfo session in sessions)
				if(session.State != Native.ConnectStateClass.Disconnected)
				{
					tsslStatus.Text = "Disconnecting " + session.UserName;
					this._sessions.DisconnectSession(session.SessionId);
				}
		}

		private void bgDisconnectSession_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			if(!base.IsDisposed)
			{
				if(e.Error != null)
				{
					tsslStatus.Text = e.Error.Message;
					this._plugin.Trace.TraceData(TraceEventType.Error, 10, e.Error);
				} else
				{
					tsslStatus.Text = "Ready";
					this.QuerySessions();
				}
			}
		}
	}
}