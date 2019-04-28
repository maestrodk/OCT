namespace OverloadClientTool
{
    partial class OCTMainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OverloadExecutable = new System.Windows.Forms.TextBox();
            this.OverloadArgs = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OlproxyExecutable = new System.Windows.Forms.TextBox();
            this.OlproxyArgs = new System.Windows.Forms.TextBox();
            this.SelectExecutable = new System.Windows.Forms.OpenFileDialog();
            this.StartButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.MapUpdateButton = new System.Windows.Forms.Button();
            this.SelectDark = new System.Windows.Forms.CheckBox();
            this.ActivityLogListBox = new System.Windows.Forms.ListBox();
            this.UseEmbeddedOlproxy = new System.Windows.Forms.CheckBox();
            this.OverloadGroupBox = new System.Windows.Forms.GroupBox();
            this.OverloadRunning = new System.Windows.Forms.PictureBox();
            this.OlproxyGroupBox = new System.Windows.Forms.GroupBox();
            this.OlproxyRunning = new System.Windows.Forms.PictureBox();
            this.UpdatingMaps = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.AutoUpdateMapsCheckBox = new System.Windows.Forms.CheckBox();
            this.UseDLCLocationCheckBox = new System.Windows.Forms.CheckBox();
            this.UseOlproxyCheckBox = new System.Windows.Forms.CheckBox();
            this.UseOlmodCheckBox = new System.Windows.Forms.CheckBox();
            this.LoggingGroupBox = new System.Windows.Forms.GroupBox();
            this.ActionsGroupBox = new System.Windows.Forms.GroupBox();
            this.CreateDesktopShortcuts = new System.Windows.Forms.Button();
            this.OverloadClientToolNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OverloadGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OverloadRunning)).BeginInit();
            this.OlproxyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OlproxyRunning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdatingMaps)).BeginInit();
            this.OptionsGroupBox.SuspendLayout();
            this.LoggingGroupBox.SuspendLayout();
            this.ActionsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Path to Overload.exe / Olmod.exe";
            // 
            // OverloadExecutable
            // 
            this.OverloadExecutable.BackColor = System.Drawing.Color.White;
            this.OverloadExecutable.Location = new System.Drawing.Point(13, 41);
            this.OverloadExecutable.Margin = new System.Windows.Forms.Padding(1);
            this.OverloadExecutable.Name = "OverloadExecutable";
            this.OverloadExecutable.Size = new System.Drawing.Size(394, 20);
            this.OverloadExecutable.TabIndex = 1;
            this.OverloadExecutable.TextChanged += new System.EventHandler(this.OverloadExecutable_TextChanged);
            this.OverloadExecutable.DoubleClick += new System.EventHandler(this.OverloadExecutable_MouseDoubleClick);
            // 
            // OverloadArgs
            // 
            this.OverloadArgs.Location = new System.Drawing.Point(13, 82);
            this.OverloadArgs.Margin = new System.Windows.Forms.Padding(2);
            this.OverloadArgs.Name = "OverloadArgs";
            this.OverloadArgs.Size = new System.Drawing.Size(394, 20);
            this.OverloadArgs.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Parameters";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Path to Olproxy.exe";
            // 
            // OlproxyExecutable
            // 
            this.OlproxyExecutable.Location = new System.Drawing.Point(13, 40);
            this.OlproxyExecutable.Margin = new System.Windows.Forms.Padding(2);
            this.OlproxyExecutable.Name = "OlproxyExecutable";
            this.OlproxyExecutable.Size = new System.Drawing.Size(394, 20);
            this.OlproxyExecutable.TabIndex = 3;
            this.OlproxyExecutable.TextChanged += new System.EventHandler(this.OlproxyExecutable_TextChanged);
            this.OlproxyExecutable.DoubleClick += new System.EventHandler(this.OlproxyExecutable_DoubleClick);
            // 
            // OlproxyArgs
            // 
            this.OlproxyArgs.Location = new System.Drawing.Point(13, 82);
            this.OlproxyArgs.Margin = new System.Windows.Forms.Padding(2);
            this.OlproxyArgs.Name = "OlproxyArgs";
            this.OlproxyArgs.Size = new System.Drawing.Size(394, 20);
            this.OlproxyArgs.TabIndex = 4;
            // 
            // SelectExecutable
            // 
            this.SelectExecutable.FileName = "SelectExecutable";
            this.SelectExecutable.Filter = "Applications|*.exe";
            // 
            // StartButton
            // 
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartButton.Location = new System.Drawing.Point(12, 28);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(65, 24);
            this.StartButton.TabIndex = 9;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Location = new System.Drawing.Point(91, 28);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(65, 24);
            this.ExitButton.TabIndex = 9;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // MapUpdateButton
            // 
            this.MapUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapUpdateButton.Location = new System.Drawing.Point(12, 56);
            this.MapUpdateButton.Name = "MapUpdateButton";
            this.MapUpdateButton.Size = new System.Drawing.Size(65, 24);
            this.MapUpdateButton.TabIndex = 9;
            this.MapUpdateButton.Text = "Update";
            this.MapUpdateButton.UseVisualStyleBackColor = true;
            this.MapUpdateButton.Click += new System.EventHandler(this.MapUpdateButton_Click);
            // 
            // SelectDark
            // 
            this.SelectDark.AutoSize = true;
            this.SelectDark.Checked = true;
            this.SelectDark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectDark.Location = new System.Drawing.Point(17, 149);
            this.SelectDark.Name = "SelectDark";
            this.SelectDark.Size = new System.Drawing.Size(81, 17);
            this.SelectDark.TabIndex = 5;
            this.SelectDark.Text = "Dark theme";
            this.SelectDark.UseVisualStyleBackColor = true;
            this.SelectDark.CheckedChanged += new System.EventHandler(this.SelectDark_CheckedChanged);
            // 
            // ActivityLogListBox
            // 
            this.ActivityLogListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActivityLogListBox.Font = new System.Drawing.Font("Calibri", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityLogListBox.ForeColor = System.Drawing.Color.Black;
            this.ActivityLogListBox.FormattingEnabled = true;
            this.ActivityLogListBox.ItemHeight = 10;
            this.ActivityLogListBox.Location = new System.Drawing.Point(21, 28);
            this.ActivityLogListBox.Margin = new System.Windows.Forms.Padding(2);
            this.ActivityLogListBox.Name = "ActivityLogListBox";
            this.ActivityLogListBox.Size = new System.Drawing.Size(392, 120);
            this.ActivityLogListBox.TabIndex = 0;
            this.ActivityLogListBox.TabStop = false;
            this.ActivityLogListBox.MouseLeave += new System.EventHandler(this.ActivityLogListBox_MouseLeave);
            // 
            // UseEmbeddedOlproxy
            // 
            this.UseEmbeddedOlproxy.AutoSize = true;
            this.UseEmbeddedOlproxy.Checked = true;
            this.UseEmbeddedOlproxy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseEmbeddedOlproxy.Location = new System.Drawing.Point(17, 52);
            this.UseEmbeddedOlproxy.Name = "UseEmbeddedOlproxy";
            this.UseEmbeddedOlproxy.Size = new System.Drawing.Size(136, 17);
            this.UseEmbeddedOlproxy.TabIndex = 5;
            this.UseEmbeddedOlproxy.Text = "Use embedded Olproxy";
            this.UseEmbeddedOlproxy.UseVisualStyleBackColor = true;
            this.UseEmbeddedOlproxy.CheckedChanged += new System.EventHandler(this.UseEmbeddedOlproxy_CheckedChanged);
            // 
            // OverloadGroupBox
            // 
            this.OverloadGroupBox.Controls.Add(this.OverloadRunning);
            this.OverloadGroupBox.Controls.Add(this.OverloadExecutable);
            this.OverloadGroupBox.Controls.Add(this.label1);
            this.OverloadGroupBox.Controls.Add(this.label2);
            this.OverloadGroupBox.Controls.Add(this.OverloadArgs);
            this.OverloadGroupBox.Location = new System.Drawing.Point(18, 17);
            this.OverloadGroupBox.Name = "OverloadGroupBox";
            this.OverloadGroupBox.Size = new System.Drawing.Size(420, 117);
            this.OverloadGroupBox.TabIndex = 11;
            this.OverloadGroupBox.TabStop = false;
            this.OverloadGroupBox.Text = "Overload";
            // 
            // OverloadRunning
            // 
            this.OverloadRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.OverloadRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
            this.OverloadRunning.Location = new System.Drawing.Point(52, -3);
            this.OverloadRunning.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.OverloadRunning.Name = "OverloadRunning";
            this.OverloadRunning.Size = new System.Drawing.Size(22, 21);
            this.OverloadRunning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.OverloadRunning.TabIndex = 10;
            this.OverloadRunning.TabStop = false;
            this.OverloadRunning.Visible = false;
            // 
            // OlproxyGroupBox
            // 
            this.OlproxyGroupBox.Controls.Add(this.OlproxyRunning);
            this.OlproxyGroupBox.Controls.Add(this.OlproxyExecutable);
            this.OlproxyGroupBox.Controls.Add(this.label3);
            this.OlproxyGroupBox.Controls.Add(this.label4);
            this.OlproxyGroupBox.Controls.Add(this.OlproxyArgs);
            this.OlproxyGroupBox.Location = new System.Drawing.Point(18, 143);
            this.OlproxyGroupBox.Name = "OlproxyGroupBox";
            this.OlproxyGroupBox.Size = new System.Drawing.Size(420, 117);
            this.OlproxyGroupBox.TabIndex = 12;
            this.OlproxyGroupBox.TabStop = false;
            this.OlproxyGroupBox.Text = "Olproxy";
            // 
            // OlproxyRunning
            // 
            this.OlproxyRunning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.OlproxyRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
            this.OlproxyRunning.Location = new System.Drawing.Point(43, -3);
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
            this.UpdatingMaps.Location = new System.Drawing.Point(106, -3);
            this.UpdatingMaps.Name = "UpdatingMaps";
            this.UpdatingMaps.Size = new System.Drawing.Size(18, 18);
            this.UpdatingMaps.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.UpdatingMaps.TabIndex = 10;
            this.UpdatingMaps.TabStop = false;
            this.UpdatingMaps.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(12, 28);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(234, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "https://www.overloadmaps.com/data/mp.json";
            this.textBox1.TextChanged += new System.EventHandler(this.OverloadExecutable_TextChanged);
            this.textBox1.DoubleClick += new System.EventHandler(this.OverloadExecutable_MouseDoubleClick);
            // 
            // OptionsGroupBox
            // 
            this.OptionsGroupBox.Controls.Add(this.AutoUpdateMapsCheckBox);
            this.OptionsGroupBox.Controls.Add(this.UseDLCLocationCheckBox);
            this.OptionsGroupBox.Controls.Add(this.SelectDark);
            this.OptionsGroupBox.Controls.Add(this.UseOlproxyCheckBox);
            this.OptionsGroupBox.Controls.Add(this.UseOlmodCheckBox);
            this.OptionsGroupBox.Controls.Add(this.UseEmbeddedOlproxy);
            this.OptionsGroupBox.Location = new System.Drawing.Point(444, 17);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(258, 204);
            this.OptionsGroupBox.TabIndex = 13;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Options";
            // 
            // AutoUpdateMapsCheckBox
            // 
            this.AutoUpdateMapsCheckBox.AutoSize = true;
            this.AutoUpdateMapsCheckBox.Location = new System.Drawing.Point(17, 126);
            this.AutoUpdateMapsCheckBox.Name = "AutoUpdateMapsCheckBox";
            this.AutoUpdateMapsCheckBox.Size = new System.Drawing.Size(139, 17);
            this.AutoUpdateMapsCheckBox.TabIndex = 5;
            this.AutoUpdateMapsCheckBox.Text = "Update maps on startup";
            this.AutoUpdateMapsCheckBox.UseVisualStyleBackColor = true;
            this.AutoUpdateMapsCheckBox.Click += new System.EventHandler(this.AutoUpdateMaps_Click);
            // 
            // UseDLCLocationCheckBox
            // 
            this.UseDLCLocationCheckBox.AutoCheck = false;
            this.UseDLCLocationCheckBox.AutoSize = true;
            this.UseDLCLocationCheckBox.Enabled = false;
            this.UseDLCLocationCheckBox.Location = new System.Drawing.Point(17, 100);
            this.UseDLCLocationCheckBox.Name = "UseDLCLocationCheckBox";
            this.UseDLCLocationCheckBox.Size = new System.Drawing.Size(201, 17);
            this.UseDLCLocationCheckBox.TabIndex = 5;
            this.UseDLCLocationCheckBox.Text = "Use Overload DLC directory for maps";
            this.UseDLCLocationCheckBox.UseVisualStyleBackColor = true;
            this.UseDLCLocationCheckBox.CheckedChanged += new System.EventHandler(this.UseDLCLocationCheckBox_CheckedChanged);
            this.UseDLCLocationCheckBox.Click += new System.EventHandler(this.UseDLCLocationCheckBox_Click);
            // 
            // UseOlproxyCheckBox
            // 
            this.UseOlproxyCheckBox.AutoSize = true;
            this.UseOlproxyCheckBox.Checked = true;
            this.UseOlproxyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseOlproxyCheckBox.Location = new System.Drawing.Point(17, 28);
            this.UseOlproxyCheckBox.Name = "UseOlproxyCheckBox";
            this.UseOlproxyCheckBox.Size = new System.Drawing.Size(83, 17);
            this.UseOlproxyCheckBox.TabIndex = 5;
            this.UseOlproxyCheckBox.Text = "Use Olproxy";
            this.UseOlproxyCheckBox.UseVisualStyleBackColor = true;
            this.UseOlproxyCheckBox.CheckedChanged += new System.EventHandler(this.UseOlproxy_CheckedChanged);
            // 
            // UseOlmodCheckBox
            // 
            this.UseOlmodCheckBox.AutoSize = true;
            this.UseOlmodCheckBox.Checked = true;
            this.UseOlmodCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseOlmodCheckBox.Location = new System.Drawing.Point(17, 75);
            this.UseOlmodCheckBox.Name = "UseOlmodCheckBox";
            this.UseOlmodCheckBox.Size = new System.Drawing.Size(78, 17);
            this.UseOlmodCheckBox.TabIndex = 5;
            this.UseOlmodCheckBox.Text = "Use Olmod";
            this.UseOlmodCheckBox.UseVisualStyleBackColor = true;
            this.UseOlmodCheckBox.CheckedChanged += new System.EventHandler(this.UseOlmod_CheckedChanged);
            // 
            // LoggingGroupBox
            // 
            this.LoggingGroupBox.Controls.Add(this.ActivityLogListBox);
            this.LoggingGroupBox.Location = new System.Drawing.Point(12, 268);
            this.LoggingGroupBox.Name = "LoggingGroupBox";
            this.LoggingGroupBox.Size = new System.Drawing.Size(426, 164);
            this.LoggingGroupBox.TabIndex = 14;
            this.LoggingGroupBox.TabStop = false;
            this.LoggingGroupBox.Text = "Activity Log";
            // 
            // ActionsGroupBox
            // 
            this.ActionsGroupBox.Controls.Add(this.StartButton);
            this.ActionsGroupBox.Controls.Add(this.CreateDesktopShortcuts);
            this.ActionsGroupBox.Controls.Add(this.ExitButton);
            this.ActionsGroupBox.Location = new System.Drawing.Point(444, 363);
            this.ActionsGroupBox.Name = "ActionsGroupBox";
            this.ActionsGroupBox.Size = new System.Drawing.Size(258, 69);
            this.ActionsGroupBox.TabIndex = 15;
            this.ActionsGroupBox.TabStop = false;
            this.ActionsGroupBox.Text = "Actions";
            // 
            // CreateDesktopShortcuts
            // 
            this.CreateDesktopShortcuts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateDesktopShortcuts.Location = new System.Drawing.Point(170, 28);
            this.CreateDesktopShortcuts.Name = "CreateDesktopShortcuts";
            this.CreateDesktopShortcuts.Size = new System.Drawing.Size(65, 24);
            this.CreateDesktopShortcuts.TabIndex = 9;
            this.CreateDesktopShortcuts.Text = "Shortcuts";
            this.CreateDesktopShortcuts.UseVisualStyleBackColor = true;
            this.CreateDesktopShortcuts.Click += new System.EventHandler(this.CreateDesktopShortcuts_Click);
            // 
            // OverloadClientToolNotifyIcon
            // 
            this.OverloadClientToolNotifyIcon.Text = "Overload Server Tool";
            this.OverloadClientToolNotifyIcon.Visible = true;
            this.OverloadClientToolNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OverloadClientToolNotifyIcon_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.UpdatingMaps);
            this.groupBox1.Controls.Add(this.MapUpdateButton);
            this.groupBox1.Location = new System.Drawing.Point(444, 251);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 106);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Master map list URL";
            // 
            // OCTMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 445);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ActionsGroupBox);
            this.Controls.Add(this.LoggingGroupBox);
            this.Controls.Add(this.OptionsGroupBox);
            this.Controls.Add(this.OlproxyGroupBox);
            this.Controls.Add(this.OverloadGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "OCTMainForm";
            this.Text = "Overload Client Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.MouseEnter += new System.EventHandler(this.Main_MouseEnter);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.OverloadGroupBox.ResumeLayout(false);
            this.OverloadGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OverloadRunning)).EndInit();
            this.OlproxyGroupBox.ResumeLayout(false);
            this.OlproxyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OlproxyRunning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdatingMaps)).EndInit();
            this.OptionsGroupBox.ResumeLayout(false);
            this.OptionsGroupBox.PerformLayout();
            this.LoggingGroupBox.ResumeLayout(false);
            this.ActionsGroupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OverloadExecutable;
        private System.Windows.Forms.TextBox OverloadArgs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox OlproxyExecutable;
        private System.Windows.Forms.TextBox OlproxyArgs;
        private System.Windows.Forms.OpenFileDialog SelectExecutable;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button MapUpdateButton;
        private System.Windows.Forms.CheckBox SelectDark;
        private System.Windows.Forms.ListBox ActivityLogListBox;
        private System.Windows.Forms.CheckBox UseEmbeddedOlproxy;
        private System.Windows.Forms.GroupBox OverloadGroupBox;
        private System.Windows.Forms.GroupBox OlproxyGroupBox;
        private System.Windows.Forms.GroupBox OptionsGroupBox;
        private System.Windows.Forms.GroupBox LoggingGroupBox;
        private System.Windows.Forms.GroupBox ActionsGroupBox;
        private System.Windows.Forms.NotifyIcon OverloadClientToolNotifyIcon;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox UseDLCLocationCheckBox;
        private System.Windows.Forms.PictureBox UpdatingMaps;
        private System.Windows.Forms.PictureBox OverloadRunning;
        private System.Windows.Forms.PictureBox OlproxyRunning;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CreateDesktopShortcuts;
        private System.Windows.Forms.CheckBox AutoUpdateMapsCheckBox;
        private System.Windows.Forms.CheckBox UseOlproxyCheckBox;
        private System.Windows.Forms.CheckBox UseOlmodCheckBox;
    }
}

