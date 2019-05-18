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

            string sourceFolder = AppDomain.CurrentDomain.BaseDirectory;
            string installFolder = args[1];
            if (!ValidDirectoryName(sourceFolder, true) && !ValidDirectoryName(installFolder, true))
            {
                Console.WriteLine("Either update folder or installation folder is invalid! Press a key to exit the OCT updater.");
                Console.ReadKey(true);
            }

            // Kill running OCT before continuing (but allow 1 second for OCT to shut itself down).
            Console.WriteLine("Installing new OCT version... pleae wait!");
            Thread.Sleep(1000);

            // Copy new files to destination, overwriting any existing files.
            DirectoryInfo dir = new DirectoryInfo(sourceFolder);
            foreach (FileInfo fi in dir.GetFiles())
            {
                if (!fi.Name.ToLower().Contains("update")) File.Copy(fi.FullName, Path.Combine(installFolder, fi.Name), true);
            }

            // Finally launch the updated OCT application.
            Console.WriteLine("New files copied - restarting OCT");

            Process appStart = new Process();
            appStart.StartInfo = new ProcessStartInfo(Path.Combine(installFolder, "OverloadClientTool.exe"),  String.Format($"-cleanup \"{sourceFolder}\""));
            appStart.StartInfo.WorkingDirectory = installFolder;
            appStart.Start();
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
