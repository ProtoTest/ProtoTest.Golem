using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.DTVE
{
    public class LandingPage : BasePageObject
    {
        public Element ShowFilterDropdown = new Element("ShowFiltersDropdown", ByE.Text("Show Filters"));
        public Element HideFilterDropdown = new Element("HideFiltersDropdown", ByE.Text("Hide Filters"));

        public override void WaitForElements()
        {
            
        }

        public LandingPage SelectFilter(string name)
        {
            string elementPath = @"//li[@data-filter='" + name + "']";
            driver.FindElement(By.XPath(elementPath)).Click();
            return new LandingPage();
        }

        public LandingPage VerifyNumberOfResult(string number)
        {

            Element textElement = new Element("Results Element", By.Id("browseResultsCount"));
            driver.WaitForElementWithText(number);
            return new LandingPage();
        }

        public LandingPage ShowAllFilters()
        {
            ShowFilterDropdown.Click();
            return new LandingPage();
        }

        public LandingPage HideAllFilters()
        {
            HideFilterDropdown.Click();
            return new LandingPage();
        }
    }
}
