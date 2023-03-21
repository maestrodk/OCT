using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public partial class OCTMain
    {
        private Color activeTextBoxColor;
        private Color inactiveTextBoxColor;

        public string HelpFileBytes
        {
            get { return Properties.Resources.Overload_Client_Tool_Help; }
        }

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

        public bool HideHiddenMaps
        {
            get { return Properties.Settings.Default.HideHiddenMaps; }
            set { Properties.Settings.Default.HideHiddenMaps = value; }
        }

        public bool TrayIcon
        {
            get { return Properties.Settings.Default.TrayOnly; }
            set { Properties.Settings.Default.TrayOnly = value; }
        }

        public bool MinimizeOnClose
        {
            get { return Properties.Settings.Default.OnlyMinimizeOnClose; }
            set { Properties.Settings.Default.OnlyMinimizeOnClose = value; }
        }

        public string StartClientHotkeyString
        {
            get { return Properties.Settings.Default.StartClientHotkeyString; }
            set { Properties.Settings.Default.StartClientHotkeyString = value; }
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

        public string D3App
        {
            get { return Properties.Settings.Default.D3App; }
            set { Properties.Settings.Default.D3App = value; }
        }

        public string D3Args
        {
            get { return Properties.Settings.Default.D3Args; }
            set { Properties.Settings.Default.D3Args = value; }
        }

        public string D2App
        {
            get { return Properties.Settings.Default.D2App; }
            set { Properties.Settings.Default.D2App = value; }
        }

        public string D1App
        {
            get { return Properties.Settings.Default.D1App; }
            set { Properties.Settings.Default.D1App = value; }
        }

        public string OlmodServerTrackerBaseUrl
        {
            get { return Properties.Settings.Default.trackerBaseUrl; }
            set { Properties.Settings.Default.trackerBaseUrl = value; }
        }

        public string OlmodServerNotes
        {
            get { return Properties.Settings.Default.notes; }
            set { Properties.Settings.Default.notes = value; }
        }

        public string OlmodServerName
        {
            get { return Properties.Settings.Default.serverName; }
            set { Properties.Settings.Default.serverName = value; }
        }

        public bool OlmodServerKeepListed
        {
            get { return Properties.Settings.Default.keepListed; }
            set { Properties.Settings.Default.keepListed = value; }
        }

        public bool OlmodIsServer
        {
            get { return Properties.Settings.Default.isServer; }
            set { Properties.Settings.Default.isServer = value; }
        }

        public bool OlproxyEmbedded
        {
            get { return Properties.Settings.Default.EmbeddedOlproxy; }
            set { Properties.Settings.Default.EmbeddedOlproxy = value; }
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

        public static bool SuppressWinKeys
        {
            get { return Properties.Settings.Default.SuppressWinKeys; }
            set { Properties.Settings.Default.SuppressWinKeys = value; }
        }

        public bool UseOlmod
        {
            get { return Properties.Settings.Default.UseOlmod; }
            set { Properties.Settings.Default.UseOlmod= value; }
        }

        public string MapListUrl
        {
            get { return Properties.Settings.Default.MapListUrl; }
            set { Properties.Settings.Default.MapListUrl = value; }
        }

        public bool UpdateOnlyExistingMaps
        {
            get { return Properties.Settings.Default.UpdateOnlyExistingMaps; }
            set { Properties.Settings.Default.UpdateOnlyExistingMaps = value; }
        }

        public bool IncludeMP
        {
            get { return Properties.Settings.Default.IncludeMP; }
            set { Properties.Settings.Default.IncludeMP = value; }
        }

        public bool IncludeSP
        {
            get { return Properties.Settings.Default.IncludeSP; }
            set { Properties.Settings.Default.IncludeSP = value; }
        }

        public bool IncludeCM
        {
            get { return Properties.Settings.Default.IncludeCM; }
            set { Properties.Settings.Default.IncludeCM = value; }
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

        public bool ShowFPS
        {
            get { return Properties.Settings.Default.ShowFPS; }
            set { Properties.Settings.Default.ShowFPS = value; }
        }
        
        public bool AutoStartServer
        {
            get { return Properties.Settings.Default.AutostartServer; }
            set { Properties.Settings.Default.AutostartServer = value; }
        }

        public bool OlmodAssistScoring
        {
            get { return Properties.Settings.Default.assistScoring; }
            set { Properties.Settings.Default.assistScoring = value; }
        }

        public bool StartMinimized
        {
            get { return Properties.Settings.Default.StartMinimized; }
            set { Properties.Settings.Default.StartMinimized = value; }
        }

        public bool TrayInsteadOfTaskBar
        {
            get { return Properties.Settings.Default.TrayOnly; }
            set { Properties.Settings.Default.TrayOnly = value; }
        }

        public string DefaultDisplay
        {
            get { return Properties.Settings.Default.DefaultDisplay; }
            set { Properties.Settings.Default.DefaultDisplay = value; }
        }

        public string GamingDisplay
        {
            get { return Properties.Settings.Default.GamingDisplay; }
            set { Properties.Settings.Default.GamingDisplay = value; }
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        public bool SwitchGaming
        {
            get { return Properties.Settings.Default.SwitchGaming; }
            set { Properties.Settings.Default.SwitchGaming = value; }
        }

        public bool SwitchDefault
        {
            get { return Properties.Settings.Default.SwitchDefault; }
            set { Properties.Settings.Default.SwitchDefault = value; }
        }

        public void FindOverloadInstall(bool onlyOverload = false, bool showInfo = false)
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
                if (showInfo) Info(String.Format($"Checking for STEAM registry key."));

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
                catch
                {
                    //logger?.ErrorLogMessage(String.Format($"Exception while checking STEAM registry key: {ex.Message}"));
                }

                // Check for a GOG install of Overload.
                if (showInfo) Info(String.Format($"Checking for GOG registry key."));

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
                catch
                {
                    //logger?.ErrorLogMessage(String.Format($"Exception while checking GOG registry key: {ex.Message}"));
                }

                // Check for a DVD install of Overload (KickStarter backer DVD).
                if (showInfo) Info(String.Format($"Checking for DVD registry key."));

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
                catch
                {
                    //logger?.ErrorLogMessage(String.Format($"Exception while checking DVD registry key: {ex.Message}"));
                }

                initPath = steamLocation ?? gogLocation ?? dvdLocation;

                if (String.IsNullOrEmpty(initPath))
                {
                    Error(String.Format($"Unable to autolocate Overload installation!"));
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

        public void LoadSettings(Dictionary<string, string> previousSettings = null)
        {
            LogDebugMessage("LoadSettings()");

            if (String.IsNullOrEmpty(OverloadPath)) FindOverloadInstall();

            // First restore old settings if we can match their setting name.
            if ((previousSettings != null) && (previousSettings.Count > 0))
            {
                SettingsPropertyCollection settings = Properties.Settings.Default.Properties;
                foreach (KeyValuePair<string, string> kvp in previousSettings)
                {
                    string name = kvp.Key;
                    string value = String.IsNullOrEmpty(kvp.Value) ? "" : kvp.Value;
                    try
                    {
                        if (settings[name] != null)
                        {
                            if (settings[name].PropertyType.Name == "String") Properties.Settings.Default[name] = value;
                            else if (settings[name].PropertyType.Name == "Boolean") Properties.Settings.Default[name] = (value.ToLower() == "true") ? true : false;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogDebugMessage($"Unable to restore previous setting '{name}': {ex.Message}");
                    }
                }
            }

            try
            {
                OverloadExecutable.Text = OverloadPath;
                OverloadArgs.Text = OverloadParameters;

                OlproxyExecutable.Text = OlproxyPath;
                OlproxyArgs.Text = OlproxyParameters;
                UseOlproxyCheckBox.Checked = UseOlproxy;
                UseEmbeddedOlproxy.Checked = OlproxyEmbedded;

                OlmodExecutable.Text = OlmodPath;
                AutoUpdateOlmod.Checked = OlmodAutoUpdate;
                UseOlmodCheckBox.Checked = UseOlmod;
                FrameTimeCheckBox.Checked = ShowFPS;
                UseOlmodGameDirArg.Checked = PassGameDirToOlmod;

                // Map settings.
                OnlineMapJsonUrl.Text = MapListUrl;
                AutoUpdateMapsCheckBox.Checked = AutoUpdateMaps;
                OnlyUpdateExistingMapsCheckBox.Checked = UpdateOnlyExistingMaps;
                UseDLCLocationCheckBox.Checked = UseDLCLocation;
                HideUnofficialMapsCheckBox.Checked = HideNonOfficialMaps;
                HideHiddenMapsCheckBox.Checked = HideHiddenMaps;
                MPMapsCheckBox.Checked = IncludeMP;
                SPMapsCheckBox.Checked = IncludeSP;
                CMMapsCheckBox.Checked = IncludeCM;

                // Pilot settings.
                AutoPilotsBackupCheckbox.Checked = AutoSavePilots;

                // General settings.
                AutoUpdateCheckBox.Checked = AutoUpdateOCT;
                EnableDebugCheckBox.Checked = Debugging;
                UseTrayIcon.Checked = TrayIcon;
                OnlyMinimizeOnClose.Checked = MinimizeOnClose;
                HotkeyStartClient.Text = StartClientHotkeyString;

                AutoStartCheckBox.Checked = AutoStartServer;
                MinimizeOnStartupCheckBox.Checked = StartMinimized;
                UseTrayIcon.Checked = TrayInsteadOfTaskBar;

                DefaultDisplayCheckBox.Checked = SwitchDefault;
                GamingDisplayCheckBox.Checked = SwitchGaming;
                SuppressWinKeysCheckBox.Checked = SuppressWinKeys;

                // Server settings.
                ServerTrackerName.Text = OlmodServerName;
                ServerTrackerNotes.Text = OlmodServerNotes;
                ServerTrackerUrl.Text = OlmodServerTrackerBaseUrl;
                ServerKeepListed.Checked = OlmodServerKeepListed;
                ServerAnnounceOnTrackerCheckBox.Checked = OlmodIsServer;
                AssistScoringCheckBox.Checked = OlmodAssistScoring;

                // Set clickable tracker URL.
                ClickableTrackerUrl.Text = ServerTrackerUrl.Text;

                // Descent 2 and 3 settings.
                Descent1Executable.Text = D1App;
                Descent2Executable.Text = D2App;
                Descent3Executable.Text = D3App;
                Descent3Args.Text = D3Args;

                // Check for change to new theme selection.
                if (String.IsNullOrEmpty(ActiveThemeName)) ActiveThemeName = "Dark Gray";
            }
            catch
            {
                DialogResult reset = MessageBox.Show("The configuration file seems to be corrupted. OCT will try to recover as best as possible but you should to check the settings.", "Uh-oh!");
                SaveSettings();
            }

            theme = Theme.GetThemeByName(ActiveThemeName);
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
            ServerRunning.Image = theme.IsRunningImage;

            Descent1Running.Image = theme.IsRunningImage;
            Descent2Running.Image = theme.IsRunningImage;
            Descent3Running.Image = theme.IsRunningImage;

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
                // Set group box title to highlight color but keep the color of its children to the theme settings.
                control.ForeColor = theme.TextHighlightColor;

                foreach (Control child in control.Controls) child.ForeColor = theme.PanelForeColor;
            }
            else if ((control.Name == "PilotsPanel") || (control.Name == "MapsPanel") || (control.Name == "ActiveThemePanel") || (control.Name == "ActivityLogPanel") || (control.Name == "TreeViewLogPanel") || (control.Name == "ServerViewPanel"))
            {
                // These panels contain a single listbox child control.
                // The panel is used to create a border around them.
                control.BackColor = theme.TextHighlightColor;
            }
            else if (control is CustomListBox)
            {
                CustomListBox listBox = control as CustomListBox;
                Color c = theme.InputBackColor;

                // Set text colors.
                listBox.ListBackColor = c;
                listBox.ListForeColor = theme.PanelForeColor;

                listBox.BackColor = c;
                listBox.ForeColor = theme.PanelForeColor;

                //checkBox.Invalidate();
            }
            else if ((control is ListBox) || (control is ListView) ||(control is TreeView))
            {
                Color c = theme.InputBackColor;

                // control.BackColor = theme.InputBackColor;
                // control.ForeColor = (control.Enabled) ? theme.InputForeColor : theme.PanelInactiveForeColor;
                control.BackColor = c;
                control.ForeColor = theme.PanelForeColor;
            }
            else if ((control is TextBox) || (control is RichTextBox) || (control is TabPage) || (control is CustomComboBox))
            {
                if (control is RichTextBox)
                {
                    (control as RichTextBox).SelectionColor = theme.ButtonEnabledBackColor;
                    (control as RichTextBox).SelectionBackColor = theme.ButtonEnabledForeColor;
                }
                else
                {
                    control.BackColor = theme.InputBackColor;
                    control.ForeColor = (control.Enabled) ? theme.InputForeColor : theme.PanelInactiveForeColor;
                }

                if (control is CustomComboBox)
                {
                    //(control as CustomComboBox).ComboBackColor = theme.InputBackColor; // theme.ButtonEnabledBackColor;
                    //(control as CustomComboBox).ComboForeColor = theme.InputForeColor; // theme.ButtonEnabledForeColor;
                    (control as CustomComboBox).ComboBackColor = theme.ButtonEnabledBackColor;
                    (control as CustomComboBox).ComboForeColor = theme.ButtonEnabledForeColor;
                    (control as CustomComboBox).ComboBorderColor = theme.InputForeColor;
                    (control as CustomComboBox).BorderColor = theme.ButtonEnabledBackColor;
                }
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

                // Set text colors.
                checkBox.BackColor = theme.PanelBackColor;
                checkBox.ForeColor = (checkBox.Enabled) ? theme.PanelForeColor : Color.Red;

                // Set checkmark colors.
                checkBox.CheckBackColor = theme.PanelBackColor;
                checkBox.CheckForeColor = theme.PanelForeColor;
                checkBox.CheckInactiveForeColor = theme.PanelInactiveForeColor;

                //checkBox.Invalidate();
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
