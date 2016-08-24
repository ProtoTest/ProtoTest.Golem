using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium;
using Golem.Core;
using Golem.WebDriver;

namespace Golem.Tests.PageObjects.Google
{
    public class GoogleResultsPage : BasePageObject
    {
        public class SearchResultItem : BaseComponent
        {
            public Element Link => new Element(this, By.CssSelector("h3.r>a"));

            public Element Url => new Element(this, By.CssSelector("cite"));

            public Element Description => new Element(this, By.CssSelector("span.st"));
        }

        public class GoogleResultsHeader : BaseComponent
        {
            public Element Logo => new Element(this, By.Id("logo"));
            public Element Search => new Element(this, By.CssSelector("input"));

            public GoogleResultsHeader(By by) : base(by)
            {
            }
        }

        private readonly Element Gmailbutton = new Element("GmailButton", By.PartialLinkText("Gmail"));
        private readonly Element SearchButton = new Element("SearchButton", By.Name("btnG"));
        private readonly Element SearchField = new Element("SearchField", By.Name("q"));
        private readonly Element SignInButton = new Element("SignInButton", By.LinkText("Sign in"));
        private Element GoogleLogo = new Element("GoogleLogo", By.XPath("//a[@title='Go to Google Home']"));
        private Element searchResult;

        public Components<SearchResultItem> SearchItem = new Components<SearchResultItem>(By.CssSelector("div.g"));
        public GoogleResultsHeader Header = new GoogleResultsHeader(By.Id("tsf"));

        public void clicktest(string text)
         {
            var component = SearchItem.First(x => x.Text.Contains(text));
            component.Description.Verify()
            .Text("Selenium is a portable software testing framework for web applications");

            var second = SearchItem.First(x => x.Text.Contains("GitHub"));
            second.Description.Verify().Not().Text("wikipedia");

            Header.Search.Verify().Visible();
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