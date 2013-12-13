using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;

namespace Golem.PageObjects.Google
{
    public class GoogleResultsPage : BasePageObject
    {

        Element GoogleLogo = new Element("GoogleLogo", By.ClassName("gb_d"));
        Element SearchField = new Element("SearchField", By.Name("q"));
        Element SearchButton = new Element("SearchButton", By.Name("btnK"));
        Element SignInButton = new Element("SignInButton", By.LinkText("Sign in"));
        Element Gmailbutton = new Element("GmailButton", By.PartialLinkText("Gmail"));
        private Element searchResult;

        public Element SearchResult(string text)
        {
            searchResult = new Element("SearchResultLink", By.PartialLinkText(text));
            return searchResult;
        }

        private string searchString;

        public GoogleResultsPage SearchFor(string text)
        {
            SearchField.SendKeys(text);
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
            GoogleLogo.Verify().Present();
            SearchButton.Verify().Present();
            SignInButton.Verify().Present();
        }
    }
}
