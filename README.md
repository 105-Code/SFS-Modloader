# SFS Modloader
_Simple modloader for Space Flight Simulator PC version._

## How to install SFS Modloader 🚀

Go to [releases](https://github.com/dani0105/SFS-Modloader/releases) and download `Installer.rar` version of modloader what do you need. Once decompressed, follow the next steps.

1. First, copy "doorstop_config.ini" and "winhttp.dll" into "Spaceflight Simulator Game" folder next to "Spaceflight Simulator.exe".
  ![Imgur](https://i.imgur.com/tXfPMNY.png)  
  This files is used to inject modloader code into unity game. the code from this files you can found in [this repo](https://github.com/NeighTools/UnityDoorstop) 
2. Copy "0Harmony.dll" and "ModLoader.dll" from "Managed" folder to "Spaceflight Simulator_Data/Managed"
  ![Imgur](https://i.imgur.com/26JJeb7.png)  
  "0Harmony.dll" is used to inject code into game in runtime and "ModLoader.dll" is used to read and execute mods.
3. Create "MODS" folder into "Spaceflight Simulator_Data"
  ![Imgur](https://i.imgur.com/gQtjemY.png)  
  "MODS" folder is used to store dll files from mods.

## How to install mods
First all, the mod need be compatible whit this modloader and be in the same game version. For install all you need to do is copy .dll mod file into "MODS" folder in "Spaceflight Simulator_Data".
once placed dll files you only need to execute game normaly, and you will see a black screen appear in your screen, that is the _modloader console_, for the moment is not possible close.
![Imgur](https://i.imgur.com/JYBMvtD.png)  

## How to develope mods
You will need `Visual Studio` and `Space Flight Simulator PC version` installed. In `Visual Studio` you need to create a new "class library(.NET Framework)" project whit the name of your mod.
Next, add in `References` the next files: `Assembly-CSharp.dll`, `UnityEngine.CoreModule.dll`, `UnityEngine.dll` and `Modloader.dll`. This files are the basic, but you can add more if you need, for example `0Harmony.dll` if you need inject code.
Now, in you main class and entry point, you need `SFSMod` interface, this is used for modloader to indetify what is your entry point and execute load method of your mod. whit that, you can start develope mods, also you will need `dnSpy` to see game code into `Assembly-CSharp.dll` file.

## How to develope new features in modloader
You will need `Visual Studio` and `Space Flight Simulator PC version` installed. First of all, fork this repository and clone, then in the `Dependecies` folder you need to put `Assembly-CSharp.dll`,`UnityEngine.CoreModule.dll` and `UnityEngine.dll`, 
this files you can found in "Spaceflight Simulator_Data/Managed". Whit that, you can start develop new features. once you have your new feature, you can create pull request in this repository and then i will see what do the changes and accept them.

