using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader
{
    public class SFSModDependencie
    {
        private string _modid;
        private string[] _versions;

        public string ModId
        {
            get { return this._modid; }
        }

        public string[] Versions
        {
            get { return this._versions; }
        }

        public SFSModDependencie(string modid, string[] versions)
        {
            this._modid = modid;
            this._versions = versions;
        }
    }
}
