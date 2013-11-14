using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael.MyAccount;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    
    public abstract class MyAccountPage : BasePageObject
    {
        public LoggedInHeaderStudent StudentHeader = null;
        public LoggedInHeaderAssessor AssessorHeader = null;

        public Element ContantInfoButton = new Element("Contant Info Button", By.XPath("//span[text()='Contact Information']"));
        public Element ProfileButton = new Element("Profile Button", By.XPath("//span[text()='Profile']"));
        public Element PasswordButton = new Element("Password Button", By.XPath("//span[text()='Password']"));

        public MyAccountPage()
        {
            if (LoginPage.isAssessor)
            {
                AssessorHeader = new LoggedInHeaderAssessor();
            }
            else
            {
                StudentHeader = new LoggedInHeaderStudent();
            }
        }

        public ContactInfoPage GoToContactInfoPage()
        {
          ContantInfoButton.Click();
            return new ContactInfoPage();
        }
        public ProfilePage GoToProfilePage()
        {
            ProfileButton.Click();
            return new ProfilePage();
        }
        public PasswordPage GoToPasswordPage()
        {
            PasswordButton.Click();
            return new PasswordPage();
        }
    }
}
