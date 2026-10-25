using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MSTSCLib;
using Plugin.RDP.Bll;

namespace Plugin.RDP.RDP
{
	public class RdpClient
	{
		/// <summary>Indicates the state of the connection bar</summary>
		public enum ConnectionBarState
		{
			/// <summary>The connection bar is set to auto-hide.</summary>
			AutoHide,
			/// <summary>The connection bar is pinned.</summary>
			Pinned,
			/// <summary>The connection bar is off.</summary>
			Off
		}

		/// <summary>Indicates how audio redirection is applied</summary>
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

		/// <summary>Indicates the quality of audio redirection</summary>
		public enum AudioRedirectionQuality
		{
			/// <summary>Audio quality is dynamically adjusted based on network performance. This is the default mode.</summary>
			Dynamic,
			/// <summary>High-quality audio redirection.</summary>
			High,
			/// <summary>Medium-quality audio redirection.</summary>
			Medium,
		}

		/// <summary>Indicates how audio capture redirection is applied</summary>
		public enum AudioCaptureRedirectionMode
		{
			/// <summary>Audio capture redirection is disabled.</summary>
			DoNotRecord,
			/// <summary>Audio capture redirection is enabled.</summary>
			Record
		}

		/// <summary>Indicates how key combinations are applied</summary>
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

		/// <summary>Indicates how the Remote Desktop Gateway server is used</summary>
		public enum GatewayUsageMethod
		{
			NoneDirect,
			ProxyDirect,
			ProxyDetect,
			Default,
			NoneDetect
		}

		/// <summary>Indicates the logon method for the Remote Desktop Gateway server</summary>
		public enum GatewayLogonMethod : UInt32
		{
			NTLM,
			SmartCard,
			Any = 4
		}

		/// <summary>Indicates the authentication level</summary>
		public enum AuthenticationLevel
		{
			None,
			Required,
			Warn
		}

		/// <summary>User preferred method to fit remote session into the client area</summary>
		public enum SessionResizeMode
		{
			/// <summary>Best available method: Dynamic → Reconnect → SmartSizing</summary>
			Auto,
			/// <summary>Change remote resolution without reconnect (Windows 8.1 / Server 2012 R2 and newer)</summary>
			Dynamic,
			/// <summary>Reconnect to the session with the new resolution</summary>
			Reconnect,
			/// <summary>Keep remote resolution and scale image on the client side</summary>
			SmartSizing,
			/// <summary>Do not resize remote session</summary>
			None,
		}

		/// <summary>Method that is used to fit remote session into the client area</summary>
		public enum ResizeMethod
		{
			/// <summary>Remote session was not resized</summary>
			None,
			/// <summary>Remote resolution changed without reconnect (Windows 8.1 / Server 2012 R2 and newer)</summary>
			Dynamic,
			/// <summary>Session reconnected with the new resolution</summary>
			Reconnect,
			/// <summary>Remote resolution unchanged, image scaled on the client side</summary>
			SmartSizing,
		}

		/// <summary>RDP Client versions</summary>
		public enum RdpClientVersion
		{
			Version5 = 5,
			Version6 = 6,
			Version7 = 7,
			Version8 = 8,
		}

		public const Int32 DefaultColorDepth = 24;
		public const Int32 DefaultRDPPort = 3389;
		public const Int32 DefaultVMConsoleConnectPort = 2179;
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
		/// <summary>Display control channel limits (MS-RDPEDISP 2.2.2.2.1)</summary>
		private const Int32 DynamicMinSize = 200;
		private const Int32 DynamicMaxSize = 8192;
		/// <summary>Server rejected dynamic resolution in the current session. Fallback methods are used until next connection</summary>
		private Boolean _isDynamicResizeFailed;
		/// <summary>Last size that was requested from the server in the current session</summary>
		private Size _lastResizeSize;
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

		private IMsRdpClient9 MsRdpClient9
			=> this.MsRdpClient as IMsRdpClient9;

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

