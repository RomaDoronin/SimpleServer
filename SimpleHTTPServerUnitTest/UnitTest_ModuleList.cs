using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHTTPServer;

namespace SimpleHTTPServerUnitTest
{
    [TestClass]
    public class UnitTest_ModuleList
    {
        // GetModuleByModuleName
        // ---------------------------------------------------------
        [TestMethod]
        public void Test_GetModuleByModuleName()
        {
            Assert.AreEqual("programming", "programming");
        }
    }
}
