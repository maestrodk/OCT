using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OverloadClientTool
{
    public class Install
    {
        public static object ConfigurationManager { get; private set; }

        public static void InstallTo(string installFolder, string debugFileName)
        {
            string applicationName = "OverloadClientTool";

            string sourceFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            if (!OverloadClientToolApplication.ValidDirectoryName(installFolder, true))
            {
                OverloadClientToolApplication.LogDebugMessage($"Install folder is invalid.", debugFileName);
                return;
            }

            OverloadClientToolApplication.LogDebugMessage($"Copying new files.", debugFileName);

            try
            {
                // At this point OCT should already have saved settings and shut itself down.
                // But we make sure it isn't running just to be on the safe side.
                KillRunningProcess(applicationName);

                // Give Windows a little time to release file locks (issue with Newtonsoft DLL).
                Thread.Sleep(2000);

                // Copy new files to destination, overwriting any existing files.
                DirectoryInfo dir = new DirectoryInfo(sourceFolder);
                foreach (FileInfo fi in dir.GetFiles())
                {
                    if (fi.Name.ToLower().EndsWith(".dll") || fi.Name.ToLower().EndsWith(".exe"))
                    {
                        File.Copy(fi.FullName, Path.Combine(installFolder, fi.Name), true);
                    }
                    else if (fi.Name.ToLower().EndsWith(applicationName + ".config"))
                    {
                        string existingConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

                        if (OverloadClientToolApplication.ValidFileName(existingConfig, true))
                        {
                            XDocument newDoc = XDocument.Load(fi.FullName);

                            XDocument oldDoc = XDocument.Load(existingConfig);

                            IEnumerable<XElement> newSettings =
                                from p in newDoc.Descendants("setting")
                                where p.Parent.Name == "OverloadClientTool.Properties.Settings"
                                select p;

                            IEnumerable<XElement> oldSettings =
                                from p in oldDoc.Descendants("setting")
                                where p.Parent.Name == "OverloadClientTool.Properties.Settings"
                                select p;

                            // <setting name = "OverloadPath" serializeAs = "String">
                            //   <Value />
                            // </setting >

                            // For each configuration property update value with current value if same setting exists.
                            foreach (XElement n in newSettings)
                            {
                                var newNameAttr = n.Attributes().Single(x => x.Name == "name");

                                bool add = true;
                                foreach (XElement o in oldSettings)
                                {
                                    var oldNameAttr = o.Attributes().Single(x => x.Name == "name");
                                    if (oldNameAttr.Value == newNameAttr.Value) newNameAttr.Parent.Value = oldNameAttr.Parent.Value;
                                }
                            }

                            newDoc.Save(fi.FullName);
                        }

                        // Copy new/updated configuration.
                        File.Copy(fi.FullName, Path.Combine(installFolder, fi.Name), true);
                    }
                }
            }
            catch (Exception ex)
            {
                OverloadClientToolApplication.LogDebugMessage($"Error updating application: {ex.Message}", debugFileName);
                MessageBox.Show($"Error copying new files: {ex.Message}");
                return;
            }

            // Launch updated application.
            try
            {
                OverloadClientToolApplication.LogDebugMessage($"Trying to restart application.", debugFileName);

                Process appStart = new Process();
                appStart.StartInfo.FileName = Path.Combine(installFolder, applicationName + ".exe");
                appStart.StartInfo.Arguments = String.Format($"-cleanup \"{sourceFolder}\"");
                appStart.StartInfo.WorkingDirectory = installFolder;
                appStart.Start();
            }
            catch (Exception ex)
            {
                OverloadClientToolApplication.LogDebugMessage($"Error restarting application: {ex.Message}", debugFileName);
                MessageBox.Show($"Error restarting application: {ex.Message}");
            }
        }

        private static void KillRunningProcess(string name)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.ProcessName.ToLower() == name.ToLower()) && (process.Id != Process.GetCurrentProcess().Id)) process.Kill();
            }
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

        public static void UnblockPath(string path)
        {
            string[] files = System.IO.Directory.GetFiles(path);
            string[] dirs = System.IO.Directory.GetDirectories(path);

            foreach (string file in files)
            {
                UnblockFile(file);
            }

            foreach (string dir in dirs)
            {
                UnblockPath(dir);
            }
        }

        public static bool UnblockFile(string fileName)
        {
            try { return DeleteFile(fileName + ":Zone.Identifier"); } catch { return false; }
        }
    }
}