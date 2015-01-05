using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    [Parallelizable]
    class SauceLabsTests : WebDriverTestBase
    {
        [FixtureInitializer]
        public void Setup()
        {
            Config.Settings.sauceLabsSettings.SauceLabsUsername = "USERNAME";
            Config.Settings.sauceLabsSettings.SauceLabsAPIKey = "APIKEY";
            Config.Settings.sauceLabsSettings.UseSauceLabs = true;
            Config.Settings.runTimeSettings.RunOnRemoteHost = true;
            Config.Settings.runTimeSettings.Platform = "Windows 7";
            Config.Settings.runTimeSettings.Version = "";
            Config.Settings.runTimeSettings.HighlightFoundElements = false;
        }
        [Test]
        [Parallelizable]
        public void Test()
        {
            driver.Navigate().GoToUrl("http://www.espn.com");
            var element = new Element("ESPN Element", By.XPath("//*[@class='espn-logo']//*[text()='ESPN']"));
            element.WaitUntil(10).Visible();
            Common.Log("Successfully navigated to " + driver.Title);
        }

        //[Test]
        //public void sauceTest()
        //{
        //    Uri commandExecutorUri = new Uri("http://ondemand.saucelabs.com/wd/hub");

        //     set up the desired capabilities
        //    DesiredCapabilities desiredCapabilites = new DesiredCapabilities("Firefox", "", Platform.CurrentPlatform); // set the desired browser
        //    desiredCapabilites.SetCapability("platform", "Windows 7"); // operating system to use
        //    desiredCapabilites.SetCapability("username", "bkitchener"); // supply sauce labs username
        //    desiredCapabilites.SetCapability("accessKey", "998969ff-ad37-4b2e-9ad7-edacd982bc59");  // supply sauce labs account key
        //    desiredCapabilites.SetCapability("name", TestContext.CurrentContext.Test.Name); // give the test a name

        //     start a new remote web driver session on sauce labs
        //    var _Driver = new RemoteWebDriver(commandExecutorUri, desiredCapabilites);
        //    _Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
        //    _Driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));

        //     navigate to the page under test
        //    _Driver.Navigate().GoToUrl("https://saucelabs.com/test/guinea-pig");
        //}
    }
}
