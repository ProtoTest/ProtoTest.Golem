using MbUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    internal class BrowserTests : WebDriverTestBase
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