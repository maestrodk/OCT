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

        public CustomCheckBox() : base()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Default colors for the checkbox.
            CheckBackColor = Color.Gray;
            CheckForeColor = Color.Red;

            base.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
            this.FlatAppearance.BorderColor = this.ForeColor;
        }

        protected override void OnPaint(PaintEventArgs paintEvent)
        {
            base.OnPaint(paintEvent);

            paintEvent.Graphics.DrawRectangle(new Pen(CheckForeColor), new Rectangle(0, 0, 14, 14));
            paintEvent.Graphics.FillRectangle(new SolidBrush(CheckBackColor), new Rectangle(1, 1, 13, 13));

            if (this.Checked)
            {
                paintEvent.Graphics.DrawLine(new Pen(this.CheckForeColor, 1),    2, 6,   5, 11);
                paintEvent.Graphics.DrawLine(new Pen(this.CheckForeColor, 1),   5, 11,  12, 2);
            }
        }
    }
}
