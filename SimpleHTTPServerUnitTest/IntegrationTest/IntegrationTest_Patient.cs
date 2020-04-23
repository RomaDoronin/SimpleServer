using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;
using SimpleHTTPServer.Constants;

namespace SimpleHTTPServerUnitTest.IntegrationTest
{
    [TestClass]
    public class IntegrationTest_Patient
    {
        private string accountId;

        public IntegrationTest_Patient()
        {
            // Регистрация
            string request = HTTPTemplate.CreateRequest("POST", "/sklexp/regist", "{" +
                "\"firstname\":\"Roman\"," +
                "\"secondname\":\"Doronin\"," +
                "\"username\":\"IntegrationTest_Patient\"," +
                "\"hash_password\":\"#9jfg3ksg34jh34g9$4geeg54zm\"}");
            SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);

            // Аутентификация
            request = HTTPTemplate.CreateRequest("PUT", "/sklexp/auth", "{" +
                "\"username\":\"IntegrationTest_Patient\"," +
                "\"hash_password\":\"#9jfg3ksg34jh34g9$4geeg54zm\"}");
            accountId = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request).contextResponse.respData.data["account_id"].ToString();
        }

        // POST /sklexp/accounts/{ACCOUNT_ID}/patient
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_POST_addPatient()
        {
            string request = HTTPTemplate.CreateRequest("POST", "/sklexp/accounts/" + accountId + "/patient", "{" +
                    "\"firstname\":\"Patient67\"," +
                    "\"secondname\":\"Ivanov\"}");

            SimpleHTTPServer.Context context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"message\":\"create success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(201, "Created", expectedRespData), headers);
        }

        // GET /sklexp/accounts/{ACCOUNT_ID}/patient
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_GET_getAllPatient()
        {
            string request = HTTPTemplate.CreateRequest("GET", "/sklexp/accounts/" + accountId + "/patient", "");

            SimpleHTTPServer.Context context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"data\":{" +
                    "" +
                "}," +
                "\"message\":\"success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(200, "OK", expectedRespData), headers);
        }

        // GET /sklexp/accounts/{ACCOUNT_ID}/patient/{PATIENT_ID}
        // ---------------------------------------------------------
        /*[TestMethod]
        public void Test_GET_getPatient()
        {
            string request = HTTPTemplate.CreateRequest("GET", "/sklexp/accounts/" + accountId + "/patient", "");

            SimpleHTTPServer.Context context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"data\":{" +
                    "" +
                "}," +
                "\"message\":\"success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(200, "OK", expectedRespData), headers);
        }

        /*[TestMethod]
        public void Test_GET_getNonExistentPatient_404_NotFound()
        {
            Assert.AreEqual(1, 0);
        }

        // PATCH /sklexp/accounts/{ACCOUNT_ID}/patient/{PATIENT_ID}
        // ---------------------------------------------------------
        /*[TestMethod]
        public void Test_PATCH_setPetMedicalCard()
        {
            Assert.AreEqual(1, 0);
        }*/
    }
}
