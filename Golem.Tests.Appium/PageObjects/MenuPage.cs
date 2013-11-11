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
    public class MenuPage : BasePageObject
    {
        public Element btn_Search = new Element("SearchButton", By.TagName("ImageView"));

        public SearchForBook ClickSearchButton()
        {
            driver.FindElements(By.TagName("ImageView"))[4].Click();
            return new SearchForBook();
        }

        public override void WaitForElements()
        {
            btn_Search.Verify().Present();
        }
    }
}
