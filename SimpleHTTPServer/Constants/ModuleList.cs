using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Constants
{
    class ModuleList
    {
        const string MODULE_REGIST = "regist";

        public static Modules.IModule GetModuleByModuleName(string moduleName)
        {
            Modules.IModule module;

            switch (moduleName)
            {
                case MODULE_REGIST:
                    module = new Modules.Regist();
                    break;
                default:
                    module = null;
                    break;
            }
            
            return module;
        }
    }
}
