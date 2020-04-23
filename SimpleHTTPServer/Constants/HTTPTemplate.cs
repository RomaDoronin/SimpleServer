using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Constants
{
    public class HTTPTemplate
    {
        public static string CreateRequest(string reqName, string url, string reqData)
        {
            if (reqData.Length > 1)
            {
                reqData = reqData.Insert(1, "\n");
            }
            int contentLength = reqData.Length;
            return
                reqName + " " + url + " " + CommonConstants.HTTP_VERSION + "\n" +
                "Content-Type: " + CommonConstants.CONTENT_TYPE_JSON + "\n" +
                "User-Agent: PostmanRuntime/x.xx.x\n" +
                "Accept: */*\n" +
                "Cache-Control: nocache\n" +
                "Postman-Token: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\n" +
                "Host: 127.0.0.1\n" +
                "Accept-Encoding: gzip, deflate, br\n" +
                "Content-Length: " + contentLength + "\n" +
                "Connection: keep-alive\n" +
                "\n" +
                reqData;
        }

        public static string CreateResponse(int statusCode, string statusMsg, string respData)
        {
            respData = new JSON(respData).ToString();
            return
                CommonConstants.HTTP_VERSION + " " + statusCode + " " + statusMsg + "\n" +
                "Content-Type: " + CommonConstants.CONTENT_TYPE_JSON + "\n" +
                "Content-Length: " + respData.Length + "\n" +
                "\n" +
                respData;
        }
    }
}
