using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public partial class InputBox : Form
    {
        OCTMain parent;
        Theme theme;

        public InputBox()
        {
            InitializeComponent();
        }

        public InputBox(string title, string label, string value, OCTMain parent, Theme theme) 
        {
            InitializeComponent();

            this.parent = parent;
            this.theme = theme;

            DialogResult = DialogResult.Cancel;
            StartPosition = FormStartPosition.CenterParent;

            Text = title;
            InputDataGroupBox.Text = label;
            Result = value;

            InputData.BorderStyle = BorderStyle.FixedSingle;
            InputData.Text = Result;
            InputOKButton.Enabled = (InputData.TextLength > 0);
        }

        public string Result = "";

        private void InputOKButton_Click(object sender, EventArgs e)
        {
            Result = InputData.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void InputData_TextChanged(object sender, EventArgs e)
        {
            InputOKButton.Enabled = (InputData.TextLength > 0);
        }

        private bool mouseDown;
        private Point lastLocation;

        private void InputBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void InputBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void InputBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void InputData_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            this.BackColor = theme.PanelBackColor;
            this.ForeColor = theme.PanelForeColor;

            InputData.BackColor = theme.InputBackColor;
            InputData.ForeColor = theme.InputForeColor;

            OCTMain.ApplyThemeToControl(this, theme);
        }
    }
}
