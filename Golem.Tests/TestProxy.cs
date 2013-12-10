using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using ProtoTest.Golem.Core;

namespace Golem.Tests
{
    class TestProxy : TestBase
    {
        [FixtureInitializer]
        public void setup()
        {
            Config.Settings.httpProxy.startProxy = true;
            Config.Settings.httpProxy.useProxy = true;
        }

        [Test]
        public void TestProxyNotNull()
        {
            Assert.IsNotNull(proxy);
        }
       
    }
}
