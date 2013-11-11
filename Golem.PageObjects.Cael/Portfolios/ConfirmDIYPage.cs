using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class ConfirmDIYPage : BasePageObject
    {

        public Element EnterPaymentButton = new Element("Enter payment button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_DIYWithPortfolio_enterPaymentButton"));
        public Element InstructorLedLink = new Element("Instructor-Led Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_DIYWithPortfolio_caelWithPortfolioHLink"));
        public Element CancelLink = new Element("Cancel Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_DIYWithPortfolio_cancelHyperLink"));

        public PaymentPage GoToPaymentPage()
        {
            EnterPaymentButton.Click();
            return new PaymentPage();
        }

        public SelectCaelCoursePage GoToInstructorLedCourse()
        {
            InstructorLedLink.Click();
            return new SelectCaelCoursePage();
        }


        public override void WaitForElements()
        {
            
        }
    }
}
