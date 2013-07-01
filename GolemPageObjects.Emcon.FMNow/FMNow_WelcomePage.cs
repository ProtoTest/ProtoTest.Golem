using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace GolemPageObjects.Emcon.FMNow
{
    public class FMNow_WelcomePage : BasePageObject

    {
        static Element txt_UserName = new Element("UserName", By.Id("logincontent_Login1_UserName"));
        static Element txt_Password = new Element("Password", By.Id("logincontent_Login1_Password"));
        Element chk_RememberMe = new Element("RememberMe", By.Id("logincontent_Login1_RememberMe"));
        static Element btn_Login = new Element("LoginButton", By.Id("logincontent_Login1_LoginButton"));

        public static FMNow_WelcomePage OpenFMNow(string url)
        {
            TestBaseClass.driver.Navigate().GoToUrl(url);
            return new FMNow_WelcomePage();
        }

        public FMNow_HomePage Login(string user, string pass)
        {
            txt_UserName.Text = user;
            txt_Password.Text = pass;
            btn_Login.WaitUntilVisible().Click();
            return new FMNow_HomePage();
        }

        public override void WaitForElements()
        {
            txt_UserName.WaitUntilVisible();
            txt_Password.WaitUntilVisible();
            btn_Login.WaitUntilVisible();
        }
    }
}
