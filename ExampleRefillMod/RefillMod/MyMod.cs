using HarmonyLib;
using ModLoader;
using SFS;
using SFS.Parts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RefillMod
{
    public class Main : SFSMod
    {
        public Main() : base("RefillMod", "ReFill", "Dani0105 and pixel_gamer579", "v1.1.1", "1.1.1"){}
        public static GameObject menuObject;

        public static Harmony patcher;
        public string getModAuthor()
        {
            return "Dani0105";
        }

        public string getModName()
        {
            return "ReFill";
        }

        public override void load()
        {
            Main.patcher = new Harmony("website.danielrojas.RefillMod");
            Main.patcher.PatchAll();

            Loader.main.suscribeOnChangeScene(this.OnSceneLoaded);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "World_PC")
            {
                GameObject menu = new GameObject("RefillMod");
                menu.AddComponent<Menu>();
                UnityEngine.Object.DontDestroyOnLoad(menu);
                menu.SetActive(true);
                Main.menuObject = menu;
                return;
            }

            if (menuObject != null)
            {
                UnityEngine.Object.Destroy(menuObject);
                menuObject = null;
            }
        }

        public override void unload()
        {
            throw new System.NotImplementedException();
        }
    }
}
