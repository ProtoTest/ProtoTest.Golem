using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using Gallio.Framework;
using Gallio.Model;
using Gallio.Common.Media;
using Golem.Framework;

namespace Golem.Framework
{
    public class TestBaseClass
    {
        public static IDictionary<string, TestDataContainer> testDataCollection;

        public static TestDataContainer testData
        {
            get
            {
                if(!testDataCollection.ContainsKey(TestContext.CurrentContext.Test.FullName))
                {
                   testDataCollection.Add(TestContext.CurrentContext.Test.FullName,new TestDataContainer());
                    return testDataCollection[TestContext.CurrentContext.Test.FullName];
                }
                    return testDataCollection[TestContext.CurrentContext.Test.FullName];
            }
        }

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

        public void AddVerificationError(string errorText)
        {
            testData.VerificationErrors.Add(new VerificationError(errorText));
        }

        private void AssertNoVerificationErrors()
        {
            if ((TestContext.CurrentContext.Outcome == Gallio.Model.TestOutcome.Passed) || (testData.VerificationErrors.Count == 0))
                return;
            int i = 1;
            TestLog.BeginMarker(Gallio.Common.Markup.Marker.AssertionFailure);
            foreach (VerificationError error in testData.VerificationErrors)
            {
                TestLog.Failures.BeginSection("Verification Error " + i);
                TestLog.Failures.WriteLine(error.errorText);
                TestLog.Failures.EmbedImage(null,error.screenshot);
                TestLog.Failures.End();
                TestContext.CurrentContext.IncrementAssertCount();
                i++;
            }
            
            TestLog.End();
            Assert.TerminateSilently(Gallio.Model.TestOutcome.Failed);
        }

        private void LogInfoIfTestFailed()
        {
            if (TestContext.CurrentContext.Outcome != Gallio.Model.TestOutcome.Passed)
            {
                TestLog.Failures.EmbedImage(null, testData.driver.GetScreenshot());
                TestLog.Failures.EmbedVideo(null, testData.recorder.Video);
            }

        }

        private string GetConfigValue(string key, string defaultValue="")
        {

            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
                return defaultValue;
            else
                return setting;
        }

        private void LoadConfigFile()
        {
            Config.RuntimeSettings.browser = WebDriverBrowser.getBrowserFromString(GetConfigValue("Browser", "Firefox"));
        }

        [SetUp]
        public void SetUp()
        {
            

            testData.recorder = Capture.StartRecording(new Gallio.Common.Media.CaptureParameters(){Zoom=.25},5);
            //testData.beforeTestEvent(TestContext.CurrentContext.Test.FullName, null);
            //
            driver = new WebDriverBrowser().LaunchBrowser();
            //FireEvent("Browser Launched", null);

        }

        [TearDown]
        public void TearDown()
        {
            //testData.afterTestEvent(TestContext.CurrentContext.Test.FullName, null);
            testData.recorder.Stop();
           // FireEvent("Browser Closed");
            LogInfoIfTestFailed();
            driver.Quit();
            testData.actions.PrintActions();
            AssertNoVerificationErrors();
            
        }

        [FixtureInitializer]
        public void Initializer()
        {
            testDataCollection = new Dictionary<string, TestDataContainer>();
            LoadConfigFile();
            
        }

        [FixtureSetUp]
        public void SuiteSetUp()
        {
            //FireEvent("Suite Started");
        }

        [FixtureTearDown]
        public void SuiteTearDown()
        {
           // FireEvent("Suite Finished");
        }

    }
}
