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
    [TestFixture]
    class DashboardTests : WebDriverTestBase
    {
        [Test]
        public void SetupCaelCourse()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email2, UserTests.password)
                .Header.GoToDashboardPage().
                SelectInstructorLedCourse().
                SelectCourse("December 1").
                EnterPayment().
                ReturnToDashboardPage();
        }

        [Test]
        public void SetupDIYCourse()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .Header.GoToDashboardPage().
                SelectDIYCourse().
                GoToPaymentPage().
                EnterPayment().
                ReturnToDashboardPage();

        }
    }
}
