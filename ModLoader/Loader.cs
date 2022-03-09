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
using SFS.UI;
using SFS.Translations;
using SFS.Input;

namespace ModLoader
{
    /// <summary>
    /// This is the main class of ModLoader. this class is injected into the game with the Unity Doorstop injector.
    /// </summary>
    public class Loader : MonoBehaviour
    {

        // This save Loader instance
        public static Loader main;
        //private Console _console;

        // This save the gameObject that implement Loader class
        public static GameObject root;

        private static FolderPath _modsFolder;

        // modlaoder version
        private const string modLoderVersion = "v1.1.0";

        public static FolderPath ModsFolder
        {
            get
            {
                return _modsFolder;
            }
        }

        private List<string> _modsLoaded;

        // List of all mods loaded in the MODS folder 
        private Dictionary<string, SFSMod> _modList;

        /// <summary>
        /// first executed method. Save the loader instance to static var and subscribe to the scene event.
        /// </summary>
        private void Awake()
        {
            Loader.main = this;
            SceneManager.sceneLoaded += this.OnSceneLoaded;
            _modsFolder = FileLocations.BaseFolder.Extend("MODS").CreateFolder();
        }

        /// <summary>
        /// This method starts reading mods and runs automatically when this class is created after the Awake method.
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
            Debug.Log("Already");
        }    

        /// <summary>
        /// get mod instance.
        /// </summary>
        /// <param name="modId">Mod ID you need</param>
        /// <returns>instance mod</returns>
        public SFSMod getMod(string modId)
        {
            if (this._modList.ContainsKey(modId))
            {
                return this._modList[modId];
            }
            return null;
        }

        /// <summary>
        /// Get a list of all loaded mods
        /// </summary>
        /// <returns> loaded mods </returns>
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
        ///     This method reads the MODS folder and identifies what the entry point is for each folder.
        /// </summary>
        /// <returns> dictionary with modid as key and SFSMod instance as value</returns>
        private Dictionary<string, SFSMod> getModList()
        {
            Dictionary<string, SFSMod> modList = new Dictionary<string, SFSMod>();

            // get a list of mod folders in the MODS folder
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

                    // check if you have SFSMod class
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
                        throw new Exception("There is already another mod with id 12 " + mod.ModId);
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
        ///Load the mod dependencies and check if it is already loaded and check its version.
        /// </summary>
        /// <param name="dependencies"> lista de dependencias que necesitas cargar primero</param>
        /// <returns> verdadero si se han cargado todas las dependencias</returns>
        private bool loadDependencies(Dictionary<string, string[]> dependencies)
        {

            // for each mod dependency
            foreach (var item in dependencies)
            {
                // is there a mod in the list?
                if (this._modList.ContainsKey(item.Key))
                {
                    // get mod dependencies
                    SFSMod dependencieMod = this._modList[item.Key];

                    // check if the dependency version is the same as the one needed by the mod
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
                        // start load dependency first
                        this.loadMod(dependencieMod);
                        continue;
                    }
                }
                // dependency does not exist or is a different version
                throw new Exception("Is necesary install " + item.Key +" "+ string.Join(", ", item.Value));
            }
            return true;
        }

        /// <summary>
        /// start checking mod version and run load method
        /// </summary>
        /// <param name="mod">mod to start load</param>
        private void loadMod(SFSMod mod)
        {
            // has this mod been loaded?, if it does not start loading
            if (this._modsLoaded.Contains(mod.ModId)) 
            { 
                return;
            }

            Debug.Log("Loading " + mod.Name);
            this._modsLoaded.Add(mod.ModId);

            // check if the version is valid for this modloader version
            if (verifyVersion(mod.ModLoderVersion, modLoderVersion))
            {
                // Does it have dependencies?
                if (mod.Dependencies != null)
                {
                    // charge them
                    this.loadDependencies(mod.Dependencies);
                }
                
                mod.loadAssets();
                mod.load();
                return;
            }

            //mod loader version is invalid
            throw new Exception(mod.Name + " need ModLoader " + mod.Version);
        }

        /// <summary>
        /// check the string of two versions to identify if they are the same
        /// </summary>
        /// <param name="version1"> version to check</param>
        /// <param name="version2"> verison to check</param>
        /// <returns>true if they are valid versions</returns>
        private bool verifyVersion(string version1, string version2)
        {
            Regex rx = new Regex(@"\bv([0-9]|[1-9][0-9]).([0-9]|[1-9][0-9]|x).([0-9]|[1-9][0-9]|x)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // has the format v1.x.x
            if (rx.IsMatch(version1) && rx.IsMatch(version2))
            {
                string[] target1 = version1.Split('.');
                string[] target2 = version2.Split('.');

                if (target1.Length == target2.Length)
                {
                    for (short index = 0; index < target2.Length; index++)
                    {
                        if (target1[index] == "x")
                        {
                            continue;
                        }
                        if (target1[index] != target2[index])
                        {
                            return false;
                        }
                    }
                    //both are valid for each other
                    return true;

                }

            }
            return false;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene change to "+scene.name);           
        }

        /// <summary>
        /// This is the mod loader entry point, this is the method that is executed after being injected into the game.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Harmony patcher = new Harmony("sfs.mod.modloader");
            patcher.PatchAll();
        }
    }

}
