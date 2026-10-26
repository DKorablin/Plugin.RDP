using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Plugin.RDP.Bll;
using Plugin.RDP.RDP;

namespace Plugin.RDP.UI
{
	internal partial class RdpClientDlg : UserControl
	{
		/// <summary>Gateway logon methods in the order of <see cref="ddlGatewayLogonMethod"/> items</summary>
		private static readonly RdpClient.GatewayLogonMethod[] GatewayLogonMethods = new RdpClient.GatewayLogonMethod[]
		{
			RdpClient.GatewayLogonMethod.NTLM,
			RdpClient.GatewayLogonMethod.SmartCard,
			RdpClient.GatewayLogonMethod.Any,
		};

		private readonly PluginWindows _plugin;

		/// <summary>Edited tree node (Client name)</summary>
		private readonly BindingSource _bsTree = new BindingSource();

		/// <summary>Edited client settings</summary>
		private readonly BindingSource _bsClient = new BindingSource();

		/// <summary>Bindings to the client settings which can be validated</summary>
		private readonly List<Binding> _clientBindings = new List<Binding>();

		/// <summary>Additional settings for the remote desktop size modes that require it</summary>
		private readonly Dictionary<RadioButton, IDesktopSizeOptions> _desktopSizeOptions = new Dictionary<RadioButton, IDesktopSizeOptions>();

		private SettingsDataSet.TreeRow _row;

		private SettingsDataSet.RdpClientRow _clientRow;

		/// <summary>The client is created right before the editor is opened</summary>
		private Boolean _isNew;

		/// <summary>Controls are filled from the edited row</summary>
		private Boolean _loading;

		/// <summary>The user requested to close the editor</summary>
		public event EventHandler CloseRequested;

		/// <summary>The edited tree row</summary>
		internal SettingsDataSet.TreeRow Row => this._row;

		/// <summary>The client was just created and still invalid, so it can be removed when the editor is closed</summary>
		internal Boolean IsDiscardable => this._isNew && this._clientRow != null && this._clientRow.HasErrors;

		public RdpClientDlg(PluginWindows plugin)
		{
			this.InitializeComponent();

			this._plugin = plugin;
			txtDomain.DefaultText = Environment.UserDomainName;
			txtUser.DefaultText = Environment.UserName;
			udPort.DefaultValue = RdpClient.DefaultRDPPort;
			tabMain.Visible = false;

			foreach(RadioButton ctrl in flowDesktopSize.Controls)
				ctrl.CheckedChanged += this.rbDesktopSize_CheckedChanged;
			this.RegisterDesktopSizeOptions(rbSizeCustom, new DesktopSizeCustomOptions());

			base.Disposed += this.RdpClientDlg_Disposed;
		}

		private void RdpClientDlg_Disposed(Object sender, EventArgs e)
		{
			this.DetachRow();
			this._bsTree.Dispose();
			this._bsClient.Dispose();
		}

		/// <summary>Attach additional settings to the remote desktop size mode</summary>
		/// <param name="mode">Desktop size mode to which settings belongs</param>
		/// <param name="options">Settings control that will be shown when the <paramref name="mode"/> is selected</param>
		private void RegisterDesktopSizeOptions<T>(RadioButton mode, T options) where T : Control, IDesktopSizeOptions
		{
			options.Visible = false;
			options.Changed += this.DesktopSizeOptions_Changed;
			pnlDesktopSizeOptions.Controls.Add(options);
			this._desktopSizeOptions.Add(mode, options);
			this.ToggleDesktopSizeOptions();
		}

		/// <summary>Show additional settings of the selected desktop size mode and hide others</summary>
		private void ToggleDesktopSizeOptions()
		{
			Control selected = null;
			foreach(KeyValuePair<RadioButton, IDesktopSizeOptions> item in this._desktopSizeOptions)
				if(item.Key.Checked)
					selected = (Control)item.Value;

			pnlDesktopSizeOptions.SuspendLayout();
			foreach(Control ctrl in pnlDesktopSizeOptions.Controls)
				ctrl.Visible = ctrl == selected;
			pnlDesktopSizeOptions.Visible = selected != null;
			pnlDesktopSizeOptions.ResumeLayout();
		}

