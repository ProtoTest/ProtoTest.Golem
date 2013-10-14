using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Golem.PageObjects.Mailinator
{
    public class InboxPage : BasePageObject
    {
        public Element InboxHeader = new Element("inbox header", By.Id("InboxNameCtrl"));
        public EmailPage OpenEmailWithText(string text, int timeoutSec=30)
        {
            driver.WaitForPresent(ByE.PartialText(text),timeoutSec).Click();
            return new EmailPage();
        }

        public InboxPage WaitForEmail(string text, int timeoutMin)
        {
            driver.WaitForPresent(ByE.PartialText(text), timeoutMin*60);
            return this;
        }

        public override void WaitForElements()
        {
            InboxHeader.Verify.Visible();
        }
    }
}
