using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Golem
{
    public class TestBaseClass
    {
        public static event ActionEvent beforeTestEvent;
        public static event ActionEvent afterTestEvent;
        public static event ActionEvent pageObjectActionEvent;
        public static event ActionEvent beforeCommandEvent;
        public static event ActionEvent afterCommandEvent;
        public static event ActionEvent beforeSuiteEvent;
        public static event ActionEvent afterSuiteEvent;
        public static event ActionEvent genericEvent;


        public delegate void ActionEvent(string name, EventArgs e);

        public ActionList actions;

        public static void FireEvent(string name)
        {
                EventArgs e = null;
                genericEvent(name, e);
        }

        private void WriteActionToLog(string name,EventArgs e)
        {
            Common.Log(name + " : " + DateTime.Now.ToString("HH:mm:ss::ffff"));
            //actions.addAction(name, time);
        }

        private static IWebDriver _driver;
        public static IWebDriver driver
        {
            get
            {
                return _driver;
            }
            set
            {
                _driver = value;
            }

        }

        public static System.Diagnostics.Stopwatch stopwatch;

        public class GollemEventArgs : EventArgs
        {
            public string name;
            public DateTime time;
        }

        [SetUp]
        public void SetUp()
        {
            beforeTestEvent(Gallio.Framework.TestContext.CurrentContext.Test.FullName, null);
            driver = new WebDriverBrowser().LaunchBrowser();
            //FireEvent("Browser Launched", null);

        }

        [TearDown]
        public void TearDown()
        {
            afterTestEvent(Gallio.Framework.TestContext.CurrentContext.Test.FullName,null);
            driver.Quit();
           // FireEvent("Browser Closed");
            actions.PrintActions();

        }

        [FixtureInitializer]
        public void Initializer()
        {
            actions = new ActionList();
            pageObjectActionEvent += new ActionEvent(WriteActionToLog);
            beforeTestEvent += new ActionEvent(WriteActionToLog);
            afterTestEvent += new ActionEvent(WriteActionToLog);
            genericEvent += new ActionEvent(WriteActionToLog);
            
        }

        [FixtureSetUp]
        public void SuiteSetUp()
        {
            //FireEvent("Suite Started");
        }

        [FixtureTearDown]
        public void SuiteTearDown()
        {
           // FireEvent("Suite Finished");
        }

    }
}