		/// <summary>Show the tree node in the editor</summary>
		/// <param name="row">The client or folder row to edit</param>
		/// <param name="isNew">The client is just created and can be removed if the user closes the editor while it is invalid</param>
		public void ToggleRdpClientRow(SettingsDataSet.TreeRow row, Boolean isNew = false)
		{
			_ = row ?? throw new ArgumentNullException(nameof(row));
			if(row == this._row)
				return;

			this.DetachRow();
			this._row = row;
			this._isNew = isNew;
			this._clientRow = row.ElementType == ElementType.Client
				? this._plugin.Settings.XmlSettings.GetClientRow(row.TreeID)
				: null;

			base.Text = row.Name;
			tabMain.Visible = this._clientRow != null;

			this._loading = true;
			try
			{
				this._bsTree.DataSource = row;
				if(txtName.DataBindings.Count == 0)
					this.BindTreeControls();

				if(this._clientRow != null)
				{
					ddlComputer.Items.Clear();
					ddlComputer.Items.AddRange(this._plugin.Settings.XmlSettings.GetServerList());

					this._bsClient.DataSource = this._clientRow;
					if(this._clientBindings.Count == 0)
						this.BindClientControls();

					this.LoadIcon();
					this.LoadDesktopSize();
				}
			} finally
			{
				this._loading = false;
			}

			row.PropertyChanged += this.Row_PropertyChanged;
			if(this._clientRow != null)
			{
				this._clientRow.PropertyChanged += this.Row_PropertyChanged;
				this.UpdateErrors();
			}
		}

		/// <summary>Stop listening to changes of the edited rows</summary>
		private void DetachRow()
		{
			if(this._row != null)
				this._row.PropertyChanged -= this.Row_PropertyChanged;
			if(this._clientRow != null)
				this._clientRow.PropertyChanged -= this.Row_PropertyChanged;
			this._clientRow = null;
		}

		/// <summary>Bind the client name to the tree row</summary>
		private void BindTreeControls()
		{
			txtName.Validating += this.txtName_Validating;//Subscribed before the binding to restore an empty name before it's written
			Binding name = txtName.DataBindings.Add(nameof(TextBox.Text), this._bsTree, nameof(SettingsDataSet.TreeRow.Name), true, DataSourceUpdateMode.OnValidation);
			name.Parse += Binding_ParseTrim;
		}

