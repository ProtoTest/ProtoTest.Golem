using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class SelectCaelCoursePage : BasePageObject
    {

        public Element CourseDropdown = new Element("Course Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SelectCael_100WithPortfolio_courseSectionDDList"));
        public Element DIYCourseLink = new Element("DIY Course Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SelectCael_100WithPortfolio_selfDirectHyperLink"));
        public Element EnterPaymentButton = new Element("Enter Payment Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SelectCael_100WithPortfolio_enterPaymentButton"));
        public Element CancelLink = new Element("Cancel Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SelectCael_100WithPortfolio_cancelHyperLink"));

        public DashboardPage CancelToDashboard()
        {
            CancelLink.Click();
            return new DashboardPage();
        }

        public PaymentPage SelectCourse(string name)
        {
            CourseDropdown.SelectOption(name);
            EnterPaymentButton.Click();
            return new PaymentPage();
        }

        public override void WaitForElements()
        {
            CourseDropdown.Verify.Present();
            EnterPaymentButton.Verify.Visible();
        }
    }
}
