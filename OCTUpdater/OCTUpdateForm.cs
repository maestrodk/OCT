using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Diagnostics;
using System.IO.Compression;

namespace OCTUpdater
{
    public partial class OCTUpdateForm : Form
    {
        private OCTRelease release = null;
        private string runningOCT = null;

        private string currentVersion = null;
        private string newVersion = null;
        private string installFolder = null;


        private OCTUpdateForm() { }

        public OCTUpdateForm(string currentVersion, string newVersion, string installFolder)
        {
            this.currentVersion = currentVersion;
            this.newVersion = newVersion;
            this.installFolder = installFolder;

            InitializeComponent();

            OCTCurrentVersion.Text = "Your current version is " + currentVersion;
            OCTNewVersion.Text = "New online version is " + newVersion;
        }

        private const string jsonOverloadClientUrl = @"https://api.github.com/repos/maestrodk/oct/releases/latest";
        private const string jsonZipField = @"browser_download_url";

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
                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => { return true; };
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

        public bool ValidFileName(string path, bool mustExist = false)
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

        public bool ValidDirectoryName(string path, bool mustExist = false)
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

        public string GetFileVersion(string fileName)
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

        public string GetFileDescription(string fileName)
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

        public void RemoveDirectory(string path)
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

        public void DownloadAndInstall(string localInstallFolder)
        {
            string localTempZip = Path.GetTempFileName() + "_OCT_Update.zip";
            string localTempFolder = Path.GetTempFileName() + "_OCT_Update";
            Directory.CreateDirectory(localTempFolder);

            bool restart = false;

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

                // Shutdown running OCT before copying files.
                KillRunningProcess("OverloadClientTool");

                // Copy all files to destination, overwriting any existing files.
                DirectoryInfo dir = new DirectoryInfo(localSourceFolder);
                foreach (FileInfo fi in dir.GetFiles()) if (!fi.Name.ToLower().Contains("newtonsoft")) File.Copy(fi.FullName, Path.Combine(localInstallFolder, fi.Name), true);

                // Time stamp OverloadClientTool.exe.
                File.SetCreationTime(Path.Combine(localInstallFolder, "OverloadClientTool.exe"), release.Created);
                File.SetLastWriteTime(Path.Combine(localInstallFolder, "OverloadClientTool.exe"), release.Created);

                restart = true;
            }
            catch (Exception ex)
            {
                // LogErrorMessage(String.Format($"Cannot download OCT installation ZIP file from Github: {ex.Message}"));
            }
            finally
            {
                if (ValidFileName(localTempZip, true)) try { File.Delete(localTempZip); } catch { }
                if (ValidDirectoryName(localTempFolder, true)) try { RemoveDirectory(localTempFolder); } catch { }
            }

            if (restart)
            {
                // Restart OCT.
                Process appStart = new Process();
                appStart.StartInfo = new ProcessStartInfo(Path.Combine(localInstallFolder, "OverloadClientTool.exe"));
                appStart.StartInfo.WorkingDirectory = localInstallFolder;
                appStart.Start();
            }

            Close();
        }

        private void UpgradeButton_Click(object sender, EventArgs e)
        {
            DownloadAndInstall(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
        }

        private void DeclineUpgrade_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OCTUpdateForm_Load(object sender, EventArgs e)
        {
            release = GetLastestRelease;
            if (release == null) Close();

            OCTNewVersion.Text = "New version available is " + release.Version;
        }
    }
}
