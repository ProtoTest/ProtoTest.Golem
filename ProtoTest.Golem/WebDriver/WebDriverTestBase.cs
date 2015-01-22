using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Rest;
using RestSharp;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// This class should be inherited by all webdriver tests.  It will automatically launch a browser and include the Driver object in each test.  
    /// </summary>
    public class WebDriverTestBase : TestBase
    {
        [Factory("GetBrowsers")] protected BrowserInfo browserInfo;
        protected static IEnumerable<BrowserInfo> GetBrowsers()
        {
            return Config.Settings.runTimeSettings.Browsers;
        }

        public static IWebDriver driver
        {
            get { return testData.driver; }
            set { testData.driver = value; }
        }

        public WebDriverBrowser.Browser browser
        {
            get { return browserInfo.browser; }
            set { browserInfo.browser = value; }
        }

        public string version
        {
            get { return browserInfo.version; }
            set { browserInfo.version = value; }
        }

        public string platform    
        {
            get { return browserInfo.platform; }
            set { browserInfo.platform = value; }
        }



        public static T OpenPage<T>(string url)
        {
            driver.Navigate().GoToUrl(url);
            //navigate twice due to IE bug
            if (Config.Settings.runTimeSettings.Browser == WebDriverBrowser.Browser.IE)
                driver.Navigate().GoToUrl(url);
            return (T) Activator.CreateInstance(typeof (T));
        }

        /// <summary>
        /// Take a screenshot and embed it within the TestLog.
        /// </summary>
        /// <param name="message">Associate a message with the screenshot (optional)</param>
        public static void LogScreenShot(string message=null)
        {
            Image screenshot = testData.driver.GetScreenshot();
            if (screenshot != null)
            {
                if (message != null) TestLog.Default.WriteLine("!------- " + message + " --------!");

                TestLog.Default.EmbedImage(null, screenshot);
            }
        }

        private void LogScreenshotIfTestFailed()
        {
            if ((Config.Settings.reportSettings.screenshotOnError) &&
                (TestContext.CurrentContext.Outcome != TestOutcome.Passed))
            {
                Image screenshot = testData.driver.GetScreenshot();
                if (screenshot != null)
                {
                    TestLog.Failures.EmbedImage(null, screenshot);
                }
            }
        }

        public void LogHtmlIfTestFailed()
        {
            try
            {
                if ((Config.Settings.reportSettings.htmlOnError) && (Common.GetTestOutcome() != TestOutcome.Passed))
                {
                    var source = driver.PageSource;
                    TestLog.AttachHtml("HTML_" + Common.GetShortTestName(95),source );
                }
            }
            catch (Exception e)
            {
                TestLog.Warnings.WriteLine("Error caught trying to get page source: " + e.Message);
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

        protected static Object browserLocker = new object();

        public void LaunchBrowser()
        {
            TestLog.WriteLine("Browser is : " + browser);
            lock (browserLocker)
            {
                if (Config.Settings.runTimeSettings.LaunchBrowser)
                {
                    if (Config.Settings.runTimeSettings.RunOnRemoteHost)
                    {
                        driver = new WebDriverBrowser().LaunchRemoteBrowser(browser, Config.Settings.runTimeSettings.HostIp);
                    }
                    else
                    {
                        driver = new WebDriverBrowser().LaunchBrowser(browser);
                    }

                    LogEvent(browser + " Browser Launched");
                    testData.actions.addAction(Common.GetCurrentTestName() + " : " + browser + " Browser Launched");
                }
            }
           
        }

        [NUnit.Framework.SetUp]
        [SetUp]
        public void SetUp()
        {
            testData.browserInfo = browserInfo;
            LaunchBrowser();
        }

        [NUnit.Framework.TearDown]
        [TearDown]
        public override void TearDownTestBase()
        {
            UpdateSAuceLabsWithTestStatus();
            LogScreenshotIfTestFailed();
            LogHtmlIfTestFailed();
            QuitBrowser();
            base.TearDownTestBase();
        }

        private void UpdateSAuceLabsWithTestStatus()
        {
            if (Config.Settings.sauceLabsSettings.UseSauceLabs)
            {
                bool passed = TestContext.CurrentContext.Outcome.Status == TestStatus.Passed;
                driver.ExecuteJavaScript("sauce:job-result=" + (passed ? "passed" : "failed"));

               
            }
        }
    }
}