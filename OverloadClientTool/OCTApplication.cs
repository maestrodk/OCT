using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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

            LogDebugMessage("Checking for new release.", debugFileName);

            OCTRelease release = GetLastestRelease;
            if (release != null)
            {
                LogDebugMessage("Got release info - checking for current vs new release info.", debugFileName);

                string newVersion = release.Version.ToLower().Replace("v.", "").Replace("v", "");

                bool upgrading = false; 
                using (var process = Process.GetCurrentProcess())
                {
                    string currentVersion = GetFileVersion(process.MainModule.FileName).Replace("v", "");
                    string[] currentVersionDotSplit = currentVersion.Split(".".ToCharArray());

                    if (currentVersionDotSplit.Length > 2) currentVersion = currentVersionDotSplit[0] + "." + currentVersionDotSplit[1] + "." + currentVersionDotSplit[2];

                    if (currentVersion != newVersion)
                    {
                        LogDebugMessage(String.Format($"New update is available - chaining to OCTUpdater.", debugFileName));

                        // Do the update.
                        Process appStart = new Process();
                        appStart.StartInfo = new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(process.MainModule.FileName), "OCTUpdater.exe"));

                        upgrading = true;

                        // Pass current version, new version and install folder.
                        appStart.StartInfo.Arguments = String.Format($"-update {currentVersion.Replace(" ", "_")} {newVersion.Replace(" ", "_")} {Path.GetDirectoryName(process.MainModule.FileName)}");

                        LogDebugMessage(String.Format($"Startiong up OCTUpdater.", debugFileName));

                        appStart.Start();
                    }
                }

                if (upgrading)
                {
                    LogDebugMessage(String.Format($"Waiting for OCTUpdater to finish.", debugFileName));

                    Thread.Sleep(2000);
                    while (GetRunningProcess("OCTUpdater") != null) Thread.Sleep(500);

                    LogDebugMessage(String.Format($"OCTUpdater no longer running - continuing to OCT main.", debugFileName));

                }
                else
                {
                    LogDebugMessage(String.Format($"No update available - continuing to OCT main.", debugFileName));
                }
            }
            else
            {
                LogDebugMessage("Could not get info on new release.", debugFileName);
            }

            try
            {
                LogDebugMessage("Enabling visual styles.", debugFileName);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                LogDebugMessage("Starting up user interface.", debugFileName);
                Application.Run(new OCTMain(args, debugFileName));
                LogDebugMessage("Shutting down user interface.", debugFileName);
            }
            catch (Exception ex)
            {
                LogDebugMessage(String.Format($"{ex.Message} at {ex.TargetSite}"), debugFileName);
            }

            LogDebugMessage("OCT application exit.", debugFileName);
        }

        public class OCTRelease
        {
            public string DownloadUrl { get; set; }
            public long Size { get; set; }
            public DateTime Created { get; set; }
            public string Version { get; set; }
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
                        wc.Headers.Add("User-Agent", "Overload Client Tool - user " + WindowsIdentity.GetCurrent().Name);
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
                catch
                {
                }

                return null;
            }
        }
    }
}
