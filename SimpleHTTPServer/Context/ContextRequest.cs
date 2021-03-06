﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    public class ContextRequest
    {
        public string httpReqName;
        public string[] url;
        public string contentType;
        public string host;
        public int contentLength;
        public JSON reqData;
        public bool isAccountRequest;

        public ContextRequest()
        {
            httpReqName = string.Empty;
            url = new string[] { };
            contentType = string.Empty;
            host = string.Empty;
            contentLength = 0;
            reqData = new JSON("{\n}");
            isAccountRequest = false;
        }
    }
}
