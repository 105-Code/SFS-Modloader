using HarmonyLib;
using SFS;
using System;
using UnityEngine;

namespace ModLoader
{
    [HarmonyPatch(typeof(BaseAssigner), "Awake")]
    class BaseAssignerAw
    {
        /// <summary>
        /// This function start the modloader into the game
        /// </summary>
        [HarmonyPostfix]
        public static void Postfix(BaseAssigner __instance)
        {
            // console start first
            GameObject modConsole = new GameObject("ModConsole");
            GameObject loader = new GameObject("ModLoader");
            Loader.root = loader;
            Console.root = modConsole;
            loader.AddComponent<Loader>();
            modConsole.AddComponent<Console>();

            UnityEngine.Object.DontDestroyOnLoad(modConsole);
            UnityEngine.Object.DontDestroyOnLoad(loader);

            loader.SetActive(true);
            modConsole.SetActive(true);
        }
 
    }
}
