using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests.PageObjects.Google
{
    public class GoogleResultsPage : BasePageObject
    {

        Element GoogleLogo = new Element("GoogleLogo", By.XPath("//a[@title='Go to Google Home']"));
        Element SearchField = new Element("SearchField", By.Name("q"));
        Element SearchButton = new Element("SearchButton", By.Name("btnG"));
        Element SignInButton = new Element("SignInButton", By.LinkText("Sign in"));
        Element Gmailbutton = new Element("GmailButton", By.PartialLinkText("Gmail"));
        private Element searchResult;

        public Element SearchResult(string text)
        {
            searchResult = new Element("SearchResultLink", By.PartialLinkText(text));
            return searchResult;
        }

        public GoogleResultsPage SearchFor(string text)
        {
            SearchField.SetText(text);
            SearchField.Submit();
            return new GoogleResultsPage();
        }

        public GoogleResultsPage VerifyResult(string text)
        {
            SearchResult(text).Verify().Present();
            return new GoogleResultsPage();
        }

        public SearchResultPage GoToResult(string text)
        {
            SearchResult(text).Click();
            return new SearchResultPage();
        }

        public GmailPage GoToGmail()
        {
            Gmailbutton.Click();
            return new GmailPage();
        }



        public override void WaitForElements()
        {
            SearchField.Verify().Present();
            //GoogleLogo.Verify().Present();
            SearchButton.Verify().Present();
            SignInButton.Verify().Present();
        }
    }
}
