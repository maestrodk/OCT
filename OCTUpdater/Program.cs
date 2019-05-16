using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCTUpdater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 4) return;
            if (args[0] != "-update") return;

            string currentVersion = args[1].Replace("_", " ");
            string newVersion = args[2].Replace("_", " ");
            string installFolder = args[2].Replace("_", " ");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OCTUpdateForm(currentVersion, newVersion, installFolder));
        }
    }
}
