using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using static OverloadClientTool.OverloadMap;

namespace OverloadClientTool
{
    public partial class OCTMain : Form
    {
        // Set a default them (might change when reading settings).
        public Theme theme = Theme.GetDarkGrayTheme;

        // Shortcut link for Startupt folde (if file exists the autostart is enabled).
        string shortcutFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "OverLoad Client Tool AutoStart.lnk");

        private bool autoStart = false;

        // This matches MJDict defined on Olproxy.
        private Dictionary<string, object> olproxyConfig = new Dictionary<string, object>();

        private olproxy.Program olproxyTask = null;
        private Thread olproxyThread = null;

        private OlmodManager olmodManager = null;

        private OverloadMapManager mapManager = null;
        private Thread mapManagerThread = null;

        private PaneController paneController = null;

        private DisplaySettings displaySettings = new DisplaySettings();

        // Directory for DLC.
        private string dlcLocation = null;

        private string debugFileName = null;

        public void LogDebugMessage(string message)
        {
            if (!Debugging) return;

            message = String.IsNullOrEmpty(message) ? Environment.NewLine : message + Environment.NewLine;
            try { System.IO.File.AppendAllText(debugFileName, String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}")); } catch { }
        }

        public OCTMain(string[] args, string debugFileName)
        {
            this.debugFileName = debugFileName;

            if (!Debugging)
            {
                try { System.IO.File.Delete(debugFileName); } catch { }
            }

            foreach (string a in args)
            {
                if (a.ToLower().Contains("-launched")) autoStart = true;
            }

            // Init map manager.
            mapManager = new OverloadMapManager(UpdateOnlyExistingMaps);

            // Initialize controls on main form.
            InitializeComponent();

            // Center main form on Desktop.
            this.StartPosition = FormStartPosition.CenterScreen;
            StatusMessage.Text = "Starting up...";

            // Setup pane control.
            paneController = new PaneController(this, PaneButtonLine, theme);
            paneController.SetupPaneButton(PaneSelectMain, PaneMain);
            paneController.SetupPaneButton(PaneSelectMapManager, PaneMaps);
            paneController.SetupPaneButton(PaneSelectPilots, PanePilots);
            paneController.SetupPaneButton(PaneSelectOverload, PaneOverload);
            paneController.SetupPaneButton(PaneSelectOlproxy, PaneOlproxy);
            paneController.SetupPaneButton(PaneSelectOlmod, PaneOlmod);
            paneController.SetupPaneButton(PaneSelectServer, PaneServer);
            paneController.SetupPaneButton(PaneSelectOptions, PaneOptions);

            // Load user preferences.
            LoadSettings();

            // Init theme listbox.
            AvailableThemesListBox.Items.Clear();
            AvailableThemesListBox.Items.AddRange(Theme.AvailableThemes);

            for (int i = 0; i < AvailableThemesListBox.Items.Count; i++)
            {
                if ((String)AvailableThemesListBox.Items[i] == theme.Name)
                {
                    AvailableThemesListBox.SelectedIndex = i;
                }
            }

            // Init pilots listbox start monitoring.
            InitPilotsListBox();

            // Init maps listbox and start monitoring.
            InitMapsListBox();

            // Prepare embedded OlproxyProgram instance before attempting to start thread.
            olproxyTask = new olproxy.Program();
            olproxyTask.SetLogger(Info);

            // Set logging for map manager.
            mapManager.SetLogger(Info, Error);

            // Init Olmod manager.
            olmodManager = new OlmodManager(Info, Error);

            // Create properties for Olproxy thread (will be update from TextBox fields whenever Olproxy is restarted).
            olproxyConfig.Add("isServer", false);
            olproxyConfig.Add("signOff", false);
            olproxyConfig.Add("trackerBaseUrl", "");
            olproxyConfig.Add("serverName", "");
            olproxyConfig.Add("notes", "");

            // Set Olproxy config.
            UpdateOlproxyConfig();
        }

        /// <summary>
        /// Creates or deletes shortcut link to startup OST.
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        private bool SetAutoStartup(bool create)
        {
            if (create)
            {
                try
                {
                    string appname = Assembly.GetExecutingAssembly().FullName.Remove(Assembly.GetExecutingAssembly().FullName.IndexOf(","));
                    string shortcutTarget = System.IO.Path.Combine(Application.StartupPath, appname + ".exe");

                    WshShell myShell = new WshShell();
                    WshShortcut myShortcut = (WshShortcut)myShell.CreateShortcut(shortcutFileName);

                    myShortcut.TargetPath = shortcutTarget;                 // Shortcut to OverloadServerTool.exe.
                    myShortcut.IconLocation = shortcutTarget + ",0";        // Use default application icon.
                    myShortcut.WorkingDirectory = Application.StartupPath;  // Working directory.
                    myShortcut.Arguments = "-launched";                     // Parameters sent to OverloadServerTool.exe.
                    myShortcut.Save();                                      // Create shortcut.

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to create autostart shortcut: {ex.Message}");
                }

                return false;
            }
            else
            {
                try
                {
                    if (System.IO.File.Exists(shortcutFileName)) System.IO.File.Delete(shortcutFileName);
                    else return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to remove autostart shortcut: {ex.Message}");
                }

                return true;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LogDebugMessage("Main_Load()");

            // Set controls colors according to the selected theme.
            UpdateTheme(theme);

            // Focus the first pane.
            paneController.SwitchToPane(PaneSelectMain);

            // Make sure no text is selected.
            Unfocus();

            // Check settings and update buttons.
            ValidateSettings();

            // Locate DLC folder.
            UpdateDLCLocation();

            // Announce ourself.
            Info("Overload Client Tool " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3) + " by Søren Michélsen.");
            Info("Olproxy 0.3.0 by Arne de Bruijn.");

            // Start background monitor for periodic log updates.
            Thread thread = new Thread(ActivityBackgroundMonitor);
            thread.IsBackground = true;
            thread.Start();

            // Check if we should auto-update maps on startup.
            if (AutoUpdateMapsCheckBox.Checked) MapUpdateButton_Click(null, null);

            // Check if we should auto-update Olmod on startup.
            if (OlmodAutoUpdate) UpdateOlmod_Click(null, null);

            if (OverloadClientToolApplication.ValidFileName(OlmodPath, true)) Info($"{OlmodVersionInfo}");
            else Verbose("Cannot locate Olmod.");

            // Check for startup options.
            OverloadClientToolNotifyIcon.Icon = Properties.Resources.oct_logo_484_12;
            this.ShowInTaskbar = !UseTrayIcon.Checked;
            OverloadClientToolNotifyIcon.Visible = UseTrayIcon.Checked;

            if (RunDedicatedServer)
            {
                if (UseTrayIcon.Checked) WindowState = FormWindowState.Minimized;
                StartButton_Click(null, null);
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }

            Defocus();

            // Check for OCT update.
            if (AutoUpdateOCT) UpdateCheck(debugFileName, false);
        }

        /// <summary>
        /// A little hack to make sure no control has focus.
        /// </summary>
        private void Unfocus()
        {
            OverloadExecutable.Focus();
            OverloadExecutable.Select(0, 0);

            OnlineMapJsonUrl.Focus();
            OnlineMapJsonUrl.Select(0, 0);

            Defocus();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Kill embedded Olproxy.
            StartStopOlproxyButton_Click(null, null);

            // Shutdown background workers.
            //StopMapsMonitoring();
            StopPilotsMonitoring();

            // Save settings for main application.
            try
            {
                SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save settings: {ex.Message}", "Error");
            }

            try
            {
                // Update config then save as json for standalone Olproxy.
                string alterateFileName = Path.Combine(OlproxyExecutable.Text, "appsettings.json");
                olproxyTask.SaveConfig(UpdateOlproxyConfig(), alterateFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save Olpropxy settings: {ex.Message}", "Error");
            }
        }

        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);

        public static string SpecialFolderLocalLowPath
        {
            get
            {
                Guid localLowId = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
                IntPtr pszPath = IntPtr.Zero;

                try
                {
                    int hr = SHGetKnownFolderPath(localLowId, 0, IntPtr.Zero, out pszPath);
                    if (hr >= 0) return Marshal.PtrToStringAuto(pszPath);

                    throw Marshal.GetExceptionForHR(hr);
                }
                finally
                {
                    if (pszPath != IntPtr.Zero) Marshal.FreeCoTaskMem(pszPath);
                }
            }
        }

        private void AddNewLogMessage(string text, bool error = false)
        {
            this.UIThread(delegate
            {
                string prefixedText = DateTime.Now.ToString("HH:mm:ss") + " " + text;
                LogTreeViewText(prefixedText, error);
            });
        }

        private void Info(string text)
        {
            AddNewLogMessage(text);
        }

        private void Verbose(string text)
        {
            AddNewLogMessage(text, true);
        }

        private void Error(string text)
        {
            AddNewLogMessage(text, true);
        }

        /// <summary>
        /// Update the DLC path if path to Overload gets updated by the user/load of settings.
        /// </summary>
        private void UpdateDLCLocation()
        {
            LogDebugMessage("UpdateDLCLocation()");

            try
            {
                if (System.IO.File.Exists(OverloadExecutable.Text))
                {
                    if (Directory.Exists(Path.GetDirectoryName(OverloadExecutable.Text))) CheckCreateDirectory(Path.Combine(Path.GetDirectoryName(OverloadExecutable.Text), "DLC"));

                    dlcLocation = Path.Combine(Path.GetDirectoryName(OverloadExecutable.Text), "DLC");
                    UseDLCLocationCheckBox.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogDebugMessage(String.Format($""));

                dlcLocation = null;
                UseDLCLocationCheckBox.Enabled = false;
                UseDLCLocation = false;
            }
        }

        /// <summary>
        /// Starts embedded Olproxy.
        /// </summary>
        private void OlproxyThread()
        {
            olproxyTask.Run(new string[1] { OverloadArgs.Text }, UpdateOlproxyConfig());
        }

        /// <summary> 
        /// Refresh and return Olproxy configuration object.
        /// </summary>
        /// <returns>A dictionary object matching MJDict</returns>
        private Dictionary<string, object> UpdateOlproxyConfig()
        {
            if (!RunDedicatedServer)
            {
                // Overload client.
                olproxyConfig["isServer"] = false;
                olproxyConfig["signOff"] = false;
                olproxyConfig["trackerBaseUrl"] = "";
                olproxyConfig["serverName"] = "";
                olproxyConfig["notes"] = "";
            }
            else
            {
                // Overload dedicated server.
                olproxyConfig["isServer"] = ServerAnnounceOnTrackerCheckBox.Checked;
                olproxyConfig["signOff"] = ServerAutoSignOffTracker.Checked;
                olproxyConfig["trackerBaseUrl"] = ServerTrackerUrl.Text;
                olproxyConfig["serverName"] = ServerTrackerName.Text;
                olproxyConfig["notes"] = ServerTrackerNotes.Text;
            }
            return olproxyConfig;
        }

        /// <summary>
        /// Start emnbedded Olproxy thread.
        /// </summary>
        private void StartOlproxyThread()
        {
            UpdateOlproxyConfig();
            olproxyThread = new Thread(OlproxyThread);
            olproxyThread.IsBackground = true;
            olproxyThread.Start();
        }

        /// <summary>
        /// Try to gracefully shutdown Olproxy thread.
        /// </summary>
        private void KillOlproxyThread()
        {
            if (olproxyThread != null)
            {
                if (olproxyTask.KillFlag == false) olproxyTask.KillFlag = true;

                int n = 25;
                while ((n-- > 0) && (olproxyTask.KillFlag == true))
                {
                    Thread.Sleep(100);
                }

                olproxyThread = null;
            }
        }

        public bool IsOlproxyRunning
        {
            get
            {
                bool running = false;

                try
                {
                    // Check Olproxy.
                    if (UseEmbeddedOlproxy.Checked)
                    {
                        if ((olproxyTask.KillFlag == false) && (olproxyThread != null) && olproxyThread.IsAlive) running = true;
                    }
                    else
                    {
                        try { running = (GetRunningProcess("olproxy") != null); } catch { }
                    }
                }
                catch
                {
                }

                return running;
            }
        }

        public bool IsOverloadRunning
        {
            get
            {
                bool running = false;
                try { if (GetRunningProcess("overload") != null) running = true; } catch { }
                return running;
            }
        }

        public bool IsOverloadOrOlmodRunning
        {
            get { return (IsOverloadRunning || IsOlmodRunning); }
        }

        public bool IsOlmodRunning
        {
            get
            {
                bool running = false;
                try { if (GetRunningProcess("olmod") != null) running = true; } catch { }
                return running;
            }
        }

        /// <summary>
        /// Background logging monitor. Sets GroupBox titles to reflect running status.
        /// </summary>
        private void ActivityBackgroundMonitor()
        {
            while (true)
            {
                Thread.Sleep(250);
                CycleTheme();
                Thread.Sleep(250);
                CycleTheme();
                Thread.Sleep(250);
                CycleTheme();

                bool overloadRunning = IsOverloadRunning;
                bool olproxyRunning = IsOlproxyRunning;
                bool olmodRunning = IsOlmodRunning;

                this.UIThread(delegate
                {
                    OverloadLogFileCheck();

                    string statusText = "Ready for some Overload action!";
                    string server = (RunDedicatedServer) ? " server" : "";

                    if (overloadRunning && !olproxyRunning && !olmodRunning) statusText = $"Overload{server}is running.";
                    else if (overloadRunning && olproxyRunning && !olmodRunning) statusText = $"Overload{server} and Olproxy (external) are running.";
                    else if (olmodRunning && !olproxyRunning) statusText = $"Overload{server} (using Olmod) is running.";
                    else if (olmodRunning && olproxyRunning) statusText = $"Overload{server} (using Olmod) and Olproxy are running.";
                    else if (olproxyRunning) statusText = "Olproxy is running.";
                    else
                    {
                        bool foundOverload = false;
                        bool foundOlmod = false;

                        try { foundOverload = (OverloadClientToolApplication.ValidFileName(OverloadExecutable.Text) && new FileInfo(OverloadExecutable.Text).Exists); } catch { }
                        try { foundOlmod = (OverloadClientToolApplication.ValidFileName(OlmodExecutable.Text) && new FileInfo(OlmodExecutable.Text).Exists); } catch { }

                        if (UseOlmodCheckBox.Checked && !foundOlmod) statusText = "Cannot find Olmod (check path)!";
                        else if (!UseOlmodCheckBox.Checked && !foundOverload) statusText = "Cannot find Overload (check path)!";
                    }

                    OverloadRunning.Visible = overloadRunning || olmodRunning;
                    OlproxyRunning.Visible = olproxyRunning;

                    UpdateOlmodButton.Enabled = !olmodRunning;
                    ServerEnableCheckBox.Enabled = !(overloadRunning || olmodRunning);

                    ApplyThemeToControl(ServerEnableCheckBox, theme);

                    StartStopButton.Text = (overloadRunning || olmodRunning) ? "Stop" : "Start";
                    StartStopOlproxyButton.Text = (olproxyRunning) ? "Stop" : "Start";

                    if (StartStopButton.Enabled)
                    {
                        StartStopButton.BackColor = theme.ButtonEnabledBackColor;
                        StartStopButton.ForeColor = theme.ButtonEnabledForeColor;
                    }
                    else
                    {
                        StartStopButton.BackColor = theme.ButtonDisabledBackColor;
                        StartStopButton.ForeColor = theme.ButtonDisabledForeColor;
                    }

                    if (UpdateOlmodButton.Enabled)
                    {
                        UpdateOlmodButton.BackColor = theme.ButtonEnabledBackColor;
                        UpdateOlmodButton.ForeColor = theme.ButtonEnabledForeColor;
                    }
                    else
                    {
                        UpdateOlmodButton.BackColor = theme.ButtonDisabledBackColor;
                        UpdateOlmodButton.ForeColor = theme.ButtonDisabledForeColor;
                    }

                    StatusMessage.Text = statusText;
                });
            }
        }

        private void CycleTheme()
        {
            this.UIThread(delegate
            {
                if (!PartyModeCheckBox.Checked) return;

                try
                {
                    if (AvailableThemesListBox.SelectedIndex >= 0)
                    {
                        if (AvailableThemesListBox.SelectedIndex == 6) AvailableThemesListBox.SelectedIndex = 0;
                        else AvailableThemesListBox.SelectedIndex++;
                    }
                }
                catch
                {
                }
            });
        }

        private void ValidateSettings()
        {
            TestSetTextBoxColor(OverloadExecutable);
            TestSetTextBoxColor(OlproxyExecutable);
            TestSetTextBoxColor(OlmodExecutable);
            ValidateButton(StartStopButton, theme);
        }

        private void TestSetTextBoxColor(TextBox textBox)
        {
            string path = textBox.Text.Trim();
            try
            {
                if (System.IO.File.Exists(path)) textBox.ForeColor = activeTextBoxColor;
                else textBox.ForeColor = inactiveTextBoxColor;
            }
            catch
            {
                textBox.ForeColor = inactiveTextBoxColor;
            }
        }

        private void OverloadExecutable_TextChanged(object sender, EventArgs e)
        {
            OverloadPath = OverloadExecutable.Text;
            ValidateSettings();
            UpdateDLCLocation();
        }

        private void OlproxyExecutable_TextChanged(object sender, EventArgs e)
        {
            OlproxyPath = OlproxyExecutable.Text;
            ValidateSettings();
        }

        private void OverloadExecutable_MouseDoubleClick(object sender, EventArgs e)
        {
            OverloadExecutable.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            SelectExecutable.FileName = Path.GetFileName(OverloadExecutable.Text);
            SelectExecutable.InitialDirectory = Path.GetDirectoryName(OverloadExecutable.Text);

            DialogResult result = SelectExecutable.ShowDialog();

            if (result == DialogResult.OK)
            {
                OverloadExecutable.Text = SelectExecutable.FileName;
                OverloadExecutable.SelectionLength = 0;

                OverloadPath = OverloadExecutable.Text;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void OlproxyExecutable_DoubleClick(object sender, EventArgs e)
        {
            OlproxyExecutable.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            SelectExecutable.FileName = Path.GetFileName(OlproxyExecutable.Text);
            SelectExecutable.InitialDirectory = Path.GetDirectoryName(OlproxyExecutable.Text);

            DialogResult result = SelectExecutable.ShowDialog();

            if (result == DialogResult.OK)
            {
                OlproxyExecutable.Text = SelectExecutable.FileName;
                OlproxyExecutable.SelectionLength = 0;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            StopButton_Click(null, null);
            Close();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartStopButton.Text == "Stop")
            {
                StopButton_Click(null, null);
                StartStopButton.Text = "Start";
            }
            else
            {
                Thread startOverloadThread = new Thread(LaunchOverload);
                startOverloadThread.IsBackground = true;
                startOverloadThread.Start();

                if (UseOlproxyCheckBox.Checked)
                {
                    Thread startOlproxyThread = new Thread(LaunchOlproxy);
                    startOlproxyThread.IsBackground = true;
                    startOlproxyThread.Start();
                }
            }
        }

        private void LaunchOlproxy()
        {
            Info("Starting up Olproxy.");

            string name = Path.GetFileNameWithoutExtension(OlproxyExecutable.Text).ToLower();
            string args = OlproxyArgs.Text;

            // Update external Olproxy config.
            string alterateFileName = Path.Combine(OlproxyExecutable.Text, "appsettings.json");
            olproxyTask.SaveConfig(UpdateOlproxyConfig(), alterateFileName);

            // Start application it is not already running.
            int running = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name) running++;
            }

            // If set to use external Olproxy and one instance is running the exit with OK status.
            if ((running == 1) && (UseEmbeddedOlproxy.Checked == false)) return;

            // If we get here either embedded Olproxy is selected or 0/more than one instance of the external Olproxy is running.
            KillRunningProcess(name);

            // Should have no running instances now.
            if (UseEmbeddedOlproxy.Checked)
            {
                if ((olproxyTask.KillFlag == false) && ((olproxyThread != null) && olproxyThread.IsAlive)) KillOlproxyThread();
                StartOlproxyThread();
                return;
            }

            // Make sure Oloroxy.exe exists.
            if (new FileInfo(OlproxyExecutable.Text).Exists == false)
            {
                MessageBox.Show("Missing Olproxy.exe!");
                return;
            }

            // Start Olproxy application.
            Process appStart = new Process();
            appStart.StartInfo = new ProcessStartInfo(OlproxyExecutable.Text, OlproxyArgs.Text);
            appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(OlproxyExecutable.Text);
            appStart.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            appStart.Start();
        }

        private void LaunchOverload()
        {
            string overloadPath = Path.GetDirectoryName(OverloadExecutable.Text);
            string overloadExe = Path.Combine(overloadPath, "overload.exe");
            string olmodExe = Path.Combine(overloadPath, "olmod.exe");

            string name = null;
            string app = null;

            // This will be enabled again by the background task.
            this.UIThread(delegate 
            {
                ServerEnableCheckBox.Enabled = false;
                ApplyThemeToControl(ServerEnableCheckBox, theme);
            });

            // If Olmod is enabled check if we should pass Overload install folder.
            string olmodStartupArgs = "";
            if (OverloadClientToolApplication.ValidDirectoryName(Path.GetDirectoryName(OverloadExecutable.Text), true))
            {
                olmodStartupArgs = " -gamedir \"" + Path.GetDirectoryName(OverloadExecutable.Text) + "\"";

                if (ShowFPS && !olmodStartupArgs.ToLower().Contains("-frametime")) olmodStartupArgs += " -frametime";

                if (RunDedicatedServer && !olmodStartupArgs.ToLower().Contains("-batchmode")) olmodStartupArgs += " -batchmode";
                if (RunDedicatedServer && !olmodStartupArgs.ToLower().Contains("-nographics")) olmodStartupArgs += " -nographics";
            }

            olmodStartupArgs = olmodStartupArgs.Trim();

            if (AutoPilotsBackupCheckbox.Checked) PilotBackupButton_Click(null, null);

            if (UseOlmodCheckBox.Checked)
            {
                if (System.IO.File.Exists(olmodExe))
                {
                    name = Path.GetFileNameWithoutExtension(olmodExe);
                    app = olmodExe;
                }
                else
                {
                    MessageBox.Show("Olmod.exe not found!");
                    return;
                }
            }
            else
            {
                if (System.IO.File.Exists(overloadExe))
                {
                    name = Path.GetFileNameWithoutExtension(overloadExe);
                    app = overloadExe;
                }
                else
                {
                    MessageBox.Show("Overload.exe not found in Overload path!");
                    return;
                }
            }

            // Start application it is not already running.
            int running = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name) running++;
            }

            if (running == 1)
            {
                if (name.ToLower().Contains("olmod")) Info("Overload (Olmod) ís already running.");
                else Info("Overload is already running.");

                return;
            }

            string server = (RunDedicatedServer) ? " dedicated server" : "";
            if (name.ToLower().Contains("olmod")) Info($"Starting up Overload{server} (using Olmod).");
            else Info($"Starting up Overload{server}.");

            // If more than one is running we kill them all and start fresh instance.
            if (running > 1) KillRunningProcess(name);

            // (Re)start application..
            Process appStart = new Process();
            appStart.StartInfo = new ProcessStartInfo(Path.GetFileName(app), (olmodStartupArgs + OverloadArgs.Text).Trim());
            appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(app);
            appStart.Start();
        }

