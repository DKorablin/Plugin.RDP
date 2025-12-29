namespace Plugin.RDP.UI
{
	partial class DesktopSizeDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Button bnOk;
			System.Windows.Forms.Button bnCancel;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.error = new System.Windows.Forms.ErrorProvider(this.components);
			bnOk = new System.Windows.Forms.Button();
			bnCancel = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.error)).BeginInit();
			this.SuspendLayout();
			// 
			// bnOk
			// 
			bnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			bnOk.Location = new System.Drawing.Point(14, 72);
			bnOk.Name = "bnOk";
			bnOk.Size = new System.Drawing.Size(75, 23);
			bnOk.TabIndex = 0;
			bnOk.Text = "&OK";
			bnOk.UseVisualStyleBackColor = true;
			// 
			// bnCancel
			// 
			bnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			bnCancel.Location = new System.Drawing.Point(95, 72);
			bnCancel.Name = "bnCancel";
			bnCancel.Size = new System.Drawing.Size(75, 23);
			bnCancel.TabIndex = 1;
			bnCancel.Text = "&Cancel";
			bnCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(14, 13);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(38, 13);
			label1.TabIndex = 2;
			label1.Text = "&Width:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(14, 39);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(41, 13);
			label2.TabIndex = 4;
			label2.Text = "&Heigth:";
			// 
			// txtWidth
			// 
			this.txtWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.error.SetIconAlignment(this.txtWidth, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtWidth.Location = new System.Drawing.Point(58, 10);
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.Size = new System.Drawing.Size(112, 20);
			this.txtWidth.TabIndex = 3;
			// 
			// txtHeight
			// 
			this.txtHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.error.SetIconAlignment(this.txtHeight, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtHeight.Location = new System.Drawing.Point(58, 36);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(112, 20);
			this.txtHeight.TabIndex = 5;
			// 
			// error
			// 
			this.error.ContainerControl = this;
			// 
			// DesktopSizeDlg
			// 
			this.AcceptButton = bnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = bnCancel;
			this.ClientSize = new System.Drawing.Size(182, 107);
			this.Controls.Add(this.txtHeight);
			this.Controls.Add(label2);
			this.Controls.Add(this.txtWidth);
			this.Controls.Add(label1);
			this.Controls.Add(bnCancel);
			this.Controls.Add(bnOk);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(640, 145);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(198, 145);
			this.Name = "DesktopSizeDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Custom size";
			((System.ComponentModel.ISupportInitialize)(this.error)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtWidth;
		private System.Windows.Forms.TextBox txtHeight;
		private System.Windows.Forms.ErrorProvider error;

	}
}