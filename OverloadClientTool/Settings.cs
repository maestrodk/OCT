using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public partial class OCTMainForm
    {
        private Color activeTextBoxColor;
        private Color inactiveTextBoxColor;

        public string OverloadPath
        {
            get { return Properties.Settings.Default.OverloadPath; }
            set { Properties.Settings.Default.OverloadPath = value; }
        }

        public bool OlproxyEmbedded
        {
            get { return Properties.Settings.Default.EmbeddedOlproxy; }
            set { Properties.Settings.Default.EmbeddedOlproxy = value; }
        }

        public string OverloadParameters
        {
            get { return Properties.Settings.Default.OverloadParameters; }
            set { Properties.Settings.Default.OverloadParameters = value; }
        }

        public string OlproxyPath
        {
            get { return Properties.Settings.Default.OlproxyPath; }
            set { Properties.Settings.Default.OlproxyPath = value; }
        }

        public string OlproxyParameters
        {
            get { return Properties.Settings.Default.OlproxyParameters; }
            set { Properties.Settings.Default.OlproxyParameters = value; }
        }

        public bool DarkTheme
        {
            get { return Properties.Settings.Default.DarkTheme; }
            set { Properties.Settings.Default.DarkTheme = value; }
        }

        public bool UseOlproxy
        {
            get { return Properties.Settings.Default.UseOlproxy; }
            set { Properties.Settings.Default.UseOlproxy = value; }
        }

        public bool UseOlmod
        {
            get { return Properties.Settings.Default.UseOlmod; }
            set { Properties.Settings.Default.UseOlmod= value; }
        }

        public bool UseDLCLocation
        {
            get { return Properties.Settings.Default.UseDLCPath; }
            set { Properties.Settings.Default.UseDLCPath = value; }
        }

        public bool AutoUpdateMaps
        {
            get { return Properties.Settings.Default.AutoUpdateMaps; }
            set { Properties.Settings.Default.AutoUpdateMaps = value; }
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            // Attempt to find Overload installation path.
            // First verify the current setting.
            if (!String.IsNullOrEmpty(OverloadPath)) if (!File.Exists(OverloadPath)) OverloadPath = null;
            
            if (String.IsNullOrEmpty(OverloadPath))
            {
                string steamLocation = null;
                string gogLocation = null;
                string dvdLocation = null;

                // Check for a STEAM install of Overload.
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 448850"))
                    {
                        if (key != null)
                        {
                            steamLocation = (string)key.GetValue("InstallLocation");
                            if (!File.Exists(Path.Combine(steamLocation, "overload.exe"))) steamLocation = null;

                            if (String.IsNullOrEmpty(steamLocation))
                            {
                                steamLocation = (string)key.GetValue("UninstallString");
                                if (!String.IsNullOrEmpty(steamLocation))
                                {
                                    string[] parts = steamLocation.Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length > 1) steamLocation = Path.Combine(Path.GetDirectoryName(parts[0]), @"steamapps\common\Overload");
                                    else steamLocation = null;
                                }
                            }
                        }
                    }
                }

                // Check for a GOG install of Overload.
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\GOG.com\Games\1309632191"))
                    {
                        if (key != null) gogLocation = (string)key.GetValue("Path");
                        if (!String.IsNullOrEmpty(gogLocation)) if(!File.Exists(Path.Combine(gogLocation, "overload.exe"))) gogLocation = null;
                    }
                }

                // Check for a DVD install of Overload (KickStarter backer DVD).
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\Revival Productions, LLC\Overload"))
                    {
                        if (key != null) dvdLocation = (string)key.GetValue("Path");
                        if (!String.IsNullOrEmpty(dvdLocation)) if (!File.Exists(Path.Combine(dvdLocation, "overload.exe"))) dvdLocation = null;
                    }
                }

                string initPath = steamLocation ?? gogLocation ?? dvdLocation;

                if (String.IsNullOrEmpty(initPath)) initPath = "";

                string olmodFileName = Path.Combine(initPath, "olmod.exe");
                string overloadFileName = Path.Combine(initPath, "overload.exe");
                string olproxyFileName = Path.Combine(initPath, "olproxy.exe");

                // Set Overload/Olmod path.
                //if (File.Exists(olmodFileName)) OverloadPath = olmodFileName;
                //else OverloadPath = overloadFileName;
                OverloadPath = overloadFileName;

                // Set Olproxy path.
                OlproxyPath = olproxyFileName;
            }

            OverloadExecutable.Text = OverloadPath;
            OverloadArgs.Text = OverloadParameters;

            OlproxyExecutable.Text = OlproxyPath;
            OlproxyArgs.Text = OlproxyParameters;

            UseEmbeddedOlproxy.Checked = OlproxyEmbedded;
            SelectDark.Checked = DarkTheme;
            UseOlmodCheckBox.Checked = UseOlmod;
            UseOlproxyCheckBox.Checked = UseOlproxy;
            AutoUpdateMapsCheckBox.Checked = AutoUpdateMaps;
            UseDLCLocationCheckBox.Checked = UseDLCLocation;
                
            // The theme colors MUST be set BEFORE attempting to validate settings.
            // This is because ValidateSettings() checks the button colors to see if
            // it is safe to start any of the .exe files.
            SetTheme();

            ValidateSettings();
        }

        private void SetTheme()
        {
            if (DarkTheme)
            {
                // Dark theme colors.
                BackColor = Color.DimGray;
                ForeColor = Color.LightGray;

                activeTextBoxColor = Color.White;
                inactiveTextBoxColor = Color.LightCoral;

                UpdatingMaps.Image = global::OverloadClientTool.Properties.Resources.arrows_light_blue_on_grey;
                OlproxyRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_light_blue_on_grey;
                OverloadRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_light_blue_on_grey;
            }
            else
            {
                // Default textbox colors.
                BackColor = Color.White;
                ForeColor = Color.Black;
            
                activeTextBoxColor =  Color.Black;
                inactiveTextBoxColor = Color.Coral;

                UpdatingMaps.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
                OlproxyRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
                OverloadRunning.Image = global::OverloadClientTool.Properties.Resources.arrows_blue_on_white;
            }

            // Set the active theme (recursively).
            ApplyThemeToControl(this);

            ValidateSettings();
        }

        /// <summary>
        /// Recursively set control colors based on type.
        /// </summary>
        /// <param name="control"></param>
        private void ApplyThemeToControl(Control control)
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child);
                }
            }

            if (control is GroupBox)
            {
                // Set group box title to blue but keep the color of its children to the theme settings.
                control.ForeColor = (DarkTheme) ? Color.LightSkyBlue : Color.RoyalBlue;
                foreach (Control child in control.Controls)
                {
                    if (SelectDark.Checked) child.ForeColor = activeTextBoxColor;
                    else child.ForeColor = activeTextBoxColor;
                }
            }
            else if ((control is TextBox) || (control is ListBox))
            {
                if (SelectDark.Checked)
                {
                    control.BackColor = Color.FromArgb(72, 72, 72); 
                    control.ForeColor = activeTextBoxColor;
                }
                else
                {
                    control.BackColor = Color.White;
                    control.ForeColor = activeTextBoxColor;
                }

            }
            else if (control is CheckBox)
            {
                control.ForeColor = ForeColor;
            }
            else if (control is Button)
            {
                // StartButton colors are controlled by ActivityBackgroundMonitor.
                if (control != StartButton) ValidateButton(control);
            }
        }

        /// <summary>
        /// Override default enabled/disabled colors for a Button control.
        /// </summary>
        /// <param name="control"></param>
        public void ValidateButton(Control control)
        {
            if (control.Enabled)
            {
                control.BackColor = (DarkTheme) ? DarkButtonEnabledBackColor : LightButtonEnabledBackColor;
                control.ForeColor = (DarkTheme) ? DarkButtonEnabledForeColor: LightButtonEnabledForeColor;
            }
            else
            {
                control.BackColor = (DarkTheme) ? DarkButtonDisabledBackColor : LightButtonDisabledBackColor;
                control.ForeColor = (DarkTheme) ? DarkButtonDisabledForeColor : LightButtonDisabledForeColor;
            }
        }
    }
}
