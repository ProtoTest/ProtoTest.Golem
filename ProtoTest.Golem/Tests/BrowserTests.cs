using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using Golem.Core;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
{
    internal class BrowserTests : WebDriverTestBase
    {
        [OneTimeSetUp]
        public void Init()
        {
            Config.settings.runTimeSettings.LaunchBrowser = false;
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Config.settings.runTimeSettings.LaunchBrowser = true;
        }

        [Test]
        public void TestIE()
        {
            driver = new InternetExplorerDriver();
            OpenPage<GoogleHomePage>("http://www.google.com/");
            driver.Quit();
        }

        [Test]
        public void TestFF()
        {
            driver = new FirefoxDriver();
            OpenPage<GoogleHomePage>("http://www.google.com/");
            driver.Quit();
        }

        [Test]
        public void TestChrome()
        {
            driver = new ChromeDriver();
            OpenPage<GoogleHomePage>("http://www.google.com/");
            driver.Quit();
        }

        [Test]
        public void TestPhantomJS()
        {
            driver = new PhantomJSDriver();
            OpenPage<GoogleHomePage>("http://www.google.com/");
            driver.Quit();
        }

        [Test]
        public void TestSafari()
        {
            driver = new SafariDriver();
            OpenPage<GoogleHomePage>("http://www.google.com/");
            driver.Quit();
        }
    }
}