using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael;
using MbUnit.Framework;
using Golem.PageObjects.Cael.Portfolios;

namespace Golem.Tests.Cael
{
    [TestFixture, DependsOn(typeof(Kentico))]
    class AssessmentTests : WebDriverTestBase
    {
        bool AssessorLogin = true;

        [Test]
        [Row("English")]
        void Decline_Assessment(string courseCateogry)
        {
            string decline_reason = "This isn't my area of expertise";

            HomePage.OpenHomePage()
                .GoToLoginPage()
                .Login(UserTests.assessor_email, UserTests.password, false, AssessorLogin).AssessorHeader
                .GoToDashboardPageForAssessor()
                .DeclineAssessment(courseCateogry)
                .ClosePopup()
                .DeclineAssessment(courseCateogry)
                .ConfirmDeclineWithReason(decline_reason);
        }

        [Test, DependsOn("Decline_Assessment")]
        [Row("English")]
        public void Accept_Assessment(string courseCategory)
        {
            HomePage.OpenHomePage()
                .GoToLoginPage()
                .Login(UserTests.assessor_email, UserTests.password, false, AssessorLogin).AssessorHeader
                .GoToDashboardPageForAssessor()
                .AcceptAssessment(courseCategory);
        }

        [Test, DependsOn("Accept_Assessment")]
        public void Verify_Slider_Value_Recommendations()
        {
            // UserTests.email2 is the assessor
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.assessor_email, UserTests.password, false, AssessorLogin).AssessorHeader
                .GoToDashboardPageForAssessor()
                .SelectPendingAssessment()
                .VerifyRecommendationSliderValues();
        }

        [Test, DependsOn("Verify_Slider_Value_Recommendations")]
        public void Assess_Portfolio_Fail_Allow_Resubmit()
        {
            Boolean allowStudentToResubmit = true;

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.assessor_email, UserTests.password, false, AssessorLogin).AssessorHeader
                    .GoToDashboardPageForAssessor()
                    .SelectPendingAssessment()
                    .ReviewAssessmentNoCredit()
                    .ReviewAssessmentConfirm(allowStudentToResubmit)
                    .SubmitAssessment().AssessorHeader.SignOut();

            // Login to Student and have them resubmit the portfolio so Assess_Portfolio_And_Submit can run
            PortfolioTests ptest = new PortfolioTests();
            ptest.SubmitPortfolio("Test Course");
        }   

        [Test,DependsOn("Assess_Portfolio_Fail_Allow_Resubmit")]
        public void Assess_Portfolio_And_Submit()
        {
            Boolean allowStudentToResubmit = true;
            int[] slider_no_credit_values = {4,4,4,4,1,1,1}; // 19
            int[] slider_credit_values = {4,4,4,4,4,1,1}; // 22

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.assessor_email, UserTests.password, false, AssessorLogin).AssessorHeader
                    .GoToDashboardPageForAssessor()
                    .SelectPendingAssessment()
                    .SetSliderValues(slider_no_credit_values)
                    .ReviewAssessmentNoCredit()
                    .ClickCancel()
                    .ReviewAssessmentNoCredit()
                    .ReviewAssessmentConfirm(allowStudentToResubmit).VerifyCreditRecommendation("19/28", false)
                    .GoBackAndEditAssessment().SetSliderValues(slider_credit_values)
                    .ReviewAssessmentCredit()
                    .VerifyCreditRecommendation("22/28", true)
                    .SubmitAssessment();
                    
        }
    }
}
