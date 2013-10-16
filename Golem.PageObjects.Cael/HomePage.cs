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
        public Element LoginButton = new Element("Login Link",By.Id("p_lt_ctl00_SignOutButton_btnSignOutLink"));
        public Element CreateUserLink = new Element("Create User Link",By.LinkText("Create Account"));
        public Element ContentContainer = new Element("Content Container", By.Id("container_content_inner"));
        public Element ScrollingContent = new Element("Scrolling content box", By.Id("myScroll"));
        public Element ContactUs_Button = new Element("Contact Us Button", By.LinkText("CONTACT US"));
        public Element FindOutNow_Button = new Element("Find out now Button", By.LinkText("FIND OUT NOW"));
        public LoggedOutHeader Header = new LoggedOutHeader();
        public Footer Footer = new Footer();

        public static HomePage OpenHomePage()
        {
            TestBaseClass.driver.Navigate().GoToUrl(Config.GetConfigValue("EnvUrl", "http://lcdev.bluemodus.com/"));
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
            ContentContainer.Verify.Visible();
            LoginButton.Verify.Visible();
            CreateUserLink.Verify.Visible();
            ScrollingContent.Verify.Visible();
        }
    }
}
