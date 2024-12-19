using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public partial class CustomCheckBox : CheckBox
    {
        public Color CheckBackColor { get; set; }
        public Color CheckForeColor { get; set; }
        public Color CheckInactiveForeColor { get; set; }

        public CustomCheckBox() : base()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Default colors for the checkbox.
            CheckBackColor = Color.Gray;
            CheckForeColor = Color.SteelBlue;
            CheckInactiveForeColor = Color.Gray;

            base.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
            this.FlatAppearance.BorderColor = this.ForeColor;
        }

        protected override void OnPaint(PaintEventArgs paintEvent)
        {
            Color checkForeColor = (this.Enabled) ? this.CheckForeColor : this.CheckInactiveForeColor;

            base.OnPaint(paintEvent);

            // Clear the background.
            paintEvent.Graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, this.Width, this.Height));

            // Paint the checkbox.
            paintEvent.Graphics.DrawRectangle(new Pen(checkForeColor), new Rectangle(0, 0, 14, 14));
            paintEvent.Graphics.FillRectangle(new SolidBrush(this.CheckBackColor), new Rectangle(1, 1, 13, 13));

            // Paint the checkmark.
            if (this.Checked)
            {
                paintEvent.Graphics.DrawLine(new Pen(checkForeColor, 1),  2, 6,   5, 11);
                paintEvent.Graphics.DrawLine(new Pen(checkForeColor, 1),  5, 11,  12, 2);
            }

            // Paint the text.
            Rectangle rect = new Rectangle(16, 1, this.Width - 16, this.Height);
            TextRenderer.DrawText(paintEvent.Graphics, this.Text, Font, rect, this.ForeColor, this.BackColor, TextFormatFlags.Left | TextFormatFlags.Default);
        }
    }
}
