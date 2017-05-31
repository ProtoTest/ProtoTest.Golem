using NUnit.Framework;
using OpenQA.Selenium;
using Golem.Core;
using Golem.WebDriver;

namespace Golem.Tests
{
    [Parallelizable]
    internal class SauceLabsTests : WebDriverTestBase
    {
        [SetUp]
        public override void SetUp()
        {
            Config.settings.sauceLabsSettings.SauceLabsUsername = "bkitchener";
            Config.settings.sauceLabsSettings.SauceLabsAPIKey = "998969ff-ad37-4b2e-9ad7-edacd982bc59";
            Config.settings.sauceLabsSettings.UseSauceLabs = true;
            Config.settings.runTimeSettings.RunOnRemoteHost = true;
            Config.settings.runTimeSettings.HighlightFoundElements = false;
            this.browser = WebDriverBrowser.Browser.IE;
           base.SetUp();
        }

        [Test]
        [Parallelizable]
        public void TestSauceLabs()
        {
            driver.Navigate().GoToUrl("http://www.espn.com");
            var element = new Element("ESPN Element", By.XPath("//*[@class='espn-logo']//*[text()='ESPN']"));
            element.WaitUntil(10).Visible();
            Log.Message("Successfully navigated to " + driver.Title);
        }
    }
}