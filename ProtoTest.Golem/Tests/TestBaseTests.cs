using MbUnit.Framework;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.Tests
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