using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;

namespace SimpleHTTPServerUnitTest
{
    [TestClass]
    public class UnitTest_StrManualLib
    {
        // GetNextWordWithDelete
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_GetNextWordWithDelete_WithSpaces()
        {
            string input = "I love programming";

            string output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("I", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("love", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("programming", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("", output);
        }

        [TestMethod]
        public void Test_GetNextWordWithDelete_WithSlashN()
        {
            string input = "I\nlove\nprogramming";

            string output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("I", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("love", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("programming", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("", output);
        }

        [TestMethod]
        public void Test_GetNextWordWithDelete_WithSlashR()
        {
            string input = "I\rlove\rprogramming";

            string output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("I", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("love", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("programming", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("", output);
        }

        [TestMethod]
        public void Test_GetNextWordWithDelete_AllSymbol()
        {
            string input = " I \rlove\r\n\rprogramming  \n";

            string output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("I", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("love", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("programming", output);

            output = StrManualLib.GetNextWordWithDelete(ref input);
            Assert.AreEqual("", output);
        }

        // RemoveSpecialSymbol
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_RemoveSpecialSymbol_RemoveQuotationMarks()
        {
            string input = "\"I love programming\"";
            string output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("I love programming", output);
        }

        [TestMethod]
        public void Test_RemoveSpecialSymbol_RemoveQuotationMarksWithComma()
        {
            string input = "\"I love programming\",";
            string output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("I love programming", output);
        }

        [TestMethod]
        public void Test_RemoveSpecialSymbol_AnyString()
        {
            string input = "I love programming";
            string output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("I love programming", output);

            input = "love";
            output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("love", output);

            input = "lov";
            output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("lov", output);

            input = "lov\"";
            output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("lov", output);
        }

        [TestMethod]
        public void Test_RemoveSpecialSymbol_InputStringLengthLessThree()
        {
            Assert.AreEqual("", StrManualLib.RemoveSpecialSymbol("\"\""));
            Assert.AreEqual("l", StrManualLib.RemoveSpecialSymbol("l\""));
            Assert.AreEqual("l", StrManualLib.RemoveSpecialSymbol("l"));
        }

        // GenerateRandomString
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_GenerateRandomString_CheckGenerateStringLength()
        {
            for (int length = 0; length < 32; length += 4)
            {
                string output = StrManualLib.GenerateRandomString(length);
                Assert.AreEqual(output.Length, length);
            }
        }

        // ConstFormatToStringFormat
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_ConstFormatToStringFormat_NormalCases()
        {
            Assert.AreEqual("Test word", StrManualLib.ConstFormatToStringFormat("TEST_WORD"));
            Assert.AreEqual("Test word s", StrManualLib.ConstFormatToStringFormat("TEST_WORD_S"));
            Assert.AreEqual("Test word s many", StrManualLib.ConstFormatToStringFormat("TEST_WORD_S_MANY"));
        }

        [TestMethod]
        public void Test_ConstFormatToStringFormat_ExtremeCases()
        {
            Assert.AreEqual(" test word s many", StrManualLib.ConstFormatToStringFormat("_TEST_WORD_S_MANY"));
            Assert.AreEqual(" ", StrManualLib.ConstFormatToStringFormat("_"));
            Assert.AreEqual("dsaf asdf sadf", StrManualLib.ConstFormatToStringFormat("dsaf asdf sadf"));
            Assert.AreEqual("Dsaf asdf sadf", StrManualLib.ConstFormatToStringFormat("DSAF asdf sadf"));
            Assert.AreEqual("", StrManualLib.ConstFormatToStringFormat(""));
        }

        // CheckStringForJsonFormat
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_CheckStringForJsonFormat_OneValue()
        {
            string input = "{\n" +
                           "    \"key\": \"value\"\n" +
                           "}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));
 
            input = "{\n    \"key\": \"value\"\n}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\n\"key\": \"value\"\n}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key\": \"value\"}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key\":\"value\"}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_AbsentOpeningBracket()
        {
            string input = "\"key\":\"value\"}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_AbsentClosingBracket()
        {
            string input = "{\"key\":\"value\"";
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat(input));
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_AbsentColon()
        {
            string input = "{\"key\" \"value\"}";
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat(input));
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_AbsentQuotationMark()
        {
            string input = "{key\": \"value\"}";
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key: \"value\"}";
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat(input));

            input = "{key: \"value\"}";
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key\": value\"}";
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key\": \"value}";
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key\": 12}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key\": true}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_ManyValue()
        {
            string input = "{\n" +
                           "    \"key1\": \"value1\",\n" +
                           "    \"key2\": \"value2\"\n" +
                           "}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\n\"key1\":\"value1\",\n\"key2\":\"value2\"\n}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key1\":\"value1\",\"key2\":\"value2\"}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\n" +
                    "    \"key1\": \"value1\",\n" +
                    "    \"key2\": \"value2\",\n" +
                    "    \"key3\": \"value3\"\n" +
                    "}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\n\"key1\": \"value1\",\n\"key2\": \"value2\",\n\"key3\": \"value3\"\n}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));

            input = "{\"key1\":\"value1\",\"key2\":\"value2\",\"key3\":\"value3\"}";
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat(input));
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_EmptyInput()
        {
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat("{}"));
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat("{ }"));
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat("{\n}"));

            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat("{ \"\":\"\" }"));
            Assert.IsFalse(StrManualLib.CheckStringForJsonFormat("{ \"\":\"value\" }"));
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat("{ \"key\":\"\" }"));
        }

        // DeleteFirstAndLastChar
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_DeleteFirstAndLastChar()
        {
            Assert.AreEqual("\"qwerty\"", StrManualLib.DeleteFirstAndLastChar("{\"qwerty\"}"));
            Assert.AreEqual("qwerty", StrManualLib.DeleteFirstAndLastChar("\"qwerty\""));
            Assert.AreEqual("wert", StrManualLib.DeleteFirstAndLastChar("qwerty"));
            Assert.AreEqual("er", StrManualLib.DeleteFirstAndLastChar("wert"));
            Assert.AreEqual("", StrManualLib.DeleteFirstAndLastChar("er"));
            Assert.AreEqual("", StrManualLib.DeleteFirstAndLastChar("a"));
            Assert.AreEqual("", StrManualLib.DeleteFirstAndLastChar(""));
        }

        // ReplaceHighObjectWithMark and ReplaceMarkWithHighObject
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_ReplaceHighObjectWithMark_StringValue()
        {
            string startStr = "\"key\":\"value\"";
            List<string> subObjectList = new List<string>();
            string actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0>:<1>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("\"value\"", subObjectList[1]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);
        }

        [TestMethod]
        public void Test_ReplaceHighObjectWithMark_StringWithSpace()
        {
            string startStr = "\"key\" : \"value\"";
            List<string> subObjectList = new List<string>();
            string actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0> : <1>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("\"value\"", subObjectList[1]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);
        }

        [TestMethod]
        public void Test_ReplaceHighObjectWithMark_StringWithBrackets()
        {
            string startStr = "\"key\":\"[value]\",\"key1\":\"{value}\"";
            List<string> subObjectList = new List<string>();
            string actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0>:<1>,<2>:<3>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("\"[value]\"", subObjectList[1]);
            Assert.AreEqual("\"key1\"", subObjectList[2]);
            Assert.AreEqual("\"{value}\"", subObjectList[3]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);
        }

        [TestMethod]
        public void Test_ReplaceHighObjectWithMark_StringValueWithSpace()
        {
            string startStr = "\"key\":\"value value\"";
            List<string> subObjectList = new List<string>();
            string actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0>:<1>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("\"value value\"", subObjectList[1]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);

            startStr = "\"key\":\"value _ value\"";
            subObjectList = new List<string>();
            actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0>:<1>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("\"value _ value\"", subObjectList[1]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);
        }

        [TestMethod]
        public void Test_ReplaceHighObjectWithMark_ListValue()
        {
            string startStr = "\"key\":[1,2,3]";
            List<string> subObjectList = new List<string>();
            string actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0>:<1>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("[1,2,3]", subObjectList[1]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);
        }

        [TestMethod]
        public void Test_ReplaceHighObjectWithMark_ListOfStringValue()
        {
            string startStr = "\"key\":[\"str1\", \"str2\", \"str3\"]";
            List<string> subObjectList = new List<string>();
            string actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0>:<1>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("[\"str1\", \"str2\", \"str3\"]", subObjectList[1]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);
        }

        [TestMethod]
        public void Test_ReplaceHighObjectWithMark_JsonValue()
        {
            string startStr = "\"key\":{\"str1\":\"str2\"}";
            List<string> subObjectList = new List<string>();
            string actualStr = StrManualLib.ReplaceHighObjectWithMark(startStr, subObjectList);
            Assert.AreEqual("<0>:<1>", actualStr);
            Assert.AreEqual("\"key\"", subObjectList[0]);
            Assert.AreEqual("{\"str1\":\"str2\"}", subObjectList[1]);

            actualStr = StrManualLib.ReplaceMarkWithHighObject(new string[] { actualStr }, subObjectList)[0];
            Assert.AreEqual(startStr, actualStr);
        }

        // SmartSplit
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_SmartSplit_OneJsonPair()
        {
            string str = "\"key\":\"value\"";
            string[] words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key\"");
            Assert.AreEqual(words[1], "\"value\"");
        }

        [TestMethod]
        public void Test_SmartSplit_ManyJsonPair()
        {
            string str = "\"key1\":\"value1\",\"key2\":\"value2\"";
            string[] words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "\"value2\"");

            str = "\"key1\":\"value1\",\"key2\":\"value2\",\"key3\":\"value3\"";
            words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "\"value2\"");
            Assert.AreEqual(words[4], "\"key3\"");
            Assert.AreEqual(words[5], "\"value3\"");
        }

        [TestMethod]
        public void Test_SmartSplit_InternalJson()
        {
            string str = "\"key1\":\"value1\",\"key2\":{\"key21\":\"value21\"}";
            string[] words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "{\"key21\":\"value21\"}");

            str = "\"key1\":\"value1\",\"key2\":{\"key21\":\"value21\"},\"key3\":{\"key31\":{\"key311\":\"value311\"}}";
            words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "{\"key21\":\"value21\"}");
            Assert.AreEqual(words[4], "\"key3\"");
            Assert.AreEqual(words[5], "{\"key31\":{\"key311\":\"value311\"}}");

            str = "\"key1\":\"value1\",\"key2\":{\"key21\":\"value21\"},\"key3\":{\"key31\":{\"key311\":\"value311\"}},\"key4\":{}";
            words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "{\"key21\":\"value21\"}");
            Assert.AreEqual(words[4], "\"key3\"");
            Assert.AreEqual(words[5], "{\"key31\":{\"key311\":\"value311\"}}");
            Assert.AreEqual(words[6], "\"key4\"");
            Assert.AreEqual(words[7], "{}");
        }

        [TestMethod]
        public void Test_SmartSplit_InternalList()
        {
            string str = "\"key1\":\"value1\",\"key2\":[\"value21\",\"value22\",\"value23\"]";
            string[] words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "[\"value21\",\"value22\",\"value23\"]");

            str = "\"key1\":\"value1\",\"key2\":[\"value21\",\"value22\",\"value23\"],\"key3\":[]";
            words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "[\"value21\",\"value22\",\"value23\"]");
            Assert.AreEqual(words[4], "\"key3\"");
            Assert.AreEqual(words[5], "[]");

            str = "\"key1\":\"value1\",\"key2\":[[\"value211\",\"value212\"],[\"value221\",\"value222\"],[\"value231\",\"value232\"]]";
            words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "[[\"value211\",\"value212\"],[\"value221\",\"value222\"],[\"value231\",\"value232\"]]");
        }

