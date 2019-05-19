namespace OverloadClientApplication
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
            this.SkipUpdateButton = new System.Windows.Forms.Button();
            this.UpdateQuestion = new System.Windows.Forms.Label();
            this.OCTCurrentVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UpgradeButton
            // 
            this.UpgradeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpgradeButton.Location = new System.Drawing.Point(24, 103);
            this.UpgradeButton.Name = "UpgradeButton";
            this.UpgradeButton.Size = new System.Drawing.Size(57, 23);
            this.UpgradeButton.TabIndex = 0;
            this.UpgradeButton.Text = "Yes";
            this.UpgradeButton.UseVisualStyleBackColor = true;
            this.UpgradeButton.Click += new System.EventHandler(this.UpgradeButton_Click);
            // 
            // OCTNewVersion
            // 
            this.OCTNewVersion.AutoSize = true;
            this.OCTNewVersion.Location = new System.Drawing.Point(22, 42);
            this.OCTNewVersion.Name = "OCTNewVersion";
            this.OCTNewVersion.Size = new System.Drawing.Size(74, 13);
            this.OCTNewVersion.TabIndex = 1;
            this.OCTNewVersion.Text = "Online version";
            // 
            // SkipUpdateButton
            // 
            this.SkipUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SkipUpdateButton.Location = new System.Drawing.Point(102, 103);
            this.SkipUpdateButton.Name = "SkipUpdateButton";
            this.SkipUpdateButton.Size = new System.Drawing.Size(57, 23);
            this.SkipUpdateButton.TabIndex = 1;
            this.SkipUpdateButton.Text = "Not now";
            this.SkipUpdateButton.UseVisualStyleBackColor = true;
            this.SkipUpdateButton.Click += new System.EventHandler(this.DeclineUpgrade_Click);
            // 
            // UpdateQuestion
            // 
            this.UpdateQuestion.AutoSize = true;
            this.UpdateQuestion.ForeColor = System.Drawing.Color.SkyBlue;
            this.UpdateQuestion.Location = new System.Drawing.Point(21, 74);
            this.UpdateQuestion.Name = "UpdateQuestion";
            this.UpdateQuestion.Size = new System.Drawing.Size(144, 13);
            this.UpdateQuestion.TabIndex = 1;
            this.UpdateQuestion.Text = "Upgrade to the new version?";
            // 
            // OCTCurrentVersion
            // 
            this.OCTCurrentVersion.AutoSize = true;
            this.OCTCurrentVersion.Location = new System.Drawing.Point(22, 21);
            this.OCTCurrentVersion.Name = "OCTCurrentVersion";
            this.OCTCurrentVersion.Size = new System.Drawing.Size(83, 13);
            this.OCTCurrentVersion.TabIndex = 1;
            this.OCTCurrentVersion.Text = "Installed version";
            // 
            // OCTUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(199, 142);
            this.Controls.Add(this.OCTCurrentVersion);
            this.Controls.Add(this.UpdateQuestion);
            this.Controls.Add(this.OCTNewVersion);
            this.Controls.Add(this.SkipUpdateButton);
            this.Controls.Add(this.UpgradeButton);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OCTUpdateForm";
            this.Text = "OCT Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpgradeButton;
        private System.Windows.Forms.Label OCTNewVersion;
        private System.Windows.Forms.Button SkipUpdateButton;
        private System.Windows.Forms.Label UpdateQuestion;
        private System.Windows.Forms.Label OCTCurrentVersion;
    }
}

