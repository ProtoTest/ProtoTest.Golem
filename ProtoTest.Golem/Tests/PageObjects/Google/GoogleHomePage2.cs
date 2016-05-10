using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests.PageObjects.Google
{
    public class GoogleHomePage2 : BasePageObject
    {
        private readonly OpenQA.Selenium.By feelingLuckyButton = By.Name("btnI");
        private readonly OpenQA.Selenium.By gmailbutton = By.ClassName("gbts");
        private readonly OpenQA.Selenium.By googleLogo = By.Id("hplogo");
        private readonly OpenQA.Selenium.By searchButton = By.Name("btnK");
        private readonly OpenQA.Selenium.By searchField = By.Name("q");
        private readonly OpenQA.Selenium.By signInButton = By.ClassName("gbit");

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