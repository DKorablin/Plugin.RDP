using System;
using System.Drawing;
using System.Windows.Forms;
using Plugin.RDP.Bll;

namespace Plugin.RDP.UI
{
	/// <summary>Width and height of the custom remote desktop size</summary>
	internal class DesktopSizeCustomOptions : UserControl, IDesktopSizeOptions
	{
		private const Int32 MinDesktopSize = 200;
		private const Int32 MaxDesktopSize = 8192;

		private readonly NumericUpDown udWidth;
		private readonly NumericUpDown udHeight;

		public event EventHandler Changed;

		public Size CustomSize
		{
			get => new Size((Int32)udWidth.Value, (Int32)udHeight.Value);
			set
			{
				udWidth.Value = Math.Max(MinDesktopSize, Math.Min(MaxDesktopSize, value.Width));
				udHeight.Value = Math.Max(MinDesktopSize, Math.Min(MaxDesktopSize, value.Height));
			}
		}

		public DesktopSizeCustomOptions()
		{
			this.udWidth = CreateSizeInput("udWidth");
			this.udHeight = CreateSizeInput("udHeight");
			this.udWidth.ValueChanged += this.udSize_ValueChanged;
			this.udHeight.ValueChanged += this.udSize_ValueChanged;

			FlowLayoutPanel flow = new FlowLayoutPanel()
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				Dock = DockStyle.Fill,
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = true,
			};
			flow.Controls.Add(CreateLabel("&Width:"));
			flow.Controls.Add(this.udWidth);
			flow.Controls.Add(CreateLabel("&Height:"));
			flow.Controls.Add(this.udHeight);

			base.AutoSize = true;
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			base.Controls.Add(flow);

			this.CustomSize = Screen.PrimaryScreen.Bounds.Size;
		}

		public void LoadFrom(DesktopSizeParser value)
		{
			if(!value.FullScreen && !value.SameAsClient)
				this.CustomSize = value.Size;
		}

		public void ApplyTo(DesktopSizeParser value)
			=> value.Size = this.CustomSize;

		private void udSize_ValueChanged(Object sender, EventArgs e)
			=> this.Changed?.Invoke(this, EventArgs.Empty);

		private static Label CreateLabel(String text)
			=> new Label()
			{
				AutoSize = true,
				Anchor = AnchorStyles.Left,//Vertically centered inside the FlowLayoutPanel row
				Text = text,
			};

		private static NumericUpDown CreateSizeInput(String name)
			=> new NumericUpDown()
			{
				Name = name,
				Minimum = MinDesktopSize,
				Maximum = MaxDesktopSize,
				Width = 70,
			};
	}
}