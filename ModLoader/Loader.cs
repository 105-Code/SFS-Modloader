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
    /// This is the main class of ModLoader. this class is injected into the game whit Unity Doorstop injector.
    /// </summary>
    public class Loader : MonoBehaviour
    {

        // This save Loader instance
        public static Loader main;

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

        // List of all mods loaded in the folder MODS
        private SFSMod[] _modList;
        /*public SFSMod[] ModList
        {
            get
        }*/

        private void Awake()
        {
            Loader.main = this;
            this._modList = new SFSMod[] { };
            this.suscribeOnChangeScene(this.OnSceneLoaded);
            _modsFolder = FileLocations.BaseFolder.Extend("MODS").CreateFolder();
        }

        private void Start()
        {
            IEnumerable<FolderPath> modsFolders =  _modsFolder.GetFoldersInFolder(false);
            string basePath = Path.Combine(FileLocations.BaseFolder, "MODS");
            Debug.Log("Reading Mods");
            Debug.LogError(new Exception("Error custom"));

            foreach (FolderPath folder in modsFolders)
            {
                string fileModPath = Path.Combine(basePath, folder.FolderName, folder.FolderName+".dll");
                try
                {
                    SFSMod mod = this.loadMod(fileModPath);
                    this._modList.AddItem(mod);
                    Debug.Log(mod.Name+" Loaded");
                }
                catch(Exception e)
                {
                    Debug.LogError("Error Reading "+folder.FolderName+" folder");
                    Debug.LogError(e);
                }
                
            }
            
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene change to "+scene.name);
        }

        public bool suscribeOnChangeScene(UnityAction<Scene, LoadSceneMode> method)
        {
            SceneManager.sceneLoaded += method;
            return true;
        }

        private SFSMod loadMod(string path)
        {
            Assembly assembly = Assembly.LoadFrom(path);
            SFSMod mod = null;

            foreach (Type typeClass in assembly.GetTypes())
            {
                if (typeClass.IsSubclassOf(typeof(SFSMod)))
                {
                    mod = (Activator.CreateInstance(typeClass) as SFSMod);
                    break;
                }
                
            }

            if(mod == null)
            {
                throw new Exception("SFSmod class not found");
            }

            if (verifyModLoaderVersion(mod.ModLoderVersion))
            {
                mod.loadAssets();
                mod.load();

                return mod;
            }
            throw new Exception(mod.Name+" need ModLoader " + mod.Version);
        }

        /// <summary>
        /// get if the mod have a valid version format and valid version for this modloader version
        /// </summary>
        /// <param name="version"> mod version</param>
        /// <returns>true if valid version</returns>
        private bool verifyModLoaderVersion(string version)
        {
            Regex rx = new Regex(@"\bv([0-9]|[1-9][0-9]).([0-9]|[1-9][0-9]|x).([0-9]|[1-9][0-9]|x)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // have the formal v1.x.x
            if (rx.IsMatch(version))
            {
                string[] modVersion = version.Split('.');
                string[] target = modLoderVersion.Split('.');
                
                if(modVersion.Length == target.Length)
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
