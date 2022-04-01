using HarmonyLib;
using SFS;
using UnityEngine;
using ModLoader.IO;
using SFS.Input;
using ModLoader.UI;
using System.Reflection;
using System;

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
            Loader.root = loader;
            IO.Console.root = modConsole;
            modConsole.AddComponent<IO.Console>();
            modConsole.SetActive(true);
            loader.AddComponent<Loader>();
            modConsole.AddComponent<Helper>();
            modConsole.AddComponent<ModsMenu>();


            UnityEngine.Object.DontDestroyOnLoad(modConsole);
            UnityEngine.Object.DontDestroyOnLoad(loader);

            loader.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(KeybindingsPC), "Awake")]
    class KeybindingsPCAwake
    {
        
        [HarmonyPostfix]
        public static void Postfix(KeybindingsPC __instance)
        {


            /*Debug.Log("Settings created");
            MethodInfo createText = __instance.GetType().GetMethod("CreateText",
            BindingFlags.NonPublic | BindingFlags.Instance);
            createText.Invoke(__instance, new object[] { "MODS" });


            foreach (Component component in __instance.textPrefab.GetComponentsInChildren<Component>())
            {
                Debug.Log(component);
            }

            KeybindingsPC.Key openConsole = KeyCode.F1;
            string message = "Show_Console";
            MethodInfo create = __instance.GetType().GetMethod("Create",
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, new Type[] { openConsole.GetType(), openConsole.GetType(), message.GetType() }, null);

            Debug.Log("Invoke Method");
            create.Invoke(__instance, new object[] { openConsole, openConsole, message });*/
            Debug.Log("Object");
            foreach (Component compoennt in __instance.gameObject.GetComponents<Component>())
            {
                Debug.Log(compoennt);
            }
            Debug.Log("Children");
            foreach (Component compoennt in __instance.gameObject.GetComponentsInChildren<Component>())
            {
                Debug.Log(compoennt);
            }



        }
    }

    [HarmonyPatch(typeof(InputManager), "ApplyDrag")]
    class InputManagerApplyDrag
    {

        [HarmonyPrefix]
        public static bool Prefix()
        {
            return !IO.Console.Main.ConsoleGui.IsVisible;
        }
    }
}
