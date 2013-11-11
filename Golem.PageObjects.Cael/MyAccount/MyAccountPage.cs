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
        public LoggedInHeader LoggedInHeader = new LoggedInHeader();

        public Element ContantInfoButton = new Element("Contant Info Button", By.XPath("//span[text()='Contact Information']"));
        public Element ProfileButton = new Element("Profile Button", By.XPath("//span[@id='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_LocalizedLabel1']"));
        public Element PasswordButton = new Element("Password Button", By.XPath("//span[@id='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_LocalizedLabel2']"));

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
