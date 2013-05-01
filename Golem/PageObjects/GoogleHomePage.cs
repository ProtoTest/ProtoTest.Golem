using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem
{
    public class GoogleHomePage : BasePageObject
    {

        Element searchField = new Element("SearchField", By.Name("q"));
        Element googleLogo = new Element("GoogleLogo", By.Id("hplogo"));
        Element searchButton = new Element("SearchButton", By.Name("btnK"));
        Element feelingLuckyButton = new Element("ImFeelingLuckyButton", By.Name("btnI"));
        Element signInButton = new Element("SignInButon", By.ClassName("gbit"));


        public static GoogleHomePage OpenGoogle()
        {
            TestBaseClass.driver.Navigate().GoToUrl("http://www.google.com/");
            return new GoogleHomePage();
        }

        public GoogleResultsPage SearchFor(string text)
        {
            searchField.Text = text;
            searchField.Submit();
            return new GoogleResultsPage();
        }

        public override void WaitForElements()
        {
            searchField.WaitForVisible();
            googleLogo.WaitForVisible();
            searchButton.WaitForVisible();
            feelingLuckyButton.WaitForVisible();
            signInButton.WaitForVisible();
        }

        public override void ValidateLoaded()
        {
        }


    }
}
