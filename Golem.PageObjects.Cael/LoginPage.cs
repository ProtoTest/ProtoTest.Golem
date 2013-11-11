using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.WebDriver.Elements.Types;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver.Elements.Types;

namespace Golem.PageObjects.Cael
{
    public class LoginPage : BasePageObject
    {
        public Field EmailField = new Field("Email Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_UserName"));
        public Field PasswordField = new Field("PasswordField", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_Password"));
        public Button SignInButton = new Button("SignInButton", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_LoginButton"));
        public Checkbox RememberMeButton = new Checkbox("RememberMeCheckbox", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_rememberMeCheckBox"));
        public LoggedOutHeader Header = new LoggedOutHeader();
        public Footer Footer = new Footer();

        public DashboardPage Login(string email, string password, bool rememberMe=false)
        {

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
        }
    }
}
