using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium.IE;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    class BrowserTests : WebDriverTestBase
    {
        [FixtureInitializer]
        public void Init()
        {
            Config.Settings.runTimeSettings.LaunchBrowser = false;

        }
        [Test]
        public void TestBrowsers()
        {
            driver = new InternetExplorerDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Close();
        }
    }
}
