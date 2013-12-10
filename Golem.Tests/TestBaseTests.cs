using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.PageObjects.Google;
using MbUnit.Framework;
using ProtoTest.Golem.Core;

namespace Golem.Tests
{
    class TestBaseTests : TestBase
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
