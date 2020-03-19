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
            json = new JSON("{\"key\": [12, 14, 16]}");
            var intList = (List<int>)(json.data["key"]);
            Assert.AreEqual(12, intList[0]);
            Assert.AreEqual(14, intList[1]);
            Assert.AreEqual(16, intList[2]);

            json = new JSON("{\"key\": [12.6, 14.4, 16.0]}");
            var floatList = (List<float>)(json.data["key"]);
            Assert.AreEqual(12.6f, floatList[0]);
            Assert.AreEqual(14.4f, floatList[1]);
            Assert.AreEqual(16.0f, floatList[2]);

            json = new JSON("{\"key\": [\"value1\", \"value2\", \"value3\"]}");
            var stringList = (List<string>)(json.data["key"]);
            Assert.AreEqual("\"value1\"", stringList[0]);
            Assert.AreEqual("\"value2\"", stringList[1]);
            Assert.AreEqual("\"value3\"", stringList[2]);

            json = new JSON("{\"key1\": [{\"key11\":\"value11\"}, {\"key12\":\"value12\"}, {\"key13\":\"value13\"}]}");
            var jsonList = (List<JSON>)(json.data["key1"]);
            Assert.AreEqual("value11", jsonList[0].data["key11"]);
            Assert.AreEqual("value12", jsonList[1].data["key12"]);
            Assert.AreEqual("value13", jsonList[2].data["key13"]);
        }

        // ToString
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_ToString_StringJSON()
        {
            json = new JSON("{\"key\": \"value\"}");
            Assert.AreEqual("{\n    \"key\": \"value\"\n}", json.ToString());

            json = new JSON("{\"key1\":\"value1\", \"key2\":\"value2\"}");
            Assert.AreEqual("{\n    \"key1\": \"value1\",\n    \"key2\": \"value2\"\n}", json.ToString());
        }

        [TestMethod]
        public void Test_ToString_NumberJSON()
        {
            json = new JSON("{\"key\": 12}");
            Assert.AreEqual("{\n    \"key\": 12\n}", json.ToString());

            json = new JSON("{\"key1\":15, \"key2\":25.6}");
            Assert.AreEqual("{\n    \"key1\": 15,\n    \"key2\": 25.6\n}", json.ToString());
        }

        [TestMethod]
        public void Test_ToString_ListJSON()
        {
            json = new JSON("{\"key1\": \"value1\", \"key2\": [21, 22]}");
            Assert.AreEqual("{\n" +
                            "    \"key1\": \"value1\",\n" +
                            "    \"key2\": [\n" +
                            "        21,\n" +
                            "        22\n" +
                            "    ]\n" +
                            "}", json.ToString());

            json = new JSON("{\"key1\": \"value1\", \"key2\": [21.6, 22.5, 23.85]}");
            Assert.AreEqual("{\n" +
                            "    \"key1\": \"value1\",\n" +
                            "    \"key2\": [\n" +
                            "        21.6,\n" +
                            "        22.5,\n" +
                            "        23.85\n" +
                            "    ]\n" +
                            "}", json.ToString());

            json = new JSON("{" +
                            "    \"key1\": \"value1\"," +
                            "    \"key2\": [21, 22]," +
                            "    \"key3\": [" +
                            "        {" +
                            "            \"key311\": \"value311\"," +
                            "            \"key312\": \"value312\"" +
                            "        }," +
                            "        {" +
                            "            \"key321\": \"value321\"" +
                            "        }" +
                            "    ]" +
                            "}");
            Assert.AreEqual("{\n" +
                            "    \"key1\": \"value1\",\n" +
                            "    \"key2\": [\n" +
                            "        21,\n" +
                            "        22\n" +
                            "    ],\n" +
                            "    \"key3\": [\n" +
                            "        {\n" +
                            "            \"key311\": \"value311\",\n" +
                            "            \"key312\": \"value312\"\n" +
                            "        },\n" +
                            "        {\n" +
                            "            \"key321\": \"value321\"\n" +
                            "        }\n" +
                            "    ]\n" +
                            "}", json.ToString());
        }

        [TestMethod]
        public void Test_ToString_InternalJSON()
        {
            json = new JSON("{\"key1\": \"value1\", \"key2\": {\"key21\":21, \"key22\":22}}");
            Assert.AreEqual("{\n" +
                            "    \"key1\": \"value1\",\n" +
                            "    \"key2\": {\n" +
                            "        \"key21\": 21,\n" +
                            "        \"key22\": 22\n" +
                            "    }\n" +
                            "}", json.ToString());

            json = new JSON("{\"key1\": \"value1\", \"key2\": {\"key21\":21.6, \"key22\":22.5, \"key23\":23.85}}");
            Assert.AreEqual("{\n" +
                            "    \"key1\": \"value1\",\n" +
                            "    \"key2\": {\n" +
                            "        \"key21\": 21.6,\n" +
                            "        \"key22\": 22.5,\n" +
                            "        \"key23\": 23.85\n" +
                            "    }\n" +
                            "}", json.ToString());

            json = new JSON("{" +
                            "    \"key1\": \"value1\"," +
                            "    \"key2\": {\"key21\": 21, \"key22\": 22}," +
                            "    \"key3\": {" +
                            "        \"key31\": {" +
                            "            \"key311\": \"value311\"," +
                            "            \"key312\": \"value312\"" +
                            "        }," +
                            "        \"key32\": {" +
                            "            \"key321\": \"value321\"" +
                            "        }" +
                            "    }" +
                            "}");
            Assert.AreEqual("{\n" +
                            "    \"key1\": \"value1\",\n" +
                            "    \"key2\": {\n" +
                            "        \"key21\": 21,\n" +
                            "        \"key22\": 22\n" +
                            "    },\n" +
                            "    \"key3\": {\n" +
                            "        \"key31\": {\n" +
                            "            \"key311\": \"value311\",\n" +
                            "            \"key312\": \"value312\"\n" +
                            "        },\n" +
                            "        \"key32\": {\n" +
                            "            \"key321\": \"value321\"\n" +
                            "        }\n" +
                            "    }\n" +
                            "}", json.ToString());
        }
    }
}
