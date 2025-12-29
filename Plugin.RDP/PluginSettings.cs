using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Plugin.RDP.Bll;

namespace Plugin.RDP
{
	public class PluginSettings : INotifyPropertyChanged
	{
		private readonly PluginWindows _plugin;
		private SettingsBll _xmlSettings;

		private Boolean _isThumbnail = false;
		private Boolean _useMultipleMonitors = false;
		private Boolean _closeWindowAfterDisconnect = true;
		private String _user;
		private String _password;
		private String _domain;

		private Keys _altTab = Keys.Alt | Keys.PageUp;
		private Keys _altShiftTab = Keys.Alt | Keys.PageDown;
		private Keys _altEsc = Keys.Alt | Keys.Insert;
		private Keys _altSpace = Keys.Alt | Keys.Delete;
		private Keys _ctrlEsc = Keys.Alt | Keys.Home;

		private Keys _ctrlAltDelete = Keys.Control | Keys.Alt | Keys.End;
		private Keys _fullScreen = Keys.Control | Keys.Alt | Keys.Pause;
		private Keys _previousSession = Keys.Control | Keys.Alt | Keys.Left;
		private Keys _selectSession = Keys.Control | Keys.Alt | Keys.Right;

		/// <summary>RDP client settings</summary>
		internal SettingsBll XmlSettings
			=> this._xmlSettings ?? (this._xmlSettings = new SettingsBll(this._plugin));

		//TODO: So far it's just a stub
		internal Boolean IsThumbnail
		{
			get => this._isThumbnail;
			set => this.SetField(ref this._isThumbnail, value, nameof(this.IsThumbnail));
		}

		[Category("Appearance")]
		[DisplayName("Use multiple monitors")]
		[Description("Open Remote Desktop Connection on all monitors")]
		[DefaultValue(false)]
		public Boolean UseMultipleMonitors
		{
			get => this._useMultipleMonitors;
			set => this.SetField(ref this._useMultipleMonitors, value, nameof(this.UseMultipleMonitors));
		}

		[Category("Appearance")]
		[DisplayName("Close after disconnect")]
		[Description("Close the connection window after disconnecting from the remote server")]
		[DefaultValue(true)]
		public Boolean CloseWindowAfterDisconnect
		{
			get => this._closeWindowAfterDisconnect;
			set => this.SetField(ref this._closeWindowAfterDisconnect, value, nameof(this.CloseWindowAfterDisconnect));
		}

		#region Credentials
		[Category("Credentials")]
		[Description("The user name logon credential")]
		public String User
		{
			get => this._user;
			set => this.SetField(ref this._user, value, nameof(this.User));
		}

		[Category("Credentials")]
		[Description("The Remote Desktop ActiveX control password, in plaintext format")]
		[PasswordPropertyText(true)]
		public String Password
		{
			get => this._password;
			set => this.SetField(ref this._password, value, nameof(this.Password));
		}

		[Category("Credentials")]
		[Description("The domain to which the current user logs on")]
		public String Domain
		{
			get => this._domain;
			set => this.SetField(ref this._domain, value, nameof(this.Domain));
		}
		#endregion Credentials

		#region ALT hot keys
		[Category("ALT hot keys")]
		[DisplayName("ALT+TAB")]
		[Editor(typeof(ShortcutKeysEditor),typeof(UITypeEditor))]
		[DefaultValue(Keys.Alt | Keys.PageUp)]
		public Keys AltTab
		{
			get => this._altTab;
			set => this.SetField(ref this._altTab, value, nameof(this.AltTab));
		}

		[Category("ALT hot keys")]
		[DisplayName("ALT+SHIFT+TAB")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Alt | Keys.PageDown)]
		public Keys AltShiftTab
		{
			get => this._altShiftTab;
			set => this.SetField(ref this._altShiftTab, value, nameof(this.AltShiftTab));
		}

		[Category("ALT hot keys")]
		[DisplayName("ALT+ESC")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Alt | Keys.Insert)]
		public Keys AltEsc
		{
			get => this._altEsc;
			set => this.SetField(ref this._altEsc, value, nameof(this.AltEsc));
		}

		[Category("ALT hot keys")]
		[DisplayName("ALT+SPACE")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Alt | Keys.Delete)]
		public Keys AltSpace
		{
			get => this._altSpace;
			set => this.SetField(ref this._altSpace, value, nameof(this.AltSpace));
		}

		[Category("ALT hot keys")]
		[DisplayName("CTRL+ESC")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Alt | Keys.Home)]
		public Keys CtrlEsc
		{
			get => this._ctrlEsc;
			set => this.SetField(ref this._ctrlEsc, value, nameof(this.CtrlEsc));
		}
		#endregion ALT hot keys

		#region CTRL+ALT hot keys
		[Category("CTRL+ALT hot keys")]
		[DisplayName("CTRL+ALT+DEL")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Control | Keys.Alt | Keys.End)]
		public Keys CtrlAltDel
		{
			get => this._ctrlAltDelete;
			set => this.SetField(ref this._ctrlAltDelete, value, nameof(this.CtrlAltDel));
		}

		[Category("CTRL+ALT hot keys")]
		[DisplayName("Full screen")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Control | Keys.Alt | Keys.Pause)]
		public Keys FullScreen
		{
			get => this._fullScreen;
			set => this.SetField(ref this._fullScreen, value, nameof(this.FullScreen));
		}

		[Category("CTRL+ALT hot keys")]
		[DisplayName("Previous session")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Control|Keys.Alt|Keys.Left)]
		public Keys PreviousSession
		{
			get => this._previousSession;
			set => this.SetField(ref this._previousSession, value, nameof(this.PreviousSession));
		}

		[Category("CTRL+ALT hot keys")]
		[DisplayName("Select session")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Keys.Control|Keys.Alt|Keys.Right)]
		public Keys SelectSession
		{
			get => this._selectSession;
			set => this.SetField(ref this._selectSession, value, nameof(this.SelectSession));
		}
		#endregion CTRL+ALT hot keys

		internal PluginSettings(PluginWindows plugin)
			=> this._plugin = plugin;

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private Boolean SetField<T>(ref T field, T value, String propertyName)
		{
			if(EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
		#endregion INotifyPropertyChanged
	}
}