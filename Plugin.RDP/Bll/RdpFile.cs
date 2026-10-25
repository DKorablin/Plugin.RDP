using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Plugin.RDP.RDP;

namespace Plugin.RDP.Bll
{
	/// <summary>Reader and writer of the Microsoft Remote Desktop Connection (.rdp) file format</summary>
	/// <remarks>https://learn.microsoft.com/en-us/azure/virtual-desktop/rdp-properties</remarks>
	internal class RdpFile
	{
		/// <summary>Description that mstsc.exe uses for DPAPI protected passwords</summary>
		private const String PasswordDescription = "psw";

		/// <summary>Setting names with type prefix and value (e.g. "i:1")</summary>
		private readonly Dictionary<String, String> _values = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);

		/// <summary>Setting names in the order they were added</summary>
		private readonly List<String> _names = new List<String>();

		/// <summary>Read .rdp file from disk</summary>
		/// <param name="filePath">Path to the .rdp file</param>
		/// <returns>Parsed .rdp file settings</returns>
		public static RdpFile Load(String filePath)
		{
			RdpFile result = new RdpFile();
			foreach(String line in File.ReadAllLines(filePath))//BOM detection handles both UTF-16 (mstsc.exe) and UTF-8 files
			{
				Int32 nameIndex = line.IndexOf(':');
				Int32 typeIndex = nameIndex < 0 ? -1 : line.IndexOf(':', nameIndex + 1);
				if(typeIndex < 0)
					continue;

				String name = line.Substring(0, nameIndex).Trim();
				String type = line.Substring(nameIndex + 1, typeIndex - nameIndex - 1).Trim();
				if(name.Length > 0 && type.Length == 1)
					result.Set(name, type, line.Substring(typeIndex + 1));
			}
			return result;
		}

		/// <summary>Write .rdp file to disk in the same encoding as mstsc.exe</summary>
		/// <param name="filePath">Path to the .rdp file</param>
		public void Save(String filePath)
		{
			List<String> lines = new List<String>(this._names.Count);
			foreach(String name in this._names)
				lines.Add(name + ":" + this._values[name]);

			File.WriteAllLines(filePath, lines, Encoding.Unicode);
		}