		/// <summary>Bind the client settings controls to the client row</summary>
		private void BindClientControls()
		{
			this.BindText(ddlComputer, nameof(SettingsDataSet.RdpClientRow.Server)).Parse += Binding_ParseTrim;
			this.BindText(txtUser, nameof(SettingsDataSet.RdpClientRow.UserName)).Parse += Binding_ParseTrim;
			this.BindText(txtPassword, nameof(SettingsDataSet.RdpClientRow.PasswordI));
			this.BindText(txtDomain, nameof(SettingsDataSet.RdpClientRow.DomainI));
			this.BindText(txtVmConsoleId, nameof(SettingsDataSet.RdpClientRow.VirtualMachineId));
			this.BindText(txtRunApplicationFilePath, nameof(SettingsDataSet.RdpClientRow.RunApplicationPathI));
			this.BindText(txtRunApplicationWorkingDir, nameof(SettingsDataSet.RdpClientRow.RunApplicationWorkingDirI));
			this.BindText(txtGatewayServer, nameof(SettingsDataSet.RdpClientRow.GatewayServer));
			this.BindText(txtGatewayUserName, nameof(SettingsDataSet.RdpClientRow.GatewayUserName));
			this.BindText(txtGatewayPassword, nameof(SettingsDataSet.RdpClientRow.GatewayPassword));
			this.BindText(txtGatewayDomain, nameof(SettingsDataSet.RdpClientRow.GatewayDomain));

			this.Bind(udPort, nameof(NumericUpDown.Value), nameof(SettingsDataSet.RdpClientRow.ConnectPortI), DataSourceUpdateMode.OnValidation);
			this.Bind(udMinutesToIdle, nameof(NumericUpDown.Value), nameof(SettingsDataSet.RdpClientRow.MinutesToIdle), DataSourceUpdateMode.OnValidation);

			this.BindChecked(cbConsole, nameof(SettingsDataSet.RdpClientRow.ConnectConsole));
			this.BindChecked(cbRunApplication, nameof(SettingsDataSet.RdpClientRow.RunApplication));
			this.BindChecked(cbRunApplicationMaximize, nameof(SettingsDataSet.RdpClientRow.RunApplicationMaximize));
			this.BindChecked(cbUseGateway, nameof(SettingsDataSet.RdpClientRow.GatewayEnabled));
			this.BindChecked(cbGatewayBypass, nameof(SettingsDataSet.RdpClientRow.GatewayBypass));
			this.BindChecked(cbGatewayShareAuth, nameof(SettingsDataSet.RdpClientRow.GatewayShareAuth));

			Binding vmConsole = this.BindChecked(cbVmConsole, nameof(SettingsDataSet.RdpClientRow.ConnectionTypeI));
			vmConsole.Format += (s, e) => e.Value = (ConnectionType)e.Value == ConnectionType.VirtualMachineConsoleConnect;
			vmConsole.Parse += (s, e) => e.Value = (Boolean)e.Value ? ConnectionType.VirtualMachineConsoleConnect : ConnectionType.Normal;

			this.BindFlag(cbRedirectDrives, nameof(SettingsDataSet.RdpClientRow.RedirectI), RedirectFlags.Drives);
			this.BindFlag(cbRedirectPorts, nameof(SettingsDataSet.RdpClientRow.RedirectI), RedirectFlags.Ports);
			this.BindFlag(cbRedirectPrinters, nameof(SettingsDataSet.RdpClientRow.RedirectI), RedirectFlags.Printers);
			this.BindFlag(cbRedirectSmartCards, nameof(SettingsDataSet.RdpClientRow.RedirectI), RedirectFlags.SmartCards);
			this.BindFlag(cbRedirectClipboard, nameof(SettingsDataSet.RdpClientRow.RedirectI), RedirectFlags.Clipboard);
			this.BindFlag(cbRedirectPoS, nameof(SettingsDataSet.RdpClientRow.RedirectI), RedirectFlags.PointOfService);
			this.BindFlag(cbRedirectAudioCapture, nameof(SettingsDataSet.RdpClientRow.RedirectI), RedirectFlags.AudioCapture);

			this.BindFlag(cbDisableWallpaper, nameof(SettingsDataSet.RdpClientRow.PerformanceFlagsI), TS_PERF.DISABLE_WALLPAPER);
			this.BindFlag(cbEnableFontSmoothing, nameof(SettingsDataSet.RdpClientRow.PerformanceFlagsI), TS_PERF.ENABLE_FONT_SMOOTHING);
			this.BindFlag(cbEnableDesktopComposition, nameof(SettingsDataSet.RdpClientRow.PerformanceFlagsI), TS_PERF.ENABLE_DESKTOP_COMPOSITION);
			this.BindFlag(cbDisableFullWindowDrag, nameof(SettingsDataSet.RdpClientRow.PerformanceFlagsI), TS_PERF.DISABLE_FULLWINDOWDRAG);
			this.BindFlag(cbDisableMenuAnimations, nameof(SettingsDataSet.RdpClientRow.PerformanceFlagsI), TS_PERF.DISABLE_MENUANIMATIONS);
			this.BindFlag(cbDisableThemeing, nameof(SettingsDataSet.RdpClientRow.PerformanceFlagsI), TS_PERF.DISABLE_THEMING);

			this.BindIndex(ddlRedirectAudio, nameof(SettingsDataSet.RdpClientRow.RedirectAudioI));
			this.BindIndex(ddlRedirectAudioQuality, nameof(SettingsDataSet.RdpClientRow.RedirectAudioQualityI));
			this.BindIndex(ddlAuthenticationLevel, nameof(SettingsDataSet.RdpClientRow.AuthenticationLevelI));
			this.BindIndex(ddlKeyboardHook, nameof(SettingsDataSet.RdpClientRow.KeyboardHookI));

			Binding colorDepth = this.Bind(ddlColorDepth, nameof(ComboBox.SelectedIndex), nameof(SettingsDataSet.RdpClientRow.ColorDepthI), DataSourceUpdateMode.OnPropertyChanged);
			colorDepth.Format += (s, e) => e.Value = ddlColorDepth.Items.IndexOf(e.Value.ToString());
			colorDepth.Parse += (s, e) => e.Value = Convert.ToInt32(ddlColorDepth.Items[(Int32)e.Value]);

			Binding gatewayLogon = this.Bind(ddlGatewayLogonMethod, nameof(ComboBox.SelectedIndex), nameof(SettingsDataSet.RdpClientRow.GatewayLogonMethodI), DataSourceUpdateMode.OnPropertyChanged);
			gatewayLogon.Format += (s, e) => e.Value = Array.IndexOf(GatewayLogonMethods, e.Value);
			gatewayLogon.Parse += (s, e) => e.Value = GatewayLogonMethods[(Int32)e.Value];
		}

