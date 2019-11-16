using System;

namespace OverloadClientTool
{
    partial class OCTMain
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OCTMain));
            this.OverloadExecutable = new System.Windows.Forms.TextBox();
            this.OverloadArgs = new System.Windows.Forms.TextBox();
            this.OlproxyExecutable = new System.Windows.Forms.TextBox();
            this.OlproxyArgs = new System.Windows.Forms.TextBox();
            this.SelectExecutable = new System.Windows.Forms.OpenFileDialog();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.MapUpdateButton = new System.Windows.Forms.Button();
            this.OverloadRunning = new System.Windows.Forms.PictureBox();
            this.OlproxyRunning = new System.Windows.Forms.PictureBox();
            this.UpdatingMaps = new System.Windows.Forms.PictureBox();
            this.OnlineMapJsonUrl = new System.Windows.Forms.TextBox();
            this.OverloadClientToolNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.PilotDeleteButton = new System.Windows.Forms.Button();
            this.PilotRenameButton = new System.Windows.Forms.Button();
            this.PilotCloneButton = new System.Windows.Forms.Button();
            this.PilotsBackupButton = new System.Windows.Forms.Button();
            this.PilotsListBox = new System.Windows.Forms.ListBox();
            this.MapDeleteButton = new System.Windows.Forms.Button();
            this.MapHideButton = new System.Windows.Forms.Button();
            this.PaneMain = new System.Windows.Forms.Panel();
            this.LogTreePanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.TreeViewLogPanel = new System.Windows.Forms.Panel();
            this.LogTreeView = new OverloadClientTool.CustomTreeView();
            this.StatusMessage = new System.Windows.Forms.Label();
            this.PaneSelectMain = new System.Windows.Forms.Button();
            this.PaneSelectMapManager = new System.Windows.Forms.Button();
            this.PaneSelectPilots = new System.Windows.Forms.Button();
            this.PaneSelectOverload = new System.Windows.Forms.Button();
            this.PaneMaps = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.MapsPanel = new System.Windows.Forms.Panel();
            this.MapsListBox = new OverloadClientTool.CustomListBox();
            this.CMMapsCheckBox = new OverloadClientTool.CustomCheckBox();
            this.SPMapsCheckBox = new OverloadClientTool.CustomCheckBox();
            this.MPMapsCheckBox = new OverloadClientTool.CustomCheckBox();
            this.HideUnofficialMapsCheckBox = new OverloadClientTool.CustomCheckBox();
            this.HideHiddenMapsCheckBox = new OverloadClientTool.CustomCheckBox();
            this.UseDLCLocationCheckBox = new OverloadClientTool.CustomCheckBox();
            this.AutoUpdateMapsCheckBox = new OverloadClientTool.CustomCheckBox();
            this.OnlyUpdateExistingMapsCheckBox = new OverloadClientTool.CustomCheckBox();
            this.MapUnhideAllButton = new System.Windows.Forms.Button();
            this.UnhideCMMapsButton = new System.Windows.Forms.Button();
            this.HideCMMapsButton = new System.Windows.Forms.Button();
            this.UnhideSPMapsButton = new System.Windows.Forms.Button();
            this.HideSPMapsButton = new System.Windows.Forms.Button();
            this.UnhideMPMapsButton = new System.Windows.Forms.Button();
            this.HideMPMapsButton = new System.Windows.Forms.Button();
            this.MapRefreshButton = new System.Windows.Forms.Button();
            this.PanePilots = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.PilotXPTextBox = new System.Windows.Forms.TextBox();
            this.PilotsPanel = new System.Windows.Forms.Panel();
            this.OpenPilotsBackupFolder = new System.Windows.Forms.LinkLabel();
            this.PilotNameLabel = new OverloadClientTool.TransparentLabel();
            this.PilotMakeActiveButton = new System.Windows.Forms.Button();
            this.AutoPilotsBackupCheckbox = new OverloadClientTool.CustomCheckBox();
            this.PilotXPSetButton = new System.Windows.Forms.Button();
            this.PaneSelectOlproxy = new System.Windows.Forms.Button();
            this.PaneSelectOlmod = new System.Windows.Forms.Button();
            this.PaneOlproxy = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.OlproxyReleases = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.StartStopOlproxyButton = new System.Windows.Forms.Button();
            this.UseEmbeddedOlproxy = new OverloadClientTool.CustomCheckBox();
            this.UseOlproxyCheckBox = new OverloadClientTool.CustomCheckBox();
            this.PaneOverload = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.OverloadLog = new System.Windows.Forms.LinkLabel();
            this.PLayOverloadLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchOverloadButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PaneOlmod = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.OlmodExecutable = new System.Windows.Forms.TextBox();
            this.OlmodReleases = new System.Windows.Forms.LinkLabel();
            this.UpdateOlmodButton = new System.Windows.Forms.Button();
            this.FrameTimeCheckBox = new OverloadClientTool.CustomCheckBox();
            this.UseOlmodGameDirArg = new OverloadClientTool.CustomCheckBox();
            this.AutoUpdateOlmod = new OverloadClientTool.CustomCheckBox();
            this.UseOlmodCheckBox = new OverloadClientTool.CustomCheckBox();
            this.PaneButtonLine = new System.Windows.Forms.Panel();
            this.PaneSelectOptions = new System.Windows.Forms.Button();
            this.PaneOptions = new System.Windows.Forms.Panel();
            this.ForceUpdateButton = new System.Windows.Forms.Button();
            this.ThemeDescriptionLabel = new System.Windows.Forms.Label();
            this.ActiveThemePanel = new System.Windows.Forms.Panel();
            this.AvailableThemesListBox = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.MinimizeOnStartupCheckBox = new OverloadClientTool.CustomCheckBox();
            this.UseTrayIcon = new OverloadClientTool.CustomCheckBox();
            this.PayPalLink = new System.Windows.Forms.LinkLabel();
            this.DisplayHelpLink = new System.Windows.Forms.LinkLabel();
            this.MailLinkLabel = new System.Windows.Forms.LinkLabel();
            this.DebugFileNameLink = new System.Windows.Forms.LinkLabel();
            this.PartyModeCheckBox = new OverloadClientTool.CustomCheckBox();
            this.EnableDebugCheckBox = new OverloadClientTool.CustomCheckBox();
            this.AutoUpdateCheckBox = new OverloadClientTool.CustomCheckBox();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ServerAnnounceOnTrackerCheckBox = new OverloadClientTool.CustomCheckBox();
            this.ServerAutoSignOffTracker = new OverloadClientTool.CustomCheckBox();
            this.AutoStartCheckBox = new OverloadClientTool.CustomCheckBox();
            this.PaneSelectServer = new System.Windows.Forms.Button();
            this.PaneServer = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.ServerTrackerName = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.ServerTrackerNotes = new System.Windows.Forms.TextBox();
            this.StartServerButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.ServerTrackerUrl = new System.Windows.Forms.TextBox();
            this.ServerRunning = new System.Windows.Forms.PictureBox();
            this.PaneOnline = new System.Windows.Forms.Panel();
            this.UpdateServerListButton = new System.Windows.Forms.Button();
            this.ServersPanel = new System.Windows.Forms.Panel();
            this.ServersListBox = new System.Windows.Forms.ListBox();
            this.label19 = new System.Windows.Forms.Label();
            this.PaneSelectOnline = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OverloadRunning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OlproxyRunning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdatingMaps)).BeginInit();
            this.PaneMain.SuspendLayout();
            this.LogTreePanel.SuspendLayout();
            this.TreeViewLogPanel.SuspendLayout();
            this.PaneMaps.SuspendLayout();
            this.panel7.SuspendLayout();
            this.MapsPanel.SuspendLayout();
            this.PanePilots.SuspendLayout();
            this.panel9.SuspendLayout();
            this.PilotsPanel.SuspendLayout();
            this.PaneOlproxy.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel2.SuspendLayout();
            this.PaneOverload.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.PaneOlmod.SuspendLayout();
            this.panel5.SuspendLayout();
            this.PaneOptions.SuspendLayout();
            this.ActiveThemePanel.SuspendLayout();
            this.PaneServer.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServerRunning)).BeginInit();
            this.PaneOnline.SuspendLayout();
            this.ServersPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OverloadExecutable
            // 
            this.OverloadExecutable.BackColor = System.Drawing.Color.Gray;
            this.OverloadExecutable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OverloadExecutable.Location = new System.Drawing.Point(10, 19);
            this.OverloadExecutable.Margin = new System.Windows.Forms.Padding(1);
            this.OverloadExecutable.Name = "OverloadExecutable";
            this.OverloadExecutable.Size = new System.Drawing.Size(478, 20);
            this.OverloadExecutable.TabIndex = 1;
            this.OverloadExecutable.TextChanged += new System.EventHandler(this.OverloadExecutable_TextChanged);
            this.OverloadExecutable.DoubleClick += new System.EventHandler(this.OverloadExecutable_MouseDoubleClick);
            // 
            // OverloadArgs
            // 
            this.OverloadArgs.BackColor = System.Drawing.Color.Gray;
            this.OverloadArgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OverloadArgs.Location = new System.Drawing.Point(10, 20);
            this.OverloadArgs.Margin = new System.Windows.Forms.Padding(2);
            this.OverloadArgs.Name = "OverloadArgs";
            this.OverloadArgs.Size = new System.Drawing.Size(478, 20);
            this.OverloadArgs.TabIndex = 2;
            this.OverloadArgs.TextChanged += new System.EventHandler(this.OverloadArgs_TextChanged);
            // 
            // OlproxyExecutable
            // 
            this.OlproxyExecutable.BackColor = System.Drawing.Color.Gray;
            this.OlproxyExecutable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OlproxyExecutable.Location = new System.Drawing.Point(6, 17);
            this.OlproxyExecutable.Margin = new System.Windows.Forms.Padding(4);
            this.OlproxyExecutable.Name = "OlproxyExecutable";
            this.OlproxyExecutable.Size = new System.Drawing.Size(501, 20);
            this.OlproxyExecutable.TabIndex = 3;
            this.OlproxyExecutable.TextChanged += new System.EventHandler(this.OlproxyExecutable_TextChanged);
            this.OlproxyExecutable.DoubleClick += new System.EventHandler(this.OlproxyExecutable_DoubleClick);
            // 
            // OlproxyArgs
            // 
            this.OlproxyArgs.BackColor = System.Drawing.Color.Gray;
            this.OlproxyArgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OlproxyArgs.Location = new System.Drawing.Point(6, 17);
            this.OlproxyArgs.Margin = new System.Windows.Forms.Padding(2);
            this.OlproxyArgs.Name = "OlproxyArgs";
            this.OlproxyArgs.Size = new System.Drawing.Size(501, 20);
            this.OlproxyArgs.TabIndex = 4;
            this.OlproxyArgs.TextChanged += new System.EventHandler(this.OlproxyArgs_TextChanged);
            // 
            // SelectExecutable
            // 
            this.SelectExecutable.FileName = "SelectExecutable";
            this.SelectExecutable.Filter = "Applications|*.exe";
            // 
            // StartStopButton
            // 
            this.StartStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartStopButton.Location = new System.Drawing.Point(405, 296);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(56, 24);
            this.StartStopButton.TabIndex = 9;
            this.StartStopButton.Text = "Start";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Location = new System.Drawing.Point(467, 296);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(60, 24);
            this.ExitButton.TabIndex = 9;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // MapUpdateButton
            // 
            this.MapUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapUpdateButton.Location = new System.Drawing.Point(301, 285);
            this.MapUpdateButton.Name = "MapUpdateButton";
            this.MapUpdateButton.Size = new System.Drawing.Size(106, 24);
            this.MapUpdateButton.TabIndex = 9;
            this.MapUpdateButton.Text = "Update maps now";
            this.MapUpdateButton.UseVisualStyleBackColor = true;
            this.MapUpdateButton.Click += new System.EventHandler(this.MapUpdateButton_Click);
            // 
            // OverloadRunning
            // 
            this.OverloadRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.OverloadRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
            this.OverloadRunning.Location = new System.Drawing.Point(17, 299);
            this.OverloadRunning.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.OverloadRunning.Name = "OverloadRunning";
            this.OverloadRunning.Size = new System.Drawing.Size(22, 21);
            this.OverloadRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.OverloadRunning.TabIndex = 10;
            this.OverloadRunning.TabStop = false;
            this.OverloadRunning.Visible = false;
            // 
            // OlproxyRunning
            // 
            this.OlproxyRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.OlproxyRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
            this.OlproxyRunning.Location = new System.Drawing.Point(74, 297);
            this.OlproxyRunning.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.OlproxyRunning.Name = "OlproxyRunning";
            this.OlproxyRunning.Size = new System.Drawing.Size(22, 21);
            this.OlproxyRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.OlproxyRunning.TabIndex = 10;
            this.OlproxyRunning.TabStop = false;
            this.OlproxyRunning.Visible = false;
            // 
            // UpdatingMaps
            // 
            this.UpdatingMaps.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.UpdatingMaps.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
            this.UpdatingMaps.Location = new System.Drawing.Point(411, 289);
            this.UpdatingMaps.Name = "UpdatingMaps";
            this.UpdatingMaps.Size = new System.Drawing.Size(18, 18);
            this.UpdatingMaps.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.UpdatingMaps.TabIndex = 10;
            this.UpdatingMaps.TabStop = false;
            this.UpdatingMaps.Visible = false;
            // 
            // OnlineMapJsonUrl
            // 
            this.OnlineMapJsonUrl.BackColor = System.Drawing.Color.Gray;
            this.OnlineMapJsonUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OnlineMapJsonUrl.Location = new System.Drawing.Point(7, 19);
            this.OnlineMapJsonUrl.Margin = new System.Windows.Forms.Padding(1);
            this.OnlineMapJsonUrl.Name = "OnlineMapJsonUrl";
            this.OnlineMapJsonUrl.Size = new System.Drawing.Size(257, 20);
            this.OnlineMapJsonUrl.TabIndex = 1;
            this.OnlineMapJsonUrl.Text = "https://www.overloadmaps.com/data/mp.json";
            this.OnlineMapJsonUrl.TextChanged += new System.EventHandler(this.OnlineMapJsonUrl_TextChanged);
            this.OnlineMapJsonUrl.DoubleClick += new System.EventHandler(this.OverloadExecutable_MouseDoubleClick);
            // 
            // OverloadClientToolNotifyIcon
            // 
            this.OverloadClientToolNotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.OverloadClientToolNotifyIcon.BalloonTipText = "Have great fun!";
            this.OverloadClientToolNotifyIcon.BalloonTipTitle = "Overload Client Tool";
            this.OverloadClientToolNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("OverloadClientToolNotifyIcon.Icon")));
            this.OverloadClientToolNotifyIcon.Text = "Overload Client Tool";
            this.OverloadClientToolNotifyIcon.Visible = true;
            this.OverloadClientToolNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OverloadClientToolNotifyIcon_MouseDoubleClick);
            // 
            // PilotDeleteButton
            // 
            this.PilotDeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PilotDeleteButton.Location = new System.Drawing.Point(220, 82);
            this.PilotDeleteButton.Name = "PilotDeleteButton";
            this.PilotDeleteButton.Size = new System.Drawing.Size(81, 23);
            this.PilotDeleteButton.TabIndex = 14;
            this.PilotDeleteButton.Text = "Delete";
            this.PilotDeleteButton.UseVisualStyleBackColor = true;
            this.PilotDeleteButton.Click += new System.EventHandler(this.PilotDeleteButton_Click);
            // 
            // PilotRenameButton
            // 
            this.PilotRenameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PilotRenameButton.Location = new System.Drawing.Point(220, 53);
            this.PilotRenameButton.Name = "PilotRenameButton";
            this.PilotRenameButton.Size = new System.Drawing.Size(81, 23);
            this.PilotRenameButton.TabIndex = 14;
            this.PilotRenameButton.Text = "Rename";
            this.PilotRenameButton.UseVisualStyleBackColor = true;
            this.PilotRenameButton.Click += new System.EventHandler(this.PilotRenameButton_Click);
            // 
            // PilotCloneButton
            // 
            this.PilotCloneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PilotCloneButton.Location = new System.Drawing.Point(220, 24);
            this.PilotCloneButton.Name = "PilotCloneButton";
            this.PilotCloneButton.Size = new System.Drawing.Size(81, 23);
            this.PilotCloneButton.TabIndex = 14;
            this.PilotCloneButton.Text = "Clone";
            this.PilotCloneButton.UseVisualStyleBackColor = true;
            this.PilotCloneButton.Click += new System.EventHandler(this.PilotCloneButton_Click);
            // 
            // PilotsBackupButton
            // 
            this.PilotsBackupButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PilotsBackupButton.Location = new System.Drawing.Point(220, 208);
            this.PilotsBackupButton.Name = "PilotsBackupButton";
            this.PilotsBackupButton.Size = new System.Drawing.Size(81, 23);
            this.PilotsBackupButton.TabIndex = 14;
            this.PilotsBackupButton.Text = "Backup pilots";
            this.PilotsBackupButton.UseVisualStyleBackColor = true;
            this.PilotsBackupButton.Click += new System.EventHandler(this.PilotBackupButton_Click);
            // 
            // PilotsListBox
            // 
            this.PilotsListBox.BackColor = System.Drawing.Color.Gray;
            this.PilotsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PilotsListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.PilotsListBox.FormattingEnabled = true;
            this.PilotsListBox.Location = new System.Drawing.Point(1, 1);
            this.PilotsListBox.Name = "PilotsListBox";
            this.PilotsListBox.Size = new System.Drawing.Size(191, 182);
            this.PilotsListBox.TabIndex = 0;
            this.PilotsListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.PilotsListBox_DrawItem);
            this.PilotsListBox.SelectedIndexChanged += new System.EventHandler(this.PilotsListBox_SelectedIndexChanged);
            // 
            // MapDeleteButton
            // 
            this.MapDeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapDeleteButton.Location = new System.Drawing.Point(162, 54);
            this.MapDeleteButton.Name = "MapDeleteButton";
            this.MapDeleteButton.Size = new System.Drawing.Size(71, 23);
            this.MapDeleteButton.TabIndex = 14;
            this.MapDeleteButton.Text = "Delete";
            this.MapDeleteButton.UseVisualStyleBackColor = true;
            this.MapDeleteButton.Click += new System.EventHandler(this.MapDelete_Click);
            // 
            // MapHideButton
            // 
            this.MapHideButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapHideButton.Location = new System.Drawing.Point(162, 25);
            this.MapHideButton.Name = "MapHideButton";
            this.MapHideButton.Size = new System.Drawing.Size(71, 23);
            this.MapHideButton.TabIndex = 14;
            this.MapHideButton.Text = "Hide";
            this.MapHideButton.UseVisualStyleBackColor = true;
            this.MapHideButton.Click += new System.EventHandler(this.MapHideButton_Click);
            // 
            // PaneMain
            // 
            this.PaneMain.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneMain.Controls.Add(this.LogTreePanel);
            this.PaneMain.Controls.Add(this.StatusMessage);
            this.PaneMain.Controls.Add(this.StartStopButton);
            this.PaneMain.Controls.Add(this.OverloadRunning);
            this.PaneMain.Controls.Add(this.ExitButton);
            this.PaneMain.Location = new System.Drawing.Point(9, 43);
            this.PaneMain.Margin = new System.Windows.Forms.Padding(0);
            this.PaneMain.Name = "PaneMain";
            this.PaneMain.Size = new System.Drawing.Size(547, 336);
            this.PaneMain.TabIndex = 18;
            // 
            // LogTreePanel
            // 
            this.LogTreePanel.Controls.Add(this.label12);
            this.LogTreePanel.Controls.Add(this.TreeViewLogPanel);
            this.LogTreePanel.Location = new System.Drawing.Point(3, 5);
            this.LogTreePanel.Name = "LogTreePanel";
            this.LogTreePanel.Size = new System.Drawing.Size(541, 288);
            this.LogTreePanel.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Activity Log";
            // 
            // TreeViewLogPanel
            // 
            this.TreeViewLogPanel.BackColor = System.Drawing.Color.Blue;
            this.TreeViewLogPanel.Controls.Add(this.LogTreeView);
            this.TreeViewLogPanel.Location = new System.Drawing.Point(7, 18);
            this.TreeViewLogPanel.Name = "TreeViewLogPanel";
            this.TreeViewLogPanel.Size = new System.Drawing.Size(517, 265);
            this.TreeViewLogPanel.TabIndex = 19;
            // 
            // LogTreeView
            // 
            this.LogTreeView.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LogTreeView.BackColor = System.Drawing.Color.Gray;
            this.LogTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.LogTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogTreeView.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.LogTreeView.FullRowSelect = true;
            this.LogTreeView.ItemHeight = 14;
            this.LogTreeView.Location = new System.Drawing.Point(1, 1);
            this.LogTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.LogTreeView.Name = "LogTreeView";
            this.LogTreeView.ShowLines = false;
            this.LogTreeView.ShowPlusMinus = false;
            this.LogTreeView.ShowRootLines = false;
            this.LogTreeView.Size = new System.Drawing.Size(515, 263);
            this.LogTreeView.TabIndex = 19;
            this.LogTreeView.TabStop = false;
            this.LogTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.LogTreeView_DrawNode);
            // 
            // StatusMessage
            // 
            this.StatusMessage.AutoSize = true;
            this.StatusMessage.Location = new System.Drawing.Point(38, 302);
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Size = new System.Drawing.Size(101, 13);
            this.StatusMessage.TabIndex = 15;
            this.StatusMessage.Text = "StatusMessageText";
            // 
            // PaneSelectMain
            // 
            this.PaneSelectMain.BackColor = System.Drawing.Color.SteelBlue;
            this.PaneSelectMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectMain.FlatAppearance.BorderSize = 0;
            this.PaneSelectMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectMain.ForeColor = System.Drawing.Color.White;
            this.PaneSelectMain.Location = new System.Drawing.Point(0, 0);
            this.PaneSelectMain.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectMain.Name = "PaneSelectMain";
            this.PaneSelectMain.Size = new System.Drawing.Size(50, 23);
            this.PaneSelectMain.TabIndex = 0;
            this.PaneSelectMain.TabStop = false;
            this.PaneSelectMain.Text = "  Main";
            this.PaneSelectMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectMain.UseVisualStyleBackColor = false;
            // 
            // PaneSelectMapManager
            // 
            this.PaneSelectMapManager.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectMapManager.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectMapManager.FlatAppearance.BorderSize = 0;
            this.PaneSelectMapManager.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectMapManager.ForeColor = System.Drawing.Color.White;
            this.PaneSelectMapManager.Location = new System.Drawing.Point(50, 0);
            this.PaneSelectMapManager.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectMapManager.Name = "PaneSelectMapManager";
            this.PaneSelectMapManager.Size = new System.Drawing.Size(54, 23);
            this.PaneSelectMapManager.TabIndex = 0;
            this.PaneSelectMapManager.TabStop = false;
            this.PaneSelectMapManager.Text = "  Maps";
            this.PaneSelectMapManager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectMapManager.UseVisualStyleBackColor = false;
            // 
            // PaneSelectPilots
            // 
            this.PaneSelectPilots.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectPilots.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectPilots.FlatAppearance.BorderSize = 0;
            this.PaneSelectPilots.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectPilots.ForeColor = System.Drawing.Color.White;
            this.PaneSelectPilots.Location = new System.Drawing.Point(104, 0);
            this.PaneSelectPilots.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectPilots.Name = "PaneSelectPilots";
            this.PaneSelectPilots.Size = new System.Drawing.Size(52, 23);
            this.PaneSelectPilots.TabIndex = 0;
            this.PaneSelectPilots.TabStop = false;
            this.PaneSelectPilots.Text = "  Pilots";
            this.PaneSelectPilots.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectPilots.UseVisualStyleBackColor = false;
            // 
            // PaneSelectOverload
            // 
            this.PaneSelectOverload.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectOverload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectOverload.FlatAppearance.BorderSize = 0;
            this.PaneSelectOverload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectOverload.ForeColor = System.Drawing.Color.White;
            this.PaneSelectOverload.Location = new System.Drawing.Point(156, 0);
            this.PaneSelectOverload.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectOverload.Name = "PaneSelectOverload";
            this.PaneSelectOverload.Size = new System.Drawing.Size(68, 23);
            this.PaneSelectOverload.TabIndex = 0;
            this.PaneSelectOverload.TabStop = false;
            this.PaneSelectOverload.Text = "  Overload";
            this.PaneSelectOverload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectOverload.UseVisualStyleBackColor = false;
            // 
            // PaneMaps
            // 
            this.PaneMaps.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneMaps.Controls.Add(this.label17);
            this.PaneMaps.Controls.Add(this.label6);
            this.PaneMaps.Controls.Add(this.panel7);
            this.PaneMaps.Controls.Add(this.MapsPanel);
            this.PaneMaps.Controls.Add(this.CMMapsCheckBox);
            this.PaneMaps.Controls.Add(this.SPMapsCheckBox);
            this.PaneMaps.Controls.Add(this.MPMapsCheckBox);
            this.PaneMaps.Controls.Add(this.HideUnofficialMapsCheckBox);
            this.PaneMaps.Controls.Add(this.HideHiddenMapsCheckBox);
            this.PaneMaps.Controls.Add(this.UseDLCLocationCheckBox);
            this.PaneMaps.Controls.Add(this.MapUpdateButton);
            this.PaneMaps.Controls.Add(this.AutoUpdateMapsCheckBox);
            this.PaneMaps.Controls.Add(this.UpdatingMaps);
            this.PaneMaps.Controls.Add(this.OnlyUpdateExistingMapsCheckBox);
            this.PaneMaps.Controls.Add(this.MapUnhideAllButton);
            this.PaneMaps.Controls.Add(this.MapHideButton);
            this.PaneMaps.Controls.Add(this.UnhideCMMapsButton);
            this.PaneMaps.Controls.Add(this.HideCMMapsButton);
            this.PaneMaps.Controls.Add(this.UnhideSPMapsButton);
            this.PaneMaps.Controls.Add(this.HideSPMapsButton);
            this.PaneMaps.Controls.Add(this.UnhideMPMapsButton);
            this.PaneMaps.Controls.Add(this.HideMPMapsButton);
            this.PaneMaps.Controls.Add(this.MapRefreshButton);
            this.PaneMaps.Controls.Add(this.MapDeleteButton);
            this.PaneMaps.Location = new System.Drawing.Point(9, 393);
            this.PaneMaps.Margin = new System.Windows.Forms.Padding(0);
            this.PaneMaps.Name = "PaneMaps";
            this.PaneMaps.Size = new System.Drawing.Size(547, 336);
            this.PaneMaps.TabIndex = 19;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(321, 177);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(195, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Include these map types when updating";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Installed maps";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label7);
            this.panel7.Controls.Add(this.OnlineMapJsonUrl);
            this.panel7.Location = new System.Drawing.Point(17, 270);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(278, 47);
            this.panel7.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "URL to online JSON map list";
            // 
            // MapsPanel
            // 
            this.MapsPanel.BackColor = System.Drawing.Color.Blue;
            this.MapsPanel.Controls.Add(this.MapsListBox);
            this.MapsPanel.Location = new System.Drawing.Point(16, 25);
            this.MapsPanel.Name = "MapsPanel";
            this.MapsPanel.Size = new System.Drawing.Size(138, 236);
            this.MapsPanel.TabIndex = 19;
            // 
            // MapsListBox
            // 
            this.MapsListBox.BackColor = System.Drawing.Color.Gray;
            this.MapsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MapsListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.MapsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MapsListBox.FormattingEnabled = true;
            this.MapsListBox.Location = new System.Drawing.Point(1, 1);
            this.MapsListBox.Name = "MapsListBox";
            this.MapsListBox.Size = new System.Drawing.Size(136, 234);
            this.MapsListBox.TabIndex = 0;
            this.MapsListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.MapsListBox_DrawItem);
            this.MapsListBox.SelectedIndexChanged += new System.EventHandler(this.MapsListBox_SelectedIndexChanged);
            this.MapsListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapsListBox_MouseMove);
            // 
            // CMMapsCheckBox
            // 
            this.CMMapsCheckBox.AutoSize = true;
            this.CMMapsCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.CMMapsCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.CMMapsCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.CMMapsCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.CMMapsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CMMapsCheckBox.Location = new System.Drawing.Point(324, 243);
            this.CMMapsCheckBox.Name = "CMMapsCheckBox";
            this.CMMapsCheckBox.Size = new System.Drawing.Size(135, 17);
            this.CMMapsCheckBox.TabIndex = 5;
            this.CMMapsCheckBox.Text = "Challenge mission maps";
            this.MainToolTip.SetToolTip(this.CMMapsCheckBox, "Select this to update challenge mission maps");
            this.CMMapsCheckBox.UseVisualStyleBackColor = false;
            this.CMMapsCheckBox.CheckedChanged += new System.EventHandler(this.CMMapsCheckBox_CheckedChanged);
            // 
            // SPMapsCheckBox
            // 
            this.SPMapsCheckBox.AutoSize = true;
            this.SPMapsCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.SPMapsCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.SPMapsCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.SPMapsCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.SPMapsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SPMapsCheckBox.Location = new System.Drawing.Point(324, 220);
            this.SPMapsCheckBox.Name = "SPMapsCheckBox";
            this.SPMapsCheckBox.Size = new System.Drawing.Size(111, 17);
            this.SPMapsCheckBox.TabIndex = 5;
            this.SPMapsCheckBox.Text = "Single player maps";
            this.MainToolTip.SetToolTip(this.SPMapsCheckBox, "Select this to update single player maps");
            this.SPMapsCheckBox.UseVisualStyleBackColor = false;
            this.SPMapsCheckBox.CheckedChanged += new System.EventHandler(this.SPMapsCheckBox_CheckedChanged);
            // 
            // MPMapsCheckBox
            // 
            this.MPMapsCheckBox.AutoSize = true;
            this.MPMapsCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.MPMapsCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.MPMapsCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.MPMapsCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.MPMapsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MPMapsCheckBox.Location = new System.Drawing.Point(324, 197);
            this.MPMapsCheckBox.Name = "MPMapsCheckBox";
            this.MPMapsCheckBox.Size = new System.Drawing.Size(101, 17);
            this.MPMapsCheckBox.TabIndex = 5;
            this.MPMapsCheckBox.Text = "Multiplayer maps";
            this.MainToolTip.SetToolTip(this.MPMapsCheckBox, "Select this to update multiplayer maps");
            this.MPMapsCheckBox.UseVisualStyleBackColor = false;
            this.MPMapsCheckBox.CheckedChanged += new System.EventHandler(this.MpMapsCheckBox_CheckedChanged);
            // 
            // HideUnofficialMapsCheckBox
            // 
            this.HideUnofficialMapsCheckBox.AutoSize = true;
            this.HideUnofficialMapsCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.HideUnofficialMapsCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.HideUnofficialMapsCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.HideUnofficialMapsCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.HideUnofficialMapsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HideUnofficialMapsCheckBox.Location = new System.Drawing.Point(324, 63);
            this.HideUnofficialMapsCheckBox.Name = "HideUnofficialMapsCheckBox";
            this.HideUnofficialMapsCheckBox.Size = new System.Drawing.Size(192, 17);
            this.HideUnofficialMapsCheckBox.TabIndex = 5;
            this.HideUnofficialMapsCheckBox.Text = "When updating hide unofficial maps";
            this.MainToolTip.SetToolTip(this.HideUnofficialMapsCheckBox, "Select this to hide maps that are not in the official map list");
            this.HideUnofficialMapsCheckBox.UseVisualStyleBackColor = false;
            this.HideUnofficialMapsCheckBox.CheckedChanged += new System.EventHandler(this.HideUnofficialMapsCheckBox_CheckedChanged);
            // 
            // HideHiddenMapsCheckBox
            // 
            this.HideHiddenMapsCheckBox.AutoSize = true;
            this.HideHiddenMapsCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.HideHiddenMapsCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.HideHiddenMapsCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.HideHiddenMapsCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.HideHiddenMapsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HideHiddenMapsCheckBox.Location = new System.Drawing.Point(324, 108);
            this.HideHiddenMapsCheckBox.Name = "HideHiddenMapsCheckBox";
            this.HideHiddenMapsCheckBox.Size = new System.Drawing.Size(139, 17);
            this.HideHiddenMapsCheckBox.TabIndex = 5;
            this.HideHiddenMapsCheckBox.Text = "Don\'t show hidden maps";
            this.HideHiddenMapsCheckBox.UseVisualStyleBackColor = false;
            this.HideHiddenMapsCheckBox.CheckedChanged += new System.EventHandler(this.HideHiddenMapsCheckBox_CheckedChanged);
            // 
            // UseDLCLocationCheckBox
            // 
            this.UseDLCLocationCheckBox.AutoCheck = false;
            this.UseDLCLocationCheckBox.AutoSize = true;
            this.UseDLCLocationCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.UseDLCLocationCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.UseDLCLocationCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.UseDLCLocationCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.UseDLCLocationCheckBox.Enabled = false;
            this.UseDLCLocationCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseDLCLocationCheckBox.Location = new System.Drawing.Point(324, 86);
            this.UseDLCLocationCheckBox.Name = "UseDLCLocationCheckBox";
            this.UseDLCLocationCheckBox.Size = new System.Drawing.Size(199, 17);
            this.UseDLCLocationCheckBox.TabIndex = 5;
            this.UseDLCLocationCheckBox.Text = "Use DLC folder for downloaded maps";
            this.MainToolTip.SetToolTip(this.UseDLCLocationCheckBox, "Save downloaded map ZIP files to Overload DLC folder");
            this.UseDLCLocationCheckBox.UseVisualStyleBackColor = false;
            this.UseDLCLocationCheckBox.Click += new System.EventHandler(this.UseDLCLocationCheckBox_Click);
            // 
            // AutoUpdateMapsCheckBox
            // 
            this.AutoUpdateMapsCheckBox.AutoSize = true;
            this.AutoUpdateMapsCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.AutoUpdateMapsCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.AutoUpdateMapsCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.AutoUpdateMapsCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.AutoUpdateMapsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoUpdateMapsCheckBox.Location = new System.Drawing.Point(324, 17);
            this.AutoUpdateMapsCheckBox.Name = "AutoUpdateMapsCheckBox";
            this.AutoUpdateMapsCheckBox.Size = new System.Drawing.Size(146, 17);
            this.AutoUpdateMapsCheckBox.TabIndex = 5;
            this.AutoUpdateMapsCheckBox.Text = "Update all maps at startup";
            this.MainToolTip.SetToolTip(this.AutoUpdateMapsCheckBox, "Do an update of all maps at startup (may take a bit of extra time)");
            this.AutoUpdateMapsCheckBox.UseVisualStyleBackColor = false;
            this.AutoUpdateMapsCheckBox.Click += new System.EventHandler(this.AutoUpdateMaps_Click);
            // 
            // OnlyUpdateExistingMapsCheckBox
            // 
            this.OnlyUpdateExistingMapsCheckBox.AutoSize = true;
            this.OnlyUpdateExistingMapsCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.OnlyUpdateExistingMapsCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.OnlyUpdateExistingMapsCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.OnlyUpdateExistingMapsCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.OnlyUpdateExistingMapsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OnlyUpdateExistingMapsCheckBox.Location = new System.Drawing.Point(324, 39);
            this.OnlyUpdateExistingMapsCheckBox.Name = "OnlyUpdateExistingMapsCheckBox";
            this.OnlyUpdateExistingMapsCheckBox.Size = new System.Drawing.Size(146, 17);
            this.OnlyUpdateExistingMapsCheckBox.TabIndex = 5;
            this.OnlyUpdateExistingMapsCheckBox.Text = "Only update existing maps";
            this.MainToolTip.SetToolTip(this.OnlyUpdateExistingMapsCheckBox, "Only update map ZIP files already on disk");
            this.OnlyUpdateExistingMapsCheckBox.UseVisualStyleBackColor = false;
            this.OnlyUpdateExistingMapsCheckBox.CheckedChanged += new System.EventHandler(this.OnlyUpdateExistingMapsCheckBox_CheckedChanged);
            // 
            // MapUnhideAllButton
            // 
            this.MapUnhideAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapUnhideAllButton.Location = new System.Drawing.Point(162, 237);
            this.MapUnhideAllButton.Name = "MapUnhideAllButton";
            this.MapUnhideAllButton.Size = new System.Drawing.Size(71, 23);
            this.MapUnhideAllButton.TabIndex = 14;
            this.MapUnhideAllButton.Text = "Unhide all";
            this.MapUnhideAllButton.UseVisualStyleBackColor = true;
            this.MapUnhideAllButton.Click += new System.EventHandler(this.MapUnhideAllButton_Click);
            // 
            // UnhideCMMapsButton
            // 
            this.UnhideCMMapsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnhideCMMapsButton.Location = new System.Drawing.Point(238, 193);
            this.UnhideCMMapsButton.Name = "UnhideCMMapsButton";
            this.UnhideCMMapsButton.Size = new System.Drawing.Size(71, 23);
            this.UnhideCMMapsButton.TabIndex = 14;
            this.UnhideCMMapsButton.Text = "Unhide CM";
            this.UnhideCMMapsButton.UseVisualStyleBackColor = true;
            this.UnhideCMMapsButton.Click += new System.EventHandler(this.UnhideCMMapsButton_Click);
            // 
            // HideCMMapsButton
            // 
            this.HideCMMapsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HideCMMapsButton.Location = new System.Drawing.Point(162, 193);
            this.HideCMMapsButton.Name = "HideCMMapsButton";
            this.HideCMMapsButton.Size = new System.Drawing.Size(71, 23);
            this.HideCMMapsButton.TabIndex = 14;
            this.HideCMMapsButton.Text = "Hide CM";
            this.HideCMMapsButton.UseVisualStyleBackColor = true;
            this.HideCMMapsButton.Click += new System.EventHandler(this.HideCMMapsButton_Click);
            // 
            // UnhideSPMapsButton
            // 
            this.UnhideSPMapsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnhideSPMapsButton.Location = new System.Drawing.Point(238, 166);
            this.UnhideSPMapsButton.Name = "UnhideSPMapsButton";
            this.UnhideSPMapsButton.Size = new System.Drawing.Size(71, 23);
            this.UnhideSPMapsButton.TabIndex = 14;
            this.UnhideSPMapsButton.Text = "Unhide SP";
            this.UnhideSPMapsButton.UseVisualStyleBackColor = true;
            this.UnhideSPMapsButton.Click += new System.EventHandler(this.UnhideSPMapsButton_Click);
            // 
            // HideSPMapsButton
            // 
            this.HideSPMapsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HideSPMapsButton.Location = new System.Drawing.Point(162, 166);
            this.HideSPMapsButton.Name = "HideSPMapsButton";
            this.HideSPMapsButton.Size = new System.Drawing.Size(71, 23);
            this.HideSPMapsButton.TabIndex = 14;
            this.HideSPMapsButton.Text = "Hide SP";
            this.HideSPMapsButton.UseVisualStyleBackColor = true;
            this.HideSPMapsButton.Click += new System.EventHandler(this.HideSPMapsButton_Click);
            // 
            // UnhideMPMapsButton
            // 
            this.UnhideMPMapsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UnhideMPMapsButton.Location = new System.Drawing.Point(238, 139);
            this.UnhideMPMapsButton.Name = "UnhideMPMapsButton";
            this.UnhideMPMapsButton.Size = new System.Drawing.Size(71, 23);
            this.UnhideMPMapsButton.TabIndex = 14;
            this.UnhideMPMapsButton.Text = "Unhide MP";
            this.UnhideMPMapsButton.UseVisualStyleBackColor = true;
            this.UnhideMPMapsButton.Click += new System.EventHandler(this.UnhideMPMapsButton_Click);
            // 
            // HideMPMapsButton
            // 
            this.HideMPMapsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HideMPMapsButton.Location = new System.Drawing.Point(162, 139);
            this.HideMPMapsButton.Name = "HideMPMapsButton";
            this.HideMPMapsButton.Size = new System.Drawing.Size(71, 23);
            this.HideMPMapsButton.TabIndex = 14;
            this.HideMPMapsButton.Text = "Hide MP";
            this.HideMPMapsButton.UseVisualStyleBackColor = true;
            this.HideMPMapsButton.Click += new System.EventHandler(this.HideMPMapsButton_Click);
            // 
            // MapRefreshButton
            // 
            this.MapRefreshButton.Enabled = false;
            this.MapRefreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapRefreshButton.Location = new System.Drawing.Point(162, 83);
            this.MapRefreshButton.Name = "MapRefreshButton";
            this.MapRefreshButton.Size = new System.Drawing.Size(71, 23);
            this.MapRefreshButton.TabIndex = 14;
            this.MapRefreshButton.Text = "Refresh";
            this.MapRefreshButton.UseVisualStyleBackColor = true;
            this.MapRefreshButton.Click += new System.EventHandler(this.MapRefresh_ClickAsync);
            // 
            // PanePilots
            // 
            this.PanePilots.BackColor = System.Drawing.Color.LightSlateGray;
            this.PanePilots.Controls.Add(this.label8);
            this.PanePilots.Controls.Add(this.panel9);
            this.PanePilots.Controls.Add(this.PilotsPanel);
            this.PanePilots.Controls.Add(this.OpenPilotsBackupFolder);
            this.PanePilots.Controls.Add(this.PilotNameLabel);
            this.PanePilots.Controls.Add(this.PilotMakeActiveButton);
            this.PanePilots.Controls.Add(this.PilotDeleteButton);
            this.PanePilots.Controls.Add(this.AutoPilotsBackupCheckbox);
            this.PanePilots.Controls.Add(this.PilotRenameButton);
            this.PanePilots.Controls.Add(this.PilotXPSetButton);
            this.PanePilots.Controls.Add(this.PilotCloneButton);
            this.PanePilots.Controls.Add(this.PilotsBackupButton);
            this.PanePilots.Location = new System.Drawing.Point(1155, 43);
            this.PanePilots.Margin = new System.Windows.Forms.Padding(0);
            this.PanePilots.Name = "PanePilots";
            this.PanePilots.Size = new System.Drawing.Size(530, 336);
            this.PanePilots.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Overload Pilots";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label9);
            this.panel9.Controls.Add(this.PilotXPTextBox);
            this.panel9.Location = new System.Drawing.Point(346, 8);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(89, 39);
            this.panel9.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Pilot XP";
            // 
            // PilotXPTextBox
            // 
            this.PilotXPTextBox.BackColor = System.Drawing.Color.Gray;
            this.PilotXPTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PilotXPTextBox.Location = new System.Drawing.Point(6, 17);
            this.PilotXPTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.PilotXPTextBox.Name = "PilotXPTextBox";
            this.PilotXPTextBox.Size = new System.Drawing.Size(81, 20);
            this.PilotXPTextBox.TabIndex = 3;
            this.MainToolTip.SetToolTip(this.PilotXPTextBox, "Enter a value from 0 .. 9999999");
            this.PilotXPTextBox.TextChanged += new System.EventHandler(this.PilotXPTextBox_TextChanged);
            // 
            // PilotsPanel
            // 
            this.PilotsPanel.BackColor = System.Drawing.Color.Blue;
            this.PilotsPanel.Controls.Add(this.PilotsListBox);
            this.PilotsPanel.Location = new System.Drawing.Point(19, 25);
            this.PilotsPanel.Name = "PilotsPanel";
            this.PilotsPanel.Size = new System.Drawing.Size(193, 184);
            this.PilotsPanel.TabIndex = 18;
            // 
            // OpenPilotsBackupFolder
            // 
            this.OpenPilotsBackupFolder.AutoSize = true;
            this.OpenPilotsBackupFolder.LinkColor = System.Drawing.Color.Blue;
            this.OpenPilotsBackupFolder.Location = new System.Drawing.Point(220, 234);
            this.OpenPilotsBackupFolder.Name = "OpenPilotsBackupFolder";
            this.OpenPilotsBackupFolder.Size = new System.Drawing.Size(123, 13);
            this.OpenPilotsBackupFolder.TabIndex = 17;
            this.OpenPilotsBackupFolder.TabStop = true;
            this.OpenPilotsBackupFolder.Text = "Open pilot backup folder";
            this.OpenPilotsBackupFolder.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.OpenPilotsBackupFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenPilotsBackupFolder_LinkClicked);
            // 
            // PilotNameLabel
            // 
            this.PilotNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PilotNameLabel.Enabled = false;
            this.PilotNameLabel.Location = new System.Drawing.Point(22, 255);
            this.PilotNameLabel.Multiline = false;
            this.PilotNameLabel.Name = "PilotNameLabel";
            this.PilotNameLabel.ReadOnly = true;
            this.PilotNameLabel.Size = new System.Drawing.Size(471, 17);
            this.PilotNameLabel.TabIndex = 17;
            this.PilotNameLabel.Text = "Pilot";
            // 
            // PilotMakeActiveButton
            // 
            this.PilotMakeActiveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PilotMakeActiveButton.Location = new System.Drawing.Point(220, 111);
            this.PilotMakeActiveButton.Name = "PilotMakeActiveButton";
            this.PilotMakeActiveButton.Size = new System.Drawing.Size(81, 23);
            this.PilotMakeActiveButton.TabIndex = 14;
            this.PilotMakeActiveButton.Text = "Select";
            this.PilotMakeActiveButton.UseVisualStyleBackColor = true;
            this.PilotMakeActiveButton.Click += new System.EventHandler(this.PilotMakeActiveButton_Click);
            // 
            // AutoPilotsBackupCheckbox
            // 
            this.AutoPilotsBackupCheckbox.AutoSize = true;
            this.AutoPilotsBackupCheckbox.CheckBackColor = System.Drawing.Color.Gray;
            this.AutoPilotsBackupCheckbox.CheckForeColor = System.Drawing.Color.SkyBlue;
            this.AutoPilotsBackupCheckbox.CheckInactiveForeColor = System.Drawing.Color.SlateGray;
            this.AutoPilotsBackupCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoPilotsBackupCheckbox.Location = new System.Drawing.Point(22, 301);
            this.AutoPilotsBackupCheckbox.Name = "AutoPilotsBackupCheckbox";
            this.AutoPilotsBackupCheckbox.Size = new System.Drawing.Size(277, 17);
            this.AutoPilotsBackupCheckbox.TabIndex = 5;
            this.AutoPilotsBackupCheckbox.Text = "Do a backup of all pilots each time Overload is started";
            this.MainToolTip.SetToolTip(this.AutoPilotsBackupCheckbox, "Check this to save a ZIP\'ed backup of all pilots when Overload/Olmod starts");
            this.AutoPilotsBackupCheckbox.UseVisualStyleBackColor = true;
            this.AutoPilotsBackupCheckbox.CheckedChanged += new System.EventHandler(this.AutoPilotsBackupCheckbox_CheckedChanged);
            // 
            // PilotXPSetButton
            // 
            this.PilotXPSetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PilotXPSetButton.Location = new System.Drawing.Point(438, 23);
            this.PilotXPSetButton.Name = "PilotXPSetButton";
            this.PilotXPSetButton.Size = new System.Drawing.Size(62, 24);
            this.PilotXPSetButton.TabIndex = 14;
            this.PilotXPSetButton.Text = "Set XP";
            this.PilotXPSetButton.UseVisualStyleBackColor = true;
            this.PilotXPSetButton.Click += new System.EventHandler(this.PilotXPSetButton_Click);
            // 
            // PaneSelectOlproxy
            // 
            this.PaneSelectOlproxy.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectOlproxy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectOlproxy.FlatAppearance.BorderSize = 0;
            this.PaneSelectOlproxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectOlproxy.ForeColor = System.Drawing.Color.White;
            this.PaneSelectOlproxy.Location = new System.Drawing.Point(224, 0);
            this.PaneSelectOlproxy.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectOlproxy.Name = "PaneSelectOlproxy";
            this.PaneSelectOlproxy.Size = new System.Drawing.Size(61, 23);
            this.PaneSelectOlproxy.TabIndex = 0;
            this.PaneSelectOlproxy.TabStop = false;
            this.PaneSelectOlproxy.Text = "  Olproxy";
            this.PaneSelectOlproxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectOlproxy.UseVisualStyleBackColor = false;
            // 
            // PaneSelectOlmod
            // 
            this.PaneSelectOlmod.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectOlmod.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectOlmod.FlatAppearance.BorderSize = 0;
            this.PaneSelectOlmod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectOlmod.ForeColor = System.Drawing.Color.White;
            this.PaneSelectOlmod.Location = new System.Drawing.Point(285, 0);
            this.PaneSelectOlmod.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectOlmod.Name = "PaneSelectOlmod";
            this.PaneSelectOlmod.Size = new System.Drawing.Size(57, 23);
            this.PaneSelectOlmod.TabIndex = 0;
            this.PaneSelectOlmod.TabStop = false;
            this.PaneSelectOlmod.Text = "  Olmod";
            this.PaneSelectOlmod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectOlmod.UseVisualStyleBackColor = false;
            // 
            // PaneOlproxy
            // 
            this.PaneOlproxy.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneOlproxy.Controls.Add(this.panel10);
            this.PaneOlproxy.Controls.Add(this.OlproxyReleases);
            this.PaneOlproxy.Controls.Add(this.panel2);
            this.PaneOlproxy.Controls.Add(this.StartStopOlproxyButton);
            this.PaneOlproxy.Controls.Add(this.OlproxyRunning);
            this.PaneOlproxy.Controls.Add(this.UseEmbeddedOlproxy);
            this.PaneOlproxy.Controls.Add(this.UseOlproxyCheckBox);
            this.PaneOlproxy.Location = new System.Drawing.Point(585, 43);
            this.PaneOlproxy.Margin = new System.Windows.Forms.Padding(0);
            this.PaneOlproxy.Name = "PaneOlproxy";
            this.PaneOlproxy.Size = new System.Drawing.Size(547, 336);
            this.PaneOlproxy.TabIndex = 21;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.OlproxyArgs);
            this.panel10.Controls.Add(this.label11);
            this.panel10.Location = new System.Drawing.Point(8, 94);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(521, 43);
            this.panel10.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Olproxy Startup Parameters";
            // 
            // OlproxyReleases
            // 
            this.OlproxyReleases.AutoSize = true;
            this.OlproxyReleases.LinkColor = System.Drawing.Color.Blue;
            this.OlproxyReleases.Location = new System.Drawing.Point(11, 208);
            this.OlproxyReleases.Name = "OlproxyReleases";
            this.OlproxyReleases.Size = new System.Drawing.Size(216, 13);
            this.OlproxyReleases.TabIndex = 17;
            this.OlproxyReleases.TabStop = true;
            this.OlproxyReleases.Text = "https://github.com/arbruijn/olproxy/releases";
            this.OlproxyReleases.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.OlproxyReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OlproxyReleases_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.OlproxyExecutable);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Location = new System.Drawing.Point(8, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(521, 43);
            this.panel2.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "External Olproxy Application";
            // 
            // StartStopOlproxyButton
            // 
            this.StartStopOlproxyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartStopOlproxyButton.Location = new System.Drawing.Point(14, 295);
            this.StartStopOlproxyButton.Name = "StartStopOlproxyButton";
            this.StartStopOlproxyButton.Size = new System.Drawing.Size(56, 24);
            this.StartStopOlproxyButton.TabIndex = 9;
            this.StartStopOlproxyButton.Text = "Start";
            this.StartStopOlproxyButton.UseVisualStyleBackColor = true;
            this.StartStopOlproxyButton.Click += new System.EventHandler(this.StartStopOlproxyButton_Click);
            // 
            // UseEmbeddedOlproxy
            // 
            this.UseEmbeddedOlproxy.AutoSize = true;
            this.UseEmbeddedOlproxy.BackColor = System.Drawing.Color.LightSlateGray;
            this.UseEmbeddedOlproxy.CheckBackColor = System.Drawing.Color.Gray;
            this.UseEmbeddedOlproxy.Checked = true;
            this.UseEmbeddedOlproxy.CheckForeColor = System.Drawing.Color.Black;
            this.UseEmbeddedOlproxy.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.UseEmbeddedOlproxy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseEmbeddedOlproxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseEmbeddedOlproxy.Location = new System.Drawing.Point(127, 13);
            this.UseEmbeddedOlproxy.Name = "UseEmbeddedOlproxy";
            this.UseEmbeddedOlproxy.Size = new System.Drawing.Size(133, 17);
            this.UseEmbeddedOlproxy.TabIndex = 5;
            this.UseEmbeddedOlproxy.Text = "Use embedded Olproxy";
            this.MainToolTip.SetToolTip(this.UseEmbeddedOlproxy, "Use the built-in Olproxy");
            this.UseEmbeddedOlproxy.UseVisualStyleBackColor = false;
            this.UseEmbeddedOlproxy.CheckedChanged += new System.EventHandler(this.UseEmbeddedOlproxy_CheckedChanged);
            // 
            // UseOlproxyCheckBox
            // 
            this.UseOlproxyCheckBox.AutoSize = true;
            this.UseOlproxyCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.UseOlproxyCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.UseOlproxyCheckBox.Checked = true;
            this.UseOlproxyCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.UseOlproxyCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.UseOlproxyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseOlproxyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseOlproxyCheckBox.Location = new System.Drawing.Point(12, 12);
            this.UseOlproxyCheckBox.Name = "UseOlproxyCheckBox";
            this.UseOlproxyCheckBox.Size = new System.Drawing.Size(80, 17);
            this.UseOlproxyCheckBox.TabIndex = 5;
            this.UseOlproxyCheckBox.Text = "Use Olproxy";
            this.MainToolTip.SetToolTip(this.UseOlproxyCheckBox, "Start Olproxy when Overload/Olmod is started");
            this.UseOlproxyCheckBox.UseVisualStyleBackColor = false;
            this.UseOlproxyCheckBox.CheckedChanged += new System.EventHandler(this.UseOlproxy_CheckedChanged);
            // 
            // PaneOverload
            // 
            this.PaneOverload.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneOverload.Controls.Add(this.panel4);
            this.PaneOverload.Controls.Add(this.panel3);
            this.PaneOverload.Controls.Add(this.OverloadLog);
            this.PaneOverload.Controls.Add(this.PLayOverloadLinkLabel);
            this.PaneOverload.Controls.Add(this.label1);
            this.PaneOverload.Controls.Add(this.SearchOverloadButton);
            this.PaneOverload.Location = new System.Drawing.Point(1155, 393);
            this.PaneOverload.Margin = new System.Windows.Forms.Padding(0);
            this.PaneOverload.Name = "PaneOverload";
            this.PaneOverload.Size = new System.Drawing.Size(530, 336);
            this.PaneOverload.TabIndex = 22;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.OverloadExecutable);
            this.panel4.Location = new System.Drawing.Point(12, 11);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(499, 47);
            this.panel4.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Overload Application";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.OverloadArgs);
            this.panel3.Location = new System.Drawing.Point(12, 66);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(499, 47);
            this.panel3.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Overload Startup Parameters";
            // 
            // OverloadLog
            // 
            this.OverloadLog.AutoSize = true;
            this.OverloadLog.LinkColor = System.Drawing.Color.Blue;
            this.OverloadLog.Location = new System.Drawing.Point(19, 171);
            this.OverloadLog.Name = "OverloadLog";
            this.OverloadLog.Size = new System.Drawing.Size(96, 13);
            this.OverloadLog.TabIndex = 18;
            this.OverloadLog.TabStop = true;
            this.OverloadLog.Text = "Open Overload log";
            this.OverloadLog.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.OverloadLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OverloadLog_LinkClicked);
            // 
            // PLayOverloadLinkLabel
            // 
            this.PLayOverloadLinkLabel.ActiveLinkColor = System.Drawing.Color.DeepSkyBlue;
            this.PLayOverloadLinkLabel.AutoSize = true;
            this.PLayOverloadLinkLabel.LinkColor = System.Drawing.Color.Blue;
            this.PLayOverloadLinkLabel.Location = new System.Drawing.Point(19, 144);
            this.PLayOverloadLinkLabel.Name = "PLayOverloadLinkLabel";
            this.PLayOverloadLinkLabel.Size = new System.Drawing.Size(126, 13);
            this.PLayOverloadLinkLabel.TabIndex = 17;
            this.PLayOverloadLinkLabel.TabStop = true;
            this.PLayOverloadLinkLabel.Text = "https://playoverload.com";
            this.PLayOverloadLinkLabel.VisitedLinkColor = System.Drawing.Color.SteelBlue;
            this.PLayOverloadLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.PlayOverload_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 299);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(331, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Use the Search button to automatically find your Overload installation";
            // 
            // SearchOverloadButton
            // 
            this.SearchOverloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchOverloadButton.Location = new System.Drawing.Point(22, 294);
            this.SearchOverloadButton.Name = "SearchOverloadButton";
            this.SearchOverloadButton.Size = new System.Drawing.Size(75, 23);
            this.SearchOverloadButton.TabIndex = 14;
            this.SearchOverloadButton.Text = "Search";
            this.SearchOverloadButton.UseVisualStyleBackColor = true;
            this.SearchOverloadButton.Click += new System.EventHandler(this.SearchOverloadButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 304);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(308, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "IF Olmod is enabled it will use the parameters setup for Overload";
            // 
            // PaneOlmod
            // 
            this.PaneOlmod.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneOlmod.Controls.Add(this.panel5);
            this.PaneOlmod.Controls.Add(this.OlmodReleases);
            this.PaneOlmod.Controls.Add(this.UpdateOlmodButton);
            this.PaneOlmod.Controls.Add(this.label2);
            this.PaneOlmod.Controls.Add(this.FrameTimeCheckBox);
            this.PaneOlmod.Controls.Add(this.UseOlmodGameDirArg);
            this.PaneOlmod.Controls.Add(this.AutoUpdateOlmod);
            this.PaneOlmod.Controls.Add(this.UseOlmodCheckBox);
            this.PaneOlmod.Location = new System.Drawing.Point(585, 393);
            this.PaneOlmod.Margin = new System.Windows.Forms.Padding(0);
            this.PaneOlmod.Name = "PaneOlmod";
            this.PaneOlmod.Size = new System.Drawing.Size(547, 336);
            this.PaneOlmod.TabIndex = 23;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.OlmodExecutable);
            this.panel5.Location = new System.Drawing.Point(25, 47);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(504, 51);
            this.panel5.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Olmod Application";
            // 
            // OlmodExecutable
            // 
            this.OlmodExecutable.BackColor = System.Drawing.Color.Gray;
            this.OlmodExecutable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OlmodExecutable.Location = new System.Drawing.Point(11, 23);
            this.OlmodExecutable.Margin = new System.Windows.Forms.Padding(2);
            this.OlmodExecutable.Name = "OlmodExecutable";
            this.OlmodExecutable.Size = new System.Drawing.Size(479, 20);
            this.OlmodExecutable.TabIndex = 3;
            this.OlmodExecutable.TextChanged += new System.EventHandler(this.OlmodExecutable_TextChanged);
            this.OlmodExecutable.DoubleClick += new System.EventHandler(this.OlmodExecutable_DoubleClick);
            // 
            // OlmodReleases
            // 
            this.OlmodReleases.AutoSize = true;
            this.OlmodReleases.LinkColor = System.Drawing.Color.Blue;
            this.OlmodReleases.Location = new System.Drawing.Point(22, 121);
            this.OlmodReleases.Name = "OlmodReleases";
            this.OlmodReleases.Size = new System.Drawing.Size(211, 13);
            this.OlmodReleases.TabIndex = 17;
            this.OlmodReleases.TabStop = true;
            this.OlmodReleases.Text = "https://github.com/arbruijn/olmod/releases";
            this.OlmodReleases.VisitedLinkColor = System.Drawing.Color.SteelBlue;
            this.OlmodReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OlmodReleases_LinkClicked);
            // 
            // UpdateOlmodButton
            // 
            this.UpdateOlmodButton.Enabled = false;
            this.UpdateOlmodButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateOlmodButton.Location = new System.Drawing.Point(406, 104);
            this.UpdateOlmodButton.Name = "UpdateOlmodButton";
            this.UpdateOlmodButton.Size = new System.Drawing.Size(109, 24);
            this.UpdateOlmodButton.TabIndex = 9;
            this.UpdateOlmodButton.Text = "Update Olmod now";
            this.UpdateOlmodButton.UseVisualStyleBackColor = true;
            this.UpdateOlmodButton.Click += new System.EventHandler(this.UpdateOlmod_Click);
            // 
            // FrameTimeCheckBox
            // 
            this.FrameTimeCheckBox.AutoSize = true;
            this.FrameTimeCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.FrameTimeCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.FrameTimeCheckBox.Checked = true;
            this.FrameTimeCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.FrameTimeCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.FrameTimeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrameTimeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FrameTimeCheckBox.Location = new System.Drawing.Point(25, 177);
            this.FrameTimeCheckBox.Name = "FrameTimeCheckBox";
            this.FrameTimeCheckBox.Size = new System.Drawing.Size(190, 17);
            this.FrameTimeCheckBox.TabIndex = 5;
            this.FrameTimeCheckBox.Text = "Show FPS (using -frametime option)";
            this.FrameTimeCheckBox.UseVisualStyleBackColor = false;
            this.FrameTimeCheckBox.CheckedChanged += new System.EventHandler(this.FrameTimeCheckBox_CheckedChanged);
            // 
            // UseOlmodGameDirArg
            // 
            this.UseOlmodGameDirArg.AutoSize = true;
            this.UseOlmodGameDirArg.BackColor = System.Drawing.Color.LightSlateGray;
            this.UseOlmodGameDirArg.CheckBackColor = System.Drawing.Color.Gray;
            this.UseOlmodGameDirArg.Checked = true;
            this.UseOlmodGameDirArg.CheckForeColor = System.Drawing.Color.Black;
            this.UseOlmodGameDirArg.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.UseOlmodGameDirArg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseOlmodGameDirArg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseOlmodGameDirArg.Location = new System.Drawing.Point(25, 154);
            this.UseOlmodGameDirArg.Name = "UseOlmodGameDirArg";
            this.UseOlmodGameDirArg.Size = new System.Drawing.Size(308, 17);
            this.UseOlmodGameDirArg.TabIndex = 5;
            this.UseOlmodGameDirArg.Text = "Tell Olmod where Overload is installed (using-gamedir option)";
            this.MainToolTip.SetToolTip(this.UseOlmodGameDirArg, "Enable this to use Olmod \'-gamedir\' to tell Olmod where Overload is installed");
            this.UseOlmodGameDirArg.UseVisualStyleBackColor = false;
            this.UseOlmodGameDirArg.CheckedChanged += new System.EventHandler(this.UseGameDirArg_CheckedChanged);
            // 
            // AutoUpdateOlmod
            // 
            this.AutoUpdateOlmod.AutoSize = true;
            this.AutoUpdateOlmod.CheckBackColor = System.Drawing.Color.Gray;
            this.AutoUpdateOlmod.Checked = true;
            this.AutoUpdateOlmod.CheckForeColor = System.Drawing.Color.SkyBlue;
            this.AutoUpdateOlmod.CheckInactiveForeColor = System.Drawing.Color.SlateGray;
            this.AutoUpdateOlmod.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoUpdateOlmod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoUpdateOlmod.Location = new System.Drawing.Point(119, 17);
            this.AutoUpdateOlmod.Name = "AutoUpdateOlmod";
            this.AutoUpdateOlmod.Size = new System.Drawing.Size(114, 17);
            this.AutoUpdateOlmod.TabIndex = 5;
            this.AutoUpdateOlmod.Text = "Auto-update Olmod";
            this.MainToolTip.SetToolTip(this.AutoUpdateOlmod, "Enable this to automatically update Olmod when OCT starts");
            this.AutoUpdateOlmod.UseVisualStyleBackColor = true;
            this.AutoUpdateOlmod.CheckedChanged += new System.EventHandler(this.AutoUpdateOlmod_CheckedChanged);
            // 
            // UseOlmodCheckBox
            // 
            this.UseOlmodCheckBox.AutoSize = true;
            this.UseOlmodCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.UseOlmodCheckBox.Checked = true;
            this.UseOlmodCheckBox.CheckForeColor = System.Drawing.Color.SkyBlue;
            this.UseOlmodCheckBox.CheckInactiveForeColor = System.Drawing.Color.SlateGray;
            this.UseOlmodCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseOlmodCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseOlmodCheckBox.Location = new System.Drawing.Point(25, 17);
            this.UseOlmodCheckBox.Name = "UseOlmodCheckBox";
            this.UseOlmodCheckBox.Size = new System.Drawing.Size(75, 17);
            this.UseOlmodCheckBox.TabIndex = 5;
            this.UseOlmodCheckBox.Text = "Use Olmod";
            this.MainToolTip.SetToolTip(this.UseOlmodCheckBox, "If checked then Olmod will be used to run Overload");
            this.UseOlmodCheckBox.UseVisualStyleBackColor = true;
            this.UseOlmodCheckBox.CheckedChanged += new System.EventHandler(this.UseOlmod_CheckedChanged);
            // 
            // PaneButtonLine
            // 
            this.PaneButtonLine.BackColor = System.Drawing.Color.MediumBlue;
            this.PaneButtonLine.Location = new System.Drawing.Point(0, 26);
            this.PaneButtonLine.Margin = new System.Windows.Forms.Padding(0);
            this.PaneButtonLine.Name = "PaneButtonLine";
            this.PaneButtonLine.Size = new System.Drawing.Size(496, 10);
            this.PaneButtonLine.TabIndex = 24;
            // 
            // PaneSelectOptions
            // 
            this.PaneSelectOptions.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectOptions.FlatAppearance.BorderSize = 0;
            this.PaneSelectOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectOptions.ForeColor = System.Drawing.Color.White;
            this.PaneSelectOptions.Location = new System.Drawing.Point(456, 0);
            this.PaneSelectOptions.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectOptions.Name = "PaneSelectOptions";
            this.PaneSelectOptions.Size = new System.Drawing.Size(57, 23);
            this.PaneSelectOptions.TabIndex = 0;
            this.PaneSelectOptions.TabStop = false;
            this.PaneSelectOptions.Text = "  Options";
            this.PaneSelectOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectOptions.UseVisualStyleBackColor = false;
            // 
            // PaneOptions
            // 
            this.PaneOptions.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneOptions.Controls.Add(this.ForceUpdateButton);
            this.PaneOptions.Controls.Add(this.ThemeDescriptionLabel);
            this.PaneOptions.Controls.Add(this.ActiveThemePanel);
            this.PaneOptions.Controls.Add(this.label13);
            this.PaneOptions.Controls.Add(this.MinimizeOnStartupCheckBox);
            this.PaneOptions.Controls.Add(this.UseTrayIcon);
            this.PaneOptions.Controls.Add(this.PayPalLink);
            this.PaneOptions.Controls.Add(this.DisplayHelpLink);
            this.PaneOptions.Controls.Add(this.MailLinkLabel);
            this.PaneOptions.Controls.Add(this.DebugFileNameLink);
            this.PaneOptions.Controls.Add(this.PartyModeCheckBox);
            this.PaneOptions.Controls.Add(this.EnableDebugCheckBox);
            this.PaneOptions.Controls.Add(this.AutoUpdateCheckBox);
            this.PaneOptions.Location = new System.Drawing.Point(1732, 43);
            this.PaneOptions.Margin = new System.Windows.Forms.Padding(0);
            this.PaneOptions.Name = "PaneOptions";
            this.PaneOptions.Size = new System.Drawing.Size(547, 336);
            this.PaneOptions.TabIndex = 25;
            // 
            // ForceUpdateButton
            // 
            this.ForceUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ForceUpdateButton.Location = new System.Drawing.Point(439, 36);
            this.ForceUpdateButton.Margin = new System.Windows.Forms.Padding(0);
            this.ForceUpdateButton.Name = "ForceUpdateButton";
            this.ForceUpdateButton.Size = new System.Drawing.Size(77, 23);
            this.ForceUpdateButton.TabIndex = 14;
            this.ForceUpdateButton.Text = "Update now";
            this.ForceUpdateButton.UseVisualStyleBackColor = true;
            this.ForceUpdateButton.Click += new System.EventHandler(this.ForceUpdateButton_Click);
            // 
            // ThemeDescriptionLabel
            // 
            this.ThemeDescriptionLabel.AutoSize = true;
            this.ThemeDescriptionLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.ThemeDescriptionLabel.Location = new System.Drawing.Point(20, 191);
            this.ThemeDescriptionLabel.Name = "ThemeDescriptionLabel";
            this.ThemeDescriptionLabel.Size = new System.Drawing.Size(144, 13);
            this.ThemeDescriptionLabel.TabIndex = 20;
            this.ThemeDescriptionLabel.Text = "Theme description goes here";
            // 
            // ActiveThemePanel
            // 
            this.ActiveThemePanel.BackColor = System.Drawing.Color.Blue;
            this.ActiveThemePanel.Controls.Add(this.AvailableThemesListBox);
            this.ActiveThemePanel.Location = new System.Drawing.Point(12, 25);
            this.ActiveThemePanel.Name = "ActiveThemePanel";
            this.ActiveThemePanel.Size = new System.Drawing.Size(176, 158);
            this.ActiveThemePanel.TabIndex = 19;
            // 
            // AvailableThemesListBox
            // 
            this.AvailableThemesListBox.BackColor = System.Drawing.Color.Gray;
            this.AvailableThemesListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AvailableThemesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.AvailableThemesListBox.FormattingEnabled = true;
            this.AvailableThemesListBox.Location = new System.Drawing.Point(1, 1);
            this.AvailableThemesListBox.Margin = new System.Windows.Forms.Padding(1);
            this.AvailableThemesListBox.Name = "AvailableThemesListBox";
            this.AvailableThemesListBox.Size = new System.Drawing.Size(174, 156);
            this.AvailableThemesListBox.TabIndex = 19;
            this.AvailableThemesListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.AvailableThemesListBox_DrawItem);
            this.AvailableThemesListBox.SelectedIndexChanged += new System.EventHandler(this.AvailableThemes_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Active Theme";
            // 
            // MinimizeOnStartupCheckBox
            // 
            this.MinimizeOnStartupCheckBox.AutoSize = true;
            this.MinimizeOnStartupCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.MinimizeOnStartupCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.MinimizeOnStartupCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.MinimizeOnStartupCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.MinimizeOnStartupCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MinimizeOnStartupCheckBox.Location = new System.Drawing.Point(272, 66);
            this.MinimizeOnStartupCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.MinimizeOnStartupCheckBox.Name = "MinimizeOnStartupCheckBox";
            this.MinimizeOnStartupCheckBox.Size = new System.Drawing.Size(113, 17);
            this.MinimizeOnStartupCheckBox.TabIndex = 5;
            this.MinimizeOnStartupCheckBox.Text = "Minimize on startup";
            this.MinimizeOnStartupCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MainToolTip.SetToolTip(this.MinimizeOnStartupCheckBox, "Select this to minimize OCT on startup");
            this.MinimizeOnStartupCheckBox.UseVisualStyleBackColor = false;
            this.MinimizeOnStartupCheckBox.CheckedChanged += new System.EventHandler(this.MinimizeOnStartupCheckBox_CheckedChanged);
            // 
            // UseTrayIcon
            // 
            this.UseTrayIcon.AutoSize = true;
            this.UseTrayIcon.BackColor = System.Drawing.Color.LightSlateGray;
            this.UseTrayIcon.CheckBackColor = System.Drawing.Color.Gray;
            this.UseTrayIcon.CheckForeColor = System.Drawing.Color.Black;
            this.UseTrayIcon.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.UseTrayIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseTrayIcon.Location = new System.Drawing.Point(272, 92);
            this.UseTrayIcon.Margin = new System.Windows.Forms.Padding(0);
            this.UseTrayIcon.Name = "UseTrayIcon";
            this.UseTrayIcon.Size = new System.Drawing.Size(85, 17);
            this.UseTrayIcon.TabIndex = 5;
            this.UseTrayIcon.Text = "Use tray icon";
            this.UseTrayIcon.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MainToolTip.SetToolTip(this.UseTrayIcon, "Minimize to tray instead of the taskbar");
            this.UseTrayIcon.UseVisualStyleBackColor = false;
            this.UseTrayIcon.CheckedChanged += new System.EventHandler(this.UseTrayIcon_CheckedChanged);
            // 
            // PayPalLink
            // 
            this.PayPalLink.AutoSize = true;
            this.PayPalLink.Location = new System.Drawing.Point(20, 305);
            this.PayPalLink.Name = "PayPalLink";
            this.PayPalLink.Size = new System.Drawing.Size(255, 13);
            this.PayPalLink.TabIndex = 18;
            this.PayPalLink.TabStop = true;
            this.PayPalLink.Text = "If you like OCT consider buying me a cup of coffee :)";
            this.MainToolTip.SetToolTip(this.PayPalLink, "This is TOTALLY optional as OCT will always be free!");
            this.PayPalLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.PayPalLink_LinkClicked);
            // 
            // DisplayHelpLink
            // 
            this.DisplayHelpLink.AutoSize = true;
            this.DisplayHelpLink.Location = new System.Drawing.Point(20, 234);
            this.DisplayHelpLink.Name = "DisplayHelpLink";
            this.DisplayHelpLink.Size = new System.Drawing.Size(97, 13);
            this.DisplayHelpLink.TabIndex = 18;
            this.DisplayHelpLink.TabStop = true;
            this.DisplayHelpLink.Text = "Help on using OCT";
            this.DisplayHelpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DisplayHelpLink_LinkClicked);
            // 
            // MailLinkLabel
            // 
            this.MailLinkLabel.AutoSize = true;
            this.MailLinkLabel.Location = new System.Drawing.Point(20, 284);
            this.MailLinkLabel.Name = "MailLinkLabel";
            this.MailLinkLabel.Size = new System.Drawing.Size(251, 13);
            this.MailLinkLabel.TabIndex = 18;
            this.MailLinkLabel.TabStop = true;
            this.MailLinkLabel.Text = "Please contact me about any issues or suggestions!";
            this.MailLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.InfoLinkLabel_LinkClicked);
            // 
            // DebugFileNameLink
            // 
            this.DebugFileNameLink.AutoSize = true;
            this.DebugFileNameLink.Location = new System.Drawing.Point(399, 15);
            this.DebugFileNameLink.Name = "DebugFileNameLink";
            this.DebugFileNameLink.Size = new System.Drawing.Size(52, 13);
            this.DebugFileNameLink.TabIndex = 17;
            this.DebugFileNameLink.TabStop = true;
            this.DebugFileNameLink.Text = "View logs";
            this.DebugFileNameLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenDebugFolder_LinkClicked);
            // 
            // PartyModeCheckBox
            // 
            this.PartyModeCheckBox.AutoSize = true;
            this.PartyModeCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.PartyModeCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.PartyModeCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.PartyModeCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.PartyModeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PartyModeCheckBox.Location = new System.Drawing.Point(272, 118);
            this.PartyModeCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.PartyModeCheckBox.Name = "PartyModeCheckBox";
            this.PartyModeCheckBox.Size = new System.Drawing.Size(79, 17);
            this.PartyModeCheckBox.TabIndex = 5;
            this.PartyModeCheckBox.Text = "Party mode!";
            this.PartyModeCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MainToolTip.SetToolTip(this.PartyModeCheckBox, "Don\'t select this (you have been warned) :) !!");
            this.PartyModeCheckBox.UseVisualStyleBackColor = false;
            // 
            // EnableDebugCheckBox
            // 
            this.EnableDebugCheckBox.AutoSize = true;
            this.EnableDebugCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.EnableDebugCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.EnableDebugCheckBox.Checked = true;
            this.EnableDebugCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.EnableDebugCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.EnableDebugCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableDebugCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EnableDebugCheckBox.Location = new System.Drawing.Point(272, 14);
            this.EnableDebugCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.EnableDebugCheckBox.Name = "EnableDebugCheckBox";
            this.EnableDebugCheckBox.Size = new System.Drawing.Size(126, 17);
            this.EnableDebugCheckBox.TabIndex = 5;
            this.EnableDebugCheckBox.Text = "Enable debug logging";
            this.EnableDebugCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MainToolTip.SetToolTip(this.EnableDebugCheckBox, "Check this to save debugging info to a text file");
            this.EnableDebugCheckBox.UseVisualStyleBackColor = false;
            this.EnableDebugCheckBox.CheckedChanged += new System.EventHandler(this.EnableDebugCheckBox_CheckedChanged);
            // 
            // AutoUpdateCheckBox
            // 
            this.AutoUpdateCheckBox.AutoSize = true;
            this.AutoUpdateCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.AutoUpdateCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.AutoUpdateCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.AutoUpdateCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.AutoUpdateCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.AutoUpdateCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoUpdateCheckBox.Location = new System.Drawing.Point(272, 40);
            this.AutoUpdateCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.AutoUpdateCheckBox.Name = "AutoUpdateCheckBox";
            this.AutoUpdateCheckBox.Size = new System.Drawing.Size(167, 17);
            this.AutoUpdateCheckBox.TabIndex = 5;
            this.AutoUpdateCheckBox.Text = "Check for update upon startup";
            this.AutoUpdateCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MainToolTip.SetToolTip(this.AutoUpdateCheckBox, "Select this to make OCT check for new release at startup");
            this.AutoUpdateCheckBox.UseVisualStyleBackColor = false;
            this.AutoUpdateCheckBox.CheckedChanged += new System.EventHandler(this.AutoUpdateCheckBox_CheckedChanged);
            // 
            // MainToolTip
            // 
            this.MainToolTip.AutoPopDelay = 10000;
            this.MainToolTip.InitialDelay = 1000;
            this.MainToolTip.ReshowDelay = 100;
            // 
            // ServerAnnounceOnTrackerCheckBox
            // 
            this.ServerAnnounceOnTrackerCheckBox.AutoSize = true;
            this.ServerAnnounceOnTrackerCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.ServerAnnounceOnTrackerCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.ServerAnnounceOnTrackerCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.ServerAnnounceOnTrackerCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.ServerAnnounceOnTrackerCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ServerAnnounceOnTrackerCheckBox.Location = new System.Drawing.Point(32, 140);
            this.ServerAnnounceOnTrackerCheckBox.Name = "ServerAnnounceOnTrackerCheckBox";
            this.ServerAnnounceOnTrackerCheckBox.Size = new System.Drawing.Size(165, 17);
            this.ServerAnnounceOnTrackerCheckBox.TabIndex = 5;
            this.ServerAnnounceOnTrackerCheckBox.Text = "Make server visible on tracker";
            this.MainToolTip.SetToolTip(this.ServerAnnounceOnTrackerCheckBox, "Send server info to tracker when running");
            this.ServerAnnounceOnTrackerCheckBox.UseVisualStyleBackColor = false;
            this.ServerAnnounceOnTrackerCheckBox.CheckedChanged += new System.EventHandler(this.ServerAnnounceOnTrackerCheckBox_CheckedChanged);
            // 
            // ServerAutoSignOffTracker
            // 
            this.ServerAutoSignOffTracker.AutoSize = true;
            this.ServerAutoSignOffTracker.BackColor = System.Drawing.Color.LightSlateGray;
            this.ServerAutoSignOffTracker.CheckBackColor = System.Drawing.Color.Gray;
            this.ServerAutoSignOffTracker.Checked = true;
            this.ServerAutoSignOffTracker.CheckForeColor = System.Drawing.Color.Black;
            this.ServerAutoSignOffTracker.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.ServerAutoSignOffTracker.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ServerAutoSignOffTracker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ServerAutoSignOffTracker.Location = new System.Drawing.Point(32, 163);
            this.ServerAutoSignOffTracker.Name = "ServerAutoSignOffTracker";
            this.ServerAutoSignOffTracker.Size = new System.Drawing.Size(194, 17);
            this.ServerAutoSignOffTracker.TabIndex = 5;
            this.ServerAutoSignOffTracker.Text = "Remove inactive server from tracker";
            this.MainToolTip.SetToolTip(this.ServerAutoSignOffTracker, "Select this to remove the server from tracker when it isn\'t running");
            this.ServerAutoSignOffTracker.UseVisualStyleBackColor = false;
            this.ServerAutoSignOffTracker.CheckedChanged += new System.EventHandler(this.ServerAutoSignOffTracker_CheckedChanged);
            // 
            // AutoStartCheckBox
            // 
            this.AutoStartCheckBox.AutoSize = true;
            this.AutoStartCheckBox.BackColor = System.Drawing.Color.LightSlateGray;
            this.AutoStartCheckBox.CheckBackColor = System.Drawing.Color.Gray;
            this.AutoStartCheckBox.CheckForeColor = System.Drawing.Color.Black;
            this.AutoStartCheckBox.CheckInactiveForeColor = System.Drawing.Color.Black;
            this.AutoStartCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.AutoStartCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoStartCheckBox.Location = new System.Drawing.Point(31, 186);
            this.AutoStartCheckBox.Name = "AutoStartCheckBox";
            this.AutoStartCheckBox.Size = new System.Drawing.Size(133, 17);
            this.AutoStartCheckBox.TabIndex = 5;
            this.AutoStartCheckBox.Text = "Start at Windows logon";
            this.MainToolTip.SetToolTip(this.AutoStartCheckBox, "Select this to autostart OCT + server at Windows startup");
            this.AutoStartCheckBox.UseVisualStyleBackColor = false;
            this.AutoStartCheckBox.CheckedChanged += new System.EventHandler(this.AutoStartCheckBox_CheckedChanged);
            // 
            // PaneSelectServer
            // 
            this.PaneSelectServer.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectServer.FlatAppearance.BorderSize = 0;
            this.PaneSelectServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectServer.ForeColor = System.Drawing.Color.White;
            this.PaneSelectServer.Location = new System.Drawing.Point(342, 0);
            this.PaneSelectServer.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectServer.Name = "PaneSelectServer";
            this.PaneSelectServer.Size = new System.Drawing.Size(57, 23);
            this.PaneSelectServer.TabIndex = 26;
            this.PaneSelectServer.TabStop = false;
            this.PaneSelectServer.Text = "  Server";
            this.PaneSelectServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectServer.UseVisualStyleBackColor = false;
            // 
            // PaneServer
            // 
            this.PaneServer.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneServer.Controls.Add(this.panel6);
            this.PaneServer.Controls.Add(this.panel8);
            this.PaneServer.Controls.Add(this.StartServerButton);
            this.PaneServer.Controls.Add(this.panel1);
            this.PaneServer.Controls.Add(this.ServerRunning);
            this.PaneServer.Controls.Add(this.ServerAnnounceOnTrackerCheckBox);
            this.PaneServer.Controls.Add(this.ServerAutoSignOffTracker);
            this.PaneServer.Controls.Add(this.AutoStartCheckBox);
            this.PaneServer.Location = new System.Drawing.Point(1732, 393);
            this.PaneServer.Margin = new System.Windows.Forms.Padding(0);
            this.PaneServer.Name = "PaneServer";
            this.PaneServer.Size = new System.Drawing.Size(547, 336);
            this.PaneServer.TabIndex = 25;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label16);
            this.panel6.Controls.Add(this.ServerTrackerName);
            this.panel6.Location = new System.Drawing.Point(22, 11);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(196, 47);
            this.panel6.TabIndex = 19;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 5);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Server Name";
            // 
            // ServerTrackerName
            // 
            this.ServerTrackerName.BackColor = System.Drawing.Color.Gray;
            this.ServerTrackerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServerTrackerName.Location = new System.Drawing.Point(10, 19);
            this.ServerTrackerName.Margin = new System.Windows.Forms.Padding(1);
            this.ServerTrackerName.Name = "ServerTrackerName";
            this.ServerTrackerName.Size = new System.Drawing.Size(178, 20);
            this.ServerTrackerName.TabIndex = 1;
            this.ServerTrackerName.TextChanged += new System.EventHandler(this.ServerTrackerName_TextChanged);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label14);
            this.panel8.Controls.Add(this.ServerTrackerNotes);
            this.panel8.Location = new System.Drawing.Point(22, 71);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(511, 50);
            this.panel8.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(144, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Information about your server";
            // 
            // ServerTrackerNotes
            // 
            this.ServerTrackerNotes.BackColor = System.Drawing.Color.Gray;
            this.ServerTrackerNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServerTrackerNotes.Location = new System.Drawing.Point(10, 20);
            this.ServerTrackerNotes.Margin = new System.Windows.Forms.Padding(2);
            this.ServerTrackerNotes.Name = "ServerTrackerNotes";
            this.ServerTrackerNotes.Size = new System.Drawing.Size(492, 20);
            this.ServerTrackerNotes.TabIndex = 2;
            this.ServerTrackerNotes.TextChanged += new System.EventHandler(this.ServerTrackerNotes_TextChanged);
            // 
            // StartServerButton
            // 
            this.StartServerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartServerButton.Location = new System.Drawing.Point(26, 298);
            this.StartServerButton.Name = "StartServerButton";
            this.StartServerButton.Size = new System.Drawing.Size(88, 24);
            this.StartServerButton.TabIndex = 9;
            this.StartServerButton.Text = "Start Server";
            this.StartServerButton.UseVisualStyleBackColor = true;
            this.StartServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.ServerTrackerUrl);
            this.panel1.Location = new System.Drawing.Point(225, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 47);
            this.panel1.TabIndex = 18;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "Tracker URL";
            // 
            // ServerTrackerUrl
            // 
            this.ServerTrackerUrl.BackColor = System.Drawing.Color.Gray;
            this.ServerTrackerUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServerTrackerUrl.Location = new System.Drawing.Point(10, 20);
            this.ServerTrackerUrl.Margin = new System.Windows.Forms.Padding(2);
            this.ServerTrackerUrl.Name = "ServerTrackerUrl";
            this.ServerTrackerUrl.Size = new System.Drawing.Size(289, 20);
            this.ServerTrackerUrl.TabIndex = 2;
            this.ServerTrackerUrl.TextChanged += new System.EventHandler(this.ServerTrackerUrl_TextChanged);
            // 
            // ServerRunning
            // 
            this.ServerRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ServerRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
            this.ServerRunning.Location = new System.Drawing.Point(119, 300);
            this.ServerRunning.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.ServerRunning.Name = "ServerRunning";
            this.ServerRunning.Size = new System.Drawing.Size(22, 21);
            this.ServerRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ServerRunning.TabIndex = 10;
            this.ServerRunning.TabStop = false;
            this.ServerRunning.Visible = false;
            // 
            // PaneOnline
            // 
            this.PaneOnline.BackColor = System.Drawing.Color.LightSlateGray;
            this.PaneOnline.Controls.Add(this.UpdateServerListButton);
            this.PaneOnline.Controls.Add(this.ServersPanel);
            this.PaneOnline.Controls.Add(this.label19);
            this.PaneOnline.Location = new System.Drawing.Point(2291, 43);
            this.PaneOnline.Margin = new System.Windows.Forms.Padding(0);
            this.PaneOnline.Name = "PaneOnline";
            this.PaneOnline.Size = new System.Drawing.Size(547, 336);
            this.PaneOnline.TabIndex = 27;
            // 
            // UpdateServerListButton
            // 
            this.UpdateServerListButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateServerListButton.Location = new System.Drawing.Point(456, 292);
            this.UpdateServerListButton.Margin = new System.Windows.Forms.Padding(0);
            this.UpdateServerListButton.Name = "UpdateServerListButton";
            this.UpdateServerListButton.Size = new System.Drawing.Size(77, 23);
            this.UpdateServerListButton.TabIndex = 14;
            this.UpdateServerListButton.Text = "Update now";
            this.UpdateServerListButton.UseVisualStyleBackColor = true;
            this.UpdateServerListButton.Click += new System.EventHandler(this.UpdateServerListButton_Click);
            // 
            // ServersPanel
            // 
            this.ServersPanel.BackColor = System.Drawing.Color.Blue;
            this.ServersPanel.Controls.Add(this.ServersListBox);
            this.ServersPanel.Location = new System.Drawing.Point(12, 25);
            this.ServersPanel.Name = "ServersPanel";
            this.ServersPanel.Size = new System.Drawing.Size(518, 249);
            this.ServersPanel.TabIndex = 19;
            // 
            // ServersListBox
            // 
            this.ServersListBox.BackColor = System.Drawing.Color.Gray;
            this.ServersListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ServersListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ServersListBox.FormattingEnabled = true;
            this.ServersListBox.Location = new System.Drawing.Point(1, 1);
            this.ServersListBox.Margin = new System.Windows.Forms.Padding(1);
            this.ServersListBox.Name = "ServersListBox";
            this.ServersListBox.Size = new System.Drawing.Size(516, 247);
            this.ServersListBox.TabIndex = 19;
            this.ServersListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ServersListBox_DrawItem);
            this.ServersListBox.SelectedIndexChanged += new System.EventHandler(this.ServersListBox_SelectedIndexChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(9, 11);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(74, 13);
            this.label19.TabIndex = 3;
            this.label19.Text = "Online servers";
            // 
            // PaneSelectOnline
            // 
            this.PaneSelectOnline.BackColor = System.Drawing.Color.MidnightBlue;
            this.PaneSelectOnline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PaneSelectOnline.FlatAppearance.BorderSize = 0;
            this.PaneSelectOnline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaneSelectOnline.ForeColor = System.Drawing.Color.White;
            this.PaneSelectOnline.Location = new System.Drawing.Point(399, 0);
            this.PaneSelectOnline.Margin = new System.Windows.Forms.Padding(0);
            this.PaneSelectOnline.Name = "PaneSelectOnline";
            this.PaneSelectOnline.Size = new System.Drawing.Size(57, 23);
            this.PaneSelectOnline.TabIndex = 28;
            this.PaneSelectOnline.TabStop = false;
            this.PaneSelectOnline.Text = "  Online";
            this.PaneSelectOnline.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PaneSelectOnline.UseVisualStyleBackColor = false;
            // 
            // OCTMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.ExitButton;
            this.ClientSize = new System.Drawing.Size(2862, 821);
            this.Controls.Add(this.PaneSelectOnline);
            this.Controls.Add(this.PaneOnline);
            this.Controls.Add(this.PaneSelectServer);
            this.Controls.Add(this.PaneServer);
            this.Controls.Add(this.PaneOptions);
            this.Controls.Add(this.PaneButtonLine);
            this.Controls.Add(this.PaneOlmod);
            this.Controls.Add(this.PaneOverload);
            this.Controls.Add(this.PaneOlproxy);
            this.Controls.Add(this.PaneMaps);
            this.Controls.Add(this.PaneSelectOptions);
            this.Controls.Add(this.PaneSelectOlmod);
            this.Controls.Add(this.PanePilots);
            this.Controls.Add(this.PaneSelectOlproxy);
            this.Controls.Add(this.PaneSelectOverload);
            this.Controls.Add(this.PaneSelectPilots);
            this.Controls.Add(this.PaneSelectMapManager);
            this.Controls.Add(this.PaneSelectMain);
            this.Controls.Add(this.PaneMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "OCTMain";
            this.Text = "Overload Client Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.OverloadRunning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OlproxyRunning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdatingMaps)).EndInit();
            this.PaneMain.ResumeLayout(false);
            this.PaneMain.PerformLayout();
            this.LogTreePanel.ResumeLayout(false);
            this.LogTreePanel.PerformLayout();
            this.TreeViewLogPanel.ResumeLayout(false);
            this.PaneMaps.ResumeLayout(false);
            this.PaneMaps.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.MapsPanel.ResumeLayout(false);
            this.PanePilots.ResumeLayout(false);
            this.PanePilots.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.PilotsPanel.ResumeLayout(false);
            this.PaneOlproxy.ResumeLayout(false);
            this.PaneOlproxy.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.PaneOverload.ResumeLayout(false);
            this.PaneOverload.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.PaneOlmod.ResumeLayout(false);
            this.PaneOlmod.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.PaneOptions.ResumeLayout(false);
            this.PaneOptions.PerformLayout();
            this.ActiveThemePanel.ResumeLayout(false);
            this.PaneServer.ResumeLayout(false);
            this.PaneServer.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServerRunning)).EndInit();
            this.PaneOnline.ResumeLayout(false);
            this.PaneOnline.PerformLayout();
            this.ServersPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox OverloadExecutable;
        private System.Windows.Forms.TextBox OverloadArgs;
        private System.Windows.Forms.TextBox OlproxyExecutable;
        private System.Windows.Forms.TextBox OlproxyArgs;
        private System.Windows.Forms.OpenFileDialog SelectExecutable;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button MapUpdateButton;
        private System.Windows.Forms.NotifyIcon OverloadClientToolNotifyIcon;
        private System.Windows.Forms.TextBox OnlineMapJsonUrl;
        private System.Windows.Forms.PictureBox UpdatingMaps;
        private System.Windows.Forms.PictureBox OverloadRunning;
        private System.Windows.Forms.PictureBox OlproxyRunning;
        private System.Windows.Forms.Button PilotDeleteButton;
        private System.Windows.Forms.Button PilotRenameButton;
        private System.Windows.Forms.Button PilotCloneButton;
        private System.Windows.Forms.Button PilotsBackupButton;
        private System.Windows.Forms.Button MapDeleteButton;
        private System.Windows.Forms.Button MapHideButton;
        private System.Windows.Forms.Panel PaneMain;
        private System.Windows.Forms.Button PaneSelectMain;
        private System.Windows.Forms.Button PaneSelectMapManager;
        private System.Windows.Forms.Button PaneSelectPilots;
        private System.Windows.Forms.Button PaneSelectOverload;
        private System.Windows.Forms.Panel PaneMaps;
        private System.Windows.Forms.Panel PanePilots;
        private System.Windows.Forms.Button PaneSelectOlproxy;
        private System.Windows.Forms.Button PaneSelectOlmod;
        private System.Windows.Forms.Panel PaneOlproxy;
        private System.Windows.Forms.Panel PaneOverload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel PaneOlmod;
        private System.Windows.Forms.Button SearchOverloadButton;
        private System.Windows.Forms.Panel PaneButtonLine;
        private System.Windows.Forms.ListBox PilotsListBox;
        private System.Windows.Forms.TextBox OlmodExecutable;
        private System.Windows.Forms.Button PilotMakeActiveButton;
        private System.Windows.Forms.Label StatusMessage;
        private TransparentLabel PilotNameLabel;
        private System.Windows.Forms.LinkLabel OlproxyReleases;
        private System.Windows.Forms.LinkLabel PLayOverloadLinkLabel;
        private System.Windows.Forms.LinkLabel OlmodReleases;
        private System.Windows.Forms.LinkLabel OpenPilotsBackupFolder;
        private System.Windows.Forms.Button PaneSelectOptions;
        private System.Windows.Forms.Panel PaneOptions;
        private System.Windows.Forms.LinkLabel DebugFileNameLink;
        private System.Windows.Forms.Button MapRefreshButton;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.LinkLabel OverloadLog;
        private System.Windows.Forms.LinkLabel MailLinkLabel;
        private System.Windows.Forms.Button UpdateOlmodButton;
        private System.Windows.Forms.LinkLabel PayPalLink;
        private System.Windows.Forms.TextBox PilotXPTextBox;
        private System.Windows.Forms.Button PilotXPSetButton;
        private System.Windows.Forms.Button ForceUpdateButton;
        private System.Windows.Forms.Button StartStopOlproxyButton;
        private System.Windows.Forms.Button MapUnhideAllButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel MapsPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel PilotsPanel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel LogTreePanel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel ActiveThemePanel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListBox AvailableThemesListBox;
        private System.Windows.Forms.Label ThemeDescriptionLabel;
        private CustomCheckBox UseDLCLocationCheckBox;
        private CustomCheckBox AutoUpdateMapsCheckBox;
        private CustomCheckBox UseOlproxyCheckBox;
        private CustomCheckBox UseOlmodCheckBox;
        private CustomCheckBox OnlyUpdateExistingMapsCheckBox;
        private CustomCheckBox AutoPilotsBackupCheckbox;
        private CustomCheckBox EnableDebugCheckBox;
        private CustomCheckBox UseOlmodGameDirArg;
        private CustomCheckBox AutoUpdateCheckBox;
        private CustomCheckBox HideUnofficialMapsCheckBox;
        private CustomCheckBox UseEmbeddedOlproxy;
        private CustomCheckBox AutoUpdateOlmod;
        private CustomCheckBox PartyModeCheckBox;
        private System.Windows.Forms.Panel TreeViewLogPanel;
        private CustomTreeView LogTreeView;
        private CustomCheckBox FrameTimeCheckBox;
        private System.Windows.Forms.Button PaneSelectServer;
        private System.Windows.Forms.Panel PaneServer;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox ServerTrackerName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox ServerTrackerUrl;
        private CustomCheckBox ServerAnnounceOnTrackerCheckBox;
        private CustomCheckBox ServerAutoSignOffTracker;
        private CustomCheckBox AutoStartCheckBox;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox ServerTrackerNotes;
        private CustomCheckBox UseTrayIcon;
        private CustomCheckBox MinimizeOnStartupCheckBox;
        private System.Windows.Forms.Button StartServerButton;
        private System.Windows.Forms.PictureBox ServerRunning;
        private System.Windows.Forms.LinkLabel DisplayHelpLink;
        private CustomCheckBox CMMapsCheckBox;
        private CustomCheckBox SPMapsCheckBox;
        private CustomCheckBox MPMapsCheckBox;
        private System.Windows.Forms.Label label17;
        private CustomListBox MapsListBox;
        private CustomCheckBox HideHiddenMapsCheckBox;
        private System.Windows.Forms.Button HideCMMapsButton;
        private System.Windows.Forms.Button HideSPMapsButton;
        private System.Windows.Forms.Button HideMPMapsButton;
        private System.Windows.Forms.Button UnhideCMMapsButton;
        private System.Windows.Forms.Button UnhideSPMapsButton;
        private System.Windows.Forms.Button UnhideMPMapsButton;
        private System.Windows.Forms.Panel PaneOnline;
        private System.Windows.Forms.Button UpdateServerListButton;
        private System.Windows.Forms.Panel ServersPanel;
        private System.Windows.Forms.ListBox ServersListBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button PaneSelectOnline;
    }
}

