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
        private const string jsonOlmodUrl = @"https://api.github.com/repos/arbruijn/olmod/releases/latest";
        private const string jsonZipField = @"browser_download_url";

        // Delegate for sending log messages to main application.
        public delegate void LogMessageDelegate(string message);

        // The loggers must be able to resolve any thread/invoke issues.
        private LogMessageDelegate infoLogger = null;
        private LogMessageDelegate errorLogger = null;

        public OlmodManager()
        {
        }

        public OlmodManager(LogMessageDelegate logger = null) : base()
        {
            if (logger != null)
            {
                this.infoLogger = logger;
                this.errorLogger = logger;
            }
        }

        public OlmodManager(LogMessageDelegate infoLogger = null, LogMessageDelegate errorLogger = null) : base()
        {
            if (infoLogger != null) this.infoLogger = infoLogger;
            if (errorLogger != null) this.errorLogger = errorLogger;
        }

        /// <summary>
        /// Send log message to parent. If parent is an UI thread then it must handle any 'Invoke' issues.
        /// </summary>
        /// <param name="s"></param>
        void LogMessage(string s)
        {
            infoLogger?.Invoke(s);
        }

        // Set delegate for logging.
        public void SetLogger(LogMessageDelegate logger = null, LogMessageDelegate errorLogger = null)
        {
            this.infoLogger = logger;
            if (this.errorLogger == null)
            {
                if (errorLogger == null) this.errorLogger = logger;
                else this.errorLogger = errorLogger;
            }
        }

        // Log an error message.
        void LogErrorMessage(string s)
        {
            LogMessage(s);
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

                    string zipUrl = olmodReleaseInfo.assets[0].browser_download_url;
                    long size = Convert.ToInt64(olmodReleaseInfo.assets[0].size);
                    DateTime created = Convert.ToDateTime(olmodReleaseInfo.assets[0].created_at, CultureInfo.InvariantCulture);
                    string version = olmodReleaseInfo.tag_name;

                    OlmodRelease release = new OlmodRelease();
                    release.DownloadUrl = zipUrl;
                    release.Size = size;
                    release.Created = created;
                    release.Version = version;

                    return release;

                }
                catch (Exception ex)
                {
                    LogErrorMessage(String.Format($"Cannot get Olmod JSON release info: {ex.Message}"));
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

                LogMessage(String.Format($"Olmod has been updated to release {release.Version}"));
            }
            catch (Exception ex)
            {
                LogErrorMessage(String.Format($"Cannot download Olmod ZIP file from Github: {ex.Message}"));
            }
            finally
            {
                if (OverloadClientApplication.ValidFileName(localTempZip, true)) try { File.Delete(localTempZip); } catch { }
                if (OverloadClientApplication.ValidDirectoryName(localTempFolder, true)) try {OverloadClientApplication.RemoveDirectory(localTempFolder); } catch { }
            }
        }
    }
}
