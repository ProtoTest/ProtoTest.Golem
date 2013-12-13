using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.WebDriver.Elements.Types;
using OpenQA.Selenium;
//using ProtoTest.Golem.WebDriver.Elements.Types;

namespace Golem.PageObjects.Cael
{
    public class LoginPage : BasePageObject
    {
        public Field EmailField = new Field("Email Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_UserName"));
        public Field PasswordField = new Field("PasswordField", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_Password"));
        public Button SignInButton = new Button("SignInButton", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_LoginButton"));
        public Checkbox RememberMeButton = new Checkbox("RememberMeCheckbox", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_rememberMeCheckBox"));
        public Link ForgotPassword_Link = new Link("Forgot password link", By.LinkText("Forgot password"));

        public LoggedOutHeader Header = new LoggedOutHeader();
        public Footer Footer = new Footer();

        public static bool isAssessor;

        public DashboardPage Login(string email, string password, bool rememberMe=false, bool assessorLogin=false)
        {
            isAssessor = assessorLogin;

            EmailField.WaitUntil().Visible().Text = email;
            PasswordField.Text = password;
            RememberMeButton.SetCheckbox(rememberMe);
            SignInButton.WaitUntil().Present().Click();
            return new DashboardPage();
        }

        public override void WaitForElements()
        {
            EmailField.Verify().Visible();
            SignInButton.Verify().Visible();
            ForgotPassword_Link.Verify().Visible();
        }

        public void ResetPassword(string email)
        {
            string form_validation_txt = "Please enter your email address";
            string password_sent_txt = "Request for password change was sent.";

            Field Email_Field = new Field("Reset password email field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_retrievePasswordTextBox"));
            Button SendPassword_Button = new Button("Reset password - send password button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_retrievePasswordButton"));
            Element Validation_Label = new Element("No email entered label", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_rqValue"));
            Element PasswordSent_Label = new Element("Password sent label", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_resultLabel"));

            ForgotPassword_Link.Click();

            // First verify form validation for no email entered
            SendPassword_Button.WaitUntil().Visible().Click();
            Validation_Label.WaitUntil().Visible().Verify().Text(form_validation_txt);

            // Officially reset the password
            Email_Field.WaitUntil().Visible().Text = email;
            SendPassword_Button.Click();
            PasswordSent_Label.WaitUntil().Visible().Verify().Text(password_sent_txt);
        }
    }
}
