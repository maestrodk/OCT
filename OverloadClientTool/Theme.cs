using System;
using System.Drawing;

namespace OverloadClientTool
{
    // http://ajaxload.info/
    //
    // Circling arrows for light theme: 0x4169E1 / 0xFFFFFF. ??
    // Circling arrows for dark theme:  0x07CEFA / 0x323232.
    //
    // Color.SteelBlue == 0x4682B4
    //
    // 
    //

    public class Theme
    {
        /// <summary>
        /// Default background color for a control.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default background color for a control.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Default background color for a control.
        /// </summary>
        public Color InputBackColor { get; set; }

        /// <summary>
        /// Default text color for a control.
        /// </summary>
        public Color InputForeColor { get; set; }

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
        public Color PanelBackColor { get; set; }

        /// <summary>
        /// Active control forecolor for forms, panels and group boxes.
        /// </summary>
        public Color PanelForeColor { get; set; }

        /// <summary>
        /// Inactive control forecolor for forms, panels and group boxes.
        /// </summary>
        public Color PanelInactiveForeColor { get; set; }

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

        public static string[] AvailableThemes
        {
            get
            {
                return new string[] { "Black", "Dark Gray", "Dark Blue", "Light", "Brown", "Green", "Orange" };
            }
        }

        public static Theme GetThemeByName(string name)
        {
            if (name == "Black") return GetBlackTheme;
            if (name == "Dark Gray") return GetDarkGrayTheme;
            if (name == "Dark Blue") return GetDarkBlueTheme;
            if (name == "Green") return GetGreenTheme;
            if (name == "Brown") return GetBrownTheme;
            if (name == "Orange") return GetOrangeTheme;
            return GetLightTheme;
        }

        /// <summary>
        /// Returns a dark theme color set.
        /// </summary>
        public static Theme GetBlackTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Black";
                theme.Description = "A black theme with a few gray elements.";

                // Enabled button.
                theme.ButtonEnabledBackColor = Color.FromArgb(64, 64, 64);
                theme.ButtonEnabledForeColor = Color.FromArgb(192, 192, 192);

                // Disabled button.
                theme.ButtonDisabledBackColor = Color.FromArgb(64, 64, 64);
                theme.ButtonDisabledForeColor = Color.FromArgb(160, 160, 160);  // This doesn't work as Windows overrides a disabled control forecolor.
 
                // Textbox and listboxes.
                theme.InputBackColor = Color.FromArgb(16, 16, 16);
                theme.InputForeColor = Color.LightGray;

                // Indicate invalid value in textbox.
                theme.InvalidForeColor = Color.LightCoral;

                // Links.
                theme.TextHighlightColor = Color.FromArgb(224, 224, 224);

                // Pane background and general text colors.
                theme.PanelBackColor = Color.FromArgb(0, 0, 0);
                theme.PanelForeColor = theme.ButtonEnabledForeColor;
                theme.PanelInactiveForeColor = Color.FromArgb(64, 64, 64);

                theme.ActivePaneButtonBackColor = theme.ButtonEnabledBackColor;
                theme.ActivePaneButtonForeColor = theme.ButtonEnabledForeColor;

                theme.InactivePaneButtonBackColor = theme.PanelBackColor;
                theme.InactivePaneButtonForeColor = Color.FromArgb(192, 192, 192);

                theme.IsRunningImage = Properties.Resources.arrows_white_on_black_000000_D3D3D3;

