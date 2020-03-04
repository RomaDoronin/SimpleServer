using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    class Context
    {
        private const string KEY_CONTENT_TYPE = "Content-Type:";
        private const string KEY_HOST = "Host:";
        private const string KEY_CONTENT_LENGTH = "Content-Length:";
        private const string REQ_DATA_START = "{";
        private const string REQ_DATA_END = "}";

        public string httpReqName;
        public string contentType;
        public string host;
        public int contentLength;
        public Dictionary<string, string> reqData;
        public bool isError;

        private string GetNextWordWithDelete(ref string str)
        {
            string word = "";
            string newStr = "";
            bool wordFound = false;

            foreach (char ch in str)
            {
                if (!wordFound)
                {
                    if (ch == ' ' || ch == '\r' || ch == '\n')
                    {
                        if (word.Length > 0)
                        {
                            wordFound = true;
                        }
                        continue;
                    }
                    word += ch;
                }
                else
                {
                    newStr += ch;
                }
            }

            str = newStr;
            return word;
        }

        private void GetHttpReqName(ref string request)
        {
            httpReqName = GetNextWordWithDelete(ref request);
        }

        private void GetContentType(ref string request)
        {
            while (KEY_CONTENT_TYPE != GetNextWordWithDelete(ref request)) { }
            contentType = GetNextWordWithDelete(ref request);
        }

        private void GetHost(ref string request)
        {
            while (KEY_HOST != GetNextWordWithDelete(ref request)) { }
            host = GetNextWordWithDelete(ref request);
        }

        private void GetContentLength(ref string request)
        {
            while (KEY_CONTENT_LENGTH != GetNextWordWithDelete(ref request)) { }
            contentLength = int.Parse(GetNextWordWithDelete(ref request));
        }

        private string RemoveSpecialSymbol(string str)
        {
            str = str.Remove(0, 1);
            if (str[str.Length - 1] != '"')
            {
                str = str.Remove(str.Length - 1, 1);
            }

            return str.Remove(str.Length - 1, 1);
        }

        private void GetReqData(ref string request)
        {
            reqData = new Dictionary<string, string>();

            while (REQ_DATA_START != GetNextWordWithDelete(ref request)) { }
            string key;
            while (REQ_DATA_END != (key = GetNextWordWithDelete(ref request)))
            {
                string value = RemoveSpecialSymbol(GetNextWordWithDelete(ref request));
                key = RemoveSpecialSymbol(key);
                reqData.Add(key, value);
            }
        }

        private void CheckForErrors()
        {
            isError = false;

            switch (httpReqName)
            {
                case "GET":
                    break;
                case "POST":
                    break;
                case "PUT":
                    break;
                case "DELETE":
                    break;
                case "PATCH":
                    break;
                default:
                    isError = true;
                    break;
            }
        }

        public Context(string request)
        {
            GetHttpReqName(ref request);
            GetContentType(ref request);
            GetHost(ref request);
            GetContentLength(ref request);
            GetReqData(ref request);
            CheckForErrors();
        }
    }
}
