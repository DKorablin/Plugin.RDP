using System;
using System.ComponentModel;
using System.Windows.Forms;
using AxMSTSCLib;

namespace Plugin.RDP.RDP
{
	internal class RdpClient8 : AxMsRdpClient7NotSafeForScripting
	{
		private readonly RdpClient _parent;

		public RdpClient8(RdpClient parent, UserControl form)
		{
			this._parent = parent;
			((ISupportInitialize)this).BeginInit();
			base.Hide();
			this.Dock = DockStyle.Fill;
			this.Enabled = true;
			this.Name = "rdpClient";
			form.Controls.Add(this);
			((ISupportInitialize)this).EndInit();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			this._parent.GotFocus();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			this._parent.LostFocus();
			base.OnLostFocus(e);
		}
	}
}