		/// <summary>Create .rdp file settings from the client settings</summary>
		/// <param name="treeRow">Tree node of the client</param>
		/// <param name="clientRow">Client connection settings</param>
		/// <param name="includePassword">Store the password encrypted for the current Windows user</param>
		/// <returns>.rdp file settings</returns>
		public static RdpFile FromClient(SettingsDataSet.TreeRow treeRow, SettingsDataSet.RdpClientRow clientRow, Boolean includePassword)
		{
			_ = treeRow ?? throw new ArgumentNullException(nameof(treeRow));
			_ = clientRow ?? throw new ArgumentNullException(nameof(clientRow));

			RdpFile result = new RdpFile();
			Boolean isVmConsole = !clientRow.IsConnectionTypeNull() && clientRow.ConnectionTypeI == ConnectionType.VirtualMachineConsoleConnect;

			String server = String.IsNullOrEmpty(clientRow.Server) ? treeRow.Name : clientRow.Server;
			Int32 port = clientRow.ConnectPortI;
			result.SetString("full address", port == RdpClient.DefaultRDPPort ? server : RdpFile.FormatAddress(server, port));
			result.SetInt32("server port", port);

			if(!String.IsNullOrEmpty(clientRow.UserName))
				result.SetString("username", clientRow.UserName);
			if(clientRow.DomainI != null)
				result.SetString("domain", clientRow.DomainI);
			if(includePassword && clientRow.PasswordI != null)
				result.Set("password 51", "b", RdpFile.ProtectPassword(clientRow.PasswordI));

			result.SetInt32("authentication level", (Int32)clientRow.AuthenticationLevelI);
			result.SetBoolean("administrative session", clientRow.ConnectConsole);

			if(isVmConsole)
			{
				result.SetString("pcb", clientRow.IsVirtualMachineIdNull() ? String.Empty : clientRow.VirtualMachineId);
				result.SetInt32("negotiate security layer", 0);
			}

			//Display
			result.SetInt32("session bpp", clientRow.ColorDepthI);
			DesktopSizeParser desktopSize = clientRow.DesktopSizeI;
			if(desktopSize.FullScreen)
				result.SetInt32("screen mode id", 2);
			else
			{
				result.SetInt32("screen mode id", 1);
				if(desktopSize.SameAsClient)
					result.SetBoolean("dynamic resolution", true);
				else
				{
					result.SetInt32("desktopwidth", desktopSize.Size.Width);
					result.SetInt32("desktopheight", desktopSize.Size.Height);
					result.SetBoolean("dynamic resolution", false);
				}
			}

			//Local resources
			RedirectFlags redirect = clientRow.RedirectI;
			result.SetString("drivestoredirect", (redirect & RedirectFlags.Drives) == RedirectFlags.Drives ? "*" : String.Empty);
			result.SetBoolean("redirectcomports", (redirect & RedirectFlags.Ports) == RedirectFlags.Ports);
			result.SetBoolean("redirectprinters", (redirect & RedirectFlags.Printers) == RedirectFlags.Printers);
			result.SetBoolean("redirectsmartcards", (redirect & RedirectFlags.SmartCards) == RedirectFlags.SmartCards);
			result.SetBoolean("redirectclipboard", (redirect & RedirectFlags.Clipboard) == RedirectFlags.Clipboard);
			result.SetBoolean("redirectposdevices", (redirect & RedirectFlags.PointOfService) == RedirectFlags.PointOfService);

			result.SetInt32("audiomode", (Int32)clientRow.RedirectAudioI);
			result.SetInt32("audioqualitymode", (Int32)clientRow.RedirectAudioQualityI);
			result.SetBoolean("audiocapturemode", clientRow.RedirectAudioCapture);
			result.SetInt32("keyboardhook", (Int32)clientRow.KeyboardHookI);

			//Experience
			TS_PERF performance = clientRow.PerformanceFlagsI;
			result.SetBoolean("disable wallpaper", (performance & TS_PERF.DISABLE_WALLPAPER) == TS_PERF.DISABLE_WALLPAPER);
			result.SetBoolean("disable full window drag", (performance & TS_PERF.DISABLE_FULLWINDOWDRAG) == TS_PERF.DISABLE_FULLWINDOWDRAG);
			result.SetBoolean("disable menu anims", (performance & TS_PERF.DISABLE_MENUANIMATIONS) == TS_PERF.DISABLE_MENUANIMATIONS);
			result.SetBoolean("disable themes", (performance & TS_PERF.DISABLE_THEMING) == TS_PERF.DISABLE_THEMING);
			result.SetBoolean("disable cursor setting", (performance & TS_PERF.DISABLE_CURSORSETTINGS) == TS_PERF.DISABLE_CURSORSETTINGS);
			result.SetBoolean("allow font smoothing", (performance & TS_PERF.ENABLE_FONT_SMOOTHING) == TS_PERF.ENABLE_FONT_SMOOTHING);
			result.SetBoolean("allow desktop composition", (performance & TS_PERF.ENABLE_DESKTOP_COMPOSITION) == TS_PERF.ENABLE_DESKTOP_COMPOSITION);

			//Programs
			if(clientRow.RunApplication)
			{
				result.SetString("alternate shell", clientRow.RunApplicationPathI);
				result.SetString("shell working directory", clientRow.RunApplicationWorkingDirI);
			}

			//Gateway
			if(clientRow.GatewayEnabled)
			{
				result.SetString("gatewayhostname", clientRow.GatewayServer);
				result.SetInt32("gatewayusagemethod", clientRow.GatewayBypass ? 2 : 1);
				result.SetInt32("gatewaycredentialssource", (Int32)clientRow.GatewayLogonMethodI);
				result.SetInt32("gatewayprofileusagemethod", 1);
				result.SetBoolean("promptcredentialonce", clientRow.GatewayShareAuth);
			} else
			{
				result.SetInt32("gatewayusagemethod", 0);
				result.SetInt32("gatewayprofileusagemethod", 0);
			}

			return result;
		}

