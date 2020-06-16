namespace OverloadClientApplication
{
    partial class OCTErrorForm
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
            this.CopyButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.ExceptionMessageText = new System.Windows.Forms.TextBox();
            this.CopyEmailButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CopyButton
            // 
            this.CopyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyButton.Location = new System.Drawing.Point(12, 223);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(79, 23);
            this.CopyButton.TabIndex = 0;
            this.CopyButton.Text = "Copy text";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Location = new System.Drawing.Point(252, 223);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(77, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 52);
            this.label1.TabIndex = 2;
            this.label1.Text = "Uh-oh! It seems that OCT crashed due to something unexpected :(\r\n\r\nTo help me to " +
    "find and fix this problem please copy the error\r\nshown below and email it to ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(144, 50);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(123, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "mickdk2010@gmail.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "            ";
            // 
            // ExceptionMessageText
            // 
            this.ExceptionMessageText.Location = new System.Drawing.Point(12, 78);
            this.ExceptionMessageText.Multiline = true;
            this.ExceptionMessageText.Name = "ExceptionMessageText";
            this.ExceptionMessageText.ReadOnly = true;
            this.ExceptionMessageText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ExceptionMessageText.ShortcutsEnabled = false;
            this.ExceptionMessageText.Size = new System.Drawing.Size(317, 128);
            this.ExceptionMessageText.TabIndex = 4;
            // 
            // CopyEmailButton
            // 
            this.CopyEmailButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CopyEmailButton.Location = new System.Drawing.Point(97, 223);
            this.CopyEmailButton.Name = "CopyEmailButton";
            this.CopyEmailButton.Size = new System.Drawing.Size(79, 23);
            this.CopyEmailButton.TabIndex = 0;
            this.CopyEmailButton.Text = "Copy email";
            this.CopyEmailButton.UseVisualStyleBackColor = true;
            this.CopyEmailButton.Click += new System.EventHandler(this.CopyEmailButton_Click);
            // 
            // OCTErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(343, 261);
            this.ControlBox = false;
            this.Controls.Add(this.ExceptionMessageText);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.CopyEmailButton);
            this.Controls.Add(this.CopyButton);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "OCTErrorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Error";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ExceptionMessageText;
        private System.Windows.Forms.Button CopyEmailButton;
    }
}

