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
    public class TestBaseClass : EventFiringTest
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
                //testData.recorder.Dispose();
            }
                
                
        }

        public void AfterTestEvent_QuitBrowser(GolemEventArgs e)
        {

            if (Config.Settings.runTimeSettings.LaunchBrowser)
            {
                    driver.Quit();
                   FireAfterProgramClosedEvent(new GolemEventArgs(browser.ToString() + " Browser Closed"));
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

                    FireAfterProgramLaunchedEvent(new GolemEventArgs(browser + " Browser Launched"));
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

        private void RemoveEvents()
        {
            GenericEvent -= new GolemEvent(WriteActionToLog);
            AfterProgramLaunchedEvent -= new GolemEvent(WriteActionToLog);
            AFterProgramClosedEvent -= new GolemEvent(WriteActionToLog);
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
       
        [TearDown]
        public void TearDown()
        {
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
    }
}

