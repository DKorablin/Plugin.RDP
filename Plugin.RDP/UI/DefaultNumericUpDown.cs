using System;
using System.Windows.Forms;
using System.Drawing;

namespace Plugin.RDP.UI
{
	internal class DefaultNumericUpDown : NumericUpDown
	{
		private static readonly Color DefaultColor = Color.Gray;
		private Decimal _defaultValue;

		public Decimal DefaultValue
		{
			get => this._defaultValue;
			set => this.Value = this._defaultValue = value;
		}

		public new Decimal Value
		{
			get => base.Value;
			set
			{
				base.Value = value;
				this.ToggleColor();
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			this.SetDefaultColor();
			base.OnLostFocus(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.ForeColor = Color.Empty;
			base.OnGotFocus(e);
		}

		private void SetDefaultColor()
			=> base.ForeColor = base.Value == this.DefaultValue
				? DefaultNumericUpDown.DefaultColor
				: Color.Empty;

		private void ToggleColor()
		{
			if(!base.Focused)
				this.SetDefaultColor();
		}
	}
}