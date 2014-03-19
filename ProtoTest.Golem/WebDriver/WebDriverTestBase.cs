using System;
using System.Collections.Generic;
using System.Drawing;
using Gallio.Common.Media;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// This class should be inherited by all webdriver tests.  It will automatically launch a browser and include the Driver object in each test.  I
    /// </summary>
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



        public void LaunchBrowser()
        {
            if (Config.Settings.runTimeSettings.LaunchBrowser)
            {
                if (Config.Settings.runTimeSettings.RunOnRemoteHost)
                {
                    driver = new WebDriverBrowser().LaunchRemoteBrowser(browser, host);
                }
                else
                {
                    driver = new WebDriverBrowser().LaunchBrowser(browser);
                }

                LogEvent(browser + " Browser Launched");
                testData.actions.addAction(Common.GetCurrentTestName() + " : " + browser + " Browser Launched");
            }
        }

        [SetUp]
        public void SetUp()
        {
            LaunchBrowser();
        }
        [TearDown]
        public override void TearDownTestBase()
        {
            LogScreenshotIfTestFailed();
            LogHtmlIfTestFailed();
            QuitBrowser();
            base.TearDownTestBase();
        }
    }
}