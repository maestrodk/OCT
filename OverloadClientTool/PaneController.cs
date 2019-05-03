using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public class PaneController
    {
        private Form parent;
        private Panel paneButtonLine;

        private Button activeButton = null;

        private Dictionary<Button, Panel> panels = new Dictionary<Button, Panel>();

        bool isDarkTheme = false;

        Color DarkControlBackColor = Color.FromArgb(72, 72, 72);
        Color LightControlBackColor = Color.LightSteelBlue;

        public PaneController(Form parentForm, Panel paneButtonLine)
        {
            this.parent = parentForm;
            this.paneButtonLine = paneButtonLine;
        }

        public void SetTheme(bool dark)
        {
            isDarkTheme = dark;
            SwitchToPane(activeButton);
        }

        public void SetupPaneButton(Button button, Panel panel)
        {
            panels.Add(button, panel);

            button.Enter += Pane_Enter;
            button.Leave += Pane_Leave;
            button.MouseHover += Pane_Hover;
            button.Click += Pane_Clicked;

            // Make first pane the active one.
            if (activeButton == null) activeButton = button;
        }

        public void SwitchToPane(Button paneButton)
        {
            // Deselect all except the new active button/panel.
            foreach (KeyValuePair<Button, Panel> kvp in panels)
            {
                Panel panel = kvp.Value;
                Button button = kvp.Key;

                if (paneButton != button)
                {
                    // Deselect pane.
                    panel.Visible = false;

                    // Set inactive button colors.
                    button.BackColor = (isDarkTheme) ? Color.DimGray : Color.White;
                }
                else
                {
                    // Set the size of parent (allow room for top button line).
                    parent.ClientSize = new Size(panel.Width, panel.Height + button.Height + 1);

                    // Show the pane.
                    panel.BackColor = (isDarkTheme) ? Color.DimGray : Color.White;
                    panel.Location = new Point(0, button.Height + 1);
                    panel.Visible = true;

                    Color backColor = (isDarkTheme) ? Color.SteelBlue : LightControlBackColor;

                    // Draw a line just below buttons.
                    paneButtonLine.Location = new Point(0, button.Height);
                    paneButtonLine.Size = new Size(panel.Width, 1);
                    paneButtonLine.BackColor = backColor;

                    // Set the active button colors.
                    button.BackColor = backColor;
                    button.ForeColor = (isDarkTheme) ? Color.White : Color.Black;
                }

                button.Refresh();
            }

            activeButton = paneButton;         
        }


        private void Pane_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == activeButton) return;
            SwitchToPane(button);
        }

        private void Pane_Enter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == activeButton) return;
        }

        private void Pane_Leave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == activeButton) return;
        }

        private void Pane_Hover(object sender, EventArgs e)
        {
        }
    }
}
