using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;

namespace SimpleHTTPServerUnitTest.Context
{
    [TestClass]
    public class UnitTest_Context
    {
        private string request;
        private string[] splitRequest;

        public UnitTest_Context()
        {
            request = "POST /sklexp/regist HTTP/1.1\n" +
                            "Content-Type: application/json\n" +
                            "User-Agent: PostmanRuntime/7.23.0\n" +
                            "Accept: */*\n" +
                            "Cache-Control: no-cache\n" +
                            "Postman-Token: c52d797f-e215-4ca5-99cb-c686b06c826d\n" +
                            "Host: 127.0.0.1\n" +
                            "Accept-Encoding: gzip, deflate, br\n" +
                            "Content-Length: 137\n" +
                            "Connection: keep-alive\n" +
                            "\n" +
                            "{\n" +
                            "    \"firstname\": \"Roman\",\n" +
                            "    \"secondname\": \"Doronin\",\n" +
                            "    \"username\": \"rdoronin\",\n" +
                            "    \"hash_password\": \"#9jfg3ksg34jh34g9$4geeg54zm\"\n" +
                            "}";

            splitRequest = request.Split(new char[] { ' ', '\n' });
        }

        // Context
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CreatedNotAccountRequest()
        {
            /*SimpleHTTPServer.Context context = new SimpleHTTPServer.Context(registRequest);

            Assert.AreEqual("POST", context.contextRequest.httpReqName);

            Assert.AreEqual("", context.contextRequest.url[0]);
            Assert.AreEqual(SimpleHTTPServer.Constants.CommonConstants.COMPANY_PREFIX, context.contextRequest.url[1]);
            Assert.AreEqual(SimpleHTTPServer.Constants.ModuleList.MODULE_REGIST, context.contextRequest.url[2]);

            Assert.AreEqual(SimpleHTTPServer.Constants.CommonConstants.HTTP_VERSION, context.contextRequest.httpVersion);

            Assert.AreEqual("", context.contextRequest.contentType);

            Assert.AreEqual("", context.contextRequest.host);

            Assert.AreEqual("", context.contextRequest.contentLength);

            Assert.AreEqual("", context.contextRequest.isAccountRequest);

            Assert.AreEqual("", context.contextRequest.reqData);*/
        }

        // httpReqName
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectHTTPReqName()
        {
            var context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("POST", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.OK);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "PUT");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("PUT", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.OK);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "GET");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("GET", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.OK);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "PATCH");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("PATCH", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.OK);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "DELETE");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("DELETE", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.OK);
        }

        [TestMethod]
        public void Test_Context_IncorrectHTTPReqName()
        {
            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "COPY");
            var context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("COPY", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.METHOD_NOT_ALLOWED);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "OPTIONS");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("OPTIONS", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.METHOD_NOT_ALLOWED);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreNotEqual("", context.contextRequest.httpReqName);
            Assert.AreEqual(context.contextResponse.statusCode, SimpleHTTPServer.Constants.StatusCode.BAD_REQUEST);
        }

        // url
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectUrl()
        {

        }

        [TestMethod]
        public void Test_Context_IncorrectCompanyPrefix()
        {

        }

        [TestMethod]
        public void Test_Context_IncorrectModuleName()
        {

        }

        [TestMethod]
        public void Test_Context_WithoutModuleName()
        {

        }

        // httpVersion
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectHTTPVersion()
        {

        }

        [TestMethod]
        public void Test_Context_IncorrectHTTPVersion()
        {

        }

        // contentType
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectContentType()
        {

        }

        [TestMethod]
        public void Test_Context_IncorrectContentType()
        {

        }

        // host
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_SetHost()
        {

        }

        // contentLength
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CulcContentLength()
        {

        }

        // isAccountRequest
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CheckIsAccountRequest()
        {

        }

        // reqData
        // ---------------------------------------------------------
    }
}