                return theme;
            }
        }

        /// <summary>
        /// Returns a dark theme color set.
        /// </summary>
        public static Theme GetDarkBlueTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Dark Blue";
                theme.Description = "A dark theme with some blue/gray elements.";

                theme.ButtonEnabledBackColor = Color.SteelBlue;
                theme.ButtonEnabledForeColor = Color.FromArgb(255, 255, 255);

                theme.ButtonDisabledBackColor = Color.FromArgb(96, 96, 96);
                theme.ButtonDisabledForeColor = Color.FromArgb(255, 255, 255);

                theme.InputBackColor = Color.FromArgb(32, 32, 32);
                theme.InputForeColor = Color.LightGray; // 0xD3D3D3

                theme.TextHighlightColor = Color.SteelBlue; // 0x4682B4

                theme.InvalidForeColor = Color.LightCoral;

                theme.PanelBackColor = Color.FromArgb(24, 24, 24);
                theme.PanelForeColor = theme.InputForeColor;
                theme.PanelInactiveForeColor = Color.FromArgb(32, 64, 96);

                theme.ActivePaneButtonBackColor = theme.ButtonEnabledBackColor;
                theme.ActivePaneButtonForeColor = Color.White;

                theme.InactivePaneButtonBackColor = theme.PanelBackColor;
                theme.InactivePaneButtonForeColor = Color.White;

                theme.IsRunningImage = Properties.Resources.arrows_blue_on_dark_gray_181818_46782B4;

                return theme;
            }
        }

        /// <summary>
        /// Returns a dark theme color set.
        /// </summary>
        public static Theme GetDarkGrayTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Dark Gray";
                theme.Description = "A dark theme with some gray elements.";

                theme.ButtonEnabledBackColor = Color.FromArgb(64, 64, 64);
                theme.ButtonEnabledForeColor = Color.FromArgb(192, 192, 192);

                theme.ButtonDisabledBackColor = Color.FromArgb(96, 96, 96);
                theme.ButtonDisabledForeColor = Color.FromArgb(255, 255, 255);

                theme.InputBackColor = Color.FromArgb(32, 32, 32);
                theme.InputForeColor = Color.FromArgb(0xC0, 0xC0, 0xC0);

                theme.TextHighlightColor = Color.LightGray;

                theme.InvalidForeColor = Color.LightCoral;

                theme.PanelBackColor = Color.FromArgb(24, 24, 24);
                theme.PanelForeColor = theme.InputForeColor;
                theme.PanelInactiveForeColor = Color.FromArgb(96, 96, 96);

                theme.ActivePaneButtonBackColor = theme.ButtonEnabledBackColor;
                theme.ActivePaneButtonForeColor = Color.White;

                theme.InactivePaneButtonBackColor = theme.PanelBackColor;
                theme.InactivePaneButtonForeColor = Color.White;

                theme.IsRunningImage = Properties.Resources.arrows_white_on_gray_181818_D3D3D3;

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
                theme.Description = "A light theme with hints of blue.";

                theme.ButtonEnabledBackColor = Color.FromArgb(200, 200, 200);
                theme.ButtonEnabledForeColor = Color.FromArgb(64, 64, 64);

                theme.ButtonDisabledBackColor = Color.FromArgb(224, 224, 224);
                theme.ButtonDisabledForeColor = Color.FromArgb(192, 192, 192);

                theme.InputBackColor = Color.FromArgb(243, 248, 255);
                theme.InputForeColor = Color.FromArgb(50, 50, 50);

                theme.TextHighlightColor = Color.SteelBlue;

                theme.InvalidForeColor = Color.Coral;

                theme.PanelBackColor = Color.White;
                theme.PanelForeColor = Color.FromArgb(50, 50, 50);
                theme.PanelInactiveForeColor = Color.FromArgb(64, 96, 160);  

                theme.ActivePaneButtonBackColor = Color.LightSteelBlue;
                theme.ActivePaneButtonForeColor = Color.Black;

                theme.InactivePaneButtonBackColor = Color.White;
                theme.InactivePaneButtonForeColor = Color.Black;

                theme.IsRunningImage = Properties.Resources.arrows_blue_on_white;

                return theme;
            }
        }

        public static Theme GetBrownTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Brown";
                theme.Description = "A brown theme with some yellow thrown in.";

                theme.ButtonEnabledBackColor = Color.FromArgb(192, 160, 32);
                theme.ButtonEnabledForeColor = Color.Black; //Color.FromArgb(224, 224, 192);

                theme.ButtonDisabledBackColor = Color.FromArgb(64, 64, 8);
                theme.ButtonDisabledForeColor = Color.FromArgb(224, 224, 192);

                theme.InputBackColor = Color.FromArgb(32, 32, 32);
                theme.InputForeColor = Color.FromArgb(0xD0, 0xE0, 0xA0);

                theme.TextHighlightColor = Color.FromArgb(208, 208, 160);

                theme.InvalidForeColor = Color.Coral;

                theme.PanelBackColor = Color.FromArgb(64, 32, 16);
                theme.PanelForeColor = Color.FromArgb(224, 224, 192);
                theme.PanelInactiveForeColor = Color.FromArgb(96, 96, 64);

                theme.ActivePaneButtonBackColor = Color.FromArgb(192, 160, 32);
                theme.ActivePaneButtonForeColor = Color.Black;

                theme.InactivePaneButtonBackColor = theme.PanelBackColor;
                theme.InactivePaneButtonForeColor = Color.FromArgb(224, 224, 192);

                theme.IsRunningImage = Properties.Resources.arrows_yellow_on_brown_402010_C0A020;

                return theme;
            }
        }

        public static Theme GetGreenTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Green";
                theme.Description = "A green theme resembling Overload green menus.";

                theme.ButtonEnabledBackColor = Color.FromArgb(64, 160, 16);
                theme.ButtonEnabledForeColor = Color.Black;// Color.FromArgb(64, 224, 64);

                theme.ButtonDisabledBackColor = Color.FromArgb(32, 96, 24);
                theme.ButtonDisabledForeColor = Color.FromArgb(128, 128, 128);

                theme.InputBackColor = Color.FromArgb(16, 32, 8);
                theme.InputForeColor = Color.FromArgb(96, 224, 96);

                theme.TextHighlightColor = Color.FromArgb(192, 224, 128);

                theme.InvalidForeColor = Color.Coral;

                theme.PanelBackColor = Color.FromArgb(0, 16, 0);
                theme.PanelForeColor = Color.FromArgb(128, 255, 128);
                theme.PanelInactiveForeColor = Color.FromArgb(64, 96, 32);

                theme.ActivePaneButtonBackColor = theme.ButtonEnabledBackColor;
                theme.ActivePaneButtonForeColor = theme.ButtonEnabledForeColor;

                theme.InactivePaneButtonBackColor = theme.PanelBackColor;
                theme.InactivePaneButtonForeColor = theme.InputForeColor;

                theme.IsRunningImage = Properties.Resources.arrows_green_on_dark_green_001000_4CC013;

                return theme;
            }
        }

        public static Theme GetOrangeTheme
        {
            get
            {
                Theme theme = new Theme();

                theme.Name = "Orange";
                theme.Description = "A orange theme resembling Overload orange menus.";

                theme.ButtonEnabledBackColor = Color.FromArgb(224, 160, 16);
                theme.ButtonEnabledForeColor = Color.Black;

                theme.ButtonDisabledBackColor = Color.FromArgb(64, 64, 24);
                theme.ButtonDisabledForeColor = Color.FromArgb(255, 80, 32);

                theme.InputBackColor = Color.FromArgb(24, 16, 8);
                theme.InputForeColor = Color.FromArgb(180, 112, 48);

                theme.TextHighlightColor = Color.FromArgb(255, 192, 160);

                theme.InvalidForeColor = Color.Coral;

                theme.PanelBackColor = Color.FromArgb(16, 16, 0);
                theme.PanelForeColor = theme.InputForeColor;
                theme.PanelInactiveForeColor = Color.FromArgb(128, 64, 24); // theme.InputForeColor;

                theme.ActivePaneButtonBackColor = theme.ButtonEnabledBackColor;
                theme.ActivePaneButtonForeColor = theme.ButtonEnabledForeColor;

                theme.InactivePaneButtonBackColor = theme.PanelBackColor;
                theme.InactivePaneButtonForeColor = theme.InputForeColor;

                theme.IsRunningImage = Properties.Resources.arrows_orange_on_dark_brown_101000_FFC013;

                return theme;
            }
        }

    }
}