using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OverloadClientTool
{
    static class OverloadClientToolApplication
    {
        static System.Threading.Mutex singleton = new Mutex(true, "OCT");

        internal static OCTMain octMain = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!singleton.WaitOne(1000, false)) return;

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
                    if (stream != null)
                    { 
                        Byte[] assemblyData = new Byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        return System.Reflection.Assembly.Load(assemblyData);
                    }
                }

                return null;
            };

            Application.ThreadException += new ThreadExceptionEventHandler(MyCommonExceptionHandlingMethod);

            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

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

            // Check if just installed and we need to cleanup.
            Dictionary<string, string> oldSettings = new Dictionary<string, string>();

            if ((args.Length > 1) && (args[0].ToLower() == "-cleanup"))
            {
                LogDebugMessage("Removing installation files in " + args[1] + ".", debugFileName);
                RemoveInstallDirectory(args[1], debugFileName);
                LogDebugMessage("Finished cleanup.", debugFileName);
            }

            string applicationFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string previousConfig = Path.Combine(applicationFolder, "previous.config");
            if (OverloadClientToolApplication.ValidFileName(previousConfig, true))
            {
                LogDebugMessage("Will merge previous settings.");

                XDocument oldDoc = XDocument.Load(previousConfig);

                IEnumerable<XElement> settings =
                    from p in oldDoc.Descendants("setting")
                      where p.Parent.Name == "OverloadClientTool.Properties.Settings"
                        select p;

                // <setting name = "OverloadPath" serializeAs = "String">
                //   <Value />
                // </setting >

                foreach (XElement o in settings)
                {
                    try
                    {
                        var oldAttr = o.Attributes().Single(x => x.Name == "name");
                        var oldName = oldAttr.Value;
                        var oldValue = oldAttr.Parent.Value;
                        oldSettings.Add(oldName, oldValue);
                        LogDebugMessage($"Read previous setting: {oldAttr} = {oldValue}");
                    }
                    catch
                    {
                    }
                }

                try { File.Delete(previousConfig); } catch { }
            }

            try
            {
                bool retry = true;

                // Execute main UI loop.
                while (retry)
                {
                    retry = false;

                    try
                    {
                        octMain = new OCTMain(args, debugFileName, oldSettings);
                    }
                    catch
                    {
                        MessageBox.Show("The configuration file seems to be corrupted and all settings had to be reset.", "Uh-oh!");

                        string configurationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OverloadClientTool");

                        string[] dirList = Directory.GetDirectories(configurationFolder, "OverloadClientTool.exe_Url_*", SearchOption.TopDirectoryOnly);
                        foreach (string dir in dirList)
                        {
                            Directory.Delete(dir, true);
                        }

                        Application.Restart();
                        Environment.Exit(0);
                    }
                }

                LogDebugMessage("Starting OCT main thread, launching UI loop.", debugFileName);
                Application.Run(octMain);
                LogDebugMessage("OCT main exit - shutting down mail startup thread.", debugFileName);
            }
            catch (Exception ex)
            {
                string message = ex.ToString(); // GetExceptionMessages(ex);
                
                LogDebugMessage($"Application crashed: {message}", debugFileName);

                OverloadClientApplication.OCTErrorForm errorForm = new OverloadClientApplication.OCTErrorForm(message);
                try 
                { 
                    OCTMain.ApplyThemeToControl(errorForm, octMain.theme);
                    errorForm.BackColor = octMain.theme.InactivePaneButtonBackColor;
                    errorForm.StartPosition = FormStartPosition.CenterParent;
                    octMain.Dispose();
                }
                catch 
                { 
                }


                errorForm.ShowDialog();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ProcesException((Exception)e.ExceptionObject);
        }

        private static void MyCommonExceptionHandlingMethod(object sender, ThreadExceptionEventArgs t)
        {
            ProcesException(t.Exception);
        }

        private static void ProcesException(Exception ex)
        { 
            string message = ex.ToString(); // GetExceptionMessages(ex);

            string startDateTime = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss");
            string debugFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OverloadClientTool\\Debug");
            string debugFileName = Path.Combine(debugFileFolder, String.Format($"OCT_Debug_{startDateTime.Replace(":", "").Replace("-", "").Replace(" ", "_")}.txt"));
            try { Directory.CreateDirectory(debugFileFolder); } catch { }

            LogDebugMessage($"Application crashed: {message}", debugFileName);

            if (octMain != null) try { octMain.ShutdownTasks(); } catch { }

            OverloadClientApplication.OCTErrorForm errorForm = new OverloadClientApplication.OCTErrorForm(message);
            OCTMain.ApplyThemeToControl(errorForm, octMain.theme);

            errorForm.BackColor = octMain.theme.InactivePaneButtonBackColor;
            errorForm.StartPosition = FormStartPosition.CenterParent;

            errorForm.ShowDialog();

            //Exception handling...
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

        public static void TrackerMessage(string message)
        {
            /*
            if (false)
            {
                string trackerFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"C:\ProgramData\Revival");
                string trackerFileName = Path.Combine(trackerFileFolder, String.Format($"OCT_Server_Tracker_Log.txt"));
                try { Directory.CreateDirectory(trackerFileFolder); } catch { }

                message = String.IsNullOrEmpty(message) ? Environment.NewLine : message + Environment.NewLine;
                try { System.IO.File.AppendAllText(trackerFileName, String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}")); } catch { }
            }
            */
        }

        public static void DedicatedServerTrackerMessage(string message)
        {
            /*
            if (false)
            {
                string trackerFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"C:\ProgramData\Revival");
                string trackerFileName = Path.Combine(trackerFileFolder, String.Format($"DedicatedServerLog.txt"));
                try { Directory.CreateDirectory(trackerFileFolder); } catch { }

                message = String.IsNullOrEmpty(message) ? Environment.NewLine : message + Environment.NewLine;
                try { System.IO.File.AppendAllText(trackerFileName, String.Format($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}")); } catch { }
            }
            */
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

        public static string GetExceptionMessages(Exception ex)
        {
            var messages = new List<string>();
            
            while (ex != null)
            {
                messages.Add(ex.Message);
                messages.Add(ex.StackTrace);
                ex = ex.InnerException;
            }

            string message = String.Join(" - ", messages);
            return message;
        }
    }
}