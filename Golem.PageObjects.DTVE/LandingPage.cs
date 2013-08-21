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
