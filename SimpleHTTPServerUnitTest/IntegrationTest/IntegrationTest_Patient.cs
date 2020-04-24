using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;
using SimpleHTTPServer.Constants;
using SimpleHTTPServer.InternalObject;

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

        private SimpleHTTPServer.Context AddPatient(string firstname, string secondname)
        {
            string request = HTTPTemplate.CreateRequest("POST", "/sklexp/accounts/" + accountId + "/patient", "{" +
                    "\"firstname\":\"" + firstname + "\"," +
                    "\"secondname\":\"" + secondname + "\"}");

            return SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
        }

        private SimpleHTTPServer.Context GetAllPatients()
        {
            string request = HTTPTemplate.CreateRequest("GET", "/sklexp/accounts/" + accountId + "/patient", "");

            return SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
        }

        private JSON GetFirstPatient()
        {
            var context = GetAllPatients();
            return ((List<JSON>)context.contextResponse.respData.data["patients"])[0];
        }

        // POST /sklexp/accounts/{ACCOUNT_ID}/patient
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_2_POST_addPatient()
        {
            SimpleHTTPServer.Context context = AddPatient("Patient67", "Ivanov");
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"message\":\"create success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(201, "Created", expectedRespData), headers);
        }

        // GET /sklexp/accounts/{ACCOUNT_ID}/patient
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_1_GET_getAllPatient_ZeroPatient()
        {
            SimpleHTTPServer.Context context = GetAllPatients();
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"data\": {" +
                    "\"patients\": []" +
                "}," +
                "\"message\":\"success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(200, "OK", expectedRespData), headers);
        }

        [TestMethod]
        public void Test_3_GET_getAllPatient_OnePatient()
        {
            var context = GetAllPatients();
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);

            var patients = (List<JSON>)context.contextResponse.respData.data["patients"];
            string patientsString = string.Empty;
            if (patients.Count == 1)
            {
                patientsString = "{" +
                            "\"patient_id\": \"" + patients[0].data["patient_id"] + "\"," +
                            "\"firstname\": \"Patient67\"," +
                            "\"secondname\": \"Ivanov\"," +
                            "\"pet_id\": \"\"" +
                        "}";
            }

            string expectedRespData = "{" +
                "\"data\": {" +
                    "\"patients\": [" +
                        patientsString +
                    "]" +
                "}," +
                "\"message\":\"success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(200, "OK", expectedRespData), headers);
        }

        [TestMethod]
        public void Test_4_GET_getAllPatient_TwoPatient()
        {
            AddPatient("Patient68", "Ivanov");
            var context = GetAllPatients();
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);

            var patients = (List<JSON>)context.contextResponse.respData.data["patients"];
            string patientsString = "{" +
                            "\"patient_id\": \"" + patients[0].data["patient_id"] + "\"," +
                            "\"firstname\": \"Patient68\"," +
                            "\"secondname\": \"Ivanov\"," +
                            "\"pet_id\": \"\"" +
                        "}";
            if (patients.Count == 2)
            {
                patientsString = patientsString.Replace(patients[0].data["patient_id"].ToString(), patients[1].data["patient_id"].ToString());
                patientsString = "{" +
                            "\"patient_id\": \"" + patients[0].data["patient_id"] + "\"," +
                            "\"firstname\": \"Patient67\"," +
                            "\"secondname\": \"Ivanov\"," +
                            "\"pet_id\": \"\"" +
                        "}," + patientsString;
            }

            string expectedRespData = "{" +
                "\"data\": {" +
                    "\"patients\": [" +
                        patientsString +
                    "]" +
                "}," +
                "\"message\":\"success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(200, "OK", expectedRespData), headers);
        }

        // GET /sklexp/accounts/{ACCOUNT_ID}/patient/{PATIENT_ID}
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_5_GET_getPatient()
        {
            AddPatient("Patient69", "Ivanov");
            var patient = GetFirstPatient();

            string request = HTTPTemplate.CreateRequest("GET", "/sklexp/accounts/" + accountId + "/patient/" + patient.data["patient_id"].ToString(), "");

            var context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"data\": {" +
                    "\"firstname\": \"" + patient.data["firstname"] + "\"," +
                    "\"secondname\": \"" + patient.data["secondname"] + "\"," +
                    "\"pet_id\": \"\"" +
                "}," +
                "\"message\":\"success\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(200, "OK", expectedRespData), headers);
        }

        [TestMethod]
        public void Test_6_GET_getNonExistentPatient_404_NotFound()
        {
            string request = HTTPTemplate.CreateRequest("GET", "/sklexp/accounts/" + accountId + "/patient/{PATIENT_ID}", "");

            var context = SimpleHTTPServer.HTTPInteraction.Client.HandleRequest(request);
            string headers = SimpleHTTPServer.HTTPInteraction.Client.PrepareResponse(context);
            string expectedRespData = "{" +
                "\"message\":\"Object not found\"" +
                "}";
            Assert.AreEqual(HTTPTemplate.CreateResponse(404, "Not Found", expectedRespData), headers);
        }

        // PATCH /sklexp/accounts/{ACCOUNT_ID}/patient/{PATIENT_ID}
        // ---------------------------------------------------------
        /*[TestMethod]
        public void Test_PATCH_setPetMedicalCard()
        {
            var patient = GetFirstPatient();
            string patientId = patient.data["patient_id"].ToString();
            string request = HTTPTemplate.CreateRequest("PATCH", "/sklexp/accounts/" + accountId + "/patient/" + patientId, "\"pet_medical_card_id\":"
            "");
        }*/
    }
}
