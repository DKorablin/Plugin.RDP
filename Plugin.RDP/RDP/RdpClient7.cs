using System;
using AxMSTSCLib;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plugin.RDP.RDP
{
	internal class RdpClient7 : AxMsRdpClient7NotSafeForScripting
	{
		private readonly RdpClient _parent;

		public RdpClient7(RdpClient parent, UserControl form)
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

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == 33 && !base.ContainsFocus)
				base.Focus();
			base.WndProc(ref m);
		}
	}
}