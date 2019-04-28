using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
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

            // Check for new map file.
            if (File.Exists(mapZipFilePath))
            {
                // Map found - compare date and size to online version.
                FileInfo fi = new FileInfo(mapZipFilePath);
                if ((fi.Length == map.Size) || (fi.LastWriteTime == map.DateTime)) return false;
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
}