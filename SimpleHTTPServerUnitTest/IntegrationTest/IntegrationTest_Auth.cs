using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;
using SimpleHTTPServer.Constants;

namespace SimpleHTTPServerUnitTest.IntegrationTest
{
    [TestClass]
    public class IntegrationTest_Auth
    {
        public IntegrationTest_Auth()
        {
            // Регистрация
            string request = HTTPTemplate.CreateRequest("POST", "/sklexp/regist", "{" +
                "\"firstname\":\"Roman\"," +
                "\"secondname\":\"Doronin\"," +
                "\"username\":\"IntegrationTest_Auth\"," +
                "\"hash_password\":\"#9jfg3ksg34jh34g9$4geeg54zm\"}");
            SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
        }

        // PUT /sklexp/auth
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_PUT_authorizeUser()
        {
            string request = HTTPTemplate.CreateRequest("PUT", "/sklexp/auth", "{" +
                "\"username\":\"IntegrationTest_Auth\"," +
                "\"hash_password\":\"#9jfg3ksg34jh34g9$4geeg54zm\"}");

            SimpleHTTPServer.Context context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"data\":{" +
                    "\"account_id\":\"" + context.contextResponse.respData.data["account_id"].ToString() + "\"" +
                "}," +
                "\"message\":\"success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(200, "OK", expectedRespData), headers);
        }

        [TestMethod]
        public void Test_PUT_authorizeUserWithInvalidCredentials_401_Unauthorized()
        {
            string request = HTTPTemplate.CreateRequest("PUT", "/sklexp/auth", "{" +
                "\"username\":\"Test_PUT_authorizeUserWithInvalidCredentials_401_Unauthorized\"," +
                "\"hash_password\":\"#9jfg3ksg34jh34g9$4geeg54zm\"}");

            SimpleHTTPServer.Context context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"message\":\"There is no such username\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(401, "Unauthorized", expectedRespData), headers);
        }
    }
}
