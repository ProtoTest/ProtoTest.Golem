using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Appium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using MbUnit.Framework;

namespace GoogleTests
{
    class AppiumTest 
    {
        [FixtureInitializer]
        public void setup()
        {
            Config.Settings.runTimeSettings.LaunchBrowser = true;
            Config.Settings.appiumSettings.launchApp = false;
            Config.Settings.appiumSettings.device = "Android";

        }
        [Test]
        public void Test()
        {
           // driver.Navigate().GoToUrl("http://www.espn.com");

        }

        [Test]
        public void Test2()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("device", "Android");
            capabilities.SetCapability("app", "Chrome");
            capabilities.SetCapability(CapabilityType.BrowserName, "Browser");
            capabilities.SetCapability(CapabilityType.Version, "4.3");
            capabilities.SetCapability(CapabilityType.Platform, "WINDOWS");
            IWebDriver driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities);
            driver.Navigate().GoToUrl("http://www.espn.com") ;

        }
    }
}