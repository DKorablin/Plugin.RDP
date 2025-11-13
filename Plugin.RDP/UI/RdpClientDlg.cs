using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Plugin.RDP.Bll;
using Plugin.RDP.RDP;
using System.Collections.Generic;

namespace Plugin.RDP.UI
{
	internal partial class RdpClientDlg : Form
	{
		private static readonly Type ClientRowType = typeof(SettingsDataSet.RdpClientRow);

		private readonly PluginWindows _plugin;

		private SettingsDataSet.TreeRow _row;

		private SettingsDataSet.TreeRow _parentRow;

		private SettingsDataSet.RdpClientRow ClientRow
			=> this._row == null
				? null
				: this._plugin.Settings.XmlSettings.GetClientRow(this._row.TreeID);

		#region Configuration
		public String ClientName
		{
			get => txtName.Text.Trim();
			set => txtName.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public String Server
		{
			get => ddlComputer.Text.Trim();
			set => ddlComputer.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public String UserName
		{
			get => txtUser.Text.Trim();
			set => txtUser.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public String PasswordI
		{
			get => txtPassword.Text;
			set => txtPassword.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public String DomainI
		{
			get => txtDomain.Text;
			set => txtDomain.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public Int16 IconIdI
		{
			get
			{
				Int16 iconIndex = 0;
				foreach(ToolStripMenuItem item in tsdbIcon.DropDownItems)
					if(item.Checked)
						break;
					else
						iconIndex++;

				return iconIndex;
			}
			set
			{
				Int32 index = 0;
				foreach(ToolStripMenuItem item in tsdbIcon.DropDownItems)
				{
					if(index == value)
					{
						tsdbIcon.Text = item.Text;
						tsdbIcon.Image = item.Image;
						item.Checked = true;
						item.Font = new Font(Control.DefaultFont, FontStyle.Bold);
					} else
					{
						item.Checked = false;
						item.Font = Control.DefaultFont;
					}
					index++;
				}
			}
		}

		[Bindable(BindableSupport.Default)]
		public Int32? ConnectPortI
		{
			get => udPort.Value == RdpClient.DefaultRDPPort ? (Int32?)null : (Int32)udPort.Value;
			set
			{
				udPort.Value = value.GetValueOrDefault(RdpClient.DefaultRDPPort);
				this.udPort_ValueChanged(udPort, EventArgs.Empty);
			}
		}

		[Bindable(BindableSupport.Default)]
		public Boolean ConnectConsole
		{
			get => cbConsole.Checked;
			set => cbConsole.Checked = value;
		}

		[Bindable(BindableSupport.Default)]
		public RedirectFlags RedirectI
		{
			get
			{
				RedirectFlags result = RedirectFlags.None;
				if(cbRedirectDrives.Checked)
					result |= RedirectFlags.Drives;
				if(cbRedirectPorts.Checked)
					result |= RedirectFlags.Ports;
				if(cbRedirectPrinters.Checked)
					result |= RedirectFlags.Printers;
				if(cbRedirectSmartCards.Checked)
					result |= RedirectFlags.SmartCards;
				if(cbRedirectClipboard.Checked)
					result |= RedirectFlags.Clipboard;
				if(cbRedirectPoS.Checked)
					result |= RedirectFlags.PointOfService;
				return result;
			}
			set
			{
				cbRedirectDrives.Checked = (value & RedirectFlags.Drives) == RedirectFlags.Drives;
				cbRedirectPorts.Checked = (value & RedirectFlags.Ports) == RedirectFlags.Ports;
				cbRedirectPrinters.Checked = (value & RedirectFlags.Printers) == RedirectFlags.Printers;
				cbRedirectSmartCards.Checked = (value & RedirectFlags.SmartCards) == RedirectFlags.SmartCards;
				cbRedirectClipboard.Checked = (value & RedirectFlags.Clipboard) == RedirectFlags.Clipboard;
				cbRedirectPoS.Checked = (value & RedirectFlags.PointOfService) == RedirectFlags.PointOfService;
			}
		}

		[Bindable(BindableSupport.Default)]
		public RdpClient.AudioRedirectionMode RedirectAudioI
		{
			get => (RdpClient.AudioRedirectionMode)ddlRedirectAudio.SelectedIndex;
			set => ddlRedirectAudio.SelectedIndex = (Int32)value;
		}

		[Bindable(BindableSupport.Default)]
		public Boolean RedirectAudioCapture
		{
			get => cbRedirectAudioCapture.Checked;
			set => cbRedirectAudioCapture.Checked = value;
		}

		[Bindable(BindableSupport.Default)]
		public RdpClient.AudioRedirectionQuality RedirectAudioQualityI
		{
			get => (RdpClient.AudioRedirectionQuality)ddlRedirectAudioQuality.SelectedIndex;
			set => ddlRedirectAudioQuality.SelectedIndex = (Int32)value;
		}

		[Bindable(BindableSupport.Default)]
		public Boolean RunApplication
		{
			get => cbRunApplication.Checked;
			set
			{
				cbRunApplication.Checked = value;
				this.cbRunApplication_CheckedChanged(cbRunApplication, EventArgs.Empty);
			}
		}

		[Bindable(BindableSupport.Default)]
		public String RunApplicationPathI
		{
			get => txtRunApplicationFilePath.Text;
			set => txtRunApplicationFilePath.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public String RunApplicationWorkingDirI
		{
			get => txtRunApplicationWorkingDir.Text;
			set => txtRunApplicationWorkingDir.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public Boolean RunApplicationMaximize
		{
			get => cbRunApplicationMaximize.Checked;
			set => cbRunApplicationMaximize.Checked = value;
		}

		[Bindable(BindableSupport.Default)]
		public AuthenticationLevel AuthenticationLevelI
		{
			get => (AuthenticationLevel)ddlAuthenticationLevel.SelectedIndex;
			set => ddlAuthenticationLevel.SelectedIndex = (Int32)value;
		}

		[Bindable(BindableSupport.Default)]
		public Int32 MinutesToIdle
		{
			get => (Int32)udMinutesToIdle.Value;
			set => udMinutesToIdle.Value = value;
		}

		[Bindable(BindableSupport.Default)]
		public RdpClient.KeyboardHookMode KeyboardHookI
		{
			get => (RdpClient.KeyboardHookMode)ddlKeyboardHook.SelectedIndex;
			set => ddlKeyboardHook.SelectedIndex = (Int32)value;
		}

		[Bindable(BindableSupport.Default)]
		public Int32 ColorDepthI
		{
			get
			{
				Int32 result = Convert.ToInt32(ddlColorDepth.SelectedItem);
				switch(result)
				{
					case 8:
					case 15:
					case 16:
					case 24:
					case 32:
						return result;
					default:
						throw new ArgumentOutOfRangeException("colorDepth", ddlColorDepth.SelectedItem.ToString(), "Valid color depth is: 8,15,16,24,32");
				}
			}
			set
			{
				switch(value)
				{
					case 8:
					case 15:
					case 16:
					case 24:
					case 32:
						ddlColorDepth.SelectedItem = value.ToString();
						break;
					default:
						throw new ArgumentOutOfRangeException("colorDepth", value.ToString(), "Valid color depth is: 8,15,16,24,32");
				}
			}
		}

		[Bindable(BindableSupport.Default)]
		public DesktopSizeParser DesktopSizeI
		{
			get
			{
				foreach(Control ctrl in flowDesktopSize.Controls)
					if(((RadioButton)ctrl).Checked)
						return new DesktopSizeParser(ctrl.Tag.ToString());
				throw new NotImplementedException();
			}
			set
			{
				if(value == null)
					value = new DesktopSizeParser();

				Boolean isCustom = true;
				foreach(Control ctrl in flowDesktopSize.Controls)
					if(value.ToString().Equals(ctrl.Tag))
					{
						((RadioButton)ctrl).Checked = true;
						isCustom = false;
					}
				if(isCustom)
				{
					rbSizeCustom.Checked = true;
					rbSizeCustom.Tag = value.ToString();
					rbSizeCustom.Text = $"Custom: {value}";
				}
			}
		}

		[Bindable(BindableSupport.Default)]
		public Boolean GatewayEnabled
		{
			get => cbUseGateway.Checked;
			set
			{
				cbUseGateway.Checked = value;
				this.cbUseGateway_CheckedChanged(cbUseGateway, EventArgs.Empty);
			}
		}

		[Bindable(BindableSupport.Default)]
		public String GatewayServer
		{
			get => txtGatewayServer.Text;
			set => txtGatewayServer.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public Boolean GatewayBypass
		{
			get => cbGatewayBypass.Checked;
			set => cbGatewayBypass.Checked = value;
		}

		[Bindable(BindableSupport.Default)]
		public Boolean GatewayShareAuth
		{
			get => cbGatewayShareAuth.Checked;
			set => cbGatewayShareAuth.Checked = value;
		}

		[Bindable(BindableSupport.Default)]
		public String GatewayUserName
		{
			get => txtGatewayUserName.Text;
			set => txtGatewayUserName.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public String GatewayPassword
		{
			get => txtGatewayPassword.Text;
			set => txtGatewayPassword.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public String GatewayDomain
		{
			get => txtGatewayDomain.Text;
			set => txtGatewayDomain.Text = value;
		}

		[Bindable(BindableSupport.Default)]
		public RdpClient.GatewayLogonMethod GatewayLogonMethodI
		{
			get
			{
				switch(ddlGatewayLogonMethod.SelectedIndex)
				{
					case 0:
						return RdpClient.GatewayLogonMethod.NTLM;
					case 1:
						return RdpClient.GatewayLogonMethod.SmartCard;
					case 2:
						return RdpClient.GatewayLogonMethod.Any;
					default:
						throw new NotImplementedException();
				}
			}
			set
			{
				switch(value)
				{
					case RdpClient.GatewayLogonMethod.NTLM:
						ddlGatewayLogonMethod.SelectedIndex = 0;
						break;
					case RdpClient.GatewayLogonMethod.SmartCard:
						ddlGatewayLogonMethod.SelectedIndex = 1;
						break;
					case RdpClient.GatewayLogonMethod.Any:
						ddlGatewayLogonMethod.SelectedIndex = 2;
						break;
					default:
						throw new NotImplementedException();
				}
			}
		}

		public TS_PERF PerformanceFlagsI
		{
			get
			{
				TS_PERF result = TS_PERF.DISABLE_NOTHING;
				if(cbDisableWallpaper.Checked)
					result |= TS_PERF.DISABLE_WALLPAPER;
				if(cbEnableFontSmoothing.Checked)
					result |= TS_PERF.ENABLE_FONT_SMOOTHING;
				if(cbEnableDesktopComposition.Checked)
					result |= TS_PERF.ENABLE_DESKTOP_COMPOSITION;
				if(cbDisableFullWindowDrag.Checked)
					result |= TS_PERF.DISABLE_FULLWINDOWDRAG;
				if(cbDisableMenuAnimations.Checked)
					result |= TS_PERF.DISABLE_MENUANIMATIONS;
				if(cbDisableThemeing.Checked)
					result |= TS_PERF.DISABLE_THEMING;
				return result;
			}
			set
			{
				cbDisableWallpaper.Checked = (value & TS_PERF.DISABLE_WALLPAPER) == TS_PERF.DISABLE_WALLPAPER;
				cbEnableFontSmoothing.Checked = (value & TS_PERF.ENABLE_FONT_SMOOTHING) == TS_PERF.ENABLE_FONT_SMOOTHING;
				cbEnableDesktopComposition.Checked = (value & TS_PERF.ENABLE_DESKTOP_COMPOSITION) == TS_PERF.ENABLE_DESKTOP_COMPOSITION;
				cbDisableFullWindowDrag.Checked = (value & TS_PERF.DISABLE_FULLWINDOWDRAG) == TS_PERF.DISABLE_FULLWINDOWDRAG;
				cbDisableMenuAnimations.Checked = (value & TS_PERF.DISABLE_MENUANIMATIONS) == TS_PERF.DISABLE_MENUANIMATIONS;
				cbDisableThemeing.Checked = (value & TS_PERF.DISABLE_THEMING) == TS_PERF.DISABLE_THEMING;
			}
		}

		[Bindable(BindableSupport.Default)]
		public ConnectionType ConnectionTypeI
		{
			get => cbVmConsole.Checked
					? ConnectionType.VirtualMachineConsoleConnect
					: ConnectionType.Normal;
			set => cbVmConsole.Checked = value == ConnectionType.VirtualMachineConsoleConnect;
		}

		[Bindable(BindableSupport.Default)]
		public String VirtualMachineId
		{
			get => txtVmConsoleId.Text;
			set => txtVmConsoleId.Text = value;
		}
		#endregion Configuration

		public RdpClientDlg(PluginWindows plugin)
		{
			this.InitializeComponent();
			
			this._plugin = plugin;
			txtDomain.DefaultText = Environment.UserDomainName;
			txtUser.DefaultText = Environment.UserName;
			udPort.DefaultValue = RdpClient.DefaultRDPPort;
			tabMain.Visible = tsbnSave.Enabled = false;
		}

		public void ToggleNewClientRow(SettingsDataSet.TreeRow parentRow)
		{
			this.RefreshControls(null, parentRow);//Set default values ​​and show/hide controls

			this.SetControlValues(null);

			txtDomain.Text = Environment.UserDomainName;
			txtUser.Text = Environment.UserName;
			udPort.Value = RdpClient.DefaultRDPPort;
		}

		public void ToggleRdpClientRow(SettingsDataSet.TreeRow row)
		{
			this.RefreshControls(row, null);//Set default values ​​and show/hide controls

			this.ClientName = this._row.Name;
			this.SetControlValues(this.ClientRow);
		}

		private void SetControlValues(SettingsDataSet.RdpClientRow instance)
		{
			foreach(PropertyInfo property in this.GetType().GetProperties(BindingFlags.Instance|BindingFlags.DeclaredOnly|BindingFlags.Public))
				if(property.IsDefined(typeof(BindableAttribute), false))
				{
					PropertyInfo rowProperty = ClientRowType.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
					Object value;
					if(instance == null)
					{
						Object[] attrs = rowProperty.GetCustomAttributes(typeof(DefaultValueAttribute), false);
						if(attrs.Length == 1)
							value = ((DefaultValueAttribute)attrs[0]).Value;
						else
							value = rowProperty.PropertyType.IsValueType
								? Activator.CreateInstance(rowProperty.PropertyType)
								: null;
					} else
						value = rowProperty.GetValue(instance, null);

					property.SetValue(this, value, null);
				}
		}

		/// <summary>Update controls, hide or show desired elements</summary>
		/// <param name="row">Row to edit</param>
		/// <param name="parentRow">Parent row for which to create a client</param>
		private void RefreshControls(SettingsDataSet.TreeRow row, SettingsDataSet.TreeRow parentRow)
		{
			if(row != null && parentRow != null)
				throw new InvalidOperationException();
			else if(parentRow != null && parentRow.ElementType == ElementType.Client)
				throw new InvalidOperationException($"Client Id: {parentRow.TreeID}. Client row can't be parent node for another client");

			tabMain.Visible = tsbnSave.Enabled = row == null || row.ElementType == ElementType.Client;

			this._row = row;
			this._parentRow = parentRow;

			List<String> titles = new List<String>(2);
			if(parentRow != null)
				titles.Add(parentRow.Name);
			if(row != null)
				titles.Add(row.Name);

			base.Text = String.Join(" - ", titles.ToArray());

			ddlComputer.Items.Clear();
			ddlComputer.Items.AddRange(this._plugin.Settings.XmlSettings.GetServerList());

			this.ClientName = null;
		}

		protected override Boolean ProcessDialogKey(Keys keyData)
		{
			switch(keyData)
			{
			case Keys.Control | Keys.S:
				base.AcceptButton.PerformClick();
				return true;
			case Keys.Escape:
				base.Hide();
				return true;
			default:
				return base.ProcessDialogKey(keyData);
			}
		}

		protected override void OnValidating(CancelEventArgs e)
		{
			Boolean cancel = false;
			if(String.IsNullOrEmpty(this.ClientName))
			{
				error.SetError(txtName, "You need to specify client name");
				txtName.BackColor = SystemColors.Info;
				cancel = true;
			} else
			{
				error.SetError(txtName, String.Empty);
				txtName.BackColor = Color.Empty;
			}

			if(String.IsNullOrEmpty(this.Server))
			{
				error.SetError(ddlComputer, "You need to specify remote computer to connect");
				ddlComputer.BackColor = SystemColors.Info;
				cancel = true;
			} else
			{
				error.SetError(ddlComputer, String.Empty);
				ddlComputer.BackColor = Color.Empty;
			}

			if(String.IsNullOrEmpty(this.UserName))
			{
				error.SetError(txtUser, "You need to specify login");
				txtUser.BackColor = SystemColors.Info;
				cancel = true;
			} else
			{
				error.SetError(txtUser, String.Empty);
				txtUser.BackColor = Color.Empty;
			}

			if(cancel)
				tabMain.SelectedIndex = 0;
			e.Cancel = cancel;

			base.OnValidating(e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = true;
			base.Hide();
			base.OnClosing(e);
		}

		private void tsbnSave_Click(Object sender, EventArgs e)
		{
			if(base.Validate())
			{
				SettingsDataSet.TreeRow clientRow;
				if(this._row != null)
				{
					if(this._row.Name != this.ClientName)
						this._plugin.Settings.XmlSettings.ModifyTreeNodeName(this._row, this.ClientName);

					clientRow = this._row;
				} else
				{
					Int32? parentTreeId = this._parentRow == null ? (Int32?)null : this._parentRow.TreeID;
					clientRow = this._plugin.Settings.XmlSettings.ModifyTreeNode(null, parentTreeId, ElementType.Client, this.ClientName);
				}

				this._plugin.Settings.XmlSettings.ModifyClient(clientRow, this);
				this._plugin.Settings.XmlSettings.Save();

				this._row = null;
				this._parentRow = null;
				base.Hide();
			}
		}

		private void bnCancel_Click(Object sender, EventArgs e)
		{
			this._row = null;
			this._parentRow = null;
			base.Hide();
		}

		private void cbRunApplication_CheckedChanged(Object sender, EventArgs e)
		{
			gbRunApplication.Enabled = cbRunApplication.Checked;
		}
		private void cbUseGateway_CheckedChanged(Object sender, EventArgs e)
		{
			gbGateway.Enabled = cbUseGateway.Checked;
		}
		private void rbSizeCustom_CheckedChanged(Object sender, EventArgs e)
		{
			if(rbSizeCustom.Checked)
				using(DesktopSizeDlg dlg = new DesktopSizeDlg(rbSizeCustom.Tag))
					if(dlg.ShowDialog() == DialogResult.OK)
					{
						rbSizeCustom.Tag = dlg.CustomSize;
						rbSizeCustom.Text = $"Custom: {dlg.CustomSize}";
					} else if(rbSizeCustom.Tag == null)
					{
						this.DesktopSizeI = new DesktopSizeParser() { SameAsClient = true, };
					}
		}

		private void udPort_ValueChanged(Object sender, EventArgs e)
			=> udPort.Font = udPort.Value == 3389 ? new Font(Control.DefaultFont, FontStyle.Regular) : new Font(Control.DefaultFont, FontStyle.Bold);

		private void ddlConnectionSpeed_SelectedIndexChanged(Object sender, EventArgs e)
		{
			TS_PERF flags = TS_PERF.DISABLE_NOTHING;
			switch(ddlConnectionSpeed.SelectedIndex)
			{
				case 0:
					break;
				case 1:
					flags |= TS_PERF.DISABLE_THEMING;
					break;
				case 2:
					flags |= TS_PERF.DISABLE_THEMING;
					flags |= TS_PERF.DISABLE_MENUANIMATIONS;
					flags |= TS_PERF.DISABLE_FULLWINDOWDRAG;
					flags |= TS_PERF.ENABLE_DESKTOP_COMPOSITION;
					break;
				case 3:
					flags |= TS_PERF.DISABLE_THEMING;
					flags |= TS_PERF.DISABLE_MENUANIMATIONS;
					flags |= TS_PERF.DISABLE_FULLWINDOWDRAG;
					flags |= TS_PERF.ENABLE_DESKTOP_COMPOSITION;
					flags |= TS_PERF.ENABLE_FONT_SMOOTHING;
					flags |= TS_PERF.DISABLE_WALLPAPER;
					break;
				case 4://Custom
					flags |= TS_PERF.DISABLE_WALLPAPER;
					flags |= TS_PERF.DISABLE_FULLWINDOWDRAG;
					break;
			}
			this.PerformanceFlagsI = flags;
		}

		private void cbVmConsole_CheckedChanged(Object sender, EventArgs e)
			=> txtVmConsoleId.Visible = cbVmConsole.Checked;

		private void tsdbIcon_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			Int16 index = 0;
			foreach(ToolStripMenuItem item in tsdbIcon.DropDownItems)
				if(item == e.ClickedItem)
				{
					this.IconIdI = index;
					break;
				} else
					index++;
		}
	}
}