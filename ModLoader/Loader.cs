using SFS;
using HarmonyLib;
using UnityEngine;
using SFS.IO;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using SFS.Builds;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace ModLoader
{
    /// <summary>
    /// This is the main class of ModLoader. this class is injected into the game with Unity Doorstop injector.
    /// </summary>
    public class Loader : MonoBehaviour
    {


        // This is de main logger, use this to show message in console
        public static ModConsole logger;

        private static Harmony patcher;

        // This save Loader instance
        public static Loader modLoader;

        // This save the gameObject that implement Loader class
        public static GameObject root;
        public static BaseAssigner baseAssigner;

        // List of all mods loaded in the folder MODS
        private SFSMod[] modList;

        public Loader()
        {
            Loader.logger = new ModConsole();
            Loader.modLoader = this;
        }

        public void Awake()
        {
            this.modList = new SFSMod[] { };
            Loader.logger.log("Loading Mods", "ModLoader");
            
            // If not existe is created and pass FolderPath
            FolderPath modFolder = FileLocations.BaseFolder.Extend("MODS").CreateFolder();
            
            // get list of files into MODS folder
            IEnumerable<FilePath> files = modFolder.GetFilesInFolder(false);
            foreach (FilePath file in files)
            {
                // get only dll files in mods folder
                if (file.Extension == "dll")
                {
                    Loader.logger.log("Reading " + file.FileName, "ModLoader");
                    // try to load mod
                    this.loadMod(modFolder.GetRelativePath(file.FileName) + "/" + file.FileName);
                }
            }
        }

        public SFSMod[] getModList()
        {
            return this.modList;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public bool suscribeOnChangeScene(UnityAction<Scene, LoadSceneMode> method)
        {
            SceneManager.sceneLoaded += method;
            return true;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            logger.log("scene change to "+ scene.name, "ModLoader");
        }

        private void loadMod(string path)
        {
            Assembly assembly = Assembly.LoadFrom(path);
            SFSMod mod = null;
            logger.log("Searching SFSMod interface", "ModLoader");
            // we search SFSMod interface in dll file dounede in MODS folder
            foreach (Type typeClass in assembly.GetTypes())
            {
                Type inteface = typeClass.GetInterface(typeof(SFSMod).Name);
                if(inteface == null)
                {
                    continue;
                }
                mod = (Activator.CreateInstance(typeClass) as SFSMod);                 
            }
            
            if (mod == null)
            {
                logger.log("File " + path + " don't have SFSMod interface","ModLoader");
                return;
            }

            Loader.logger.log("SFSMod interface found", "ModLoader");

            try
            {
                logger.log("Loading " + mod.getModName(), mod.getModAuthor());

                // execute entry point of mod
                mod.load();
                logger.log("Loaded " + mod.getModName(), mod.getModAuthor());
                this.modList.AddItem(mod);
            }
            catch( Exception e)
            {
                logger.log("Error loading " + mod.getModName(), mod.getModAuthor());
                logger.logError(e);
            }
        }

        /// <summary>
        /// This is the mod loader entry point, this is the method execute after be injected in the game
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Loader.patcher = new Harmony("SFS.mod.loader");
            Loader.patcher.PatchAll();
        }
    }

}
