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
    class DeclineAssessmentPopup : BasePageObject
    {
        public Element Close_Button = new Element("Decline Assessment Popup - CLOSE Button", By.XPath("//div[@id='diaglogDA']/div")); 
        public Element TakeIt_Button = new Element("Decline Assessment Popup - I'LL TAKE IT Button", By.Id("btnNevermind"));
        public Element Decline_Button = new Element("Decline Assessment Popup - DECLINE button", By.Id("btnDecline"));
        public Element Decline_ReasonDrop = new Element("Decline Assessment Popup - Reason Dropdown", By.Id("ddDeclineReason"));
        public Element Decline_Optional = new Element("Decline Assessment Popup - Tell Us More text field", By.Id("txtDeclineComments"));

        public DashBoardAssessmentPage ClosePopup()
        {
            Close_Button.Click();
            return new DashBoardAssessmentPage();
        }

        public DashBoardAssessmentPage ConfirmDeclineWithReason(string reason=null, string optionalTellUsMore=null)
        {
            if (reason != null)  Decline_ReasonDrop.SelectOption(reason);
            if(optionalTellUsMore != null)   Decline_Optional.Text = optionalTellUsMore;
            Decline_Button.Click();
            return new DashBoardAssessmentPage();
        }

        public override void WaitForElements()
        {
            Close_Button.Verify.Visible();
            TakeIt_Button.Verify.Visible();
        }
    }
}
