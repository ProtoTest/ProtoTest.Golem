using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.PageObjects.Google;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace Golem.Tests
{
    class TestProxy : WebDriverTestBase
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

        [Test]
        public void TestProxyWorks()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/");
        }
       
    }
}