        // Return process if instance is active otherwise return null.
        public Process GetRunningProcess(string name)
        {
            if (String.IsNullOrEmpty(name)) return null;
            foreach (Process process in Process.GetProcesses()) if (process.ProcessName.ToLower() == name.ToLower()) return process;
            return null;
        }

        private void KillRunningProcess(string name)
        {
            foreach (Process process in Process.GetProcesses()) if (process.ProcessName.ToLower() == name.ToLower()) process.Kill();
        }

        // listBoxLog.Log(Level.Debug, "A debug level message");
        // listBoxLog.Log(Level.Verbose, "A verbose level message");
        // listBoxLog.Log(Level.Info, "A info level message");
        // listBoxLog.Log(Level.Warning, "A warning level message");
        // listBoxLog.Log(Level.Error, "A error level message");
        // listBoxLog.Log(Level.Critical, "A critical level message");
        // listBoxLog.Paused = !listBoxLog.Paused;

        private void StopButton_Click(object sender, EventArgs e)
        {
            Info("Shutting down active tasks.");

            ValidateButton(StartStopButton, theme);

            Defocus();

            KillRunningProcess("overload");
            KillRunningProcess("olmod");

            StartStopOlproxyButton_Click(null, null);
        }

        /// <summary>
        /// Unfocus all controls.
        /// </summary>
        private void Defocus()
        {
            label1.Focus();
        }

