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
        public static event GolemEvent GenericEvent;
        public static event GolemEvent AfterBrowserLaunchedEvent;
        public static event GolemEvent AfterBrowserClosedEvent;
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


#pragma warning restore 67
        private void WriteActionToLog(string name, EventArgs e)
        {
            if(Config.Settings.reportSettings.commandLogging)
                Common.Log("(" + DateTime.Now.ToString("HH:mm:ss::ffff") + ") : " + name);
        }
        private void WriteActionToLog(GolemEventArgs e)
        {
            if (Config.Settings.reportSettings.commandLogging)
                Common.Log(e.GetEventInfo());
        }
        private void AddAction(string name, EventArgs e)
        {
            testData.actions.addAction(name);
        }

        public delegate void ActionEvent(string name, EventArgs e);
        public delegate void GolemEvent(GolemEventArgs e);

        public static void LogEvent(string name)
        {
            GenericEvent(new GolemEventArgs(name));
        }

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
            public GolemEventArgs(string eventName="")
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

        #endregion

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

        private void AfterTestEvent_AssertNoVerificationErrors(GolemEventArgs e)
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

        private void OnTestFailed_LogScreenshot(GolemEventArgs e)
        {
            if (Config.Settings.reportSettings.screenshotOnError)
            {
                    TestLog.Failures.EmbedImage(null, testData.driver.GetScreenshot());

            }

        }

        public void OnTestFailed_LogHtml(GolemEventArgs e)
        {
            if ((Config.Settings.reportSettings.htmlOnError) && (Common.GetTestOutcome() != TestOutcome.Passed))
            {
                TestLog.AttachHtml("HTML_" + Common.GetShortTestName(95),driver.PageSource);
            }
        }

        public void OnTestFailed_LogVideo(GolemEventArgs e)
        {
            if (Config.Settings.reportSettings.videoRecordingOnError)
            {
                TestLog.Failures.EmbedVideo("Video_" + Common.GetShortTestName(90), testData.recorder.Video);
                testData.recorder.Dispose();
            }
                
                
        }

        public void AfterTestEvent_QuitBrowser(GolemEventArgs e)
        {

            if (Config.Settings.runTimeSettings.LaunchBrowser)
            {
                    driver.Quit();
                    AfterBrowserClosedEvent(new GolemEventArgs(browser.ToString() + " Browser Closed"));
            }
                
        }

        public void FireTestStatusEvent()
        {

            if(TestContext.CurrentContext.Outcome== TestOutcome.Passed)
            {
                    OnTestPassedEvent(new GolemEventArgs("Test Passed"));
            }
            else if(TestContext.CurrentContext.Outcome==  TestOutcome.Failed)
            {
                    OnTestFailedEvent(new GolemEventArgs("Test Failed"));
            }
            else 
            {
            OnTestSkippedEvent(new GolemEventArgs("Test Unknown"));
            }

        }

        public void AfterTestEvent_LogActions(GolemEventArgs e)
        {
            if (Config.Settings.reportSettings.actionLogging)
                testData.actions.PrintActionTimings();
        }

        public void BeforeTestEvent_StartVideoRecording(GolemEventArgs e)
        {
            if (Config.Settings.reportSettings.videoRecordingOnError)
                    testData.recorder = Capture.StartRecording(new CaptureParameters() { Zoom = .25 }, 5);          
        }

        public void AfterTestEvent_StopVideoRecording(GolemEventArgs e)
        {
            try
            {
                if (Config.Settings.reportSettings.videoRecordingOnError)
                    testData.recorder.Stop();
            }
            catch (Exception err)
            {
               TestLog.Failures.WriteLine(err.Message);
            }
            
        }

        private void LogHttpTrafficMetrics()
        {

            if (Config.Settings.httpProxy.startProxy)
            {
                testData.proxy.GetSessionMetrics();
                TestLog.BeginSection("HTTP Metrics");
                TestLog.WriteLine("Number of Requests : " + testData.proxy.numSessions);
                TestLog.WriteLine("Min Response Time : " + testData.proxy.minResponseTime);
                TestLog.WriteLine("Max Response Time : " + testData.proxy.maxResponseTime);
                TestLog.WriteLine("Avg Response Time : " + testData.proxy.avgResponseTime);
                TestLog.End();
            }
        }


        private int GetNewProxyPort()
        {
            Config.Settings.httpProxy.proxyPort++;
            return Config.Settings.httpProxy.proxyPort;
        }

        private void AfterTestEvent_GetHTTPTrafficInfo(GolemEventArgs e)
        {
            if (Config.Settings.httpProxy.startProxy)
            {
                string name = Common.GetShortTestName(80);
                testData.proxy.SaveSessionsToFile();
                TestLog.Attach(new BinaryAttachment("HTTP_Traffic_" + name + ".saz", "application/x-fiddler-session-archive", File.ReadAllBytes(testData.proxy.GetSazFilePath())));

                LogHttpTrafficMetrics();
                
                testData.proxy.ClearSessionList();
            }
        }

        private void BeforeTestEvent_StartProxy(GolemEventArgs e)
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    
                    testData.proxy = new FiddlerProxy();
                    testData.proxy.StartFiddler();
                }
            }
            catch (Exception)
            {
            }
        }

        private void AfterTestEvent_QuitProxy(GolemEventArgs e)
        {
            if (Config.Settings.httpProxy.startProxy)
            {
                testData.proxy.QuitFiddler();
            }
        }



        public void BeforeTestEvent_LaunchBrowser(GolemEventArgs e)
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
                    AfterBrowserLaunchedEvent(new GolemEventArgs(browser + " Browser Launched"));
                    //LogEvent(browser + " Browser Launched");
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
            GenericEvent += new GolemEvent(WriteActionToLog);
            AfterBrowserLaunchedEvent += new GolemEvent(WriteActionToLog);
            AfterBrowserClosedEvent += new GolemEvent(WriteActionToLog);
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

        private void RemoveEvents()
        {
            GenericEvent -= new GolemEvent(WriteActionToLog);
            AfterBrowserLaunchedEvent -= new GolemEvent(WriteActionToLog);
            AfterBrowserClosedEvent -= new GolemEvent(WriteActionToLog);
            BeforeSuiteEvent -= new GolemEvent(WriteActionToLog);
            AfterSuiteEvent -= new GolemEvent(WriteActionToLog);
            BeforeTestEvent -= new GolemEvent(WriteActionToLog);
            AfterTestEvent -= new GolemEvent(WriteActionToLog);
            BeforeCommandEvent -= new GolemEvent(WriteActionToLog);
            AfterCommandEvent -= new GolemEvent(WriteActionToLog);
            PageObjectCreatedEvent -= new GolemEvent(WriteActionToLog);
            PageObjectMethodEvent -= new GolemEvent(WriteActionToLog);
            PageObjectFinishedLoadingEvent -= new GolemEvent(WriteActionToLog);
            OnTestPassedEvent -= new GolemEvent(WriteActionToLog);
            OnTestFailedEvent -= new GolemEvent(WriteActionToLog);
            OnTestSkippedEvent -= new GolemEvent(WriteActionToLog);

        }
       
        [SetUp]
        public void SetUp()
        {
            BeforeTestEvent(new GolemEventArgs("Test Starting"));  
            
        }

        [TearDown]
        public void TearDown()
        {

           // AfterTestEvent(new GolemEventArgs("Test Finished"));
            FireTestStatusEvent();
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

        [FixtureInitializer]
        public void Initialize()
        {

            testDataCollection = new Dictionary<string, TestDataContainer>();
            
            SetTestExecutionSettings();
            BeforeTestEvent += BeforeTestEvent_LaunchBrowser;
            BeforeTestEvent += BeforeTestEvent_StartVideoRecording;
            BeforeTestEvent += BeforeTestEvent_StartProxy;
            AfterTestEvent +=  AfterTestEvent_StopVideoRecording;
            OnTestFailedEvent += OnTestFailed_LogScreenshot;
            OnTestFailedEvent += OnTestFailed_LogVideo;
            OnTestFailedEvent += OnTestFailed_LogHtml;
            AfterTestEvent += AfterTestEvent_QuitBrowser;
            AfterTestEvent += AfterTestEvent_QuitProxy;
            AfterTestEvent += AfterTestEvent_LogActions;
            AfterTestEvent += AfterTestEvent_GetHTTPTrafficInfo;
            AfterTestEvent += AfterTestEvent_AssertNoVerificationErrors;
    
        }


        
        [FixtureSetUp]
        public void SuiteSetUp()
        {
            SetupEvents();
            BeforeSuiteEvent(new GolemEventArgs("Suite Started"));
            
        }


        [FixtureTearDown]
        public void SuiteTearDown()
        {
            AfterSuiteEvent(new GolemEventArgs("Suite Finished"));
            RemoveEvents();
            
        }
    }
}

