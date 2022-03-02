using System;
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
        private SFSModDependencie[] _dependencies;
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
        ///     Get who create this mod
        /// </summary>
        public string Author
        {
            get { return this._author; }
        }

        /// <summary>
        ///     Get what modloader version need
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
        ///     get a descripton about what do this mod
        /// </summary>
        public string Description
        {
            get { return this._description; }
        }

        /// <summary>
        ///     get the list of mods need it to work.
        /// </summary>
        public SFSModDependencie[] Dependencies
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


        protected SFSMod(string id,string name, string author, string modLoderVersion, string version, string description = "", string assetsFilename = null , SFSModDependencie[] dependencies = null )
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
        /// This method load assets from mod folder
        /// </summary>
        public void loadAssets()
        {
            if(this._assetsFilename == null || this._assetsFilename == "")
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
            
            Debug.Log(this._name+" assets loaded!");
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
