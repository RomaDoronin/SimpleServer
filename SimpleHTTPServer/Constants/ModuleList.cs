﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Constants
{
    public class ModuleList
    {
        // Without account
        public const string MODULE_REGIST = "regist";
        public const string MODULE_AUTH = "auth";

        // With account
        public const string MODULE_PATIENT = "patient";

        public static Modules.IModule GetModuleByModuleName(string moduleName)
        {
            Modules.IModule module;

            switch (moduleName)
            {
                case MODULE_REGIST:
                    module = new Modules.Regist();
                    break;
                case MODULE_AUTH:
                    module = new Modules.Auth();
                    break;
                case MODULE_PATIENT:
                    module = new Modules.Patient();
                    break;
                default:
                    module = null;
                    break;
            }
            
            return module;
        }
    }
}
