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

namespace Golem.Framework
{
    public class TestBaseClass
    {
        public static readonly Object padlock = new Object();
        public static IDictionary<string, TestDataContainer> testDataCollection;
        public static TestDataContainer testData
        {
            get
            {
                
                    string name = Common.GetCurrentTestName();
                    if (!testDataCollection.ContainsKey(name))
                    {
                        lock (padlock)
                        {
                            var container = new TestDataContainer(name);
                            testDataCollection.Add(name, container);
                            return container;
                        }
                    }
                    return testDataCollection[name];
            }
        }
        private static Object locker = new object();
        public static FiddlerProxy proxy; 

        public static IWebDriver driver
        {
            get
            {
                return testData.driver;
            }
            set
            {
                testData.driver = value;
            }
        }

        #region Events
        public static event ActionEvent BeforeTestEvent;
        public static event ActionEvent AfterTestEvent;
        public static event ActionEvent PageObjectActionEvent;
        public static event ActionEvent BeforeCommandEvent;
        public static event ActionEvent AfterCommandEvent;
        public static event ActionEvent BeforeSuiteEvent;
        public static event ActionEvent AfterSuiteEvent;
        public static event ActionEvent GenericEvent;

        private void WriteActionToLog(string name, EventArgs e)
        {
            Common.Log("(" + DateTime.Now.ToString("HH:mm:ss::ffff") + ") : " + name);
        }
        private void LogAction(string name, EventArgs e)
        {
            testData.actions.addAction(name);
        }

        public delegate void ActionEvent(string name, EventArgs e);

        public static void LogEvent(string name)
        {
            GenericEvent(name, null);
        }

        public class GolemEventArgs : EventArgs
        {
            public string name;
            public DateTime time;
            public IWebDriver driver;
            public IWebElement element;
        }

        #endregion


        public static void AddVerificationError(string errorText)
        {
            LogEvent("--> VerificationError Found: " + errorText);
            testData.VerificationErrors.Add(new VerificationError(errorText));
        }

        private void AssertNoVerificationErrors()
        {
            if (testData.VerificationErrors.Count == 0)
                return;
            int i = 1;
            TestLog.BeginMarker(Marker.AssertionFailure);
            foreach (VerificationError error in testData.VerificationErrors)
            {
                TestLog.Failures.BeginSection("Verification Error " + i);
                TestLog.Failures.WriteLine(error.errorText);
                TestLog.Failures.EmbedImage(null,error.screenshot);
                TestLog.Failures.End();
                i++;
            }
            TestLog.End();
            Assert.TerminateSilently(TestOutcome.Failed);
        }

        private void LogScreenshotIfTestFailed()
        {
            if ((Config.Settings.reportSettings.screenshotOnError)&&(TestContext.CurrentContext.Outcome != TestOutcome.Passed))
            {
                    TestLog.Failures.EmbedImage(null, testData.driver.GetScreenshot());

            }

        }

        public void LogHtmlIfTestFailed()
        {
            if ((Config.Settings.reportSettings.htmlOnError) && (Common.GetTestOutcome() != TestOutcome.Passed))
            {
                TestLog.AttachHtml("HTML_" + Common.GetShortTestName(95),driver.PageSource);
            }
        }

        public void LogVideoIfTestFailed()
        {
            if ((Config.Settings.reportSettings.videoRecordingOnError) && (Common.GetTestOutcome() != TestOutcome.Passed))
            {
                TestLog.Failures.EmbedVideo("Video_" + Common.GetShortTestName(90), testData.recorder.Video);
            }
                
                
        }

        public void QuitBrowser()
        {
            if (Config.Settings.runTimeSettings.launchBrowser)
            {
                if (driver.CurrentWindowHandle != null)
                {
                    driver.Quit();
                    LogEvent(Common.GetCurrentTestName() + " : " + Config.Settings.runTimeSettings.browser.ToString() + " Browser Closed");
                    testData.actions.addAction(Common.GetCurrentTestName() + " : " + Config.Settings.runTimeSettings.browser.ToString() + " Browser Closed");
                }
            }
                
        }

        public void LogActions()
        {
            if (Config.Settings.reportSettings.actionLogging)
                testData.actions.PrintActionTimings();
        }

        public void StartVideoRecording()
        {
            if (Config.Settings.reportSettings.videoRecordingOnError)
                    testData.recorder = Capture.StartRecording(new CaptureParameters() { Zoom = .25 }, 5);          
        }

        public void StopVideoRecording()
        {
            try
            {
                if (Config.Settings.reportSettings.videoRecordingOnError)
                    testData.recorder.Stop();
            }
            catch (Exception e)
            {
               TestLog.Failures.WriteLine(e.Message);
            }
            
        }

        private void StartProxy()
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    proxy = new FiddlerProxy();
                    proxy.StartFiddler();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void GetHttpTraffic()
        {
            if (Config.Settings.httpProxy.startProxy)
            {
                string name = Common.GetShortTestName(80);
                proxy.SaveSessionsToFile();
                TestLog.Attach(new BinaryAttachment("HTTP_Traffic_" + name + ".saz", "application/x-fiddler-session-archive", File.ReadAllBytes(proxy.GetSazFilePath())));
                proxy.ClearSessionList();
            }
        }

        private void QuitProxy()
        {
            if (Config.Settings.httpProxy.startProxy)
            {
                proxy.QuitFiddler();
            }
        }

        public void LaunchBrowser()
        {
            lock (locker)
            {
                if (Config.Settings.runTimeSettings.launchBrowser)
                {
                    driver = new WebDriverBrowser().LaunchBrowser();
                    LogEvent(Config.Settings.runTimeSettings.browser.ToString() + " Browser Launched");
                    testData.actions.addAction(Common.GetCurrentTestName() + " : " + Config.Settings.runTimeSettings.browser.ToString() + " Browser Launched");
                }
            }
        }

        public void SetDegreeOfParallelism()
        {
            TestAssemblyExecutionParameters.DegreeOfParallelism = Config.Settings.runTimeSettings.degreeOfParallelism;
        }
       
        [SetUp]
        public void SetUp()
        {
            LogEvent(Common.GetCurrentTestName() + " started");
            StartVideoRecording();
            LaunchBrowser();
        }

        [TearDown]
        public void TearDown()
        {
                LogEvent(Common.GetCurrentTestName() + " " + Common.GetTestOutcome().DisplayName);
                StopVideoRecording();
                LogScreenshotIfTestFailed();
                LogVideoIfTestFailed();
                LogHtmlIfTestFailed();
                QuitBrowser();
                LogActions();
                AssertNoVerificationErrors();
                GetHttpTraffic();
        }

        [FixtureSetUp]
        public void SuiteSetUp()
        {
            SetupEvents();
            testDataCollection = new Dictionary<string, TestDataContainer>();
            SetDegreeOfParallelism();
            StartProxy();
            //LogEvent("Suite Started");
        }

        private void SetupEvents()
        {
            PageObjectActionEvent += new TestBaseClass.ActionEvent(LogAction);
            BeforeTestEvent += new TestBaseClass.ActionEvent(WriteActionToLog);
            AfterTestEvent += new TestBaseClass.ActionEvent(WriteActionToLog);
            GenericEvent += new TestBaseClass.ActionEvent(WriteActionToLog);
        }

        [FixtureTearDown]
        public void SuiteTearDown()
        {
            QuitProxy();
            // LogEvent("Suite Finished");
        }
    }
}
