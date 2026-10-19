using System;

namespace Plugin.RDP
{
	internal static class Constant
	{
		/// <summary>Name of the XML file with remote desktop connection settings</summary>
		public const String SettingsFileName = "RdpClient.xml";

		/// <summary>File dialog filter for Remote Desktop Connection files</summary>
		public const String RdpFileFilter = "Remote Desktop Connection (*.rdp)|*.rdp|All files (*.*)|*.*";
	}
}