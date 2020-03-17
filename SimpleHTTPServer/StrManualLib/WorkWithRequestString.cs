using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    public enum RequestPosition
    {
        HTTP_REQ_NAME = 0,
        URL = 1,
        HTTP_VERSION = 2,
        CONTENT_TYPE = 4,
        HOST = 14,
        CONTENT_LENGTH = 20,
        REQ_DATA_START = 24
    }

    public class WorkWithRequestString
    {
        //private string request;
        //private string[] splitRequest;

        public static void SetHTTPReqName(ref string request, ref string[] splitRequest, string httpReqName)
        {
            request = request.Replace(splitRequest[(int)RequestPosition.HTTP_REQ_NAME], httpReqName);
            splitRequest[(int)RequestPosition.HTTP_REQ_NAME] = httpReqName;
        }

        public static void SetUrl(ref string request, ref string[] splitRequest, string url)
        {
            request = request.Replace(splitRequest[(int)RequestPosition.URL], url);
            splitRequest[(int)RequestPosition.URL] = url;
        }

        public static void SetHTTPVersion(ref string request, ref string[] splitRequest, string httpVersion)
        {
            request = request.Replace(splitRequest[(int)RequestPosition.HTTP_VERSION], httpVersion);
            splitRequest[(int)RequestPosition.HTTP_VERSION] = httpVersion;
        }

        public static void SetContentType(ref string request, ref string[] splitRequest, string contentType)
        {
            request = request.Replace(splitRequest[(int)RequestPosition.CONTENT_TYPE], contentType);
            splitRequest[(int)RequestPosition.CONTENT_TYPE] = contentType;
        }

        public static void SetHost(ref string request, ref string[] splitRequest, string host)
        {
            request = request.Replace(splitRequest[(int)RequestPosition.HOST], host);
            splitRequest[(int)RequestPosition.HOST] = host;
        }

        public static void SetContentLength(ref string request, ref string[] splitRequest, string contentLength)
        {
            request = request.Replace(splitRequest[(int)RequestPosition.CONTENT_LENGTH], contentLength);
            splitRequest[(int)RequestPosition.CONTENT_LENGTH] = contentLength;
        }

        public static void SetReqData(ref string request, ref string[] splitRequest, string reqData)
        {
            string oldReqData = string.Empty;
            for (int count = (int)RequestPosition.REQ_DATA_START; count < splitRequest.Length; count++)
            {
                oldReqData += splitRequest[count];
            }
            request = request.Replace(oldReqData, reqData);
        }
    }
}
