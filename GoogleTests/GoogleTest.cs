using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Golem;
using OpenQA.Selenium;

namespace Golem
{
    class GoogleTest : TestBaseClass
    {
        [FixtureInitializer]
        public void setup()
        {
            beforeTestEvent += new ActionEvent(GoogleTest_beforeTestEvent);
            pageObjectActionEvent += new ActionEvent(GoogleTest_actionEvent);
        }
        
        [Test]
        public void Test()
        {
            
            
            GoogleHomePage.OpenGoogle().SearchFor("Testing");
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
