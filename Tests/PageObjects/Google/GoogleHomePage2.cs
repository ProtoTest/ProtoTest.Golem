using OpenQA.Selenium;
using Golem.WebDriver;

namespace Golem.Tests.PageObjects.Google
{
    public class GoogleHomePage2 : BasePageObject
    {
        private readonly By feelingLuckyButton = By.Name("btnI");
        private readonly By gmailbutton = By.ClassName("gbts");
        private readonly By googleLogo = By.Id("hplogo");
        private readonly By searchButton = By.Name("btnK");
        private readonly By searchField = By.Name("q");
        private readonly By signInButton = By.ClassName("gbit");

        public GmailPage GoToGmail()
        {
            driver.FindElement(gmailbutton).Click();
            return new GmailPage();
        }

        public GoogleResultsPage SearchFor(string text)
        {
            var SearchField = driver.FindElement(searchField);
            SearchField.Clear();
            SearchField.SendKeys(text);
            SearchField.Submit();
            return new GoogleResultsPage();
        }

        public override void WaitForElements()
        {
            driver.WaitForPresent(searchField);
            driver.WaitForPresent(searchButton);
            driver.WaitForPresent(googleLogo);
            driver.WaitForPresent(feelingLuckyButton);
            driver.WaitForPresent(signInButton);
        }
    }
}