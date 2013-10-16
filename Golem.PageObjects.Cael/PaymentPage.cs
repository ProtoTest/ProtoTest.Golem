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
        public Element ContinueButton = new Element("Continue Button",
            ByE.PartialAttribute("input", "@id", "continueButton"));
        public Element CancelLink = new Element("Cancel Link",By.LinkText("Cancel"));

        public PurchaseConfirmationPage EnterPayment()
        {
            ContinueButton.Click();
            return new PurchaseConfirmationPage();
        }

        public override void WaitForElements()
        {
            ContinueButton.Verify.Visible();
            CancelLink.Verify.Visible();
        }
    }

    public class PortfolioPaymentPage : PaymentPage
    {
        
    }
}