		/// <summary>Copy .rdp file settings to the client settings row</summary>
		/// <param name="clientRow">Client connection settings to fill</param>
		/// <exception cref="InvalidDataException">The file does not contain the server address</exception>
		public void ToClient(SettingsDataSet.RdpClientRow clientRow)
		{
			_ = clientRow ?? throw new ArgumentNullException(nameof(clientRow));

			String address = this.GetString("full address");
			if(String.IsNullOrWhiteSpace(address))
				throw new InvalidDataException("The file does not contain a server address (full address)");

			//Connection type is set before the port, so the default port of the connection type is not stored
			String pcb = this.GetString("pcb");
			clientRow.ConnectionTypeI = String.IsNullOrEmpty(pcb) ? ConnectionType.Normal : ConnectionType.VirtualMachineConsoleConnect;
			if(!String.IsNullOrEmpty(pcb))
				clientRow.VirtualMachineId = pcb;

			RdpFile.ParseAddress(address, out String server, out Int32? port);
			clientRow.Server = server;
			clientRow.ConnectPortI = port ?? this.GetInt32("server port") ?? 0;

			String userName = this.GetString("username") ?? String.Empty;
			String domain = this.GetString("domain");
			Int32 domainIndex = userName.IndexOf('\\');
			if(String.IsNullOrEmpty(domain) && domainIndex > 0)
			{//DOMAIN\user is split to match the client settings dialog
				domain = userName.Substring(0, domainIndex);
				userName = userName.Substring(domainIndex + 1);
			}
			clientRow.UserName = userName;
			clientRow.DomainI = domain;
			clientRow.PasswordI = RdpFile.UnprotectPassword(this.GetString("password 51"));

			Int32? authenticationLevel = this.GetInt32("authentication level");
			clientRow.AuthenticationLevelI = authenticationLevel == 1 || authenticationLevel == 2
				? (AuthenticationLevel)authenticationLevel.Value
				: AuthenticationLevel.NoAuthentication;
			clientRow.ConnectConsole = this.GetBoolean("administrative session") ?? this.GetBoolean("connect to console") ?? false;

			//Display
			clientRow.ColorDepthI = this.GetInt32("session bpp") ?? RdpClient.DefaultColorDepth;
			Int32 width = this.GetInt32("desktopwidth") ?? 0;
			Int32 height = this.GetInt32("desktopheight") ?? 0;
			DesktopSizeParser desktopSize = new DesktopSizeParser();
			if(this.GetInt32("screen mode id") == 2)
				desktopSize.FullScreen = true;
			else if(width > 0 && height > 0 && this.GetBoolean("dynamic resolution") != true)
				desktopSize.Size = new Size(width, height);
			clientRow.DesktopSizeI = desktopSize;

			//Local resources. Missing values use mstsc.exe defaults
			String drives = this.GetString("drivestoredirect");
			RedirectFlags redirect = RedirectFlags.None;
			if(!String.IsNullOrEmpty(drives) || this.GetBoolean("redirectdrives") == true)
				redirect |= RedirectFlags.Drives;
			if(this.GetBoolean("redirectcomports") == true)
				redirect |= RedirectFlags.Ports;
			if(this.GetBoolean("redirectprinters") ?? true)
				redirect |= RedirectFlags.Printers;
			if(this.GetBoolean("redirectsmartcards") == true)
				redirect |= RedirectFlags.SmartCards;
			if(this.GetBoolean("redirectclipboard") ?? true)
				redirect |= RedirectFlags.Clipboard;
			if(this.GetBoolean("redirectposdevices") == true)
				redirect |= RedirectFlags.PointOfService;
			clientRow.RedirectI = redirect;

			Int32 audioMode = this.GetInt32("audiomode") ?? 0;
			clientRow.RedirectAudioI = audioMode >= 0 && audioMode <= 2 ? (RdpClient.AudioRedirectionMode)audioMode : RdpClient.AudioRedirectionMode.Redirect;
			Int32 audioQuality = this.GetInt32("audioqualitymode") ?? 0;
			clientRow.RedirectAudioQualityI = audioQuality >= 0 && audioQuality <= 2 ? (RdpClient.AudioRedirectionQuality)audioQuality : RdpClient.AudioRedirectionQuality.Dynamic;
			clientRow.RedirectAudioCapture = this.GetBoolean("audiocapturemode") ?? false;
			Int32 keyboardHook = this.GetInt32("keyboardhook") ?? 2;
			clientRow.KeyboardHookI = keyboardHook >= 0 && keyboardHook <= 2 ? (RdpClient.KeyboardHookMode)keyboardHook : RdpClient.KeyboardHookMode.FullScreen;

			//Experience
			TS_PERF performance = TS_PERF.DISABLE_NOTHING;
			if(this.GetBoolean("disable wallpaper") == true)
				performance |= TS_PERF.DISABLE_WALLPAPER;
			if(this.GetBoolean("disable full window drag") == true)
				performance |= TS_PERF.DISABLE_FULLWINDOWDRAG;
			if(this.GetBoolean("disable menu anims") == true)
				performance |= TS_PERF.DISABLE_MENUANIMATIONS;
			if(this.GetBoolean("disable themes") == true)
				performance |= TS_PERF.DISABLE_THEMING;
			if(this.GetBoolean("disable cursor setting") == true)
				performance |= TS_PERF.DISABLE_CURSORSETTINGS;
			if(this.GetBoolean("allow font smoothing") == true)
				performance |= TS_PERF.ENABLE_FONT_SMOOTHING;
			if(this.GetBoolean("allow desktop composition") == true)
				performance |= TS_PERF.ENABLE_DESKTOP_COMPOSITION;
			clientRow.PerformanceFlagsI = performance;

			//Programs
			String shell = this.GetString("alternate shell");
			clientRow.RunApplication = !String.IsNullOrEmpty(shell);
			clientRow.RunApplicationPathI = shell;
			clientRow.RunApplicationWorkingDirI = this.GetString("shell working directory");

			//Gateway
			String gatewayServer = this.GetString("gatewayhostname");
			Int32 gatewayUsage = this.GetInt32("gatewayusagemethod") ?? 0;
			clientRow.GatewayEnabled = !String.IsNullOrEmpty(gatewayServer) && gatewayUsage != 0 && gatewayUsage != 4;
			clientRow.GatewayServer = gatewayServer ?? String.Empty;
			clientRow.GatewayBypass = gatewayUsage == 2;
			clientRow.GatewayShareAuth = this.GetBoolean("promptcredentialonce") ?? false;
			Int32 gatewayLogon = this.GetInt32("gatewaycredentialssource") ?? 4;
			clientRow.GatewayLogonMethodI = gatewayLogon == 0 || gatewayLogon == 1 ? (RdpClient.GatewayLogonMethod)gatewayLogon : RdpClient.GatewayLogonMethod.Any;
		}

