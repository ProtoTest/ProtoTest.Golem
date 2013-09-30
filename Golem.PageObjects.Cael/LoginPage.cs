using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class LoginPage : BasePageObject
    {
        public Element EmailField = new Element("Email Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_UserName"));
        public Element PasswordField = new Element("PasswordField", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_Password"));
        public Element SignInButton = new Element("SignInButton", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_LoginButton"));
        public Element RememberMeButton = new Element("RememberMeCheckbox", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SignIn_login_rememberMeCheckBox"));
        public LoggedOutHeader Header = new LoggedOutHeader();
        public Footer Footer = new Footer();

        public DashboardPage Login(string email, string password, bool rememberMe=false)
        {
            EmailField.Text = email;
            PasswordField.Text = password;
            RememberMeButton.SetCheckbox(rememberMe);
            SignInButton.Click();
            return new DashboardPage();
        }

        public override void WaitForElements()
        {
            EmailField.VerifyVisible(30);
            SignInButton.VerifyVisible(30);
        }
    }
}
