using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Edge;
using Golem.Core;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;
using OpenQA.Selenium.Remote;

namespace Golem.Tests
{
    internal class BrowserTests : TestBase
    {
        //        [OneTimeSetUp]
        //        public void Init()
        //        {
        //            Config.settings.runTimeSettings.LaunchBrowser = false;
        //        }
        //
        //        [OneTimeTearDown]
        //        public void Teardown()
        //        {
        //            Config.settings.runTimeSettings.LaunchBrowser = true;
        //        }
        //
        //        [Test]
        //        public void TestIE()
        //        {
        //            driver = new InternetExplorerDriver();
        //            OpenPage<GoogleHomePage>("http://www.google.com/");
        //            driver.Quit();
        //        }
        //
        //        [Test]
        //        public void TestFF()
        //        {
        //            driver = new FirefoxDriver();
        //            OpenPage<GoogleHomePage>("http://www.google.com/");
        //            driver.Quit();
        //        }
        //
        //        [Test]
        //        public void TestChrome()
        //        {
        //            driver = new ChromeDriver();
        //            OpenPage<GoogleHomePage>("http://www.google.com/");
        //            driver.Quit();
        //        }
        //
        //        [Test]
        //        public void TestPhantomJS()
        //        {
        //            driver = new PhantomJSDriver();
        //            OpenPage<GoogleHomePage>("http://www.google.com/");
        //            driver.Quit();
        //        }
        //
        //        [Test]
        //        public void TestSafari()
        //        {
        //            driver = new SafariDriver();
        //            OpenPage<GoogleHomePage>("http://www.google.com/");
        //            driver.Quit();
        //        }
        //
        //        [Test]
        //        public void TestEdge()
        //        {
        //            driver = new EdgeDriver();
        //            OpenPage<GoogleHomePage>("http://www.google.com/");
        //            driver.Quit();
        //        }

        [Test]
        public void TestRemote()
        {
            var browserInfo = new BrowserInfo(WebDriverBrowser.Browser.Chrome);
            
            var count = Config.settings.runTimeSettings.Browsers.Count();
            if (count > 0)
            {
                browserInfo = Config.settings.runTimeSettings.Browsers.First();
            }

            var driver = new WebDriverBrowser().LaunchRemoteBrowser(browserInfo.browser,
                "127.0.0.1");
            driver.Navigate().GoToUrl("http://www.google.com/");
            driver.Quit();
        }
    }
}