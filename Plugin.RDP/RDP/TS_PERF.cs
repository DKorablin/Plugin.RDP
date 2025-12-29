using System;

namespace Plugin.RDP
{
	/// <summary>Specifies a set of features that can be set at the server to improve performance.</summary>
	[Flags]
	public enum TS_PERF : UInt32
	{
		/// <summary>No features are disabled.</summary>
		DISABLE_NOTHING = 0x00000000,
		/// <summary>Wallpaper on the desktop is not displayed.</summary>
		DISABLE_WALLPAPER = 0x00000001,
		/// <summary>Full-window drag is disabled; only the window outline is displayed when the window is moved.</summary>
		DISABLE_FULLWINDOWDRAG = 0x00000002,
		/// <summary>Menu animations are disabled.</summary>
		DISABLE_MENUANIMATIONS = 0x00000004,
		/// <summary>Themes are disabled.</summary>
		DISABLE_THEMING = 0x00000008,
		/// <summary>Enable enhanced graphics. </summary>
		ENABLE_ENHANCED_GRAPHICS = 0x00000010,
		/// <summary>No shadow is displayed for the cursor.</summary>
		DISABLE_CURSOR_SHADOW = 0x00000020,
		/// <summary>Cursor blinking is disabled.</summary>
		DISABLE_CURSORSETTINGS = 0x00000040,
		/// <summary>Enable font smoothing.</summary>
		ENABLE_FONT_SMOOTHING = 0x00000080,
		/// <summary>Enable desktop composition.</summary>
		ENABLE_DESKTOP_COMPOSITION = 0x00000100,
		/// <summary>Set internally for clients not aware of this setting.</summary>
		DEFAULT_NONPERFCLIENT_SETTING = 0x40000000,
		/// <summary>Reserved and used internally by the client. </summary>
		RESERVED1 = 0x80000000,
	}
}