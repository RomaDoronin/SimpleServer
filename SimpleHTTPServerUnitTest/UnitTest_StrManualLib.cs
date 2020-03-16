using System;
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
            Assert.AreEqual(" love programmi", output);

            input = "love";
            output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("o", output);

            input = "lov";
            output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("", output);

            input = "lov\"";
            output = StrManualLib.RemoveSpecialSymbol(input);
            Assert.AreEqual("ov", output);
        }

        [TestMethod]
        public void Test_RemoveSpecialSymbol_InputStringLengthLessThree()
        {
            Assert.AreEqual("", StrManualLib.RemoveSpecialSymbol("\"\""));
            Assert.AreEqual("", StrManualLib.RemoveSpecialSymbol("l\""));

            var ex = Assert.ThrowsException<Exception>(() => StrManualLib.RemoveSpecialSymbol("l"));
            Assert.AreEqual(ex.Message, "Invalid input string");
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

            var ex = Assert.ThrowsException<Exception>(() => StrManualLib.ConstFormatToStringFormat(""));
            Assert.AreEqual(ex.Message, "Invalid input string");
        }

        // CheckStringForJsonFormat
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_CheckStringForJsonFormat_TEST1()
        {
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat("{}"));
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat("{ }"));
            Assert.IsTrue(StrManualLib.CheckStringForJsonFormat("{ \"\":\"\" }"));
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_TEST2()
        {
            Assert.AreEqual(1, 0);
        }

        [TestMethod]
        public void Test_CheckStringForJsonFormat_TEST3()
        {
            Assert.AreEqual(1, 0);
        }
    }
}
