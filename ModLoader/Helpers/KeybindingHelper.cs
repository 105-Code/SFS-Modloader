using SFS.Builds;
using SFS.Input;
using SFS.World;
using System;
using UnityEngine;

namespace ModLoader.Helpers
{
    /// <summary>
    /// Use this class if you need add key events.
    /// </summary>
    public class KeybindingHelper
    {

        /// <summary>
        /// Add onKeyDown on world and map
        /// </summary>
        /// <param name="key">key to trigger the event</param>
        /// <param name="action"> is executed when the event occurs</param>
        /// <example>
        ///     KeybindingHelper.AddOnKeyDownWorld(Keycode.T, this.myMethod);
        /// </example>
        public static void AddOnKeyDownWorld(I_Key key, Action action)
        {
            if (GameManager.main != null)
            {
                GameManager.main.world_Input.keysNode.AddOnKeyDown(key, action);
                GameManager.main.map_Input.keysNode.AddOnKeyDown(key, action);
                return;
            }
            Debug.LogError("This method only works in World Scene");
        }

        /// <summary>
        /// Add onKeyDown on builder
        /// </summary>
        /// <param name="key">key to trigger the event</param>
        /// <param name="action"> is executed when the event occurs</param>
        /// <example>
        ///     KeybindingHelper.AddOnKeyDownBuilder(Keycode.T, this.myMethod);
        /// </example>
        public static void AddOnKeyDownBuilder(I_Key key, Action action)
        {
            if (BuildManager.main != null)
            {
                BuildManager.main.build_Input.keysNode.AddOnKeyDown(key, action);
                return;
            }
            Debug.LogError("This method only works in Builder Scene");
        }
    }
}
