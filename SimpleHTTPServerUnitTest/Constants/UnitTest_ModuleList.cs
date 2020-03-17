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
            SimpleHTTPServer.Modules.IModule module;

            module = SimpleHTTPServer.Constants.ModuleList.GetModuleByModuleName(SimpleHTTPServer.Constants.ModuleList.MODULE_AUTH);
            Assert.AreEqual((new SimpleHTTPServer.Modules.Auth()).GetType(), module.GetType());

            module = SimpleHTTPServer.Constants.ModuleList.GetModuleByModuleName(SimpleHTTPServer.Constants.ModuleList.MODULE_PATIENT);
            Assert.AreEqual((new SimpleHTTPServer.Modules.Patient()).GetType(), module.GetType());

            module = SimpleHTTPServer.Constants.ModuleList.GetModuleByModuleName(SimpleHTTPServer.Constants.ModuleList.MODULE_REGIST);
            Assert.AreEqual((new SimpleHTTPServer.Modules.Regist()).GetType(), module.GetType());
        }

        [TestMethod]
        public void Test_GetModuleByModuleName_ExtremeCases()
        {
            SimpleHTTPServer.Modules.IModule module;

            module = SimpleHTTPServer.Constants.ModuleList.GetModuleByModuleName("RandomModule");
            Assert.IsNull(module);

            module = SimpleHTTPServer.Constants.ModuleList.GetModuleByModuleName("");
            Assert.IsNull(module);

            string str = null;
            module = SimpleHTTPServer.Constants.ModuleList.GetModuleByModuleName(str);
            Assert.IsNull(module);
        }
    }
}
