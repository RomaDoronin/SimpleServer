using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Modules
{
    public interface IModule
    {
        void Validate(Context context);
        void ProcessRequest(Context context);
    }
}
