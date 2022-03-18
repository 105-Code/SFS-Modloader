using HarmonyLib;
using SFS;
using UnityEngine;
using ModLoader.IO;
using SFS.Input;
using ModLoader.UI;

namespace ModLoader
{
    [HarmonyPatch(typeof(BaseAssigner), "Awake")]
    class BaseAssignerAw
    {
        /// <summary>
        /// This function starts the modloader in the game.
        /// </summary>
        [HarmonyPostfix]
        public static void Postfix(BaseAssigner __instance)
        {
            // console start first
            GameObject modConsole = new GameObject("ModConsole");
            GameObject loader = new GameObject("ModLoader");
            Loader.root = loader;
            Console.root = modConsole;
            modConsole.AddComponent<Console>();
            modConsole.SetActive(true);
            loader.AddComponent<Loader>();
            modConsole.AddComponent<Helper>();
            modConsole.AddComponent<ModsMenu>();

            UnityEngine.Object.DontDestroyOnLoad(modConsole);
            UnityEngine.Object.DontDestroyOnLoad(loader);

            loader.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(InputManager), "ApplyDrag")]
    class InputManagerApplyDrag
    {

        [HarmonyPrefix]
        public static bool Prefix()
        {
            return !Console.Main.ConsoleGui.IsVisible;
        }
    }
}
