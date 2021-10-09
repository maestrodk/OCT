using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public class CustomListBox : ListBox
    {
        public Color ListBackColor { get; set; }
        public Color ListForeColor { get; set; }

        public CustomListBox() : base()
        {
            // Default colors for the checkbox.
            ListBackColor = Color.Gray;
            ListForeColor = Color.SteelBlue;

            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += OnDrawItem;
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

            BackColor = ListBackColor;
            ForeColor = ListForeColor;

            if (e.Index >= 0)
            {
                e.DrawBackground();
                // e.Graphics.DrawRectangle(Pens.Red, 2, e.Bounds.Y + 2, 14, 14); // Simulate an icon.

                var textRect = e.Bounds;                
                // textRect.X += 20;
                // textRect.Width -= 20;

                string itemText = DesignMode ? "CustomListBox" : Items[e.Index].ToString();
                TextRenderer.DrawText(e.Graphics, itemText, e.Font, textRect, ListForeColor, flags);
                e.DrawFocusRectangle();
            }
        }
    }
}