		/// <summary>Sets initial remote desktop size. Can be called only before connection</summary>
		public void SetDesktopSize(Size size)
		{
			this.MsRdpClient.DesktopHeight = Math.Min(RdpClient.MaxDesktopHeight, size.Height);
			this.MsRdpClient.DesktopWidth = Math.Min(RdpClient.MaxDesktopWidth, size.Width) & ~1;//Even width to be compatible with dynamic resize
		}

		/// <summary>Resets resize state. Should be called when session is disconnected</summary>
		public void ResetResizeState()
		{
			this._isDynamicResizeFailed = false;
			this._lastResizeSize = Size.Empty;
		}

		/// <summary>Fits remote session to the client area (or to the screen in full screen mode) using the method selected by user</summary>
		/// <param name="size">New client area size</param>
		/// <param name="mode">User preferred resize method. Explicitly selected method is not replaced with another one if it fails</param>
		/// <param name="allowReconnect">Reconnect is supported by the current connection type</param>
		/// <param name="force">Resize even if the size is the same as last requested (remote size was changed outside, e.g. by full screen mode)</param>
		/// <returns>Method that was used to fit remote session</returns>
		public ResizeMethod ResizeSession(Size size, SessionResizeMode mode, Boolean allowReconnect, Boolean force)
		{
			if(mode == SessionResizeMode.None || this.ConnectionStatus != ConnectionState.Connected)
				return ResizeMethod.None;

			Size newSize = new Size(
				Math.Max(RdpClient.DynamicMinSize, Math.Min(RdpClient.DynamicMaxSize, size.Width)) & ~1,
				Math.Max(RdpClient.DynamicMinSize, Math.Min(RdpClient.DynamicMaxSize, size.Height)));
			Size currentSize = this._lastResizeSize.IsEmpty
				? new Size(this.MsRdpClient.DesktopWidth, this.MsRdpClient.DesktopHeight)
				: this._lastResizeSize;
			if(!force && newSize == currentSize)
				return ResizeMethod.None;

			ResizeMethod result = ResizeMethod.None;
			switch(mode)
			{
			case SessionResizeMode.Auto:
				if(!this._isDynamicResizeFailed && this.UpdateSessionDisplaySettings(newSize))
					result = ResizeMethod.Dynamic;
				else if(allowReconnect && this.Reconnect(newSize))
					result = ResizeMethod.Reconnect;
				else
				{
					this.AdvancedSettings2.SmartSizing = true;
					result = ResizeMethod.SmartSizing;
				}
				break;
			case SessionResizeMode.Dynamic:
				if(!this._isDynamicResizeFailed && this.UpdateSessionDisplaySettings(newSize))
					result = ResizeMethod.Dynamic;
				break;
			case SessionResizeMode.Reconnect:
				if(!allowReconnect)
					this._plugin?.Trace.TraceEvent(TraceEventType.Warning, 0, "Reconnect with new resolution is not supported for {0} connection type", this.MsRdpClient.Server);
				else if(this.Reconnect(newSize))
					result = ResizeMethod.Reconnect;
				break;
			case SessionResizeMode.SmartSizing:
				this.AdvancedSettings2.SmartSizing = true;
				result = ResizeMethod.SmartSizing;
				break;
			default:
				throw new NotSupportedException(mode.ToString());
			}

			this._lastResizeSize = newSize;//Failed size is also stored to prevent retries with the same size
			return result;
		}

