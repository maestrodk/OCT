using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OverloadClientTool
{
    public class OlmodManager
    {
        private const string jsonOlmodUrl = @"https://api.github.com/repos/arbruijn/olmod/releases";
        private const string jsonZipField = @"browser_download_url";

        // Delegate for sending log messages to main application.
        public delegate void LogMessageDelegate(string message);

        // The loggers must be able to resolve any thread/invoke issues.
        private LogMessageDelegate infoLogger = null;
        private LogMessageDelegate verboseLogger = null;

        private OlmodManager()
        {
        }

        public OlmodManager(LogMessageDelegate infoLogger = null, LogMessageDelegate verboseLogger = null)
        {
            this.infoLogger = infoLogger;
            this.verboseLogger = verboseLogger;
        }

        /// <summary>
        /// Send log message to parent. If parent is an UI thread then it must handle any 'Invoke' issues.
        /// </summary>
        /// <param name="s"></param>
        void LogMessage(string s)
        {
            infoLogger?.Invoke(s);
        }

        // Log an error message.
        void LogVerboseMessage(string s)
        {
            if (verboseLogger != null) verboseLogger?.Invoke(s);
            else LogMessage(s);

            Debug.WriteLine(s);
        }

        public class OlmodRelease
        {
            public string DownloadUrl { get; set; }
            public long Size { get; set; }
            public DateTime Created { get; set; }
            public string Version { get; set; }
        }

        public OlmodRelease GetLastestRelease
        {
            get
            {
                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => { return true; };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string json = "";

                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "Overload Client Tool - user " + WindowsIdentity.GetCurrent().Name);
                        json = wc.DownloadString(jsonOlmodUrl);
                    }

                    dynamic olmodReleaseInfo = JsonConvert.DeserializeObject(json);

                    // Find latest non-prerelease version.
                    int i = 0;
                    int pub = -1;
                    int pre  = -1;
                    while (i < olmodReleaseInfo.Count) 
                    {
                        if ((pre < 0) && (olmodReleaseInfo[i].prerelease == true)) pre = i;
                        if ((pub < 0) && (olmodReleaseInfo[i].prerelease == false)) pub = i;
                        i++;
                    }

                    if (pub < 0)
                    {
                        LogVerboseMessage($"Cannot get Olmod JSON release info.");
                        return null;
                    }

                    string zipUrl = olmodReleaseInfo[pub].assets[0].browser_download_url;
                    long size = Convert.ToInt64(olmodReleaseInfo[pub].assets[0].size);
                    DateTime created = Convert.ToDateTime(olmodReleaseInfo[pub].assets[0].created_at, CultureInfo.InvariantCulture);
                    string version = olmodReleaseInfo[pub].tag_name;

                    OlmodRelease release = new OlmodRelease();
                    release.DownloadUrl = zipUrl;
                    release.Size = size;
                    release.Created = created;
                    release.Version = version;

                    return release;

                }
                catch (Exception ex)
                {
                    LogVerboseMessage($"Cannot get Olmod JSON release info: {ex.Message}");
                }

                return null;
            }
        }

        public void DownloadAndInstallOlmod(OlmodRelease release, string localInstallFolder)
        {
            string localTempZip = Path.GetTempFileName() + "_OCT_Olmod_Update.zip";
            string localTempFolder = Path.GetTempFileName() + "_OCT_Olmod_Update";
            Directory.CreateDirectory(localTempFolder);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "Overload Client Tool - user " + WindowsIdentity.GetCurrent().Name);
                    wc.DownloadFile(release.DownloadUrl, localTempZip);
                }

                ZipFile.ExtractToDirectory(localTempZip, localTempFolder);

                // Copy all files to destination, overwriting any existing files.
                DirectoryInfo dir = new DirectoryInfo(localTempFolder);
                foreach (FileInfo fi in dir.GetFiles()) File.Copy(fi.FullName, Path.Combine(localInstallFolder, fi.Name), true);

                // Time stamp olmod.exe.
                File.SetCreationTime(Path.Combine(localInstallFolder, "olmod.exe"), release.Created);
                File.SetLastWriteTime(Path.Combine(localInstallFolder, "olmod.exe"), release.Created);

                LogVerboseMessage($"Olmod has been updated.");
            }
            catch (Exception ex)
            {
                LogVerboseMessage($"Cannot download/install Olmod: {ex.Message}");
            }
            finally
            {
                if (OverloadClientToolApplication.ValidFileName(localTempZip, true)) try { File.Delete(localTempZip); } catch { }
                if (OverloadClientToolApplication.ValidDirectoryName(localTempFolder, true)) try {OverloadClientToolApplication.RemoveDirectory(localTempFolder); } catch { }
            }
        }
    }
}
