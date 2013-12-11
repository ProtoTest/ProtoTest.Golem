using System;
using System.Collections.Generic;
using System.IO;
using Gallio.Common.Markup;
using Gallio.Common.Media;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Proxy;

namespace ProtoTest.Golem.WebDriver
{
    public class WebDriverTestBase : TestBase
    {
        [Factory("GetBrowser")] public WebDriverBrowser.Browser browser;
        [Factory("GetHosts")] public string host;

        public static IWebDriver driver
        {
            get { return testData.driver; }
            set { testData.driver = value; }
        }

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

        public static IEnumerable<string> GetHosts()
        {
                return Config.Settings.runTimeSettings.Hosts;          
        }

        public static T OpenPage<T>(string url)
        {
            driver.Navigate().GoToUrl(url);
         //   driver.Manage().Window.Maximize();
            return (T) Activator.CreateInstance(typeof (T));
        }

        private void LogScreenshotIfTestFailed()
        {
            if ((Config.Settings.reportSettings.screenshotOnError) &&
                (TestContext.CurrentContext.Outcome != TestOutcome.Passed))
            {
                TestLog.Failures.EmbedImage(null, testData.driver.GetScreenshot());
            }
        }

        public void LogHtmlIfTestFailed()
        {
            try
            {
                if ((Config.Settings.reportSettings.htmlOnError) && (Common.GetTestOutcome() != TestOutcome.Passed))
                {
                    TestLog.AttachHtml("HTML_" + Common.GetShortTestName(95), driver.PageSource);
                }
            }
            catch (Exception)
            {
                TestLog.Warnings.WriteLine("Error caught trying to get page source");
            }
           
        }

        public void LogVideoIfTestFailed()
        {
            if ((Config.Settings.reportSettings.videoRecordingOnError) &&
                (Common.GetTestOutcome() != TestOutcome.Passed))
            {
                TestLog.Failures.EmbedVideo("Video_" + Common.GetShortTestName(90), testData.recorder.Video);
                testData.recorder.Dispose();
            }
        }

        public void QuitBrowser()
        {
            if (Config.Settings.runTimeSettings.LaunchBrowser)
            {
                driver.Quit();
                driver = null;
                LogEvent(browser + " Browser Closed");
            }
        }

        public void StartVideoRecording()
        {
            if (Config.Settings.reportSettings.videoRecordingOnError)
                testData.recorder = Capture.StartRecording(new CaptureParameters {Zoom = .25}, 5);
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

        public void LaunchBrowser()
        {
            lock (locker)
            {
                if (Config.Settings.runTimeSettings.LaunchBrowser)
                {
                    if (Config.Settings.runTimeSettings.RunOnRemoteHost)
                    {
                        driver = new WebDriverBrowser().LaunchRemoteBrowser(browser,host);
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
                    var capabilities = new DesiredCapabilities();
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

        [SetUp]
        public void SetUp()
        {
            StartVideoRecording();
            LaunchBrowser();
        }

        [TearDown]
        public void TearDown()
        {
            StopVideoRecording();
            LogScreenshotIfTestFailed();
            LogVideoIfTestFailed();
            LogHtmlIfTestFailed();
            QuitBrowser();
            //GetHTTPTrafficInfo();
        }
    }
}