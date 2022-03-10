using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ModLoader
{
    /// <summary>
    /// This interface is a mod entry point. so if you want to create mod, you need implement this interface
    /// in the main class, but only in the main class to prevent errors.
    /// </summary>
    public abstract class SFSMod
    {
        private string _modId;
        private string _name;
        private string _author;
        private string _modLoderVersion;
        private string _version;
        private string _description;
        private Dictionary<string, string[]> _dependencies;
        private AssetBundle _assets;
        private string _assetsFilename;

        public string ModId
        {
            get { return this._modId; }
        }

        /// <summary>
        ///     Get mod name
        /// </summary>
        public string Name
        {
            get { return this._name; }
        }

        /// <summary>
        ///     Get who creates this mod
        /// </summary>
        public string Author
        {
            get { return this._author; }
        }

        /// <summary>
        ///     Get what version of modloader this mod needs
        /// </summary>
        /// <value>
        ///     v1.0.x
        /// </value>
        public string ModLoderVersion
        {
            get { return this._modLoderVersion; }
        }

        public string Version
        {
            get { return this._version; }
        }

        /// <summary>
        ///    get a description about what this mod does
        /// </summary>
        public string Description
        {
            get { return this._description; }
        }

        /// <summary>
        ///    Get the list of mods you need to run.
        /// </summary>
        public Dictionary<string, string[]> Dependencies
        {
            get { return this._dependencies; }
        }

        /// <summary>
        ///     get the assets used for this mod.
        /// </summary>
        public AssetBundle Assets
        {
            get { return this._assets; }
        }

        protected SFSMod(string id, string name, string author, string modLoderVersion, string version, string description = "", string assetsFilename = null, Dictionary<string, string[]> dependencies = null)
        {
            _modId = id;
            _name = name;
            _author = author;
            _modLoderVersion = modLoderVersion;
            _version = version;
            _description = description;
            _dependencies = dependencies;
            _assetsFilename = assetsFilename;
        }

        /// <summary>
        /// This method loads assets from the mod folder.
        /// </summary>
        public void loadAssets()
        {
            if (string.IsNullOrEmpty(this._assetsFilename))
            {
                return;
            }

            string assetFilePath = Path.Combine(FileLocations.BaseFolder, "MODS", this._name, this._assetsFilename);
            AssetBundle assets = AssetBundle.LoadFromFile(assetFilePath);
            if (assets == null)
            {
                throw new Exception("Assets file not found");
            }

            this._assets = assets;

            Debug.Log(this._name + " assets loaded!");
        }

        /// <summary>
        ///     This method is called before Base's components are fully initialized allowing for hooking in their initialization 
        /// </summary>
        public virtual void early_load()
        {
        }

        /// <summary>
        ///     This method is called for modloader to start your mod 
        /// </summary>
        public abstract void load();

        /// <summary>
        ///     This method is called for modloader to remove your mod. 
        /// </summary>
        public abstract void unload();
    }
}
