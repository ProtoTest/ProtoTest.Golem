using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class HomePage : BasePageObject
    {
        public HomePage()
        {

            WaitForElements();
        }
        public Element LoginButton = new Element("Login Link",By.Id("p_lt_ctl00_SignOutButton_btnSignOutLink"));
        public Element CreateUserLink = new Element("Create User Link",By.LinkText("Create Account"));
        public Element ContentContainer = new Element("Content Container",By.Id("container_content_inner"));

        
        public static HomePage OpenHomePage()
        {
            TestBaseClass.driver.Navigate().GoToUrl(@"http://lcdev.bluemodus.com/");
            return new HomePage();
        }
        public LoginPage GoToLoginPage()
        {
            LoginButton.Click();
            return new LoginPage();
        }

        public CreateUserPage GoToCreateUserPage()
        {
            CreateUserLink.Click();
            return new CreateUserPage();
        }

        public override void WaitForElements()
        {
            ContentContainer.VerifyVisible(60);
            LoginButton.VerifyVisible(30);
            CreateUserLink.VerifyVisible(30);
        }
    }
}
