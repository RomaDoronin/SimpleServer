using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;
using System.Collections.Generic;

namespace SimpleHTTPServerUnitTest.Context
{
    [TestClass]
    public class UnitTest_JSON
    {
        private JSON json;

        // JSON by string
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_JSON_CreatWithEmptyString()
        {
            bool result = false;

            try
            {
                json = new JSON("");
            }
            catch
            {
                result = true;
            }
            if (!result)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test_JSON_CreatWithEmptyJSON()
        {
            json = new JSON("{}");
            Assert.AreEqual(0, json.data.Count);

            json = new JSON("{  }");
            Assert.AreEqual(0, json.data.Count);

            json = new JSON("{\n}");
            Assert.AreEqual(0, json.data.Count);
        }

        [TestMethod]
        public void Test_JSON_CreatWithValueString()
        {
            json = new JSON("{\"key\": \"value\"}");
            Assert.AreEqual("value", json.data["key"]);

            json = new JSON("{\"key1\": \"value1\",\n" +
                            " \"key2\": \"value2\"}");
            Assert.AreEqual("value1", json.data["key1"]);
            Assert.AreEqual("value2", json.data["key2"]);
        }

        [TestMethod]
        public void Test_JSON_CreatWithValueNumber()
        {
            json = new JSON("{\"key\": 0}");
            Assert.AreEqual(0, json.data["key"]);

            json = new JSON("{\"key1\": 12,\n" +
                            " \"key2\": 0.36}");
            Assert.AreEqual(12, json.data["key1"]);
            Assert.AreEqual(0.36f, json.data["key2"]);
        }

        [TestMethod]
        public void Test_JSON_CreatWithValueJSON()
        {
            json = new JSON("{\n" +
                            "    \"subJson\": {\n" +
                            "        \"key\": \"value\",\n" +
                            "        \"keyNum\": 12\n" +
                            "    }\n" +
                            "}");

            Assert.AreEqual((new JSON("{}")).GetType(), json.data["subJson"].GetType());
            JSON subJson = (JSON)json.data["subJson"];
            Assert.AreEqual("value", subJson.data["key"]);
            Assert.AreEqual(12, subJson.data["keyNum"]);

            json = new JSON("{\n" +
                            "    \"key1\": {\n" +
                            "        \"key11\": \"value11\",\n" +
                            "        \"key12\": 12\n" +
                            "    },\n" +
                            "    \"key2\": \"value2\",\n" +
                            "    \"key3\": {\n" +
                            "        \"key31\": {\n" +
                            "            \"key311\": \"value311\",\n" +
                            "            \"key312\": 312\n" +
                            "        }\n" +
                            "    }\n" +
                            "}");

            JSON key1 = (JSON)json.data["key1"];
            Assert.AreEqual("value11", key1.data["key11"]);
            Assert.AreEqual(12, key1.data["key12"]);
            JSON key3 = (JSON)json.data["key3"];
            JSON key31 = (JSON)key3.data["key31"];
            Assert.AreEqual("value311", key31.data["key311"]);
            Assert.AreEqual(312, key31.data["key312"]);
        }

        [TestMethod]
        public void Test_JSON_CreatWithValueList()
        {
            json = new JSON("{\"key\": \"value\"}");
            Assert.AreEqual("value", json.data["key"]);

            json = new JSON("{\"key1\": \"value1\",\n" +
                            " \"key2\": \"value2\"}");
            Assert.AreEqual("value1", json.data["key1"]);
            Assert.AreEqual("value2", json.data["key2"]);
        }

        // ToString
        // ---------------------------------------------------------
    }
}
