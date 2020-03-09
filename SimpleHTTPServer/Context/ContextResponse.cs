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
        public string contentType;
        public int contentLength;
        public JSON respData;

        public ContextResponse()
        {
            httpVersion = string.Empty;
            statusCode = Constants.StatusCode.OK;
            contentType = string.Empty;
            contentLength = 0;
            respData = new JSON("{\n}");
        }
    }
}
