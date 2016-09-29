using NUnit.Framework;
using OpenQA.Selenium;
using Golem.WebDriver;

namespace Golem.Tests
{
    internal class TestDriver : WebDriverTestBase
    {
        [Test]
        public void TestWebElementExtensions()
        {
            driver.Navigate().GoToUrl("http://google.com");
            driver.WaitForPresent(By.Name("q"), 20).Highlight();
            driver.WaitForNotVisible(By.Name("zas"));
        }

        [Test]
        public void TestWebDriverExtensions()
        {
            driver.Navigate().GoToUrl("http://google.com");
            driver.WaitForElementWithText("Advertisingz").Highlight();
            driver.FindElementWithText("Advertising").Highlight();
        }
    }
}