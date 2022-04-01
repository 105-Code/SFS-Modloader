using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModLoader
{
    /// <summary>
    /// General helper for scenes. Do not use it, it will change in future versions
    /// </summary>
    [Obsolete("Helper is deprecated, please use SceneHelper instead.")]
    public class Helper : MonoBehaviour
    {
        public static Helper instance;

        // Events
        /// <summary>
        /// Fired every frame in the Base Scene
        /// </summary>
        public static event EventHandler OnUpdateBaseScene;
        /// <summary>
        /// Fired every frame in the Home Scene
        /// </summary>
        public static event EventHandler OnUpdateHomeScene;
        /// <summary>
        /// Fired every frame in the Build Scene
        /// </summary>
        public static event EventHandler OnUpdateBuildScene;
        /// <summary>
        /// Fired every frame in the World Scene
        /// </summary>
        public static event EventHandler OnUpdateWorldScene;

        /// <summary>
        /// Fired when the Base Scene (the first scene when game loads) is loaded
        /// </summary>
        public static event EventHandler OnBaseSceneLoaded;
        /// <summary>
        /// Fired when the start menu scene loaded -> After player has gone to another scene and come back
        /// </summary>
        public static event EventHandler OnHomeSceneLoaded;
        /// <summary>
        /// Fired when the Build Scene is loaded
        /// </summary>
        public static event EventHandler OnBuildSceneLoaded;
        /// <summary>
        /// Fired when the World Scene is Loaded
        /// </summary>
        public static event EventHandler OnWorldSceneLoaded;


        // Variables
        public static scene currentScene;

        private void Awake()
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Fire all of the OnUpdate Events
        /// </summary>
        private void Update()
        {
            switch (currentScene)
            {
                case scene.Base:
                    OnUpdateBaseScene?.Invoke(this, EventArgs.Empty);
                    break;

                case scene.Home:
                    OnUpdateHomeScene?.Invoke(this, EventArgs.Empty);
                    break;

                case scene.Build:
                    OnUpdateBuildScene?.Invoke(this, EventArgs.Empty);
                    break;

                case scene.World:
                    OnUpdateWorldScene?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        /// <summary>
        /// Fire all of the OnSceneLoaded Events
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "Base_PC":
                    currentScene = ModLoader.scene.Base;
                    OnBaseSceneLoaded?.Invoke(this, EventArgs.Empty);
                    break;

                case "Home_PC":
                    currentScene = ModLoader.scene.Home;
                    OnHomeSceneLoaded?.Invoke(this, EventArgs.Empty);
                    break;

                case "Build_PC":
                    currentScene = ModLoader.scene.Build;
                    OnBuildSceneLoaded?.Invoke(this, EventArgs.Empty);
                    break;

                case "World_PC":
                    currentScene = ModLoader.scene.World;
                    OnWorldSceneLoaded?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }

    public enum scene
    {
        Base,
        Home,
        Build,
        World
    }
}
