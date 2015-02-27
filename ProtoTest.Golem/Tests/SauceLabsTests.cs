using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    [Parallelizable]
    internal class SauceLabsTests : WebDriverTestBase
    {
        [FixtureSetUp]
        public void Setup()
        {
            Config.Settings.sauceLabsSettings.SauceLabsUsername = "bkitchener";
            Config.Settings.sauceLabsSettings.SauceLabsAPIKey = "998969ff-ad37-4b2e-9ad7-edacd982bc59";
            Config.Settings.sauceLabsSettings.UseSauceLabs = true;
            Config.Settings.runTimeSettings.RunOnRemoteHost = true;
            Config.Settings.runTimeSettings.Platform = "Windows 8";
            Config.Settings.runTimeSettings.Version = "";
            Config.Settings.runTimeSettings.HighlightFoundElements = false;
            testData.browserInfo = browserInfo;
            LaunchBrowser();
        }

        [Test]
        [Parallelizable]
        public void TestSauceLabs()
        {
            driver.Navigate().GoToUrl("http://www.espn.com");
            var element = new Element("ESPN Element", By.XPath("//*[@class='espn-logo']//*[text()='ESPN']"));
            element.WaitUntil(10).Visible();
            Common.Log("Successfully navigated to " + driver.Title);
        }
    }
}