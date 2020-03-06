using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    class ContextRequest
    {
        public string httpReqName;
        public string url;
        public string httpVersion;
        public string contentType;
        public string host;
        public int contentLength;
        public JSON reqData;

        public ContextRequest()
        {
            httpReqName = string.Empty;
            url = string.Empty;
            httpVersion = string.Empty;
            contentType = string.Empty;
            host = string.Empty;
            contentLength = 0;
            reqData = new JSON("{\n}");
        }
    }
}
