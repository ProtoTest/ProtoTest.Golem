using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Common.Markup;
using Gallio.Framework.Pattern;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using Gallio.Framework;
using Gallio.Model;
using Gallio.Common.Media;
using Golem.Framework;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;

namespace Golem.Framework
{
    public class EventFiringTest
    {
        public class GolemEventArgs : EventArgs
        {
            public string eventName;
            public string testName;
            public string suiteName;
            public string fullName;
            public string pageObjectName;
            public string pageObjectActionName;
            public DateTime timeStamp;
            public IWebDriver driver;
            public GolemEventArgs(string eventName = "")
            {
                this.eventName = eventName;
                if (TestBaseClass.driver != null)
                    this.driver = TestBaseClass.driver;
                this.testName = Common.GetCurrentTestFunctionName();
                this.suiteName = Common.GetCurrentSuiteName();
                this.fullName = Common.GetCurrentTestName();
                this.pageObjectName = Common.GetCurrentClassName();
                this.pageObjectActionName = Common.GetCurrentMethodName();
                this.timeStamp = Common.GetCurrentTimeStamp();
            }

            public GolemEventArgs(string eventName, string objectName, string actionName)
            {
                this.eventName = eventName;
                if (TestBaseClass.driver != null)
                    this.driver = TestBaseClass.driver;
                this.testName = Common.GetCurrentTestFunctionName();
                this.suiteName = Common.GetCurrentSuiteName();
                this.fullName = Common.GetCurrentTestName();
                this.pageObjectName = objectName;
                this.pageObjectActionName = actionName;
                this.timeStamp = Common.GetCurrentTimeStamp();
            }

            public string GetEventInfo()
            {
                string message = "";
                message += timeStamp + " : " + fullName + " : ";
                if (pageObjectName != "")
                {
                    if (pageObjectActionName != "")
                    {
                        message += pageObjectName + "." + pageObjectActionName + " : ";
                    }
                    else
                    {
                        message += pageObjectName + " : ";
                    }
                }

                message += eventName;
                return message;
            }
        }
        public delegate void GolemEvent(GolemEventArgs e);

        public static event GolemEvent GenericEvent;
        public static event GolemEvent AfterProgramLaunchedEvent;
        public static event GolemEvent AFterProgramClosedEvent;
        public static event GolemEvent BeforeTestEvent;
        public static event GolemEvent AfterTestEvent;
        public static event GolemEvent PageObjectCreatedEvent;
        public static event GolemEvent PageObjectFinishedLoadingEvent;
        public static event GolemEvent PageObjectMethodEvent;
        public static event GolemEvent BeforeCommandEvent;
        public static event GolemEvent AfterCommandEvent;
        public static event GolemEvent BeforeSuiteEvent;
        public static event GolemEvent AfterSuiteEvent;
        public static event GolemEvent OnTestPassedEvent;
        public static event GolemEvent OnTestFailedEvent;
        public static event GolemEvent OnTestSkippedEvent;

        public EventFiringTest()
        {
            GenericEvent += new GolemEvent(WriteActionToLog);
            AfterProgramLaunchedEvent += new GolemEvent(WriteActionToLog);
            AFterProgramClosedEvent += new GolemEvent(WriteActionToLog);
            BeforeSuiteEvent += new GolemEvent(WriteActionToLog);
            AfterSuiteEvent += new GolemEvent(WriteActionToLog);
            BeforeTestEvent += new GolemEvent(WriteActionToLog);
            AfterTestEvent += new GolemEvent(WriteActionToLog);
            BeforeCommandEvent += new GolemEvent(WriteActionToLog);
            AfterCommandEvent += new GolemEvent(WriteActionToLog);
            PageObjectCreatedEvent += new GolemEvent(WriteActionToLog);
            PageObjectMethodEvent += new GolemEvent(WriteActionToLog);
            PageObjectFinishedLoadingEvent += new GolemEvent(WriteActionToLog);
            OnTestPassedEvent += new GolemEvent(WriteActionToLog);
            OnTestFailedEvent += new GolemEvent(WriteActionToLog);
            OnTestSkippedEvent += new GolemEvent(WriteActionToLog);
        }

        [SetUp]
        public void SetUp()
        {
            BeforeTestEvent(new GolemEventArgs("Test Starting"));

        }

        [TearDown]
        public void TearDown()
        {
            FireTestStatusEvent();
            AfterTestEvent(new GolemEventArgs("Test Finished"));
            
        }


        [FixtureSetUp]
        public void SuiteSetUp()
        {
           // SetupEvents();
            BeforeSuiteEvent(new GolemEventArgs("Suite Started"));

        }


        [FixtureTearDown]
        public void SuiteTearDown()
        {
            AfterSuiteEvent(new GolemEventArgs("Suite Finished"));
          //  RemoveEvents();

        }

        public void FireTestStatusEvent()
        {

            if (TestContext.CurrentContext.Outcome == TestOutcome.Passed)
            {
                OnTestPassedEvent(new GolemEventArgs("Test Passed"));
            }
            else if (TestContext.CurrentContext.Outcome == TestOutcome.Failed)
            {
                OnTestFailedEvent(new GolemEventArgs("Test Failed"));
            }
            else
            {
                OnTestSkippedEvent(new GolemEventArgs("Test Unknown"));
            }

        }

        protected void WriteActionToLog(GolemEventArgs e)
        {
            if (Config.Settings.reportSettings.commandLogging)
                Common.Log(e.GetEventInfo());
        }

      
        public static void LogEvent(string name)
        {
            GenericEvent(new GolemEventArgs(name));
        }

        public static void FireBeforeCommandEvent(GolemEventArgs e)
        {
            BeforeCommandEvent(e);
        }
        public static void FireAfterCommandEvent(GolemEventArgs e)
        {
            AfterCommandEvent(e);
        }
        public static void FirePageObjectActionEvent(GolemEventArgs e)
        {
            PageObjectMethodEvent(e);
        }
        public static void FirePageObjectCreatedEvent(GolemEventArgs e)
        {
            PageObjectCreatedEvent(e);
        }
        public static void FirePageObjectFinishedLoadingEvent(GolemEventArgs e)
        {
            PageObjectFinishedLoadingEvent(e);
        }
        public static void FireAfterProgramClosedEvent(GolemEventArgs e)
        {
            AFterProgramClosedEvent(e);

        }
        public static void FireAfterProgramLaunchedEvent(GolemEventArgs e)
        {
            AfterProgramLaunchedEvent(e);
        
        }
        
        
        

       
    }
}

