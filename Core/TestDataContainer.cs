using System;
using System.Collections.Generic;
using System.Linq;
using Gallio.Common.Media;
using NUnit.Framework;
using OpenQA.Selenium;
using Golem.WebDriver;

namespace Golem.Core
{
    public class TestDataContainer
    {
        private readonly object eventLocker = new object();
        public ActionList actions;
        public BrowserInfo browserInfo;
        public ConfigSettings configSettings;
        private bool eventsRegistered;
        public ScreenRecorder recorder;
        public string testName;
        public List<VerificationError> VerificationErrors;
        public string ExceptionMessage = "";
        public string StackTrace = "";
        public TestContext.ResultAdapter Result;
        public string Status = "";
        public string ReportPath = "";
        public string ScreenshotPath = "";
        public string VideoPath = "";
        public string ClassName = "";
        public string MethodName = null;

        public TestDataContainer(string name)
        {
            testName = name;
            actions = new ActionList();
            VerificationErrors = new List<VerificationError>();
            configSettings = Config.GetDefaultConfig();

            var count = configSettings.runTimeSettings.Browsers.Count();
            if (count > 0)
            {
                browserInfo = configSettings.runTimeSettings.Browsers.First();
            }
            else
            {
                browserInfo = new BrowserInfo(WebDriverBrowser.Browser.Chrome);
            }

            SetupEvents();
            
        }

        public void GetCurrentResult()
        {
            Result = TestContext.CurrentContext.Result;
        }

        public IWebDriver driver { get; set; }

        public void LogEvent(string name, ActionList.Action.ActionType type=ActionList.Action.ActionType.Other)
        {
            TestBase.testData.actions.addAction(name, type);
        }

        private void WriteActionToLog(string name, EventArgs e)
        {
            TestBase.overlay.Text = name;
//            if (Config.settings.Report.diagnosticLog)
//                Log.Message(string.Format("({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), name));
            AddAction(name, e);
        }

        private void AddAction(string name, EventArgs e)
        {
            TestBase.testData.actions.addAction(name);
        }

        private void SetupEvents()
        {
            lock (eventLocker)
            {
                PageObjectActionEvent += AddAction;
                BeforeTestEvent += WriteActionToLog;
                AfterTestEvent += WriteActionToLog;
                GenericEvent += WriteActionToLog;
                eventsRegistered = true;
            }
        }

        private void RemoveEvents()
        {
            lock (eventLocker)
            {
                if (eventsRegistered)
                {
                    PageObjectActionEvent -= AddAction;
                    BeforeTestEvent -= WriteActionToLog;
                    AfterTestEvent -= WriteActionToLog;
                    GenericEvent -= WriteActionToLog;
                    eventsRegistered = false;
                }
            }
        }

        private delegate void ActionEvent(string name, EventArgs e);
#pragma warning disable 67
        private static event ActionEvent BeforeTestEvent;
        private static event ActionEvent AfterTestEvent;
        private static event ActionEvent PageObjectActionEvent;
        private static event ActionEvent BeforeCommandEvent;
        private static event ActionEvent AfterCommandEvent;
        private static event ActionEvent BeforeSuiteEvent;
        private static event ActionEvent AfterSuiteEvent;
        private static event ActionEvent GenericEvent;
#pragma warning restore 67
    }
}