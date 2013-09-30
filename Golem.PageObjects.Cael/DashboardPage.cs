using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Cael.Setup_Portfolio;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class DashboardPage : BasePageObject
    {
        public LoggedInHeader LoggedInHeader = new LoggedInHeader();
        public Footer Footer = new Footer();

        public Element DIY_Button = new Element("Do It Yourself Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_NewStudent_diyWithPortfolioHyperLink"));
        public Element InstructorLed_Button = new Element("Instructor Led Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_NewStudent_caelWithPortfolioHyperLink"));
        public Element LeftColumn_Container = new Element("Left column container", By.Id("left_col"));
        public Element RightColumn_Container = new Element("Right column container", By.Id("right_col"));

        public Element GetStarted_Link = new Element("Get Started Link", ByE.Text("Get Started"));
        public Element StartAnotherPortfolio_Button = new Element("Start Another portfolio button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_Portfolios_portfolioHyperLink"));

        public EditPortfolioPage OpenPortfolio(string title)
        {
            Element link = new Element("Course Title Link", By.LinkText(title));
            link.Click();
            return new EditPortfolioPage();
        }

        public PaymentPage StartAnotherPortfolio()
        {
            StartAnotherPortfolio_Button.Click();
            return new PaymentPage();
        }

        public SetupPortfolioPage GetStarted()
        {
            GetStarted_Link.Click();
            return new SetupPortfolioPage();
        }



        public ConfirmDIYPage SelectDIYCourse()
        {
            DIY_Button.Click();
            return new ConfirmDIYPage();
        }
        public SelectCaelCoursePage SelectCaelCourse()
        {
            InstructorLed_Button.Click();
            return new SelectCaelCoursePage();
        }

        public override void WaitForElements()
        {
           // LeftColumn_Container.VerifyVisible(30);
           // RightColumn_Container.VerifyVisible(30);
        }
    }
}
