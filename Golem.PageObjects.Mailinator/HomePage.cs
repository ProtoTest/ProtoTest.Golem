using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;


namespace Golem.PageObjects.Mailinator
{
    public class HomePage : BasePageObject
    {
        public Element EmailField = new Element("Email Field", By.Id("inboxfield"));
        public Element CheckItButton = new Element("Check It Button", ByE.PartialText("Check it"));

        public static HomePage OpenPage()
        {
            TestBaseClass.driver.Navigate().GoToUrl("http://mailinator.com/");
            return new HomePage();
        }
        public InboxPage Login(string email)
        {
            EmailField.Text = email;
            CheckItButton.Click();
            return new InboxPage();

        }

        public override void WaitForElements()
        {
            EmailField.VerifyPresent(30);
        }
    }
}
