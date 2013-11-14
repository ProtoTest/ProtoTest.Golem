using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael.MyAccount;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class LoggedInHeaderAssessor : LoggedInHeader
    {
        private const string TAG = "LoggedInHeaderAssessor - ";

        public Element Assessments_Link = new Element(TAG + "Assessments Link", By.LinkText("Assessments"));

        public void GotoAssessmentsPage()
        {
            Assessments_Link.Click();
            // return some page
        }

        public override void WaitForElements()
        {
            base.WaitForElements();
            Assessments_Link.Verify().Visible();
        }
    }
}