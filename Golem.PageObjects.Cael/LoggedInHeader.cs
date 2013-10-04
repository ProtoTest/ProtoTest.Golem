using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Cael.MyAccount;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class LoggedInHeader : BasePageObject
    {
        public Element Welcome_Link = new Element("Welcome Link", ByE.PartialText("Welcome,"));
        public Element SignOut_Link = new Element("Sign Out Link", By.Id("p_lt_ctl00_SignOutButton_btnSignOutLink"));
        public Element Logo_Button = new Element("Logo", By.Id("logo"));

        public Element Dashboard_Link = new Element("Dashboard Link", By.LinkText("Dashboard"));
        public Element Portfolios_Link = new Element("Portfolios Link", By.LinkText("Portfolios"));
        public Element Advising_Link = new Element("Advising Link", By.LinkText("Advising"));
        public Element MyAccount_Link = new Element("MyAccount Button", By.LinkText("My Account"));

        public ContactInfoPage ClickWelcomeLink()
        {
            Welcome_Link.Click();
            return new ContactInfoPage();
        }



        public ContactInfoPage GoToMyAccountPage()
        {
            MyAccount_Link.Click();
            return new ContactInfoPage();
        }

        public HomePage SignOut()
        {
            SignOut_Link.Click();
            return new HomePage();
        }

        public DashboardPage GoToDashboardPage()
        {
            Dashboard_Link.Click();
            return new DashboardPage();
        }

        public DashBoardAssessmentPage GoToDashboardPageForAssessor()
        {
            Dashboard_Link.Click();
            return new DashBoardAssessmentPage();
        }

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
            Welcome_Link.VerifyVisible(30);
            SignOut_Link.VerifyVisible(30);
            Dashboard_Link.VerifyVisible(30);
            Portfolios_Link.VerifyVisible(30);
            MyAccount_Link.VerifyVisible(30);
            Advising_Link.VerifyVisible(30);
        }
    }
}
