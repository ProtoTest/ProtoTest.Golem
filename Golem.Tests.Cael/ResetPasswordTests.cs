using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallio.Runtime.Formatting;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael;
using Golem.PageObjects.Mailinator;
using MbUnit.Framework;
using HomePage = Golem.PageObjects.Cael.HomePage;

namespace Golem.Tests.Cael
{
    [TestFixture, DependsOn(typeof(UserTests))]
    public class ResetPasswordTests : WebDriverTestBase
    {
        string email_subject = "Request for change password";

        [Test]
        public void ResetPasswordFormValidation()
        {
            HomePage.OpenHomePage().GoToLoginPage().ResetPassword(UserTests.email1);

            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
               Login(UserTests.email1).
               WaitForEmail("Request for change password", 20).
               OpenEmailWithText("Request for change password").ClickTextInBody("this link");

            ResetPasswordPage page = new ResetPasswordPage();
            page.VerifyFormValidations();

            // Cleanup
            DeletePasswordResetEmail();
        }

        [Test]
        public void ResetPassword()
        {
            HomePage.OpenHomePage().GoToLoginPage().ResetPassword(UserTests.email1);

            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
               Login(UserTests.email1).
               WaitForEmail("Request for change password", 20).
               OpenEmailWithText("Request for change password").ClickTextInBody("this link");

            ResetPasswordPage page = new ResetPasswordPage();
            page.ResetPassword(UserTests.password);

            // Cleanup
            DeletePasswordResetEmail();
        }


        [Test, DependsOn("ResetPasswordFormValidation"), DependsOn("ResetPassword")]
        public void ResetPasswordCancelRequest()
        {
            HomePage.OpenHomePage().GoToLoginPage().ResetPassword(UserTests.email1);

            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
               Login(UserTests.email1).
               WaitForEmail("Request for change password", 20).
               OpenEmailWithText("Request for change password").
               ClickLinkInBody("this link", "cancel");

            // I don't want to create a new page just to verify the one label text. 
            // Verify the page has text 'Password reset has been cancelled.'
            WebDriverTestBase.driver.FindElement(OpenQA.Selenium.By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LCResetPassword1_plcMess_lI")).
                Verify().Visible().Verify().Text("Password reset has been cancelled.");
        }

        [Test, DependsOn("ResetPasswordCancelRequest")]
        public void VerifyResetPasswordEmailLinksInvalid()
        {
            string link_not_valid_txt = "This link is no longer valid.";
            string link_not_valid_id = "p_lt_ctl02_pageplaceholder_p_lt_ctl00_LCResetPassword1_plcMess_lE";

            // Click the link in the email to reset the password
            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
               Login(UserTests.email1).
               WaitForEmail(email_subject, 20).
               OpenEmailWithText(email_subject).ClickTextInBody("this link");

            WebDriverTestBase.driver.FindElement(OpenQA.Selenium.By.Id(link_not_valid_id)).Verify().Visible().Verify().Text(link_not_valid_txt);

            // Click the link in the email to cancel the password request
            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
               Login(UserTests.email1).
               WaitForEmail(email_subject, 20).
               OpenEmailWithText(email_subject).
               ClickLinkInBody("this link", "cancel");

            WebDriverTestBase.driver.FindElement(OpenQA.Selenium.By.Id(link_not_valid_id)).Verify().Visible().Verify().Text(link_not_valid_txt);

            // Cleanup and delete the email for the next test
            DeletePasswordResetEmail();
            
        }

        private void DeletePasswordResetEmail()
        {
            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
               Login(UserTests.email1).
               WaitForEmail(email_subject, 20).
               OpenEmailWithText(email_subject).DeleteEmail();
        }
    }
}
