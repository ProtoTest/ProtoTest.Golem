using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Cael;
using MbUnit.Framework;

namespace Golem.Tests.Cael
{
    class AssessmentTests : TestBaseClass
    {
        //string email = "prototestassessor@mailinator.com";
        //string password = "prototest123!!";

        [Test]
        void VerifySliderValueRecommendations()
        {
            // UserTests.email2 is the assessor
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email2, UserTests.password).Header.GoToDashboardPageForAssessor().SelectPendingAssessment().VerifyRecommendationSliderValues();
        }

        [Test]
        void AssessPortfolioAndSubmitTest()
        {
            int[] slider_no_credit_values = {4,4,4,4,1,1,1}; // 19
            int[] slider_credit_values = {4,4,4,4,4,1,1}; // 22

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email2, UserTests.password).Header
                    .GoToDashboardPageForAssessor()
                    .SelectPendingAssessment()
                    .SetSliderValues(slider_no_credit_values)
                    .ReviewAssessmentNoCredit()
                    .ClickCancel()
                    .ReviewAssessmentNoCredit()
                    .ReviewAssessmentConfirm(true).VerifyCreditRecommendation("19/28", false)
                    .GoBackAndEditAssessment().SetSliderValues(slider_credit_values)
                    .ReviewAssessmentCredit()
                    .VerifyCreditRecommendation("22/28", true)
                    .SubmitAssessment();
                    
        }
       
    }
}
