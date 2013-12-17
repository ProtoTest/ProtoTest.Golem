using System;
using System.Collections.Generic;
using System.Text;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.PageObjects.Google;
using OpenQA.Selenium;

namespace ProtoTest.Golem.PageObjects.Google
{
    public class GoogleHomePage : BasePageObject
    {

        Element searchField = new Element("SearchField", By.Name("q"));
        Element googleLogo = new Element("GoogleLogo", By.Id("hplogo"));
        Element searchButton = new Element("SearchButton", By.Name("btnK"));
        Element feelingLuckyButton = new Element("ImFeelingLuckyButton", By.Name("btnI"));
        Element signInButton = new Element("SignInButon", By.LinkText("Sign in"));
        Element gmailbutton = new Element("GmailButton", By.ClassName("gbts"));

        public static GoogleHomePage OpenGoogle()
        {
            
            WebDriverTestBase.driver.Navigate().GoToUrl(@"http://www.google.com/");
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
