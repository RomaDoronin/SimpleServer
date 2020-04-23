using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;
using SimpleHTTPServer.Constants;

namespace SimpleHTTPServerUnitTest.IntegrationTest
{
    [TestClass]
    public class IntegrationTest_Regist
    {
        // POST /sklexp/regist
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_POST_registUser()
        {
            string request = HTTPTemplate.CreateRequest("POST", "/sklexp/regist", "{" +
                "\"firstname\":\"Roman\"," +
                "\"secondname\":\"Doronin\"," +
                "\"username\":\"Test_POST_registUser\"," +
                "\"hash_password\":\"#9jfg3ksg34jh34g9$4geeg54zm\"}");

            SimpleHTTPServer.Context context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"message\":\"create success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(201, "Created", expectedRespData),  headers);
        }

        [TestMethod]
        public void Test_POST_registTwoUserWithSameUsername_409_Conflict()
        {
            string request = HTTPTemplate.CreateRequest("POST", "/sklexp/regist", "{" +
                "\"firstname\":\"Roman\"," +
                "\"secondname\":\"Doronin\"," +
                "\"username\":\"Test_POST_registTwoUserWithSameUsername\"," +
                "\"hash_password\":\"#9jfg3ksg34jh34g9$4geeg54zm\"}");

            SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            SimpleHTTPServer.Context context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"message\":\"A user with this username already exists\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(409, "Conflict", expectedRespData), headers);
        }
    }
}
