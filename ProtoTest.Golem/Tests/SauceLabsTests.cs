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
    }
}
