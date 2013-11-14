using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael;
using MbUnit.Framework;

namespace Golem.Tests.Cael
{
    [TestFixture, DependsOn(typeof(UserTests))]
    class DashboardTests : WebDriverTestBase
    {
        [Test]
        public void SetupCaelCourse()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email2, UserTests.password)
                .StudentHeader.GoToDashboardPage().
                SelectInstructorLedCourse().
                SelectCourse("February 01, 2014").
                EnterPayment().
                ReturnToDashboardPage();
        }

        [Test]
        public void SetupDIYCourse()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .StudentHeader.GoToDashboardPage().
                SelectDIYCourse().
                GoToPaymentPage().
                EnterPayment().
                ReturnToDashboardPage();

        }
    }
}
