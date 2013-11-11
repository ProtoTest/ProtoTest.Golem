using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver.Elements;

namespace Golem.PageObjects.Cael
{
    public class PurchaseConfirmationPage : BasePageObject
    {
        public Element ReturnToDashboard_Link = new Element("Return to Dashboard link",By.LinkText("Return to your Dashboard"));
        public DashboardPage ReturnToDashboardPage()
        {
            ReturnToDashboard_Link.Click();
            return new DashboardPage();
        }

        public override void WaitForElements()
        {
            ReturnToDashboard_Link.Verify().Visible();
        }
    }
}
