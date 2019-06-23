# Overload Client Tool Help 

- Last revised 2019-06-10 by Søren "Maestro" Michélsen
- This revision applies to OCT v1.8.5+

### About the author
I'm 57, married, no children but we have 4 cats. In my daytime I work as a Dynamics AX/.NET developer with focus on EDI and integration. When not working I spend my time doing photography, music (piano & guitar) and of course gaming. Overload is my favorite game as I love Descent-style games and used to play D1/D2/D3 when I was a bit younger (at that time my alias was 'Mick' playing on the SWM team).

Should you wish to contact you can reach me  on Discord, my ID is Maestro#4825.

### Acknowledgement
Thanks to everyone helping me with suggestions and bug reports while working on OCT with special thanks going to the following people:
- Arne
- Roncli
- Luponix
- Obi-Wan Kenobi
- sLLiK
- Tobias
- jazzyjet
- terminal
- Members and friends of the SWM team:
  - Dreawus
  - JoBo-One
  - Hiflier
  - Zhrothgar
  - KAHA
  - Mr. Aesthetic

### Introduction

[Overload Client Tool](https://github.com/maestrodk/OCT) is a UI frontend for [Revival Productions](https://www.revivalprod.com/) superb 6-DOF shooter [Overload](https://playoverload.com/). You don't need OCT to play Overload but it offers several benefits that isn't a part of the original game.

OCT was created to make it easier to maintain maps, use Olproxy + Olmod and whatever else is needed to get the most out of Overload. 


Note that originally there also was a Overload _Server_ Tool (OST) also written by the same author. But both tools have since merged into a single tool (OCT). So don't get misled by the 'client' part, OCT does everything OST did (and some more :)). 

#### Main features

- Easy-to-use frontend for playing Overload / hosting an Overload server (or both)
- Support for [Olproxy](https://github.com/arbruijn/olproxy) (by Arne de Bruijn):
	- Built-in Olproxy code base (standalone Olproxy is not required)
	- Support for external standalone Olproxy application
	- Olproxy may be turned on/off at any time
	- Olproxy may be switched from embedded / external at any time
	- When server is running Olproxy can optionally send info to the online tracker
- Support for [Olmod](https://github.com/arbruijn/olmod) (by Arne de Bruijn):
	- Auto-update Olmod to latest release from within OCT
	- Overload startup parameters sent to Olmod when launched
	- Option to use **-gamedir** option to tell Overload where Olmod
	- Option to use **-frametime** option to enable Olmod FPS feature
- Overload server support:
	- Run a dedicated Overload server
	- Run an Overload server + an Overload client is supported
	- Optionally send server info to online tracker
	- Start/stop Overload server indepently of a Overload client
- Map management:
	- Uses public MP map list at [Overload Maps](https://www.overloadmaps.com/) download maps
	- Optionally auto-update maps when OCT starts
	- Optionally auto-hide maps not in the public MP map list (avoids map version issues)
	- Optionally update only existing local msps
	- Supports map located in either Overload DLC folder or Overload ProgrammData folder
	- Optionally move maps from Overload DLC folder <=> Overload ProgramData folder
	- Hide/unhide any map
	- Delete maps
- Pilot management:
	- View/clone/delete/rename Overload pilots
	- View/set any pilots XP 
	- Backup pilots (with option to auto-backup pilots when launching Overload)
	- Set active pilot (this requires OCT to be started with administrative rights)
- Color themes (currently 7 themes to choose from - more to come)
- Small footprint and every feature just 1-2 mouseclicks away


### Some information for the individual tabs in OCT

Below you'll find a more detailed description of each of the different tabs shown in OCT:
- Main
- Maps
- Pilots
- Overload
- Olproxy
- Olmod
- Server
- Options

#### Main

The Main tab is where you start/stop Overload (using the `Start` button). It is also where you view a running log of all actions. Especially when using Olproxy the log shows any interaction being exchange between the local Olproxy running on your computer and a remote Olproxy used by a remote server or remote clients. 

When OCT starts it will display the version of OCT itself but also the version of the embedded Olproxy and Olmod. If Olmod auto-update is enabled you will also see a message in case Olmod gets updated. 

_Note that if a log entry is longer than can be shown in the log window hovering the mouse over the entry will display the complete text._

At the bottom you can always see the running status of Overload/Olmod and Olproxy (if running as a client). The `Start` button will change to either `Stop` if Overload/Olmod is running. 

Note that this only pertains to a client Overload/Olmod - if you're running a server Overload/Olmod at the same time that is a separate Overload instance that is controlled using the `Server` tab.

Also note that when you chose to exit OCT (using the `Exit` button) _all_ running instances of Overload/Olmod/Olproxy will be automatically shut down.

#### Maps

Here you can see all maps installed in either the Overload DLC or the Overload ProgramData folder. To see information on a specific map just hover the mouse over the entry in the listbox to view the map ZIP filename, size and location.

The `Hide` button will change between 'Hide' and 'Unhide' depending whether the selected map is hidden or not. Hiding a map (using the `Hide` button) is useful to limit the maps visible to Overload as you then can find your favorite maps faster (less clicking!). 

When auto-updating there is an option to automatically hide local maps not included in the official map list, this is to avoid Overload getting confused in case more than one ZIP file contains the same map. It strongly recommended to enable this option!

The `Refresh` button will download a fresh copy of selected map. Use this if the local map ZIP file has been corrupted. This button is only selectable if the map is included on the official map list.

Per default only MP maps are downloaded but you can choose to also include SP (single player) and CM (challenge missions) when updating maps. Please note that SP/CM maps can be quite big and downloading these maps can take some time. OCT will ask you if you want the JSON URL to be changed to include all map types (here you should chose 'Yes' or otherwise only MP maps will be updated).

When you toggle the `Use DLC folder for downloaded maps` OCT will ask you if you want to move all maps between the ProgramData folder or the DLC folder (the move depending on what the option is set to). You don't have to do this as OCT will check ZIP files in both folders when updating maps and show maps found in either folder in the listbox. If the same map ZIP is stored in _both_ folders OCT will try its best to avoid confusion (like hiding/unhiding both maps in tandem etc).

When during updates OCT works like this:

- First the official list of MP maps is retrieved
- Then the ProgramData folder and the DLC Folder is checked for local map ZIP files:
	- If online map is newer it is marked for download
	- If online map isn't found and OCT is setup to hide unofficial maps the local map ZIP file is hidden
	- If other local map ZIP files contains an MP file with the same name the local map ZIP file is 'hidden'

Note that SP (single player) and CM (Challenge Mission) maps are _never_ auto-hidden when maps are updated. But you can select to hide/unhide these manually if you want.

_The DLC folder is a subfolder to the main Overload game folder._

_The ProgramData folder would normally be C:\ProgramData\Revival\Overload._

_A hidden map have '\_OCT\_Hidden' appended to its ZIP filename._


#### Pilots

Most of the options on the Pilots tab is available inside OCT but they are somewhat easier to use in OCT. There are two exception to this:

- OCT allows you to directly set a pilots XP 
- OCT allow you to create a ZIP'ed backup of all pilots (either manually or automatically when Overload launches)

To set a pilots XP simply select the pilot and enter a value in the `Pilot XP`field and then click `Set XP`. Note that this option is only available when Overload isn't running to prevent any conflict.


#### Overload

When OCT launches the first time or if the path set in `Overload Application` isn't valid OCT will try to locate your Overload installation. It does this by looking in the Windows registry for a STEAM, DVD og GOG (Good Old Games) registry key. If found the path to Overload.exe is set automatically and then OCT tries to also locate Olmod in the same folder where Overload.exe was found.

If Overload cannot be automatically located you must tell OCT where it is. To do this simply doubleclick the `Overload Application` field and navigate to + select Overload.exe. 

Likewise if Olmod cannot be located you must tell OCT where it is installed. Or you can go to the Olmod tab and tell OCT to download and install it (then it will be installed into the Overload game folder).

The `Search` button at the bottom of the Overload tab does the same auto-search as when OCT first runs.

The `Overload Startup Parameters` will be auto-set on first run of OCT. After that you can change this field to whatever you like. Note that OCT does some sanity checking when using this field as some parameters are server-only and others doesn't make much sense if user for an Overload client. If using Olmod then the paramters set here will be added to then parameters setup for Olmod.

#### Olproxy

[Olproxy](https://github.com/arbruijn/olproxy) is a LAN-to-Internet proxy created by Arne de Bruijn. In a nutshell it allows you to connect to a 'LAN' server that is hosted online. To use the feature simply enabled Olproxy, start Overload and then start a LAN game where you enter the remote servers IP address or domain name in the password field. The remote Olproxy (on the Internet server) and your local Olproxy then talk together to connect your Overload client with the remote Overload server. From there on Olproxy doesn't interfere with the actual gameplay.

Use the settings provided in this tab to tell OCT if and how to use Olproxy. If you want to use the external standalone Olproxy application you must manually download and install this as OCT does not (currently) have a feature to do this (that may come in the future). You only need to do this if you don't want to use the Olproxy embedded into OCT of course. The tab has a link to Olproxy on Github.

Setting the path to the external Olproxy.exe is done in the same way as setting the path to Overload and Olmod - simply doubleclick the `External Olproxy Application`field and select the Olproxy.exe file.

When starting a Overload server OCT will configure Olproxy to use the server settings setup on the Server tab. If needed OCT will start/stop Olproxy temporarily to set Olproxy so that server information is only sent if the server is running and if set to do so according to the settings on the Server tab. If you are only running a Overload client (or if you stop the server) then Olproxy will switch to use 'client' mode, e.g. no information will be sent to the tracker.


#### Olmod

Generally using Olmod is always recommended except when you want to do a CM and want your public score to be updated - or if you are joining a server game where the use of Olmod is prohibited.

If you have Olmod installed to your Overload game folder then OCT will find it automatically. Otherwise you have to doubleclick the `Olmod Application` field and select Olmod.exe where you have installed it - also you must select the option `Tell Olmod where Overload is installed (using -gamedir option)` - otherwise Olmod will be unable to run as it needs the Overload game files.

When running a server OCT will include/exclude the settings available on this tab as needed (e.g. `Show FPS` is never used for the server).


#### Server

_Note: You need to make sure your firewall is setup to allow incoming traffic on UDP ports 7000-8001!_

On this tab you tell OCT  how to run an Overload server. Enter whatever you like into the fields `Server Name` and `Information about your server` and select if you want to inform the tracker about your server (check the `Make server visible on tracker` and `Remove inactive server from tracker`).

_The option `Start at Windows login` was also in the original OST (Overload Server Tool). It isn't very useful and currently only included for 'historical reasons'. In a future version of OCT it will probably be replaced by a new mechanism that can auto-start an  Overload server automatically when the computer starts without requiring a user to login to Windows at all._


#### Options

This tab contains options that pertains to OCT itself. As the options available here are self-explanatory there isn't much additional info to convey - except I suggest not using the `Party Mode` if you are epileptic :)
