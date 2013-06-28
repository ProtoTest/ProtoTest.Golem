using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Golem.Framework;
using OpenQA.Selenium;
using Golem.PageObjects.Google;

namespace Golem.Framework
{
    class EventTest : TestBaseClass
    {
        [FixtureInitializer]
        public void setup()
        {
            //beforeTestEvent += new ActionEvent(GoogleTest_beforeTestEvent);
           // pageObjectActionEvent += new ActionEvent(GoogleTest_actionEvent);
        }
        
        [Test]
        public void Test()
        {
            GoogleHomePage.OpenGoogle().SearchFor("Selenium").VerifyResult("Selenium - Web Browser Automation");
        }

        void GoogleTest_actionEvent(string name, EventArgs e)
        {
            Common.Log("I SEE : " + name);
        }

        void GoogleTest_beforeTestEvent(string name, EventArgs e)
        {
            Common.Log("I SEE:  " + name);
        }
    }
}