		/// <summary>Get string setting value</summary>
		/// <param name="name">Setting name</param>
		/// <returns>Setting value or null if setting not found</returns>
		public String GetString(String name)
			=> this._values.TryGetValue(name, out String value) ? value.Substring(2) : null;

		/// <summary>Get integer setting value</summary>
		/// <param name="name">Setting name</param>
		/// <returns>Setting value or null if setting not found or value is not a number</returns>
		public Int32? GetInt32(String name)
			=> Int32.TryParse(this.GetString(name), NumberStyles.Integer, CultureInfo.InvariantCulture, out Int32 value) ? value : (Int32?)null;

		/// <summary>Get boolean setting value stored as 0 or 1</summary>
		/// <param name="name">Setting name</param>
		/// <returns>Setting value or null if setting not found</returns>
		public Boolean? GetBoolean(String name)
		{
			Int32? value = this.GetInt32(name);
			return value == null ? (Boolean?)null : value.Value != 0;
		}

		public void SetString(String name, String value)
			=> this.Set(name, "s", value ?? String.Empty);

		public void SetInt32(String name, Int32 value)
			=> this.Set(name, "i", value.ToString(CultureInfo.InvariantCulture));

		public void SetBoolean(String name, Boolean value)
			=> this.SetInt32(name, value ? 1 : 0);

