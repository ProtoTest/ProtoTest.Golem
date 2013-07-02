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
            txt_UserName.SendKeys(UserName); 
            txt_mockPass.Click();
            txt_Password.Text = Password;
            btn_Login.Click();
            return new FMX_HomePage();
        }
        
        public override void WaitForElements()
        {
            div_LoginMenu.VerifyVisible(10);
            txt_UserName.VerifyVisible(10);
            txt_Password.VerifyPresent(10);
            btn_Login.VerifyVisible(10);
            btn_ForgotPassword.VerifyVisible(10);
            tab_Home.VerifyVisible(10);
            
        }


    }
}
