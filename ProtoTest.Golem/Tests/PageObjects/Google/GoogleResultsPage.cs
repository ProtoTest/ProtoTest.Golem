using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests.PageObjects.Google
{
    public class GoogleResultsPage : BasePageObject
    {
        public class SearchResultItem : BaseComponent
        {
            public Component Link = new Component(By.CssSelector("h3.r>a"));
            public Component Url = new Component(By.CssSelector("cite"));
            public Component Description = new Component(By.CssSelector("span"));
        }

        private readonly Element Gmailbutton = new Element("GmailButton", By.PartialLinkText("Gmail"));
        private readonly Element SearchButton = new Element("SearchButton", By.Name("btnG"));
        private readonly Element SearchField = new Element("SearchField", By.Name("q"));
        private readonly Element SignInButton = new Element("SignInButton", By.LinkText("Sign in"));
        private Element GoogleLogo = new Element("GoogleLogo", By.XPath("//a[@title='Go to Google Home']"));
        private Element searchResult;

        private Components<SearchResultItem> SearchItem = new Components<SearchResultItem>(By.CssSelector("div.g"));

        public void clicktest(string text)
         {
            var component = SearchItem.First(x => x.Text.Contains(text));
            component.Highlight(-1, "grey");
            component.Description.Verify()
            .Text("Selenium is a portable software testing framework for web applications");
        }
        
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