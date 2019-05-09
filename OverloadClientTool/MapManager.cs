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
using System.Web;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;

namespace OverloadClientTool
{
    public class OverloadMap
    {
        private const string HiddenMarker = "_OCT_Hidden";

        public string Url;                          // Null if the map ZIP only exist locally.
        public DateTime DateTime;                   // DateTime of last online map ZIP update as defined in the online JSON map list.
        public int Size = 0;                        // Size of ZIP file (on disk or as defined in the online JSON map list).
        public string ZipName = null;               // ZIP file name (excluding path / URL).
        public string LocalZipFileName = null;      // Null if the file is listed online but not found locally (may or may not end with HiddenMarker).
        
        public OverloadMap(string url, DateTime dateTime, int size, string mapZipName)
        {
            this.Url = url;
            this.DateTime = dateTime;
            this.Size = size;

            mapZipName = HttpUtility.UrlDecode(mapZipName);

            // If Url is null then get file name only of mapZipName.
            // Otherwise mapZipName must be online (and without any other URL components but the file name).
            if (String.IsNullOrEmpty(this.Url))
            {
                this.LocalZipFileName = mapZipName;
                this.ZipName = Path.GetFileName(mapZipName).Replace(HiddenMarker, ""); 
            }
            else
            {
                this.ZipName = mapZipName;
            }

            // Unescape ZIP file name.

        }

        public bool InDLC
        {
            get { return !String.IsNullOrEmpty(LocalZipFileName) && LocalZipFileName.Contains(Path.PathSeparator + "DLC" + Path.PathSeparator); }
        }

        public bool Hidden
        {
            get { return !String.IsNullOrEmpty(LocalZipFileName) && LocalZipFileName.EndsWith(HiddenMarker); }
        }

        /// <summary>
        /// Map ZIP file is local if a local path is defined.
        /// </summary>
        public bool IsLocal
        {
            get { return !String.IsNullOrEmpty(LocalZipFileName); }
        }

        /// <summary>
        /// Map ZIP file is online if we have an URL.
        /// </summary>
        public bool IsOnline
        {
            get { return !String.IsNullOrEmpty(Url); }
        }

        public bool CanBeUpdated
        {
            get { return IsOnline; }
        }

        public void Hide()
        {
            if (Hidden) return;

            try
            {
                string newLocalZipFileName = LocalZipFileName + HiddenMarker;
                File.Move(LocalZipFileName, newLocalZipFileName);
                LocalZipFileName = newLocalZipFileName;
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
                string newLocalZipFileName = LocalZipFileName.Replace(HiddenMarker, "");
                File.Move(LocalZipFileName, newLocalZipFileName);
                LocalZipFileName = newLocalZipFileName;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            string result = ZipName;

            // TO-DO: Se om result ender med HIddenMarker (burde ikke ske).

            if (result.ToLower().EndsWith(".zip")) result = result.Substring(0, result.Length - ".zip".Length);

            result += (Hidden) ? " (Hidden)" : "";
            //result += (IsOnline) ? " (Online)" : "";
            //result += (IsLocal) ? " (Local)" : "";

            return result.Trim();
        }

        public string ToolTip
        {
            get
            {
                string result = String.Format($"{ZipName}");

                if (!String.IsNullOrEmpty(Url)) result += String.Format($" , {Size / 1024} KB");

                if (InDLC) result += String.Format($", Overload DLC folder");
                else result += String.Format($", Overload application data folder");

                return result.Trim();
            }
        }

        /// <summary>
        /// Returns TRUE if this map has the same ZIP map filename as 'compareToMap' 
        /// Doesn't matter if either map is hidden or not.
        /// </summary>
        /// <param name="compareZipName"></param>
        /// <returns></returns>
        public bool SameZipFileName(OverloadMap compareToMap)
        {
            return (compareToMap.ZipName.Replace(HiddenMarker, "").ToLower() == this.ZipName.ToLower().Replace(HiddenMarker, ""));
        }

        /// <summary>
        /// Returns true is this map ZipName has the same ZIP map filename as 'compareToZipName'.
        /// </summary>
        /// <param name="compareToZipName"></param>
        /// <returns></returns>
        public bool SameZipFileName(string compareToZipName)
        {
            return (compareToZipName.Replace(HiddenMarker, "").ToLower() == this.ZipName.ToLower().Replace(HiddenMarker, ""));
        }
    }

