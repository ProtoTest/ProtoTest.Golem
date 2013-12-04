using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
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

        public void ClickLinkInBody(string link_text, string href_partial_text)
        {
            EmailBodyTextField.FindElement(ByE.PartialText(link_text)).GetParent().FindInSiblings(ByE.PartialAttribute("a", "@href", href_partial_text)).Click();
        }

        public void DeleteEmail()
        {
            Element Delete_Button = new Element("Delete email button", By.ClassName("icon-trash"));
            Delete_Button.Click();
        }

        public override void WaitForElements()
        {
            EmailBodyTextField.Verify().Visible();
        }
    }
}
