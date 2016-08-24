using NUnit.Framework;
using Golem.Core;

namespace Golem.Tests
{
    internal class TestBaseTests : TestBase
    {
        [Test]
        public void TestDataContainerCreated()
        {
            Assert.IsNotNull(testData);
            Assert.IsNotNull(testData.VerificationErrors);
            Assert.IsNotNull(testData.actions);
        }
    }
}