        private void StopExitButton_Click(object sender, EventArgs e)
        {
            StopButton_Click(null, null);
            Close();
        }

        private void UseEmbeddedOlproxy_CheckedChanged(object sender, EventArgs e)
        {
            OlproxyEmbedded = UseEmbeddedOlproxy.Checked;

            Info((UseEmbeddedOlproxy.Checked) ? "Using embedded Olproxy." : "Using standalone Olproxy application.");

            if (UseEmbeddedOlproxy.Checked == true)
            {
                // Kill Olproxy application if it is running.
                string olproxyName = Path.GetFileNameWithoutExtension(OlproxyExecutable.Text).ToLower();
                Process process = GetRunningProcess(olproxyName);
                if (process != null)
                {
                    // Kill running Olproxy application. 
                    process.Kill();

                    // Start embedded Olproxy task.
                    StartOlproxyThread();
                }
            }
            else
            {
                // Kill Olproxy task.
                if ((olproxyTask != null) && (olproxyTask.KillFlag == false) && ((olproxyThread != null) && olproxyThread.IsAlive))
                {
                    KillOlproxyThread();
                    LaunchOlproxy();
                }
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                // Tray or minimize to task bar?
                if (UseTrayIcon.Checked)
                {
                    Hide();
                    OverloadClientToolNotifyIcon.Visible = true;
                }
            }
        }

