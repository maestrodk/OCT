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
using System.IO.Compression;

namespace OverloadClientTool
{
    /// <summary>
    /// Overload map definition.
    /// </summary>
    public class OverloadMap
    {
        private const string MapHiddenMarker = "_OCT_Hidden";

        public string Url;                          // Null if the map ZIP only exist locally.
        public DateTime DateTime;                   // DateTime of last online map ZIP update as defined in the online JSON map list.
        public int Size = 0;                        // Size of ZIP file (on disk or as defined in the online JSON map list).
        public string ZipName = null;               // ZIP file name (excluding path / URL).

        public string LocalZipFileName = null;      // Full path to ZIP file if map exists locally in the Overload application data folder.
        public string LocalDLCZipFileName = null;   // Full path to ZIP file if map exists locally in the Overload application data folder.

        public OverloadMap(string url, DateTime dateTime, int size, string mapZipName)
        {
            this.Url = url;
            this.DateTime = dateTime;
            this.Size = size;

            // Decode URL to create a displayable map ZIP name.
            mapZipName = HttpUtility.UrlDecode(mapZipName);

            // If Url is null then get file name only of mapZipName.
            // Otherwise mapZipName must be online and the name is alread stripped from other URL path segments.
            if (String.IsNullOrEmpty(this.Url))
            {
                if (mapZipName.Contains(Path.DirectorySeparatorChar + "DLC" + Path.DirectorySeparatorChar)) this.LocalDLCZipFileName = mapZipName;
                else this.LocalZipFileName = mapZipName;

                this.ZipName = Path.GetFileName(mapZipName).Replace(MapHiddenMarker, "");
            }
            else
            {
                this.ZipName = mapZipName;
            }
        }

        /// <summary>
        /// Is map local and found in Overload DLC folder?
        /// </summary>
        public bool InDLCFolder
        {
            get { return !String.IsNullOrEmpty(LocalDLCZipFileName); }
        }

        /// <summary>
        /// Is map local and found in Overload DLC folder?
        /// </summary>
        public bool InApplicationDataFolder
        {
            get { return !String.IsNullOrEmpty(LocalZipFileName); }
        }

        /// <summary>
        /// Is map hidden (ends with hidden marker)?
        /// </summary>
        public bool Hidden
        {
            get
            {
                if (!String.IsNullOrEmpty(LocalZipFileName)) return LocalZipFileName.EndsWith(MapHiddenMarker);
                if (!String.IsNullOrEmpty(LocalDLCZipFileName)) return LocalDLCZipFileName.EndsWith(MapHiddenMarker);

                // In case a local file does not exists...
                return false;
            }

            set
            {
                if (value == true) Hide();
                else Unhide();
            }
        }

        /// <summary>
        /// Determines if map exists locally.
        /// </summary>
        public bool IsLocal
        {
            get { return InDLCFolder || InApplicationDataFolder; }
        }

        /// <summary>
        /// Map ZIP file is online if we have an URL.
        /// </summary>
        public bool IsOnline
        {
            get { return !String.IsNullOrEmpty(Url); }
        }

        public void Hide()
        {
            try
            {
                // The map ZIP file may exist in both the application data folder and the DLC folder.
                // Ensure hidden state is set for both!

                if (OverloadClientToolApplication.ValidFileName(LocalZipFileName, true) && !LocalZipFileName.EndsWith(MapHiddenMarker))
                {
                    string newLocalZipFileName = LocalZipFileName + MapHiddenMarker;
                    File.Move(LocalZipFileName, newLocalZipFileName);
                    LocalZipFileName = newLocalZipFileName;
                }

                if (OverloadClientToolApplication.ValidFileName(LocalDLCZipFileName, true) && !LocalDLCZipFileName.EndsWith(MapHiddenMarker))
                {
                    string newLocalDLCZipFileName = LocalDLCZipFileName + MapHiddenMarker;
                    File.Move(LocalDLCZipFileName, newLocalDLCZipFileName);
                    LocalDLCZipFileName = newLocalDLCZipFileName;
                }
            }
            catch
            {
            }
        }

