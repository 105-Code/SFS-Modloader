using HarmonyLib;
using SFS.World;

namespace RefillMod
{
    [HarmonyPatch(typeof(Rocket), "Awake")]
    public class RocketAwake
    {

        [HarmonyPostfix]
        public static void Postfix(Rocket __instance)
        {
            Menu.playerRocket = __instance;
        }
    }
}
