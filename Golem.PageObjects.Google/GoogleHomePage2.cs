using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;
using Golem.PageObjects.Google;

namespace Golem.PageObjects.Google
{
    public class GoogleHomePage2 : BasePageObject
    {
        By searchField = By.Name("q");
        By googleLogo = By.Id("hplogo");
        By searchButton = By.Name("btnK");
        By feelingLuckyButton =  By.Name("btnI");
        By signInButton =  By.ClassName("gbit");
        By gmailbutton = By.ClassName("gbts");

        public static GoogleHomePage2 OpenGoogle()
        {
            WebDriverTestBase.driver.Navigate().GoToUrl("http://www.google.com/");
            return new GoogleHomePage2();
        }

        public GmailPage GoToGmail()
        {
            driver.FindElement(gmailbutton).Click();
            return new GmailPage();
        }

        public GoogleResultsPage SearchFor(string text)
        {
           IWebElement SearchField =  driver.FindElement(searchField);
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
