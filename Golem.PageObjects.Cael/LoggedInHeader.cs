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
    public class LoggedInHeader : BasePageObject
    {
        public Element Welcome_Link = new Element("Welcome Link", ByE.PartialText("Welcome,"));
        public Element SignOut_Link = new Element("Sign Out Link", By.Id("p_lt_ctl00_SignOutButton_btnSignOutLink"));
        public Element Logo_Button = new Element("Logo", By.Id("logo"));

        public Element Dashboard_Link = new Element("Dashboard Link", By.LinkText("Dashboard"));
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

        public override void WaitForElements()
        {
            Welcome_Link.Verify().Visible();
            SignOut_Link.Verify().Visible();
            MyAccount_Link.Verify().Visible();
            Dashboard_Link.Verify().Visible();
        }
    }
}
