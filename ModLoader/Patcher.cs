using HarmonyLib;
using SFS;
using UnityEngine;
using ModLoader.IO;
using SFS.Input;
using ModLoader.UI;
using System.Reflection;
using System;
using ModLoader.Helpers;
using SFS.UI;

namespace ModLoader
{
    [HarmonyPatch(typeof(BaseAssigner), "Awake")]
    class BaseAssignerAwake
    {
        /// <summary>
        ///     This function starts the modloader in the game.
        /// </summary>
        [HarmonyPostfix]
        public static void Postfix(BaseAssigner __instance)
        {
            // console start first
            GameObject modConsole = new GameObject("ModConsole");
            GameObject loader = new GameObject("ModLoader");
            GameObject helpers = new GameObject("Helpers");

            Loader.root = loader;
            IO.Console.root = modConsole;

            modConsole.AddComponent<IO.Console>();
            modConsole.SetActive(true);

            loader.AddComponent<Loader>();
            modConsole.AddComponent<ModsMenu>();

            helpers.AddComponent<SceneHelper>();
            helpers.AddComponent<Helper>(); // remove for v2.0.0

            UnityEngine.Object.DontDestroyOnLoad(modConsole);
            UnityEngine.Object.DontDestroyOnLoad(loader);
            UnityEngine.Object.DontDestroyOnLoad(helpers);

            loader.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(KeybindingsPC), "Awake")]
    class KeybindingsPCAwake
    {
        /// <summary>
        ///     Start after Awake at KeybindingsPC. This is used to create mod key bindings.
        /// </summary>
        /// <param name="__instance">KeybidnigsPc class instance. is used to get prefabs</param>
        [HarmonyPostfix]
        public static void Postfix(KeybindingsPC __instance)
        {
            ModSettings modSettings = __instance.gameObject.AddComponent<ModSettings>();
            modSettings.TextPrefab = __instance.textPrefab;
            modSettings.KeybindingPrefab = __instance.keybindingPrefab;
            modSettings.KeybindingsHolder = __instance.keybindingsHolder;
            modSettings.SpacePrefab = __instance.spacePrefab;
        }
    }

    [HarmonyPatch(typeof(KeybindingsPC), "ResetKeybindings")]
    class KeybindingsPCResetKeybindings
    {
        /// <summary>
        ///     Start after ResetKeybindings at KeybindingsPC. This is used to reset keybindings for mods.
        /// </summary>
        [HarmonyPostfix]
        public static void Postfix()
        {
            ModSettings.main.resetKeybindings();
        }
    }

    [HarmonyPatch(typeof(InputManager), "ApplyDrag")]
    class InputManagerApplyDrag
    {
        /// <summary>
        ///    Start before ApplyDrag at InputManager. This is used to prevent drag input when the mmodloader console is open.
        /// </summary>
        /// <returns> False to prevent drag input or true to continue</returns>
        [HarmonyPrefix]
        public static bool Prefix()
        {
            return !IO.Console.Main.ConsoleGui.IsVisible;
        }
    }
}
