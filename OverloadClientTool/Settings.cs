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
    public partial class OCTMain
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

        public string OlmodPath
        {
            get { return Properties.Settings.Default.OlmodPath; }
            set { Properties.Settings.Default.OlmodPath = value; }
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

        public static bool Debugging
        {
            get { return Properties.Settings.Default.DebugLogging; }
            set { Properties.Settings.Default.DebugLogging = value; }
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

        public bool AutoSavePilots
        {
            get { return Properties.Settings.Default.AutoSavePilots; }
            set { Properties.Settings.Default.AutoSavePilots = value; }
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        public void FindOverloadInstall(bool onlyOverload = false)
        {
            LogDebugMessage("FindOverloadInstall()");

            bool found = false;
            try
            {
                found = new FileInfo(OverloadPath).Exists;
            }
            catch
            {
            }

            if (!found)
            {
                string steamLocation = null;
                string gogLocation = null;
                string dvdLocation = null;

                // Check for a STEAM install of Overload.
                logger?.ErrorLogMessage(String.Format($"Checking for STEAM registry key."));

                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 448850"))
                        {
                            if (key != null)
                            {
                                try
                                {
                                    steamLocation = (string)key.GetValue("InstallLocation");
                                    string steamOverloadName = Path.Combine(steamLocation, "overload.exe");
                                    if (!File.Exists(Path.Combine(steamLocation, "overload.exe"))) steamLocation = null;
                                }
                                catch
                                {
                                    steamLocation = null;
                                }

                                if (String.IsNullOrEmpty(steamLocation))
                                {
                                    try
                                    {
                                        steamLocation = (string)key.GetValue("UninstallString");
                                        if (!String.IsNullOrEmpty(steamLocation))
                                        {
                                            string[] parts = steamLocation.Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            if (parts.Length > 1)
                                            {
                                                steamLocation = Path.Combine(Path.GetDirectoryName(parts[0]), @"steamapps\common\Overload");
                                            }
                                            else
                                            {
                                                steamLocation = null;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        steamLocation = null;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //logger?.ErrorLogMessage(String.Format($"Exception while checking STEAM registry key: {ex.Message}"));
                }

                // Check for a GOG install of Overload.
                logger?.ErrorLogMessage(String.Format($"Checking for GOG registry key."));

                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        using (var key = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\GOG.com\Games\1309632191"))
                        {
                            if (key != null) gogLocation = (string)key.GetValue("Path");
                            if (!String.IsNullOrEmpty(gogLocation)) if (!File.Exists(Path.Combine(gogLocation, "overload.exe"))) gogLocation = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //logger?.ErrorLogMessage(String.Format($"Exception while checking GOG registry key: {ex.Message}"));
                }

                // Check for a DVD install of Overload (KickStarter backer DVD).
                logger?.ErrorLogMessage(String.Format($"Checking for DVD registry key."));
                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        using (var key = hklm.OpenSubKey(@"SOFTWARE\WOW6432Node\Revival Productions, LLC\Overload"))
                        {
                            if (key != null) dvdLocation = (string)key.GetValue("Path");
                            if (!String.IsNullOrEmpty(dvdLocation)) if (!File.Exists(Path.Combine(dvdLocation, "overload.exe"))) dvdLocation = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //logger?.ErrorLogMessage(String.Format($"Exception while checking DVD registry key: {ex.Message}"));
                }

                initPath = steamLocation ?? gogLocation ?? dvdLocation;

                if (String.IsNullOrEmpty(initPath))
                {
                    logger?.ErrorLogMessage(String.Format($"Unable to autolocate Overload installation!"));
                    initPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                }

                string overloadFileName = Path.Combine(initPath, "overload.exe");
                string olproxyFileName = Path.Combine(initPath, "olproxy.exe");

                OverloadPath = overloadFileName;

                // Set Olproxy path.
                if (!onlyOverload) OlproxyPath = olproxyFileName;
            }

            // Set Olmod.exe path to Overload installation folder if not found.
            if (String.IsNullOrEmpty(OlmodPath) || !ValidFileName(OlmodPath))
            {
                OlmodPath = Path.Combine(initPath, "olmod.exe");
                if (OCTMain.ValidFileName(OlmodPath)) OlmodPath = Path.Combine(Path.GetDirectoryName(OverloadPath), "olmod.exe");
            }
            else
            {
                try
                {
                    string test = Path.Combine(Path.GetDirectoryName(OverloadPath), "olmod.exe");
                    if (new FileInfo(test).Exists) OlmodPath = test;
                }
                catch
                {
                }
            }

            OverloadExecutable.Text = OverloadPath;
            OlproxyExecutable.Text = OlproxyPath;
            OlmodExecutable.Text = OlmodPath;
        }

        private string initPath = "";

        public void LoadSettings()
        {
            LogDebugMessage("LoadSettings()");

            if (String.IsNullOrEmpty(OverloadPath)) FindOverloadInstall();

            OlmodExecutable.Text = OlmodPath;

            OverloadExecutable.Text = OverloadPath;
            OverloadArgs.Text = OverloadParameters;

            OlproxyExecutable.Text = OlproxyPath;
            OlproxyArgs.Text = OlproxyParameters;

            UseEmbeddedOlproxy.Checked = OlproxyEmbedded;
            DarkThemeCheckBox.Checked = DarkTheme;
            UseOlmodCheckBox.Checked = UseOlmod;
            UseOlproxyCheckBox.Checked = UseOlproxy;
            AutoUpdateMapsCheckBox.Checked = AutoUpdateMaps;
            UseDLCLocationCheckBox.Checked = UseDLCLocation;
            AutoPilotsBackupCheckbox.Checked = AutoSavePilots;

            // Get debug setting and update debug file name info.
            EnableDebugCheckBox.Checked = Debugging;
            DebugFileNameLink.Visible = EnableDebugCheckBox.Checked;
        }

        private void UpdateTheme(Theme theme)
        {
            LogDebugMessage("UpdateTheme()");

            this.BackColor = theme.BackColor;
            this.ForeColor = theme.ForeColor;

            activeTextBoxColor = theme.ForeColor;
            inactiveTextBoxColor = theme.InvalidForeColor;

            UpdatingMaps.Image = theme.IsRunningImage;
            OlproxyRunning.Image = theme.IsRunningImage;
            OverloadRunning.Image = theme.IsRunningImage;

            // Set the active theme (recursively).
            ApplyThemeToControl(this, theme);
            paneController.SetTheme(theme);

            ValidateSettings();
        }

        private void DrawBorder(Control control) //DrawBorder(Control control, Color color, int width)
        {
            Color color = (DarkTheme) ? Color.Blue : Color.White;
            int width = 1;

            ControlPaint.DrawBorder(control.CreateGraphics(),
                control.ClientRectangle,
                color,
                width,
                ButtonBorderStyle.Solid,
                color,
                width,
                ButtonBorderStyle.Solid,
                color,
                width,
                ButtonBorderStyle.Solid,
                color,
                width,
                ButtonBorderStyle.Solid);
        }

        /// <summary>
        /// Recursively set control colors based on type.
        /// </summary>
        /// <param name="control"></param>
        public static void ApplyThemeToControl(Control control, Theme theme)
        {
            if (control.Controls.Count > 0) foreach (Control child in control.Controls) ApplyThemeToControl(child, theme);

            if ((control is GroupBox) || (control.Name == "StatusMessage"))
            {
                // Set group box title to blue but keep the color of its children to the theme settings.
                control.ForeColor = theme.TextHighlightColor;

                foreach (Control child in control.Controls) child.ForeColor = theme.ForeColor;
            }
            else if ((control is TextBox) || (control is ListBox) || (control is ListView) || (control is TabPage))
            {
                ApplyThemeToSingleControl(control, theme);
            }
            else if (control is RichTextBox)
            {
                control.BackColor = theme.BackColor;
                control.ForeColor = theme.ForeColor;
            }
            else if (control is LinkLabel)
            {
                LinkLabel link = control as LinkLabel;
                link.BackColor = theme.BackColor;
                link.ForeColor = theme.TextHighlightColor;
                link.VisitedLinkColor = theme.TextHighlightColor;
                link.LinkColor  = theme.TextHighlightColor;
                link.ActiveLinkColor = theme.TextHighlightColor;
            }
            else if (control is CheckBox)
            {
                control.ForeColor = theme.ForeColor;
            }
            else if (control is Button)
            {
                Button button = control as Button;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;

                // StartButton colors are controlled by ActivityBackgroundMonitor.
                ValidateButton(control, theme);
            }
        }

        public static void ApplyThemeToSingleControl(Control control, Theme theme)
        {
            control.BackColor = theme.ControlBackColor;
            control.ForeColor = theme.ControlForeColor;
        }
 
        /// <summary>
        /// Override default enabled/disabled colors for a Button control.
        /// </summary>
        /// <param name="control"></param>
        public static void ValidateButton(Control control, Theme theme)
        {
            if (control.Enabled)
            {
                control.BackColor = theme.ButtonEnabledBackColor;
                control.ForeColor = theme.ButtonEnabledForeColor;
            }
            else
            {
                control.BackColor = theme.ButtonDisabledBackColor;
                control.ForeColor = theme.ButtonDisabledForeColor;
            }
        }
    }
}
