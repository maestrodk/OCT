using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public class CustomComboBox : ComboBox
    {
        private const int WM_PAINT = 0xF;
        private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;

        public Color ComboBackColor { get; set; }
        public Color ComboForeColor { get; set; }
        public Color ComboBorderColor { get; set; }

        public CustomComboBox() : base()
        {
            this.BorderColor = Color.DimGray;
            this.FlatStyle = FlatStyle.Flat;

            this.DrawItem += new DrawItemEventHandler(CustomComboBox_DrawItem);
        }

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "DimGray")]
        public Color BorderColor { get; set; }

        void CustomComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            CustomComboBox combo = sender as CustomComboBox;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(ComboForeColor), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(ComboBackColor), new Point(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(ComboBackColor), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(ComboForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }


            e.DrawFocusRectangle();

            using (var p = new Pen(ComboBorderColor, 1))
            {
                //e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            }
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    // Uncomment this if you don't want the "highlight border".                   
                    using (var p = new Pen(ComboBorderColor, 1))
                    {
                        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                    }
                    
                    using (var p = new Pen(ComboBackColor, 2))
                    {
                        g.DrawRectangle(p, 2, 2, Width - buttonWidth - 4, Height - 4);
                    }
                }
            }
        }

    }
}
