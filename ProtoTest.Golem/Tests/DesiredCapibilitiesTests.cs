using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    internal class DesiredCapibilitiesTests : WebDriverTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            var data = testData;
            Config.Settings.sauceLabsSettings.SauceLabsUsername = "bkitchener";
            Config.Settings.sauceLabsSettings.SauceLabsAPIKey = "998969ff-ad37-4b2e-9ad7-edacd982bc59";
            Config.Settings.sauceLabsSettings.UseSauceLabs = true;
            Config.Settings.runTimeSettings.RunOnRemoteHost = true;
            Config.Settings.runTimeSettings.HighlightFoundElements = false;
            browserInfo = new BrowserInfo(WebDriverBrowser.Browser.Firefox);
            base.SetUp();
        }

        [Test]
        [Parallelizable]
        public void TestSauceLabs()
        {
            var newData = testData;
            driver.Navigate().GoToUrl("http://www.espn.com");
            var element = new Element("ESPN Element", By.XPath("//*[@class='espn-logo']//*[text()='ESPN']"));
            element.WaitUntil(10).Visible();
            Common.Log("Successfully navigated to " + driver.Title);
        }
    }
}
