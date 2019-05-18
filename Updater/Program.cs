using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            }

            if (args[0].ToLower() != "-installfolder")
            {
                Console.WriteLine("Missing -installfolder <folder>! Press a key to exit the OCT updater.");
                Console.ReadKey(true);
            }

            string sourceFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string installFolder = args[1];

            if (!ValidDirectoryName(sourceFolder, true) && !ValidDirectoryName(installFolder, true))
            {
                Console.WriteLine("Either update folder or installation folder is invalid! Press a key to exit the OCT updater.");
                Console.ReadKey(true);
            }

            // Kill running OCT before continuing (but allow 1 second for OCT to shut itself down).
            Console.WriteLine("Updating Overload Client Tool - please wait!");

            Console.WriteLine();
            Console.WriteLine(String.Format($"Source folder: {sourceFolder}"));
            Console.WriteLine(String.Format($"Install folder: {installFolder}"));
            Console.WriteLine();

            try
            {
                // Allow OCT to do a graceful shutdown before continuing.
                Thread.Sleep(500);

                KillRunningProcess("OverloadClientTool");

                // Copy new files to destination, overwriting any existing files.
                DirectoryInfo dir = new DirectoryInfo(sourceFolder);
                foreach (FileInfo fi in dir.GetFiles()) if (!fi.Name.ToLower().Contains("update")) File.Copy(fi.FullName, Path.Combine(installFolder, fi.Name), true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} - Press a key to exit the OCT updater.");
                Console.ReadKey(true);
                return;
            }

            // Finally launch the updated OCT application.
            Console.WriteLine("New files copied - restarting OCT");

            try
            {
                string app = Path.Combine(installFolder, "OverloadClientTool.exe");
                string arg = String.Format($"-cleanup \"{Path.GetDirectoryName(sourceFolder)}\"");
                string dir = Path.GetDirectoryName(installFolder);

                Console.WriteLine();
                Console.WriteLine(String.Format($"Destination: {dir}"));
                Console.WriteLine(String.Format($"Application: {app}"));
                Console.WriteLine(String.Format($"Arguments: {arg}"));
                Console.WriteLine();

                Console.WriteLine($"Update completed - Press a key to exit the OCT updater and start OCT");
                Console.ReadKey(true);

                Process appStart = new Process();
                appStart.StartInfo = new ProcessStartInfo(Path.Combine(installFolder, "OverloadClientTool.exe"), String.Format($"-cleanup \"{Path.GetDirectoryName(sourceFolder)}\""));
                appStart.StartInfo.WorkingDirectory = Path.GetDirectoryName(installFolder);
                appStart.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} - Press a key to exit the OCT updater.");
                Console.ReadKey(true);
            }
        }

        private static void KillRunningProcess(string name)
        {
            foreach (Process process in Process.GetProcesses()) if (process.ProcessName.ToLower() == name.ToLower()) process.Kill();
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
    }
}
