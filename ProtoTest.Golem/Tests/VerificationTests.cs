using System.Collections.Generic;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    internal class VerificationTests : TestBase
    {
        [Test]
        public void TestVerificationCount()
        {
            Assert.AreEqual(testData.VerificationErrors.Count, 0);
            AddVerificationError("Test error");
            Assert.AreEqual(testData.VerificationErrors.Count, 1);
            testData.VerificationErrors = new List<VerificationError>();
        }

        [Test]
        public void TestAssertionCount()
        {
            Assert.True(true == true);
            var result = TestContext.CurrentContext.Result;
//            Assert.AreEqual(TestContext.CurrentContext.Result.PassCount, 0);
            AddVerificationError("Test Error");
//            Assert.AreEqual(TestContext.CurrentContext.Test., 2);
            testData.VerificationErrors = new List<VerificationError>();
        }
    }
}