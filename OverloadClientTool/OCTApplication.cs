using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OverloadClientTool
{
    static class OverloadClientToolApplication
    {    
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To embed a dll in a compiled exe:
            // 1 - Change the properties of the dll in References so that Copy Local=false
            // 2 - Add the dll file to the project as an additional file not just a reference
            // 3 - Change the properties of the file so that Build Action=Embedded Resource
            // 4 - Paste this code before Application.Run in the main exe
            AppDomain.CurrentDomain.AssemblyResolve += (Object sender, ResolveEventArgs resolveArgs) =>
            {
                String thisExe = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                System.Reflection.AssemblyName embeddedAssembly = new System.Reflection.AssemblyName(resolveArgs.Name);
                String resourceName = thisExe + "." + embeddedAssembly.Name + ".dll";

                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return System.Reflection.Assembly.Load(assemblyData);
                }
            };

            // Setup debug logging.
            string startDateTime = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss");
            string debugFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OverloadClientTool\\Debug");
            string debugFileName = Path.Combine(debugFileFolder, String.Format($"OCT_Debug_{startDateTime.Replace(":", "").Replace("-", "").Replace(" ", "_")}.txt"));
            try { Directory.CreateDirectory(debugFileFolder); } catch { }

            LogDebugMessage("OCT application startup.", debugFileName);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if ((args.Length > 1) && args[0].ToLower().StartsWith("-install"))
            {
                Install.InstallTo(args[1], debugFileName);
                return;
            }

            if ((args.Length > 1) && (args[0].ToLower() == "-cleanup"))
            {
                LogDebugMessage("Removing installation files in " + args[1], debugFileName);
                RemoveInstallDirectory(args[1], debugFileName);
                LogDebugMessage("Finished cleanup.", debugFileName);
            }

            try
            {
                LogDebugMessage("Starting OCT main UI thread.", debugFileName);
                Application.Run(new OCTMain(args, debugFileName));
                LogDebugMessage("OCT main exit - shutting UI thread.", debugFileName);
            }
            catch (Exception ex)
            {
                LogDebugMessage(String.Format($"OCT exited due to an unexpected error: {ex.Message} at {ex.TargetSite}"), debugFileName);
                string message = $"OCT crashed due to an unexpected error: {ex.Message} at {ex.TargetSite}";
                MessageBox.Show(message, "Internal error");
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

        public static bool ValidFileName(string fileName, bool mustExist = false)
        {
            try
            {
                bool test = new FileInfo(fileName).Exists;
                if (mustExist) return test;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidDirectoryName(string folderName, bool mustExist = false)
        {
            try
            {
                bool test = new DirectoryInfo(folderName).Exists;
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

        public static void RemoveInstallDirectory(string path, string debugFileName)
        {
            if (!ValidDirectoryName(path, true)) return;

            try
            {
                while (Directory.GetParent(path).Name.ToLower().Contains("oct_update")) path = Directory.GetParent(path).FullName;

                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    foreach (FileInfo fi in dir.GetFiles()) fi.Delete();
                    foreach (DirectoryInfo di in dir.GetDirectories())
                    {
                        RemoveDirectory(di.FullName);
                        di.Delete();
                    }

                    try { Directory.Delete(path); } catch { }

                }
                catch (Exception ex)
                {
                    LogDebugMessage(String.Format($"Cannot cleanup install folder: {ex.Message}"), debugFileName);
                }
            }
            catch (Exception ex)
            {
                LogDebugMessage(String.Format($"Cannot cleanup install folder: {ex.Message}"), debugFileName);
            }
        }

        public static string VersionStringFix(string version)
        {
            var result = string.Empty;

            foreach (char c in version) if ((c == '.') || (c >= '0' && c <= '9')) result += c;

            if (result.StartsWith(".")) result = result.Substring(1);

            string[] parts = result.Split(".".ToCharArray());
            if ((parts.Length > 3) && result.EndsWith(".0")) result = result.Substring(0, result.Length - 2);

            return result;
        }
    }
}