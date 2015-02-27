using System;
using System.Collections.Generic;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using TestContext = Gallio.Framework.TestContext;

namespace ProtoTest.Golem.Appium
{
    public class AppiumTestBase : TestBase
    {
        [Factory("GetHosts")] public string host;

        public ProcessRunner server = new ProcessRunner(Config.Settings.appiumSettings.appiumServerPath + "node" + " " +
                                                        Config.Settings.appiumSettings.appiumServerPath +
                                                        "node_modules\\appium\\bin\\appium.js");

        public static IWebDriver driver
        {
            get { return testData.driver; }
            set { testData.driver = value; }
        }

        public static IEnumerable<BrowserInfo> GetBrowsers()
        {
            return Config.Settings.runTimeSettings.Browsers;
        }

        public static T OpenPage<T>()
        {
            return (T) Activator.CreateInstance(typeof (T));
        }

        public void LogScreenshotIfTestFailed()
        {
            if ((Config.Settings.reportSettings.screenshotOnError) &&
                (TestContext.CurrentContext.Outcome != TestOutcome.Passed))
            {
                var screenshot = testData.driver.GetScreenshot();
                if (screenshot != null)
                {
                    TestLog.Failures.EmbedImage(null, screenshot);
                }
            }
        }

        public void LogSourceIfTestFailed()
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

        public void QuitAppium()
        {
            if (Config.Settings.appiumSettings.launchApp && driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }

        [TestFixtureSetUp]
        [FixtureSetUp]
        public void SetupFixture()
        {
            server.StartProcess();
        }

        [TestFixtureTearDown]
        [FixtureTearDown]
        public void TeardowmFixture()
        {
            server.StopProcess();
        }

        [NUnit.Framework.SetUp]
        [MbUnit.Framework.SetUp]
        public void SetUp()
        {
            LaunchApp();
        }

        [NUnit.Framework.TearDown]
        [MbUnit.Framework.TearDown]
        public void Teardown()
        {
            LogScreenshotIfTestFailed();
            LogSourceIfTestFailed();
            QuitAppium();
        }

        public void LaunchApp()
        {
            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, "");
            capabilities.SetCapability("deviceName", Config.Settings.appiumSettings.device);
            capabilities.SetCapability("autoLaunch", Config.Settings.appiumSettings.launchApp);
            capabilities.SetCapability("platformVersion", Config.Settings.appiumSettings.platformVersion);


            if (Config.Settings.appiumSettings.device.Contains("droid"))
            {
                capabilities.SetCapability("app", Config.Settings.appiumSettings.appPath);
                capabilities.SetCapability("appPackage", Config.Settings.appiumSettings.package);
                capabilities.SetCapability("appActivity", Config.Settings.appiumSettings.activity);
                capabilities.SetCapability("platformName", "Android");
            }
            else
            {
                if (Config.Settings.appiumSettings.useIpa)
                {
                    capabilities.SetCapability("platformName", "iOS");
                    capabilities.SetCapability("ipa", Config.Settings.appiumSettings.appPath);
                    capabilities.SetCapability("app", Config.Settings.appiumSettings.bundleId);
                }
                else
                {
                    capabilities.SetCapability("platformName", "iOS");
                    capabilities.SetCapability(CapabilityType.BrowserName, "iOS");
                    capabilities.SetCapability(CapabilityType.Platform, "Mac");
                    capabilities.SetCapability("app", Config.Settings.appiumSettings.appPath);
                }
            }
            var tempDriver = driver;
            try
            {
                tempDriver =
                    new ScreenshotRemoteWebDriver(
                        new Uri(string.Format("http://{0}:{1}/wd/hub", host, Config.Settings.appiumSettings.appiumPort)),
                        capabilities);
            }
            catch (Exception)
            {
                Common.Log("Did not get a driver.  Trying again");
                server.StopProcess();
                server.StartProcess();
                tempDriver =
                    new ScreenshotRemoteWebDriver(
                        new Uri(string.Format("http://{0}:{1}/wd/hub", host, Config.Settings.appiumSettings.appiumPort)),
                        capabilities);
            }

            driver = new EventedWebDriver(tempDriver).driver;
        }
    }
}