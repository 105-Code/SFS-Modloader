using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader.Mod
{
    /// <summary>
    ///     Use this interface to create keybinding settings for your mod
    /// </summary>
    /// <example>
    /// public class MySettings : SFSSettings
    /// {
    ///     public string getModId(){
    ///         return "ModId";
    ///     }
    ///     
    ///     public string getModFolder(){
    ///         return "path";
    ///     }
    ///     
    ///      public KeybindingsPC.Key My_Key = KeyCode.T;
    /// }
    /// </example>
    public interface SFSSettings
    {

        /// <summary>
        ///     Get mod id. This is used to identify for keybiding for another mods.
        /// </summary>
        /// <returns>
        ///     My unique mod id.
        /// </returns>
        /// <example>
        /// public string getModId(){
        ///     return MyMod.main.ModId;
        /// }
        /// </example>
        string getModId();

    }
}
