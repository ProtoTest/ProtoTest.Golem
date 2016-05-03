﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using TestContext = NUnit.Framework.TestContext;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    ///     This class should be inherited by all webdriver tests.  It will automatically launch a browser and include the
    ///     Driver object in each test.
    /// </summary>
    [TestFixture]
    public class WebDriverTestBase : TestBase
    {

        protected static Object browserLocker = new object();

        protected BrowserInfo browserInfo = new BrowserInfo(Config.Settings.runTimeSettings.Browser);

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

        protected static IEnumerable<BrowserInfo> GetBrowsers()
        {

            return Config.Settings.runTimeSettings.Browsers;
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
        ///     Take a screenshot and embed it within the TestLog.
        /// </summary>
        /// <param name="message">Associate a message with the screenshot (optional)</param>
        public static void LogScreenShot(string message = null)
        {
            var screenshot = testData.driver.GetScreenshot();
            if (screenshot != null)
            {
                if (message != null) Log.Message("!------- " + message + " --------!");

                Log.Image(screenshot);
            }
        }

        private void LogScreenshotIfTestFailed()
        {
            if ((Config.Settings.reportSettings.screenshotOnError) &&
                (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed))
            {
                var screenshot = testData.driver.GetScreenshot();
                if (screenshot != null)
                {
                    Log.Image(screenshot);
                }
            }
        }

        public void LogHtmlIfTestFailed()
        {
            try
            {
                if ((Config.Settings.reportSettings.htmlOnError) && (Common.GetTestOutcome() != TestStatus.Passed))
                {
                    var source = driver.PageSource;
                    Log.Html("HTML_" + Common.GetShortTestName(95), source);
                }
            }
            catch (Exception e)
            {
                Log.Warning("Error caught trying to get page source: " + e.Message);
            }
        }

        public void QuitBrowser()
        {
            if (Config.Settings.runTimeSettings.LaunchBrowser)
            {
                if (driver != null)
                {
                    driver.Quit();
                    driver = null;
                    Log.Message(browser + " Browser Closed");
                }
            }
        }

        public void LaunchBrowser()
        {
            Log.Message("Browser is : " + browser);
            lock (browserLocker)
            {
                if (Config.Settings.runTimeSettings.LaunchBrowser)
                {
                    if (Config.Settings.runTimeSettings.RunOnRemoteHost)
                    {
                        driver = new WebDriverBrowser().LaunchRemoteBrowser(browser,
                            Config.Settings.runTimeSettings.HostIp);
                    }
                    else
                    {
                        driver = new WebDriverBrowser().LaunchBrowser(browser);
                    }

                    Log.Message(browser + " Browser Launched");
                    testData.actions.addAction(Common.GetCurrentTestName() + " : " + browser + " Browser Launched");
                }
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            testData.browserInfo = browserInfo;
            LaunchBrowser();
        }

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
                var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
                driver.ExecuteJavaScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            }
        }
    }
}