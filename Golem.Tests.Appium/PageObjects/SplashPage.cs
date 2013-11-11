using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppiumTests.PageObjects;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;

namespace Golem.PageObjects.Appium.Bookworm
{
    public class SplashPage : BasePageObject
    {


        public Element lnk_TapToBegin = new Element("TapToBegin",By.Name("Tap to begin"));

        public static SplashPage OpenBookWorm()
        {
            return new SplashPage();
        }

        public override void WaitForElements()
        {
            lnk_TapToBegin.Verify().Present();
        }
        public MenuPage ClickTapToBeginLink()
        {
            lnk_TapToBegin.Click();
            return new MenuPage();
        }
    }
}
