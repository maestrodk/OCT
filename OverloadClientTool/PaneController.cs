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
        private OCTMain parent;
        private Panel paneButtonLine;

        private Button activeButton = null;

        private Dictionary<Button, Panel> panels = new Dictionary<Button, Panel>();

        private Theme theme;

        public PaneController(Form parentForm, Panel paneButtonLine, Theme theme)
        {
            this.parent = (parentForm as OCTMain);
            this.parent.LogDebugMessage("PaneController.ctor()");
            this.theme = theme;
            this.paneButtonLine = paneButtonLine;
        }

        public void SetTheme(Theme theme)
        {
            this.theme = theme;

            foreach (KeyValuePair<Button, Panel> kvp in panels)
            {
                Button button = kvp.Key;
                Panel panel = kvp.Value;

                // Set the position and size of OCTMain form (allow room for line just beneath panel buttons).
                panel.Location = new Point(0, button.Height + 1);
                parent.ClientSize = new Size(panel.Width, panel.Height + button.Height + 1);

                // Apply colors to them.
                OCTMain.ApplyThemeToControl(panel, theme);

                // Need to set this here to override default backcolor as all other
                // panels use ActivePaneButtonBackColor to draw borders around the listbox controls.
                panel.BackColor = theme.PanelBackColor;
                
                // Draw line beneath buttons using same color as the active button.
                paneButtonLine.Location = new Point(0, button.Height);
                paneButtonLine.Size = new Size(panel.Width, 1);
                paneButtonLine.BackColor = theme.ActivePaneButtonBackColor;

                // Set button colors.
                if (kvp.Key == activeButton)
                {
                    kvp.Key.BackColor = theme.ActivePaneButtonBackColor;
                    kvp.Key.ForeColor = theme.ActivePaneButtonForeColor;
                }
                else
                {
                    kvp.Key.BackColor = theme.InactivePaneButtonBackColor;
                    kvp.Key.ForeColor = theme.InactivePaneButtonForeColor;
                }
            }

            SwitchToPane(activeButton);
        }

        public void SetupPaneButton(Button button, Panel panel)
        {
            button.BackColor = theme.InactivePaneButtonBackColor;
            button.ForeColor = theme.InactivePaneButtonForeColor;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            button.Enter += Pane_Enter;
            button.Leave += Pane_Leave;
            button.MouseHover += Pane_Hover;
            button.Click += Pane_Clicked;

            panels.Add(button, panel);

            // Make first pane the active one.
            if (activeButton == null) activeButton = button;
        }

        public void SwitchToPane(Button paneButton)
        {
            parent.LogDebugMessage("PaneController.SwitchToPane()");

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
                    button.BackColor = theme.InactivePaneButtonBackColor;
                    button.ForeColor = theme.InactivePaneButtonForeColor;
                }
                else
                {
                    // Select the pane.
                    panel.Visible = true;

                    // Set the active button colors.
                    button.BackColor = theme.ActivePaneButtonBackColor;
                    button.ForeColor = theme.ActivePaneButtonForeColor;                   
                }

                //button.Refresh();
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
