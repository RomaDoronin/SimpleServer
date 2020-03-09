using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Constants
{
    class ModuleList
    {
        public static Modules.IModule GetModuleByModuleName(string moduleName)
        {
            Modules.IModule module;

            switch (moduleName)
            {
                case "regist":
                    module = new Modules.Regist();
                    break;
                default:
                    module = new Modules.Regist();
                    break;
            }
            
            return module;
        }
    }
}
