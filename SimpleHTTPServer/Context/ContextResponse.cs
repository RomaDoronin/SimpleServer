using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    class ContextResponse
    {
        public string httpVersion;
        public Constants.StatusCode statusCode;
        public string message;
        public string contentType;
        public JSON respData;

        public ContextResponse()
        {
            httpVersion = string.Empty;
            statusCode = Constants.StatusCode.OK;
            message = string.Empty;
            contentType = string.Empty;
            respData = new JSON("{\n}");
        }
    }
}
