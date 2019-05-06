using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Process thisProcess = Process.GetCurrentProcess();
            //
            //Process process = Process.GetProcesses().First(p => p.ProcessName.Contains("OverloadClientTool") && (p != thisProcess));
            //if (process != null)
            //{
            //IntPtr ipHwnd = process.MainWindowHandle;
            //Thread.Sleep(250);
            //SetForegroundWindow(ipHwnd);
            //return;
            // }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OCTMain(args));
        }

    }
}
