using Newtonsoft.Json;
using OverloadClientApplication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    static class OverloadClientApplication
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Setup debug logging.
            string startDateTime = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss");
            string debugFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OverloadClientTool\\Debug");
            string debugFileName = Path.Combine(debugFileFolder, String.Format($"OCT_Debug_{startDateTime.Replace(":", "").Replace("-", "").Replace(" ", "_")}.txt"));
            try { Directory.CreateDirectory(debugFileFolder); } catch { }

            LogDebugMessage("OCT application startup.", debugFileName);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check for update.
            if (UpdateCheck(debugFileName)) return;

            try
            {
                LogDebugMessage("Starting OCT main UI thread.", debugFileName);
                Application.Run(new OCTMain(args, debugFileName));
                LogDebugMessage("OCT main exit - shutting UI thread.", debugFileName);
            }
            catch (Exception ex)
            {
                LogDebugMessage(String.Format($"OCT exited due to an unexpected error: {ex.Message} at {ex.TargetSite}"), debugFileName);
            }
        }

        public static void LogDebugMessage(string message, string logFileName = null)
        {
            message = String.IsNullOrEmpty(message) ? Environment.NewLine : message + Environment.NewLine;

            bool ok = false;
            try
            {
                if (!String.IsNullOrEmpty(logFileName)) System.IO.File.AppendAllText(logFileName, String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}"));
                ok = true;
            }
            catch
            {
            }

            if (!ok)
            {
                string debugFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OverloadClientTool\\Debug");
                string debugFileName = Path.Combine(debugFileFolder, String.Format($"OCT_Debug.txt"));
                try { Directory.CreateDirectory(debugFileFolder); } catch { }
                try { System.IO.File.AppendAllText(debugFileName, String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}")); } catch { }
            }
        }

        public static bool ValidFileName(string path, bool mustExist = false)
        {
            try
            {
                bool test = new FileInfo(path).Exists;
                if (mustExist) return test;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidDirectoryName(string path, bool mustExist = false)
        {
            try
            {
                bool test = new DirectoryInfo(path).Exists;
                if (mustExist) return test;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetFileVersion(string fileName)
        {
            try
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
                return fileVersionInfo.FileVersion;
            }
            catch
            {
                return null;
            }
        }

        public static string GetFileDescription(string fileName)
        {
            try
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
                return fileVersionInfo.FileDescription;
            }
            catch
            {
                return null;
            }
        }

        public static void RemoveDirectory(string path)
        {
            if (!ValidDirectoryName(path, true)) return;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (FileInfo fi in dir.GetFiles()) fi.Delete();
                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    RemoveDirectory(di.FullName);
                    di.Delete();
                }
            }
            catch
            {
            }
        }

        public static Process GetRunningProcess(string name)
        {
            if (String.IsNullOrEmpty(name)) return null;
            foreach (Process process in Process.GetProcesses()) if (process.ProcessName.ToLower() == name.ToLower()) return process;
            return null;
        }
  
        public static bool UpdateCheck(string debugFileName, bool forceUpdate = false)
        {
            LogDebugMessage("Checking for new release.", debugFileName);

            try
            {
                OCTRelease release = GetLastestRelease;
                if (release != null)
                {
                    LogDebugMessage("Got release info - checking for current vs new release info.", debugFileName);

                    // Fix version numbers so they are both in xx.yy.zz format (3 components and numbers only).
                    string newVersion = VersionStringFix(release.Version);
                    string[] newVersionSplit = newVersion.Split(".".ToCharArray());
                    if (newVersionSplit.Length > 3) newVersion = newVersionSplit[0] + "." + newVersionSplit[1] + "." + newVersionSplit[2];

                    string currentVersion = null;
                    using (var process = Process.GetCurrentProcess()) currentVersion = GetFileVersion(process.MainModule.FileName);
                    string[] currentVersionSplit = currentVersion.Split(".".ToCharArray());
                    if (currentVersionSplit.Length > 3) currentVersion = currentVersionSplit[0] + "." + currentVersionSplit[1] + "." + currentVersionSplit[2];

                    // Check if update is available.
                    if (!String.IsNullOrEmpty(currentVersion) && (currentVersion != newVersion))
                    {
                        return PerformUpdate(release, currentVersion, newVersion, Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
                    }
                    else
                    {
                        LogDebugMessage(String.Format($"No update available - continuing OCT startup.", debugFileName));
                    }
                }
                else
                {
                    LogDebugMessage("Could not get info on new OCT release (timeout or unexpected error).", debugFileName);
                }
            }
            catch (Exception ex)
            {
                LogDebugMessage(String.Format($"Unable to check/perform OCT update: {ex.Message}"), debugFileName);
            }

            return false;
        }

        private static string VersionStringFix(string version)
        {
            var result = string.Empty;
            foreach (char c in version) if ((c == '.') || (c >= '0' && c <= '9')) result += c;
            if (result.StartsWith(".")) result = result.Substring(1);
            return result;
        }

        private static void KillRunningProcess(string name)
        {
            foreach (Process process in Process.GetProcesses()) if (process.ProcessName.ToLower() == name.ToLower()) process.Kill();
        }

        private static bool PerformUpdate(OCTRelease release, string currentVersion, string newVersion, string installFolder)
        {
            OCTUpdateForm updateForm = new OCTUpdateForm(release, currentVersion, newVersion, installFolder);
            updateForm.StartPosition = FormStartPosition.CenterScreen;
            if (updateForm.ShowDialog() == DialogResult.Cancel) return false;

            string localTempZip = Path.GetTempFileName() + "_OCT_Update.zip";
            string localTempFolder = Path.GetTempFileName() + "_OCT_Update";
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

                string localSourceFolder = localTempFolder;

                // If the ZIP contains a folder then the files to copy will be inside this.
                DirectoryInfo subDirList = new DirectoryInfo(localTempFolder);
                DirectoryInfo[] subDirs = subDirList.GetDirectories();
                if (subDirs.Length > 0) localSourceFolder = subDirs[0].FullName;

                Process appStart = new Process();
                appStart.StartInfo = new ProcessStartInfo(Path.Combine(localSourceFolder, "Updater.exe"));
                appStart.StartInfo.Arguments = String.Format($"-installfolder \"{installFolder}\"");

                appStart.StartInfo.WorkingDirectory = localSourceFolder;
                appStart.Start();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format($"Error downloading/running OCT updater from Github: {ex.Message}"));
                return false;
            }
        }

        public static OCTRelease GetLastestRelease
        {
            get
            {
                string jsonOverloadClientUrl = @"https://api.github.com/repos/maestrodk/oct/releases/latest";

                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = (Binder, certificate, chain, errors) => { return true; };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string json = "";

                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "OCT - user " + WindowsIdentity.GetCurrent().Name);
                        json = wc.DownloadString(jsonOverloadClientUrl);
                    }

                    dynamic octReleaseInfo = JsonConvert.DeserializeObject(json);

                    string zipUrl = octReleaseInfo.assets[0].browser_download_url;
                    long size = Convert.ToInt64(octReleaseInfo.assets[0].size);
                    DateTime created = Convert.ToDateTime(octReleaseInfo.assets[0].created_at, CultureInfo.InvariantCulture);
                    string version = octReleaseInfo.tag_name;

                    OCTRelease release = new OCTRelease();
                    release.DownloadUrl = zipUrl;
                    release.Size = size;
                    release.Created = created;
                    release.Version = version;

                    return release;
                }
                catch (Exception ex)
                {
                }

                return null;
            }
        }
    }
}
