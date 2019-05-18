using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Diagnostics;
using System.IO.Compression;

using OverloadClientTool;

namespace OverloadClientApplication
{
    public partial class OCTUpdateForm : Form
    {
        private OCTRelease release = null;
        private string runningOCT = null;

        private string currentVersion = null;
        private string newVersion = null;
        private string installFolder = null;

        private OCTUpdateForm() { }

        public OCTUpdateForm(OCTRelease release, string currentVersion, string newVersion, string installFolder)
        {
            this.currentVersion = currentVersion;
            this.newVersion = newVersion;
            this.installFolder = installFolder;
            this.release = release;

            InitializeComponent();

            OCTCurrentVersion.Text = "Your current version is " + currentVersion;
            OCTNewVersion.Text = "New online version is " + newVersion;
        }

        private const string jsonOverloadClientUrl = @"https://api.github.com/repos/maestrodk/oct/releases/latest";
        private const string jsonZipField = @"browser_download_url";

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

 
        private void UpgradeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DeclineUpgrade_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
