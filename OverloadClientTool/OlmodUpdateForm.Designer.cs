namespace OverloadClientApplication
{
    partial class OlmodUpdateForm
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
            this.OlmodNewVersion = new System.Windows.Forms.Label();
            this.SkipUpdateButton = new System.Windows.Forms.Button();
            this.UpdateQuestion = new System.Windows.Forms.Label();
            this.OlmodCurrentVersion = new System.Windows.Forms.Label();
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
            this.OlmodNewVersion.AutoSize = true;
            this.OlmodNewVersion.Location = new System.Drawing.Point(22, 42);
            this.OlmodNewVersion.Name = "OCTNewVersion";
            this.OlmodNewVersion.Size = new System.Drawing.Size(74, 13);
            this.OlmodNewVersion.TabIndex = 1;
            this.OlmodNewVersion.Text = "Online version";
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
            this.OlmodCurrentVersion.AutoSize = true;
            this.OlmodCurrentVersion.Location = new System.Drawing.Point(22, 21);
            this.OlmodCurrentVersion.Name = "OCTCurrentVersion";
            this.OlmodCurrentVersion.Size = new System.Drawing.Size(83, 13);
            this.OlmodCurrentVersion.TabIndex = 1;
            this.OlmodCurrentVersion.Text = "Installed version";
            // 
            // OlmodUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(189, 142);
            this.ControlBox = false;
            this.Controls.Add(this.OlmodCurrentVersion);
            this.Controls.Add(this.UpdateQuestion);
            this.Controls.Add(this.OlmodNewVersion);
            this.Controls.Add(this.SkipUpdateButton);
            this.Controls.Add(this.UpgradeButton);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "OlmodUpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Olmod Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpgradeButton;
        private System.Windows.Forms.Label OlmodNewVersion;
        private System.Windows.Forms.Button SkipUpdateButton;
        private System.Windows.Forms.Label UpdateQuestion;
        private System.Windows.Forms.Label OlmodCurrentVersion;
    }
}

