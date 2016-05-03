using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    class TestInstance : TestSource
    {

        [Test]
        public void test()
        {
            Log.Message(this.browserInfo.browser.ToString());
        }
    }
    
    [TestFixture, TestFixtureSource("GetBrowsers")]
    class TestSource
    {
        protected BrowserInfo browserInfo;
        public TestSource()
        {
            Log.Message("BASE");
        }

        public TestSource(BrowserInfo browser)
        {
            this.browserInfo = browser;
        }

        protected static IEnumerable<BrowserInfo> GetBrowsers()
        {
            return Config.Settings.runTimeSettings.Browsers;
        }
    }
}
