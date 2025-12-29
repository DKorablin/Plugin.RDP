namespace Plugin.RDP
{
	partial class PanelRdpClient
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelRdpClient));
			this.gridSearch = new AlphaOmega.Windows.Forms.SearchGrid();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsbnAdd = new System.Windows.Forms.ToolStripSplitButton();
			this.tsmiAddClient = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAddNode = new System.Windows.Forms.ToolStripMenuItem();
			this.tsbnDelete = new System.Windows.Forms.ToolStripButton();
			this.tvList = new Plugin.RDP.UI.RdpClientTreeView();
			this.cmsClient = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiClientConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiClientFocus = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiClientDisconnect = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiClientLogOff = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiClientListSessions = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiClientProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.ilList = new System.Windows.Forms.ImageList(this.components);
			this.bgList = new System.ComponentModel.BackgroundWorker();
			this.cmsNode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiNodeAddClient = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiNodeAddNode = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMain.SuspendLayout();
			this.cmsClient.SuspendLayout();
			this.cmsNode.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridSearch
			// 
			this.gridSearch.DataGrid = null;
			this.gridSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.gridSearch.EnableFindCase = true;
			this.gridSearch.EnableFindHighlight = true;
			this.gridSearch.EnableFindPrevNext = true;
			this.gridSearch.EnableSearchHighlight = false;
			this.gridSearch.ListView = null;
			this.gridSearch.Location = new System.Drawing.Point(3, 155);
			this.gridSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.gridSearch.Name = "gridSearch";
			this.gridSearch.Size = new System.Drawing.Size(440, 29);
			this.gridSearch.TabIndex = 1;
			this.gridSearch.TreeView = null;
			this.gridSearch.Visible = false;
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnAdd,
            this.tsbnDelete});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(150, 25);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStrip1";
			// 
			// tsbnAdd
			// 
			this.tsbnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddClient,
            this.tsmiAddNode});
			this.tsbnAdd.Image = global::Plugin.RDP.Properties.Resources.iconClientNew;
			this.tsbnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnAdd.Name = "tsbnAdd";
			this.tsbnAdd.Size = new System.Drawing.Size(32, 22);
			this.tsbnAdd.ToolTipText = "Add Terminal Client";
			this.tsbnAdd.ButtonClick += new System.EventHandler(this.tsbnAdd_Click);
			this.tsbnAdd.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsbnAdd_DropDownItemClicked);
			// 
			// tsmiAddClient
			// 
			this.tsmiAddClient.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tsmiAddClient.Image = global::Plugin.RDP.Properties.Resources.iconClientNew;
			this.tsmiAddClient.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsmiAddClient.Name = "tsmiAddClient";
			this.tsmiAddClient.Size = new System.Drawing.Size(131, 22);
			this.tsmiAddClient.Text = "Add &Client";
			// 
			// tsmiAddNode
			// 
			this.tsmiAddNode.Image = global::Plugin.RDP.Properties.Resources.iconFolderNew;
			this.tsmiAddNode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsmiAddNode.Name = "tsmiAddNode";
			this.tsmiAddNode.Size = new System.Drawing.Size(131, 22);
			this.tsmiAddNode.Text = "Add &Node";
			// 
			// tsbnDelete
			// 
			this.tsbnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnDelete.Enabled = false;
			this.tsbnDelete.Image = global::Plugin.RDP.Properties.Resources.iconDelete;
			this.tsbnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnDelete.Name = "tsbnDelete";
			this.tsbnDelete.Size = new System.Drawing.Size(23, 22);
			this.tsbnDelete.ToolTipText = "Remove selected node";
			this.tsbnDelete.Click += new System.EventHandler(this.tsbnDelete_Click);
			// 
			// tvList
			// 
			this.tvList.AllowDrop = true;
			this.tvList.ContextMenuStrip = this.cmsClient;
			this.tvList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvList.ImageIndex = 0;
			this.tvList.ImageList = this.ilList;
			this.tvList.LabelEdit = true;
			this.tvList.Location = new System.Drawing.Point(0, 25);
			this.tvList.Name = "tvList";
			this.tvList.SelectedImageIndex = 0;
			this.tvList.Size = new System.Drawing.Size(150, 125);
			this.tvList.TabIndex = 1;
			this.tvList.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvList_AfterLabelEdit);
			this.tvList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvList_AfterSelect);
			this.tvList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvList_KeyDown);
			this.tvList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvList_MouseClick);
			this.tvList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvList_MouseDoubleClick);
			// 
			// cmsClient
			// 
			this.cmsClient.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClientConnect,
            this.tsmiClientFocus,
            this.tsmiClientDisconnect,
            this.tsmiClientLogOff,
            this.tsmiClientListSessions,
            this.tsmiClientProperties});
			this.cmsClient.Name = "cmsClient";
			this.cmsClient.Size = new System.Drawing.Size(139, 136);
			this.cmsClient.Opening += new System.ComponentModel.CancelEventHandler(this.cmsClient_Opening);
			this.cmsClient.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsClient_ItemClicked);
			// 
			// tsmiClientConnect
			// 
			this.tsmiClientConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsmiClientConnect.Name = "tsmiClientConnect";
			this.tsmiClientConnect.Size = new System.Drawing.Size(138, 22);
			this.tsmiClientConnect.Text = "&Connect";
			// 
			// tsmiClientFocus
			// 
			this.tsmiClientFocus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tsmiClientFocus.Name = "tsmiClientFocus";
			this.tsmiClientFocus.Size = new System.Drawing.Size(138, 22);
			this.tsmiClientFocus.Text = "&Focus";
			// 
			// tsmiClientDisconnect
			// 
			this.tsmiClientDisconnect.Name = "tsmiClientDisconnect";
			this.tsmiClientDisconnect.Size = new System.Drawing.Size(138, 22);
			this.tsmiClientDisconnect.Text = "&Disconnect";
			// 
			// tsmiClientLogOff
			// 
			this.tsmiClientLogOff.Name = "tsmiClientLogOff";
			this.tsmiClientLogOff.Size = new System.Drawing.Size(138, 22);
			this.tsmiClientLogOff.Text = "Log Off";
			// 
			// tsmiClientListSessions
			// 
			this.tsmiClientListSessions.Name = "tsmiClientListSessions";
			this.tsmiClientListSessions.Size = new System.Drawing.Size(138, 22);
			this.tsmiClientListSessions.Text = "&List sessions";
			// 
			// tsmiClientProperties
			// 
			this.tsmiClientProperties.Name = "tsmiClientProperties";
			this.tsmiClientProperties.Size = new System.Drawing.Size(138, 22);
			this.tsmiClientProperties.Text = "&Properties";
			// 
			// ilList
			// 
			this.ilList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilList.ImageStream")));
			this.ilList.TransparentColor = System.Drawing.Color.Magenta;
			this.ilList.Images.SetKeyName(0, "iconFolder.bmp");
			this.ilList.Images.SetKeyName(1, "iconClientDisconnected.bmp");
			this.ilList.Images.SetKeyName(2, "iconClientConnected.bmp");
			// 
			// bgList
			// 
			this.bgList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgList_DoWork);
			this.bgList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgList_RunWorkerCompleted);
			// 
			// cmsNode
			// 
			this.cmsNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNodeAddNode,
			this.tsmiNodeAddClient});
			this.cmsNode.Name = "cmsNode";
			this.cmsNode.Size = new System.Drawing.Size(153, 48);
			this.cmsNode.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsNode_ItemClicked);
			// 
			// tsmiNodeAddClient
			// 
			this.tsmiNodeAddClient.Name = "tsmiNodeAddClient";
			this.tsmiNodeAddClient.Size = new System.Drawing.Size(152, 22);
			this.tsmiNodeAddClient.Text = "Add &Client";
			// 
			// tsmiNodeAddNode
			// 
			this.tsmiNodeAddNode.Name = "tsmiNodeAddNode";
			this.tsmiNodeAddNode.Size = new System.Drawing.Size(152, 22);
			this.tsmiNodeAddNode.Text = "Add &Node";
			// 
			// PanelRdpClient
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridSearch);
			this.Controls.Add(this.tvList);
			this.Controls.Add(this.tsMain);
			this.Name = "PanelRdpClient";
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.cmsClient.ResumeLayout(false);
			this.cmsNode.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private Plugin.RDP.UI.RdpClientTreeView tvList;
		private System.Windows.Forms.ToolStripSplitButton tsbnAdd;
		private System.Windows.Forms.ToolStripMenuItem tsmiAddClient;
		private System.Windows.Forms.ToolStripMenuItem tsmiAddNode;
		private System.Windows.Forms.ToolStripButton tsbnDelete;
		private System.Windows.Forms.ImageList ilList;
		private System.Windows.Forms.ContextMenuStrip cmsClient;
		private System.Windows.Forms.ToolStripMenuItem tsmiClientConnect;
		private System.Windows.Forms.ToolStripMenuItem tsmiClientProperties;
		private System.Windows.Forms.ToolStripMenuItem tsmiClientListSessions;
		private System.Windows.Forms.ToolStripMenuItem tsmiClientLogOff;
		private System.ComponentModel.BackgroundWorker bgList;
		private AlphaOmega.Windows.Forms.SearchGrid gridSearch;
		private System.Windows.Forms.ContextMenuStrip cmsNode;
		private System.Windows.Forms.ToolStripMenuItem tsmiNodeAddClient;
		private System.Windows.Forms.ToolStripMenuItem tsmiNodeAddNode;
		private System.Windows.Forms.ToolStripMenuItem tsmiClientFocus;
		private System.Windows.Forms.ToolStripMenuItem tsmiClientDisconnect;
	}
}
