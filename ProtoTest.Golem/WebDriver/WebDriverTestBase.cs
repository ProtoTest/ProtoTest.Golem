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
            //   driver.Manage().Window.Maximize();
            return (T) Activator.CreateInstance(typeof (T));
        }

        private void LogScreenshotIfTestFailed()
        {
            if ((Config.Settings.reportSettings.screenshotOnError) &&
                (TestContext.CurrentContext.Outcome != TestOutcome.Passed))
            {
                Image screenshot = testData.driver.GetScreenshot();
                if (screenshot != null)
                    TestLog.Failures.EmbedImage(null, screenshot);
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

        [SetUp]
        public void SetUp()
        {
            LaunchBrowser();
        }

        [TearDown]
        public void TearDown()
        {
            LogScreenshotIfTestFailed();
            LogHtmlIfTestFailed();
            QuitBrowser();
        }
    }
}