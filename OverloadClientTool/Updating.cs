using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Principal;
using System.Windows.Forms;
using Newtonsoft.Json;
using OverloadClientApplication;

namespace OverloadClientTool
{
    public partial class OCTMain : Form
    {
        private void AutoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AutoUpdateOCT = AutoUpdateCheckBox.Checked;
        }

        private void ForceUpdateButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            UpdateCheck(debugFileName, true);
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

        public void UpdateCheck(string debugFileName, bool forceUpdate = false)
        {
            try
            {
                OverloadClientToolApplication.LogDebugMessage("Checking for new release.", debugFileName);

                OCTRelease release = GetLastestRelease;
                if (release != null)
                {
                    OverloadClientToolApplication.LogDebugMessage("Checking release version.", debugFileName);

                    // Fix version numbers so they are both in xx.yy.zz format (3 components and numbers only).
                    string newVersion = OverloadClientToolApplication.VersionStringFix(release.Version);
                    string[] newVersionSplit = newVersion.Split(".".ToCharArray());
                    if (newVersionSplit.Length > 3) newVersion = newVersionSplit[0] + "." + newVersionSplit[1] + "." + newVersionSplit[2];

                    string currentVersion = null;
                    using (var process = Process.GetCurrentProcess()) currentVersion = OverloadClientToolApplication.GetFileVersion(process.MainModule.FileName);
                    string[] currentVersionSplit = currentVersion.Split(".".ToCharArray());
                    if (currentVersionSplit.Length > 3) currentVersion = currentVersionSplit[0] + "." + currentVersionSplit[1] + "." + currentVersionSplit[2];

                    // Check if update is available.
                    if (forceUpdate || (!String.IsNullOrEmpty(currentVersion) && (currentVersion != newVersion)))
                    {
                        PerformUpdate(release, currentVersion, newVersion, Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
                    }
                    else
                    {
                        OverloadClientToolApplication.LogDebugMessage(String.Format($"No update available - continuing startup.", debugFileName));
                    }
                }
                else
                {
                    OverloadClientToolApplication.LogDebugMessage("Unable to get release info.", debugFileName);
                }
            }
            catch (Exception ex)
            {
                OverloadClientToolApplication.LogDebugMessage(String.Format($"Unable to check/perform update: {ex.Message}"), debugFileName);
            }
        }

        public void PerformUpdate(OCTRelease release, string currentVersion, string newVersion, string installFolder)
        {
            OCTUpdateForm updateForm = new OCTUpdateForm(release, currentVersion, newVersion, installFolder);

            ApplyThemeToControl(updateForm, theme);
            updateForm.StartPosition = FormStartPosition.CenterParent;

            if (updateForm.ShowDialog() == DialogResult.Cancel) return;

            string localTempZip = Path.GetTempFileName() + "_OCT_Update.zip";
            string localTempFolder = Path.GetTempFileName() + "_OCT_Update";
            Directory.CreateDirectory(localTempFolder);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "OCT - user " + WindowsIdentity.GetCurrent().Name);
                    wc.DownloadFile(release.DownloadUrl, localTempZip);
                }

                ZipFile.ExtractToDirectory(localTempZip, localTempFolder);
                try { System.IO.File.Delete(localTempZip); } catch { }

                string localSourceFolder = localTempFolder;

                // If the ZIP contains a folder then the files to copy will be inside this.
                DirectoryInfo subDirList = new DirectoryInfo(localTempFolder);
                DirectoryInfo[] subDirs = subDirList.GetDirectories();
                if (subDirs.Length > 0) localSourceFolder = subDirs[0].FullName;

                Process appStart = new Process();
                appStart.StartInfo = new ProcessStartInfo(Path.Combine(localSourceFolder, "OverloadClientTool.exe"));
                appStart.StartInfo.Arguments = String.Format($"-install \"{installFolder}\"");
                appStart.StartInfo.WorkingDirectory = localSourceFolder;

                // Do graceful shutdown before launching the updater.
                Main_FormClosing(null, null);

                appStart.Start();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format($"Unable to update application: {ex.Message}", "Error"));
            }
        }
    }
}
