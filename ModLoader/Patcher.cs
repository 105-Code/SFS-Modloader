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

            GameObject modConsole = new GameObject("ModConsole");
            GameObject loader = new GameObject("ModLoader");
            loader.AddComponent<Loader>();
            modConsole.AddComponent<ModConsole>();

            UnityEngine.Object.DontDestroyOnLoad(modConsole);
            UnityEngine.Object.DontDestroyOnLoad(loader);

            loader.SetActive(true);
            modConsole.SetActive(true);

            Loader.root = loader;
            ModConsole.root = modConsole;
        }
 
    }
}
