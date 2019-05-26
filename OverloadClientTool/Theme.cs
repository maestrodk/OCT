using System;
using System.Drawing;

namespace OverloadClientTool
{
    // http://ajaxload.info/
    //
    // Circling arrows for light theme: 0x4169E1 / 0xFFFFFF. 
    // Circling arrows for dark theme: 0x7CEFA / 0x3232328.

    public class Theme
    {
        /// <summary>
        /// Default background color for a control.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default background color for a control.
        /// </summary>
        public Color ControlBackColor { get; set; }

        /// <summary>
        /// Default text color for a control.
        /// </summary>
        public Color ControlForeColor { get; set; }

        /// <summary>
        /// Background color for a enabled button.
        /// </summary>
        public Color ButtonEnabledBackColor { get; set; }

        /// <summary>
        /// Text color for a enabled button.
        /// </summary>
        public Color ButtonEnabledForeColor { get; set; }

        /// <summary>
        /// Background color for a disabled button.
        /// </summary>
        public Color ButtonDisabledBackColor { get; set; }

        /// <summary>
        /// Text color for a disabled button.
        /// </summary>
        public Color ButtonDisabledForeColor { get; set; }

        /// <summary>
        /// Background color for focused pane button.
        /// </summary>
        public Color ActivePaneButtonBackColor { get; set; }

        /// <summary>
        /// Text for focused pane button.
        /// </summary>
        public Color ActivePaneButtonForeColor { get; set; }

        /// <summary>
        /// Background color for a inactive pane button.
        /// </summary>
        public Color InactivePaneButtonBackColor { get; set; }

        /// <summary>
        /// Text color for a inactive pane button.
        /// </summary>
        public Color InactivePaneButtonForeColor { get; set; }

        /// <summary>
        /// Background color for forms, panels and group boxes.
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// Text color for forms, panels and group boxes.
        /// </summary>
        public Color ForeColor { get; set; }

        /// <summary>
        /// Text color for invalid data in a TextBox/RichTextBox.
        /// </summary>
        public Color InvalidForeColor { get; set; }

        /// <summary>
        /// Color of title in GroupBox, links and other important text elements.
        /// </summary>
        public Color TextHighlightColor { get; set; }

        /// <summary>
        /// The image shown for active tasks/applications.
        /// </summary>
        public Image IsRunningImage;

        /// <summary>
        /// Returns a dark theme color set.
        /// </summary>
        public static Theme GetDarkTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Dark";

                theme.ButtonEnabledBackColor = Color.FromArgb(128, 128, 128);
                theme.ButtonEnabledForeColor = Color.FromArgb(255, 255, 255);
                theme.ButtonDisabledBackColor = Color.FromArgb(96, 96, 96);
                theme.ButtonDisabledForeColor = Color.FromArgb(255, 255, 255);

                theme.ControlBackColor = Color.FromArgb(32, 32, 32);
                theme.ControlForeColor = Color.LightGray;

                theme.TextHighlightColor = Color.SteelBlue;

                theme.InvalidForeColor = Color.LightCoral;

                theme.BackColor = Color.FromArgb(50, 50, 50);
                theme.ForeColor = Color.LightGray;

                theme.ActivePaneButtonBackColor = Color.SteelBlue;
                theme.ActivePaneButtonForeColor = Color.White;

                theme.InactivePaneButtonBackColor = Color.FromArgb(50, 50, 50);
                theme.InactivePaneButtonForeColor = Color.White;

                theme.IsRunningImage = Properties.Resources.arrows_light_blue_on_grey_2;

                return theme;
            }
        }

        /// <summary>
        /// Returns a dark theme color set.
        /// </summary>
        public static Theme GetLightTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Light";

                theme.ButtonEnabledBackColor = Color.FromArgb(200, 200, 200);
                theme.ButtonEnabledForeColor = Color.FromArgb(64, 64, 64);

                theme.ButtonDisabledBackColor = Color.FromArgb(224, 224, 224);
                theme.ButtonDisabledForeColor = Color.FromArgb(192, 192, 192);

                theme.ControlBackColor = Color.FromArgb(243, 248, 255);
                theme.ControlForeColor = Color.FromArgb(50, 50, 50);

                theme.TextHighlightColor = Color.SteelBlue;

                theme.InvalidForeColor = Color.Coral;

                theme.BackColor = Color.White;
                theme.ForeColor = Color.FromArgb(50, 50, 50);

                theme.ActivePaneButtonBackColor = Color.LightSteelBlue;
                theme.ActivePaneButtonForeColor = Color.Black;

                theme.InactivePaneButtonBackColor = Color.White;
                theme.InactivePaneButtonForeColor = Color.Black;

                theme.IsRunningImage = Properties.Resources.arrows_blue_on_white;

                return theme;
            }
        }
    }
}