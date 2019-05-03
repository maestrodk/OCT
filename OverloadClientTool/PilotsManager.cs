using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public partial class OCTMainForm
    {
        private string pilotsPath = SpecialFolderLocalLowPath + "\\Revival\\Overload";
        private string pilotsBackupPath = SpecialFolderLocalLowPath + "\\Revival\\Overload\\Pilot Backup";

        private List<string> currentPilots = null;

        BackgroundWorker pilotsBackgroundWorker = null;
        
        private void InitPilotsListBox()
        {
            currentPilots = Pilots;

            // Init listbox.
            foreach (string pilot in currentPilots) PilotsListBox.Items.Add(pilot);

            // Disallow deleting the last pilot.
            PilotDeleteButton.Enabled = (PilotsListBox.Items.Count > 1);

            // Begin monitoring folder.
            pilotsBackgroundWorker = new BackgroundWorker();
            pilotsBackgroundWorker.DoWork += BackgroundPilotChecker;
            pilotsBackgroundWorker.RunWorkerAsync();
        }

        private void StopPilotsMonitoring()
        {
            if (pilotsBackgroundWorker != null)
            {
                pilotsBackgroundWorker.DoWork -= BackgroundPilotChecker;
                pilotsBackgroundWorker.Dispose();
            }            
        }

        private void BackgroundPilotChecker(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(5000);

                List<string> pilotsInFolder = Pilots;

                // See if update is required.
                bool update = (pilotsInFolder.Count != currentPilots.Count);
                if (!update) foreach (string pilotName in pilotsInFolder) if (!currentPilots.Contains(pilotName)) update = true;

                if (update)
                {
                    // Update pilot list and refresh listbox content.
                    currentPilots = pilotsInFolder;
                    PilotsListBox.Invoke(new Action(() => PilotsListBox.Items.Clear()));
                    PilotsListBox.Invoke(new Action(() => { foreach (string pilot in currentPilots) PilotsListBox.Items.Add(pilot); }));
                    PilotsListBox.Invoke(new Action(() => PilotsListBox.Invalidate()));
                }
            }
        }

        private List<string> Pilots
        {
            get
            {
                List<string> pilots = new List<string>();

                string[] list = Directory.GetFiles(pilotsPath, "*.xconfig");

                // Allow all 3 pilot files to be created before checking.
                Thread.Sleep(250);

                foreach (string p in list)
                {
                    string name = Path.GetFileNameWithoutExtension(p);
                    string prefs = Path.Combine(pilotsPath, name + ".xprefs");
                    string scores = Path.Combine(pilotsPath, name + ".xscores");

                    if (!String.IsNullOrEmpty(p) && System.IO.File.Exists(prefs) && System.IO.File.Exists(scores)) pilots.Add(name);
                }

                return pilots;
            }
        }

        private void PilotsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void PilotBackupButton_Click(object sender, EventArgs e)
        {
            BackupAllPilots();
        }

        private void PilotRenameButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet available :)");
        }

        private void PilotDeleteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet available :)");
        }

        private void PilotCloneButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet available :)");
        }

        private void AutoPilotsBackupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            AutoSavePilots = AutoPilotsBackupCheckbox.Checked;
        }

        private string BackupAllPilots()
        {
            string timeStamp = DateTime.Now.ToString("yyyy:MM:dd_HH:mm:ss").Replace(":", "");
            string zipName = Path.Combine(pilotsBackupPath, "Overload_Pilots_" + timeStamp + ".zip");

            Directory.CreateDirectory(pilotsBackupPath);

            using (var fileStream = new FileStream(zipName, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    foreach (string pilot in Pilots)
                    {
                        string xconfig = Path.Combine(pilotsPath, pilot + ".xconfig");
                        string prefs = Path.Combine(pilotsPath, pilot + ".xprefs");
                        string scores = Path.Combine(pilotsPath, pilot + ".xscores");

                        FileInfo fiXconfig = new FileInfo(xconfig);
                        FileInfo fiPrefs = new FileInfo(prefs);
                        FileInfo fiScores = new FileInfo(scores);

                        if (fiXconfig.Exists && fiPrefs.Exists && fiScores.Exists)
                        {
                            byte[] buffer = File.ReadAllBytes(xconfig);
                            var zipArchiveEntry = archive.CreateEntry(Path.GetFileName(xconfig), CompressionLevel.Optimal);
                            using (var zipStream = zipArchiveEntry.Open())  zipStream.Write(buffer, 0, buffer.Length);

                            buffer = File.ReadAllBytes(prefs);
                            zipArchiveEntry = archive.CreateEntry(Path.GetFileName(prefs), CompressionLevel.Optimal);
                            using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(buffer, 0, buffer.Length);

                            buffer = File.ReadAllBytes(xconfig);
                            zipArchiveEntry = archive.CreateEntry(Path.GetFileName(xconfig), CompressionLevel.Optimal);
                            using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(buffer, 0, buffer.Length);
                        }
                    }

                    return String.Format($"{Path.GetFileName(zipName)}");
                }
            }
        }
    }
}
