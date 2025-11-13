using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MSTSCLib;
using Plugin.RDP.Bll;

namespace Plugin.RDP.RDP
{
	public class RdpClient
	{
		public enum ConnectionBarState
		{
			AutoHide,
			Pinned,
			Off
		}
		public enum AudioRedirectionMode : UInt32
		{
			/// <summary>Audio redirection is enabled and the option for redirection is "Bring to this computer". This is the default mode.</summary>
			Redirect = 0,
			/// <summary>
			/// Audio redirection is enabled and the option is "Leave at remote computer".
			/// The "Leave at remote computer" option is supported only when connecting remotely to a host computer that is running Windows Vista.
			/// If the connection is to a host computer that is running Windows Server 2008, the option "Leave at remote computer" is changed to "Do not play".
			/// </summary>
			Remote = 1,
			/// <summary>Audio redirection is enabled and the mode is "Do not play".</summary>
			NoSound = 2,
		}
		public enum AudioRedirectionQuality
		{
			Dynamic,
			High,
			Medium
		}
		public enum AudioCaptureRedirectionMode
		{
			DoNotRecord,
			Record
		}
		public enum KeyboardHookMode
		{
			/// <summary>Apply key combinations only locally at the client computer.</summary>
			Local = 0,
			/// <summary>Apply key combinations at the remote server.</summary>
			Remote = 1,
			/// <summary>Apply key combinations to the remote server only when the client is running in full-screen mode. This is the default value.</summary>
			FullScreen = 2,
		}

		/// <summary>Indicates the connection state of the control</summary>
		/// <remarks>https://msdn.microsoft.com/en-us/library/aa382835(v=vs.85).aspx</remarks>
		public enum ConnectionState
		{
			/// <summary>The control is not connected.</summary>
			Disconnected = 0,
			/// <summary>The control is connected.</summary>
			Connected = 1,
			/// <summary>The control is establishing a connection.</summary>
			Connecting = 2,
			/// <summary>Try to establish lost connection</summary>
			Reconnecting = 3,
		}

		public enum GatewayUsageMethod
		{
			NoneDirect,
			ProxyDirect,
			ProxyDetect,
			Default,
			NoneDetect
		}
		public enum GatewayLogonMethod : UInt32
		{
			NTLM,
			SmartCard,
			Any = 4
		}
		public enum AuthenticationLevel
		{
			None,
			Required,
			Warn
		}

		public enum RdpClientVersion
		{
			Version5 = 5,
			Version6 = 6,
			Version7 = 7,
			Version8 = 8,
		}

		public const Int32 DefaultColorDepth = 24;
		public const Int32 DefaultRDPPort = 3389;
		//public const Int32 DefaultVMConsoleConnectPort = 2179;
		public const Int32 PerfDisableCursorBlinking = 64;
		public const Int32 PerfDisableCursorShadow = 32;
		public const Int32 PerfDisableFullWindowDrag = 2;
		public const Int32 PerfDisableMenuAnimations = 4;
		public const Int32 PerfDisableNothing = 0;
		public const Int32 PerfDisableTheming = 8;
		public const Int32 PerfDisableWallpaper = 1;
		public const Int32 PerfEnableDesktopComposition = 256;
		public const Int32 PerfEnableEnhancedGraphics = 16;
		public const Int32 PerfEnableFontSmoothing = 128;
		private const Int32 MaxFreeClients = 0;
		public static Boolean SupportsGatewayCredentials = false;
		public static Boolean SupportsAdvancedAudioVideoRedirection = false;
		public static Boolean SupportsFineGrainedRedirection = false;
		public static Boolean SupportsMonitorSpanning = false;
		public static Boolean SupportsRemoteSessionActions = false;
		public static Boolean SupportsPanning = false;
		private static Stack<RdpClient> FreeList = new Stack<RdpClient>(0);
		private PluginWindows _plugin;
		public static Int32 MaxDesktopHeight;
		public static Int32 MaxDesktopWidth;
		private static readonly Object _initializeLock = new Object();
		private static Boolean _initialized = false;
		private static RdpClientVersion UseClientVersion;
		public MethodInvoker GotFocus;
		public MethodInvoker LostFocus;
		private readonly RdpClient5 _rdpClient5;
		private readonly RdpClient6 _rdpClient6;
		private readonly RdpClient7 _rdpClient7;
		private readonly RdpClient8 _rdpClient8;