		private Binding Bind(Control control, String propertyName, String dataMember, DataSourceUpdateMode updateMode)
		{
			Binding result = control.DataBindings.Add(propertyName, this._bsClient, dataMember, true, updateMode);
			this._clientBindings.Add(result);
			return result;
		}

		/// <summary>Bind text field, the value is stored when the control loses focus</summary>
		private Binding BindText(Control control, String dataMember)
			=> this.Bind(control, nameof(Control.Text), dataMember, DataSourceUpdateMode.OnValidation);

		/// <summary>Bind checkbox, the value is stored as soon as it's changed</summary>
		private Binding BindChecked(CheckBox control, String dataMember)
			=> this.Bind(control, nameof(CheckBox.Checked), dataMember, DataSourceUpdateMode.OnPropertyChanged);

		/// <summary>Bind drop down list where item index is equal to the enum value</summary>
		private void BindIndex(ComboBox control, String dataMember)
		{
			Binding binding = this.Bind(control, nameof(ComboBox.SelectedIndex), dataMember, DataSourceUpdateMode.OnPropertyChanged);
			binding.Format += (s, e) => e.Value = Convert.ToInt32(e.Value);
			binding.Parse += (s, e) => e.Value = Enum.ToObject(e.DesiredType, e.Value);
		}

		/// <summary>Bind checkbox to the single flag of the enum value</summary>
		/// <param name="control">Checkbox to bind</param>
		/// <param name="dataMember">The flags enum property of the client row</param>
		/// <param name="flag">The flag which is toggled by the checkbox</param>
		private void BindFlag(CheckBox control, String dataMember, Enum flag)
		{
			Int64 mask = Convert.ToInt64(flag);
			Binding binding = this.BindChecked(control, dataMember);
			binding.Format += (s, e) => e.Value = (Convert.ToInt64(e.Value) & mask) == mask;
			binding.Parse += (s, e) =>
			{
				Int64 value = Convert.ToInt64(this._clientRow.GetType().GetProperty(dataMember).GetValue(this._clientRow, null));
				value = (Boolean)e.Value ? value | mask : value & ~mask;
				e.Value = Enum.ToObject(e.DesiredType, value);
			};
		}

		private static void Binding_ParseTrim(Object sender, ConvertEventArgs e)
			=> e.Value = e.Value?.ToString().Trim();

		/// <summary>The edited value is really changed, validate and save it</summary>
		private void Row_PropertyChanged(Object sender, PropertyChangedEventArgs e)
		{
			if(this._loading)
				return;

			this._plugin.Settings.XmlSettings.CommitClient(this._row);
			base.Text = this._row.Name;
			if(this._clientRow != null)
				this.UpdateErrors();
		}

		/// <summary>Show validation errors of the client row near the bound controls</summary>
		private void UpdateErrors()
		{
			IDataErrorInfo errors = this._clientRow;
			foreach(Binding binding in this._clientBindings)
			{
				String message = errors[binding.BindingMemberInfo.BindingField];
				error.SetError(binding.Control, message);
				if(!String.IsNullOrEmpty(message))
					binding.Control.BackColor = SystemColors.Info;
				else if(binding.Control.BackColor == SystemColors.Info)
					binding.Control.BackColor = Color.Empty;
			}
		}