        private void OverloadClientToolNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            //OverloadClientToolNotifyIcon.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void UseOlmod_CheckedChanged(object sender, EventArgs e)
        {
            UseOlmod = UseOlmodCheckBox.Checked;
            Info((UseOlmodCheckBox.Checked) ? "Olmod enabled." : "Olmod disabled.");
        }

        private void UseOlproxy_CheckedChanged(object sender, EventArgs e)
        {
            UseOlproxy = UseOlproxyCheckBox.Checked;
            StartStopOlproxyButton.Visible = UseOlproxy;
            Verbose((UseOlproxyCheckBox.Checked) ? "Olproxy enabled." : "Olproxy disabled.");
            if (!UseOlproxy) StartStopOlproxyButton_Click(null, null);
        }

        private void SearchOverloadButton_Click(object sender, EventArgs e)
        {
            FindOverloadInstall(false, true);
        }

        private void OlmodExecutable_DoubleClick(object sender, EventArgs e)
        {
            OlmodExecutable.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            if (String.IsNullOrEmpty(OlmodExecutable.Text)) OlmodExecutable.Text = Path.Combine(Path.GetDirectoryName(OverloadExecutable.Text), "olmod.exe");

            SelectExecutable.FileName = Path.GetFileName(OlmodExecutable.Text);
            SelectExecutable.InitialDirectory = Path.GetDirectoryName(OlmodExecutable.Text);

            DialogResult result = SelectExecutable.ShowDialog();

            if (result == DialogResult.OK)
            {
                OlmodExecutable.Text = SelectExecutable.FileName;
                OlmodExecutable.SelectionLength = 0;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void OlmodReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo(OlmodReleases.Text)); } catch { }
        }

        private void OlproxyReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo(OlproxyReleases.Text)); } catch { }
        }

        /// <summary>
        /// Return true only if directory name is valid and exists/is created.
        /// </summary>
        /// <param name="directoryName">Name of directory to check/create./param>
        /// <returns></returns>
        private bool CheckCreateDirectory(string directoryName)
        {
            if (!OverloadClientToolApplication.ValidDirectoryName(directoryName)) return false;
            try { Directory.CreateDirectory(directoryName); } catch { }
            try { return Directory.Exists(directoryName); } catch { return false; }
        }

        private void PlayOverload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(PLayOverloadLinkLabel.Text);
                Process.Start(sInfo);
            }
            catch
            {
            }
        }

        private void OpenPilotsBackupFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CheckCreateDirectory(pilotsBackupPath)) try { Process.Start(new ProcessStartInfo(@"file:///" + pilotsBackupPath)); } catch { MessageBox.Show("Cannot open pilots backup folder!", "Error"); }
        }

        private void EnableDebugCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Debugging = EnableDebugCheckBox.Checked;
            //Info((EnableDebugCheckBox.Checked) ? "Debug logging enabled." : "Debug logging disabled.");
        }

        private void OpenDebugFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CheckCreateDirectory(Path.GetDirectoryName(debugFileName))) try { Process.Start(new ProcessStartInfo("file:///" + Path.GetDirectoryName(debugFileName))); } catch { MessageBox.Show("Cannot open debug log folder!", "Error"); }
        }

        private void OlmodExecutable_TextChanged(object sender, EventArgs e)
        {
            OlmodPath = OlmodExecutable.Text;
            ValidateSettings();
        }

        private void OlproxyArgs_TextChanged(object sender, EventArgs e)
        {
            OlproxyParameters = OlproxyArgs.Text;
        }

        private void OverloadArgs_TextChanged(object sender, EventArgs e)
        {
            OverloadParameters = OverloadArgs.Text;
        }

        private void OnlineMapJsonUrl_TextChanged(object sender, EventArgs e)
        {
            MapListUrl = OnlineMapJsonUrl.Text;
        }

        private void InfoLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "mailto:mickdk2010@gmail.com?subject=Overload Client Tool";
                proc.Start();
            }
            catch
            {
            }
        }

        #region MapManager

        private object mapChangeLock = new object();

        private static bool SameMap(OverloadMap map1, OverloadMap map2)
        {
            return (map1.InDLCFolder == map2.InDLCFolder) && (map1.ZipName.ToLower() == map2.ZipName.ToLower());
        }

        private void UpdateMapList()
        {
            string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Revival");
            appPath = Path.Combine(appPath, "Overload");

            // Construct the paths to where local maps can be stored.
            string dlcFolder = OverloadClientToolApplication.ValidDirectoryName(OverloadPath) ? Path.Combine(Path.GetDirectoryName(OverloadPath), "DLC") : null;
            string appFolder = OverloadClientToolApplication.ValidDirectoryName(appPath) ? appPath : null;

            mapManager.SaveNewMapsToDLCFolder = UseDLCLocation && OverloadClientToolApplication.ValidDirectoryName(dlcFolder, true);
            mapManager.UpdateMapList(MapListUrl, dlcFolder, appFolder);
        }

        private void InitMapsListBox()
        {
            LogDebugMessage("InitMapsListBox()");

            UpdateMapList();
            UpdateMapListBox();
        }

        private void SetMapButtons()
        {
            if (MapsListBox.SelectedIndex >= 0)
            {
                OverloadMap map = (OverloadMap)MapsListBox.Items[MapsListBox.SelectedIndex];

                MapHideButton.Text = (map.Hidden) ? "Unhide" : "Hide";

                MapHideButton.Enabled = map.IsLocal;
                MapDeleteButton.Enabled = map.IsLocal;
                MapHideButton.Enabled = map.IsLocal;
                MapRefreshButton.Enabled = map.IsOnline && !map.Hidden;
            }
            else
            {
                MapDeleteButton.Enabled = false;
                MapHideButton.Text = "Hide";
                MapHideButton.Enabled = false;
                MapRefreshButton.Enabled = false;
            }

            ApplyThemeToControl(PaneMaps, theme);
        }

        private void MapDelete_Click(object sender, EventArgs e)
        {
            if (MapsListBox.SelectedIndex >= 0)
            {
                OverloadMap map = (OverloadMap)MapsListBox.Items[MapsListBox.SelectedIndex];
                if (!map.IsLocal) return;

                if (MessageBox.Show(String.Format($"Delete map '{map.ZipName}' from disk?"), "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lock (mapChangeLock)
                    {
                        try
                        {
                            MapsListBox.Items.Remove(map);
                            try { System.IO.File.Delete(map.LocalZipFileName); } catch { }
                            try { System.IO.File.Delete(map.LocalDLCZipFileName); } catch { }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(String.Format($"Whoops! Cannot delete map '{map.ZipName}': {ex.Message}"));
                        }

                        UpdateMapList();
                        UpdateMapListBox();
                    }
                }
            }
        }

        private void MapsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (mapChangeLock) SetMapButtons();
        }

        private async void MapRefresh_ClickAsync(object sender, EventArgs e)
        {
            OverloadMap map = (OverloadMap)MapsListBox.Items[MapsListBox.SelectedIndex];

            if (map.Hidden)
            {
                MessageBox.Show("Cannot refresh a hidden map!", "Map is hidden");
                return;
            }

            string dlcFolder = OverloadClientToolApplication.ValidDirectoryName(OverloadPath) ? Path.Combine(Path.GetDirectoryName(OverloadPath), "DLC") : null;
            mapManager.SaveNewMapsToDLCFolder = UseDLCLocation && OverloadClientToolApplication.ValidDirectoryName(dlcFolder, true);

            await mapManager.UpdateMap(map, true);

            UpdateMapListBox();
        }

        private void MapHideButton_Click(object sender, EventArgs e)
        {
            OverloadMap map = (OverloadMap)MapsListBox.Items[MapsListBox.SelectedIndex];
            if (!map.IsLocal) return;

            int index = MapsListBox.SelectedIndex;
            try
            {
                map.Hidden = !map.Hidden;
                UpdateMapListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format($"Whoops! Cannot {((map.Hidden) ? "unhide" : "hide")} map {map.ZipName}: {ex.Message}"));
                SetMapButtons();
            }

            OnlineMapJsonUrl.Select(0, 0);
            MapsListBox.SetSelected(index, true);
        }

        private void UpdateMapListBox(string focusName = null)
        {
            MapsListBox.Items.Clear();

            bool anyHidden = false;

            foreach (KeyValuePair<string, OverloadMap> map in mapManager.SortedMaps)
            {
                if (map.Value.IsLocal)
                {
                    MapsListBox.Items.Add(map.Value);
                    if (map.Value.Hidden) anyHidden = true;
                }
            }

            if (!String.IsNullOrEmpty(focusName))
            {
                for (int i = 0; i < MapsListBox.Items.Count; i++)
                {
                    OverloadMap map = ((KeyValuePair<string, OverloadMap>)MapsListBox.Items[i]).Value;
                    if (map.SameZipFileName(focusName))
                    {
                        MapsListBox.SelectedIndex = i;
                        return;
                    }
                }
            }

            MapUnhideAllButton.Enabled = anyHidden;
            ApplyThemeToControl(MapUnhideAllButton, theme);

            SetMapButtons();
        }

        private void MapUpdateButton_Click(object sender, EventArgs e)
        {
            MapUpdateButton.Enabled = false;

            string dlcFolder = OverloadClientToolApplication.ValidDirectoryName(OverloadPath) ? Path.Combine(Path.GetDirectoryName(OverloadPath), "DLC") : null;
            mapManager.SaveNewMapsToDLCFolder = UseDLCLocation && OverloadClientToolApplication.ValidDirectoryName(dlcFolder, true);

            // Start updating maps in a separate thread.
            mapManagerThread = new Thread(UpdateMapThread);
            mapManagerThread.IsBackground = true;
            mapManagerThread.Start();
        }

        /// <summary>
        /// Update maps in the background.
        /// </summary>
        private void UpdateMapThread()
        {
            this.UIThread(delegate
            {
                UpdatingMaps.Visible = true;
                MapsListBox.Enabled = false;

                MapRefreshButton.Enabled = false;
                MapDeleteButton.Enabled = false;
                MapHideButton.Enabled = false;

                UseDLCLocationCheckBox.Enabled = false;
                AutoUpdateMapsCheckBox.Enabled = false;
                OnlyUpdateExistingMapsCheckBox.Enabled = false;

                if (UpdateOnlyExistingMaps) Verbose(String.Format("Checking for updated maps."));
                else Verbose(String.Format("Checking for new/updated maps."));

                mapManager.SaveNewMapsToDLCFolder = (UseDLCLocation && !String.IsNullOrEmpty(dlcLocation));
                Verbose(String.Format("Overload " + ((UseDLCLocation && !String.IsNullOrEmpty(dlcLocation)) ? "DLC" : "application") + " folder used for new maps."));
            });

            // UpdateAllMaps() must not touch UI elements!
            mapManager.UpdateAllMaps(OnlineMapJsonUrl.Text, dlcLocation, null);

            this.UIThread(delegate
            {
                if (UpdateOnlyExistingMaps) Verbose(String.Format($"Map check finished: {mapManager.Checked} maps, {mapManager.Updated} updated."));
                else Verbose(String.Format($"Map check finished: {mapManager.Checked} maps checked, {mapManager.Created} created, {mapManager.Updated} updated."));

                UpdateMapListBox();

                UpdatingMaps.Visible = false;
                MapUpdateButton.Enabled = true;
                MapsListBox.Enabled = true;

                UseDLCLocationCheckBox.Enabled = true;
                AutoUpdateMapsCheckBox.Enabled = true;
                OnlyUpdateExistingMapsCheckBox.Enabled = true;

                SetMapButtons();
            });
        }

        /// <summary>
        /// Move maps from either of the two possible directorys.
        /// </summary>
        /// <param name="overloadMapLocation"></param>
        /// <param name="dlcLocation"></param>
        private void MoveMaps(string source, string destination)
        {
            if (!CheckCreateDirectory(source) || !CheckCreateDirectory(destination))
            {
                MessageBox.Show("Something went wrong in checking source/destination folders for map move!", "Unable to move maps");

                source = String.IsNullOrEmpty(source) ? "<null>" : source;
                destination = String.IsNullOrEmpty(destination) ? "<null>" : destination;

                Error(String.Format($"Map move from {source} to {destination} failed!"));

            }
            else
            {
                string[] files = Directory.GetFiles(source, "*.zip");
                foreach (string fileName in files)
                {
                    // Exclude DLC content (only move maps).
                    bool move = true;
                    string test = Path.GetFileNameWithoutExtension(fileName).ToUpper();
                    if (!fileName.ToLower().EndsWith(".zip") || (test.Contains("DLC0") || test.Contains("DLC1"))) move = false;

                    if (move)
                    {
                        string sourceFileName = Path.Combine(source, Path.GetFileName(fileName));
                        string destinationFileName = Path.Combine(destination, Path.GetFileName(fileName));

                        try
                        {
                            FileInfo fiSource = new FileInfo(sourceFileName);

                            if (System.IO.File.Exists(destinationFileName)) System.IO.File.Delete(destinationFileName);
                            System.IO.File.Move(sourceFileName, destinationFileName);

                            // Set local files date and time.
                            System.IO.File.SetCreationTime(destinationFileName, fiSource.CreationTime);
                            System.IO.File.SetLastWriteTime(destinationFileName, fiSource.LastWriteTime);
                        }
                        catch (Exception ex)
                        {
                            Error(String.Format($"Map move {sourceFileName} to {destinationFileName} failed: {ex.Message}"));
                        }
                    }
                }
            }

            UpdateMapList();
            UpdateMapListBox();
        }

        private void UseDLCLocationCheckBox_Click(object sender, EventArgs e)
        {
            string overloadMapLocation = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Revival\Overload";

            if (UseDLCLocationCheckBox.Checked == false)
            {
                // Setting the check mark.
                DialogResult result = MessageBox.Show("Move existing maps to the Overload DLC directory?", "Move maps?", MessageBoxButtons.YesNoCancel);
                switch (result)
                {

                    case DialogResult.Cancel:
                        break;

                    case DialogResult.No:
                        UseDLCLocationCheckBox.Checked = true;
                        UseDLCLocation = true;
                        Verbose(String.Format("Overload DLC directory used for maps."));
                        break;

                    default:
                        // TO-DO: Move existing maps.
                        UseDLCLocationCheckBox.Checked = true;
                        UseDLCLocation = true;
                        Verbose(String.Format("Overload DLC directory used for maps."));
                        MoveMaps(overloadMapLocation, dlcLocation);
                        break;
                }
            }
            else
            {
                // Clearing the check mark.
                DialogResult result = MessageBox.Show("Move existing maps to [ProgramData]\\Overload\\Revival directory?", "Move maps?", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Cancel:
                        break;

                    case DialogResult.No:
                        Verbose(String.Format("Overload ProgramData directory used for maps."));
                        UseDLCLocationCheckBox.Checked = false;
                        UseDLCLocation = false;
                        break;

                    default:
                        Verbose(String.Format("Overload ProgramData directory used for maps."));
                        MoveMaps(dlcLocation, overloadMapLocation);
                        UseDLCLocationCheckBox.Checked = false;
                        UseDLCLocation = false;
                        break;
                }
            }
        }

        private void AutoUpdateMaps_Click(object sender, EventArgs e)
        {
            AutoUpdateMaps = AutoUpdateMapsCheckBox.Checked;
        }

        private void OnlyUpdateExistingMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOnlyExistingMaps = OnlyUpdateExistingMapsCheckBox.Checked;
            mapManager.OnlyUpdateExistingMaps = UpdateOnlyExistingMaps;
        }

        private void MapsListBox_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = lb.IndexFromPoint(e.Location);

            if (index >= 0 && index < lb.Items.Count)
            {
                OverloadMap map = (OverloadMap)MapsListBox.Items[index];
                string toolTipString = map.DisplayMapInfo;

                // Don't do anything tooltip text is the current tooltip .
                if (MainToolTip.GetToolTip(lb) != toolTipString) MainToolTip.SetToolTip(lb, toolTipString);
            }
            else
            {
                MainToolTip.Hide(lb);
            }
        }

        private void HideUnofficialMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HideNonOfficialMaps = HideUnofficialMapsCheckBox.Checked;
            mapManager.HideUnOfficialMaps = HideNonOfficialMaps;
        }

        private void MapUnhideAllButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.SortedMaps)
            {
                if (setMap.Value.Hidden) setMap.Value.Hidden = false;
            }

            UpdateMapListBox();
            Unfocus();
        }

        #endregion

        #region Overload Application Settings

        private string outputLogFileName = SpecialFolderLocalLowPath + Path.DirectorySeparatorChar + "Revival" + Path.DirectorySeparatorChar + "Overload" + Path.DirectorySeparatorChar + "output_log.txt";

        private void OverloadLogFileCheck()
        {
            try
            {
                OverloadLog.Visible = System.IO.File.Exists(outputLogFileName);
            }
            catch
            {
                OverloadLog.Visible = false;
            }
        }

        private void OverloadLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo sInfo = new ProcessStartInfo("notepad.exe", outputLogFileName);
                Process.Start(sInfo);
            }
            catch
            {
            }
        }
        #endregion

        private void UseGameDirArg_CheckedChanged(object sender, EventArgs e)
        {
            PassGameDirToOlmod = UseOlmodGameDirArg.Checked;
        }

        #region Olmod updating

        private void UpdateOlmod_Click(object sender, EventArgs e)
        {
            if (IsOlmodRunning)
            {
                Info("Olmod is running - skipping version check/update.");
                return;
            }

            // Decide where to unpack Olmod ZIP.
            string olmodInstallFolder = null;

            if (OverloadClientToolApplication.ValidFileName(OlmodPath, true)) olmodInstallFolder = Path.GetDirectoryName(OlmodPath);
            if ((olmodInstallFolder == null) && (OverloadClientToolApplication.ValidFileName(OverloadPath, true))) olmodInstallFolder = Path.GetDirectoryName(OverloadPath);

            if (olmodInstallFolder == null)
            {
                Error("No valid folder found to install/update Olmod.");
                return;
            }

            // Get latest online release info.
            OlmodManager.OlmodRelease latest = olmodManager.GetLastestRelease;
            if (latest == null)
            {
                Error("Unable to get latest Olmod release info from Github.");
                return;
            }

            // Check if update is required. We use the ZIP date to stamp Olmod.exe
            if ((OverloadClientToolApplication.ValidFileName(OlmodPath, true)) && (new FileInfo(OlmodPath).CreationTimeUtc == latest.Created))
            {
                if (sender != null) Info("Already using the latest Olmod version.");
                return;
            }

            Info("Installing latest Olmod release from Github.");

            try
            {
                olmodManager.DownloadAndInstallOlmod(latest, olmodInstallFolder);
            }
            catch
            {
            }
            finally
            {
                ValidateSettings();
            }
        }

        private string OlmodVersionInfo
        {
            get
            {
                // Check Olmod version (using GameMod.dll).
                string olmodVersion = OverloadClientToolApplication.GetFileVersion(OlmodPath.ToLower().Replace("olmod.exe", "GameMod.dll"));
                olmodVersion = OverloadClientToolApplication.VersionStringFix(olmodVersion);
                return String.Format($"Olmod {olmodVersion} by Arne de Bruijn.");
            }
        }

        private void AutoUpdateOlmod_CheckedChanged(object sender, EventArgs e)
        {
            OlmodAutoUpdate = AutoUpdateOlmod.Checked;
        }
        #endregion

        private void PayPalLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "https://paypal.me/SorenM";
                proc.Start();
            }
            catch
            {
            }
        }

        private void StartStopOlproxyButton_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                // Only launch Olproxy if OCT isn't shutting down.
                if (!IsOlproxyRunning)
                {
                    LaunchOlproxy();
                    return;
                }
            }

            // Either we are shutting down or user wants to shut down Olproxy.
            if (IsOlproxyRunning)
            {
                Info("Shutting down Olproxy.");

                // ValidateButton(StartButton, theme);
                // Defocus();

                KillRunningProcess("olproxy");
                if ((olproxyTask.KillFlag == false) && ((olproxyThread != null) && olproxyThread.IsAlive)) KillOlproxyThread();
            }
        }

        private void PilotXPTextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void AvailableThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AvailableThemesListBox.SelectedIndex >= 0)
            {
                string newThemeName = (string)AvailableThemesListBox.Items[AvailableThemesListBox.SelectedIndex];

                theme = Theme.GetThemeByName(newThemeName);
                ActiveThemeName = newThemeName;

                UpdateTheme(theme);
                ThemeDescriptionLabel.Text = theme.Description;

                AvailableThemesListBox.Invalidate();
            }
        }

        // 1. Set the property DrawMode to DrawMode.OwnerDrawFixed.
        // 2. Overreide DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Template_DrawItem);
        private void Template_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Only created here to allow the code to compile without warnings!
            ListBox listBox1 = new ListBox();

            if (e.Index < 0) return;

            // If the item state is selected them change the back color.
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          theme.ActivePaneButtonBackColor);

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();

            // Draw the current item text
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(theme.PanelForeColor), e.Bounds, StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void AvailableThemesListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // If the item state is selected them change the back color.
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          theme.ActivePaneButtonBackColor);
            }

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();

            // Draw the current item text
            e.Graphics.DrawString(AvailableThemesListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(theme.PanelForeColor), e.Bounds, StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void PilotsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // If the item state is selected them change the back color.
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          theme.ActivePaneButtonBackColor);
            }

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();

            // Draw the current item text
            e.Graphics.DrawString(PilotsListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(theme.PanelForeColor), e.Bounds, StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void MapsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // If the item state is selected them change the back color.
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          theme.ActivePaneButtonBackColor);
            }

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();

            // Draw the current item text
            e.Graphics.DrawString(MapsListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(theme.PanelForeColor), e.Bounds, StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        string temptooltiptext = "";

        private void MainToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            temptooltiptext = e.ToolTipText;
            e.Graphics.DrawString(e.ToolTipText, treeViewFont, Brushes.Black, new PointF(2, 2));
        }

        private void MainToolTip_Popup(object sender, PopupEventArgs e)
        {
            //e.ToolTipSize = TextRenderer.MeasureText(MainToolTip.GetToolTip(e.AssociatedControl), new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular));
        }

        private void AutoPilotsBackupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            AutoSavePilots = AutoPilotsBackupCheckbox.Checked;
        }

        private void AutoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AutoUpdateOCT = AutoUpdateCheckBox.Checked;
        }

        private Font treeViewFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);

        public void LogTreeViewText(string text, bool error = false)
        {
            if (LogTreeView.Nodes.Count > 999) LogTreeView.Nodes[0].Remove();

            LogTreeView.Nodes.Add(text);
 
            LogTreeView.ShowNodeToolTips = true;
            TreeNode node = LogTreeView.Nodes[LogTreeView.Nodes.Count - 1];
            node.ToolTipText = text;
            node.Tag = (error) ? "Error" : "Ihfo";

            LogTreeView.Nodes[LogTreeView.Nodes.Count - 1].EnsureVisible();
        }

        private void LogTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            String text = e.Node.Text;

            Rectangle rect = new Rectangle(0, e.Node.Bounds.Y, LogTreeView.Width, e.Node.Bounds.Height);
            Size size = TextRenderer.MeasureText(text as string, treeViewFont);

            while (size.Width > (rect.Width + 8))
            {
                text = text.Substring(0, text.Length - 3);
                size = TextRenderer.MeasureText(text as string, treeViewFont);
            }

            if (text.Length < e.Node.Text.Length) text += "...";

            // Draw the background of the ListBox control for each item.
            rect.Y++;
            e.Graphics.FillRectangle(new SolidBrush(theme.InputBackColor), rect);
            rect.Y--;

            Color color = theme.InputForeColor;
            if (((String)e.Node.Tag).ToLower().Contains("error")) color = theme.TextHighlightColor;

            // Draw the current item text
            e.Graphics.DrawString(text, treeViewFont, new SolidBrush(color), e.Bounds, StringFormat.GenericDefault);
        }

        private void FrameTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowFPS = FrameTimeCheckBox.Checked;
        }

        #region Server
        private void ServerTrackerName_TextChanged(object sender, EventArgs e)
        {
            OlproxyServerName = ServerTrackerName.Text;
        }

        private void ServerTrackerUrl_TextChanged(object sender, EventArgs e)
        {
            OlproxyTrackerBaseUrl = ServerTrackerUrl.Text;
        }

        private void ServerAnnounceOnTrackerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OlproxyIsServer = ServerAnnounceOnTrackerCheckBox.Checked;
        }

        private void ServerAutoSignOffTracker_CheckedChanged(object sender, EventArgs e)
        {
            OlproxySignOff = ServerAutoSignOffTracker.Checked;
        }

        private void AutoStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoStartup(AutoStartCheckBox.Checked);
            StartWithWindows = AutoStartCheckBox.Checked;
        }

        private void ServerTrackerNotes_TextChanged(object sender, EventArgs e)
        {
            OlproxyNotes = ServerTrackerNotes.Text;
        }

        private void EnableServerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RunDedicatedServer = ServerEnableCheckBox.Checked;
        }
        #endregion

        private void UseTrayIcon_CheckedChanged(object sender, EventArgs e)
        {
            TrayInsteadOfTaskBar = UseTrayIcon.Checked;
            ShowInTaskbar = !UseTrayIcon.Checked;
            OverloadClientToolNotifyIcon.Visible = UseTrayIcon.Checked;
        }
    }
}