		public Control Control
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8;
				else if(this._rdpClient7 != null)
					return this._rdpClient7;
				else if(this._rdpClient6 != null)
					return this._rdpClient6;
				else return this._rdpClient5;
			}
		}

		public IMsRdpClient MsRdpClient
			=> this.GetOcx() as IMsRdpClient;

		private IMsRdpClient8 MsRdpClient8
			=> this.MsRdpClient as IMsRdpClient8;

		private IMsRdpClient10 MsRdpClient10
			=> this.MsRdpClient as IMsRdpClient10;

		/// <summary>Retrieves the connection state of the current control</summary>
		/// <remarks>https://msdn.microsoft.com/en-us/library/aa382835(v=vs.85).aspx</remarks>
		public ConnectionState ConnectionStatus
			=> (ConnectionState)this.MsRdpClient.Connected;

		public Version Version
		{
			get
			{
				String result;
				if(this._rdpClient8 != null)
					result = this._rdpClient8.Version;
				else if(this._rdpClient7 != null)
					result = this._rdpClient7.Version;
				else if(this._rdpClient6 != null)
					result = this._rdpClient6.Version;
				else if(this._rdpClient5 != null)
					result = this._rdpClient5.Version;
				else throw new NotSupportedException();
				return new Version(result);
			}
		}

		public IMsRdpClientAdvancedSettings AdvancedSettings2
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.AdvancedSettings2;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.AdvancedSettings2;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.AdvancedSettings2;
				else
					return this._rdpClient5.AdvancedSettings2;
			}
		}

		public IMsRdpClientAdvancedSettings4 AdvancedSettings5
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.AdvancedSettings5;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.AdvancedSettings5;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.AdvancedSettings5;
				else
					return this._rdpClient5.AdvancedSettings5;
			}
		}

		public IMsRdpClientAdvancedSettings5 AdvancedSettings6
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.AdvancedSettings6;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.AdvancedSettings6;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.AdvancedSettings6;
				else
					return this._rdpClient5.AdvancedSettings6;
			}
		}

		public IMsRdpClientAdvancedSettings6 AdvancedSettings7
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.AdvancedSettings7;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.AdvancedSettings7;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.AdvancedSettings7;
				else
					return null;
			}
		}

		public IMsRdpClientAdvancedSettings7 AdvancedSettings8
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.AdvancedSettings8;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.AdvancedSettings8;
				else
					return null;
			}
		}

		private IMsRdpClientNonScriptable3 ClientNonScriptable3
			=> this.GetOcx() as IMsRdpClientNonScriptable3;

		public IMsRdpClientTransportSettings TransportSettings
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.TransportSettings;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.TransportSettings;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.TransportSettings;
				else
					return this._rdpClient5.TransportSettings;
			}
		}

		public IMsRdpClientTransportSettings2 TransportSettings2
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.TransportSettings2;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.TransportSettings2;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.TransportSettings2;
				else
					return null;
			}
		}

		/*public IMsRdpClientTransportSettings3 TransportSettings3
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.TransportSettings3;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.TransportSettings3;
				else
					return null;
			}
		}*/

		public IMsTscSecuredSettings SecuredSettings
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.SecuredSettings;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.SecuredSettings;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.SecuredSettings;
				return this._rdpClient5.SecuredSettings;
			}
		}

		public IMsRdpClientSecuredSettings SecuredSettings2
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.SecuredSettings2;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.SecuredSettings2;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.SecuredSettings2;
				else
					return this._rdpClient5.SecuredSettings2;
			}
		}

		public ITSRemoteProgram RemoteProgram
		{
			get
			{
				if(this._rdpClient8 != null)
					return this._rdpClient8.RemoteProgram;
				else if(this._rdpClient7 != null)
					return this._rdpClient7.RemoteProgram;
				else if(this._rdpClient6 != null)
					return this._rdpClient6.RemoteProgram;
				else
					return this._rdpClient5.RemoteProgram;
			}
		}

		public static String AudioRedirectionModeToString(RdpClient.AudioRedirectionMode mode)
		{
			switch(mode)
			{
				case RdpClient.AudioRedirectionMode.Redirect:
					return "Bring to this computer";
				case RdpClient.AudioRedirectionMode.Remote:
					return "Leave at remote computer";
				case RdpClient.AudioRedirectionMode.NoSound:
					return "Do not play";
				default:
					throw new InvalidOperationException("Unexpected AudioRedirectionMode:" + mode.ToString());
			}
		}

		public static RdpClient.AudioRedirectionMode AudioRedirectionModeFromString(String text)
			=> Utils.EnumFromString<RdpClient.AudioRedirectionMode>(text, new Utils.EnumToStringDelegate<RdpClient.AudioRedirectionMode>(RdpClient.AudioRedirectionModeToString));

		public static String AudioRedirectionQualityToString(RdpClient.AudioRedirectionQuality mode)
		{
			switch(mode)
			{
				case RdpClient.AudioRedirectionQuality.Dynamic:
					return "Dynamically adjusted";
				case RdpClient.AudioRedirectionQuality.High:
					return "High quality";
				case RdpClient.AudioRedirectionQuality.Medium:
					return "Medium quality";
				default:
					throw new InvalidOperationException("Unexpected AudioRedirectionQuality:" + mode.ToString());
			}
		}

		public static RdpClient.AudioRedirectionQuality AudioRedirectionQualityFromString(String text)
			=> Utils.EnumFromString<RdpClient.AudioRedirectionQuality>(text, new Utils.EnumToStringDelegate<RdpClient.AudioRedirectionQuality>(RdpClient.AudioRedirectionQualityToString));

		public static String AudioCaptureRedirectionModeToString(RdpClient.AudioCaptureRedirectionMode mode)
		{
			switch(mode)
			{
				case RdpClient.AudioCaptureRedirectionMode.DoNotRecord:
					return "Do not record";
				case RdpClient.AudioCaptureRedirectionMode.Record:
					return "Record from this computer";
				default:
					throw new InvalidOperationException("Unexpected AudioCaptureRedirectionMode:" + mode.ToString());
			}
		}

		public static RdpClient.AudioCaptureRedirectionMode AudioCaptureRedirectionModeFromString(String text)
			=> Utils.EnumFromString<RdpClient.AudioCaptureRedirectionMode>(text, new Utils.EnumToStringDelegate<RdpClient.AudioCaptureRedirectionMode>(RdpClient.AudioCaptureRedirectionModeToString));

		public static String KeyboardHookModeToString(RdpClient.KeyboardHookMode mode)
		{
			switch(mode)
			{
				case RdpClient.KeyboardHookMode.Local:
					return "On the local computer";
				case RdpClient.KeyboardHookMode.Remote:
					return "On the remote computer";
				case RdpClient.KeyboardHookMode.FullScreen:
					return "In full screen mode only";
				default:
					throw new InvalidOperationException("Unexpected KeyboardHookMode:" + mode.ToString());
			}
		}

		public static RdpClient.KeyboardHookMode KeyboardHookModeFromString(String text)
			=> Utils.EnumFromString<RdpClient.KeyboardHookMode>(text, new Utils.EnumToStringDelegate<RdpClient.KeyboardHookMode>(RdpClient.KeyboardHookModeToString));

		public static String GatewayLogonMethodToString(RdpClient.GatewayLogonMethod mode)
		{
			switch(mode)
			{
				case RdpClient.GatewayLogonMethod.NTLM:
					return "Ask for password (NTLM)";
				case RdpClient.GatewayLogonMethod.SmartCard:
					return "Smart card";
				case RdpClient.GatewayLogonMethod.Any:
					return "Allow me to select later";
			}
			return null;
		}

		public static RdpClient.GatewayLogonMethod GetGatewayLogonMethod(String text)
			=> Utils.EnumFromString<RdpClient.GatewayLogonMethod>(text, new Utils.EnumToStringDelegate<RdpClient.GatewayLogonMethod>(RdpClient.GatewayLogonMethodToString));

		public static String GatewayUsageMethodToString(RdpClient.GatewayUsageMethod mode)
		{
			if(mode == RdpClient.GatewayUsageMethod.NoneDirect)
				return "Do not use a Gateway server";
			if(mode == RdpClient.GatewayUsageMethod.NoneDetect)
				return "Automatically detect Gateway";
			throw new InvalidOperationException("Unexpected GatewayUsageMethod:" + mode.ToString());
		}

		public static RdpClient.GatewayUsageMethod GatewayUsageMethodFromString(String text)
			=> Utils.EnumFromString<RdpClient.GatewayUsageMethod>(text, new Utils.EnumToStringDelegate<RdpClient.GatewayUsageMethod>(RdpClient.GatewayUsageMethodToString));

		public static String AuthenticationLevelToString(RdpClient.AuthenticationLevel mode)
		{
			switch(mode)
			{
				case RdpClient.AuthenticationLevel.None:
					return "No authentication";
				case RdpClient.AuthenticationLevel.Required:
					return "Do not connect if authentication fails";
				case RdpClient.AuthenticationLevel.Warn:
					return "Warn if authentication fails";
				default:
					throw new InvalidOperationException("Unexpected AuthenticationLevel:" + mode.ToString());
			}
		}

		public static RdpClient.AuthenticationLevel AuthenticationLevelFromString(String text)
			=> Utils.EnumFromString<RdpClient.AuthenticationLevel>(text, new Utils.EnumToStringDelegate<RdpClient.AuthenticationLevel>(RdpClient.AuthenticationLevelToString));

		private RdpClient(PluginWindows plugin, UserControl form)
		{
			this._plugin = plugin;
			switch(RdpClient.UseClientVersion)
			{
			case RdpClientVersion.Version8:
				this._rdpClient8 = new RdpClient8(this, form);
				break;
			case RdpClientVersion.Version7:
				this._rdpClient7 = new RdpClient7(this, form);
				break;
			case RdpClientVersion.Version6:
				this._rdpClient6 = new RdpClient6(this, form);
				break;
			default:
				this._rdpClient5 = new RdpClient5(this, form);
				break;
			}
			this.GotFocus = (MethodInvoker)Delegate.Combine(this.GotFocus, new MethodInvoker(this.UpdateLabel));
			this.LostFocus = (MethodInvoker)Delegate.Combine(this.LostFocus, new MethodInvoker(this.UpdateLabel));
		}

		public void UpdateLabel()
		{ }

		public void SetDesktopSize(Size size)
		{
			this.MsRdpClient.DesktopHeight = Math.Min(RdpClient.MaxDesktopHeight, size.Height);
			this.MsRdpClient.DesktopWidth = Math.Min(RdpClient.MaxDesktopWidth, size.Width);
		}

		public static void Initialize(PluginWindows plugin, UserControl form)
		{
			if(!RdpClient._initialized)
				lock(RdpClient._initializeLock)
					if(!RdpClient._initialized)
					{
						try
						{
							Version version;
							using(RdpClient5 client = new RdpClient5(null, form))
							{
								plugin.Trace.TraceInformation("RDP client version: {0}", client.Version);
								version = new Version(client.Version);
							}

							RdpClient.UseClientVersion = RdpClientVersion.Version5;
							if(version.Build >= 6001)
							{
								RdpClient.UseClientVersion = RdpClientVersion.Version6;
								RdpClient.SupportsMonitorSpanning = true;
								if(version.Build >= 7600)
									RdpClient.UseClientVersion = RdpClientVersion.Version7;
								if(version.Build >= 9200)
									RdpClient.UseClientVersion = RdpClientVersion.Version8;
							}
							for(Int32 loop = 0; loop < Math.Max(1, 0); loop++)
								RdpClient.FreeList.Push(new RdpClient(plugin, form));

							RdpClient rdpClient = RdpClient.FreeList.Peek();
							RdpClient.MaxDesktopWidth = 4096;
							RdpClient.MaxDesktopHeight = 2048;
							if(rdpClient.AdvancedSettings7 != null)
								RdpClient.SupportsGatewayCredentials = true;
							if(rdpClient.AdvancedSettings8 != null)
								RdpClient.SupportsAdvancedAudioVideoRedirection = true;
							if(rdpClient.ClientNonScriptable3 != null)
								RdpClient.SupportsFineGrainedRedirection = true;
							if(rdpClient.MsRdpClient8 != null)
								RdpClient.SupportsRemoteSessionActions = true;
						} finally
						{
							RdpClient._initialized = true;
						}
					}
		}

		public static RdpClient AllocClient(PluginWindows plugin, UserControl form)
		{
			RdpClient rdpClient = null;
			Stack<RdpClient> freeList;
			Monitor.Enter(freeList = RdpClient.FreeList);
			try
			{
				if(RdpClient.FreeList.Count > 0)
					rdpClient = RdpClient.FreeList.Pop();
			} finally
			{
				Monitor.Exit(freeList);
			}
			if(rdpClient == null)
				rdpClient = new RdpClient(plugin, form);
			return rdpClient;
		}

		public static void ReleaseClient(RdpClient client)
		{
			client._plugin = null;
			if(RdpClient.FreeList.Count >= 0)
			{
				//TODO: Commented
				//Program.TheForm.RemoveFromClientPanel(client.Control);
				return;
			}
			Stack<RdpClient> freeList;
			Monitor.Enter(freeList = RdpClient.FreeList);
			try
			{
				RdpClient.FreeList.Push(client);
			} finally
			{
				Monitor.Exit(freeList);
			}
		}

		public void GoFullScreen()
		{
			this.AdvancedSettings2.SmartSizing = false;
			this.MsRdpClient.FullScreen = true;
		}

		public void LeaveFullScreen()
		{
			this.AdvancedSettings2.SmartSizing = true;
			this.MsRdpClient.FullScreen = false;
		}

		public void SetText()
		{
			const String connecting = "Connecting...";
			const String disconnected = "Disconnected";
			if(this._rdpClient8 != null)
			{
				this._rdpClient8.ConnectingText = connecting;
				this._rdpClient8.DisconnectedText = disconnected;
			} else if(this._rdpClient7 != null)
			{
				this._rdpClient7.ConnectingText = connecting;
				this._rdpClient7.DisconnectedText = disconnected;
			} else if(this._rdpClient6 != null)
			{
				this._rdpClient6.ConnectingText = connecting;
				this._rdpClient6.DisconnectedText = disconnected;
			} else
			{
				this._rdpClient5.ConnectingText = connecting;
				this._rdpClient5.DisconnectedText = disconnected;
			}
		}

		public Object GetOcx()
		{
			if(this._rdpClient8 != null)
				return this._rdpClient8.GetOcx();
			else if(this._rdpClient7 != null)
				return this._rdpClient7.GetOcx();
			else if(this._rdpClient6 != null)
				return this._rdpClient6.GetOcx();
			else
				return this._rdpClient5.GetOcx();
		}

		public String GetErrorDescription(UInt32 disconnectReason)
		{
			if(this._rdpClient8 != null)
				return this._rdpClient8.GetErrorDescription(disconnectReason, (UInt32)this.MsRdpClient.ExtendedDisconnectReason);
			if(this._rdpClient7 != null)
				return this._rdpClient7.GetErrorDescription(disconnectReason, (UInt32)this.MsRdpClient.ExtendedDisconnectReason);
			if(this._rdpClient6 != null)
				return this._rdpClient6.GetErrorDescription(disconnectReason, (UInt32)this.MsRdpClient.ExtendedDisconnectReason);
			else
				return this._rdpClient5.GetErrorDescription(disconnectReason, (UInt32)this.MsRdpClient.ExtendedDisconnectReason);
		}

		#region Events
		public event EventHandler OnConnected
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnConnected += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnConnected += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnConnected += value;
				else
					this._rdpClient5.OnConnected += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnConnected -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnConnected -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnConnected -= value;
				else
					this._rdpClient5.OnConnected -= value;
			}
		}

		public event EventHandler OnConnecting
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnConnecting += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnConnecting += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnConnecting += value;
				else
					this._rdpClient5.OnConnecting += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnConnecting -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnConnecting -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnConnecting -= value;
				else
					this._rdpClient5.OnConnecting -= value;
			}
		}

		public event AxMSTSCLib.IMsTscAxEvents_OnAutoReconnectingEventHandler OnAutoReconnecting
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnAutoReconnecting += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnAutoReconnecting += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnAutoReconnecting += value;
				else
					this._rdpClient5.OnAutoReconnecting += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnAutoReconnecting -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnAutoReconnecting -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnAutoReconnecting -= value;
				else
					this._rdpClient5.OnAutoReconnecting -= value;
			}
		}

		public event AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEventHandler OnDisconnected
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnDisconnected += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnDisconnected += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnDisconnected += value;
				else
					this._rdpClient5.OnDisconnected += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnDisconnected -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnDisconnected -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnDisconnected -= value;
				else
					this._rdpClient5.OnDisconnected -= value;
			}
		}

		public event EventHandler OnRequestGoFullScreen
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnRequestGoFullScreen += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnRequestGoFullScreen += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnRequestGoFullScreen += value;
				else
					this._rdpClient5.OnRequestGoFullScreen += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnRequestGoFullScreen -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnRequestGoFullScreen -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnRequestGoFullScreen -= value;
				else
					this._rdpClient5.OnRequestGoFullScreen -= value;
			}
		}

		public event EventHandler OnRequestLeaveFullScreen
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnRequestLeaveFullScreen += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnRequestLeaveFullScreen += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnRequestLeaveFullScreen += value;
				else
					this._rdpClient5.OnRequestLeaveFullScreen += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnRequestLeaveFullScreen -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnRequestLeaveFullScreen -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnRequestLeaveFullScreen -= value;
				else
					this._rdpClient5.OnRequestLeaveFullScreen -= value;
			}
		}

		public event EventHandler OnRequestContainerMinimize
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnRequestContainerMinimize += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnRequestContainerMinimize += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnRequestContainerMinimize += value;
				else
					this._rdpClient5.OnRequestContainerMinimize += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnRequestContainerMinimize -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnRequestContainerMinimize -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnRequestContainerMinimize -= value;
				else
					this._rdpClient5.OnRequestContainerMinimize -= value;
			}
		}

		public event AxMSTSCLib.IMsTscAxEvents_OnConfirmCloseEventHandler OnConfirmClose
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnConfirmClose += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnConfirmClose += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnConfirmClose += value;
				else
					this._rdpClient5.OnConfirmClose += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnConfirmClose -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnConfirmClose -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnConfirmClose -= value;
				else
					this._rdpClient5.OnConfirmClose -= value;
			}
		}

		public event EventHandler OnIdleTimeoutNotification
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnIdleTimeoutNotification += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnIdleTimeoutNotification += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnIdleTimeoutNotification += value;
				else
					this._rdpClient5.OnIdleTimeoutNotification += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnIdleTimeoutNotification -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnIdleTimeoutNotification -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnIdleTimeoutNotification -= value;
				else
					this._rdpClient5.OnIdleTimeoutNotification -= value;
			}
		}
		#endregion Events
	}
}