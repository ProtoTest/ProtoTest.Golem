using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class DashboardPage : BasePageObject
    {
        public HeaderComponent header = new HeaderComponent();

        public Element DIY_Button = new Element("Do It Yourself Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_NewStudent_diyWithPortfolioHyperLink"));
        public Element InstructorLed_Button = new Element("Instructor Led Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_NewStudent_caelWithPortfolioHyperLink"));

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
            DIY_Button.VerifyVisible(30);
            InstructorLed_Button.VerifyVisible(30);
        }
    }
}
