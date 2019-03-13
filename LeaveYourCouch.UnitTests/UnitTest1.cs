using System;
using LeaveYourCouch.Mvc;
using LeaveYourCouch.Services;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeaveYourCouch.UnitTests
{
    [TestClass]
    public class EmailServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {

            var result = SecretConfiguration.Get("fake:apclient");

            Assert.AreEqual("668478945646", result);
        }
    }
}
