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
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<Loader>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            gameObject.SetActive(true);
            Loader.root = gameObject;
            Loader.baseAssigner = __instance;
        }
 
    }
}