    public class OverloadMapManager
    {
        // URL for online map list JSON.
        private readonly string DefaultOnlineMapListUrl = @"https://www.overloadmaps.com/data/mp.json";

        // List of maps found online.
        public SortedList<string, OverloadMap> Maps = new SortedList<string, OverloadMap>();

        // Note! Must be same string as defined in OCTMain!
        private const string HiddenMarker = "_OCT_Hidden";

        // Default (and recommended) location for local maps.
        private string applicationMapFolder = null;

        // Optional local map location is Overload DLC (not recommended but used by some people so also supported).
        private string dlcMapFolder = null;

        // Delegate for sending log messages to main application.
        public delegate void LogMessageDelegate(string message);
        private LogMessageDelegate logger = null;
        private LogMessageDelegate loggerError = null;

        // These are updated after each map update check.
        public int Checked = 0;
        public int Created = 0;
        public int Updated = 0;
        public int Errors = 0;

        // Parent form.
        private OCTMain parent;

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
            parent.UIThread(delegate
            {
                logger?.Invoke(s);
            });
        }

        // Log an error message.
        void LogErrorMessage(string s)
        {
            LogMessage(s);
            Debug.WriteLine(s);
        }

        // Prevent default constructor.
        private OverloadMapManager() { }

        public OverloadMapManager(OCTMain parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Updates both online maps available for download as well as local maps.
        /// </summary>
        /// <param name="mapListUrl">URL to online JSON master map list.</param>
        /// <param name="dlcMaps">Path to local DLC folder (or null).</param>
        /// <param name="applicationMaps">Path to local application folder (or null).</param>
        public void UpdateMapList(string mapListUrl = null, string dlcMaps = null, string applicationMaps = null)
        {
            // Check for override of default URL to JSON map list.
            if (String.IsNullOrEmpty(mapListUrl)) mapListUrl = DefaultOnlineMapListUrl;

            // Check DLC/application directory names.
            if (!String.IsNullOrEmpty(dlcMaps))
            {
                dlcMapFolder = dlcMaps;

                if (!OverloadClientApplication.ValidDirectoryName(dlcMapFolder))
                {
                    LogErrorMessage("DLC directory name is invalid!");
                    return;
                }
            }

            if (!String.IsNullOrEmpty(applicationMaps))
            {
                applicationMapFolder = applicationMaps;

                if (!OverloadClientApplication.ValidDirectoryName(applicationMapFolder))
                {
                    LogErrorMessage("Application data directory name is invalid!");
                    return;
                }
            }

            // Need at least one defined folder to store maps.
            if (String.IsNullOrEmpty(applicationMapFolder) && String.IsNullOrEmpty(dlcMapFolder))
            {
                LogErrorMessage("No application data directory or DLC directory found!");
                return;
            }

            // Collect all new online and local maps.
            SortedList<string, OverloadMap> newMapList = new SortedList<String, OverloadMap>();

            // First find all online maps.
            try
            {
                // Get URL base so we can construct full URLs to the online map ZIP files.
                Uri uri = new Uri(mapListUrl);
                string baseUrl = String.Concat(uri.Scheme, Uri.SchemeDelimiter, uri.Host);

                // Get map list and build internal map master list.
                string jsonMapList = new WebClient().DownloadString(mapListUrl);
                dynamic mapList = JsonConvert.DeserializeObject(jsonMapList);
                foreach (var map in mapList)
                {
                    // Check to make sure it is a ZIP file before adding it to the list.
                    string mapUrl = baseUrl + map.url;
                    Uri mapUri = new Uri(mapUrl);
                    if (mapUri.Segments.Length > 2)
                    {
                        // Get the ZIP file name from URL.
                        string mapZipName = mapUri.Segments[mapUri.Segments.Length - 1];
                        if (mapZipName.ToLower().EndsWith(".zip"))
                        {
                            OverloadMap newMap = new OverloadMap(baseUrl + map.url, UnixTimeStampToDateTime(Convert.ToDouble(map.mtime)), Convert.ToInt32(map.size), mapZipName);
                            newMapList.Add(mapZipName.ToLower(), newMap);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorMessage(String.Format($"Unable to get online map list: {ex.Message}"));
            }

            // Now add any local maps or update info if existing local map file exists.
            if (OverloadClientApplication.ValidDirectoryName(applicationMapFolder))
            {
                try
                {
                    Directory.CreateDirectory(applicationMapFolder);

                    string[] list = Directory.GetFiles(applicationMapFolder, "*.zip*");
                    foreach (string mapFileName in list)
                    {
                        if (mapFileName.EndsWith(HiddenMarker) || mapFileName.ToLower().EndsWith(".zip"))
                        {
                            string mapKey = Path.GetFileName(mapFileName).ToLower();

                            FileInfo fiLocalMap = new FileInfo(mapFileName);
                            OverloadMap newMap = new OverloadMap(null, UnixTimeStampToDateTime(0), Convert.ToInt32(fiLocalMap.Length), mapFileName);

                            bool found = false;
                            foreach (KeyValuePair<string, OverloadMap> map in newMapList)
                            {
                                if (newMap.SameZipFileName(map.Value))
                                {
                                    // We found a local map that matches the ZIP filename as found online.
                                    found = true;
                                    map.Value.LocalZipFileName = fiLocalMap.FullName;
                                    map.Value.ZipName = fiLocalMap.Name.Replace(HiddenMarker, "");
                                }

                            }

                            if (!found) newMapList.Add(mapFileName.ToLower(), newMap);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogErrorMessage(String.Format($"Unable to get local maps: {ex.Message}"));
                }
            }

            if (OverloadClientApplication.ValidDirectoryName(dlcMapFolder))
            {
                try
                {
                    Directory.CreateDirectory(dlcMapFolder);

                    string[] list = Directory.GetFiles(dlcMapFolder, "*.zip*");
                    foreach (string mapFileName in list)
                    {
                        if (mapFileName.EndsWith(HiddenMarker) || mapFileName.ToLower().EndsWith(".zip"))
                        {
                            string mapKey = Path.GetFileName(mapFileName).ToLower();
                            FileInfo fiLocalMap = new FileInfo(mapFileName);
                            OverloadMap newMap = new OverloadMap(null, UnixTimeStampToDateTime(0), Convert.ToInt32(fiLocalMap.Length), mapFileName);

                            bool found = false;
                            foreach (KeyValuePair<string, OverloadMap> map in newMapList)
                            {
                                if (newMap.SameZipFileName(map.Value))
                                {
                                    // We found a local map that matches the ZIP filename as found online.
                                    found = true;
                                    map.Value.LocalZipFileName = fiLocalMap.FullName;
                                    map.Value.ZipName = fiLocalMap.Name.Replace(HiddenMarker, "");
                                }
                            }

                            if (!found) newMapList.Add(mapFileName.ToLower(), newMap);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogErrorMessage(String.Format($"Unable to get local maps: {ex.Message}"));
                }
            }

            // Update maps.
            Maps = newMapList;
        }

        /// <summary>
        /// Get online master map list.
        /// </summary>
        /// <param name="mapListUrl">URL for online JSON list.</param>
        /// <param name="dlcMaps">Path to local Overload DLC directory.</param>
        /// <param name="applicationDataMaps">Path to local Overload application data directory.</param>
        /// <returns></returns>
        public bool UpdateAllMaps(string mapListUrl = null, string dlcMaps = null, string applicationDataMaps = null)
        {
            Checked = 0;
            Created = 0;
            Updated = 0;
            Errors = 0;

            // Get the online master map list.
            UpdateMapList(mapListUrl, dlcMaps, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Revival\Overload");

            // Should never happen but the online list could be offline/empty for some reason.
            if (Maps.Count == 0) return false;

            try
            {
                List<Task> downloadTasks = new List<Task>();

                foreach (KeyValuePair<string, OverloadMap> sortedMmap in Maps)
                {
                    var task = UpdateMap(sortedMmap.Value);
                    downloadTasks.Add(task);
                    try { task.Start(); } catch { }
                }

                // Wait for all downloads to complete before allowing further actions.
                Task.WaitAll(downloadTasks.ToArray());
            }
            catch (Exception ex)
            {
                LogErrorMessage(String.Format($"Unable to update maps: {ex.Message}"));
            }

            return (Errors != 0) ? false : true;
        }

        /// <summary>
        /// Update a single Overload map.
        /// </summary>
        /// <param name="map">The map to update.</param>
        /// <returns></returns>
        public async Task<bool> UpdateMap(OverloadMap map, bool forceUpdate = false)
        {
            Checked++;

            if (String.IsNullOrEmpty(map.Url)) return false;

            string mapZipName = map.ZipName;
            string mapDirectoryPath = String.IsNullOrEmpty(dlcMapFolder) ? applicationMapFolder : dlcMapFolder;
            string mapZipFilePath = WebUtility.UrlDecode(Path.Combine(mapDirectoryPath, mapZipName));
            string mapZipDisplayName = WebUtility.UrlDecode(mapZipName).Trim();

            // Create (DLC or application data) directory if it doesn't exist.
            if (!Directory.Exists(mapDirectoryPath)) Directory.CreateDirectory(mapDirectoryPath);

            // Don't update hidden maps.
            if (File.Exists(mapZipFilePath + HiddenMarker)) return false;

            // Check for new map file.
            if (!forceUpdate && OverloadClientApplication.ValidFileName(mapZipFilePath))
            {
                if (File.Exists(mapZipFilePath))
                {
                    // Map already downloaded. Compare date and size against online version.
                    FileInfo fi = new FileInfo(mapZipFilePath);
                    if ((fi.Length == map.Size) && (fi.LastWriteTime == map.DateTime)) return false;
                }
                else
                {
                    // Only update existing maps?
                    if (parent.UpdateOnlyExistingMaps) return false;
                }
            }

            try
            {
                bool existingFile = File.Exists(mapZipFilePath);

                // Download map.
                DownloadResult result = await DownloadMapFile(map, new Uri(map.Url), mapZipFilePath, mapZipDisplayName);

                if (result.Exception != null) throw result.Exception;
                if (result.Succes == false)
                {
                    if (String.IsNullOrEmpty(result.Message)) result.Message = String.Format($"Unable to download {mapZipDisplayName}");
                    try { File.Delete(mapZipFilePath); } catch { }
                    throw new Exception(String.Format($"Unable to download {result.Message }"));
                }

                // LogMessage(String.Format($"Downloading map {mapZipDisplayName}."));

                if (existingFile) Updated++;
                else Created++;

                return true;
            }
            catch (Exception ex)
            {
                Errors++;

                LogErrorMessage(String.Format($"Error downloading {mapZipDisplayName}: {ex.Message}"));
                return false;
            }        
        }

        internal class DownloadResult
        {
            public string Message = "";
            public Exception Exception = null;
            public bool Succes = false;

            public DownloadResult(string message = null, Exception exception = null)
            {
                this.Message = message;
                this.Exception = exception;
            }
        }

        /// <summary>
        /// Asynchronously download a map.
        /// </summary>
        /// <param name="map">The map to download</param>
        /// <param name="url">URL of the online map ZIP</param>
        /// <param name="filePath">Path to local file name</param>
        /// <param name="displayName">Friendly display name</param>
        /// <returns>DownloadResult detailing outcome of the download</returns>
        internal Task<DownloadResult> DownloadMapFile(OverloadMap map, Uri url, string filePath, string displayName)
        {
            DownloadResult result = new DownloadResult();

            var tcs = new TaskCompletionSource<DownloadResult>();

            Task.Run(async () =>
            {
                bool hasProgresChanged = false;
                var timer = new System.Timers.Timer(new TimeSpan(0, 0, 30).TotalMilliseconds);
                var client = new WebClient();

                void downloadHandler(object s, DownloadProgressChangedEventArgs e) => hasProgresChanged = true;

                void timerHandler(object s, ElapsedEventArgs e)
                {
                    timer.Stop();

                    if (hasProgresChanged)
                    {
                        timer.Start();
                        hasProgresChanged = false;
                    }
                    else
                    {
                        result.Message = String.Format($"Timeout downloading {displayName}");

                        CleanUp();
                        tcs.TrySetException(new TimeoutException("Download timed out"));
                    }
                }

                void CleanUp()
                {
                    client.DownloadProgressChanged -= downloadHandler;
                    client.Dispose();
                    timer.Elapsed -= timerHandler;
                    timer.Dispose();
                }

                try
                {
                    client.DownloadProgressChanged += downloadHandler;
                    timer.Elapsed += timerHandler;
                    timer.Start();

                    LogMessage(String.Format($"Downloading map {displayName}."));

                    await client.DownloadFileTaskAsync(url, filePath);

                    LogMessage(String.Format($"{displayName} download complete."));

                    map.LocalZipFileName = filePath;
                    map.ZipName = Path.GetFileName(filePath);

                    // Set local files date and time.
                    File.SetCreationTime(filePath, map.DateTime);
                    File.SetLastWriteTime(filePath, map.DateTime);

                    result.Succes = true;
                }
                catch (Exception ex)
                {
                    result.Message = String.Format($"Error downloading {displayName}", ex);

                    tcs.TrySetException(ex);
                }
                finally
                {
                    CleanUp();
                }

                return tcs.TrySetResult(result);
            });

            return tcs.Task;
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
        // BackgroundWorker mapsBackgroundWorker = null;

        private object mapChangeLock = new object();

        private static bool SameMap(OverloadMap map1, OverloadMap map2)
        {
            return (map1.InDLC == map2.InDLC) && (map1.ZipName.ToLower() == map2.ZipName.ToLower());
        }

        private void UpdateMapList()
        {
            string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Revival");
            appPath = Path.Combine(appPath, "Overload");

            // Construct the paths to where local maps can be stored.
            string dlcFolder = (UseDLCLocation && OverloadClientApplication.ValidDirectoryName(OverloadPath)) ? Path.Combine(OverloadPath, "DLC") : null;
            string appFolder = OverloadClientApplication.ValidDirectoryName(appPath) ? appPath : null;

            mapManager.UpdateMapList(MapListUrl, dlcFolder, appFolder);
        }

        private void InitMapsListBox()
        {
            LogDebugMessage("InitMapsListBox()");

            UpdateMapList();
            UpdateListBox();
            SetMapButtons();

            // Begin monitoring folder.
            // mapsBackgroundWorker = new BackgroundWorker();
            // mapsBackgroundWorker.DoWork += BackgroundMapsChecker;
            // mapsBackgroundWorker.RunWorkerAsync();
        }

        private void SetMapButtons()
        {
            if (MapsListBox.SelectedIndex >= 0)
            {
                OverloadMap map = ((KeyValuePair<string, OverloadMap>)MapsListBox.Items[MapsListBox.SelectedIndex]).Value;

                MapHideButton.Text = (map.Hidden) ? "Unhide" : "Hide";

                MapHideButton.Enabled = map.IsLocal;
                MapDeleteButton.Enabled = map.IsLocal;
                MapHideButton.Enabled = map.IsLocal;
                MapRefreshButton.Enabled = map.IsOnline;
            }
            else
            {
                MapDeleteButton.Enabled = false;
                MapHideButton.Text = "Hide";
                MapHideButton.Enabled = false;
                MapRefreshButton.Enabled = false;
            }
        }

        #region MaybeImplementLater
        /****
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
                Thread.Sleep(250);

                lock (mapChangeLock)
                {
                    this.UIThread(delegate
                    {
                        CheckAndUpdateMaplist();

                        MapHideButton.Enabled = (MapsListBox.SelectedIndex >= 0);
                        MapDeleteButton.Enabled = (MapsListBox.SelectedIndex >= 0);
                        MapRefreshButton.Enabled = (MapsListBox.SelectedIndex >= 0);
                    });
                }
            }
        }

        private void CheckAndUpdateMaplist()
        {
            List<MapFile> mapsInFolder = LocalMapFiles;

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

                MapHideButton.Enabled = (MapsListBox.SelectedIndex >= 0);
                MapDeleteButton.Enabled = (MapsListBox.SelectedIndex >= 0);
            }
        }
        ******/
        #endregion

        private void MapDelete_Click(object sender, EventArgs e)
        {
            if (MapsListBox.SelectedIndex >= 0)
            {
                OverloadMap map = ((KeyValuePair<string, OverloadMap>)MapsListBox.Items[MapsListBox.SelectedIndex]).Value;
                if (!map.IsLocal) return;

                if (MessageBox.Show(String.Format($"Delete map '{map.ZipName}' from disk?"), "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    lock (mapChangeLock)
                    {
                        try
                        {
                            MapsListBox.Items.Remove(map);
                            File.Delete(map.LocalZipFileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(String.Format($"Whoops! Cannot delete map '{map.ZipName}': {ex.Message}"));
                        }

                        UpdateMapList();
                        UpdateListBox();
                    }
                }
            }
        }

        private void MapsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (mapChangeLock) SetMapButtons();
        }

        private void MapRefresh_Click(object sender, EventArgs e)
        {
            OverloadMap map = ((KeyValuePair<string, OverloadMap>)MapsListBox.Items[MapsListBox.SelectedIndex]).Value;

            if (map.Hidden)
            {
                MessageBox.Show("Cannot refresh a hidden map!", "Map is hidden");
                return;
            }

            mapManager.UpdateMap(map, true);
            UpdateListBox();
        }

        private void MapHideButton_Click(object sender, EventArgs e)
        {
            OverloadMap map = ((KeyValuePair<string, OverloadMap>)MapsListBox.Items[MapsListBox.SelectedIndex]).Value;
            if (!map.IsLocal) return;

            try
            {
                if (map.Hidden) map.Unhide();
                else map.Hide();

                UpdateListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format($"Whoops! Cannot {((map.Hidden) ? "unhide" : "hide")} map {map.ZipName}: {ex.Message}"));
                SetMapButtons();
            }
        }

        private void UpdateListBox(string focusName = null)
        {
            MapsListBox.Items.Clear();
            MapsListBox.DisplayMember = "Value";
            MapsListBox.ValueMember = "Value";

            foreach (KeyValuePair<string, OverloadMap> setMap in mapManager.Maps)
            {
                if (setMap.Value.IsLocal) MapsListBox.Items.Add(setMap);
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

            SetMapButtons();
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

                Verbose(String.Format("Overload " + ((UseDLCLocation) ? "DLC" : "application") + " folder used for maps."));
            });

            // UpdateAllMaps() must not touch UI elements!
            mapManager.UpdateAllMaps(OnlineMapJsonUrl.Text);

            this.UIThread(delegate
            {
                if (UpdateOnlyExistingMaps) Verbose(String.Format($"Map check finished: {mapManager.Checked} maps, {mapManager.Updated} updated."));
                else Verbose(String.Format($"Map check finished: {mapManager.Checked} maps checked, {mapManager.Created} created, {mapManager.Updated} updated."));

                UpdateListBox();

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

        private void AutoUpdateMaps_Click(object sender, EventArgs e)
        {
            AutoUpdateMaps = AutoUpdateMapsCheckBox.Checked;
        }

        private void OnlyUpdateExistingMapsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOnlyExistingMaps = OnlyUpdateExistingMapsCheckBox.Checked;
        }

        private void MapsListBox_MouseMove(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = lb.IndexFromPoint(e.Location);

            if (index >= 0 && index < lb.Items.Count)
            {
                OverloadMap map = ((KeyValuePair<string, OverloadMap>)MapsListBox.Items[index]).Value;
                string toolTipString = map.ToolTip;

                // Don't do anything tooltip text is the current tooltip .
                if (MapsToolTip.GetToolTip(lb) != toolTipString) MapsToolTip.SetToolTip(lb, toolTipString);
            }
            else
                MapsToolTip.Hide(lb);
        }
    }
}