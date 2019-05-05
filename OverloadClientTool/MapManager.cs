using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OverloadClientTool
{
    internal class OverloadMap
    {
        public string Url;
        public DateTime DateTime;
        public int Size = 0;

        public OverloadMap(string url, DateTime dateTime, int size)
        {
            this.Url = url;
            this.DateTime = dateTime;
            this.Size = size;
        }
    }

    public class OverloadMapManager
    {
        // URL for online map list JSON.
        private string OnlineMapListUrl =  @"https://www.overloadmaps.com/data/mp.json";

        // Note! Must be same string as defined in OCTMain!
        private const string HiddenMarker = "_OCT_Hidden";

        // Default (and recommended) location for local maps.
        private string ApplicationDataOverloadPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Revival\Overload";

        // Optional local map location is Overload DLC (not recommended but used by some people so also supported).
        private string OverloadDLCPath = null;

        // Delegate for sending log messages to main application.
        public delegate void LogMessageDelegate(string message);
        private LogMessageDelegate logger = null;
        private LogMessageDelegate loggerError = null;

        // These are updated after each map update check.
        public int Checked = 0;
        public int Created = 0;
        public int Updated = 0;
        public int Errors = 0;

        // Set delegate for logging.
        public void SetLogger(LogMessageDelegate logger = null, LogMessageDelegate errorLogger = null)
        {
            this.logger = logger;
            if (this.loggerError == null)
            {
                if (errorLogger == null) this.loggerError = logger;
                else this.loggerError = errorLogger;
            }
        }

        // Log an informational message.
        void LogMessage(string s)
        {
            logger?.Invoke(s);
            Debug.WriteLine(s);
        }

        // Log an errir message.
        void LogErrorMessage(string s)
        {
            loggerError?.Invoke(s);
            Debug.WriteLine(s);
        }

        public OverloadMapManager()
        {
        }

        /// <summary>
        /// Get online master map list.
        /// </summary>
        /// <param name="mapListUrl">URL for online JSON list.</param>
        /// <param name="dlcMaps">Path to local Overload DLC directory.</param>
        /// <param name="applicationDataMaps">Path to local Overload application data directory.</param>
        /// <returns></returns>
        public bool Update(string mapListUrl = null, string dlcMaps = null, string applicationDataMaps = null)
        {
            Checked = 0;
            Created = 0;
            Updated = 0;
            Errors = 0;

            // Check for override of default URL.
            if (!String.IsNullOrEmpty(mapListUrl)) OnlineMapListUrl = mapListUrl;

            // Don't use these overrides unless you absolutely have to.
            if (!String.IsNullOrEmpty(dlcMaps)) OverloadDLCPath = dlcMaps;
            if (!String.IsNullOrEmpty(applicationDataMaps)) ApplicationDataOverloadPath = applicationDataMaps;

            List<OverloadMap> maps = new List<OverloadMap>();

            try
            {
                // Get URL base so we can construct full URLs to the online map ZIP files.
                Uri uri = new Uri(OnlineMapListUrl); 
                string baseUrl = String.Concat(uri.Scheme, Uri.SchemeDelimiter, uri.Host); 

                // Get map list and build internal map master list.
                string jsonMapList = new WebClient().DownloadString(OnlineMapListUrl);
                dynamic mapList = JsonConvert.DeserializeObject(jsonMapList);
                foreach (var map in mapList)
                {
                    maps.Add(new OverloadMap(baseUrl + map.url, UnixTimeStampToDateTime(Convert.ToDouble(map.mtime)), Convert.ToInt32(map.size)));
                }

                // Iterate master list and download all new/updated maps.
                for (int i = 0; i < maps.Count;  i++) UpdateMap(maps[i]);
            }
            catch (Exception ex)
            {
                LogErrorMessage(String.Format($"Cannot read map list: {ex.Message}"));
            }

            return (Errors != 0) ? false : true;
        }

        /// <summary>
        /// Update a single Overload map.
        /// </summary>
        /// <param name="map">The map to update.</param>
        /// <returns></returns>
        private bool UpdateMap(OverloadMap map)
        {
            if (String.IsNullOrEmpty(map.Url)) return false;

            // Get the ZIP name from the URL.
            Uri uri = new Uri(map.Url);
            if (uri.Segments.Length < 2) return false;

            // Get the ZIP file name from URL.
            string mapZipName = uri.Segments[uri.Segments.Length - 1];
            if (!mapZipName.ToLower().EndsWith(".zip")) return false;

            Checked++;

            string mapDirectoryPath = String.IsNullOrEmpty(OverloadDLCPath) ? ApplicationDataOverloadPath : OverloadDLCPath;
            string mapZipFilePath = WebUtility.UrlDecode(Path.Combine(mapDirectoryPath, mapZipName));
            string mapZipDisplayName = WebUtility.UrlDecode(mapZipName).Trim();

            // Create directory if it doesn't exist.
            if (!Directory.Exists(mapDirectoryPath)) Directory.CreateDirectory(mapDirectoryPath);

            // Don't update hidden maps.
            if (File.Exists(mapZipFilePath + HiddenMarker)) return false;

                // Check for new map file.
                if (File.Exists(mapZipFilePath))
            {
                // Map found - compare date and size to online version.
                FileInfo fi = new FileInfo(mapZipFilePath);
                if ((fi.Length == map.Size) && (fi.LastWriteTime == map.DateTime)) return false;
            }

            // New map or map needs to be updated.            
            using (var client = new WebClient())
            {
                try
                {
                    bool existingFile = File.Exists(mapZipFilePath);

                    // Get the map from master.
                    client.DownloadFile(map.Url, mapZipFilePath);

                    // Set local files date and time.
                    File.SetCreationTime(mapZipFilePath, map.DateTime);
                    File.SetLastWriteTime(mapZipFilePath, map.DateTime);

                    if (existingFile)
                    {
                        Updated++;
                        LogMessage(String.Format($"Updating map {mapZipDisplayName}."));
                    }
                    else
                    {
                        Created++;
                        LogMessage(String.Format($"Retrieving map {mapZipDisplayName}."));
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Errors++;

                    LogErrorMessage(String.Format($"Error downloading map {mapZipDisplayName}: {ex.Message}"));
                    return false;
                }
            }
        }

        /// <summary>
        /// Convert from UNIX time to .NET DateTime.
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch.
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    public partial class OCTMain
    {
        private const string HiddenMarker = "_OCT_Hidden";

        internal class MapFile
        {
            public string FileName;
            public string Name;

            public bool InDLCFolder;
            public bool Hidden;

            public MapFile(string fileName)
            {
                this.FileName = fileName;
                this.Name = Path.GetFileNameWithoutExtension(FileName);
                this.Hidden = fileName.EndsWith(HiddenMarker);
                this.InDLCFolder = FileName.ToUpper().Contains("\\DLC\\");
            }

            public override string ToString()
            {
                return Name + ((Hidden) ? " [Hidden]" : "") + ((InDLCFolder) ? " [DLC]" : "");
            }

            public void Hide()
            {
                if (Hidden) return;

                try
                {
                    File.Move(FileName, FileName + HiddenMarker);
                    FileName += HiddenMarker;
                    Hidden = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void Unhide()
            {
                if (!Hidden) return;

                try
                {
                    File.Move(FileName, FileName.Replace(HiddenMarker, ""));
                    FileName = FileName.Replace(HiddenMarker, "");
                    Hidden = false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private List<MapFile> currentMaps = null;

        BackgroundWorker mapsBackgroundWorker = null;

        private object mapChangeLock = new object();

        private static bool SameMap(MapFile map1, MapFile map2)
        {
            return (map1.InDLCFolder == map2.InDLCFolder) && (map1.Name.ToLower() == map2.Name.ToLower());
        }

        private void InitMapsListBox()
        {
            currentMaps = Maps;

            // Init listbox.
            foreach (MapFile mapFile in currentMaps) MapsListBox.Items.Add(mapFile);

            MapHideButton.Enabled = (MapsListBox.SelectedIndex >= 0);
            MapDeleteButton.Enabled = (MapsListBox.SelectedIndex >= 0);

            // Begin monitoring folder.
            mapsBackgroundWorker = new BackgroundWorker();
            mapsBackgroundWorker.DoWork += BackgroundMapsChecker;
            mapsBackgroundWorker.RunWorkerAsync();
        }

        private void StopMapsMonitoring()
        {
            if (mapsBackgroundWorker != null)
            {
                mapsBackgroundWorker.DoWork -= BackgroundMapsChecker;
                mapsBackgroundWorker.Dispose();
            }
        }

        private void BackgroundMapsChecker(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(1000);

                this.UIThread(delegate
                {
                    CheckAndUpdateMaplist();
                });
            }
        }

        private void CheckAndUpdateMaplist()
        {
            List<MapFile> mapsInFolder = Maps;

            // See if update is required.
            bool update = (mapsInFolder.Count != currentMaps.Count);

            if (!update)
            {
                foreach (MapFile map1 in mapsInFolder)
                {
                    bool found = false;
                    foreach (MapFile map2 in currentMaps) if (!SameMap(map1, map2)) found = true;
                    update = !found;
                }
            }

            if (update)
            {
                // Update map list and refresh listbox content.
                currentMaps = mapsInFolder;
                MapsListBox.Items.Clear();
                foreach (MapFile mapFile in currentMaps) MapsListBox.Items.Add(mapFile);

                // MapsListBox.Invalidate();
            }
        }

        private List<MapFile> Maps
        {
            get
            {
                List<MapFile> maps = new List<MapFile>();

                string mapsLocation = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Revival\Overload";

                string[] list = Directory.GetFiles(mapsLocation, "*.zip*");
                foreach (string mapFileName in list)
                {
                    if (mapFileName.EndsWith(HiddenMarker) || mapFileName.ToLower().EndsWith(".zip")) maps.Add(new MapFile(mapFileName));
                }
                
                if (Directory.Exists(Path.GetDirectoryName(OverloadExecutable.Text)) && UseDLCLocationCheckBox.Checked)
                {
                    string dlcLocation = Path.Combine(Path.GetDirectoryName(OverloadExecutable.Text), "DLC");
                    Directory.CreateDirectory(dlcLocation);

                    list = Directory.GetFiles(dlcLocation, "*.zip*");
                    foreach (string mapFileName in list)
                    {
                        if (mapFileName.EndsWith(HiddenMarker) || mapFileName.ToLower().EndsWith(".zip")) maps.Add(new MapFile(mapFileName));
                    }
                }

                return maps;
            }
        }

        private void MapOnlyExisting_CheckedChanged(object sender, EventArgs e)
        {
            AutoUpdateMaps = MapOnlyExisting.Checked;
        }

        private void MapDelete_Click(object sender, EventArgs e)
        {
            MapFile mapFile = (MapFile)MapsListBox.Items[MapsListBox.SelectedIndex];

            if (MessageBox.Show(String.Format($"Delete map '{mapFile.Name}' from disk?"), "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lock (mapChangeLock)
                {
                    try
                    {
                        MapsListBox.Items.Remove(mapFile);
                        File.Delete(mapFile.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format($"Whoops! Cannot delete map '{mapFile.Name}': {ex.Message}"));
                    }

                    MapHideButton.Enabled = (MapsListBox.SelectedIndex >= 0);
                    MapDeleteButton.Enabled = (MapsListBox.SelectedIndex >= 0);
                }
            }
        }

        private void MapsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (mapChangeLock)
            {
                if (MapsListBox.SelectedIndex >= 0)
                {
                    MapFile mapFile = (MapFile)MapsListBox.Items[MapsListBox.SelectedIndex];
                    MapDeleteButton.Enabled = true;
                    MapHideButton.Text = (mapFile.Hidden) ? "Unhide" : "Hide";
                    MapHideButton.Enabled = true;
                }
                else
                {
                    MapDeleteButton.Enabled = false;
                    MapHideButton.Text = "Hide";
                    MapHideButton.Enabled = false;
                }
            }
        }

        private void MapHideButton_Click(object sender, EventArgs e)
        {
            lock (mapChangeLock)
            {
                // Make sure that we got a selected map.
                if (MapsListBox.SelectedIndex >= 0)
                {
                    MapFile mapFile = (MapFile)MapsListBox.Items[MapsListBox.SelectedIndex];
                    string hideUnhide = (mapFile.Hidden) ? "unhide" : "hide";
                    try
                    {
                        if (mapFile.Hidden) mapFile.Unhide();
                        else mapFile.Hide();

                        MapsListBox.Items[MapsListBox.SelectedIndex] = mapFile;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format($"Whoops! Cannot {hideUnhide} map {mapFile.Name}: {ex.Message}"));
                    }
                }
            }
        }

        private void MapUpdateButton_Click(object sender, EventArgs e)
        {
            MapUpdateButton.Enabled = false;

            // Start updating maps in a separate thread.
            mapManagerThread = new Thread(UpdateMapThread);
            mapManagerThread.IsBackground = true;
            mapManagerThread.Start();
        }

        /// <summary>
        /// Move maps from either of the two possible directorys.
        /// </summary>
        /// <param name="overloadMapLocation"></param>
        /// <param name="dlcLocation"></param>
        private void MoveMaps(string source, string destination)
        {
            string[] files = Directory.GetFiles(source, "*.zip");
            foreach (string fileName in files)
            {
                // Exclude DLC content (only move maps).
                bool move = true;
                string test = Path.GetFileNameWithoutExtension(fileName).ToUpper();
                if (!fileName.ToLower().EndsWith(".zip") || (test.Contains("DLC0") || test.Contains("DLC1"))) move = false;

                if (move) System.IO.File.Move(Path.Combine(source, Path.GetFileName(fileName)), Path.Combine(destination, Path.GetFileName(fileName)));
            }
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
                        Verbose(String.Format("Overload DLC directory used for maps."));
                        break;

                    default:
                        // TO-DO: Move existing maps.
                        UseDLCLocationCheckBox.Checked = true;
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
                        break;

                    default:
                        Verbose(String.Format("Overload ProgramData directory used for maps."));
                        MoveMaps(dlcLocation, overloadMapLocation);
                        UseDLCLocationCheckBox.Checked = false;
                        break;
                }
            }
        }
    }
}