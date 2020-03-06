using System.Collections.Generic;

namespace SimpleHTTPServer
{
    class Context
    {
        private const string KEY_CONTENT_TYPE = "Content-Type:";
        private const string KEY_HOST = "Host:";
        private const string KEY_CONTENT_LENGTH = "Content-Length:";
        private const string REQ_DATA_START = "{";
        private const string REQ_DATA_END = "}";

        private bool isError;

        public ContextRequest contextRequest;
        public ContextResponse contextResponse;

        public bool IsError { get => isError; }

        private void GetHttpReqName(ref string request)
        {
            httpReqName = StrManualLib.GetNextWordWithDelete(ref request);
        }

        private void GetContentType(ref string request)
        {
            while (KEY_CONTENT_TYPE != StrManualLib.GetNextWordWithDelete(ref request)) { }
            contentType = StrManualLib.GetNextWordWithDelete(ref request);
        }

        private void GetHost(ref string request)
        {
            while (KEY_HOST != StrManualLib.GetNextWordWithDelete(ref request)) { }
            host = StrManualLib.GetNextWordWithDelete(ref request);
        }

        private void GetContentLength(ref string request)
        {
            while (KEY_CONTENT_LENGTH != StrManualLib.GetNextWordWithDelete(ref request)) { }
            contentLength = int.Parse(StrManualLib.GetNextWordWithDelete(ref request));
        }

        private void GetReqData(ref string request)
        {
            reqData = new Dictionary<string, string>();

            while (REQ_DATA_START != StrManualLib.GetNextWordWithDelete(ref request))
            {
                if (0 == request.Length)
                {
                    return;
                }
            }
            string key;
            while (REQ_DATA_END != (key = StrManualLib.GetNextWordWithDelete(ref request)))
            {
                string value = RemoveSpecialSymbol(StrManualLib.GetNextWordWithDelete(ref request));
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
            contextRequest = new ContextRequest();
            contextResponse = new ContextResponse();

            GetHttpReqName(ref request);
            GetContentType(ref request);
            GetHost(ref request);
            GetContentLength(ref request);
            GetReqData(ref request);
            CheckForErrors();
        }
    }
}
