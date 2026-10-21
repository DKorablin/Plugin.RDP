using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Plugin.RDP.Bll;
using Plugin.RDP.RDP;

namespace Plugin.RDP
{
	public class PluginSettings : INotifyPropertyChanged
	{
		private static class Defaults
		{
			public const Boolean IsThumbnail = false;
			public const Boolean UseMultipleMonitors = false;
			public const Boolean CloseWindowAfterDisconnect = true;
			public const RdpClient.SessionResizeMode ResizeMode = RdpClient.SessionResizeMode.Auto;
			public const Keys AltTab = Keys.Alt | Keys.PageUp;
			public const Keys AltShiftTab = Keys.Alt | Keys.PageDown;
			public const Keys AltEsc = Keys.Alt | Keys.Insert;
			public const Keys AltSpace = Keys.Alt | Keys.Delete;

			public const Keys CtrlEsc = Keys.Alt | Keys.Home;
			public const Keys CtrlAltDelete = Keys.Control | Keys.Alt | Keys.End;

			public const Keys FullScreen = Keys.Control | Keys.Alt | Keys.Pause;
			public const Keys PreviousSession = Keys.Control | Keys.Alt | Keys.Left;
			public const Keys SelectSession = Keys.Control | Keys.Alt | Keys.Right;
		}
		private readonly PluginWindows _plugin;
		private SettingsBll _xmlSettings;

		private Boolean _isThumbnail = Defaults.IsThumbnail;
		private Boolean _useMultipleMonitors = Defaults.UseMultipleMonitors;
		private Boolean _closeWindowAfterDisconnect = Defaults.CloseWindowAfterDisconnect;
		private RdpClient.SessionResizeMode _resizeMode = Defaults.ResizeMode;
		private String _user;
		private String _password;
		private String _domain;

		private Keys _altTab = Defaults.AltTab;
		private Keys _altShiftTab = Defaults.AltShiftTab;
		private Keys _altEsc = Defaults.AltEsc;
		private Keys _altSpace = Defaults.AltSpace;

		private Keys _ctrlEsc = Defaults.CtrlEsc;
		private Keys _ctrlAltDelete = Defaults.CtrlAltDelete;

		private Keys _fullScreen = Defaults.FullScreen;
		private Keys _previousSession = Defaults.PreviousSession;
		private Keys _selectSession = Defaults.SelectSession;

		/// <summary>RDP client settings</summary>
		internal SettingsBll XmlSettings
			=> this._xmlSettings ?? (this._xmlSettings = new SettingsBll(this._plugin));

		//TODO: So far it's just a stub
		[DefaultValue(Defaults.IsThumbnail)]
		internal Boolean IsThumbnail
		{
			get => this._isThumbnail;
			set => this.SetField(ref this._isThumbnail, value, nameof(this.IsThumbnail));
		}

		[Category("Appearance")]
		[DisplayName("Use multiple monitors")]
		[Description("Open Remote Desktop Connection on all monitors")]
		[DefaultValue(Defaults.UseMultipleMonitors)]
		public Boolean UseMultipleMonitors
		{
			get => this._useMultipleMonitors;
			set => this.SetField(ref this._useMultipleMonitors, value, nameof(this.UseMultipleMonitors));
		}

		[Category("Appearance")]
		[DisplayName("Close after disconnect")]
		[Description("Close the connection window after disconnecting from the remote server")]
		[DefaultValue(Defaults.CloseWindowAfterDisconnect)]
		public Boolean CloseWindowAfterDisconnect
		{
			get => this._closeWindowAfterDisconnect;
			set => this.SetField(ref this._closeWindowAfterDisconnect, value, nameof(this.CloseWindowAfterDisconnect));
		}

		[Category("Appearance")]
		[DisplayName("Session resize mode")]
		[Description(@"How the remote session is fitted into the window when the window is resized (applies to new connections with ""Same as client area"" desktop size):
Auto - Dynamic, then Reconnect, then SmartSizing
Dynamic - change remote resolution without reconnect (Windows 8.1 / Server 2012 R2 and newer)
Reconnect - reconnect with the new resolution
SmartSizing - scale the image without changing remote resolution
None - do not resize")]
		[DefaultValue(Defaults.ResizeMode)]
		public RdpClient.SessionResizeMode ResizeMode
		{
			get => this._resizeMode;
			set => this.SetField(ref this._resizeMode, value, nameof(this.ResizeMode));
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
		[DefaultValue(Defaults.AltTab)]
		public Keys AltTab
		{
			get => this._altTab;
			set => this.SetField(ref this._altTab, value, nameof(this.AltTab));
		}

		[Category("ALT hot keys")]
		[DisplayName("ALT+SHIFT+TAB")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Defaults.AltShiftTab)]
		public Keys AltShiftTab
		{
			get => this._altShiftTab;
			set => this.SetField(ref this._altShiftTab, value, nameof(this.AltShiftTab));
		}

		[Category("ALT hot keys")]
		[DisplayName("ALT+ESC")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Defaults.AltEsc)]
		public Keys AltEsc
		{
			get => this._altEsc;
			set => this.SetField(ref this._altEsc, value, nameof(this.AltEsc));
		}

		[Category("ALT hot keys")]
		[DisplayName("ALT+SPACE")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Defaults.AltSpace)]
		public Keys AltSpace
		{
			get => this._altSpace;
			set => this.SetField(ref this._altSpace, value, nameof(this.AltSpace));
		}

		[Category("ALT hot keys")]
		[DisplayName("CTRL+ESC")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Defaults.CtrlEsc)]
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
		[DefaultValue(Defaults.CtrlAltDelete)]
		public Keys CtrlAltDel
		{
			get => this._ctrlAltDelete;
			set => this.SetField(ref this._ctrlAltDelete, value, nameof(this.CtrlAltDel));
		}

		[Category("CTRL+ALT hot keys")]
		[DisplayName("Full screen")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Defaults.FullScreen)]
		public Keys FullScreen
		{
			get => this._fullScreen;
			set => this.SetField(ref this._fullScreen, value, nameof(this.FullScreen));
		}

		[Category("CTRL+ALT hot keys")]
		[DisplayName("Previous session")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Defaults.PreviousSession)]
		public Keys PreviousSession
		{
			get => this._previousSession;
			set => this.SetField(ref this._previousSession, value, nameof(this.PreviousSession));
		}

		[Category("CTRL+ALT hot keys")]
		[DisplayName("Select session")]
		[Editor(typeof(ShortcutKeysEditor), typeof(UITypeEditor))]
		[DefaultValue(Defaults.SelectSession)]
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