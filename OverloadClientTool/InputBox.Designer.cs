namespace OverloadClientTool
{
    partial class InputBox
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
            this.InputDataGroupBox = new System.Windows.Forms.GroupBox();
            this.InputData = new System.Windows.Forms.TextBox();
            this.InputOKButton = new System.Windows.Forms.Button();
            this.InputCancelButton = new System.Windows.Forms.Button();
            this.InputDataGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputDataGroupBox
            // 
            this.InputDataGroupBox.Controls.Add(this.InputData);
            this.InputDataGroupBox.Location = new System.Drawing.Point(12, 12);
            this.InputDataGroupBox.Name = "InputDataGroupBox";
            this.InputDataGroupBox.Size = new System.Drawing.Size(273, 52);
            this.InputDataGroupBox.TabIndex = 12;
            this.InputDataGroupBox.TabStop = false;
            this.InputDataGroupBox.Text = "Overload Application";
            // 
            // InputData
            // 
            this.InputData.BackColor = System.Drawing.Color.Gray;
            this.InputData.ForeColor = System.Drawing.Color.White;
            this.InputData.Location = new System.Drawing.Point(17, 20);
            this.InputData.Margin = new System.Windows.Forms.Padding(1);
            this.InputData.Name = "InputData";
            this.InputData.Size = new System.Drawing.Size(243, 20);
            this.InputData.TabIndex = 1;
            this.InputData.TextChanged += new System.EventHandler(this.InputData_TextChanged);
            this.InputData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputData_KeyPress);
            // 
            // InputOKButton
            // 
            this.InputOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InputOKButton.Location = new System.Drawing.Point(159, 70);
            this.InputOKButton.Name = "InputOKButton";
            this.InputOKButton.Size = new System.Drawing.Size(60, 23);
            this.InputOKButton.TabIndex = 15;
            this.InputOKButton.Text = "OK";
            this.InputOKButton.UseVisualStyleBackColor = true;
            this.InputOKButton.Click += new System.EventHandler(this.InputOKButton_Click);
            // 
            // InputCancelButton
            // 
            this.InputCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.InputCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InputCancelButton.Location = new System.Drawing.Point(225, 70);
            this.InputCancelButton.Name = "InputCancelButton";
            this.InputCancelButton.Size = new System.Drawing.Size(60, 23);
            this.InputCancelButton.TabIndex = 15;
            this.InputCancelButton.Text = "Cancel";
            this.InputCancelButton.UseVisualStyleBackColor = true;
            // 
            // InputBox
            // 
            this.AcceptButton = this.InputOKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.CancelButton = this.InputCancelButton;
            this.ClientSize = new System.Drawing.Size(303, 103);
            this.Controls.Add(this.InputDataGroupBox);
            this.Controls.Add(this.InputCancelButton);
            this.Controls.Add(this.InputOKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.InputBox_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.InputBox_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.InputBox_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InputBox_MouseUp);
            this.InputDataGroupBox.ResumeLayout(false);
            this.InputDataGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox InputDataGroupBox;
        private System.Windows.Forms.TextBox InputData;
        private System.Windows.Forms.Button InputOKButton;
        private System.Windows.Forms.Button InputCancelButton;
    }
}