        [TestMethod]
        public void Test_SmartSplit_InternalListPlusJson()
        {
            string str = "\"key1\":\"value1\",\"key2\":{\"key21\":[{\"key211\":\"value211\"},{\"key212\":\"value212\"},{\"key213\":\"value213\"}]}";
            string[] words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "\"value1\"");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "{\"key21\":[{\"key211\":\"value211\"},{\"key212\":\"value212\"},{\"key213\":\"value213\"}]}");
        }

        [TestMethod]
        public void Test_SmartSplit_ListOfJson()
        {
            string str = "\"key1\":[{\"key11\":\"value11\"},{\"key12\":\"value12\"},{\"key13\":\"value13\"}]";
            string[] words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "[{\"key11\":\"value11\"},{\"key12\":\"value12\"},{\"key13\":\"value13\"}]");
        }

        [TestMethod]
        public void Test_SmartSplit_ListOfJsonPlusJsonOfList()
        {
            string str = "\"key1\":[{\"key11\":\"value11\"},{\"key12\":\"value12\"},{\"key13\":\"value13\"}],\"key2\":{\"key21\":[5,1,4]}";
            string[] words = StrManualLib.SmartSplit(str, new char[] { ':', ',' });
            Assert.AreEqual(words[0], "\"key1\"");
            Assert.AreEqual(words[1], "[{\"key11\":\"value11\"},{\"key12\":\"value12\"},{\"key13\":\"value13\"}]");
            Assert.AreEqual(words[2], "\"key2\"");
            Assert.AreEqual(words[3], "{\"key21\":[5,1,4]}");
        }
    }
}