        public void Unhide()
        {
            try
            {
                // The map ZIP file may exist in both the application data folder and the DLC folder.
                // Ensure hidden state is set for both!

                if (OverloadClientToolApplication.ValidFileName(LocalZipFileName, true) && LocalZipFileName.EndsWith(MapHiddenMarker))
                {
                    string newLocalZipFileName = LocalZipFileName.Replace(MapHiddenMarker, "");
                    File.Move(LocalZipFileName, newLocalZipFileName);
                    LocalZipFileName = newLocalZipFileName;
                }

                if (OverloadClientToolApplication.ValidFileName(LocalDLCZipFileName, true) && LocalDLCZipFileName.EndsWith(MapHiddenMarker))
                {
                    string newLocalDLCZipFileName = LocalDLCZipFileName.Replace(MapHiddenMarker, "");
                    File.Move(LocalDLCZipFileName, newLocalDLCZipFileName);
                    LocalDLCZipFileName = newLocalDLCZipFileName;
                }
            }
            catch
            {
            }
        }

        public override string ToString()
        {
            string result = ZipName;

            if (result.ToLower().EndsWith(".zip")) result = result.Substring(0, result.Length - ".zip".Length);

            result += (Hidden) ? " (Hidden)" : "";

            //result += (IsOnline) ? " (Online)" : "";
            //result += (InApplicationDataFolder) ? " (App)" : "";
            //result += (InDLCFolder) ? " (DLC)" : "";

            return result.Trim();
        }

        /// <summary>
        /// On Windows this method is used for a ToolTip display when the mouse i hovering over a specific map.
        /// </summary>
        public string DisplayMapInfo
        {
            get
            {
                string result = String.Format($"{ZipName}");

                if (!String.IsNullOrEmpty(Url))
                {
                    result += String.Format($", {Size / 1024} KB");
                }
                else
                {
                    if (InApplicationDataFolder)
                    {
                        try
                        {
                            long size = new FileInfo(LocalZipFileName).Length;
                            result += String.Format($", {Size / 1024} KB");
                        }
                        catch
                        {
                        }
                    }
                    else if (InDLCFolder)
                    {
                        try
                        {
                            long size = new FileInfo(LocalDLCZipFileName).Length;
                            result += String.Format($", {Size / 1024} KB");
                        }
                        catch
                        {
                        }
                    }
                }

                if (InApplicationDataFolder) result += String.Format($", Application data folder");
                if (InDLCFolder) result += String.Format($", DLC folder");

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
            return compareToMap.ZipName.Replace(MapHiddenMarker, "").ToLower() == this.ZipName.ToLower().Replace(MapHiddenMarker, "");
        }

        public bool SameZipFileName(string zipFileName)
        {
            if (!OverloadClientToolApplication.ValidFileName(zipFileName, true)) return false;
            zipFileName = Path.GetFileName(zipFileName);
            return zipFileName.Replace(MapHiddenMarker, "").ToLower() == this.ZipName.ToLower().Replace(MapHiddenMarker, "");
        }

        /// <summary>
        /// Returns true is this map ZipName has the same ZIP map filename as 'compareToZipName'.
        /// </summary>
        /// <param name="compareToZipName"></param>
        /// <returns></returns>
        public static bool ContainsMultiplayerMap(string zipFileName)
        {
            using (ZipArchive zip = ZipFile.Open(zipFileName, ZipArchiveMode.Read)) foreach (ZipArchiveEntry entry in zip.Entries) if (entry.Name.ToLower().EndsWith(".mp")) return true;
            return false;
        }
    }

    public class OverloadMapManager
    {
        // Set the maximum of map downloads to be run in parallel to avoid timeout errors/overloading the map server.
        private const int MaxSimultanousDownloads = 4;

        // Timeout for download actions in seconds.
        private const int DownloadTimeoutSeconds = 60;

        // URL for online map list JSON.
        private readonly string DefaultOnlineMapListUrl = @"https://www.overloadmaps.com/data/mp.json";

        // List of maps found online.
        public SortedList<string, OverloadMap> Maps = new SortedList<string, OverloadMap>();

        // Note! MUST be same string as defined in OCTMain!
        private const string HiddenMarker = "_OCT_Hidden";

        // Default (and recommended) location for local maps.
        private string applicationMapFolder = null;

        // Optional local map location is Overload DLC (not recommended but used by some people so also supported).
        private string dlcMapFolder = null;
        public bool SaveNewMapsToDLCFolder = false;

