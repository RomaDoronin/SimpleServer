﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Constants
{
    enum UrlPositionNumber
    {
        COMPANY_PREFIX = 1,
        MODULE_NAME = 2
    }

    class CommonConstants
    {
        public const string HTTP_VERSION = "HTTP/1.1";
        public const string COMPANY_PREFIX = "/sklexp";
        public const string CONTENT_TYPE_JSON = "application/json";
        public const string DEFAULT_RESPONSE_MSG = "success";
        public const char URL_SEPARATOR = '/';
    }
}