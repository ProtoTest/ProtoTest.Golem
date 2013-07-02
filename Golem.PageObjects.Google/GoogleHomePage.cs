using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.PageObjects.Google;
using OpenQA.Selenium;

namespace Golem.PageObjects.Google
{
    public class GoogleHomePage : BasePageObject
    {

        Element searchField = new Element("SearchField", By.Name("q"));
        Element googleLogo = new Element("GoogleLogo", By.Id("hplogo"));
        Element searchButton = new Element("SearchButton", By.Name("btnK"));
        Element feelingLuckyButton = new Element("ImFeelingLuckyButton", By.Name("btnI"));
        Element signInButton = new Element("SignInButon", By.ClassName("gbit"));
        Element gmailbutton = new Element("GmailButton", By.ClassName("gbts"));

        public static GoogleHomePage OpenGoogle()
        {
            
            TestBaseClass.driver.Navigate().GoToUrl("http://www.google.com/");
            return new GoogleHomePage();
        }

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
            searchField.VerifyPresent(10);
            googleLogo.VerifyPresent(10);
            searchButton.VerifyPresent(10);
            feelingLuckyButton.VerifyPresent(10);
            signInButton.VerifyPresent(10);
        }
    }
}
