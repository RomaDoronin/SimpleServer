using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    public class ContextResponse
    {
        public Constants.StatusCode statusCode;
        public string message;
        public JSON respData;

        public ContextResponse()
        {
            statusCode = Constants.StatusCode.OK;
            message = string.Empty;
            respData = new JSON("{\n}");
        }
    }
}
