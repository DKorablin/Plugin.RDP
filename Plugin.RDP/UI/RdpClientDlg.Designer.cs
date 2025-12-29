namespace Plugin.RDP.UI
{
	partial class RdpClientDlg
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.TabPage tabGeneral;
			System.Windows.Forms.TabPage tabConnecion;
			System.Windows.Forms.Label label11;
			System.Windows.Forms.Label label10;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.TabPage tabLocal;
			System.Windows.Forms.Label label13;
			System.Windows.Forms.Label label12;
			System.Windows.Forms.Label label7;
			System.Windows.Forms.GroupBox gbRedirect;
			System.Windows.Forms.FlowLayoutPanel flowRedirect;
			System.Windows.Forms.TabPage tabApplication;
			System.Windows.Forms.Label label9;
			System.Windows.Forms.Label label8;
			System.Windows.Forms.TabPage tabDisplay;
			System.Windows.Forms.GroupBox gbDesktopSize;
			System.Windows.Forms.RadioButton rbSize800x600;
			System.Windows.Forms.RadioButton rbSize1024x768;
			System.Windows.Forms.RadioButton rbSize1280x800;
			System.Windows.Forms.RadioButton rbSize1280x1024;
			System.Windows.Forms.RadioButton rbSize1440x900;
			System.Windows.Forms.RadioButton rbSize1600x1200;
			System.Windows.Forms.RadioButton rbSize1680x1050;
			System.Windows.Forms.RadioButton rbSize1920x1200;
			System.Windows.Forms.RadioButton rbSizeSame;
			System.Windows.Forms.RadioButton rbSizeFull;
			System.Windows.Forms.Label label14;
			System.Windows.Forms.Label label15;
			System.Windows.Forms.Label label16;
			System.Windows.Forms.Label label17;
			System.Windows.Forms.Label label18;
			System.Windows.Forms.Label label19;
			System.Windows.Forms.TabPage tabGateway;
			System.Windows.Forms.TabPage tabPerfomance;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdpClientDlg));
			this.txtDomain = new AlphaOmega.Windows.Forms.DefaultTextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUser = new AlphaOmega.Windows.Forms.DefaultTextBox();
			this.ddlComputer = new System.Windows.Forms.ComboBox();
			this.txtVmConsoleId = new System.Windows.Forms.TextBox();
			this.cbVmConsole = new System.Windows.Forms.CheckBox();
			this.udMinutesToIdle = new System.Windows.Forms.NumericUpDown();
			this.ddlAuthenticationLevel = new System.Windows.Forms.ComboBox();
			this.udPort = new Plugin.RDP.UI.DefaultNumericUpDown();
			this.cbConsole = new System.Windows.Forms.CheckBox();
			this.ddlKeyboardHook = new System.Windows.Forms.ComboBox();
			this.ddlRedirectAudioQuality = new System.Windows.Forms.ComboBox();
			this.cbRedirectAudioCapture = new System.Windows.Forms.CheckBox();
			this.ddlRedirectAudio = new System.Windows.Forms.ComboBox();
			this.cbRedirectDrives = new System.Windows.Forms.CheckBox();
			this.cbRedirectPrinters = new System.Windows.Forms.CheckBox();
			this.cbRedirectClipboard = new System.Windows.Forms.CheckBox();
			this.cbRedirectPorts = new System.Windows.Forms.CheckBox();
			this.cbRedirectSmartCards = new System.Windows.Forms.CheckBox();
			this.cbRedirectPoS = new System.Windows.Forms.CheckBox();
			this.cbRunApplication = new System.Windows.Forms.CheckBox();
			this.gbRunApplication = new System.Windows.Forms.GroupBox();
			this.cbRunApplicationMaximize = new System.Windows.Forms.CheckBox();
			this.txtRunApplicationWorkingDir = new System.Windows.Forms.TextBox();
			this.txtRunApplicationFilePath = new System.Windows.Forms.TextBox();
			this.flowDesktopSize = new System.Windows.Forms.FlowLayoutPanel();
			this.rbSizeCustom = new System.Windows.Forms.RadioButton();
			this.ddlColorDepth = new System.Windows.Forms.ComboBox();
			this.cbUseGateway = new System.Windows.Forms.CheckBox();
			this.gbGateway = new System.Windows.Forms.GroupBox();
			this.ddlGatewayLogonMethod = new System.Windows.Forms.ComboBox();
			this.txtGatewayDomain = new System.Windows.Forms.TextBox();
			this.txtGatewayPassword = new System.Windows.Forms.TextBox();
			this.txtGatewayUserName = new System.Windows.Forms.TextBox();
			this.cbGatewayShareAuth = new System.Windows.Forms.CheckBox();
			this.cbGatewayBypass = new System.Windows.Forms.CheckBox();
			this.txtGatewayServer = new System.Windows.Forms.TextBox();
			this.cbDisableThemeing = new System.Windows.Forms.CheckBox();
			this.cbDisableMenuAnimations = new System.Windows.Forms.CheckBox();
			this.cbDisableFullWindowDrag = new System.Windows.Forms.CheckBox();
			this.cbEnableDesktopComposition = new System.Windows.Forms.CheckBox();
			this.cbEnableFontSmoothing = new System.Windows.Forms.CheckBox();
			this.cbDisableWallpaper = new System.Windows.Forms.CheckBox();
			this.ddlConnectionSpeed = new System.Windows.Forms.ComboBox();
			this.label20 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.error = new System.Windows.Forms.ErrorProvider(this.components);
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsbnSave = new System.Windows.Forms.ToolStripButton();
			this.tsdbIcon = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmiIcon2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiIcon1 = new System.Windows.Forms.ToolStripMenuItem();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label5 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			tabGeneral = new System.Windows.Forms.TabPage();
			tabConnecion = new System.Windows.Forms.TabPage();
			label11 = new System.Windows.Forms.Label();
			label10 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			tabLocal = new System.Windows.Forms.TabPage();
			label13 = new System.Windows.Forms.Label();
			label12 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			gbRedirect = new System.Windows.Forms.GroupBox();
			flowRedirect = new System.Windows.Forms.FlowLayoutPanel();
			tabApplication = new System.Windows.Forms.TabPage();
			label9 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			tabDisplay = new System.Windows.Forms.TabPage();
			gbDesktopSize = new System.Windows.Forms.GroupBox();
			rbSize800x600 = new System.Windows.Forms.RadioButton();
			rbSize1024x768 = new System.Windows.Forms.RadioButton();
			rbSize1280x800 = new System.Windows.Forms.RadioButton();
			rbSize1280x1024 = new System.Windows.Forms.RadioButton();
			rbSize1440x900 = new System.Windows.Forms.RadioButton();
			rbSize1600x1200 = new System.Windows.Forms.RadioButton();
			rbSize1680x1050 = new System.Windows.Forms.RadioButton();
			rbSize1920x1200 = new System.Windows.Forms.RadioButton();
			rbSizeSame = new System.Windows.Forms.RadioButton();
			rbSizeFull = new System.Windows.Forms.RadioButton();
			label14 = new System.Windows.Forms.Label();
			label15 = new System.Windows.Forms.Label();
			label16 = new System.Windows.Forms.Label();
			label17 = new System.Windows.Forms.Label();
			label18 = new System.Windows.Forms.Label();
			label19 = new System.Windows.Forms.Label();
			tabGateway = new System.Windows.Forms.TabPage();
			tabPerfomance = new System.Windows.Forms.TabPage();
			groupBox1.SuspendLayout();
			tabGeneral.SuspendLayout();
			tabConnecion.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udMinutesToIdle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udPort)).BeginInit();
			tabLocal.SuspendLayout();
			gbRedirect.SuspendLayout();
			flowRedirect.SuspendLayout();
			tabApplication.SuspendLayout();
			this.gbRunApplication.SuspendLayout();
			tabDisplay.SuspendLayout();
			gbDesktopSize.SuspendLayout();
			this.flowDesktopSize.SuspendLayout();
			tabGateway.SuspendLayout();
			this.gbGateway.SuspendLayout();
			tabPerfomance.SuspendLayout();
			this.tabMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.error)).BeginInit();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(17, 38);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(49, 17);
			label1.TabIndex = 2;
			label1.Text = "&Name:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(8, 20);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(73, 17);
			label2.TabIndex = 0;
			label2.Text = "&Computer:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(8, 53);
			label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(42, 17);
			label3.TabIndex = 2;
			label3.Text = "&User:";
			// 
			// groupBox1
			// 
			groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupBox1.Controls.Add(this.txtDomain);
			groupBox1.Controls.Add(label5);
			groupBox1.Controls.Add(this.txtPassword);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(this.txtUser);
			groupBox1.Controls.Add(this.ddlComputer);
			groupBox1.Controls.Add(label3);
			groupBox1.Location = new System.Drawing.Point(9, 7);
			groupBox1.Margin = new System.Windows.Forms.Padding(4);
			groupBox1.Name = "groupBox1";
			groupBox1.Padding = new System.Windows.Forms.Padding(4);
			groupBox1.Size = new System.Drawing.Size(329, 145);
			groupBox1.TabIndex = 4;
			groupBox1.TabStop = false;
			groupBox1.Text = "Login";
			// 
			// txtDomain
			// 
			this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDomain.DefaultText = null;
			this.txtDomain.Location = new System.Drawing.Point(89, 113);
			this.txtDomain.Margin = new System.Windows.Forms.Padding(4);
			this.txtDomain.MaxLength = 2147483647;
			this.txtDomain.Name = "txtDomain";
			this.txtDomain.PlaceHolderText = null;
			this.txtDomain.Size = new System.Drawing.Size(231, 22);
			this.txtDomain.TabIndex = 7;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(8, 117);
			label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(60, 17);
			label5.TabIndex = 6;
			label5.Text = "&Domain:";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(89, 81);
			this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(231, 22);
			this.txtPassword.TabIndex = 5;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(8, 85);
			label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(73, 17);
			label4.TabIndex = 4;
			label4.Text = "&Password:";
			// 
			// txtUser
			// 
			this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUser.DefaultText = null;
			this.error.SetIconAlignment(this.txtUser, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtUser.Location = new System.Drawing.Point(89, 49);
			this.txtUser.Margin = new System.Windows.Forms.Padding(4);
			this.txtUser.MaxLength = 2147483647;
			this.txtUser.Name = "txtUser";
			this.txtUser.PlaceHolderText = null;
			this.txtUser.Size = new System.Drawing.Size(231, 22);
			this.txtUser.TabIndex = 3;
			// 
			// ddlComputer
			// 
			this.ddlComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlComputer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.ddlComputer.FormattingEnabled = true;
			this.error.SetIconAlignment(this.ddlComputer, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.ddlComputer.Location = new System.Drawing.Point(89, 16);
			this.ddlComputer.Margin = new System.Windows.Forms.Padding(4);
			this.ddlComputer.Name = "ddlComputer";
			this.ddlComputer.Size = new System.Drawing.Size(231, 24);
			this.ddlComputer.TabIndex = 1;
			// 
			// tabGeneral
			// 
			tabGeneral.Controls.Add(groupBox1);
			tabGeneral.Location = new System.Drawing.Point(4, 25);
			tabGeneral.Margin = new System.Windows.Forms.Padding(4);
			tabGeneral.Name = "tabGeneral";
			tabGeneral.Padding = new System.Windows.Forms.Padding(4);
			tabGeneral.Size = new System.Drawing.Size(349, 260);
			tabGeneral.TabIndex = 0;
			tabGeneral.Text = "General";
			tabGeneral.UseVisualStyleBackColor = true;
			// 
			// tabConnecion
			// 
			tabConnecion.Controls.Add(this.txtVmConsoleId);
			tabConnecion.Controls.Add(this.cbVmConsole);
			tabConnecion.Controls.Add(this.udMinutesToIdle);
			tabConnecion.Controls.Add(label11);
			tabConnecion.Controls.Add(this.ddlAuthenticationLevel);
			tabConnecion.Controls.Add(label10);
			tabConnecion.Controls.Add(this.udPort);
			tabConnecion.Controls.Add(label6);
			tabConnecion.Controls.Add(this.cbConsole);
			tabConnecion.Location = new System.Drawing.Point(4, 25);
			tabConnecion.Margin = new System.Windows.Forms.Padding(4);
			tabConnecion.Name = "tabConnecion";
			tabConnecion.Size = new System.Drawing.Size(349, 260);
			tabConnecion.TabIndex = 1;
			tabConnecion.Text = "Connection";
			tabConnecion.UseVisualStyleBackColor = true;
			// 
			// txtVmConsoleId
			// 
			this.txtVmConsoleId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtVmConsoleId.Location = new System.Drawing.Point(135, 130);
			this.txtVmConsoleId.Margin = new System.Windows.Forms.Padding(4);
			this.txtVmConsoleId.Name = "txtVmConsoleId";
			this.txtVmConsoleId.Size = new System.Drawing.Size(201, 22);
			this.txtVmConsoleId.TabIndex = 8;
			this.txtVmConsoleId.Visible = false;
			// 
			// cbVmConsole
			// 
			this.cbVmConsole.AutoSize = true;
			this.cbVmConsole.Location = new System.Drawing.Point(12, 133);
			this.cbVmConsole.Margin = new System.Windows.Forms.Padding(4);
			this.cbVmConsole.Name = "cbVmConsole";
			this.cbVmConsole.Size = new System.Drawing.Size(109, 21);
			this.cbVmConsole.TabIndex = 7;
			this.cbVmConsole.Text = "&VM Console:";
			this.cbVmConsole.UseVisualStyleBackColor = true;
			this.cbVmConsole.CheckedChanged += new System.EventHandler(this.cbVmConsole_CheckedChanged);
			// 
			// udMinutesToIdle
			// 
			this.udMinutesToIdle.Location = new System.Drawing.Point(135, 98);
			this.udMinutesToIdle.Margin = new System.Windows.Forms.Padding(4);
			this.udMinutesToIdle.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
			this.udMinutesToIdle.Name = "udMinutesToIdle";
			this.udMinutesToIdle.Size = new System.Drawing.Size(101, 22);
			this.udMinutesToIdle.TabIndex = 6;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(8, 102);
			label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(103, 17);
			label11.TabIndex = 5;
			label11.Text = "&Minutes to idle:";
			// 
			// ddlAuthenticationLevel
			// 
			this.ddlAuthenticationLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlAuthenticationLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlAuthenticationLevel.FormattingEnabled = true;
			this.ddlAuthenticationLevel.Items.AddRange(new object[] {
            "No authentication",
            "Do not connect if authentication fails",
            "Warn if authentication fails"});
			this.ddlAuthenticationLevel.Location = new System.Drawing.Point(135, 65);
			this.ddlAuthenticationLevel.Margin = new System.Windows.Forms.Padding(4);
			this.ddlAuthenticationLevel.Name = "ddlAuthenticationLevel";
			this.ddlAuthenticationLevel.Size = new System.Drawing.Size(201, 24);
			this.ddlAuthenticationLevel.TabIndex = 4;
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(8, 69);
			label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(102, 17);
			label10.TabIndex = 3;
			label10.Text = "&Authentication:";
			// 
			// udPort
			// 
			this.udPort.DefaultValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.udPort.Location = new System.Drawing.Point(135, 33);
			this.udPort.Margin = new System.Windows.Forms.Padding(4);
			this.udPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.udPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udPort.Name = "udPort";
			this.udPort.Size = new System.Drawing.Size(101, 22);
			this.udPort.TabIndex = 2;
			this.udPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udPort.ValueChanged += new System.EventHandler(this.udPort_ValueChanged);
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(8, 37);
			label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(38, 17);
			label6.TabIndex = 1;
			label6.Text = "&Port:";
			// 
			// cbConsole
			// 
			this.cbConsole.AutoSize = true;
			this.cbConsole.Location = new System.Drawing.Point(135, 5);
			this.cbConsole.Margin = new System.Windows.Forms.Padding(4);
			this.cbConsole.Name = "cbConsole";
			this.cbConsole.Size = new System.Drawing.Size(151, 21);
			this.cbConsole.TabIndex = 0;
			this.cbConsole.Text = "Connect to &console";
			this.cbConsole.UseVisualStyleBackColor = true;
			// 
			// tabLocal
			// 
			tabLocal.Controls.Add(this.ddlKeyboardHook);
			tabLocal.Controls.Add(label13);
			tabLocal.Controls.Add(this.ddlRedirectAudioQuality);
			tabLocal.Controls.Add(this.cbRedirectAudioCapture);
			tabLocal.Controls.Add(label12);
			tabLocal.Controls.Add(label7);
			tabLocal.Controls.Add(this.ddlRedirectAudio);
			tabLocal.Controls.Add(gbRedirect);
			tabLocal.Location = new System.Drawing.Point(4, 25);
			tabLocal.Margin = new System.Windows.Forms.Padding(4);
			tabLocal.Name = "tabLocal";
			tabLocal.Size = new System.Drawing.Size(349, 260);
			tabLocal.TabIndex = 2;
			tabLocal.Text = "Local Resources";
			tabLocal.UseVisualStyleBackColor = true;
			// 
			// ddlKeyboardHook
			// 
			this.ddlKeyboardHook.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlKeyboardHook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlKeyboardHook.FormattingEnabled = true;
			this.ddlKeyboardHook.Items.AddRange(new object[] {
            "On the local computer",
            "On the remote computer",
            "In full screen mode only"});
			this.ddlKeyboardHook.Location = new System.Drawing.Point(120, 98);
			this.ddlKeyboardHook.Margin = new System.Windows.Forms.Padding(4);
			this.ddlKeyboardHook.Name = "ddlKeyboardHook";
			this.ddlKeyboardHook.Size = new System.Drawing.Size(216, 24);
			this.ddlKeyboardHook.TabIndex = 8;
			// 
			// label13
			// 
			label13.AutoSize = true;
			label13.Location = new System.Drawing.Point(8, 102);
			label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label13.Name = "label13";
			label13.Size = new System.Drawing.Size(108, 17);
			label13.TabIndex = 7;
			label13.Text = "&Keyboard hook:";
			// 
			// ddlRedirectAudioQuality
			// 
			this.ddlRedirectAudioQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlRedirectAudioQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlRedirectAudioQuality.FormattingEnabled = true;
			this.ddlRedirectAudioQuality.Items.AddRange(new object[] {
            "Dynamic",
            "High",
            "Medium"});
			this.ddlRedirectAudioQuality.Location = new System.Drawing.Point(120, 65);
			this.ddlRedirectAudioQuality.Margin = new System.Windows.Forms.Padding(4);
			this.ddlRedirectAudioQuality.Name = "ddlRedirectAudioQuality";
			this.ddlRedirectAudioQuality.Size = new System.Drawing.Size(216, 24);
			this.ddlRedirectAudioQuality.TabIndex = 6;
			// 
			// cbRedirectAudioCapture
			// 
			this.cbRedirectAudioCapture.AutoSize = true;
			this.cbRedirectAudioCapture.Location = new System.Drawing.Point(120, 5);
			this.cbRedirectAudioCapture.Margin = new System.Windows.Forms.Padding(4);
			this.cbRedirectAudioCapture.Name = "cbRedirectAudioCapture";
			this.cbRedirectAudioCapture.Size = new System.Drawing.Size(118, 21);
			this.cbRedirectAudioCapture.TabIndex = 4;
			this.cbRedirectAudioCapture.Text = "&Audio capture";
			this.cbRedirectAudioCapture.UseVisualStyleBackColor = true;
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(8, 69);
			label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(98, 17);
			label12.TabIndex = 5;
			label12.Text = "Sound &quality:";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(8, 37);
			label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(104, 17);
			label7.TabIndex = 2;
			label7.Text = "Remote &sound:";
			// 
			// ddlRedirectAudio
			// 
			this.ddlRedirectAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlRedirectAudio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlRedirectAudio.FormattingEnabled = true;
			this.ddlRedirectAudio.Items.AddRange(new object[] {
            "Bring to this computer",
            "Leave at remote computer",
            "Do not play"});
			this.ddlRedirectAudio.Location = new System.Drawing.Point(120, 33);
			this.ddlRedirectAudio.Margin = new System.Windows.Forms.Padding(4);
			this.ddlRedirectAudio.Name = "ddlRedirectAudio";
			this.ddlRedirectAudio.Size = new System.Drawing.Size(216, 24);
			this.ddlRedirectAudio.TabIndex = 1;
			// 
			// gbRedirect
			// 
			gbRedirect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			gbRedirect.Controls.Add(flowRedirect);
			gbRedirect.Location = new System.Drawing.Point(4, 132);
			gbRedirect.Margin = new System.Windows.Forms.Padding(4);
			gbRedirect.Name = "gbRedirect";
			gbRedirect.Padding = new System.Windows.Forms.Padding(4);
			gbRedirect.Size = new System.Drawing.Size(339, 116);
			gbRedirect.TabIndex = 0;
			gbRedirect.TabStop = false;
			gbRedirect.Text = "Redirect";
			// 
			// flowRedirect
			// 
			flowRedirect.Controls.Add(this.cbRedirectDrives);
			flowRedirect.Controls.Add(this.cbRedirectPrinters);
			flowRedirect.Controls.Add(this.cbRedirectClipboard);
			flowRedirect.Controls.Add(this.cbRedirectPorts);
			flowRedirect.Controls.Add(this.cbRedirectSmartCards);
			flowRedirect.Controls.Add(this.cbRedirectPoS);
			flowRedirect.Dock = System.Windows.Forms.DockStyle.Fill;
			flowRedirect.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowRedirect.Location = new System.Drawing.Point(4, 19);
			flowRedirect.Margin = new System.Windows.Forms.Padding(4);
			flowRedirect.Name = "flowRedirect";
			flowRedirect.Size = new System.Drawing.Size(331, 93);
			flowRedirect.TabIndex = 1;
			// 
			// cbRedirectDrives
			// 
			this.cbRedirectDrives.AutoSize = true;
			this.cbRedirectDrives.Location = new System.Drawing.Point(4, 4);
			this.cbRedirectDrives.Margin = new System.Windows.Forms.Padding(4);
			this.cbRedirectDrives.Name = "cbRedirectDrives";
			this.cbRedirectDrives.Size = new System.Drawing.Size(70, 21);
			this.cbRedirectDrives.TabIndex = 0;
			this.cbRedirectDrives.Text = "&Drives";
			this.cbRedirectDrives.UseVisualStyleBackColor = true;
			// 
			// cbRedirectPrinters
			// 
			this.cbRedirectPrinters.AutoSize = true;
			this.cbRedirectPrinters.Location = new System.Drawing.Point(4, 33);
			this.cbRedirectPrinters.Margin = new System.Windows.Forms.Padding(4);
			this.cbRedirectPrinters.Name = "cbRedirectPrinters";
			this.cbRedirectPrinters.Size = new System.Drawing.Size(79, 21);
			this.cbRedirectPrinters.TabIndex = 1;
			this.cbRedirectPrinters.Text = "&Printers";
			this.cbRedirectPrinters.UseVisualStyleBackColor = true;
			// 
			// cbRedirectClipboard
			// 
			this.cbRedirectClipboard.AutoSize = true;
			this.cbRedirectClipboard.Checked = true;
			this.cbRedirectClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRedirectClipboard.Location = new System.Drawing.Point(4, 62);
			this.cbRedirectClipboard.Margin = new System.Windows.Forms.Padding(4);
			this.cbRedirectClipboard.Name = "cbRedirectClipboard";
			this.cbRedirectClipboard.Size = new System.Drawing.Size(90, 21);
			this.cbRedirectClipboard.TabIndex = 2;
			this.cbRedirectClipboard.Text = "Clip&board";
			this.cbRedirectClipboard.UseVisualStyleBackColor = true;
			// 
			// cbRedirectPorts
			// 
			this.cbRedirectPorts.AutoSize = true;
			this.cbRedirectPorts.Location = new System.Drawing.Point(102, 4);
			this.cbRedirectPorts.Margin = new System.Windows.Forms.Padding(4);
			this.cbRedirectPorts.Name = "cbRedirectPorts";
			this.cbRedirectPorts.Size = new System.Drawing.Size(63, 21);
			this.cbRedirectPorts.TabIndex = 3;
			this.cbRedirectPorts.Text = "P&orts";
			this.cbRedirectPorts.UseVisualStyleBackColor = true;
			// 
			// cbRedirectSmartCards
			// 
			this.cbRedirectSmartCards.AutoSize = true;
			this.cbRedirectSmartCards.Location = new System.Drawing.Point(102, 33);
			this.cbRedirectSmartCards.Margin = new System.Windows.Forms.Padding(4);
			this.cbRedirectSmartCards.Name = "cbRedirectSmartCards";
			this.cbRedirectSmartCards.Size = new System.Drawing.Size(106, 21);
			this.cbRedirectSmartCards.TabIndex = 4;
			this.cbRedirectSmartCards.Text = "Smart &cards";
			this.cbRedirectSmartCards.UseVisualStyleBackColor = true;
			// 
			// cbRedirectPoS
			// 
			this.cbRedirectPoS.AutoSize = true;
			this.cbRedirectPoS.Location = new System.Drawing.Point(102, 62);
			this.cbRedirectPoS.Margin = new System.Windows.Forms.Padding(4);
			this.cbRedirectPoS.Name = "cbRedirectPoS";
			this.cbRedirectPoS.Size = new System.Drawing.Size(129, 21);
			this.cbRedirectPoS.TabIndex = 5;
			this.cbRedirectPoS.Text = "Po&int of Service";
			this.cbRedirectPoS.UseVisualStyleBackColor = true;
			// 
			// tabApplication
			// 
			tabApplication.Controls.Add(this.cbRunApplication);
			tabApplication.Controls.Add(this.gbRunApplication);
			tabApplication.Location = new System.Drawing.Point(4, 25);
			tabApplication.Margin = new System.Windows.Forms.Padding(4);
			tabApplication.Name = "tabApplication";
			tabApplication.Size = new System.Drawing.Size(349, 260);
			tabApplication.TabIndex = 3;
			tabApplication.Text = "Applications";
			tabApplication.UseVisualStyleBackColor = true;
			// 
			// cbRunApplication
			// 
			this.cbRunApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbRunApplication.AutoSize = true;
			this.cbRunApplication.Location = new System.Drawing.Point(133, 2);
			this.cbRunApplication.Margin = new System.Windows.Forms.Padding(4);
			this.cbRunApplication.Name = "cbRunApplication";
			this.cbRunApplication.Size = new System.Drawing.Size(196, 21);
			this.cbRunApplication.TabIndex = 0;
			this.cbRunApplication.Text = "Run application on startup";
			this.cbRunApplication.UseVisualStyleBackColor = true;
			this.cbRunApplication.CheckedChanged += new System.EventHandler(this.cbRunApplication_CheckedChanged);
			// 
			// gbRunApplication
			// 
			this.gbRunApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbRunApplication.Controls.Add(this.cbRunApplicationMaximize);
			this.gbRunApplication.Controls.Add(this.txtRunApplicationWorkingDir);
			this.gbRunApplication.Controls.Add(label9);
			this.gbRunApplication.Controls.Add(this.txtRunApplicationFilePath);
			this.gbRunApplication.Controls.Add(label8);
			this.gbRunApplication.Enabled = false;
			this.gbRunApplication.Location = new System.Drawing.Point(4, 4);
			this.gbRunApplication.Margin = new System.Windows.Forms.Padding(4);
			this.gbRunApplication.Name = "gbRunApplication";
			this.gbRunApplication.Padding = new System.Windows.Forms.Padding(4);
			this.gbRunApplication.Size = new System.Drawing.Size(339, 247);
			this.gbRunApplication.TabIndex = 0;
			this.gbRunApplication.TabStop = false;
			this.gbRunApplication.Text = "Run application";
			// 
			// cbRunApplicationMaximize
			// 
			this.cbRunApplicationMaximize.AutoSize = true;
			this.cbRunApplicationMaximize.Location = new System.Drawing.Point(13, 138);
			this.cbRunApplicationMaximize.Margin = new System.Windows.Forms.Padding(4);
			this.cbRunApplicationMaximize.Name = "cbRunApplicationMaximize";
			this.cbRunApplicationMaximize.Size = new System.Drawing.Size(159, 21);
			this.cbRunApplicationMaximize.TabIndex = 4;
			this.cbRunApplicationMaximize.Text = "&Maximize application";
			this.cbRunApplicationMaximize.UseVisualStyleBackColor = true;
			// 
			// txtRunApplicationWorkingDir
			// 
			this.txtRunApplicationWorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRunApplicationWorkingDir.Location = new System.Drawing.Point(13, 105);
			this.txtRunApplicationWorkingDir.Margin = new System.Windows.Forms.Padding(4);
			this.txtRunApplicationWorkingDir.Name = "txtRunApplicationWorkingDir";
			this.txtRunApplicationWorkingDir.Size = new System.Drawing.Size(316, 22);
			this.txtRunApplicationWorkingDir.TabIndex = 3;
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(9, 84);
			label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(84, 17);
			label9.TabIndex = 2;
			label9.Text = "&Working dir:";
			// 
			// txtRunApplicationFilePath
			// 
			this.txtRunApplicationFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRunApplicationFilePath.Location = new System.Drawing.Point(13, 44);
			this.txtRunApplicationFilePath.Margin = new System.Windows.Forms.Padding(4);
			this.txtRunApplicationFilePath.Name = "txtRunApplicationFilePath";
			this.txtRunApplicationFilePath.Size = new System.Drawing.Size(316, 22);
			this.txtRunApplicationFilePath.TabIndex = 1;
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(9, 25);
			label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(155, 17);
			label8.TabIndex = 0;
			label8.Text = "File path and file name:";
			// 
			// tabDisplay
			// 
			tabDisplay.Controls.Add(gbDesktopSize);
			tabDisplay.Controls.Add(this.ddlColorDepth);
			tabDisplay.Controls.Add(label14);
			tabDisplay.Location = new System.Drawing.Point(4, 25);
			tabDisplay.Margin = new System.Windows.Forms.Padding(4);
			tabDisplay.Name = "tabDisplay";
			tabDisplay.Size = new System.Drawing.Size(349, 260);
			tabDisplay.TabIndex = 4;
			tabDisplay.Text = "Display";
			tabDisplay.UseVisualStyleBackColor = true;
			// 
			// gbDesktopSize
			// 
			gbDesktopSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			gbDesktopSize.Controls.Add(this.flowDesktopSize);
			gbDesktopSize.Location = new System.Drawing.Point(4, 37);
			gbDesktopSize.Margin = new System.Windows.Forms.Padding(4);
			gbDesktopSize.Name = "gbDesktopSize";
			gbDesktopSize.Padding = new System.Windows.Forms.Padding(4);
			gbDesktopSize.Size = new System.Drawing.Size(339, 217);
			gbDesktopSize.TabIndex = 2;
			gbDesktopSize.TabStop = false;
			gbDesktopSize.Text = "Remote Desktop Size";
			// 
			// flowDesktopSize
			// 
			this.flowDesktopSize.Controls.Add(rbSize800x600);
			this.flowDesktopSize.Controls.Add(rbSize1024x768);
			this.flowDesktopSize.Controls.Add(rbSize1280x800);
			this.flowDesktopSize.Controls.Add(rbSize1280x1024);
			this.flowDesktopSize.Controls.Add(rbSize1440x900);
			this.flowDesktopSize.Controls.Add(rbSize1600x1200);
			this.flowDesktopSize.Controls.Add(rbSize1680x1050);
			this.flowDesktopSize.Controls.Add(rbSize1920x1200);
			this.flowDesktopSize.Controls.Add(rbSizeSame);
			this.flowDesktopSize.Controls.Add(rbSizeFull);
			this.flowDesktopSize.Controls.Add(this.rbSizeCustom);
			this.flowDesktopSize.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowDesktopSize.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowDesktopSize.Location = new System.Drawing.Point(4, 19);
			this.flowDesktopSize.Margin = new System.Windows.Forms.Padding(4);
			this.flowDesktopSize.Name = "flowDesktopSize";
			this.flowDesktopSize.Size = new System.Drawing.Size(331, 194);
			this.flowDesktopSize.TabIndex = 0;
			// 
			// rbSize800x600
			// 
			rbSize800x600.AutoSize = true;
			rbSize800x600.Location = new System.Drawing.Point(4, 4);
			rbSize800x600.Margin = new System.Windows.Forms.Padding(4);
			rbSize800x600.Name = "rbSize800x600";
			rbSize800x600.Size = new System.Drawing.Size(91, 21);
			rbSize800x600.TabIndex = 0;
			rbSize800x600.Tag = "800x600";
			rbSize800x600.Text = "800 x 600";
			rbSize800x600.UseVisualStyleBackColor = true;
			// 
			// rbSize1024x768
			// 
			rbSize1024x768.AutoSize = true;
			rbSize1024x768.Location = new System.Drawing.Point(4, 33);
			rbSize1024x768.Margin = new System.Windows.Forms.Padding(4);
			rbSize1024x768.Name = "rbSize1024x768";
			rbSize1024x768.Size = new System.Drawing.Size(99, 21);
			rbSize1024x768.TabIndex = 4;
			rbSize1024x768.Tag = "1024x768";
			rbSize1024x768.Text = "1024 x 768";
			rbSize1024x768.UseVisualStyleBackColor = true;
			// 
			// rbSize1280x800
			// 
			rbSize1280x800.AutoSize = true;
			rbSize1280x800.Location = new System.Drawing.Point(4, 62);
			rbSize1280x800.Margin = new System.Windows.Forms.Padding(4);
			rbSize1280x800.Name = "rbSize1280x800";
			rbSize1280x800.Size = new System.Drawing.Size(99, 21);
			rbSize1280x800.TabIndex = 1;
			rbSize1280x800.Tag = "1280x800";
			rbSize1280x800.Text = "1280 x 800";
			rbSize1280x800.UseVisualStyleBackColor = true;
			// 
			// rbSize1280x1024
			// 
			rbSize1280x1024.AutoSize = true;
			rbSize1280x1024.Location = new System.Drawing.Point(4, 91);
			rbSize1280x1024.Margin = new System.Windows.Forms.Padding(4);
			rbSize1280x1024.Name = "rbSize1280x1024";
			rbSize1280x1024.Size = new System.Drawing.Size(107, 21);
			rbSize1280x1024.TabIndex = 5;
			rbSize1280x1024.Tag = "1280x1024";
			rbSize1280x1024.Text = "1280 x 1024";
			rbSize1280x1024.UseVisualStyleBackColor = true;
			// 
			// rbSize1440x900
			// 
			rbSize1440x900.AutoSize = true;
			rbSize1440x900.Location = new System.Drawing.Point(4, 120);
			rbSize1440x900.Margin = new System.Windows.Forms.Padding(4);
			rbSize1440x900.Name = "rbSize1440x900";
			rbSize1440x900.Size = new System.Drawing.Size(99, 21);
			rbSize1440x900.TabIndex = 2;
			rbSize1440x900.Tag = "1440x900";
			rbSize1440x900.Text = "1440 x 900";
			rbSize1440x900.UseVisualStyleBackColor = true;
			// 
			// rbSize1600x1200
			// 
			rbSize1600x1200.AutoSize = true;
			rbSize1600x1200.Location = new System.Drawing.Point(4, 149);
			rbSize1600x1200.Margin = new System.Windows.Forms.Padding(4);
			rbSize1600x1200.Name = "rbSize1600x1200";
			rbSize1600x1200.Size = new System.Drawing.Size(107, 21);
			rbSize1600x1200.TabIndex = 6;
			rbSize1600x1200.Tag = "1600x1200";
			rbSize1600x1200.Text = "1600 x 1200";
			rbSize1600x1200.UseVisualStyleBackColor = true;
			// 
			// rbSize1680x1050
			// 
			rbSize1680x1050.AutoSize = true;
			rbSize1680x1050.Location = new System.Drawing.Point(119, 4);
			rbSize1680x1050.Margin = new System.Windows.Forms.Padding(4);
			rbSize1680x1050.Name = "rbSize1680x1050";
			rbSize1680x1050.Size = new System.Drawing.Size(107, 21);
			rbSize1680x1050.TabIndex = 3;
			rbSize1680x1050.Tag = "1680x1050";
			rbSize1680x1050.Text = "1680 x 1050";
			rbSize1680x1050.UseVisualStyleBackColor = true;
			// 
			// rbSize1920x1200
			// 
			rbSize1920x1200.AutoSize = true;
			rbSize1920x1200.Location = new System.Drawing.Point(119, 33);
			rbSize1920x1200.Margin = new System.Windows.Forms.Padding(4);
			rbSize1920x1200.Name = "rbSize1920x1200";
			rbSize1920x1200.Size = new System.Drawing.Size(107, 21);
			rbSize1920x1200.TabIndex = 7;
			rbSize1920x1200.Tag = "1920x1200";
			rbSize1920x1200.Text = "1920 x 1200";
			rbSize1920x1200.UseVisualStyleBackColor = true;
			// 
			// rbSizeSame
			// 
			rbSizeSame.AutoSize = true;
			rbSizeSame.Checked = true;
			rbSizeSame.Location = new System.Drawing.Point(119, 62);
			rbSizeSame.Margin = new System.Windows.Forms.Padding(4);
			rbSizeSame.Name = "rbSizeSame";
			rbSizeSame.Size = new System.Drawing.Size(154, 21);
			rbSizeSame.TabIndex = 8;
			rbSizeSame.TabStop = true;
			rbSizeSame.Tag = "0";
			rbSizeSame.Text = "&Same as client area";
			rbSizeSame.UseVisualStyleBackColor = true;
			// 
			// rbSizeFull
			// 
			rbSizeFull.AutoSize = true;
			rbSizeFull.Location = new System.Drawing.Point(119, 91);
			rbSizeFull.Margin = new System.Windows.Forms.Padding(4);
			rbSizeFull.Name = "rbSizeFull";
			rbSizeFull.Size = new System.Drawing.Size(98, 21);
			rbSizeFull.TabIndex = 9;
			rbSizeFull.Tag = "1";
			rbSizeFull.Text = "&Full screen";
			rbSizeFull.UseVisualStyleBackColor = true;
			// 
			// rbSizeCustom
			// 
			this.rbSizeCustom.AutoSize = true;
			this.rbSizeCustom.Location = new System.Drawing.Point(119, 120);
			this.rbSizeCustom.Margin = new System.Windows.Forms.Padding(4);
			this.rbSizeCustom.Name = "rbSizeCustom";
			this.rbSizeCustom.Size = new System.Drawing.Size(76, 21);
			this.rbSizeCustom.TabIndex = 10;
			this.rbSizeCustom.TabStop = true;
			this.rbSizeCustom.Text = "Custom";
			this.rbSizeCustom.UseVisualStyleBackColor = true;
			this.rbSizeCustom.CheckedChanged += new System.EventHandler(this.rbSizeCustom_CheckedChanged);
			// 
			// ddlColorDepth
			// 
			this.ddlColorDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlColorDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlColorDepth.FormattingEnabled = true;
			this.ddlColorDepth.Items.AddRange(new object[] {
            "8",
            "15",
            "16",
            "24",
            "32"});
			this.ddlColorDepth.Location = new System.Drawing.Point(105, 4);
			this.ddlColorDepth.Margin = new System.Windows.Forms.Padding(4);
			this.ddlColorDepth.Name = "ddlColorDepth";
			this.ddlColorDepth.Size = new System.Drawing.Size(240, 24);
			this.ddlColorDepth.TabIndex = 1;
			// 
			// label14
			// 
			label14.AutoSize = true;
			label14.Location = new System.Drawing.Point(9, 7);
			label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label14.Name = "label14";
			label14.Size = new System.Drawing.Size(87, 17);
			label14.TabIndex = 0;
			label14.Text = "&Color Depth:";
			// 
			// label15
			// 
			label15.AutoSize = true;
			label15.Location = new System.Drawing.Point(13, 32);
			label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label15.Name = "label15";
			label15.Size = new System.Drawing.Size(93, 17);
			label15.TabIndex = 0;
			label15.Text = "&Server name:";
			// 
			// label16
			// 
			label16.AutoSize = true;
			label16.Location = new System.Drawing.Point(13, 121);
			label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label16.Name = "label16";
			label16.Size = new System.Drawing.Size(81, 17);
			label16.TabIndex = 4;
			label16.Text = "&User name:";
			// 
			// label17
			// 
			label17.AutoSize = true;
			label17.Location = new System.Drawing.Point(13, 153);
			label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label17.Name = "label17";
			label17.Size = new System.Drawing.Size(73, 17);
			label17.TabIndex = 6;
			label17.Text = "&Password:";
			// 
			// label18
			// 
			label18.AutoSize = true;
			label18.Location = new System.Drawing.Point(13, 185);
			label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label18.Name = "label18";
			label18.Size = new System.Drawing.Size(60, 17);
			label18.TabIndex = 8;
			label18.Text = "&Domain:";
			// 
			// label19
			// 
			label19.AutoSize = true;
			label19.Location = new System.Drawing.Point(13, 218);
			label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label19.Name = "label19";
			label19.Size = new System.Drawing.Size(103, 17);
			label19.TabIndex = 10;
			label19.Text = "&Logon method:";
			// 
			// tabGateway
			// 
			tabGateway.Controls.Add(this.cbUseGateway);
			tabGateway.Controls.Add(this.gbGateway);
			tabGateway.Location = new System.Drawing.Point(4, 25);
			tabGateway.Margin = new System.Windows.Forms.Padding(4);
			tabGateway.Name = "tabGateway";
			tabGateway.Size = new System.Drawing.Size(349, 260);
			tabGateway.TabIndex = 5;
			tabGateway.Text = "Gateway";
			tabGateway.UseVisualStyleBackColor = true;
			// 
			// cbUseGateway
			// 
			this.cbUseGateway.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbUseGateway.AutoSize = true;
			this.cbUseGateway.Location = new System.Drawing.Point(149, 2);
			this.cbUseGateway.Margin = new System.Windows.Forms.Padding(4);
			this.cbUseGateway.Name = "cbUseGateway";
			this.cbUseGateway.Size = new System.Drawing.Size(180, 21);
			this.cbUseGateway.TabIndex = 0;
			this.cbUseGateway.Text = "&Use TS Gateway server";
			this.cbUseGateway.UseVisualStyleBackColor = true;
			this.cbUseGateway.CheckedChanged += new System.EventHandler(this.cbUseGateway_CheckedChanged);
			// 
			// gbGateway
			// 
			this.gbGateway.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbGateway.Controls.Add(this.ddlGatewayLogonMethod);
			this.gbGateway.Controls.Add(label19);
			this.gbGateway.Controls.Add(this.txtGatewayDomain);
			this.gbGateway.Controls.Add(label18);
			this.gbGateway.Controls.Add(this.txtGatewayPassword);
			this.gbGateway.Controls.Add(label17);
			this.gbGateway.Controls.Add(this.txtGatewayUserName);
			this.gbGateway.Controls.Add(label16);
			this.gbGateway.Controls.Add(this.cbGatewayShareAuth);
			this.gbGateway.Controls.Add(this.cbGatewayBypass);
			this.gbGateway.Controls.Add(this.txtGatewayServer);
			this.gbGateway.Controls.Add(label15);
			this.gbGateway.Enabled = false;
			this.gbGateway.Location = new System.Drawing.Point(4, 4);
			this.gbGateway.Margin = new System.Windows.Forms.Padding(4);
			this.gbGateway.Name = "gbGateway";
			this.gbGateway.Padding = new System.Windows.Forms.Padding(4);
			this.gbGateway.Size = new System.Drawing.Size(339, 247);
			this.gbGateway.TabIndex = 0;
			this.gbGateway.TabStop = false;
			this.gbGateway.Text = "Gateway settings";
			// 
			// ddlGatewayLogonMethod
			// 
			this.ddlGatewayLogonMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlGatewayLogonMethod.FormattingEnabled = true;
			this.ddlGatewayLogonMethod.Items.AddRange(new object[] {
            "Ask for password (NTLM)",
            "Smart card",
            "Allow me to select later"});
			this.ddlGatewayLogonMethod.Location = new System.Drawing.Point(115, 214);
			this.ddlGatewayLogonMethod.Margin = new System.Windows.Forms.Padding(4);
			this.ddlGatewayLogonMethod.Name = "ddlGatewayLogonMethod";
			this.ddlGatewayLogonMethod.Size = new System.Drawing.Size(215, 24);
			this.ddlGatewayLogonMethod.TabIndex = 11;
			// 
			// txtGatewayDomain
			// 
			this.txtGatewayDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGatewayDomain.Location = new System.Drawing.Point(115, 181);
			this.txtGatewayDomain.Margin = new System.Windows.Forms.Padding(4);
			this.txtGatewayDomain.Name = "txtGatewayDomain";
			this.txtGatewayDomain.Size = new System.Drawing.Size(215, 22);
			this.txtGatewayDomain.TabIndex = 9;
			// 
			// txtGatewayPassword
			// 
			this.txtGatewayPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGatewayPassword.Location = new System.Drawing.Point(115, 149);
			this.txtGatewayPassword.Margin = new System.Windows.Forms.Padding(4);
			this.txtGatewayPassword.Name = "txtGatewayPassword";
			this.txtGatewayPassword.Size = new System.Drawing.Size(215, 22);
			this.txtGatewayPassword.TabIndex = 7;
			// 
			// txtGatewayUserName
			// 
			this.txtGatewayUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGatewayUserName.Location = new System.Drawing.Point(115, 117);
			this.txtGatewayUserName.Margin = new System.Windows.Forms.Padding(4);
			this.txtGatewayUserName.Name = "txtGatewayUserName";
			this.txtGatewayUserName.Size = new System.Drawing.Size(215, 22);
			this.txtGatewayUserName.TabIndex = 5;
			// 
			// cbGatewayShareAuth
			// 
			this.cbGatewayShareAuth.AutoSize = true;
			this.cbGatewayShareAuth.Location = new System.Drawing.Point(115, 89);
			this.cbGatewayShareAuth.Margin = new System.Windows.Forms.Padding(4);
			this.cbGatewayShareAuth.Name = "cbGatewayShareAuth";
			this.cbGatewayShareAuth.Size = new System.Drawing.Size(197, 21);
			this.cbGatewayShareAuth.TabIndex = 3;
			this.cbGatewayShareAuth.Text = "Share gateway &credentials";
			this.cbGatewayShareAuth.UseVisualStyleBackColor = true;
			// 
			// cbGatewayBypass
			// 
			this.cbGatewayBypass.AutoSize = true;
			this.cbGatewayBypass.Location = new System.Drawing.Point(115, 60);
			this.cbGatewayBypass.Margin = new System.Windows.Forms.Padding(4);
			this.cbGatewayBypass.Name = "cbGatewayBypass";
			this.cbGatewayBypass.Size = new System.Drawing.Size(200, 21);
			this.cbGatewayBypass.TabIndex = 2;
			this.cbGatewayBypass.Text = "&Bypass for local addresses";
			this.cbGatewayBypass.UseVisualStyleBackColor = true;
			// 
			// txtGatewayServer
			// 
			this.txtGatewayServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGatewayServer.Location = new System.Drawing.Point(115, 28);
			this.txtGatewayServer.Margin = new System.Windows.Forms.Padding(4);
			this.txtGatewayServer.Name = "txtGatewayServer";
			this.txtGatewayServer.Size = new System.Drawing.Size(215, 22);
			this.txtGatewayServer.TabIndex = 1;
			// 
			// tabPerfomance
			// 
			tabPerfomance.Controls.Add(this.cbDisableThemeing);
			tabPerfomance.Controls.Add(this.cbDisableMenuAnimations);
			tabPerfomance.Controls.Add(this.cbDisableFullWindowDrag);
			tabPerfomance.Controls.Add(this.cbEnableDesktopComposition);
			tabPerfomance.Controls.Add(this.cbEnableFontSmoothing);
			tabPerfomance.Controls.Add(this.cbDisableWallpaper);
			tabPerfomance.Controls.Add(this.ddlConnectionSpeed);
			tabPerfomance.Controls.Add(this.label20);
			tabPerfomance.Location = new System.Drawing.Point(4, 25);
			tabPerfomance.Margin = new System.Windows.Forms.Padding(4);
			tabPerfomance.Name = "tabPerfomance";
			tabPerfomance.Size = new System.Drawing.Size(349, 260);
			tabPerfomance.TabIndex = 6;
			tabPerfomance.Text = "Perfomance";
			tabPerfomance.UseVisualStyleBackColor = true;
			// 
			// cbDisableThemeing
			// 
			this.cbDisableThemeing.AutoSize = true;
			this.cbDisableThemeing.Location = new System.Drawing.Point(9, 214);
			this.cbDisableThemeing.Margin = new System.Windows.Forms.Padding(4);
			this.cbDisableThemeing.Name = "cbDisableThemeing";
			this.cbDisableThemeing.Size = new System.Drawing.Size(81, 21);
			this.cbDisableThemeing.TabIndex = 7;
			this.cbDisableThemeing.Tag = "0x00000008";
			this.cbDisableThemeing.Text = "Themes";
			this.cbDisableThemeing.UseVisualStyleBackColor = true;
			// 
			// cbDisableMenuAnimations
			// 
			this.cbDisableMenuAnimations.AutoSize = true;
			this.cbDisableMenuAnimations.Location = new System.Drawing.Point(9, 186);
			this.cbDisableMenuAnimations.Margin = new System.Windows.Forms.Padding(4);
			this.cbDisableMenuAnimations.Name = "cbDisableMenuAnimations";
			this.cbDisableMenuAnimations.Size = new System.Drawing.Size(207, 21);
			this.cbDisableMenuAnimations.TabIndex = 6;
			this.cbDisableMenuAnimations.Tag = "0x00000004";
			this.cbDisableMenuAnimations.Text = "Menu and window animation";
			this.cbDisableMenuAnimations.UseVisualStyleBackColor = true;
			// 
			// cbDisableFullWindowDrag
			// 
			this.cbDisableFullWindowDrag.AutoSize = true;
			this.cbDisableFullWindowDrag.Location = new System.Drawing.Point(9, 158);
			this.cbDisableFullWindowDrag.Margin = new System.Windows.Forms.Padding(4);
			this.cbDisableFullWindowDrag.Name = "cbDisableFullWindowDrag";
			this.cbDisableFullWindowDrag.Size = new System.Drawing.Size(282, 21);
			this.cbDisableFullWindowDrag.TabIndex = 5;
			this.cbDisableFullWindowDrag.Tag = "0x00000002";
			this.cbDisableFullWindowDrag.Text = "Show contents of window while dragging";
			this.cbDisableFullWindowDrag.UseVisualStyleBackColor = true;
			// 
			// cbEnableDesktopComposition
			// 
			this.cbEnableDesktopComposition.AutoSize = true;
			this.cbEnableDesktopComposition.Location = new System.Drawing.Point(9, 129);
			this.cbEnableDesktopComposition.Margin = new System.Windows.Forms.Padding(4);
			this.cbEnableDesktopComposition.Name = "cbEnableDesktopComposition";
			this.cbEnableDesktopComposition.Size = new System.Drawing.Size(161, 21);
			this.cbEnableDesktopComposition.TabIndex = 4;
			this.cbEnableDesktopComposition.Tag = "0x00000100";
			this.cbEnableDesktopComposition.Text = "Desktop composition";
			this.cbEnableDesktopComposition.UseVisualStyleBackColor = true;
			// 
			// cbEnableFontSmoothing
			// 
			this.cbEnableFontSmoothing.AutoSize = true;
			this.cbEnableFontSmoothing.Location = new System.Drawing.Point(9, 101);
			this.cbEnableFontSmoothing.Margin = new System.Windows.Forms.Padding(4);
			this.cbEnableFontSmoothing.Name = "cbEnableFontSmoothing";
			this.cbEnableFontSmoothing.Size = new System.Drawing.Size(127, 21);
			this.cbEnableFontSmoothing.TabIndex = 3;
			this.cbEnableFontSmoothing.Tag = "0x00000080";
			this.cbEnableFontSmoothing.Text = "Font smoothing";
			this.cbEnableFontSmoothing.UseVisualStyleBackColor = true;
			// 
			// cbDisableWallpaper
			// 
			this.cbDisableWallpaper.AutoSize = true;
			this.cbDisableWallpaper.Location = new System.Drawing.Point(9, 73);
			this.cbDisableWallpaper.Margin = new System.Windows.Forms.Padding(4);
			this.cbDisableWallpaper.Name = "cbDisableWallpaper";
			this.cbDisableWallpaper.Size = new System.Drawing.Size(161, 21);
			this.cbDisableWallpaper.TabIndex = 2;
			this.cbDisableWallpaper.Tag = "0x00000001";
			this.cbDisableWallpaper.Text = "Desktop background";
			this.cbDisableWallpaper.UseVisualStyleBackColor = true;
			// 
			// ddlConnectionSpeed
			// 
			this.ddlConnectionSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlConnectionSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlConnectionSpeed.FormattingEnabled = true;
			this.ddlConnectionSpeed.Items.AddRange(new object[] {
            "Modem (28.8 Kbps)",
            "Modem (56 Kbps)",
            "Broadband (128 Kbps - 1.5Mbps)",
            "LAN (10 Mbps or higher)",
            "Custom"});
			this.ddlConnectionSpeed.Location = new System.Drawing.Point(9, 39);
			this.ddlConnectionSpeed.Margin = new System.Windows.Forms.Padding(4);
			this.ddlConnectionSpeed.Name = "ddlConnectionSpeed";
			this.ddlConnectionSpeed.Size = new System.Drawing.Size(332, 24);
			this.ddlConnectionSpeed.TabIndex = 1;
			this.ddlConnectionSpeed.SelectedIndexChanged += new System.EventHandler(this.ddlConnectionSpeed_SelectedIndexChanged);
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(5, 17);
			this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(126, 17);
			this.label20.TabIndex = 0;
			this.label20.Text = "Connection &speed:";
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.error.SetIconAlignment(this.txtName, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
			this.txtName.Location = new System.Drawing.Point(76, 34);
			this.txtName.Margin = new System.Windows.Forms.Padding(4);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(296, 22);
			this.txtName.TabIndex = 3;
			// 
			// tabMain
			// 
			this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabMain.Controls.Add(tabGeneral);
			this.tabMain.Controls.Add(tabDisplay);
			this.tabMain.Controls.Add(tabConnecion);
			this.tabMain.Controls.Add(tabPerfomance);
			this.tabMain.Controls.Add(tabLocal);
			this.tabMain.Controls.Add(tabApplication);
			this.tabMain.Controls.Add(tabGateway);
			this.tabMain.Location = new System.Drawing.Point(16, 66);
			this.tabMain.Margin = new System.Windows.Forms.Padding(4);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(357, 289);
			this.tabMain.TabIndex = 4;
			// 
			// error
			// 
			this.error.ContainerControl = this;
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnSave,
            this.tsdbIcon});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(389, 27);
			this.tsMain.TabIndex = 5;
			// 
			// tsbnSave
			// 
			this.tsbnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnSave.Image = global::Plugin.RDP.Properties.Resources.FileSave;
			this.tsbnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnSave.Name = "tsbnSave";
			this.tsbnSave.Size = new System.Drawing.Size(24, 24);
			this.tsbnSave.Click += new System.EventHandler(this.tsbnSave_Click);
			// 
			// tsdbIcon
			// 
			this.tsdbIcon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsdbIcon.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiIcon1,
            this.tsmiIcon2});
			this.tsdbIcon.Image = ((System.Drawing.Image)(resources.GetObject("tsdbIcon.Image")));
			this.tsdbIcon.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsdbIcon.Name = "tsdbIcon";
			this.tsdbIcon.Size = new System.Drawing.Size(34, 24);
			this.tsdbIcon.ToolTipText = "Client icon";
			this.tsdbIcon.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsdbIcon_DropDownItemClicked);
			// 
			// tsmiIcon2
			// 
			this.tsmiIcon2.Image = global::Plugin.RDP.Properties.Resources.iconRDP2Disconnected;
			this.tsmiIcon2.Name = "tsmiIcon2";
			this.tsmiIcon2.Size = new System.Drawing.Size(181, 26);
			this.tsmiIcon2.Text = "Icon2";
			// 
			// tsmiIcon1
			// 
			this.tsmiIcon1.Image = global::Plugin.RDP.Properties.Resources.iconClientDisconnected.ToBitmap();
			this.tsmiIcon1.Checked = true;
			this.tsmiIcon1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiIcon1.Name = "tsmiIcon1";
			this.tsmiIcon1.Size = new System.Drawing.Size(181, 26);
			this.tsmiIcon1.Text = "Icon1";
			// 
			// RdpClientDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.ClientSize = new System.Drawing.Size(389, 370);
			this.Controls.Add(this.tsMain);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.txtName);
			this.Controls.Add(label1);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(738, 542);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(338, 356);
			this.Name = "RdpClientDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create RDP client";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			tabGeneral.ResumeLayout(false);
			tabConnecion.ResumeLayout(false);
			tabConnecion.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.udMinutesToIdle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udPort)).EndInit();
			tabLocal.ResumeLayout(false);
			tabLocal.PerformLayout();
			gbRedirect.ResumeLayout(false);
			flowRedirect.ResumeLayout(false);
			flowRedirect.PerformLayout();
			tabApplication.ResumeLayout(false);
			tabApplication.PerformLayout();
			this.gbRunApplication.ResumeLayout(false);
			this.gbRunApplication.PerformLayout();
			tabDisplay.ResumeLayout(false);
			tabDisplay.PerformLayout();
			gbDesktopSize.ResumeLayout(false);
			this.flowDesktopSize.ResumeLayout(false);
			this.flowDesktopSize.PerformLayout();
			tabGateway.ResumeLayout(false);
			tabGateway.PerformLayout();
			this.gbGateway.ResumeLayout(false);
			this.gbGateway.PerformLayout();
			tabPerfomance.ResumeLayout(false);
			tabPerfomance.PerformLayout();
			this.tabMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.error)).EndInit();
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.ComboBox ddlComputer;
		private AlphaOmega.Windows.Forms.DefaultTextBox txtUser;
		private System.Windows.Forms.CheckBox cbConsole;
		private System.Windows.Forms.ErrorProvider error;
		private System.Windows.Forms.TextBox txtPassword;
		private AlphaOmega.Windows.Forms.DefaultTextBox txtDomain;
		private DefaultNumericUpDown udPort;
		private System.Windows.Forms.CheckBox cbRedirectSmartCards;
		private System.Windows.Forms.CheckBox cbRedirectPorts;
		private System.Windows.Forms.CheckBox cbRedirectClipboard;
		private System.Windows.Forms.CheckBox cbRedirectPrinters;
		private System.Windows.Forms.CheckBox cbRedirectDrives;
		private System.Windows.Forms.CheckBox cbRedirectPoS;
		private System.Windows.Forms.ComboBox ddlRedirectAudio;
		private System.Windows.Forms.CheckBox cbRunApplication;
		private System.Windows.Forms.GroupBox gbRunApplication;
		private System.Windows.Forms.TextBox txtRunApplicationFilePath;
		private System.Windows.Forms.TextBox txtRunApplicationWorkingDir;
		private System.Windows.Forms.ComboBox ddlAuthenticationLevel;
		private System.Windows.Forms.CheckBox cbRunApplicationMaximize;
		private System.Windows.Forms.NumericUpDown udMinutesToIdle;
		private System.Windows.Forms.CheckBox cbRedirectAudioCapture;
		private System.Windows.Forms.ComboBox ddlRedirectAudioQuality;
		private System.Windows.Forms.ComboBox ddlKeyboardHook;
		private System.Windows.Forms.ComboBox ddlColorDepth;
		private System.Windows.Forms.FlowLayoutPanel flowDesktopSize;
		private System.Windows.Forms.RadioButton rbSizeCustom;
		private System.Windows.Forms.CheckBox cbUseGateway;
		private System.Windows.Forms.TextBox txtGatewayDomain;
		private System.Windows.Forms.TextBox txtGatewayPassword;
		private System.Windows.Forms.TextBox txtGatewayUserName;
		private System.Windows.Forms.CheckBox cbGatewayShareAuth;
		private System.Windows.Forms.CheckBox cbGatewayBypass;
		private System.Windows.Forms.TextBox txtGatewayServer;
		private System.Windows.Forms.ComboBox ddlGatewayLogonMethod;
		private System.Windows.Forms.GroupBox gbGateway;
		private System.Windows.Forms.CheckBox cbDisableThemeing;
		private System.Windows.Forms.CheckBox cbDisableMenuAnimations;
		private System.Windows.Forms.CheckBox cbDisableFullWindowDrag;
		private System.Windows.Forms.CheckBox cbEnableDesktopComposition;
		private System.Windows.Forms.CheckBox cbEnableFontSmoothing;
		private System.Windows.Forms.CheckBox cbDisableWallpaper;
		private System.Windows.Forms.ComboBox ddlConnectionSpeed;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox txtVmConsoleId;
		private System.Windows.Forms.CheckBox cbVmConsole;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton tsbnSave;
		private System.Windows.Forms.ToolStripDropDownButton tsdbIcon;
		private System.Windows.Forms.ToolStripMenuItem tsmiIcon2;
		private System.Windows.Forms.ToolStripMenuItem tsmiIcon1;

	}
}