		/// <summary>Changes remote resolution without reconnect using display control virtual channel</summary>
		/// <remarks>Requires Windows 8.1 / Server 2012 R2 or newer on the server side</remarks>
		private Boolean UpdateSessionDisplaySettings(Size size)
		{
			IMsRdpClient9 client = this.MsRdpClient9;
			if(client == null)
			{
				this._isDynamicResizeFailed = true;
				this._plugin?.Trace.TraceEvent(TraceEventType.Warning, 0, "Dynamic resize is not supported by RDP client version {0}", this.Version);
				return false;
			}

			// Physical size in millimeters. Valid range is 10..10000, otherwise it is ignored by the server.
			Int32 dpi = this.Control.DeviceDpi > 0 ? this.Control.DeviceDpi : 96;
			UInt32 physicalWidth = (UInt32)Math.Max(10, Math.Min(10000, size.Width * 254 / (dpi * 10)));
			UInt32 physicalHeight = (UInt32)Math.Max(10, Math.Min(10000, size.Height * 254 / (dpi * 10)));

			try
			{
				// orientation: 0 - landscape
				// desktopScaleFactor: 100..500 (percent)
				// deviceScaleFactor: 100, 140 or 180
				client.UpdateSessionDisplaySettings((UInt32)size.Width, (UInt32)size.Height, physicalWidth, physicalHeight, 0, 100, 100);
				this.AdvancedSettings2.SmartSizing = false;
				return true;
			} catch(Exception exc) when(exc is COMException || exc is ArgumentException || exc is NotImplementedException)
			{
				this._isDynamicResizeFailed = true;
				this._plugin?.Trace.TraceEvent(TraceEventType.Warning, 0, "Dynamic resize is not supported by {0}: {1}", this.MsRdpClient.Server, exc.Message);
				return false;
			}
		}

		/// <summary>Reconnects to the session with new resolution</summary>
		private Boolean Reconnect(Size size)
		{
			IMsRdpClient8 client = this.MsRdpClient8;
			if(client == null)
				return false;

			UInt32 width = (UInt32)(Math.Min(RdpClient.MaxDesktopWidth, size.Width) & ~1);
			UInt32 height = (UInt32)Math.Min(RdpClient.MaxDesktopHeight, size.Height);
			try
			{
				if(client.Reconnect(width, height) == ControlReconnectStatus.controlReconnectStarted)
				{
					this.AdvancedSettings2.SmartSizing = false;
					return true;
				}
			} catch(COMException exc)
			{
				this._plugin?.Trace.TraceEvent(TraceEventType.Warning, 0, "Reconnect with new resolution failed for {0}: {1}", this.MsRdpClient.Server, exc.Message);
			}
			return false;
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
								plugin.Trace.TraceEvent(TraceEventType.Information, 0, "RDP client version: {0}", client.Version);
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
			this.MsRdpClient.FullScreen = false;

			// Force redraw after leaving full screen
			this.Control.Refresh();
			this.Control.Update();
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

		public event EventHandler OnLoginComplete
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnLoginComplete += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnLoginComplete += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnLoginComplete += value;
				else
					this._rdpClient5.OnLoginComplete += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnLoginComplete -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnLoginComplete -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnLoginComplete -= value;
				else
					this._rdpClient5.OnLoginComplete -= value;
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

		public event EventHandler OnEnterFullScreenMode
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnEnterFullScreenMode += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnEnterFullScreenMode += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnEnterFullScreenMode += value;
				else
					this._rdpClient5.OnEnterFullScreenMode += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnEnterFullScreenMode -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnEnterFullScreenMode -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnEnterFullScreenMode -= value;
				else
					this._rdpClient5.OnEnterFullScreenMode -= value;
			}
		}

		public event EventHandler OnLeaveFullScreenMode
		{
			add
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnLeaveFullScreenMode += value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnLeaveFullScreenMode += value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnLeaveFullScreenMode += value;
				else
					this._rdpClient5.OnLeaveFullScreenMode += value;
			}
			remove
			{
				if(this._rdpClient8 != null)
					this._rdpClient8.OnLeaveFullScreenMode -= value;
				else if(this._rdpClient7 != null)
					this._rdpClient7.OnLeaveFullScreenMode -= value;
				else if(this._rdpClient6 != null)
					this._rdpClient6.OnLeaveFullScreenMode -= value;
				else
					this._rdpClient5.OnLeaveFullScreenMode -= value;
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