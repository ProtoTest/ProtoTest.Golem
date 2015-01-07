using System;
using System.Collections.Generic;
using System.Drawing;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Appium
{
    public class AppiumTestBase : TestBase
    {
        public ProcessRunner server = new ProcessRunner(Config.Settings.appiumSettings.appiumServerPath + "node" + " " +
                                                     Config.Settings.appiumSettings.appiumServerPath +
                                                     "node_modules\\appium\\bin\\appium.js");

        [Factory("GetHosts")]
        public string host;

        public static IWebDriver driver
        {
            get { return testData.driver; }
            set { testData.driver = value; }
        }


        public static IEnumerable<WebDriverHost> GetHosts()
        {
            return Config.Settings.runTimeSettings.Hosts;
        }

        public static T OpenPage<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        public void LogScreenshotIfTestFailed()
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
            if (Config.Settings.appiumSettings.launchApp && driver!=null)
            {
                driver.Quit();
                driver = null;
            }
        }

        [NUnit.Framework.TestFixtureSetUp]
        [FixtureSetUp]
        public void SetupFixture()
        {            
            server.StartProcess();
        }

        [NUnit.Framework.TestFixtureTearDown]
        [FixtureTearDown]
        public void TeardowmFixture()
        {
            server.StopProcess();
        }

        [NUnit.Framework.SetUp]
        [SetUp]
        public void SetUp()
        {
            LaunchApp();
        }

        [NUnit.Framework.TearDown]
        [TearDown]
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
                    capabilities.SetCapability("app",Config.Settings.appiumSettings.bundleId);
                        
                }
                else
                {
                    capabilities.SetCapability("platformName", "iOS");
                    capabilities.SetCapability(CapabilityType.BrowserName, "iOS");
                    capabilities.SetCapability(CapabilityType.Platform, "Mac");
                    capabilities.SetCapability("app", Config.Settings.appiumSettings.appPath);    
                }
            }
            IWebDriver tempDriver = driver;
            try
            {
                tempDriver  = new ScreenshotRemoteWebDriver(new Uri(string.Format("http://{0}:{1}/wd/hub", host, Config.Settings.appiumSettings.appiumPort)), capabilities);
            }
            catch (Exception)
            {
                Common.Log("Did not get a driver.  Trying again");
                server.StopProcess();
                server.StartProcess();
                tempDriver  = new ScreenshotRemoteWebDriver(new Uri(string.Format("http://{0}:{1}/wd/hub", host, Config.Settings.appiumSettings.appiumPort)), capabilities);
            }
                
            driver = new EventedWebDriver(tempDriver).driver;  
        }
    }
}
