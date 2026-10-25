using System;
using Plugin.RDP.Bll;

namespace Plugin.RDP.UI
{
	/// <summary>Additional settings for the remote desktop size mode, shown inside the "Remote Desktop Size" group when the mode is selected</summary>
	/// <remarks>Implementation must be a <see cref="System.Windows.Forms.Control"/></remarks>
	internal interface IDesktopSizeOptions
	{
		/// <summary>The user changed the options</summary>
		event EventHandler Changed;

		/// <summary>Fill the options controls from the stored value</summary>
		/// <param name="value">Stored desktop size settings which belongs to this mode</param>
		void LoadFrom(DesktopSizeParser value);

		/// <summary>Write the options to the value created for the selected mode</summary>
		/// <param name="value">Desktop size settings which will be stored</param>
		void ApplyTo(DesktopSizeParser value);
	}
}