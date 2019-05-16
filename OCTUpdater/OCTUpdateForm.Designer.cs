namespace OCTUpdater
{
    partial class OCTUpdateForm
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
            this.UpgradeButton = new System.Windows.Forms.Button();
            this.OCTNewVersion = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.OCTCurrentVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UpgradeButton
            // 
            this.UpgradeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpgradeButton.Location = new System.Drawing.Point(247, 24);
            this.UpgradeButton.Name = "UpgradeButton";
            this.UpgradeButton.Size = new System.Drawing.Size(73, 23);
            this.UpgradeButton.TabIndex = 0;
            this.UpgradeButton.Text = "Upgrade";
            this.UpgradeButton.UseVisualStyleBackColor = true;
            this.UpgradeButton.Click += new System.EventHandler(this.UpgradeButton_Click);
            // 
            // OCTNewVersion
            // 
            this.OCTNewVersion.AutoSize = true;
            this.OCTNewVersion.Location = new System.Drawing.Point(24, 66);
            this.OCTNewVersion.Name = "OCTNewVersion";
            this.OCTNewVersion.Size = new System.Drawing.Size(66, 13);
            this.OCTNewVersion.TabIndex = 1;
            this.OCTNewVersion.Text = "New version";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(247, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Skip";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.DeclineUpgrade_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.SkyBlue;
            this.label2.Location = new System.Drawing.Point(24, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "A new version of OCT is available!";
            // 
            // OCTCurrentVersion
            // 
            this.OCTCurrentVersion.AutoSize = true;
            this.OCTCurrentVersion.Location = new System.Drawing.Point(24, 46);
            this.OCTCurrentVersion.Name = "OCTCurrentVersion";
            this.OCTCurrentVersion.Size = new System.Drawing.Size(78, 13);
            this.OCTCurrentVersion.TabIndex = 1;
            this.OCTCurrentVersion.Text = "Current version";
            // 
            // OCTUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(344, 107);
            this.Controls.Add(this.OCTCurrentVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OCTNewVersion);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UpgradeButton);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OCTUpdateForm";
            this.Text = "Overload Client Tool Updater";
            this.Load += new System.EventHandler(this.OCTUpdateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpgradeButton;
        private System.Windows.Forms.Label OCTNewVersion;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label OCTCurrentVersion;
    }
}