        // Delegate for sending log messages to main application.
        public delegate void LogMessageDelegate(string message);

        // The loggers must be able to resolve any thread/invoke issues.
        private LogMessageDelegate logger = null;
        private LogMessageDelegate errorLogger = null;

        // These are updated after each map update check.
        public int Checked = 0;
        public int Created = 0;
        public int Updated = 0;
        public int Errors = 0;

        public bool OnlyUpdateExistingMaps { get; set; }
        public bool HideUnOfficialMaps { get; set; }

        // Set delegate for logging.
        public void SetLogger(LogMessageDelegate logger = null, LogMessageDelegate errorLogger = null)
        {
            this.logger = logger;
            if (this.errorLogger == null)
            {
                if (errorLogger == null) this.errorLogger = logger;
                else this.errorLogger = errorLogger;
            }
        }

        /// <summary>
        /// Send log message to parent. If parent is an UI thread then it must handle any 'Invoke' issues.
        /// </summary>
        /// <param name="s"></param>
        void LogMessage(string s)
        {
            logger?.Invoke(s);
        }

        // Log an error message.
        void LogErrorMessage(string s)
        {
            errorLogger?.Invoke(s);
            Debug.WriteLine(s);
        }

        // Prevent default constructor.
        public OverloadMapManager(bool onlyExistingMaps = false)
        {
            OnlyUpdateExistingMaps = onlyExistingMaps;
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

                if (!OverloadClientToolApplication.ValidDirectoryName(dlcMapFolder))
                {
                    LogErrorMessage("DLC directory name is invalid!");
                    return;
                }
            }

            if (!String.IsNullOrEmpty(applicationMaps))
            {
                applicationMapFolder = applicationMaps;

                if (!OverloadClientToolApplication.ValidDirectoryName(applicationMapFolder))
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
                            // Uppercase first char in map name.
                            mapZipName = mapZipName.Substring(0, 1).ToUpper() + mapZipName.Substring(1);
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
            if (OverloadClientToolApplication.ValidDirectoryName(applicationMapFolder))
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

                                    // Uppercase first char in map name and remove hidden marker if present.
                                    string mapZipName = fiLocalMap.Name;
                                    mapZipName = mapZipName.Substring(0, 1).ToUpper() + mapZipName.Substring(1);
                                    mapZipName = mapZipName.Replace(HiddenMarker, "");
                                    map.Value.ZipName = mapZipName;

                                    // Save full path to ZIP file in the application data folder.
                                    map.Value.LocalZipFileName = fiLocalMap.FullName;
                                }
                            }

                            if (!found)
                            {
                                // See if we should hide MP maps if not included in the official map list.
                                if (HideUnOfficialMaps && OverloadMap.ContainsMultiplayerMap(mapFileName) && !newMap.Hidden)
                                {
                                    string zipName = newMap.ZipName;
                                    if (zipName.ToLower().EndsWith(".zip")) zipName = zipName.Substring(0, zipName.Length - ".zip".Length);
                                    LogMessage($"Hidding {zipName} as it is not in the official MP map list.");
                                    newMap.Hide();
                                }

                                newMapList.Add(mapKey, newMap);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogErrorMessage(String.Format($"Unable to get local maps: {ex.Message}"));
                }
            }

            if (OverloadClientToolApplication.ValidDirectoryName(dlcMapFolder))
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

                                    // Uppercase first char in map name and remove hidden marker if present.
                                    string mapZipName = fiLocalMap.Name;
                                    mapZipName = mapZipName.Substring(0, 1).ToUpper() + mapZipName.Substring(1);
                                    mapZipName = mapZipName.Replace(HiddenMarker, "");
                                    map.Value.ZipName = mapZipName;

