using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModLoader.Helpers
{
    /// <summary>
    /// Trigger events related to scene change
    /// </summary>
    /// Authors: RYAN8990, dani0105
    class SceneHelper : MonoBehaviour
    {
        /// <summary>
        /// Fired when the Base Scene (the first scene when game loads) is loaded
        /// </summary>
        public static event EventHandler<Scene> OnBaseSceneLoaded;

        /// <summary>
        /// Fired when the start menu scene loaded -> After player has gone to another scene and come back
        /// </summary>
        public static event EventHandler<Scene> OnHomeSceneLoaded;

        /// <summary>
        /// Fired when the Build Scene is loaded
        /// </summary>
        public static event EventHandler<Scene> OnBuildSceneLoaded;

        /// <summary>
        /// Fired when the World Scene is Loaded
        /// </summary>
        public static event EventHandler<Scene> OnWorldSceneLoaded;

        /// <summary>
        /// Fired when the any scene is loaded
        /// </summary>
        public static event EventHandler<Scene> OnSceneLoaded;

        /// <summary>
        /// Get current scene
        /// </summary>
        public static scene currentScene;

        private void Awake()
        {
            SceneManager.sceneLoaded += handleSceneLoaded;
        }

        /// <summary>
        /// Fire all of the OnSceneLoaded Events
        /// </summary>
        private void handleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnSceneLoaded?.Invoke(this, scene);
            switch (scene.name)
            {
                case "Base_PC":
                    currentScene = SceneHelper.scene.Base;
                    OnBaseSceneLoaded?.Invoke(this, scene);
                    break;

                case "Home_PC":
                    currentScene = SceneHelper.scene.Home;
                    OnHomeSceneLoaded?.Invoke(this, scene);
                    break;

                case "Build_PC":
                    currentScene = SceneHelper.scene.Build;
                    OnBuildSceneLoaded?.Invoke(this, scene);
                    break;

                case "World_PC":
                    currentScene = SceneHelper.scene.World;
                    OnWorldSceneLoaded?.Invoke(this, scene);
                    break;
            }
        }

        /// <summary>
        /// Enume for all available SFS scenes
        /// </summary>
        public enum scene
        {
            Base,
            Home,
            Build,
            World
        }
    }

    
}
