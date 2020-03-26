using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;
using SimpleHTTPServer.Constants;

namespace SimpleHTTPServerUnitTest.Context
{
    [TestClass]
    public class UnitTest_Context
    {
        private string request;
        private string[] splitRequest;
        private SimpleHTTPServer.Context context;

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
            SimpleHTTPServer.Context context = new SimpleHTTPServer.Context(request);
            Assert.IsNotNull(context);
        }

        // httpReqName
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectHTTPReqName()
        {
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("POST", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.OK, context.contextResponse.statusCode);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "PUT");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("PUT", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.OK, context.contextResponse.statusCode);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "GET");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("GET", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.OK, context.contextResponse.statusCode);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "PATCH");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("PATCH", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.OK, context.contextResponse.statusCode);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "DELETE");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("DELETE", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.OK, context.contextResponse.statusCode);
        }

        [TestMethod]
        public void Test_Context_IncorrectHTTPReqName()
        {
            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "COPY");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("COPY", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.METHOD_NOT_ALLOWED, context.contextResponse.statusCode);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "OPTIONS");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("OPTIONS", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.METHOD_NOT_ALLOWED, context.contextResponse.statusCode);

            WorkWithRequestString.SetHTTPReqName(ref request, ref splitRequest, "");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreNotEqual("", context.contextRequest.httpReqName);
            Assert.AreEqual(StatusCode.BAD_REQUEST, context.contextResponse.statusCode);
        }

        // url
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectUrl()
        {
            context = new SimpleHTTPServer.Context(request);
            int reqLength = 3;
            Assert.AreEqual(reqLength, context.contextRequest.url.Length);
            Assert.AreEqual("", context.contextRequest.url[0]);
            Assert.AreEqual(CommonConstants.COMPANY_PREFIX, context.contextRequest.url[(int)UrlPositionNumber.COMPANY_PREFIX]);
            Assert.AreEqual(ModuleList.MODULE_REGIST, context.contextRequest.url[(int)UrlPositionNumber.MODULE_NAME]);

            string accountId = "g1HYN9WbpLQVsstOww8mrfwe";
            WorkWithRequestString.SetUrl(ref request, ref splitRequest, string.Format("/sklexp/accounts/{0}/patient", accountId));
            context = new SimpleHTTPServer.Context(request);
            reqLength = 5;
            Assert.AreEqual(reqLength, context.contextRequest.url.Length);
            Assert.AreEqual("", context.contextRequest.url[0]);
            Assert.AreEqual(CommonConstants.COMPANY_PREFIX, context.contextRequest.url[(int)UrlPositionNumberWithAccount.COMPANY_PREFIX]);
            Assert.AreEqual(CommonConstants.ACCOUNT_PREFIX, context.contextRequest.url[(int)UrlPositionNumberWithAccount.ACCOUNTS]);
            Assert.AreEqual(accountId, context.contextRequest.url[(int)UrlPositionNumberWithAccount.ACCOUNT_ID]);
            Assert.AreEqual(ModuleList.MODULE_PATIENT, context.contextRequest.url[(int)UrlPositionNumberWithAccount.MODULE_NAME]);
        }

        [TestMethod]
        public void Test_Context_IncorrectCompanyPrefix()
        {
            WorkWithRequestString.SetUrl(ref request, ref splitRequest, "/skl/auth");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreNotEqual(CommonConstants.COMPANY_PREFIX, context.contextRequest.url[(int)UrlPositionNumber.COMPANY_PREFIX]);
            Assert.AreEqual(StatusCode.BAD_REQUEST, context.contextResponse.statusCode);

            WorkWithRequestString.SetUrl(ref request, ref splitRequest, "//auth");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreNotEqual(CommonConstants.COMPANY_PREFIX, context.contextRequest.url[(int)UrlPositionNumber.COMPANY_PREFIX]);
            Assert.AreEqual(StatusCode.BAD_REQUEST, context.contextResponse.statusCode);
        }

        [TestMethod]
        public void Test_Context_IncorrectModuleName()
        {
            WorkWithRequestString.SetUrl(ref request, ref splitRequest, "/sklexp/au");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("au", context.contextRequest.url[(int)UrlPositionNumber.MODULE_NAME]);
            Assert.AreEqual(StatusCode.NOT_FOUND, context.contextResponse.statusCode);
        }

        [TestMethod]
        public void Test_Context_WithoutModuleName()
        {
            WorkWithRequestString.SetUrl(ref request, ref splitRequest, "/sklexp/");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("", context.contextRequest.url[(int)UrlPositionNumber.MODULE_NAME]);
            Assert.AreEqual(StatusCode.NOT_FOUND, context.contextResponse.statusCode);

            int reqLength = 2;
            WorkWithRequestString.SetUrl(ref request, ref splitRequest, "/sklexp");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(reqLength, context.contextRequest.url.Length);
            Assert.AreEqual(StatusCode.NOT_FOUND, context.contextResponse.statusCode);
        }

        // contentType
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectContentType()
        {
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(CommonConstants.CONTENT_TYPE_JSON, context.contextRequest.contentType);
        }

        [TestMethod]
        public void Test_Context_IncorrectContentType()
        {
            string anyOtherContentType = "application/any_other";
            WorkWithRequestString.SetContentType(ref request, ref splitRequest, anyOtherContentType);
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(anyOtherContentType, context.contextRequest.contentType);
            Assert.AreEqual(StatusCode.UNSUPPORTED_MEDIA_TYPE, context.contextResponse.statusCode);
        }

        // host
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_SetHost()
        {
            const string LOCAL_HOST = "127.0.0.1";
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(LOCAL_HOST, context.contextRequest.host);
        }

        // contentLength
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CulcContentLength()
        {
            int contentLength = 0;
            WorkWithRequestString.SetContentLength(ref request, ref splitRequest, contentLength.ToString());
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(contentLength, context.contextRequest.contentLength);

            for (contentLength = 1; contentLength < 2049; contentLength *= 2)
            {
                WorkWithRequestString.SetContentLength(ref request, ref splitRequest, contentLength.ToString());
                context = new SimpleHTTPServer.Context(request);
                Assert.AreEqual(contentLength, context.contextRequest.contentLength);
            }
        }

        // isAccountRequest
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CheckIsAccountRequest()
        {
            WorkWithRequestString.SetUrl(ref request, ref splitRequest, "/sklexp/auth");
            context = new SimpleHTTPServer.Context(request);
            Assert.IsFalse(context.contextRequest.isAccountRequest);

            string accountId = "g1HYN9WbpLQVsstOww8mrfwe";
            WorkWithRequestString.SetUrl(ref request, ref splitRequest, string.Format("/sklexp/accounts/{0}/patient", accountId));
            context = new SimpleHTTPServer.Context(request);
            Assert.IsTrue(context.contextRequest.isAccountRequest);
            Assert.AreEqual(StatusCode.NOT_FOUND, context.contextResponse.statusCode);
        }

        // reqData
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_Context_CorrectReqData()
        {
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual("Roman", context.contextRequest.reqData.data["firstname"].ToString());
            Assert.AreEqual("Doronin", context.contextRequest.reqData.data["secondname"].ToString());
            Assert.AreEqual("rdoronin", context.contextRequest.reqData.data["username"].ToString());
            Assert.AreEqual("#9jfg3ksg34jh34g9$4geeg54zm", context.contextRequest.reqData.data["hash_password"].ToString());
        }

        [TestMethod]
        public void Test_Context_ReqDataWithoutComma()
        {
            WorkWithRequestString.SetReqData(ref request, ref splitRequest, "{\n" +
                                                                            "    \"key1\": \"value1\"\n" +
                                                                            "    \"key2\": \"value2\"\n" +
                                                                            "}");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(StatusCode.BAD_REQUEST, context.contextResponse.statusCode);
        }

        [TestMethod]
        public void Test_Context_ReqDataWithoutColon()
        {
            WorkWithRequestString.SetReqData(ref request, ref splitRequest, "{\n" +
                                                                            "    \"key1\" \"value1\",\n" +
                                                                            "    \"key2\": \"value2\"\n" +
                                                                            "}");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(StatusCode.BAD_REQUEST, context.contextResponse.statusCode);

            WorkWithRequestString.SetReqData(ref request, ref splitRequest, "{\n" +
                                                                            "    \"key1\": \"value1\",\n" +
                                                                            "    \"key2\" \"value2\"\n" +
                                                                            "}");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(StatusCode.BAD_REQUEST, context.contextResponse.statusCode);

            WorkWithRequestString.SetReqData(ref request, ref splitRequest, "{\n" +
                                                                            "    \"key1\" \"value1\",\n" +
                                                                            "    \"key2\" \"value2\"\n" +
                                                                            "}");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(StatusCode.BAD_REQUEST, context.contextResponse.statusCode);
        }

        [TestMethod]
        public void Test_Context_EmptyReqData()
        {
            WorkWithRequestString.SetReqData(ref request, ref splitRequest, "{ }");
            context = new SimpleHTTPServer.Context(request);
            Assert.AreEqual(0, context.contextRequest.reqData.data.Count);
            Assert.AreEqual(StatusCode.OK, context.contextResponse.statusCode);
        }
    }
}
