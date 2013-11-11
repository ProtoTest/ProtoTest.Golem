using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class ReviewAssessmentConfirmModal : BasePageObject
    {
        Element StudentResubmit_Yes_Radio = new Element("Student Re-submit portfolio YES Radio", By.Id("rdYes"));
        Element StudentResubmit_No_Radio = new Element("Student Re-submit portfolio NO Radio", By.Id("rdNo"));
        Element Cancel_Button = new Element("Cancel Button", By.XPath("//div[@class='left-button-helper2']/p/a"));
        Element ContinueReviewAssessment_Button = new Element("Continue to review assessment Button", By.Id("bContinue"));

        public AssessPortfolioPage ClickCancel()
        {
            Cancel_Button.Click();
            return new AssessPortfolioPage();
        }

        /// <summary>
        ///     Submit on the Review Assessment Confirm modal popup
        /// </summary>
        /// <param name="studentResubmit">Check radio to allow the student to resubmit the portfolio</param>
        public ReviewAssessmentConfirmPage ReviewAssessmentConfirm(Boolean studentResubmit)
        {
            if (studentResubmit == true)
            {
                StudentResubmit_Yes_Radio.Click();
            }
            else
            {
                StudentResubmit_No_Radio.Click();
            }

            ContinueReviewAssessment_Button.Click();
            return new ReviewAssessmentConfirmPage();
        }

        public override void WaitForElements()
        {

            ContinueReviewAssessment_Button.Verify().Visible().Verify().Text("CONTINUE TO REVIEW ASSESSMENT");
        }
    }
}
