namespace Plugin.RDP.UI
{
	partial class RemoteSessionsDlg
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
			this.bgQuerySessions = new System.ComponentModel.BackgroundWorker();
			this.lvSessions = new Plugin.RDP.UI.TypedListView();
			this.cmsSession = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDisconnect = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiLogoff = new System.Windows.Forms.ToolStripMenuItem();
			this.bgLogoffSession = new System.ComponentModel.BackgroundWorker();
			this.bgDisconnectSession = new System.ComponentModel.BackgroundWorker();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.cmsSession.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// bgQuerySessions
			// 
			this.bgQuerySessions.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgQuerySessions_DoWork);
			this.bgQuerySessions.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgQuerySessions_RunWorkerCompleted);
			// 
			// lvSessions
			// 
			this.lvSessions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvSessions.ContextMenuStrip = this.cmsSession;
			this.lvSessions.FullRowSelect = true;
			this.lvSessions.Location = new System.Drawing.Point(13, 13);
			this.lvSessions.Name = "lvSessions";
			this.lvSessions.Size = new System.Drawing.Size(267, 228);
			this.lvSessions.TabIndex = 1;
			this.lvSessions.UseCompatibleStateImageBehavior = false;
			this.lvSessions.View = System.Windows.Forms.View.Details;
			this.lvSessions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSessions_KeyDown);
			// 
			// cmsSession
			// 
			this.cmsSession.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRefresh,
            this.tsmiDisconnect,
            this.tsmiLogoff});
			this.cmsSession.Name = "cmsSession";
			this.cmsSession.Size = new System.Drawing.Size(138, 70);
			this.cmsSession.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSession_ItemClicked);
			this.cmsSession.Opening += new System.ComponentModel.CancelEventHandler(this.cmsSession_Opening);
			// 
			// tsmiRefresh
			// 
			this.tsmiRefresh.Name = "tsmiRefresh";
			this.tsmiRefresh.Size = new System.Drawing.Size(137, 22);
			this.tsmiRefresh.Text = "&Refresh";
			// 
			// tsmiDisconnect
			// 
			this.tsmiDisconnect.Name = "tsmiDisconnect";
			this.tsmiDisconnect.Size = new System.Drawing.Size(137, 22);
			this.tsmiDisconnect.Text = "&Disconnect";
			// 
			// tsmiLogoff
			// 
			this.tsmiLogoff.Name = "tsmiLogoff";
			this.tsmiLogoff.Size = new System.Drawing.Size(137, 22);
			this.tsmiLogoff.Text = "&Logoff";
			// 
			// bgLogoffSession
			// 
			this.bgLogoffSession.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgLogoffSession_DoWork);
			this.bgLogoffSession.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgLogoffSession_RunWorkerCompleted);
			// 
			// bgDisconnectSession
			// 
			this.bgDisconnectSession.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgDisconnectSession_DoWork);
			this.bgDisconnectSession.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgDisconnectSession_RunWorkerCompleted);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 244);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(292, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// tsslStatus
			// 
			this.tsslStatus.Name = "tsslStatus";
			this.tsslStatus.Size = new System.Drawing.Size(38, 17);
			this.tsslStatus.Text = "Ready";
			// 
			// RemoteSessionsDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.lvSessions);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RemoteSessionsDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Remote sessions";
			this.cmsSession.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TypedListView lvSessions;
		private System.ComponentModel.BackgroundWorker bgQuerySessions;
		private System.Windows.Forms.ContextMenuStrip cmsSession;
		private System.Windows.Forms.ToolStripMenuItem tsmiRefresh;
		private System.Windows.Forms.ToolStripMenuItem tsmiDisconnect;
		private System.Windows.Forms.ToolStripMenuItem tsmiLogoff;
		private System.ComponentModel.BackgroundWorker bgLogoffSession;
		private System.ComponentModel.BackgroundWorker bgDisconnectSession;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
	}
}