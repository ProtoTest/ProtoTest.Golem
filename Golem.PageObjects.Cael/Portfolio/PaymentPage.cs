using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class PaymentPage : BasePageObject
    {
        public Element ContinueButton = new Element("Continue Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_DIYWithPortfolio_1_continueButton"));
        public Element CancelLink = new Element("Cancel Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_DIYWithPortfolio_1_cancelHyperLink"));

        public void EnterPayment()
        {
            ContinueButton.Click();

        }

        public override void WaitForElements()
        {
            ContinueButton.VerifyVisible(30);
            CancelLink.VerifyVisible(30);
        }
    }
}
