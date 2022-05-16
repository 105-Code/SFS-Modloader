using HarmonyLib;
using UnityEngine;
using SFS.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;
using SFS.UI;
using SFS.Input;
using SFS.Translations;
using ModLoader.UI;
using ModLoader.Helpers;

namespace ModLoader
{
    /// <summary>
    ///     This is the main class of ModLoader. this class is injected into the game with the Unity Doorstop injector.
    /// </summary>
    public class Loader : MonoBehaviour
    {

        /// <summary>
        ///     This save Loader instance
        /// </summary>
        public static Loader main;

        /// <summary>
        ///     This save the gameObject that implement Loader class
        /// </summary>
        public static GameObject root;

        private static readonly Regex version_regex = new Regex(@"\bv([0-9]+)(\.([0-9]+|x)){2}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     Current Modlaoder version
        /// </summary>
        private const string modLoderVersion = "v1.3.1";

        /// <summary>
        ///     Get folder where mods are
        /// </summary>
        public static FolderPath ModsFolder
        {
            get { return FileLocations.BaseFolder.Extend("MODS"); }
        }

        private List<SFSMod> _modsLoaded = new List<SFSMod>();

        // List of all mods loaded in the MODS folder 
        private Dictionary<string, SFSMod> _modList;

        /// <summary>
        ///     First executed method. Save the loader instance to static var, subscribe to the scene event and load mods for early patches.
        /// </summary>
        private void Awake()
        {
            Loader.main = this;
            FolderPath modsFolder = ModsFolder.CreateFolder();

            Debug.Log($"--- ModLoader {modLoderVersion} ---");

            Debug.Log("Early loading mods");
            this._modList = this.getModList();

            foreach (SFSMod mod in this._modList.Values)
            {
                if (this._modsLoaded.Any(m => m.ModId == mod.ModId))
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

            Debug.Log("Early load finished");
            SceneHelper.OnHomeSceneLoaded += this.OnHomeSceneLoaded;
        }

        /// <summary>
        ///     Is called when Home scenes has been loaded
        /// </summary>
        /// <param name="sender">Who send this event</param>
        /// <param name="scene">Scene information</param>
        private void OnHomeSceneLoaded(object sender, Scene scene)
        {
            this.insertModsButton();
        }

        /// <summary>
        ///     This method starts reading mods and runs automatically when this class is created after the Awake method.
        /// </summary>
        private void Start()
        {
            Debug.Log("Starting mods");
            foreach (SFSMod mod in _modsLoaded)
            {
                mod.load();
            }
            this.insertModsButton();
        }

        private void insertModsButton()
        {
            GameObject modsButton = GameObject.Instantiate(GameObject.Find("Exit Button"));
            ButtonPC buttonPC = modsButton.GetComponent<ButtonPC>();
            TextAdapter textAdapter = modsButton.GetComponentInChildren<TextAdapter>();
            Destroy(modsButton.GetComponent<TranslationSelector>());
            modsButton.name = "Mods Button";
            textAdapter.Text = "MODS";// add text button

            //click events
            buttonPC.holdEvent = new HoldUnityEvent();
            buttonPC.clickEvent = new ClickUnityEvent();
            buttonPC.clickEvent.AddListener(delegate (OnInputEndData data) {
                ModsMenu.Main.Open();
            });

            // screen position
            modsButton.transform.SetParent(GameObject.Find("Buttons").transform);
            buttonPC.transform.localScale = new Vector3(1,1,1);
        }

        /// <summary>
        ///     Get mod instance.
        /// </summary>
        /// <param name="modId">Mod ID you need</param>
        /// <returns>instance mod</returns>
        public SFSMod getMod(string modId)
        {
            foreach(SFSMod mod in this._modsLoaded)
            {
                if(mod.ModId == modId)
                {
                    return mod;
                }
            }
            return null;
        }

        /// <summary>
        ///     Get a list of all loaded mods
        /// </summary>
        /// <returns> Loaded mods </returns>
        public SFSMod[] getMods()
        {
            return this._modsLoaded.ToArray();
        }

        /// <summary>
        ///     This method reads the MODS folder and identifies what the entry point is for each folder.
        /// </summary>
        /// <returns> Dictionary with modId as key and SFSMod instance as value</returns>
        private Dictionary<string, SFSMod> getModList()
        {
            Dictionary<string, SFSMod> modList = new Dictionary<string, SFSMod>();

            // find if the user misplaced the dll file
            this.detectIndividualDlls();

            // get a list of mod folders in the MODS folder
            IEnumerable<FolderPath> modsFolders = ModsFolder.GetFoldersInFolder(false);
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
                        if (typeof(SFSMod).IsAssignableFrom(typeClass))
                        {
                            mod = (Activator.CreateInstance(typeClass) as SFSMod);
                            break;
                        }
                    }

                    if (mod == null)
                    {
                        throw new Exception(folder.FolderName + " entry point not found");
                    }

                    if (modList.ContainsKey(mod.ModId))
                    {
                        throw new Exception("There is already another mod with id " + mod.ModId);
                    }
                    mod.ModFolder = Path.Combine(basePath, folder.FolderName);
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
        ///     Load the mod dependencies and check if it is already loaded and check its version.
        /// </summary>
        /// <param name="dependencies"> 
        ///     List of dependencies that need to be loaded first
        /// </param>
        /// <returns> 
        ///     True if all dependencies have been loaded
        /// </returns>
        private bool loadDependencies(Dictionary<string, string[]> dependencies)
        {
            // for each mod dependency
            foreach (var item in dependencies)
            {
                // is the dependency present
                if (this._modList.TryGetValue(item.Key, out var dependency))
                {
                    // check if the dependency version is the same as the one needed by the mod
                    bool versionFlag = false;
                    foreach (string version in item.Value)
                    {
                        if (verifyVersion(version, dependency.Version))
                        {
                            versionFlag = true;
                            break;
                        }
                    }

                    // the version is valid
                    if (versionFlag)
                    {
                        // start load dependency first
                        this.loadMod(dependency);
                        continue;
                    }
                }

                // dependency does not exist or is a different version
                throw new Exception("Is necesary install " + item.Key + " " + string.Join(", ", item.Value));
            }

            return true;
        }

        /// <summary>
        ///     Start checking mod version and run load method
        /// </summary>
        /// <param name="mod">
        ///     Mod to start load
        /// </param>
        private void loadMod(SFSMod mod)
        {
            // has this mod been loaded?, if it does not start loading
            if (this._modsLoaded.Any(m => m.ModId == mod.ModId))
            {
                return;
            }

            Debug.Log("Loading " + mod.Name);

            // check if the version is valid for this modloader version
            if (verifyVersion(mod.ModLoderVersion, modLoderVersion,true))
            {
                // Does it have dependencies?
                if (mod.Dependencies != null)
                {
                    // charge them
                    this.loadDependencies(mod.Dependencies);
                }

                mod.loadAssets();
                mod.early_load();
                this._modsLoaded.Add(mod);
                return;
            }

            //mod loader version is invalid
            throw new Exception(mod.Name + " need ModLoader " + mod.ModLoderVersion);
        }

        /// <summary>
        ///     Check the string of two versions to identify if they are the same
        /// </summary>
        /// <param name="targetVersion"> 
        ///     Version to check
        /// </param>
        /// <param name="currentVersion"> 
        ///     Verison to check
        /// </param>
        /// <param name="lessOrEqual">
        ///     Check if targetVersion is less than or equal to currentVersion
        /// </param>
        private bool verifyVersion(string targetVersion, string currentVersion, bool lessOrEqual = false)
        {
            // has the format v1.x.x
            if (version_regex.IsMatch(targetVersion) && version_regex.IsMatch(currentVersion))
            {
                string[] target = targetVersion.Split('.');
                string[] current = currentVersion.Split('.');

                bool minor = false;
                if (target.Length == current.Length)
                {
                    for (short index = 0; index < current.Length; index++)
                    {
                        if (target[index] == "x" || target[index] == current[index])
                        {
                            continue;
                        }
                        // check if targetVersion is MIN that currentVersion
                        if (lessOrEqual)
                        {
                            // the previous section was MIN, if it was min the rest are MIN too
                            if (minor)
                            {
                                continue;
                            }
                            if (int.TryParse(target[index], out int num1))
                            {
                                if(int.TryParse(current[index], out int num2))
                                {
                                    // the targetVersion section is MIN that version 2 section
                                    if(num1 < num2) 
                                    {
                                        minor = true;
                                        continue;
                                    }
                                }
                            }
                        }
                        return false;
                    }
                    //both are valid for each other
                    return true;

                }

            }
            return false;
        }

        /// <summary>
        ///     This function looks in the MODS folder for incorrectly installed mods and moves them to a new folder 
        ///     where they should be.
        /// </summary>
        private void detectIndividualDlls()
        {
            try
            {
                Debug.Log("Searching mods improperly installed");
                FolderPath newFolder;
                foreach (FilePath file in ModsFolder.GetFilesInFolder(false))
                {
           
                    if(file.Extension == "dll")
                    {
                        Debug.Log($"{file.FileName} incorrectly installed!");
                        newFolder = ModsFolder.Extend(file.CleanFileName);
                        if (!newFolder.FolderExists())
                        {
                            newFolder = newFolder.CreateFolder();
                        }
                        Debug.Log($"{file.FileName} moved!");
                        file.Move(newFolder.ExtendToFile(file.FileName));
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Failed to check for incorrectly installed mods!");
            }
        }

        /// <summary>
        ///     This is the mod loader entry point, this is the method that is executed after being injected into the game.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Harmony patcher = new Harmony("sfs.mod.modloader");
            patcher.PatchAll();
        }
    }
}
