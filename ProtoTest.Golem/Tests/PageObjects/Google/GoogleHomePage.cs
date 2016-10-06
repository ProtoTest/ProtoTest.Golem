using OpenQA.Selenium;
using Golem.WebDriver;

namespace Golem.Tests.PageObjects.Google
{
    public class GoogleHomePage : BasePageObject
    {
        private readonly Element feelingLuckyButton = new Element("ImFeelingLuckyButton", By.Name("btnI"));
        private readonly Element gmailbutton = new Element("GmailButton", By.ClassName("gbts"));
        private readonly Element googleLogo = new Element("GoogleLogo", By.Id("hplogo"));
        private readonly Element searchButton = new Element("SearchButton", By.Name("btnK"));
        private readonly Element searchField = new Element("SearchField", By.Name("q"));
        private readonly Element signInButton = new Element("SignInButon", By.LinkText("Sign in"));

        public GmailPage GoToGmail()
        {
            gmailbutton.Click();
            return new GmailPage();
        }

        public GoogleResultsPage SearchFor(string text)
        {
            searchField.Text = text;
            searchField.Submit();
            return new GoogleResultsPage();
        }

        public override void WaitForElements()
        {
            searchField.Verify().Present();
            googleLogo.Verify().Present();
            searchButton.Verify().Present();
            feelingLuckyButton.Verify().Present();
            signInButton.Verify().Present();
        }

        public void VerifyImages()
        {
            searchField.Verify().Image();
            googleLogo.Verify().Image();
            searchButton.Verify().Image();
            feelingLuckyButton.Verify().Image();
            signInButton.Verify().Image();
        }
    }
}