		/// <summary>Fill the desktop size controls from the client row</summary>
		private void LoadDesktopSize()
		{
			DesktopSizeParser value = this._clientRow.DesktopSizeI;

			RadioButton selected = rbSizeCustom;
			foreach(RadioButton ctrl in flowDesktopSize.Controls)
				if(value.ToString().Equals(ctrl.Tag))
				{
					selected = ctrl;
					break;
				}

			if(this._desktopSizeOptions.TryGetValue(selected, out IDesktopSizeOptions options))
				options.LoadFrom(value);
			selected.Checked = true;
			this.ToggleDesktopSizeOptions();
		}

		/// <summary>Store the selected desktop size and its options to the client row</summary>
		private void SaveDesktopSize()
		{
			if(this._loading || this._clientRow == null)
				return;

			foreach(RadioButton ctrl in flowDesktopSize.Controls)
				if(ctrl.Checked)
				{
					DesktopSizeParser result = new DesktopSizeParser(ctrl.Tag?.ToString());
					if(this._desktopSizeOptions.TryGetValue(ctrl, out IDesktopSizeOptions options))
						options.ApplyTo(result);
					this._clientRow.DesktopSizeI = result;
					return;
				}
		}

		/// <summary>Select the client icon from the client row</summary>
		private void LoadIcon()
		{
			Int32 index = 0;
			foreach(ToolStripMenuItem item in tsdbIcon.DropDownItems)
			{
				if(index == this._clientRow.IconIdI)
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

		protected override Boolean ProcessDialogKey(Keys keyData)
		{
			switch(keyData)
			{
			case Keys.Escape:
				base.Validate();//Store the value of the focused field before the editor is closed
				this.OnCloseRequested();
				return true;
			default:
				return base.ProcessDialogKey(keyData);
			}
		}

		/// <summary>Check whether the node or any of its children is edited by this editor</summary>
		/// <param name="node">Tree node to check</param>
		/// <returns>The node or any of its children is the edited row</returns>
		internal Boolean IsEditing(RdpClientTreeNode node)
		{
			if(node.Tag != null && node.Tag == this._row)
				return true;

			foreach(RdpClientTreeNode child in node.Nodes)
				if(this.IsEditing(child))
					return true;
			return false;
		}

		private void OnCloseRequested()
			=> this.CloseRequested?.Invoke(this, EventArgs.Empty);

		private void txtName_Validating(Object sender, CancelEventArgs e)
		{
			if(String.IsNullOrWhiteSpace(txtName.Text) && this._row != null)
				txtName.Text = this._row.Name;//Node name is required
		}

		private void cbRunApplication_CheckedChanged(Object sender, EventArgs e)
		{
			gbRunApplication.Enabled = cbRunApplication.Checked;
		}
		private void cbUseGateway_CheckedChanged(Object sender, EventArgs e)
		{
			gbGateway.Enabled = cbUseGateway.Checked;
		}
		private void rbDesktopSize_CheckedChanged(Object sender, EventArgs e)
		{
			if(((RadioButton)sender).Checked)
			{
				this.ToggleDesktopSizeOptions();
				this.SaveDesktopSize();
			}
		}

		private void DesktopSizeOptions_Changed(Object sender, EventArgs e)
			=> this.SaveDesktopSize();

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

			if(!this._loading && this._clientRow != null)
				this._clientRow.PerformanceFlagsI = flags;//Single change, bound checkboxes are refreshed from the row
		}

		private void cbVmConsole_CheckedChanged(Object sender, EventArgs e)
		{
			txtVmConsoleId.Visible = cbVmConsole.Checked;
			udPort.DefaultValue = cbVmConsole.Checked ? RdpClient.DefaultVMConsoleConnectPort : RdpClient.DefaultRDPPort;
		}

		private void tsdbIcon_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(this._clientRow == null)
				return;

			base.Validate();//ToolStrip doesn't take focus, so store the focused field first
			this._clientRow.IconIdI = (Int16)tsdbIcon.DropDownItems.IndexOf(e.ClickedItem);
			this.LoadIcon();
		}
	}
}
