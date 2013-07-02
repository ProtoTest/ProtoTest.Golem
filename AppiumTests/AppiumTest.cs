using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.PageObjects.Appium.Bookworm;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

namespace GoogleTests
{
    class AppiumTest : TestBaseClass
    {
 
        [Test]
        public void Test()
        {
            SplashPage.OpenBookWorm()
                      .ClickTapToBeginLink()
                      .ClickSearchButton()
                      .SearchFor("tolkein")
                      .AddBookByTitle("The Magical World of the Inklings")
                      .GoBack();
           
        }
    }
}
