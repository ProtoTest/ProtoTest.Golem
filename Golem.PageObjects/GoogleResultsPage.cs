using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.PageObjects.Google
{
    public class GoogleResultsPage : BasePageObject
    {
        
        Element GoogleLogo = new Element("GoogleLogo", By.ClassName("gbqlca"));
        Element SearchField = new Element("SearchField", By.Name("q"));
        Element SearchButton = new Element("SearchButton", By.Name("btnK"));
        Element SignInButton = new Element("SignInButton", By.ClassName("gbit"));
        Element Gmailbutton = new Element("GmailButton", By.PartialLinkText("Gmail"));

        public GoogleResultsPage() { }
        public Element SearchResult(string text)
        {
            return new Element("SearchResultLink", By.PartialLinkText(text));
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
            SearchResult(text).VerifyPresent(10);
            return new GoogleResultsPage();
        }

        public void GoToResult(string text)
        {
            SearchResult(text).Click();
            return;
        }

        public GmailPage GoToGmail()
        {
            Gmailbutton.Click();
            return new GmailPage();
        }



        public override void WaitForElements()
        {
            SearchField.VerifyPresent(10);
            GoogleLogo.VerifyPresent(10);
            SearchButton.VerifyPresent(10);
            SignInButton.VerifyPresent(10);
        }
    }
}
