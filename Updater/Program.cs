using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Missing install folder argument! Press a key to exit the OCT updater.");
                Console.ReadKey(true);
                return;
            }

            if (args[0].ToLower() != "-installfolder")
            {
                Console.WriteLine("Missing -installfolder <folder>! Press a key to exit the OCT updater.");
                Console.ReadKey(true);
                return;
            }

            string sourceFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string installFolder = args[1];

            if (!ValidDirectoryName(sourceFolder, true) && !ValidDirectoryName(installFolder, true))
            {
                Console.WriteLine("Either update folder or installation folder is invalid! Press a key to exit the OCT updater.");
                Console.ReadKey(true);
                return;
            }

            // Kill running OCT before continuing (but allow 1 second for OCT to shut itself down).
            Console.WriteLine("Updating Overload Client Tool - please wait!");
            Console.WriteLine();

            //Console.WriteLine(String.Format($"Source folder: {sourceFolder}"));
            //Console.WriteLine(String.Format($"Install folder: {installFolder}"));
            //Console.WriteLine();

            UnblockPath(sourceFolder);

            try
            {
                // At this point OCT will already have saved settings and shut itself down.
                // But we make sure it isn't running just to be on the safe side.
                KillRunningProcess("OverloadClientTool");

                // Copy new files to destination, overwriting any existing files.
                DirectoryInfo dir = new DirectoryInfo(sourceFolder);
                foreach (FileInfo fi in dir.GetFiles())
                {
                    if (!fi.Name.ToLower().Contains("update"))
                    {
                        Console.WriteLine(String.Format($"Copying {fi.Name}"));
                        File.Copy(fi.FullName, Path.Combine(installFolder, fi.Name), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying new files: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine($"Press a key to exit the OCT updater.");
                Console.ReadKey(true);
                return;
            }

            // Launch updated OCT application.
            try
            {
                Process appStart = new Process();
                appStart.StartInfo.FileName = Path.Combine(installFolder, "OverloadClientTool.exe");
                appStart.StartInfo.Arguments = String.Format($"-cleanup \"{sourceFolder}\"");
                appStart.StartInfo.WorkingDirectory = installFolder;
                appStart.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine($"Press a key to exit the OCT updater.");
                Console.ReadKey(true);
            }
        }

        private static void KillRunningProcess(string name)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == name.ToLower()) process.Kill();
            }
        }

        private static bool ValidDirectoryName(string path, bool mustExist = false)
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

        public static Process GetRunningProcess(string name)
        {
            if (String.IsNullOrEmpty(name)) return null;
            foreach (Process process in Process.GetProcesses()) if (process.ProcessName.ToLower() == name.ToLower()) return process;
            return null;
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
