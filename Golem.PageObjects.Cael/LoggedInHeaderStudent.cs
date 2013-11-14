using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael.MyAccount;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class LoggedInHeaderStudent : LoggedInHeader
    {
        public Element Portfolios_Link = new Element("Portfolios Link", By.LinkText("Portfolios"));
        public Element Advising_Link = new Element("Advising Link", By.LinkText("Advising"));

        public PortfoliosPage GoToPortfoliosPage()
        {
            Portfolios_Link.Click();
            return new PortfoliosPage();
        }

        public void GoToAdvisingPage()
        {
            Advising_Link.Click();
            //return new AdvisingPage();
        }

        public override void WaitForElements()
        {
            base.WaitForElements();
            Portfolios_Link.Verify().Visible();
            Advising_Link.Verify().Visible();
        }
    }
}
