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
#pragma warning disable 67
        public static event ActionEvent  BeforeTestEvent;

        public static event ActionEvent AfterTestEvent;
        public static event ActionEvent PageObjectActionEvent;
        public static event ActionEvent BeforeCommandEvent;
        public static event ActionEvent AfterCommandEvent;
        public static event ActionEvent BeforeSuiteEvent;
        public static event ActionEvent AfterSuiteEvent;
        public static event ActionEvent GenericEvent;
#pragma warning restore 67
        private void WriteActionToLog(string name, EventArgs e)
        {
            Common.Log("(" + DateTime.Now.ToString("HH:mm:ss::ffff") + ") : " + name);
        }
        private void AddAction(string name, EventArgs e)
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

        [Factory("GetBrowser")]
        public WebDriverBrowser.Browser browser;

        public static IEnumerable<WebDriverBrowser.Browser> GetBrowser
        {
            get
            {
                foreach (WebDriverBrowser.Browser browser in Config.Settings.runTimeSettings.Browsers)
                {
                    yield return browser;
                }
            }
        }

        public static T OpenPage<T>(string url)
        {
            driver.Navigate().GoToUrl(url);
            return (T)Activator.CreateInstance(typeof(T));
        }

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
                testData.recorder.Dispose();
            }
                
                
        }

        public void QuitBrowser()
        {
            if (Config.Settings.runTimeSettings.LaunchBrowser)
            {
                if (driver.CurrentWindowHandle != null)
                {
                    driver.Quit();
                    LogEvent(browser.ToString() + " Browser Closed");
                    //testData.actions.addAction(browser.ToString() + " Browser Closed");
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


        private int GetNewProxyPort()
        {
            Config.Settings.httpProxy.proxyPort++;
            return Config.Settings.httpProxy.proxyPort;
        }

        private void GetHttpTraffic()
        {
            if (Config.Settings.httpProxy.startProxy)
            {
                string name = Common.GetShortTestName(80);
                testData.proxy.SaveSessionsToFile();
                TestLog.Attach(new BinaryAttachment("HTTP_Traffic_" + name + ".saz", "application/x-fiddler-session-archive", File.ReadAllBytes(testData.proxy.GetSazFilePath())));
                testData.proxy.ClearSessionList();
            }
        }

        private void StartProxy()
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    
                    testData.proxy = new FiddlerProxy(Config.Settings.httpProxy.proxyPort, true);
                    testData.proxy.StartFiddler();
                }
            }
            catch (Exception e)
            {

            }
        }

        private void QuitProxy()
        {
            if (Config.Settings.httpProxy.startProxy)
            {
                testData.proxy.QuitFiddler();
            }
        }

        public void LaunchBrowser()
        {
            lock (locker)
            {
                if (Config.Settings.runTimeSettings.LaunchBrowser)
                {
                    if (Config.Settings.runTimeSettings.RunOnRemoteHost)
                    {
                        driver = new WebDriverBrowser().LaunchRemoteBrowser(browser,Config.Settings.runTimeSettings.HostIp);
                    }
                    else
                    {
                        driver = new WebDriverBrowser().LaunchBrowser(browser);
                    }
                    
                    LogEvent(browser + " Browser Launched");
                    testData.actions.addAction(Common.GetCurrentTestName() + " : " + browser + " Browser Launched");
                }
                if (Config.Settings.appiumSettings.launchApp)
                {
                    DesiredCapabilities capabilities = new DesiredCapabilities();
                    capabilities.SetCapability(CapabilityType.BrowserName, "");
                    capabilities.SetCapability("device", Config.Settings.appiumSettings.appOs);
                    capabilities.SetCapability("app", Config.Settings.appiumSettings.appPath);
                    capabilities.SetCapability("app-package", Config.Settings.appiumSettings.package);
                    capabilities.SetCapability("app-activity", Config.Settings.appiumSettings.activity);

                    var tempDriver = new RemoteWebDriver(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities);
                    driver = new EventedWebDriver(tempDriver).driver;
                }
            }
        }

        public void SetTestExecutionSettings()
        {
            TestAssemblyExecutionParameters.DegreeOfParallelism = Config.Settings.runTimeSettings.DegreeOfParallelism;
            TestAssemblyExecutionParameters.DefaultTestCaseTimeout =
                TimeSpan.FromMinutes(Config.Settings.runTimeSettings.TestTimeoutMin);
        }

        private void SetupEvents()
        {
            PageObjectActionEvent += new TestBaseClass.ActionEvent(AddAction);
            BeforeTestEvent += new TestBaseClass.ActionEvent(WriteActionToLog);
            AfterTestEvent += new TestBaseClass.ActionEvent(WriteActionToLog);
            GenericEvent += new TestBaseClass.ActionEvent(WriteActionToLog);
        }

        private void RemoveEvents()
        {
            PageObjectActionEvent -= new TestBaseClass.ActionEvent(AddAction);
            BeforeTestEvent -= new TestBaseClass.ActionEvent(WriteActionToLog);
            AfterTestEvent -= new TestBaseClass.ActionEvent(WriteActionToLog);
            GenericEvent -= new TestBaseClass.ActionEvent(WriteActionToLog);
        }
       
        [SetUp]
        public void SetUp()
        {
            BeforeTestEvent("Before Test", null);
            LogEvent(Common.GetCurrentTestName() + " started");
            StartVideoRecording();
            StartProxy();
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
            QuitProxy();
            LogActions();
            AssertNoVerificationErrors();
            GetHttpTraffic();
            DeleteTestData();


        }

        private void DeleteTestData()
        {
            string testName = Common.GetCurrentTestName();
            if (!testDataCollection.ContainsKey(testName))
            {
                testDataCollection.Remove(testName);
            }
        }

        [FixtureSetUp]
        public void SuiteSetUp()
        {
            
            SetupEvents();
            testDataCollection = new Dictionary<string, TestDataContainer>();
            SetTestExecutionSettings();
            //LogEvent("Suite Started");
        }


        [FixtureTearDown]
        public void SuiteTearDown()
        {
            RemoveEvents();
            // LogEvent("Suite Finished");
        }
    }
}
