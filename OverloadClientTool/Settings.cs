using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
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

        public bool HideNonOfficialMaps
        {
            get { return Properties.Settings.Default.HideNonOfficialMaps; }
            set { Properties.Settings.Default.HideNonOfficialMaps = value; }
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

        public string MapListUrl
        {
            get { return Properties.Settings.Default.MapListUrl; }
            set { Properties.Settings.Default.MapListUrl = value; }
        }

        public string OlproxyParameters
        {
            get { return Properties.Settings.Default.OlproxyParameters; }
            set { Properties.Settings.Default.OlproxyParameters = value; }
        }

        public string ActiveThemeName
        {
            get { return Properties.Settings.Default.ActiveThemeName; }
            set { Properties.Settings.Default.ActiveThemeName = value; }
        }

        public bool OlmodAutoUpdate
        {
            get { return Properties.Settings.Default.AutoUpdateOlmod; }
            set { Properties.Settings.Default.AutoUpdateOlmod = value; }
        }

        public bool PassGameDirToOlmod
        {
            get { return Properties.Settings.Default.PassGameDirToOlmod; }
            set { Properties.Settings.Default.PassGameDirToOlmod = value; }
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

        public bool UpdateOnlyExistingMaps
        {
            get { return Properties.Settings.Default.UpdateOnlyExistingMaps; }
            set { Properties.Settings.Default.UpdateOnlyExistingMaps = value; }
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

        public bool AutoUpdateOCT
        {
            get { return Properties.Settings.Default.AutoUpdateOCT; }
            set { Properties.Settings.Default.AutoUpdateOCT = value; }
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
                Info(String.Format($"Checking for STEAM registry key."));

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
                Info(String.Format($"Checking for GOG registry key."));

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
                Info(String.Format($"Checking for DVD registry key."));
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
                    Info(String.Format($"Unable to autolocate Overload installation!"));
                    initPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                }

                string overloadFileName = Path.Combine(initPath, "overload.exe");
                string olproxyFileName = Path.Combine(initPath, "olproxy.exe");

                OverloadPath = overloadFileName;

                // Set Olproxy path.
                if (!onlyOverload) OlproxyPath = olproxyFileName;
            }

            // Set Olmod.exe path to Overload installation folder if not found.
            if (String.IsNullOrEmpty(OlmodPath) || !OverloadClientToolApplication.ValidFileName(OlmodPath))
            {
                OlmodPath = Path.Combine(initPath, "olmod.exe");
                if (OverloadClientToolApplication.ValidFileName(OlmodPath)) OlmodPath = Path.Combine(Path.GetDirectoryName(OverloadPath), "olmod.exe");
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
            UseOlmodCheckBox.Checked = UseOlmod;
            UseOlproxyCheckBox.Checked = UseOlproxy;
            AutoUpdateMapsCheckBox.Checked = AutoUpdateMaps;
            UseDLCLocationCheckBox.Checked = UseDLCLocation;
            AutoPilotsBackupCheckbox.Checked = AutoSavePilots;
            OnlyUpdateExistingMapsCheckBox.Checked = UpdateOnlyExistingMaps;
            OnlineMapJsonUrl.Text = MapListUrl;
            AutoUpdateCheckBox.Checked = AutoUpdateOCT;
            UseOlmodGameDirArg.Checked = PassGameDirToOlmod;
            AutoUpdateOlmod.Checked = OlmodAutoUpdate;
            HideUnofficialMapsCheckBox.Checked = HideNonOfficialMaps;

            // Check for change to new theme selection.
            if (String.IsNullOrEmpty(ActiveThemeName)) ActiveThemeName = "Dark Gray";
            theme = Theme.GetThemeByName(ActiveThemeName);

            // Get debug setting and update debug file name info.
            EnableDebugCheckBox.Checked = Debugging;
        }
        
        private void UpdateTheme(Theme theme)
        {
            LogDebugMessage("UpdateTheme()");

            this.BackColor = theme.PanelBackColor;
            this.ForeColor = theme.PanelForeColor;

            activeTextBoxColor = theme.PanelForeColor;
            inactiveTextBoxColor = theme.InvalidForeColor;

            UpdatingMaps.Image = theme.IsRunningImage;
            OlproxyRunning.Image = theme.IsRunningImage;
            OverloadRunning.Image = theme.IsRunningImage;

            // Set the active theme (recursively).
            ApplyThemeToControl(this, theme);

            // Apply theme to specific controls.
            paneController.SetTheme(theme);

            ValidateSettings();
        }

        /// <summary>
        /// Recursively set control colors based on type.
        /// </summary>
        /// <param name="control"></param>
        public static void ApplyThemeToControl(Control control, Theme theme)
        {
            if (control.Controls.Count > 0) foreach (Control child in control.Controls) ApplyThemeToControl(child, theme);

            if ((control is GroupBox) || (control.Name == "StatusMessage") || (control.Name == "PilotNameLabel")) 
            {
                // Set group box title to blue but keep the color of its children to the theme settings.
                control.ForeColor = theme.TextHighlightColor;

                foreach (Control child in control.Controls) child.ForeColor = theme.PanelForeColor;
            }
            else if ((control.Name == "PilotsPanel") || (control.Name == "MapsPanel") || (control.Name == "ActiveThemePanel") || (control.Name == "ActivityLogPanel") || (control.Name == "TreeViewLogPanel"))
            {
                // These panels contain a single listbox child control.
                // The panel is used to create a border around them.
                control.BackColor = theme.TextHighlightColor;
            }
            else if ((control is TextBox) || (control is RichTextBox) || (control is ListBox) || (control is ListView) || (control is TabPage) || (control is TreeView))
            {
                control.BackColor = theme.InputBackColor;
                control.ForeColor = theme.InputForeColor;
            }
            else if ((control is ListBox) || (control is ListView) || (control is TreeView))
            {
                control.BackColor = theme.InputBackColor;
                control.ForeColor = theme.InputForeColor;
            }
            else if (control is LinkLabel)
            {
                LinkLabel link = control as LinkLabel;
                link.BackColor = theme.PanelBackColor;
                link.ForeColor = theme.TextHighlightColor;
                link.VisitedLinkColor = theme.TextHighlightColor;
                link.LinkColor  = theme.TextHighlightColor;
                link.ActiveLinkColor = theme.TextHighlightColor;
            }
            else if (control is Label)
            {
                control.BackColor = theme.PanelBackColor;
                control.ForeColor = theme.TextHighlightColor;
            }
            else if (control is CustomCheckBox)
            {
                CustomCheckBox checkBox = control as CustomCheckBox;

                checkBox.BackColor = theme.PanelBackColor;
                checkBox.ForeColor = theme.PanelForeColor;

                checkBox.CheckBackColor = theme.InputBackColor;
                checkBox.CheckForeColor = theme.TextHighlightColor;

                // Easy fix for text location.
                if (!checkBox.Text.StartsWith(" ")) checkBox.Text = " " + checkBox.Text;
            }
            else if (control is CheckBox)
            {
                CheckBox checkBox = control as CheckBox;
                checkBox.BackColor = theme.InputBackColor;
                checkBox.ForeColor = theme.InactivePaneButtonBackColor;
                // checkBox.FlatAppearance.CheckedBackColor = Color.White; // No effect?
            }
            else if (control is Button)
            {
                Button button = control as Button;
                ValidateButton(button, theme);
            }
        }
 
        /// <summary>
        /// Override default enabled/disabled colors for a Button control.
        /// </summary>
        /// <param name="control"></param>
        public static void ValidateButton(Button button, Theme theme)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            if (button.Enabled)
            {
                button.BackColor = theme.ButtonEnabledBackColor;
                button.ForeColor = theme.ButtonEnabledForeColor;
            }
            else
            {
                button.BackColor = theme.ButtonDisabledBackColor;
                button.ForeColor = theme.ButtonDisabledForeColor;
            }

            button.FlatAppearance.BorderColor = button.BackColor;
        }
    }
}
