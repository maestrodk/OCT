# Overload Client Tool

OCT is a Windows GUI front end for Overload

### Features

- Embedded Olproxy (standalone Olproxy.exe is not needed)

- Optionally use Olmod.exe to run Overload (both auto/manual update)

- Supports Olmod '-gamedir' argument

- Auto/manual update OCT feature

- Search for Olmod and standalone Olproxy feature 

- Optionally use standalone Olproxy.exe (with switch-on-the-fly)

- Ensures that only one instance of Olproxy is running

- Ensures that only one instance of Overload.exe is running

- Map manager to auto/manually update maps (with an option to only update existing maps)

- Map manager can auto-hide unofficial maps (prevents Overload from getting confused)

- Supports both Overload Application Data folder and Overload DLC folder for maps

- Supports moving of maps between Application Data folder and Overload DLC folder

- Maps can be hidden

- Pilot manager to clone/delete/rename/backup pilots

- Pilot XP feature to set XP to any value between 0..999999

- Many different color themes to choose from

#### Notes:

- Debug logs are saved to C:\Users\<USER>\AppData\Local\OverloadClientTool\Debug

- Pilot backups (zipped) are stored in C:\Users\<WindowsUserName>\AppData\LocalLow\Revival\Overload\Pilot Backup

- Maps can be stored either in C:\ProgramData\Revival\Overload or in the DLC subfolder in your Overload installation folder (from version 1.6+ maps can be placed in either folder)

- Maps are hidden by appending "_OCT_Hidden" to their ZIP file name

- Pilot list will automatically be updated (if changed on disk outside of OCT) after a few seconds
