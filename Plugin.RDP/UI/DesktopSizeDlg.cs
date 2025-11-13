using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plugin.RDP.UI
{
	internal partial class DesktopSizeDlg : Form
	{
		public String CustomSize
		{
			get => $"{txtWidth.Text}x{txtHeight.Text}";
			set
			{
				if(!String.IsNullOrEmpty(value))
				{
					String[] wh = value.Split('x');
					txtWidth.Text = wh[0];
					txtHeight.Text = wh[1];
				}
			}
		}

		public DesktopSizeDlg(Object customSize)
		{
			this.InitializeComponent();
			if(customSize != null)
				this.CustomSize = customSize.ToString();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(base.DialogResult == DialogResult.OK)
			{
				Boolean cancel = false;
				if(!Int32.TryParse(txtWidth.Text, out _))
				{
					error.SetError(txtWidth, "Invalid width");
					cancel = true;
				}
				if(!Int32.TryParse(txtHeight.Text, out _))
				{
					error.SetError(txtHeight, "Invalid height");
					cancel = true;
				}
				e.Cancel = cancel;
			}
			base.OnClosing(e);
		}
	}
}