using HarmonyLib;
using UnityEngine;
using SFS.IO;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;
using System.Text.RegularExpressions;

namespace ModLoader
{
    /// <summary>
    /// This is the main class of ModLoader. this class is injected into the game with Unity Doorstop injector.
    /// </summary>
    public class Loader : MonoBehaviour
    {

        // This save Loader instance
        public static Loader main;
        private Console _console;

        // This save the gameObject that implement Loader class
        public static GameObject root;

        private static FolderPath _modsFolder;

        private const string modLoderVersion = "v1.1.0";

        public static FolderPath ModsFolder
        {
            get
            {
                return _modsFolder;
            }
        }

        private List<string> _modsLoaded;

        // List of all mods loaded in the folder MODS
        private Dictionary<string, SFSMod> _modList;

        /// <summary>
        /// first method executed. Save loader instance in static var and suscribe to scene event. 
        /// </summary>
        private void Awake()
        {
            Loader.main = this;
            this.suscribeOnChangeScene(this.OnSceneLoaded);
            _modsFolder = FileLocations.BaseFolder.Extend("MODS").CreateFolder();
        }

        /// <summary>
        /// This method start to read mods and is executed automatically when this class is created after Awake method. 
        /// </summary>
        private void Start()
        {
            Debug.Log("Reading mods");
            this._modList = this.getModList();
            this._modsLoaded = new List<string>();
            
            foreach (SFSMod mod in this._modList.Values)
            {
                if(this._modsLoaded.Contains(mod.ModId))
                {
                    continue;
                }

                try
                {
                    this.loadMod(mod);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            //this.postloading();
            Debug.Log("All Ready");
        }

        /*private void postloading()
        {
            this._console = Console.root.GetComponent<Console>();
            this._console.ConsoleGui.Mods = this.getMods();
            this._console.ConsoleGui.SceneGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        }*/

        //this._consoleGui.Mods = Loader.main.getMods();
        //Loader.main.suscribeOnChangeScene(this.OnSceneLoaded);

        /// <summary>
        /// get mod instance.
        /// </summary>
        /// <param name="modId">Mod ID you need</param>
        /// <returns>instance mod</returns>
        public SFSMod getMod(string modId)
        {
            return this._modList[modId];
        }

        public SFSMod[] getMods()
        {
            List<SFSMod>mods = new List<SFSMod>();
            foreach(SFSMod mod in this._modList.Values)
            {
                mods.Add(mod);
            }
            return mods.ToArray();
        }

        /// <summary>
        ///     This method read MODS folder and identify what is the entry point for each folder.
        /// </summary>
        /// <returns> dictionary whit modid as key and SFSMod instance as value</returns>
        private Dictionary<string, SFSMod> getModList()
        {
            Dictionary<string, SFSMod> modList = new Dictionary<string, SFSMod>();

            // get list of mod Folders in MODS folder
            IEnumerable<FolderPath> modsFolders = _modsFolder.GetFoldersInFolder(false);
            string basePath = Path.Combine(FileLocations.BaseFolder, "MODS");

            foreach (FolderPath folder in modsFolders)
            {
                string fileModPath = Path.Combine(basePath, folder.FolderName, folder.FolderName + ".dll");
                try
                {
                    // get mod assembly
                    Assembly assembly = Assembly.LoadFrom(fileModPath);
                    SFSMod mod = null;

                    // verfiy if hace SFSMod class
                    foreach (Type typeClass in assembly.GetTypes())
                    {
                        if (typeClass.IsSubclassOf(typeof(SFSMod)))
                        {
                            mod = (Activator.CreateInstance(typeClass) as SFSMod);
                            break;
                        }

                    }

                    if (mod == null)
                    {
                        throw new Exception(folder.FolderName+" entry point not found");

                    }

                    if (modList.ContainsKey(mod.ModId))
                    {
                        throw new Exception("Already existe another mod whit id "+mod.ModId);
                    }

                    modList.Add(mod.ModId, mod);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

            }
            return modList;
        }

        /// <summary>
        /// Load mod dependecies and chec if is already loaded and check their version.
        /// </summary>
        /// <param name="dependencies"> list of dependecies that you need to load first</param>
        /// <returns> true if all dependecies have been loaded </returns>
        private bool loadDependencies(Dictionary<string, string[]> dependencies)
        {
 
            // for each mod dependencie
            foreach (var item in dependencies)
            {
                // exist mod in the list?
                if (this._modList.ContainsKey(item.Key))
                {
                    // get mod dependecie
                    SFSMod dependencieMod = this._modList[item.Key];

                    // verify if the dependencie version is the same that mod need
                    bool versionFlag = false;
                    foreach(string version in item.Value)
                    {
                        if(verifyVersion(dependencieMod.Version, version))
                        {
                            versionFlag = true;
                            break;
                        }
                    }

                    // the version is valid
                    if (versionFlag)
                    {
                        // start load dependencie first
                        this.loadMod(dependencieMod);
                        continue;
                    }
                }
                // dependencie not exist or is diferent version
                throw new Exception("Is necesary install " + item.Key +" "+ string.Join(", ", item.Value));
            }
            return true;
        }

        /// <summary>
        /// start to check version mod and run load method
        /// </summary>
        /// <param name="mod">mod to start load</param>
        private void loadMod(SFSMod mod)
        {
            // this mod has been load?, if not start load
            if (this._modsLoaded.Contains(mod.ModId)) 
            { 
                return;
            }

            Debug.Log("Loading " + mod.Name);
            this._modsLoaded.Add(mod.ModId);

            // check if the version is valid for this modloader version
            if (verifyVersion(mod.ModLoderVersion, modLoderVersion))
            {
                // Have dependencies?
                if(mod.Dependencies != null)
                {
                    // load them
                    this.loadDependencies(mod.Dependencies);
                }
                
                mod.loadAssets();
                mod.load();
                return;
            }

            // the mod loader version is not valid
            throw new Exception(mod.Name + " need ModLoader " + mod.Version);
        }

        /// <summary>
        /// check two versions string to identify if are the same
        /// </summary>
        /// <param name="version1"> version to check</param>
        /// <param name="version2"> verison to check</param>
        /// <returns>true if are valid versions</returns>
        private bool verifyVersion(string version1, string version2)
        {
            Regex rx = new Regex(@"\bv([0-9]|[1-9][0-9]).([0-9]|[1-9][0-9]|x).([0-9]|[1-9][0-9]|x)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // have the formal v1.x.x
            if (rx.IsMatch(version1) && rx.IsMatch(version2))
            {
                string[] modVersion = version1.Split('.');
                string[] target = version2.Split('.');

                if (modVersion.Length == target.Length)
                {
                    for (short index = 0; index < target.Length; index++)
                    {
                        if (modVersion[index] == "x")
                        {
                            continue;
                        }
                        if (modVersion[index] != target[index])
                        {
                            return false;
                        }
                    }
                    // have the format and is valid version for this modloader version
                    return true;

                }

            }
            return false;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene change to "+scene.name);
            /*if (this._console)
            {
                this._console.ConsoleGui.SceneGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            }*/
           
        }

        /// <summary>
        /// suscribe to scene change event
        /// </summary>
        /// <param name="method">method what do you want to suscribe</param>
        /// <returns>true if subscribed</returns>
        public bool suscribeOnChangeScene(UnityAction<Scene, LoadSceneMode> method)
        {
            SceneManager.sceneLoaded += method;
            return true;
        }


        /// <summary>
        /// This is the mod loader entry point, this is the method execute after be injected in the game
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Harmony patcher = new Harmony("SFS.mod.loader");
            patcher.PatchAll();
        }
    }

}
