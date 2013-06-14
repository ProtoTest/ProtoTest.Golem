using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Golem.Framework;
using OpenQA.Selenium;
using Golem.Framework.PageObjects.Google;

namespace Golem.Tests.Google
{
    public class GoogleTest : TestBaseClass
    {

        [Test]
        public static void Test()
        {
            GoogleHomePage.
                OpenGoogle().
                SearchFor("Selenium").
                VerifyResult("Selenium - Web Browser Automation").
                GoToResult("Selenium - Web Browser Automation");
        }

        [Test]
        [Row("Selenium", "Selenium - Web Browser Automation")]
        [Row("ProtoTest", "ProtoTest - IT Staffing and Mobile App Testing Lab")]
        [Row("Soasta", "SOASTA - Wikipedia, the free encyclopedia")]
        public void DDT_Test(string searchText, string searchResult)
        {
            GoogleHomePage.OpenGoogle().SearchFor(searchText).VerifyResult(searchResult);
        }
        
    }
}
