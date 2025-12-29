using System;

namespace Plugin.RDP
{
	/// <summary>Local resource redirection flags</summary>
	[Flags]
	public enum RedirectFlags
	{
		/// <summary>No redirection</summary>
		None = 0x0,
		/// <summary>Redirect local drives</summary>
		Drives = 0x01,
		/// <summary>Forward ports</summary>
		Ports = 0x02,
		/// <summary>Forward printers</summary>
		Printers = 0x04,
		/// <summary>Forward smart cards</summary>
		SmartCards = 0x08,
		/// <summary>Forward the clipboard</summary>
		Clipboard = 0x10,
		/// <summary>Forward Point of Service devices</summary>
		PointOfService = 0x20,
	}
}