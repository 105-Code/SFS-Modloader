using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader
{
    /// <summary>
    /// This interface is a mod entry point. so if you want to create mod, you need implement this interface
    /// in the main class, but only in the main class to prevent errors.
    /// </summary>
    public interface SFSMod
    {

        /// <summary>
        ///     The modder can specify mod name in this function.
        /// </summary>
        string getModName();

        /// <summary>
        ///  get the creator mod.
        /// </summary>
        string getModAuthor();

        /// <summary>
        ///     This is very important,because mod loader use this method to execute your mod.  
        /// </summary>
        void load();

        /// <summary>
        ///     For now this method do nothing, so you dont neet write nothing here. 
        /// </summary>
        void unload();

    }
}
