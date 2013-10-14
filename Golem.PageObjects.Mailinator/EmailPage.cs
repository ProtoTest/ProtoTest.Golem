using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Mailinator
{
    public class EmailPage : BasePageObject
    {
        public Element EmailBodyTextField = new Element("Email body text field", By.ClassName("mailview"));

        public void ClickTextInBody(string text)
        {
            EmailBodyTextField.FindElement(ByE.PartialText(text)).Click();
        }

        public override void WaitForElements()
        {
            EmailBodyTextField.Verify.Visible();
        }
    }
}
