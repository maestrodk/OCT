﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using minijson;

namespace OverloadClientTool
{
    public partial class OCTMain : Form
    {
        DisplayConfiguration DisplayConfiguration = DisplayConfiguration.ActiveConfiguration;

        IEnumerable<Monitor> Monitors = Monitor.AllMonitors;
        IEnumerable<string> MonitorFriendlyNames = Monitor.GetAllMonitorFriendlyNames();

        public bool exited = false;
        public bool shutdown = false;
        private bool trayExitClick = false;
        
        private bool NeedToRunPostApp { get; set; } = false;
        private bool NeedToRestoreDisplay { get; set; } = false;

        private Font treeViewFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);

        public float ScaleFactor { get; set; } = 1.0f;

        // Set a default them (might change when reading settings).
        public Theme theme = Theme.GetDarkGrayTheme;

        // Keyboard hook to enable hotkeys.
        public KeyboardHook keyboardHook = null;

        // Low level keyboard hook.
        EnableDisableKeys enableDisableKeys = new EnableDisableKeys();

        // Shortcut link for Startupt folder (if file exists the auto4 is enabled).
        string shortcutFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "OverLoad Client Tool AutoStart.lnk");

        // This matches MJDict defined on Olproxy.
        private Dictionary<string, object> olmodConfig = new Dictionary<string, object>();

        private OlmodManager olmodManager = null;

        private OverloadMapManager mapManager = null;

        internal Thread mapManagerThread = null;
        internal Thread pingerThread = null;
        internal Thread backgroundThread = null;

        private PaneController paneController = null;

        private DisplaySettings displaySettings = new DisplaySettings();

        private int serverProcessId = 0;

        private int D3ProcessId = 0;
        private int D2ProcessId = 0;
        private int D1ProcessId = 0;

        // Directory for DLC.
        private string dlcLocation = null;

        private string debugFileName = null;

        // Tray icon menu.
        private ContextMenu trayContextMenu = new ContextMenu();

        private MenuItem trayMenuItemStart = new System.Windows.Forms.MenuItem();
        private MenuItem trayMenuItemStartServer = new System.Windows.Forms.MenuItem();
        private MenuItem trayMenuItemExit = new System.Windows.Forms.MenuItem();
        private MenuItem trayMenuItemSwitch = new System.Windows.Forms.MenuItem();

        private OverloadPinger pinger = new OverloadPinger();

        public void LogDebugMessage(string message)
        {
            if (!Debugging) return;

            message = String.IsNullOrEmpty(message) ? Environment.NewLine : message + Environment.NewLine;
            try { System.IO.File.AppendAllText(debugFileName, String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}")); } catch { }
        }

        public OCTMain(string[] args, string debugFileName, Dictionary<string, string> previousSettings = null)
        {
            this.debugFileName = debugFileName;

            if (!Debugging)
            {
                try { System.IO.File.Delete(debugFileName); } catch { }
            }

            foreach (string a in args)
            {
                //if (a.ToLower().Contains("-launched")) autoStart = true;
            }

            // Init map manager.
            mapManager = new OverloadMapManager(UpdateOnlyExistingMaps, IncludeMP, IncludeSP, IncludeCM);

            // Initialize controls on main form.
            InitializeComponent();

            if (WindowSize != "Small")
            {
                float NewFontSize = 12f;
                if (WindowSize == "Normal") NewFontSize = 10f;

                ScaleFactor = NewFontSize / Font.Size;
                Font = new Font(this.Font.FontFamily.Name, NewFontSize);

                // Fix text size and line heights.
                treeViewFont = new Font(this.Font.FontFamily.Name, NewFontSize);

                LogTreeView.Font = treeViewFont;
                LogTreeView.ItemHeight = (int)((((float)LogTreeView.ItemHeight) * ScaleFactor)); //  19;  // Default is 14.

                MapsListBox.Font = treeViewFont;
                MapsListBox.ItemHeight = (int)((((float)MapsListBox.ItemHeight) * ScaleFactor)); //  20;  // Default is 13.
                MapsListBox.ItemHeight++;

                PilotsListBox.Font = treeViewFont;
                PilotsListBox.ItemHeight = (int)((((float)PilotsListBox.ItemHeight) * ScaleFactor)); //  20;  // Default is 13.
                PilotsListBox.ItemHeight++;
                if (NewFontSize == 12f) PilotsListBox.ItemHeight++;

                AvailableThemesListBox.Font = treeViewFont;

                if (NewFontSize == 12f) AvailableThemesListBox.ItemHeight++;
                AvailableThemesListBox.ItemHeight = (int)((((float)AvailableThemesListBox.ItemHeight) * ScaleFactor)); //  20;  // Default is 13.
                if (NewFontSize == 10f) AvailableThemesListBox.ItemHeight++;

                ServersListView.Font = treeViewFont;
                if (NewFontSize == 12f) ServersListView.Height--;                

                foreach (ColumnHeader header in ServersListView.Columns)
                {
                    header.Width = (int)(((float)header.Width) * ScaleFactor);
                }
            }

            // Initialize tray menu item 1.
            trayMenuItemStart.Index = 0;
            trayMenuItemStart.Text = "&Start Overload";
            trayMenuItemStart.Click += new System.EventHandler(StartButton_Click);

            // Initialize tray menu item 2.
            trayMenuItemStartServer.Index = 1;
            trayMenuItemStartServer.Text = "St&art Overload server";
            trayMenuItemStartServer.Click += new System.EventHandler(StartServerButton_Click);

            // Initialize tray menu item 2.
            trayMenuItemSwitch.Index = 2;
            trayMenuItemSwitch.Text = "Switch to &taskbar";
            trayMenuItemSwitch.Click += new System.EventHandler(SwitchToTaskBar_Click);

            // Initialize tray menu item 4.
            trayMenuItemExit.Index = 3;
            trayMenuItemExit.Text = "E&xit OCT";
            trayMenuItemExit.Click += new System.EventHandler(StopExitButton_Click);

            // Setuy tray menu.
            trayContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { trayMenuItemStart, trayMenuItemStartServer, trayMenuItemSwitch, trayMenuItemExit });
            OverloadClientToolNotifyIcon.ContextMenu = trayContextMenu;

            // Center main form on Desktop.
            this.StartPosition = FormStartPosition.CenterScreen;
            StatusMessage.Text = "Starting up...";

            // Setup pane control.
            paneController = new PaneController(this, PaneButtonLine, theme);
            paneController.SetupPaneButton(PaneSelectMain, PaneMain);
            paneController.SetupPaneButton(PaneSelectMapManager, PaneMaps);
            paneController.SetupPaneButton(PaneSelectPilots, PanePilots);
            paneController.SetupPaneButton(PaneSelectOverload, PaneOverload);
            paneController.SetupPaneButton(PaneSelectOlmod, PaneOlmod);
            paneController.SetupPaneButton(PaneSelectServer, PaneServer);
            paneController.SetupPaneButton(PaneSelectOnline, PaneOnline);
            paneController.SetupPaneButton(PaneSelectDescent, PanelDescent);
            paneController.SetupPaneButton(PaneSelectOptions, PaneOptions);

            // Load user preferences.
            LoadSettings(previousSettings);

            // Reflect auto-startup setting.
            ToogleAutostartCheckBox.Checked = System.IO.File.Exists(shortcutFileName);

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

            // Set logging for map manager.
            mapManager.SetLogger(Info, Error);

            // Init Olmod manager.
            olmodManager = new OlmodManager(Info, Error);

            // Create properties for Olmod .
            olmodConfig.Add("isServer", false);
            olmodConfig.Add("keepListed", false);
            olmodConfig.Add("assistScoring", false);
            olmodConfig.Add("trackerBaseUrl", "");
            olmodConfig.Add("serverName", "");
            olmodConfig.Add("notes", "");
        }

        private void SwitchToTaskBar_Click(object sender, EventArgs e)
        {
            UseTrayIcon.Checked = false;
            ShowInTaskbar = true;
            OverloadClientToolNotifyIcon.Visible = false;
        }

        private void ToogleAutostartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoStartup(ToogleAutostartCheckBox.Checked);
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
                    myShortcut.Arguments = "-launched";                     // Parameters sent to OverloadClientTool.exe.
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

        //private Form BlackForm { get; set; } = null;
        
        private Form CreateBlackForm()//Image image)
        {
            Form form = new Form();
            form.ControlBox = false;
            form.FormBorderStyle = FormBorderStyle.None;
            //frm.BackgroundImage = image;
            form.BackgroundImageLayout = ImageLayout.Zoom;
            Screen scr = Screen.AllScreens.Length > 1 ? Screen.AllScreens[1] : Screen.PrimaryScreen;
            form.Location = new Point(scr.Bounds.Left, scr.Bounds.Top);
            form.Size = scr.Bounds.Size;
            form.BackColor = Color.Black;
            form.Show();
            return form;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LogDebugMessage("Main_Load()");

            // Blank second monitor.
            //BlackForm = CreateBlackForm();

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

            // Adjust server browser labels.
            // label13 = IP.
            // label21 = Server name.
            // label22 = Game mode.
            // label23 = Players.
            // label24 = Max players.
            // labe27 = Ping.

            LabelServerName.Location = new Point(LabelServerIP.Location.X + ServersListView.Columns[0].Width, ActiveThemeLabel.Location.Y);
            LabelServerGameMode.Location = new Point(LabelServerName.Location.X + ServersListView.Columns[1].Width, ActiveThemeLabel.Location.Y);
            LabelServerPlayers.Location = new Point(LabelServerGameMode.Location.X + ServersListView.Columns[2].Width - 4, ActiveThemeLabel.Location.Y);
            LabelServerMaxPlayers.Location = new Point(LabelServerPlayers.Location.X + ServersListView.Columns[3].Width + 2, ActiveThemeLabel.Location.Y);
            LabelServerPing.Location = new Point(LabelServerMaxPlayers.Location.X + ServersListView.Columns[4].Width, LabelServerIP.Location.Y);

            // Add this here so it is ready.
            ServersListView.ListViewItemSorter = new ListViewItemComparer();
            ((ListViewItemComparer)ServersListView.ListViewItemSorter).Order = SortOrder.Descending;
            ((ListViewItemComparer)ServersListView.ListViewItemSorter).Column = 3;

            // Setup and hide server sort arrow symbols.
            LabelUpArrow.Text = ((char)61613).ToString();
            LabelUpArrow.Visible = false;
            LabelDownArrow.Text = ((char)61615).ToString();
            LabelDownArrow.Visible = false;

            // Announce ourself.
            Info("Overload Client Tool " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3) + " by Søren 'Maestro' Michélsen.");
            Info("Olproxy 0.3.1 by Arne de Bruijn.");

            // Start background monitor for periodic log updates.
            backgroundThread = new Thread(ActivityBackgroundMonitor);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();

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

            if (MinimizeOnStartupCheckBox.Checked) WindowState = FormWindowState.Minimized;
            else this.WindowState = FormWindowState.Normal;

            Defocus();

            // Check for OCT update.
            if (AutoUpdateOCT) UpdateCheck(debugFileName, false);

            // Setup dislays. At this time default values are set 
            Dictionary<uint, string> displays = DisplayManager.Displays;
            GamingMonitorComboBox.Items.Clear();
            DefaultMonitorComboBox.Items.Clear();
            foreach (KeyValuePair<uint, string> kvp in displays)
            {
                DefaultMonitorComboBox.Items.Add(kvp.Value);
                GamingMonitorComboBox.Items.Add(kvp.Value);
            }

            if (displays.ContainsValue(DefaultDisplay))
            {
                DefaultMonitorComboBox.SelectedItem = DefaultDisplay;
            }

            if (displays.ContainsValue(GamingDisplay))
            {
                GamingMonitorComboBox.SelectedItem = GamingDisplay;
            }

            // Set parent form for servers to enable logging.
            Servers.Parent = this;

            // Start background pinger.
            pingerThread = new Thread(pinger.PingUpdateThread);
            pingerThread.IsBackground = true;
            pingerThread.Start();

            // Populate server list.
            UpdateServerListButton_Click(null, null);

            if (AutoStartServer) StartServerButton_Click(null, null);

            // Enable keyboard hook.
            keyboardHook = new KeyboardHook(true);
            keyboardHook.KeyDown += KeyboardHook_KeyDown;

            // Enable low level keyboard hook.
            enableDisableKeys.KeyHook();

            // Determine available gamemod.dll files.
            UpdateGameModList();

            LogDebugMessage("Main_Load() done");
        }

        public void UpdateGameModList()
        {
            GameModComboBox.Items.Clear();

            string[] gameModFiles = Directory.GetFiles(Path.GetDirectoryName(OlmodPath), "GameMod*.dll");
            foreach (string gameModFile in gameModFiles)
            {
                try
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(gameModFile);

                    string version = versionInfo.FileVersion;
                    if (version.EndsWith(".0")) version = version.Substring(0, version.Length - 2);

                    string gameModFileName = "GameMod_" + version + ".dll";
                    gameModFileName = Path.Combine(Path.GetDirectoryName(gameModFile), gameModFileName);

                    if (!System.IO.File.Exists(gameModFileName) && (Path.GetFileName(gameModFile).ToLower() == "gamemod.dll"))
                    {
                        System.IO.File.Copy(gameModFile, Path.Combine(Path.GetDirectoryName(gameModFile), gameModFileName));
                        GameModComboBox.Items.Add(Path.GetFileName(gameModFileName));
                    }
                    else
                    {
                        if (Path.GetFileName(gameModFile) != "GameMod.dll") GameModComboBox.Items.Add(Path.GetFileName(gameModFile));
                    }
                }
                catch
                {
                }

                GameModComboBox.Enabled = GameModComboBox.Items.Count > 0;
                if (IsOverloadOrOlmodRunning) GameModComboBox.Enabled = false;
            }
        }

        private void KeyboardHook_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            string testKey = key.ToString();
            if ((testKey.ToLower() == "lwin") || (testKey.ToLower() == "apps"))
            {
                // Either the Windows key or the Apps (right-click) key.
                return;
            }

            if (HotkeyStartClient.Focused)
            {
                //if (key.ToString().Contains("ControlKey") || key.ToString().Contains("AltKey") || key.ToString().Contains("ShiftKey")) ;

                string modifiers = "";
                modifiers += (KeyboardHook.CtrlPressed) ? "<CTRL> + " : "";
                modifiers += (KeyboardHook.ShiftPressed) ? "<SHIFT> + " : "";
                modifiers += (KeyboardHook.AltPressed) ? "<ALT> + " : "";

                if (key.ToString().Contains("ControlKey") || key.ToString().Contains("AltKey") || key.ToString().Contains("ShiftKey"))
                {
                    if (!String.IsNullOrEmpty(modifiers)) HotkeyStartClient.Text = modifiers + "???";
                    return;
                }

                string setKey = modifiers + key.ToString();
                HotkeyStartClient.Text = setKey;

                // Save if input contains real key sequence only.
                if (!setKey.Contains("???")) StartClientHotkeyString = setKey;
            }
            else
            {
                string modifiers = "";
                modifiers += (KeyboardHook.CtrlPressed) ? "<CTRL> + " : "";
                modifiers += (KeyboardHook.ShiftPressed) ? "<SHIFT> + " : "";
                modifiers += (KeyboardHook.AltPressed) ? "<ALT> + " : "";

                if ((modifiers + key.ToString()) == HotkeyStartClient.Text)
                {
                    if (StartStopButton.Text.Contains("Start"))
                    {
                        StartButton_Click(null, null);
                    }
                    else
                    {
                        StopButton_Click(null, null);
                    }
                }
            }
        }

        private void HotkeyStartClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void ClearHotkeyButton_Click(object sender, EventArgs e)
        {
            HotkeyStartClient.Text = "";
            StartClientHotkeyString = "";
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

        internal void ShutdownTasks()
        {
            if (exited) return;

            // Shutdown background workers.
            try { StopPilotsMonitoring(); } catch { }
            try { pingerThread.Abort(); } catch { }
            try { backgroundThread.Abort(); } catch { }

            // Shut down server.
            if (serverProcessId > 0) StartServerButton_Click(null, null);

            exited = true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            RestoreDisplayConfiguration();
            RunPostApp();

            if ((sender != null) && (e != null))
            {
                // Not a forced shutdown.
                if (!trayExitClick && (e.CloseReason != CloseReason.WindowsShutDown))
                {
                    // User didn't use the tray exit option and Windows isn't doing a shutdown either.
                    // Check to see if we should only exit if the Shift key is being pressed.
                    if (MinimizeOnClose == true)
                    {
                        // Only continue exit if user is helding down the shift button.
                        if (!(Control.ModifierKeys == Keys.Shift))
                        {
                            e.Cancel = true;
                            WindowState = FormWindowState.Minimized;
                            return;
                        }
                    }
                }
            }

            // Shutdown background workers.
            ShutdownTasks();

            CheckDisplaySwitch();

            // Save settings for main application.
            try
            {
                SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save settings: {ex.Message}", "Error");
            }

            CheckDisplaySwitch();
        }

        // This saves a JSON config file to the Olmod application folder.
        public void SetOlmodSettings()
        {
            string path = String.IsNullOrEmpty(OlmodPath) ? OverloadPath : OlmodPath;

            if (String.IsNullOrEmpty(path)) return;

            path = Path.GetDirectoryName(path);
            if (Directory.Exists(path) == false) return;

            string configFileName = Path.Combine(path, "olmodSettings.json");

            // Set parameters.
            olmodConfig["isServer"] = OlmodIsServer;
            olmodConfig["keepListed"] = OlmodIsServer;
            olmodConfig["assistScoring"] = OlmodAssistScoring;
            olmodConfig["trackerBaseUrl"] = OlmodServerTrackerBaseUrl;
            olmodConfig["serverName"] = OlmodServerName;
            olmodConfig["notes"] = OlmodServerNotes;

            string jsonString = MiniJson.ToString(olmodConfig);

            try { System.IO.File.WriteAllText(configFileName, jsonString); } catch { }
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

        public void AddNewLogMessage(string text, bool error = false)
        {
            this.UIThread(delegate
            {
                string prefix = DateTime.Now.ToString("HH:mm:ss") + " ";

                Graphics graphics = this.CreateGraphics();
                SizeF prefixSize = graphics.MeasureString(prefix, treeViewFont);

                string remaining = text;

                int count = 0;

                while (remaining.Length > 0)
                {
                    count++;
                    text = remaining;

                    SizeF textSize = graphics.MeasureString(text, treeViewFont);

                    while ((prefixSize.Width + textSize.Width + SystemInformation.VerticalScrollBarWidth + 2) > LogTreeView.Width)
                    {
                        int i = text.LastIndexOf(' ');
                        if (i < 0) text.LastIndexOf('-');

                        if (i > 0) text = text.Substring(0, i).Trim();
                        else text = text.Substring(0, text.Length - 1).Trim();

                        textSize = graphics.MeasureString(text, treeViewFont);
                    }

                    remaining = remaining.Substring(text.Length);

                    LogTreeViewText(prefix + text, error);

                    /*
                    if (remaining.Length > 0)
                    { 
                        if (count == 1) LogTreeViewText(prefix + text + "...", error);
                        else LogTreeViewText(prefix + "..." + text + "...", error);
                    }
                    else
                    {
                        if (count > 1) LogTreeViewText(prefix + "..." + text, error);                        
                        else LogTreeViewText(prefix + text, error);
                    }*/
                }
            });
        }

        private void Info(string text)
        {
            if (text.Contains("/api/stats"))
            {
                try
                {
                    text = text.Substring(text.IndexOf("{"));
                    Dictionary<string, object> list = (Dictionary<string, object>)MiniJson.Parse(text);

                    string id = list["type"] as string;

                    string matchMode, level, attacker, defender, player;
                    List<object> players;
                    int maxPlayers, timeLimit, scoreLimit, secs, mins;
                    TimeSpan ts;

                    switch (id.ToLower())
                    {
                        case "startgame":
                            matchMode = list["matchMode"].ToString();
                            level = list["level"].ToString();
                            maxPlayers = Convert.ToInt32(list["maxPlayers"].ToString());
                            timeLimit = Convert.ToInt32(list["timeLimit"].ToString());
                            scoreLimit = Convert.ToInt32(list["scoreLimit"].ToString());
                            players = (List<object>)list["players"];
                            DateTime dt = DateTime.SpecifyKind(Convert.ToDateTime(list["start"].ToString()), DateTimeKind.Utc);
                            dt = TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.Local);
                            AddNewLogMessage($"Starting {matchMode} {level} with {players.Count}. Time limit {timeLimit}, score limit {scoreLimit}.");
                            break;

                        case "endgame":
                            AddNewLogMessage($"Game ended.");
                            break;

                        case "kill":
                            attacker = list["attacker"].ToString();
                            defender = list["defender"].ToString();
                            if (attacker != defender) AddNewLogMessage($"{defender} was killed by {attacker}.");
                            else AddNewLogMessage($"{defender} had a fatal accident.");
                            break;

                        case "connect":
                            // player, time (seconds).
                            player = list["player"].ToString();
                            secs = Convert.ToInt32(list["time"].ToString());
                            mins = secs / 60;
                            secs = secs % 60;
                            ts = new TimeSpan(0, mins, secs);
                            AddNewLogMessage($"{player} joined the server at " + ts.ToString("mm:ss") + ".");
                            break;

                        case "disconnect":
                            // player, time (seconds).
                            player = list["player"].ToString();
                            secs = Convert.ToInt32(list["time"].ToString());
                            mins = secs / 60;
                            secs = secs % 60;
                            ts = new TimeSpan(0, mins, secs);
                            AddNewLogMessage($"{player} left the server at " + ts.ToString("mm:ss") + ".");
                            break;

                        case "lobbystatus":
                            // matchMode, maxPlayers, timeLimit (seconds), scoreLimit, level, players[], turnSpeedLimit, powerupSpawn, friendlyFire, joinInProgress, teamCount.
                            matchMode = list["matchMode"].ToString();
                            level = list["level"].ToString();
                            maxPlayers = Convert.ToInt32(list["maxPlayers"].ToString());
                            timeLimit = Convert.ToInt32(list["timeLimit"].ToString());
                            scoreLimit = Convert.ToInt32(list["scoreLimit"].ToString());
                            players = (List<object>)list["players"];
                            string playerString = "";
                            foreach (string p in players) playerString += String.IsNullOrEmpty(playerString) ? p : ", " + p;
                            AddNewLogMessage($"Lobby {matchMode} {players.Count}/{maxPlayers} players, {level},");
                            break;

                        default:
                            break;
                    }
                }
                catch
                {

                }

                return;
            }

            if (text.StartsWith("Server:"))
            {
                if (text.Contains("HOST: ProcessReady")) AddNewLogMessage("Server is ready.");
                return;
            }

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
            catch
            {
                LogDebugMessage(String.Format($""));

                dlcLocation = null;
                UseDLCLocationCheckBox.Enabled = false;
                UseDLCLocation = false;
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

        public bool IsD3Running
        {
            get
            {
                bool running = false;
                string name = Path.GetFileNameWithoutExtension(Descent3Executable.Text);
                if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);
                try { if (GetRunningProcess(name) != null) running = true; } catch { }
                return running;
            }
        }

        public bool IsD2Running
        {
            get
            {
                bool running = false;
                string name = Path.GetFileNameWithoutExtension(Descent2Executable.Text);
                if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);
                try { if (GetRunningProcess(name) != null) running = true; } catch { }
                return running;
            }
        }

        public bool IsD1Running
        {
            get
            {
                bool running = false;
                string name = Path.GetFileNameWithoutExtension(Descent1Executable.Text);
                if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);
                try { if (GetRunningProcess(name) != null) running = true; } catch { }
                return running;
            }
        }

        /// <summary>
        /// Background logging monitor. Sets GroupBox titles to reflect running status.
        /// </summary>
        private void ActivityBackgroundMonitor()
        {
            int requestInterval = Servers.ServerRefreshIntervalSeconds;
            Stopwatch requestTimer = new Stopwatch();
            requestTimer.Restart();

            Stopwatch pingTimer = new Stopwatch();
            pingTimer.Restart();

            while (shutdown == false)
            {
                bool overloadRunning = IsOverloadRunning;
                bool olmodRunning = IsOlmodRunning;

                try { GameModComboBox.Enabled = !(overloadRunning || olmodRunning); } catch { }

                bool d3Running = IsD3Running;
                bool d2Running = IsD2Running;
                bool d1Running = IsD1Running;

                if (!overloadRunning && !olmodRunning && !d3Running && !d2Running && !shutdown)
                {
                    // See if we should switch primary display.
                    this.UIThread(delegate { CheckDisplaySwitch(); });
                }

                this.UIThread(delegate
                {
                    if (overloadRunning || olmodRunning)
                    {
                        if (BlankSecondMonitorCheckBox.Checked && (Screen.AllScreens.Length > 1))
                        {
                            /*
                            if (BlackForm == null) BlackForm = CreateBlackForm();

                            Screen primary = Screen.PrimaryScreen;
                            Screen secondary = (primary == Screen.AllScreens[0]) ? Screen.AllScreens[1] : Screen.AllScreens[0];

                            BlackForm.Location = new Point(secondary.Bounds.X, secondary.Bounds.Y);
                            BlackForm.Size = secondary.Bounds.Size;
                            */
                            if (DisplayConfiguration != null)
                            {
                                NeedToRestoreDisplay = true;
                                int displayNumber = (Screen.PrimaryScreen == Screen.AllScreens[0]) ? 1 : 0;
                                Monitor.Disconnect(displayNumber);
                            }
                        }
                    }
                    else
                    {
                        /*
                        if (BlackForm != null)
                        {
                            BlackForm.Dispose();
                            BlackForm = null;
                        }
                        */

                        RestoreDisplayConfiguration();
                        RunPostApp();
                    }
                });


                int reqHours = requestInterval / 3600;
                int reqMins = (requestInterval - (reqHours * 3600)) / 60;
                int reqSecs = requestInterval - (reqHours * 3600) - (reqMins * 60);

                if (requestTimer.Elapsed > new TimeSpan(reqHours, reqMins, reqSecs))
                {
                    this.UIThread(delegate { UpdateServerListButton_Click(null, null); });
                    requestInterval = Servers.ServerRefreshIntervalSeconds;
                    requestTimer.Restart();
                }

                if (pingTimer.Elapsed > new TimeSpan(0, 0, 5))
                {
                    this.UIThread(delegate { UpdatePingTimes(); });
                    pingTimer.Restart();
                }

                this.UIThread(delegate
                {
                    OverloadLogFileCheck();

                    string statusText = "";
                    string server = "";

                    if (overloadRunning && !olmodRunning) statusText = $"Overload{server}is running.";
                    else if (olmodRunning) statusText = $"Overload{server} (using Olmod) is running.";
                    else
                    {
                        bool foundOverload = false;
                        bool foundOlmod = false;

                        try { foundOverload = (OverloadClientToolApplication.ValidFileName(OverloadExecutable.Text) && new FileInfo(OverloadExecutable.Text).Exists); } catch { }
                        try { foundOlmod = (OverloadClientToolApplication.ValidFileName(OlmodExecutable.Text) && new FileInfo(OlmodExecutable.Text).Exists); } catch { }

                        if (UseOlmodCheckBox.Checked && !foundOlmod) statusText = "Cannot find Olmod (check path)!";
                        else if (!UseOlmodCheckBox.Checked && !foundOverload) statusText = "Cannot find Overload (check path)!";
                    }

                    if (serverProcessId > 0) statusText += " Server is running.";

                    OverloadRunning.Visible = overloadRunning || olmodRunning || (serverProcessId > 0);
                    ServerRunning.Visible = serverProcessId > 0;

                    Descent3Running.Visible = d3Running;
                    Descent2Running.Visible = d2Running;
                    Descent1Running.Visible = d1Running;

                    UpdateOlmodButton.Enabled = !olmodRunning;

                    StartStopButton.Text = (overloadRunning || olmodRunning) ? "Stop client" : "Start client";

                    StartD1.Text = (d1Running) ? "Stop D1" : "Start D1";
                    StartD2.Text = (d1Running) ? "Stop D2" : "Start D2";
                    StartD3Main.Text = (d3Running) ? "Stop D3" : "Start D3";

                    trayMenuItemStart.Text = (overloadRunning || olmodRunning) ? "&Stop" : "&Start";

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

                CycleTheme();
                Thread.Sleep(1000);

                CycleTheme();
                Thread.Sleep(1000);

                CycleTheme();
                Thread.Sleep(850);
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
            TestSetTextBoxColor(OlmodExecutable);
            TestSetTextBoxColor(Descent3Executable);
            TestSetTextBoxColor(Descent2Executable);
            TestSetTextBoxColor(OnStartAppPath);
            TestSetTextBoxColor(OnStopAppPath);
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

        private void ExitButton_Click(object sender, EventArgs e)
        {
            StopButton_Click(null, null);
            Close();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartStopButton.Text.Contains("Stop"))
            {
                enableDisableKeys.SuppressWinKeys = false;

                StopButton_Click(null, null);
                StartStopButton.Text = "Start client";
            }
            else
            {
                // Switch display if enabled and the display exists.
                CheckSwitchDisplay();

                enableDisableKeys.SuppressWinKeys = SuppressWinKeys;

                LaunchOverloadClient();
            }
        }

        private void CheckSwitchDisplay()
        {
            if (GamingDisplayCheckBox.Checked)
            {
                string gamingDisplay = GamingMonitorComboBox.SelectedItem as string;
                if (DisplayManager.Displays.ContainsValue(gamingDisplay))
                {
                    if (gamingDisplay != DisplayManager.PrimaryDisplay) DisplayManager.SetAsPrimaryMonitor(gamingDisplay);
                }
            }
        }

        private void LaunchOverloadClient()
        {
            LogDebugMessage("LaunchOverloadClient()");

            string exePath = OverloadExecutable.Text;
            string olmodExe = OlmodExecutable.Text;

            if (UseOlmodCheckBox.Checked)
            {
                exePath = OlmodExecutable.Text;
                if (!OverloadClientToolApplication.ValidFileName(exePath, true))
                {
                    MessageBox.Show("Olmod (.exe) application not found!");
                    return;
                }
            }
            else
            {
                if (!OverloadClientToolApplication.ValidFileName(exePath, true))
                {
                    MessageBox.Show("Overload (.exe) application not found!");
                    return;
                }
            }

            LogDebugMessage($"Setting up name and parameters");

            string name = Path.GetFileNameWithoutExtension(exePath);
            string path = Path.GetDirectoryName(OverloadExecutable.Text);

            if (path.EndsWith("\\")) path = path.Substring(0, path.Length - 1);

            // Prepare command line parameters.
            string commandLineArgs = OverloadParameters.Trim();

            // Add Olmod parameters if Olmod is enabled.
            if (UseOlmodCheckBox.Checked)
            {
                if (PassGameDirToOlmod && !commandLineArgs.ToLower().Contains("-gamedir")) commandLineArgs = " -gamedir \"" + path + "\"";
                if (ShowFPS && !commandLineArgs.ToLower().Contains("-frametime")) commandLineArgs += " -frametime";
            }

            commandLineArgs += " " + OverloadParameters;
            commandLineArgs = commandLineArgs.Trim();

            LogDebugMessage($"Setting up name and parameters");

            // Start application it is not already running.
            int running = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.Id != serverProcessId) && (process.ProcessName.ToLower() == name)) running++;
            }

            if (running == 1)
            {
                if (name.ToLower().Contains("olmod")) Info("Overload (Olmod) ís already running.");
                else Info("Overload is already running.");
                return;
            }

            if (name.ToLower().Contains("olmod")) Info($"Starting up Overload (using Olmod).");
            else Info($"Starting up Overload.");

            // If more than one is running we kill them all and start fresh instance.
            if (running > 1) KillRunningProcess(name);

            LogDebugMessage($"Launcing{exePath} with \"{commandLineArgs}\"");

            // (Re)start application..
            Process appStart = new Process
            {
                StartInfo = new ProcessStartInfo(exePath, commandLineArgs)
            };

            appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            appStart.Start();

            // Check if we need to start pre-app.
            try 
            {
                NeedToRunPostApp = true;

                Process preAppStart = new Process
                {
                    StartInfo = new ProcessStartInfo(OnStartAppPath.Text)
                    {
                        WorkingDirectory = Path.GetDirectoryName(OnStartAppPath.Text)
                    }
                };

                preAppStart.Start();
            } 
            catch
            {
            }
        }

        private void RunPostApp()
        {
            if (NeedToRunPostApp)
            {
                NeedToRunPostApp = false;
                try
                {
                    Process postApp = new Process
                    {
                        StartInfo = new ProcessStartInfo(OnStopAppPath.Text)
                        {
                            WorkingDirectory = Path.GetDirectoryName(OnStopAppPath.Text)
                        }
                    };

                    postApp.Start();
                }
                catch
                {
                }
            }
        }

        private void RestoreDisplayConfiguration()
        {
            if (NeedToRestoreDisplay)
            {
                NeedToRestoreDisplay = false;
                if (DisplayConfiguration != null)
                {
                    DisplayConfiguration.ActiveConfiguration = DisplayConfiguration;
                }
            }
        }

        // Return process if instance is active otherwise return null.
        public Process GetRunningProcess(string name)
        {
            if (String.IsNullOrEmpty(name)) return null;

            foreach (Process process in Process.GetProcesses())
            {
                if ((process.Id != serverProcessId) && process.ProcessName.ToLower() == name.ToLower()) return process;
            }
            return null;
        }

        private void KillRunningProcess(string name)
        {
            if (String.IsNullOrEmpty(name)) return;

            foreach (Process process in Process.GetProcesses())
            {
                if ((process.Id != serverProcessId) && process.ProcessName.ToLower() == name.ToLower()) process.Kill();
            }
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
            ValidateButton(StartStopButton, theme);

            Defocus();

            RestoreDisplayConfiguration();
            RunPostApp();

            Info("Shutting down Overload.");

            KillRunningProcess("overload");
            KillRunningProcess("olmod");
        }

        private void CheckDisplaySwitch()
        {
            // Switch display if enabled and the display exists.
            if (DefaultDisplayCheckBox.Checked)
            {
                string defaultDisplay = DefaultMonitorComboBox.SelectedItem as string;
                if (DisplayManager.Displays.ContainsValue(defaultDisplay))
                {
                    if (defaultDisplay != DisplayManager.PrimaryDisplay) DisplayManager.SetAsPrimaryMonitor(defaultDisplay);
                }
            }
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
            // WindowState = FormWindowState.Normal;

            if (WindowState == FormWindowState.Normal) WindowState = FormWindowState.Minimized;
            else WindowState = FormWindowState.Normal;
        }

        private void UseOlmod_CheckedChanged(object sender, EventArgs e)
        {
            UseOlmod = UseOlmodCheckBox.Checked;
            Info((UseOlmodCheckBox.Checked) ? "Olmod enabled." : "Olmod disabled.");
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

        private void OverloadMapDatabase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string url = OverloadMapDatabaseUrl.Text;
                url += "?teamname=JAT&modus=3";

                ProcessStartInfo sInfo = new ProcessStartInfo(url);
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

            mapManager.Resort(NewMapsFirstCheckBox.Checked);

            foreach (KeyValuePair<string, OverloadMap> map in mapManager.SortedMaps)
            {
                if (map.Value.IsLocal && !(HideHiddenMaps && map.Value.Hidden))
                {
                    MapsListBox.Items.Add(map.Value);
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

            UnhideAllMapsButton.Enabled = true; // anyHidden;
            ApplyThemeToControl(UnhideAllMapsButton, theme);

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
                MapsPanel.Enabled = false;

                MapRefreshButton.Enabled = false;
                MapDeleteButton.Enabled = false;
                MapHideButton.Enabled = false;

                UseDLCLocationCheckBox.Enabled = false;
                AutoUpdateMapsCheckBox.Enabled = false;
                OnlyUpdateExistingMapsCheckBox.Enabled = false;
                HideUnofficialMapsCheckBox.Enabled = false;
                HideHiddenMapsCheckBox.Enabled = false;

                MPMapsCheckBox.Enabled = false;
                SPMapsCheckBox.Enabled = false;
                CMMapsCheckBox.Enabled = false;

                HideMPMapsButton.Enabled = false;
                HideSPMapsButton.Enabled = false;
                HideCMMapsButton.Enabled = false;
                UnhideMPMapsButton.Enabled = false;
                UnhideSPMapsButton.Enabled = false;
                UnhideCMMapsButton.Enabled = false;
                UnhideAllMapsButton.Enabled = false;

                OnlineMapJsonUrl.Enabled = false;

                ApplyThemeToControl(MapsPanel, theme);

                if (UpdateOnlyExistingMaps) Verbose(String.Format("Checking for updated maps."));
                else Verbose(String.Format("Checking for new/updated maps."));

                mapManager.SaveNewMapsToDLCFolder = (UseDLCLocation && !String.IsNullOrEmpty(dlcLocation));
                Verbose(String.Format("Overload " + ((UseDLCLocation && !String.IsNullOrEmpty(dlcLocation)) ? "DLC" : "application") + " folder used for new maps."));
            });

            // UpdateAllMaps() cannot touch UI elements!
            mapManager.UpdateAllMaps(OnlineMapJsonUrl.Text, dlcLocation, null);

            this.UIThread(delegate
            {
                if (UpdateOnlyExistingMaps) Verbose(String.Format($"Map check finished: {mapManager.Checked} maps, {mapManager.Updated} updated."));
                else Verbose(String.Format($"Map check finished: {mapManager.Checked} maps checked, {mapManager.Created} created, {mapManager.Updated} updated."));

                UpdateMapListBox();

                UpdatingMaps.Visible = false;
                MapUpdateButton.Enabled = true;

                MapsListBox.Enabled = true;
                MapsPanel.Enabled = true;

                UseDLCLocationCheckBox.Enabled = true;
                AutoUpdateMapsCheckBox.Enabled = true;
                OnlyUpdateExistingMapsCheckBox.Enabled = true;
                HideUnofficialMapsCheckBox.Enabled = true;
                HideHiddenMapsCheckBox.Enabled = true;

                MPMapsCheckBox.Enabled = true;
                SPMapsCheckBox.Enabled = true;
                CMMapsCheckBox.Enabled = true;

                HideMPMapsButton.Enabled = true;
                HideSPMapsButton.Enabled = true;
                HideCMMapsButton.Enabled = true;
                UnhideMPMapsButton.Enabled = true;
                UnhideSPMapsButton.Enabled = true;
                UnhideCMMapsButton.Enabled = true;
                UnhideAllMapsButton.Enabled = true;

                OnlineMapJsonUrl.Enabled = true;

                ApplyThemeToControl(MapsPanel, theme);

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

            latest.Version = OverloadClientToolApplication.VersionStringFix(latest.Version);

            string olmodVersion = OverloadClientToolApplication.GetFileVersion(OlmodPath.ToLower().Replace("olmod.exe", "GameMod.dll"));

            if (olmodVersion != null) olmodVersion = OverloadClientToolApplication.VersionStringFix(olmodVersion);

            if (olmodVersion != null)
            {
                // A version of Olmod is already installed.
                //if (olmodVersion == latest.Version) return;

                if (sender != null)
                {
                    OverloadClientApplication.OlmodUpdateForm updateForm = new OverloadClientApplication.OlmodUpdateForm(olmodVersion, latest.Version);
                    ApplyThemeToControl(updateForm, theme);
                    updateForm.BackColor = theme.InactivePaneButtonBackColor;
                    updateForm.StartPosition = FormStartPosition.CenterParent;
                    if (updateForm.ShowDialog() == DialogResult.Cancel) return;
                }
                else if (olmodVersion == latest.Version)
                {
                    return;
                }
            }

            Info("Installing current Olmod release from Github.");

            try
            {
                olmodManager.DownloadAndInstallOlmod(latest, olmodInstallFolder);

                // May have a new version of GameMod.dll to add to the selection list.
                UpdateGameModList();
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
                return String.Format($"Olmod {olmodVersion}");
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

        private void AvailableThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AvailableThemesListBox.SelectedIndex >= 0)
            {
                string newThemeName = (string)AvailableThemesListBox.Items[AvailableThemesListBox.SelectedIndex];

                theme = Theme.GetThemeByName(newThemeName);
                ActiveThemeName = newThemeName;

                UpdateTheme(theme);

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

        public void LogTreeViewText(string text, bool error = false)
        {
            if (LogTreeView.Nodes.Count > 9999) LogTreeView.Nodes[0].Remove();

            LogTreeView.Nodes.Add(text);

            LogTreeView.ShowNodeToolTips = true;
            TreeNode node = LogTreeView.Nodes[LogTreeView.Nodes.Count - 1];
            node.ToolTipText = text;
            node.Tag = (error) ? "Error" : "Info";

            LogTreeView.Nodes[LogTreeView.Nodes.Count - 1].EnsureVisible();
        }

        private void LogTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            String nodeText = String.IsNullOrEmpty(e.Node.Text) ? "- This message shouldn't be shown - " : e.Node.Text;
            string text = nodeText;

            Rectangle rect = new Rectangle(0, 0, 0, 0);
            try
            {
                rect = new Rectangle(0, e.Node.Bounds.Y, LogTreeView.Width, e.Node.Bounds.Height);
                Size size = TextRenderer.MeasureText(text, treeViewFont);

                while (size.Width > (rect.Width + 8))
                {
                    text = text.Substring(0, text.Length - 3);
                    size = TextRenderer.MeasureText(text, treeViewFont);
                }

                if (text.Length < nodeText.Length) text += "...";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception in LogTreeView_DrawNode [1]");
            }

            Color color = theme.InputForeColor;
            try
            {
                // Draw the background of the ListBox control for each item.
                rect.Y++;
                e.Graphics.FillRectangle(new SolidBrush(theme.InputBackColor), rect);
                rect.Y--;

                string test = e.Node.Tag as string;
                test = String.IsNullOrEmpty(test) ? "" : test;
                if (test.ToLower().Contains("error")) color = theme.TextHighlightColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception in LogTreeView_DrawNode [2]");
            }

            try
            {
                // Draw the current item text
                e.Graphics.DrawString(text, treeViewFont, new SolidBrush(color), e.Bounds, StringFormat.GenericDefault);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception in LogTreeView_DrawNode [3]");
            }

        }

        private void FrameTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowFPS = FrameTimeCheckBox.Checked;
        }

        private void OnlyMinimizeOnClose_CheckedChanged(object sender, EventArgs e)
        {
            MinimizeOnClose = OnlyMinimizeOnClose.Checked;
        }

        #region Server
        private void ServerTrackerName_TextChanged(object sender, EventArgs e)
        {
            OlmodServerName = ServerTrackerName.Text;
        }

        private void ServerTrackerUrl_TextChanged(object sender, EventArgs e)
        {
            OlmodServerTrackerBaseUrl = ServerTrackerUrl.Text;
            ClickableTrackerUrl.Text = ServerTrackerUrl.Text;
        }

        private void ServerAnnounceOnTrackerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OlmodIsServer = ServerAnnounceOnTrackerCheckBox.Checked;
        }

        private void ServerAutoSignOffTracker_CheckedChanged(object sender, EventArgs e)
        {
            OlmodServerKeepListed = ServerKeepListed.Checked;
        }

        private void AutoStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AutoStartServer = AutoStartCheckBox.Checked;
        }

        private void AssistScoringCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            OlmodAssistScoring = AssistScoringCheckBox.Checked;
        }

        private void ServerTrackerNotes_TextChanged(object sender, EventArgs e)
        {
            OlmodServerNotes = ServerTrackerNotes.Text;
        }
        #endregion

        private void UseTrayIcon_CheckedChanged(object sender, EventArgs e)
        {
            TrayInsteadOfTaskBar = UseTrayIcon.Checked;
            ShowInTaskbar = !UseTrayIcon.Checked;
            OverloadClientToolNotifyIcon.Visible = UseTrayIcon.Checked;
        }

        private void MinimizeOnStartupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            StartMinimized = MinimizeOnStartupCheckBox.Checked;
        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            if (serverProcessId > 0)
            {
                try
                {
                    foreach (Process process in Process.GetProcesses())
                    {
                        if (process.Id == serverProcessId) process.Kill();
                    }
                }
                catch
                {
                }
                finally
                {
                    serverProcessId = 0;
                }

                StartServerButton.Text = "Start server";
                StartServerButtonMain.Text = StartServerButton.Text;
                trayMenuItemStartServer.Text = "St&art server";

                return;
            }

            StartServerButton.Text = "Stop server";
            StartServerButtonMain.Text = StartServerButton.Text;
            trayMenuItemStartServer.Text = "St&op server";

            string exePath = OverloadExecutable.Text;
            string olmodExe = OlmodExecutable.Text;

            if (UseOlmodCheckBox.Checked)
            {
                exePath = OlmodExecutable.Text;
                if (!OverloadClientToolApplication.ValidFileName(exePath, true))
                {
                    StartServerButton.Text = "Start server";
                    StartServerButtonMain.Text = StartServerButton.Text;
                    MessageBox.Show("Olmod (.exe) application not found!");
                    return;
                }

                // Save settings for Olmod.
                SetOlmodSettings();
            }
            else
            {
                if (!OverloadClientToolApplication.ValidFileName(exePath, true))
                {
                    StartServerButton.Text = "Start server";
                    StartServerButtonMain.Text = StartServerButton.Text;
                    MessageBox.Show("Overload (.exe) application not found!");
                    return;
                }
            }

            string name = Path.GetFileNameWithoutExtension(exePath);

            // Prepare command line parameters.
            string commandLineArgs = OverloadParameters.Trim();

            // Setup server parameters.
            if (!commandLineArgs.ToLower().Contains("-batchmode")) commandLineArgs += " -batchmode";
            if (!commandLineArgs.ToLower().Contains("-nographics")) commandLineArgs += " -nographics";

            string path = Path.GetDirectoryName(OverloadExecutable.Text);
            if (path.EndsWith("\\")) path = path.Substring(0, path.Length - 1);

            // Add Olmod parameters if Olmod is enabled.
            if (UseOlmodCheckBox.Checked)
            {
                if (PassGameDirToOlmod && !commandLineArgs.ToLower().Contains("-gamedir")) commandLineArgs += " -gamedir \"" + path + "\"";
            }

            commandLineArgs += " " + OverloadParameters;
            commandLineArgs = commandLineArgs.Trim();

            LogDebugMessage($"Launching server {exePath} with \"{commandLineArgs}\"");

            // Start server.
            if (name.ToLower().Contains("olmod")) Info($"Starting up Overload server (using Olmod).");
            else Info($"Starting up Overload server.");

            // (Re)start application..
            Process appStart = new Process
            {
                StartInfo = new ProcessStartInfo(exePath, commandLineArgs)
            };

            appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);

            appStart.StartInfo.UseShellExecute = false;

            appStart.StartInfo.RedirectStandardError = true;
            appStart.StartInfo.RedirectStandardOutput = true;

            appStart.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(ServerLogging);
            appStart.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(ServerErrorLogging);

            appStart.EnableRaisingEvents = true;
            appStart.Exited += new System.EventHandler(ServerProcessExit);

            appStart.Start();

            serverProcessId = appStart.Id;

            appStart.BeginErrorReadLine();
            appStart.BeginOutputReadLine();
        }

        private void ServerProcessExit(object sender, EventArgs e)
        {
            Info(String.Format($"Server process has exited."));
        }

        private void ServerErrorLogging(object sender, DataReceivedEventArgs e)
        {
            string s = ServerCleanString(e.Data);
            if (s != null) Info("Server: " + ServerCleanString(e.Data));
        }

        private void ServerLogging(object sender, DataReceivedEventArgs e)
        {
            string s = ServerCleanString(e.Data);
            if (s != null) Info("Server: " + ServerCleanString(e.Data));
        }

        private string ServerCleanString(string s)
        {
            if (s == null) return null;

            int am = s.IndexOf("AM:");
            int pm = s.IndexOf("PM:");

            if (am > 0) s = s.Substring(am + 3).Trim();
            else if (pm > 0) s = s.Substring(pm + 3).Trim();

            if (s.StartsWith("......")) s = "  " + s.Substring(6);

            return s;
        }

        private void DisplayHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string tempPdfFile = Path.Combine(Path.GetTempPath(), "Overload Client Tool Help.html");
            System.IO.File.WriteAllText(tempPdfFile, HelpFileBytes);
            using (Process helpProcess = new Process())
            {
                helpProcess.StartInfo.FileName = tempPdfFile;
                helpProcess.Start();
            }
        }

        private void MpMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            IncludeMP = MPMapsCheckBox.Checked;
            mapManager.IncludeMP = IncludeMP;
        }

        private void SPMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            IncludeSP = SPMapsCheckBox.Checked;
            mapManager.IncludeSP = IncludeSP;

            if (IncludeSP && !OnlineMapJsonUrl.Text.ToLower().EndsWith("all.json"))
            {
                if (MessageBox.Show("Also include all map types when uypdating maps?", "Update map JSON URL?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    OnlineMapJsonUrl.Text = @"https://www.overloadmaps.com/data/all.json";
                    MapListUrl = OnlineMapJsonUrl.Text;
                }
            }
        }

        private void CMMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            IncludeCM = CMMapsCheckBox.Checked;
            mapManager.IncludeCM = IncludeCM;

            if (IncludeCM && !OnlineMapJsonUrl.Text.ToLower().EndsWith("all.json"))
            {
                if (MessageBox.Show("Also include all map types when uypdating maps?", "Update map JSON URL?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    OnlineMapJsonUrl.Text = @"https://www.overloadmaps.com/data/all.json";
                    MapListUrl = OnlineMapJsonUrl.Text;
                }
            }
        }

        private void HideHiddenMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HideHiddenMaps = HideHiddenMapsCheckBox.Checked;
            UpdateMapListBox();
            Unfocus();
        }

        private void HideMPMapsButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.SortedMaps)
            {
                if (setMap.Value.IsMPMap) setMap.Value.Hidden = true;
            }

            UpdateMapListBox();
            Unfocus();
        }

        private void HideSPMapsButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.SortedMaps)
            {
                if (setMap.Value.IsSPMap) setMap.Value.Hidden = true;
            }

            UpdateMapListBox();
            Unfocus();
        }

        private void HideCMMapsButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.SortedMaps)
            {
                if (setMap.Value.IsCMMap) setMap.Value.Hidden = true;
            }

            UpdateMapListBox();
            Unfocus();
        }

        private void UnhideMPMapsButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.SortedMaps)
            {
                if (setMap.Value.IsMPMap) setMap.Value.Hidden = false;
            }

            UpdateMapListBox();
            Unfocus();
        }

        private void UnhideSPMapsButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.SortedMaps)
            {
                if (setMap.Value.IsSPMap) setMap.Value.Hidden = false;
            }

            UpdateMapListBox();
            Unfocus();
        }

        private void UnhideCMMapsButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.SortedMaps)
            {
                if (setMap.Value.IsCMMap) setMap.Value.Hidden = false;
            }

            UpdateMapListBox();
            Unfocus();
        }

        List<Server> globalServers = null;

        private void UpdateServerListButton_Click(object sender, EventArgs e)
        {
            if (exited || shutdown) return;

            string oldIP = "";

            if ((ServersListView.SelectedIndices == null) || (ServersListView.SelectedIndices.Count < 1))
            {
                CurrentServerNotes.Text = "";
                CurrentServerStarted.Text = "";
                CurrentServerMap.Text = "";
            }
            else
            {
                int oldIndex = ServersListView.SelectedIndices[0];
                oldIP = ServersListView.Items[oldIndex].SubItems[0].Text;
            }

            List<Server> servers = Servers.ServerList;

            if (servers == null) servers = globalServers;
            if (servers == null) return;

            globalServers = servers;

            // IP, Name, Mode, Players, MaxPlayers, Started, Notes.

            ServersListView.Items.Clear();
            foreach (Server server in servers)
            {
                pinger.AddHost(server.IP);

                string[] values = new string[6];
                values[0] = server.IP;
                values[1] = server.Name;
                values[2] = server.Mode;
                values[3] = server.NumPlayers.ToString().PadLeft(3);
                values[4] = server.MaxNumPlayers.ToString().PadLeft(3);
                values[5] = pinger.PingTime(server.IP);

                ListViewItem lvi = new ListViewItem(values);
                lvi.Tag = server.IP;

                ServersListView.Items.Add(lvi);
            }

            SortServers(((ListViewItemComparer)ServersListView.ListViewItemSorter).Column, true);

            int i = 0;
            foreach (ListViewItem item in ServersListView.Items)
            {
                if (item.SubItems[0].Text == oldIP) ServersListView.Items[i].Selected = true;
                i++;
            }
        }

        internal void UpdatePingTimes()
        {
            if (exited || shutdown) return;

            for (int i = 0; i < ServersListView.Items.Count; i++)
            {
                try
                {
                    ListViewItem item = ServersListView.Items[i];
                    item.SubItems[5].Text = pinger.PingTime(item.SubItems[0].Text);
                }
                catch
                {
                }
            }
        }

        private void ServersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ServersListView.SelectedIndices.Count < 1)
            {
                CurrentServerNotes.Text = "";
                CurrentServerStarted.Text = "";
                CurrentServerMap.Text = "";
                return;
            }

            int i = ServersListView.SelectedIndices[0];
            string ip = ServersListView.Items[i].SubItems[0].Text;

            if (globalServers == null) globalServers = Servers.ServerList;

            if (globalServers == null) return;

            foreach (Server server in globalServers)
            {
                if (server.IP == ip)
                {
                    CurrentServerNotes.Text = server.Notes;
                    CurrentServerStarted.Text = server.Started.ToString("yyyy-MM-dd HH:mm:ss");
                    CurrentServerMap.Text = server.Map;
                }
            }
        }

        internal void DrawServerListViewBackground(Theme theme)
        {
            //ServersListView.BackColor = theme.PanelBackColor;
            ServersListView.BackColor = theme.InputBackColor;

            //Graphics graphics = this.CreateGraphics();
            //Point locationOnForm = ServersListView.FindForm().PointToClient(ServersListView.Parent.PointToScreen(ServersListView.Location));
            // Draw the background of the ListBox control for each item.
            //graphics.FillRectangle(new SolidBrush(Color.Red), new RectangleF(locationOnForm, ServersListView.ClientSize));
        }

        private void ServersListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            //if (e.ColumnIndex < 0) return;

            // Draw the background of the column header.
            //e.Graphics.FillRectangle(new SolidBrush(theme.ButtonEnabledBackColor), e.Bounds);

            // Draw the current item text
            //e.Graphics.DrawString(ServersListView.Columns[e.ColumnIndex].Text, treeViewFont, new SolidBrush(theme.ButtonEnabledForeColor), e.Bounds, StringFormat.GenericDefault);
        }

        private void ServersListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.ItemIndex < 0) return;

            // Draw the background of the ListBox control for each item.
            //e.Graphics.FillRectangle(new SolidBrush(theme.PanelBackColor), e.Bounds);

            // Draw the current item text

            //string text = ServersListView.Items[e.ItemIndex].SubItems[0].Text;
            //e.Graphics.DrawString(ServersListBox.Items[e.ItemIndex]., treeViewFont, new SolidBrush(theme.PanelForeColor), e.Bounds, StringFormat.GenericDefault);
        }

        private void ServersListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ItemIndex < 0) return;

            //Color b = theme.PanelBackColor;
            Color b = theme.InputBackColor;
            Color f = theme.PanelForeColor;

            if ((ServersListView.SelectedIndices != null) && (ServersListView.SelectedIndices.Count > 0))
            {
                if (e.ItemIndex == ServersListView.SelectedIndices[0])
                {
                    b = theme.ButtonEnabledBackColor;
                    f = theme.ButtonEnabledForeColor;
                }
            }

            // Draw the background of the ListBox control for each item.
            e.Graphics.FillRectangle(new SolidBrush(b), e.Bounds);

            // Lower the text a bit.
            Rectangle bounds = e.Bounds;
            bounds.X += 1;
            bounds.Y += 1;

            // Draw the current item text
            string text = e.SubItem.Text.Replace("\r", "").Replace("\n", "");

            // Don't show '0' for 'numPlayers' or 'maxNumPlayers'.
            if ((e.ColumnIndex == 3) || (e.ColumnIndex == 4))
            {
                if (text.Trim() == "0") text = "";
            }

            // Ping time.
            if (e.ColumnIndex == 5)
            {
                while (text.StartsWith("0")) text = text.Substring(1);
                if (text == "9999") text = "";
            }

            e.Graphics.DrawString(text, treeViewFont, new SolidBrush(f), bounds, StringFormat.GenericTypographic);
        }

        private void ServersListView_DoubleClick(object sender, EventArgs e)
        {
            if (ServersListView.SelectedIndices.Count < 1)
            {
                CurrentServerNotes.Text = "";
                CurrentServerStarted.Text = "";
                CurrentServerMap.Text = "";
                return;
            }

            int i = ServersListView.SelectedIndices[0];
            string ip = ServersListView.Items[i].SubItems[0].Text;


            Clipboard.SetText(ip);

            Graphics graphics = ServersListView.CreateGraphics();
            Color b = theme.ButtonEnabledBackColor;
            Color f = theme.ButtonEnabledForeColor;

            Rectangle rect = ServersListView.Items[i].GetBounds(ItemBoundsPortion.Entire);
            rect.Height--;

            graphics.DrawRectangle(new Pen(f), rect);
            Thread.Sleep(100);
            graphics.DrawRectangle(new Pen(b), rect);
        }

        private class ListViewItemComparer : IComparer
        {
            public SortOrder Order = SortOrder.Descending;
            public int Column = 0;

            public ListViewItemComparer()
            {
            }

            public ListViewItemComparer(int column)
            {
                Column = column;
            }

            public int Compare(object x, object y)
            {
                int result = String.Compare(((ListViewItem)x).SubItems[Column].Text, ((ListViewItem)y).SubItems[Column].Text);

                if (Order == SortOrder.Descending) return -result;
                else return result;
            }
        }

        private void SortServers(int column, bool noChange = false)
        {
            ListViewItemComparer comparer = (ListViewItemComparer)ServersListView.ListViewItemSorter;

            if (noChange == false)
            {
                if (comparer.Column != column)
                {
                    comparer.Column = column;
                }
                else
                {
                    if (comparer.Order == SortOrder.Ascending) comparer.Order = SortOrder.Descending;
                    else comparer.Order = SortOrder.Ascending;
                }
            }

            if (column == 0) SetSortArrow(LabelServerIP, comparer.Order);
            if (column == 1) SetSortArrow(LabelServerName, comparer.Order);
            if (column == 2) SetSortArrow(LabelServerGameMode, comparer.Order);
            if (column == 3) SetSortArrow(LabelServerPlayers, comparer.Order);
            if (column == 4) SetSortArrow(LabelServerMaxPlayers, comparer.Order);
            if (column == 5) SetSortArrow(LabelServerPing, comparer.Order);

            ServersListView.Sort();
        }

        private void SetSortArrow(Label label, SortOrder order)
        {
            if (order == SortOrder.Descending)
            {
                LabelDownArrow.Visible = false;
                LabelUpArrow.ForeColor = theme.TextHighlightColor;
                LabelUpArrow.BackColor = theme.PanelBackColor;
                LabelUpArrow.Location = new Point(label.Location.X + label.Width - 4, label.Location.Y);
                LabelUpArrow.Visible = true;
            }
            else
            {
                LabelUpArrow.Visible = false;
                LabelDownArrow.ForeColor = theme.TextHighlightColor;
                LabelDownArrow.BackColor = theme.PanelBackColor;
                LabelDownArrow.Location = new Point(label.Location.X + label.Width - 4, label.Location.Y);
                LabelDownArrow.Visible = true;
            }
        }

        private void LabelServerIP_Click(object sender, EventArgs e)
        {
            SortServers(0);
        }

        private void LabelServerName_Click(object sender, EventArgs e)
        {
            SortServers(1);
        }

        private void LabelServerGameMode_Click(object sender, EventArgs e)
        {
            SortServers(2);
        }

        private void LabelServerPlayers_Click(object sender, EventArgs e)
        {
            SortServers(3);
        }

        private void LabelServerMaxPlayers_Click(object sender, EventArgs e)
        {
            SortServers(4);
        }

        private void LabelServerPing_Click(object sender, EventArgs e)
        {
            SortServers(5);
        }

        private void DefaultDisplayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SwitchDefault = DefaultDisplayCheckBox.Checked;
        }

        private void GamingDisplayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SwitchGaming = GamingDisplayCheckBox.Checked;
        }

        private void DefaultMonitorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DefaultDisplay = DefaultMonitorComboBox.SelectedItem as string;

            new System.Threading.Timer((s) =>
            {
                DefaultMonitorComboBox.Invoke(new Action(() =>
                {
                    Defocus();
                }));
            }, null, 10, System.Threading.Timeout.Infinite);
        }

        private void GamingMonitorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GamingDisplay = GamingMonitorComboBox.SelectedItem as string;

            new System.Threading.Timer((s) =>
            {
                GamingMonitorComboBox.Invoke(new Action(() =>
                {
                    Defocus();
                }));
            }, null, 10, System.Threading.Timeout.Infinite);
        }

        private void SuppressWinKeysCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SuppressWinKeys = SuppressWinKeysCheckBox.Checked;
        }

        private void Descent2Executable_TextChanged(object sender, EventArgs e)
        {
            D2App = Descent2Executable.Text;
            ValidateSettings();
        }

        private void Descent3Executable_TextChanged(object sender, EventArgs e)
        {
            D3App = Descent3Executable.Text;
            ValidateSettings();
        }

        private void Descent3Executable_DoubleClick(object sender, EventArgs e)
        {
            Descent3Executable.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            SelectExecutable.FileName = Path.GetFileName(Descent3Executable.Text);
            SelectExecutable.InitialDirectory = Path.GetDirectoryName(Descent3Executable.Text);

            DialogResult result = SelectExecutable.ShowDialog();

            if (result == DialogResult.OK)
            {
                Descent3Executable.Text = SelectExecutable.FileName;
                Descent3Executable.SelectionLength = 0;

                D3App = Descent3Executable.Text;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void Descent2Executable_DoubleClick(object sender, EventArgs e)
        {
            Descent3Executable.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            SelectExecutable.FileName = Path.GetFileName(Descent2Executable.Text);
            SelectExecutable.InitialDirectory = Path.GetDirectoryName(Descent2Executable.Text);

            DialogResult result = SelectExecutable.ShowDialog();

            if (result == DialogResult.OK)
            {
                Descent2Executable.Text = SelectExecutable.FileName;
                Descent2Executable.SelectionLength = 0;

                D2App = Descent2Executable.Text;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void Descent3Args_TextChanged(object sender, EventArgs e)
        {
            D3Args = Descent3Args.Text;
        }

        private void StartD3Main_Click(object sender, EventArgs e)
        {
            if (StartD3Main.Text.Contains("Stop D3"))
            {
                enableDisableKeys.SuppressWinKeys = false;

                KillD3();

                StartD3Main.Text = "Start D3";
            }
            else
            {
                // Switch display if enabled and the display exists.
                CheckSwitchDisplay();

                enableDisableKeys.SuppressWinKeys = SuppressWinKeys;

                LaunchD3();

                StartD3Main.Text = "Stop D3";
            }
        }

        private void KillD3()
        {
            string name = Path.GetFileNameWithoutExtension(Descent3Executable.Text);
            if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);
            KillRunningProcess(name);
        }

        private void LaunchD3()
        {
            LogDebugMessage("LaunchD3()");

            string exePath = Descent3Executable.Text;
            if (!OverloadClientToolApplication.ValidFileName(exePath, true))
            {
                MessageBox.Show("Descent 3 (main.exe) not found!");
                return;
            }

            LogDebugMessage($"Setting up name and parameters");

            string name = Path.GetFileNameWithoutExtension(exePath);
            if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);

            // Prepare command line parameters.
            string commandLineArgs = D3Args.Trim();

            // Start application it is not already running.
            int running = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name) running++;
            }

            if (running > 0)
            {
                Info("Descent 3 (main.exe) ís already running.");
                return;
            }

            LogDebugMessage($"Launcing {exePath} with \"{commandLineArgs}\"");

            // Start application..
            Process appStart = new Process
            {
                StartInfo = new ProcessStartInfo(exePath, commandLineArgs)
            };

            appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            appStart.Start();

            D3ProcessId = appStart.Id;
        }

        private void KillD2()
        {
            string name = Path.GetFileNameWithoutExtension(Descent2Executable.Text);
            if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);
            KillRunningProcess(name);
        }

        private void StartD2_Click(object sender, EventArgs e)
        {
            if (StartD2.Text.Contains("Stop D2"))
            {
                enableDisableKeys.SuppressWinKeys = false;

                KillD2();

                StartD2.Text = "Start D2";
            }
            else
            {
                // Switch display if enabled and the display exists.
                CheckSwitchDisplay();

                enableDisableKeys.SuppressWinKeys = SuppressWinKeys;

                LaunchD2();

                StartD2.Text = "Stop D2";
            }
        }

        private void LaunchD2()
        {
            LogDebugMessage("LaunchD2()");

            string exePath = Descent2Executable.Text;
            if (!OverloadClientToolApplication.ValidFileName(exePath, true))
            {
                MessageBox.Show("Descent 2 application not found!");
                return;
            }

            LogDebugMessage($"Setting up name and parameters");

            string name = Path.GetFileNameWithoutExtension(exePath);
            if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);

            // Prepare command line parameters.
            //string commandLineArgs = D3Args.Trim();

            // Start application it is not already running.
            int running = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name) running++;
            }

            if (running > 0)
            {
                Info("Descent 2 ís already running.");
                return;
            }

            Info($"Starting up Descent 2.");

            LogDebugMessage($"Launcing {exePath}");

            // (Re)start application..
            Process appStart = new Process
            {
                StartInfo = new ProcessStartInfo(exePath /*commandLineArgs */)
            };

            appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            appStart.Start();

            D2ProcessId = appStart.Id;
        }

        private void DescentForumSOD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo(DescentForumSOD.Text)); } catch { }
        }

        private void DataiListLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo(DataiListLink.Text)); } catch { }
        }

        private void DXXRebirthLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo(DXXRebirthLink.Text)); } catch { }
        }

        private void KillD1()
        {
            string name = Path.GetFileNameWithoutExtension(Descent1Executable.Text);
            if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);
            KillRunningProcess(name);
        }

        private void StartD1_Click(object sender, EventArgs e)
        {
            if (StartD1.Text.Contains("Stop D1"))
            {
                enableDisableKeys.SuppressWinKeys = false;

                KillD1();

                StartD1.Text = "Start D1";
            }
            else
            {
                // Switch display if enabled and the display exists.
                CheckSwitchDisplay();

                enableDisableKeys.SuppressWinKeys = SuppressWinKeys;

                LaunchD1();

                StartD1.Text = "Stop D1";
            }
        }

        private void LaunchD1()
        {
            LogDebugMessage("LaunchD2()");

            string exePath = Descent1Executable.Text;
            if (!OverloadClientToolApplication.ValidFileName(exePath, true))
            {
                MessageBox.Show("Descent application not found!");
                return;
            }

            string name = Path.GetFileNameWithoutExtension(exePath);
            if (name.EndsWith("\\")) name = name.Substring(0, name.Length - 1);

            // Start application it is not already running.
            int running = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name) running++;
            }

            if (running > 0)
            {
                Info("Descent ís already running.");
                return;
            }

            Info($"Starting up Descent.");

            LogDebugMessage($"Launcing {exePath}");

            // (Re)start application..
            Process appStart = new Process
            {
                StartInfo = new ProcessStartInfo(exePath /*commandLineArgs */)
            };

            appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            appStart.Start();

            D1ProcessId = appStart.Id;
        }

        private void Descent1Executable_TextChanged(object sender, EventArgs e)
        {
            D1App = Descent1Executable.Text;
            ValidateSettings();
        }

        private void Descent1Executable_DoubleClick(object sender, EventArgs e)
        {
            Descent1Executable.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            SelectExecutable.FileName = Path.GetFileName(Descent1Executable.Text);
            SelectExecutable.InitialDirectory = Path.GetDirectoryName(Descent1Executable.Text);

            DialogResult result = SelectExecutable.ShowDialog();

            if (result == DialogResult.OK)
            {
                Descent1Executable.Text = SelectExecutable.FileName;
                Descent1Executable.SelectionLength = 0;

                D1App = Descent1Executable.Text;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void DefaultMonitorComboBox_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void GamingMonitorComboBox_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void GameModComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(IsOverloadOrOlmodRunning))
            {
                try
                {
                    System.IO.File.Copy(Path.Combine(Path.GetDirectoryName(OlmodPath), GameModComboBox.SelectedItem.ToString()), Path.Combine(Path.GetDirectoryName(OlmodPath), "GameMod.dll"), true);
                }
                catch
                {
                }
            }
        }

        private void ClickableTrackerUrl_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(ClickableTrackerUrl.Text);
                Process.Start(sInfo);
            }
            catch
            {
            }
        }

        private void OverloadMaps_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(OverloadMaps.Text);
                Process.Start(sInfo);
            }
            catch
            {
            }
        }

        private void WindowSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WindowSize = WindowSizeComboBox.SelectedItem.ToString();
        }

        private void OnStartAppPath_TextChanged(object sender, EventArgs e)
        {
            OnStartApp = OnStartAppPath.Text;
            ValidateSettings();
        }

        private void OnStopAppPath_TextChanged(object sender, EventArgs e)
        {
            OnStopApp = OnStopAppPath.Text;
            ValidateSettings();
        }

        private void OnStartApp_DoubleClick(object sender, EventArgs e)
        {
            OnStartAppPath.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            try
            {
                OpenAppDialog.FileName = Path.GetFileName(OnStartAppPath.Text);
                OpenAppDialog.InitialDirectory = Path.GetDirectoryName(OnStartAppPath.Text);
            }
            catch
            {
            }

            DialogResult result = OpenAppDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                OnStartAppPath.Text = OpenAppDialog.FileName;
                OnStartAppPath.SelectionLength = 0;

                OnStartApp = OnStartAppPath.Text;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void OnStopAppPath_DoubleClick(object sender, EventArgs e)
        {
            OnStopAppPath.SelectionLength = 0;

            string save = Directory.GetCurrentDirectory();

            try
            {
                OpenAppDialog.FileName = Path.GetFileName(OnStopAppPath.Text);
                OpenAppDialog.InitialDirectory = Path.GetDirectoryName(OnStopAppPath.Text);
            }
            catch
            {
            }

            DialogResult result = OpenAppDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                OnStopAppPath.Text = OpenAppDialog.FileName;
                OnStopAppPath.SelectionLength = 0;

                OnStopApp = OnStopAppPath.Text;
            }

            Directory.SetCurrentDirectory(save);
        }

        private void NewMapsFirstCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            NewestMapFirst = this.NewMapsFirstCheckBox.Checked;
            UpdateMapListBox();
        }

        private void BlackSecondMonitorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            BlankSecondMonitor = BlankSecondMonitorCheckBox.Checked;
        }
    }
}