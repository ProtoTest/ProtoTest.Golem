using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FmXwelcomePage : BasePageObject
    {
        
        Element div_LoginMenu = new Element("LoginMenuBar", By.Id("ctl00_pnlMenu"));
        //these elements belong to the LoginMenu bar
        Element txt_UserName = new Element("UserName", By.Id("ctl00_cphLogin_Login1_UserName"));
        Element txt_mockPass = new Element("mockPass", By.Id("ctl00_cphLogin_Login1_mockpass"));
        Element txt_Password = new Element("Password", By.Id("ctl00_cphLogin_Login1_Password"));
        Element btn_Login = new Element("LoginButton", By.Id("ctl00_cphLogin_Login1_Login"));
        Element btn_ForgotPassword = new Element("ForgotPasswordButton", By.Id("ctl00_cphLogin_Login1_lbtnFgtPss"));

        //Home tab
        Element tab_Home = new Element("HomeTab", By.Id("ctl00_lnkLogin"));

        public static FmXwelcomePage OpenFMX(string url)
        {
            TestBaseClass.driver.Navigate().GoToUrl(url);
            return new FmXwelcomePage();
        }

        public FmXwelcomePage ForgotPassword()
        {
            btn_ForgotPassword.Click();
            return new FmXwelcomePage();
        }

        public FMX_HomePage Login(string UserName, string Password)
        {
            //Common.GetCurrentClassAndMethodName();
            txt_UserName.WaitUntil.Present();
            txt_UserName.WaitUntil.Visible().Text = UserName;
            txt_mockPass.WaitUntil.Present();
            txt_mockPass.WaitUntil.Visible().Click();
            txt_Password.WaitUntil.Visible().Text = Password;
            btn_Login.WaitUntil.Visible().Click();
            return new FMX_HomePage();
        }
        
        public override void WaitForElements()
        {
            div_LoginMenu.Verify.Visible();
            txt_UserName.Verify.Visible();
            txt_Password.Verify.Present();
            btn_Login.Verify.Visible();
            btn_ForgotPassword.Verify.Visible();
            tab_Home.Verify.Visible();
            
        }


    }
}
