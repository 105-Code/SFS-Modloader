using SFS.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        private string _modFolder;

        /// <summary>
        ///     Get folder where is store this mod
        /// </summary>
        public string ModFolder
        {
            get
            {
                return this._modFolder;
            }
            set
            {
                this._modFolder = value;
            }
        }

        /// <summary>
        ///     Get Mod ID This ID is used to identify each mod. There cannot be two mods with the same ID
        /// </summary>
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
        ///     Get what min version of modloader this mod needs
        /// </summary>
        /// <value>
        ///     v1.0.x
        /// </value>
        public string ModLoderVersion
        {
            get { return this._modLoderVersion; }
        }

        /// <summary>
        ///     Get mod version.
        /// </summary>
        public string Version
        {
            get { return this._version; }
        }

        /// <summary>
        ///    Get a description about what this mod does
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
        ///     Get the assets used for this mod.
        /// </summary>
        public AssetBundle Assets
        {
            get { return this._assets; }
        }

        /// <summary>
        ///     This is the entry point for all modifications.
        /// </summary>
        /// <param name="id">Unique id for your mod</param>
        /// <param name="name">Mod name</param>
        /// <param name="author">who created this mod?</param>
        /// <param name="modLoderVersion"> What minimum version of modloader do you need?</param>
        /// <param name="version">What version of your mod is it?</param>
        /// <param name="description">What can your mod do?</param>
        /// <param name="assetsFilename">Do you need an assetbundle? charge it</param>
        /// <param name="dependencies">Do you need another mod to work? check it</param>
        /// <example>
        /// public ExampleMod():base("exampleMod", "Example Mod", "My description", "v1.x.x", "v1.0.0"){
        /// 
        /// }
        /// </example>
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

            string assetFilePath = Path.Combine(this.ModFolder, this._assetsFilename);
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
