# SFS Modloader
_Simple modloader for Space Flight Simulator PC version. MacOS fixes by pixelgaming579._

## English
### How to install SFS Modloader 🚀

Go to [releases](https://github.com/dani0105/SFS-Modloader/releases) and download `Installer.rar` modloader version you need. Once decompressed, follow the next steps.

1. First, copy "doorstop_config.ini" and "winhttp.dll" into "Spaceflight Simulator Game" folder next to "Spaceflight Simulator.exe".  
  ![Imgur](https://i.imgur.com/tXfPMNY.png)  
  This files is used to inject modloader code into unity game. the code for these files can be found at [this repo](https://github.com/NeighTools/UnityDoorstop)  
2. Copy "0Harmony.dll" and "ModLoader.dll" from "Managed" folder to "Spaceflight Simulator_Data/Managed"  
  ![Imgur](https://i.imgur.com/26JJeb7.png)  
  "0Harmony.dll" is used to inject code into game in runtime and "ModLoader.dll" is used to read and execute mods.  
3. Create "MODS" folder into "Spaceflight Simulator_Data"  
  ![Imgur](https://i.imgur.com/gQtjemY.png)  
  "MODS" folder is used to store mod dll files.  

### How to install mods
First of all, the mod need be compatible with this modloader and be on the same version of the game. To install all you need to do is copy mod .dll  file into "MODS" folder in "Spaceflight Simulator_Data".
Once the dll files are placed, you just need to execute game normally and you will see a black screen appear on your screen, that is the _modloader console_, at the moment is not possible to close.  
![Imgur](https://i.imgur.com/JYBMvtD.png)  

### How to develop mods
You will need `Visual Studio` and `Space Flight Simulator PC version` installed. In `Visual Studio` you need to create a new "class library(.NET Framework)" project with the name of your mod.
Next, add in `References` the next files: `Assembly-CSharp.dll`, `UnityEngine.CoreModule.dll`, `UnityEngine.dll` and `Modloader.dll`. These files are the basic, but you can add more if you need, for example `0Harmony.dll` if you need to inject code.  
Now, in your main class and entry point, you need `SFSMod` interface, this is used for modloader to identify what is your entry point and run the load method of your mod. With that you can start developing mods, also you will need `dnSpy` to see game code into `Assembly-CSharp.dll` file.

### How to develop new features in modloader
You will need `Visual Studio` and `Space Flight Simulator PC version` installed. First of all, fork this repository and clone, then in the `Dependecies` folder you need to put `Assembly-CSharp.dll`,`UnityEngine.CoreModule.dll` and `UnityEngine.dll`, 
these files you can find in "Spaceflight Simulator_Data/Managed". With that you can start developing new features. Once you have your new feature, you can create pull request on this repository.  

## Español
### Como instalar SFS Modloader 🚀
Ve a las versiones del mod [aquí](https://github.com/dani0105/SFS-Modloader/releases) y descargue el archivo `Installer.rar` de la versión que desee. Una vez descomprimido, siga los siguientes pasos:
1. Primero, copie "doorstop_config.in" y "winhttp.dll" dentro de la carpeta "Spaceflight Simulator Game" junto a "Spaceflight Simulator.exe".  
  ![Imgur](https://i.imgur.com/tXfPMNY.png)  
  Estos archivos son usados para inyectar el código del mod loder dentro del juego en unity. El código de este archivo lo puedes encontrar en este [this repo](https://github.com/NeighTools/UnityDoorstop)  
2. Copie "0Harmony.dll" y "ModLoader.dll" de la carpeta "Managed" a "Spaceflight Simulator_Data/Managed"  
   ![Imgur](https://i.imgur.com/26JJeb7.png)  
  "0Harmony.dll" es usado para inyectar código dentro del juego en tiempo de ejecución y "ModLoader.dll" es usado para leer y ejecutar los mods.
3. Cree la carpeta "MODS" dentro de "Spaceflight Simulator_Data"  
  ![Imgur](https://i.imgur.com/gQtjemY.png)  
  La carpeta "MODS" es usada para almacenar los archivos dll de los mods 

### Como instalar mods
Primero que todo, el mod necesita ser compatible con el modloader y estar en la misma versión del juego. Para instalar todo lo que necesitas es copiar el archivo .dll del mod dentro de la carpeta "MODS" en "Spaceflight Simulator_Data". Una vez colocado los archivos dll solo necesitas ejecutar el juego normalmente y veras una pantalla negra aparecer en tu pantalla, eso es la _ consola de modloader_, que por el momento no es posible cerrar.  
![Imgur](https://i.imgur.com/JYBMvtD.png)  

### Como desarrollar mods
Vas a necesitar `Visual Studio` y `Space Flight Simulator versión de steam` instalado. En `Visual Studio` vas a crear un nuevo proyecto del tipo "librería de clases(.NET Framework)" con el nombre de tu mod. Seguido, agrega en `Referencias` los siguientes archivos: `Assembly-CSharp.dll`, `UnityEngine.CoreModule.dll`, `UnityEngine.dll` y `Modloader.dll`. Esos archivos son los básicos, pero puedes agregar más si lo necesitas, por ejemplo `0Harmony.dll` si necesitas inyectar código. Ahora, en tu clase principal y punto de entrada, vas a necesitar implementar la interfaz `SFSMod`, esta interfaz es usada por el Modloader para identificar cuál es tu punto de entrada y ejecutar el método load de tu mod. Con eso puede empezar a desarrollar mods, además vas a necesitar `dnSpy` para ver el código del juego dentro de `Assembly-CSharp.dll`.

### Como desarrollar nuevas características
Vas a necesitar `Visual Studio` y `Space Flight Simulator versión de steam` instalado. Primero que todo, Cree un fork del repositorio y clónalo en tu computadora, Entonces en la carpeta `Dependecies` va a necesitar poner los archivos `Assembly-CSharp.dll`, `UnityEngine.CoreModule.dll` y `UnityEngine.dll`, estos archivos los puedes encontrar en "Spaceflight Simulator_Data/Managed". Con eso puedes empezar a desarrollar nuevas características. Una vez que tengas tu nueva característica, puedes crear un Pull request en este repositorio.