                                    // Save full path to ZIP file in the DLC folder.
                                    map.Value.LocalDLCZipFileName = fiLocalMap.FullName;
                                }
                            }

                            if (!found)
                            {
                                // See if we should hide MP maps if not included in the official map list.
                                if (HideUnOfficialMaps && OverloadMap.ContainsMultiplayerMap(mapFileName) && !newMap.Hidden)
                                {
                                    string zipName = newMap.ZipName;
                                    if (zipName.ToLower().EndsWith(".zip")) zipName = zipName.Substring(0, zipName.Length - ".zip".Length);
                                    LogMessage($"Hidding {zipName} as it is not in the official MP map list.");
                                    newMap.Hide();
                                }

                                newMapList.Add(mapKey, newMap);
                            }
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
        public bool UpdateAllMaps(string mapListUrl, string dlcMaps, string applicationDataMaps)
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

                int started = 0;
                foreach (KeyValuePair<string, OverloadMap> sortedMap in Maps)
                {
                    var task = UpdateMap(sortedMap.Value, false);

                    downloadTasks.Add(task);
                    try { task.Start(); } catch { }

                    if (++started > MaxSimultanousDownloads)
                    {
                        Task.WaitAll(downloadTasks.ToArray());
                        downloadTasks = new List<Task>();
                        started = 0;
                    }
                }

                // Wait for all remaining downloads to complete before allowing further actions.
                if (started > 0) Task.WaitAll(downloadTasks.ToArray());
            }
            catch (Exception ex)
            {
                LogErrorMessage(String.Format($"Unable to update maps: {ex.Message}"));
            }
            finally
            {
                foreach (KeyValuePair<string, OverloadMap> sortedMap in Maps)
                {
                    //if (!OverloadClientApplication.ValidFileName(sortedMap.Value.LocalZipFileName, true)) sortedMap.Value.LocalZipFileName = null;
                    //if (!OverloadClientApplication.ValidFileName(sortedMap.Value.LocalDLCZipFileName, true)) sortedMap.Value.LocalDLCZipFileName = null;
                }
            }

            return (Errors != 0) ? false : true;
        }

        /// <summary>
        /// Update a single Overload map.
        /// </summary>
        /// <param name="map">The map to update.</param>
        /// <returns></returns>
        public async Task<bool> UpdateMap(OverloadMap map, bool forceUpdate)
        {
            Checked++;

            if (String.IsNullOrEmpty(map.Url)) return false;

            string mapZipName = map.ZipName;
            string mapDirectoryPath = (String.IsNullOrEmpty(dlcMapFolder) || !SaveNewMapsToDLCFolder) ? applicationMapFolder : dlcMapFolder;
            string mapZipFilePath = WebUtility.UrlDecode(Path.Combine(mapDirectoryPath, mapZipName));
            string mapZipDisplayName = WebUtility.UrlDecode(mapZipName).Trim();

            // Check if we should use existing download path.
            if (map.InDLCFolder)
            {
                mapDirectoryPath = Path.GetDirectoryName(map.LocalDLCZipFileName);
                mapZipFilePath = map.LocalDLCZipFileName;
            }
            else if (map.InApplicationDataFolder)
            {
                mapDirectoryPath = Path.GetDirectoryName(map.LocalZipFileName);
                mapZipFilePath = map.LocalZipFileName;
            }

            // Create (DLC or application data) directory if it doesn't exist.
            if (!Directory.Exists(mapDirectoryPath)) Directory.CreateDirectory(mapDirectoryPath);

            // Don't update hidden maps.
            if (File.Exists(mapZipFilePath + HiddenMarker)) return false;

            // Check for new map file.
            if (!forceUpdate && OverloadClientToolApplication.ValidFileName(mapZipFilePath))
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
                    if (OnlyUpdateExistingMaps) return false;
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
                var timer = new System.Timers.Timer(new TimeSpan(0, 0, DownloadTimeoutSeconds).TotalMilliseconds);
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

                    //LogMessage(String.Format($"Downloading map {displayName}."));

                    await client.DownloadFileTaskAsync(url, filePath);

                    LogMessage(String.Format($"{displayName} download complete."));

                    if (!filePath.Contains(Path.DirectorySeparatorChar + "DLC" + Path.DirectorySeparatorChar)) map.LocalZipFileName = filePath;
                    else map.LocalDLCZipFileName = filePath;

                    map.ZipName = Path.GetFileName(filePath);

                    // Set local files date and time.
                    File.SetCreationTime(filePath, map.DateTime);
                    File.SetLastWriteTime(filePath, map.DateTime);

                    result.Succes = true;
                }
                catch (Exception ex)
                {
                    result.Message = String.Format($"Error downloading {displayName}: {ex.Message}");
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

}