		private void Set(String name, String type, String value)
		{
			if(!this._values.ContainsKey(name))
				this._names.Add(name);
			this._values[name] = type + ":" + value;
		}

		/// <summary>Format the server address with non-default port</summary>
		private static String FormatAddress(String server, Int32 port)
			=> server.IndexOf(':') >= 0 && server[0] != '['
				? String.Format(CultureInfo.InvariantCulture, "[{0}]:{1}", server, port)//IPv6
				: String.Format(CultureInfo.InvariantCulture, "{0}:{1}", server, port);

		/// <summary>Split the server address to host and port</summary>
		/// <param name="address">Address in formats: host, host:port, [IPv6]:port or IPv6</param>
		private static void ParseAddress(String address, out String server, out Int32? port)
		{
			address = address.Trim();
			Int32 portIndex;
			if(address.Length > 0 && address[0] == '[')
			{
				Int32 closeIndex = address.IndexOf(']');
				server = closeIndex < 0 ? address.Trim('[') : address.Substring(1, closeIndex - 1);
				portIndex = closeIndex < 0 ? -1 : address.IndexOf(':', closeIndex);
			} else
			{
				portIndex = address.IndexOf(':');
				if(portIndex != address.LastIndexOf(':'))
					portIndex = -1;//IPv6 address without port
				server = portIndex < 0 ? address : address.Substring(0, portIndex);
			}

			port = portIndex >= 0 && Int32.TryParse(address.Substring(portIndex + 1), NumberStyles.Integer, CultureInfo.InvariantCulture, out Int32 value)
				? value
				: (Int32?)null;
		}

		/// <summary>Encrypt the password the same way as mstsc.exe (DPAPI, current user)</summary>
		/// <param name="password">Plain text password</param>
		/// <returns>Hex encoded encrypted password</returns>
		private static String ProtectPassword(String password)
		{
			Byte[] encrypted = RdpFile.InvokeDpapi(Encoding.Unicode.GetBytes(password), true)
				?? throw new InvalidOperationException("Failed to encrypt the password. Error: " + Marshal.GetLastWin32Error());

			StringBuilder result = new StringBuilder(encrypted.Length * 2);
			foreach(Byte b in encrypted)
				result.Append(b.ToString("X2", CultureInfo.InvariantCulture));
			return result.ToString();
		}

		/// <summary>Decrypt the password encrypted by mstsc.exe</summary>
		/// <param name="hex">Hex encoded encrypted password</param>
		/// <returns>Plain text password or null if it was encrypted by another user or on another computer</returns>
		private static String UnprotectPassword(String hex)
		{
			if(String.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
				return null;

			Byte[] encrypted = new Byte[hex.Length / 2];
			for(Int32 loop = 0; loop < encrypted.Length; loop++)
				if(!Byte.TryParse(hex.Substring(loop * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out encrypted[loop]))
					return null;

			Byte[] decrypted = RdpFile.InvokeDpapi(encrypted, false);
			return decrypted == null ? null : Encoding.Unicode.GetString(decrypted).TrimEnd('\0');
		}

		private static Byte[] InvokeDpapi(Byte[] data, Boolean protect)
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			Native.DataBlob output = new Native.DataBlob();
			try
			{
				Native.DataBlob input = new Native.DataBlob() { cbData = data.Length, pbData = handle.AddrOfPinnedObject(), };
				Boolean success = protect
					? Native.CryptProtectData(ref input, PasswordDescription, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Native.CRYPTPROTECT_UI_FORBIDDEN, ref output)
					: Native.CryptUnprotectData(ref input, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Native.CRYPTPROTECT_UI_FORBIDDEN, ref output);
				if(!success)
					return null;

				Byte[] result = new Byte[output.cbData];
				Marshal.Copy(output.pbData, result, 0, output.cbData);
				return result;
			} finally
			{
				handle.Free();
				if(output.pbData != IntPtr.Zero)
					Native.LocalFree(output.pbData);
			}
		}
	}
}