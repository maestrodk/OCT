using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    public partial class OCTMain
    {
        private string pilotsPath = SpecialFolderLocalLowPath + Path.DirectorySeparatorChar + "Revival" + Path.DirectorySeparatorChar + "Overload";
        private string pilotsBackupPath = SpecialFolderLocalLowPath + Path.DirectorySeparatorChar + "Revival" + Path.DirectorySeparatorChar + "Overload" + Path.DirectorySeparatorChar + "Pilot Backup";

        private List<string> currentPilots = null;

        BackgroundWorker pilotsBackgroundWorker = null;        
 
        private void InitPilotsListBox()
        {
            LogDebugMessage("InitPilotsListBox()");
            
            // Init listbox.
            CheckAndUpdatePilots();

            if (!IsElevated) PilotMakeActiveButton.Visible = false;

            //BackColor = theme.BackColor;
            //ForeColor = theme.ForeColor;

            // Begin monitoring folder.
            pilotsBackgroundWorker = new BackgroundWorker();
            pilotsBackgroundWorker.DoWork += BackgroundPilotChecker;
            pilotsBackgroundWorker.RunWorkerAsync();
        }

        private void CheckAndUpdatePilots()
        {
            this.UIThread(delegate
            {
                // Check if a pilot was added/removed.
                List<string> overloadPilots = OverloadPilots;

                bool update = (currentPilots == null) || (overloadPilots.Count != currentPilots.Count);
                if (!update) foreach (string pilotName in overloadPilots) if (!currentPilots.Contains(pilotName)) update = true;

                if (update)
                {
                    // Update listbox content.
                    currentPilots = overloadPilots;
                    PilotsListBox.Items.Clear();
                    foreach (string pilot in currentPilots) PilotsListBox.Items.Add(pilot);
                }

                // Enabled/disable buttons.
                bool delete = (PilotsListBox.Items.Count > 1) && (PilotsListBox.SelectedIndex >= 0);     // Don't allow last pilot to be deleted.
                bool rename = (PilotsListBox.SelectedIndex >= 0);
                bool clone = (PilotsListBox.SelectedIndex >= 0);
                bool select = (PilotsListBox.SelectedIndex >= 0);

                if (select)
                {
                    string pilotSelected = (string)PilotsListBox.Items[PilotsListBox.SelectedIndex];
                    string pilotCurrent = CurrentPilot;
                    if (pilotSelected.ToLower() == pilotCurrent.ToLower()) select = false;
                }

                if (IsOverloadOrOlmodRunning)
                {
                    // Don't allow deleting/renaming/setting XP if Overload is running.
                    delete = false;
                    rename = false;
                    select = false;
                }

                if (!IsElevated) select = false;

                PilotDeleteButton.Enabled = delete;
                PilotRenameButton.Enabled = rename;
                PilotCloneButton.Enabled = clone;
                PilotMakeActiveButton.Enabled = select;

                // Set current pilot name.
                string testPilotName = CurrentPilot;
                if (String.IsNullOrEmpty(testPilotName))
                {
                    PilotNameLabel.Text = "";
                }
                else
                {
                    try
                    {
                        string activeText = " is the active pilot.";
                        if (PilotNameLabel.Text != (testPilotName + activeText))
                        {
                            PilotNameLabel.Text = "";
                            PilotNameLabel.SelectionFont = new Font(PilotNameLabel.Font, FontStyle.Bold);
                            PilotNameLabel.AppendText(testPilotName);
                            PilotNameLabel.SelectionFont = new Font(PilotNameLabel.Font, FontStyle.Regular);
                            PilotNameLabel.AppendText(activeText);
                        }
                    }
                    catch
                    {
                    }
                }

                try
                {
                    OCTMain.ApplyThemeToControl(PanePilots, theme);
                }
                catch
                {
                }

                ValidateSetXp();
            });
        }

        private void StopPilotsMonitoring()
        {
            if (pilotsBackgroundWorker != null)
            {
                pilotsBackgroundWorker.DoWork -= BackgroundPilotChecker;
                pilotsBackgroundWorker.Dispose();
                pilotsBackgroundWorker = null;
            }            
        }
        
        private void BackgroundPilotChecker(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(1000);
                CheckAndUpdatePilots();
            }
        }

        private List<string> OverloadPilots
        {
            get
            {
                try { Directory.CreateDirectory(pilotsPath); } catch { }

                List<string> pilots = new List<string>();
                string[] list = Directory.GetFiles(pilotsPath, "*.xconfig");

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
            CheckAndUpdatePilots();

            if (PilotsListBox.SelectedIndex >= 0)
            {
                string pilotSelected = (string)PilotsListBox.Items[PilotsListBox.SelectedIndex];
                GetPilotXP(pilotSelected);
            }
            else
            {
                GetPilotXP(null);
            }
        }

        private void PilotBackupButton_Click(object sender, EventArgs e)
        {
            Verbose("Backing up pilots: " + BackupAllPilots());
        }

        private void PilotRenameButton_Click(object sender, EventArgs e)
        {
            string pilotSelected = (string)PilotsListBox.Items[PilotsListBox.SelectedIndex];

            if (CurrentPilot.ToLower() == pilotSelected)
            {
                MessageBox.Show($"Can't rename the active pilot!");
                return;
            }

            InputBox inputBox = new InputBox("Rename pilot", String.Format($"Enter a new name for pilot '{pilotSelected}'"), pilotSelected, this, theme);
            DialogResult result = inputBox.ShowDialog();

            if (result == DialogResult.OK)
            {
                string newPilot = inputBox.Result;

                if (newPilot.ToLower() == pilotSelected.ToLower())
                {
                    MessageBox.Show(String.Format($"New pilot name must be different from the old name!", "Rename operation cancelled"));
                    return;
                }

                for (int p = 0; p < PilotsListBox.Items.Count; p++)
                {
                    if ((p != PilotsListBox.SelectedIndex) && (pilotSelected.ToLower() == newPilot.ToLower()))
                    {
                        MessageBox.Show(String.Format($"A pilot named {newPilot} already exists!", "Rename operation cancelled"));
                        return;
                    }
                }

                // Make sure Overload/Oldmod isn't running.
                Thread.Sleep(500);
                if (IsOverloadOrOlmodRunning)
                {
                    MessageBox.Show("Overload is running - unsafe to rename pilot!", "Rename operation cancelled");
                    return;
                }                
                else
                {
                    // Rename pilot.
                    string[] list = Directory.GetFiles(pilotsPath, "*.xconfig");

                    try
                    {
                        File.Move(Path.Combine(pilotsPath, pilotSelected + ".xconfig"), Path.Combine(pilotsPath, newPilot + ".xconfig"));
                        File.Move(Path.Combine(pilotsPath, pilotSelected + ".xprefs"), Path.Combine(pilotsPath, newPilot + ".xprefs"));
                        File.Move(Path.Combine(pilotsPath, pilotSelected + ".xscores"), Path.Combine(pilotsPath, newPilot + ".xscores"));
                    }
                    catch (Exception ex)
                    {
                        // Try to roll back changes.
                        try { File.Move(Path.Combine(pilotsPath, newPilot + ".xconfig"), Path.Combine(pilotsPath, pilotSelected + ".xconfig")); } catch { }
                        try { File.Move(Path.Combine(pilotsPath, newPilot + ".xprefs"), Path.Combine(pilotsPath, pilotSelected + ".xprefs")); } catch { }
                        try { File.Move(Path.Combine(pilotsPath, newPilot + ".xscores"), Path.Combine(pilotsPath, pilotSelected + ".xscores")); } catch { }

                        MessageBox.Show($"Something went wrong during pilot rename (will try to rollback changes): {ex.Message}!");
                    }
                    finally
                    {
                        // Make sure list is current.
                        CheckAndUpdatePilots();
                    }
                }
            }
        }

        private void PilotDeleteButton_Click(object sender, EventArgs e)
        {
            string pilotSelected = (string)PilotsListBox.Items[PilotsListBox.SelectedIndex];

            if (CurrentPilot.ToLower() == pilotSelected)
            {
                MessageBox.Show($"Can't delete the active pilot!");
                return;
            }

            if (MessageBox.Show(String.Format($"Really delete pilot '{pilotSelected}'"), "Delete pilot", MessageBoxButtons.YesNo) != DialogResult.Yes) return;            

            // Make sure Overload/Oldmod isn't running.
            Thread.Sleep(500);
            if (IsOverloadOrOlmodRunning)
            {
                MessageBox.Show("Overload is running - unsafe to delete pilot!", "Delete operation cancelled");
                return;
            }
            else
            {
                // Rename pilot.
                string[] list = Directory.GetFiles(pilotsPath, "*.xconfig");

                try
                {
                    File.Delete(Path.Combine(pilotsPath, pilotSelected + ".xconfig"));
                    File.Delete(Path.Combine(pilotsPath, pilotSelected + ".xprefs"));
                    File.Delete(Path.Combine(pilotsPath, pilotSelected + ".xscores"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error trying to delete pilot: {ex.Message}");
                }
                finally
                {
                    // Make sure list is current.
                    CheckAndUpdatePilots();
                }
            }
        }

        private void PilotCloneButton_Click(object sender, EventArgs e)
        {
            string pilotSelected = (string)PilotsListBox.Items[PilotsListBox.SelectedIndex];

            InputBox inputBox = new InputBox("Clone pilot", String.Format($"Enter a name for the new pilot '{pilotSelected}'"), pilotSelected, this, theme);
            DialogResult result = inputBox.ShowDialog();

            if (result == DialogResult.OK)
            {
                string newPilot = inputBox.Result;

                for (int p = 0; p < PilotsListBox.Items.Count; p++)
                {
                    if ((p != PilotsListBox.SelectedIndex) && (pilotSelected.ToLower() == newPilot.ToLower()))
                    {
                        MessageBox.Show(String.Format($"A pilot named {newPilot} already exists!", "Clone operation aborted"));
                        return;
                    }
                }

                if (pilotSelected.ToLower() == newPilot.ToLower())
                {
                    MessageBox.Show("New pilot name is the same as the selected pilot!", "Unable to rename");
                }

                // Make sure Overload/Oldmod isn't running.
                Thread.Sleep(500);
                if (IsOverloadOrOlmodRunning)
                {
                    MessageBox.Show("Overload is running - unsafe to rename pilot!", "Unable to rename");
                    return;
                }
                else
                {
                    // Rename pilot.
                    string[] list = Directory.GetFiles(pilotsPath, "*.xconfig");

                    try
                    {
                        File.Copy(Path.Combine(pilotsPath, pilotSelected + ".xconfig"), Path.Combine(pilotsPath, newPilot + ".xconfig"));
                        File.Copy(Path.Combine(pilotsPath, pilotSelected + ".xprefs"), Path.Combine(pilotsPath, newPilot + ".xprefs"));
                        File.Copy(Path.Combine(pilotsPath, pilotSelected + ".xscores"), Path.Combine(pilotsPath, newPilot + ".xscores"));
                    }
                    catch (Exception ex)
                    {
                        // Try to roll back changes.
                        try { File.Delete(Path.Combine(pilotsPath, newPilot + ".xconfig")); } catch { }
                        try { File.Delete(Path.Combine(pilotsPath, newPilot + ".xprefs")); } catch { }
                        try { File.Delete(Path.Combine(pilotsPath, newPilot + ".xscores")); } catch { }

                        MessageBox.Show("Something went wrong during pilot rename (will try to rollback changes)!");
                    }
                    finally
                    {
                        // Make sure list is current.
                        CheckAndUpdatePilots();
                    }
                }
            }
        }

        private bool IsElevated
        {
            get
            {
                bool isElevated = false;
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                return isElevated;
            }
        }

        private void PilotMakeActiveButton_Click(object sender, EventArgs e)
        {
            if (IsOverloadOrOlmodRunning) return;

            string pilotSelected = null;
            bool select = (PilotsListBox.SelectedIndex >= 0);

            if (select)
            {
                pilotSelected = (string)PilotsListBox.Items[PilotsListBox.SelectedIndex];
                string pilotCurrent = CurrentPilot;
                if (pilotSelected.ToLower() == pilotCurrent.ToLower()) return;
            }

            // Overload is not running - safe to set active pilot?
            CurrentPilot = pilotSelected;
            CheckAndUpdatePilots();
        }

        private string BackupAllPilots()
        {
            string timeStamp = DateTime.Now.ToString("yyyy:MM:dd_HH:mm:ss").Replace(":", "");
            string zipName = Path.Combine(pilotsBackupPath, "Overload_Pilots_" + timeStamp + ".zip");

            Directory.CreateDirectory(pilotsBackupPath);

            using (var fileStream = new FileStream(zipName, FileMode.Create))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    foreach (string pilot in OverloadPilots)
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
                            using (var zipStream = zipArchiveEntry.Open()) zipStream.Write(buffer, 0, buffer.Length);

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

        public byte[] AddByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }

        private string CurrentPilot
        { 
            get
            {
                string pilotName = "";

                using (var hklm = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (var key = hklm.OpenSubKey(@"SOFTWARE\Revival\Overload"))
                    {
                        if (key != null)
                        {
                            byte[] pilotBytes = (byte[])key.GetValue(@"pilot_h187966731");
                            if (pilotBytes.Length > 0) pilotName = Encoding.ASCII.GetString(pilotBytes, 0, pilotBytes.Length -1);
                        }
                    }
                }

                return pilotName;
            }

            set
            {
                byte[] pilotBytes = Encoding.ASCII.GetBytes((value + '\0').ToCharArray());

                using (var hklm = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (var key = hklm.OpenSubKey(@"SOFTWARE\Revival\Overload", true))
                    {
                        if (key != null)
                        {
                            key.SetValue(@"pilot_h187966731", pilotBytes);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Let user adjust XP value (but only if Overload/Olmod aren't running).
        /// </summary>
        private void ValidateSetXp()
        {
            if (PilotsListBox.SelectedIndex >= 0)
            {
                PilotXPTextBox.Enabled = true;

                // Indicate invalid value using foreground color.
                bool ok = false;
                try
                {
                    int xp = Convert.ToInt32(PilotXPTextBox.Text);
                    if ((xp >= 0) && xp <= 9999999) ok = true;
                }
                catch
                {
                }

                // Set color
                PilotXPTextBox.ForeColor = (ok) ? activeTextBoxColor: inactiveTextBoxColor;

                // Enable button only if value is OK and Olmod/Overload aren't running.
                PilotXPSetButton.Enabled = ok && !IsOverloadOrOlmodRunning;
            }
            else
            {
                // No pilot selected.
                PilotXPTextBox.Enabled = false;
                PilotXPSetButton.Enabled = false;
            }
        }

        private void GetPilotXP(string pilotName)
        {
            if (String.IsNullOrEmpty(pilotName))
            {
                PilotXPTextBox.Enabled = false;
                PilotXPSetButton.Enabled = false;
                return;
            }

            try { Directory.CreateDirectory(pilotsPath); } catch { }

            try
            {
                string xprefsFileName = Path.Combine(pilotsPath, pilotName + ".xprefs");
                string xprefs = File.ReadAllText(xprefsFileName);
                string[] xprefsList = xprefs.Split(";".ToCharArray(), StringSplitOptions.None);
                for (int i = 0; i < xprefsList.Length; i++)
                {
                    if (xprefsList[i].StartsWith("PS_XP2:")) PilotXPTextBox.Text = xprefsList[i].Split(":".ToCharArray(), StringSplitOptions.None)[1];
                }
            }
            catch
            {
                PilotXPTextBox.Text = "???";
            }
        }

        private void SetPilotXP(string pilotName)
        {
            if (String.IsNullOrEmpty(pilotName))
            {
                PilotXPTextBox.Enabled = false;
                PilotXPSetButton.Enabled = false;
                return;
            }

            try
            {
                bool update = false;

                string xprefsFileName = Path.Combine(pilotsPath, pilotName + ".xprefs");
                string xprefs = File.ReadAllText(xprefsFileName);
                string[] xprefsList = xprefs.Split(";".ToCharArray(), StringSplitOptions.None);
                for (int i = 0; i < xprefsList.Length; i++)
                {
                    if (xprefsList[i].StartsWith("PS_XP2:"))
                    {
                        string[] xprefsFields = xprefsList[i].Split(":".ToCharArray(), StringSplitOptions.None);

                        if (xprefsFields[1] != PilotXPTextBox.Text)
                        {
                            xprefsList[i] = xprefsFields[0] + ":" + PilotXPTextBox.Text + ":" + xprefsFields[2];
                            update = true;
                        }
                    }
                }

                if (update)
                {
                    string text = "";
                    for (int i = 0; i < xprefsList.Length; i++)
                    {
                        text += String.IsNullOrEmpty(text) ? "" : ";";
                        text += xprefsList[i];
                    }

                    File.WriteAllText(xprefsFileName, text);
                }

            }
            catch
            {
                MessageBox.Show($"Cannot update XP for pilot {pilotName}!", "Error");
            }

            ValidateSetXp();
        }

        private void PilotXPSetButton_Click(object sender, EventArgs e)
        {
            CheckAndUpdatePilots();

            if (PilotsListBox.SelectedIndex >= 0)
            {
                string pilotSelected = (string)PilotsListBox.Items[PilotsListBox.SelectedIndex];
                SetPilotXP(pilotSelected);
            }
            else
            {
                GetPilotXP(null);
            }
        }
    }
}