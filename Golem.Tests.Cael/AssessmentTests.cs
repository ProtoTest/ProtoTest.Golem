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
        string email = "prototestassessor@mailinator.com";
        string password = "prototest123!!";

        [Test]
        void VerifySliderValueRecommendations()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(email, password).Header.GoToDashboardPageForAssessor().SelectPendingAssessment().VerifyRecommendationSliderValues();
        }
    }
}
