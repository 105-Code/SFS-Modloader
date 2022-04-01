using SFS.Builds;
using SFS.Input;
using SFS.World;
using System;

namespace ModLoader.Helpers
{
    /// <summary>
    /// Use this class if you need add key events.
    /// </summary>
    static class KeybindingHelper
    {

        /// <summary>
        /// Add onKeyDown on world and map
        /// </summary>
        /// <param name="key">key to trigger the event</param>
        /// <param name="action"> is executed when the event occurs</param>
        public static void AddOnKeyDownWorld(I_Key key, Action action)
        {
            GameManager.main.world_Input.keysNode.AddOnKeyDown(key, action);
            GameManager.main.map_Input.keysNode.AddOnKeyDown(key, action);
        }

        /// <summary>
        /// Add onKeyDown on builder
        /// </summary>
        /// <param name="key">key to trigger the event</param>
        /// <param name="action"> is executed when the event occurs</param>
        public static void AddOnKeyDownBuilder(I_Key key, Action action)
        {
            BuildManager.main.build_Input.keysNode.AddOnKeyDown(key, action);
        }
    }
}
