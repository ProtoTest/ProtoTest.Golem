using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Safari;
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
        public void TestIE()
        {
            driver = new InternetExplorerDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Quit();
        }
        [Test]
        public void TestFF()
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Quit();
        }
        [Test]
        public void TestChrome()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Quit();
        }
        [Test]
        public void TestSafari()
        {
            driver = new SafariDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            driver.Quit();
        }
    }
}
