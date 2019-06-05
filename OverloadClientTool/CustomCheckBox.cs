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
            CheckForeColor = Color.Red;
            CheckInactiveForeColor = Color.Orange;

            base.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
            this.FlatAppearance.BorderColor = this.ForeColor;
        }

        protected override void OnPaint(PaintEventArgs paintEvent)
        {
            base.OnPaint(paintEvent);

            Size size = this.Size;

            Color foreColor = (this.Enabled) ? this.CheckForeColor : this.CheckInactiveForeColor;

            // Paint the checkbox.
            paintEvent.Graphics.DrawRectangle(new Pen(foreColor), new Rectangle(0, 0, 14, 14));
            paintEvent.Graphics.FillRectangle(new SolidBrush(this.CheckBackColor), new Rectangle(1, 1, 13, 13));

            // Paint the checkmark.
            if (this.Checked)
            {
                paintEvent.Graphics.DrawLine(new Pen(foreColor, 1),  2, 6,   5, 11);
                paintEvent.Graphics.DrawLine(new Pen(foreColor, 1),  5, 11,  12, 2);
            }

            // Paint the text.
            paintEvent.Graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(15, 0, 19, this.Height));
            Rectangle rect = new Rectangle(16, 1, this.Width - 16, this.Height);
            TextRenderer.DrawText(paintEvent.Graphics, this.Text, Font, rect, foreColor, this.BackColor, TextFormatFlags.Left | TextFormatFlags.Default);
        }
    }
}
