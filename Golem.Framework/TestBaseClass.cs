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

        public static void AddVerificationError(string errorText)
        {
            testData.FireEvent("--> VerificationError Found: " + errorText);
            testData.VerificationErrors.Add(new VerificationError(errorText));
        }

        private void AssertNoVerificationErrors()
        {
            if (testData.VerificationErrors.Count == 0)
                return;
            int i = 1;
            TestLog.BeginMarker(Gallio.Common.Markup.Marker.AssertionFailure);
            Assert.Multiple(delegate 
            { 
            foreach (VerificationError error in testData.VerificationErrors)
            {
                TestLog.Failures.BeginSection("Verification Error " + i);
                TestLog.Failures.WriteLine(error.errorText);
                TestLog.Failures.EmbedImage(null,error.screenshot);
                TestLog.Failures.End();
                //Assert.Fail(error.errorText);
                //TestLog.Failures.EmbedImage(null,error.screenshot);
                
                i++;
            }
            });
            TestLog.End();
            Assert.TerminateSilently(Gallio.Model.TestOutcome.Failed);
        }

        private void LogScreenshotIfTestFailed()
        {
            if ((Config.Settings.reportSettings.screenshotOnError)&&(TestContext.CurrentContext.Outcome != Gallio.Model.TestOutcome.Passed))
            {
                    TestLog.Failures.EmbedImage(null, testData.driver.GetScreenshot());

            }

        }

        public void LogHtmlIfTestFailed()
        {
            if ((Config.Settings.reportSettings.htmlOnError) && (Common.GetTestOutcome() != TestOutcome.Passed))
            {
                TestLog.AttachHtml("HTML_" + Common.GetShortTestName(95),driver.PageSource);
            }
        }

        public void LogVideoIfTestFailed()
        {
            if ((Config.Settings.reportSettings.videoRecordingOnError) && (Common.GetTestOutcome() != TestOutcome.Passed))
            {
                TestLog.Failures.EmbedVideo("Video_" + Common.GetShortTestName(90), testData.recorder.Video);
            }
                
                
        }

        public void QuitBrowser()
        {
            if (Config.Settings.runTimeSettings.launchBrowser)
            {
                if (driver.CurrentWindowHandle != null)
                {
                    driver.Quit();
                    testData.FireEvent(Config.Settings.runTimeSettings.browser.ToString() + " Browser Closed");
                }
            }
                
        }

        public void LogActions()
        {
            if (Config.Settings.reportSettings.actionLogging)
                testData.actions.PrintActions();
        }

        public void StartVideoRecording()
        {
            if (Config.Settings.reportSettings.videoRecordingOnError)
                testData.recorder = Capture.StartRecording(new Gallio.Common.Media.CaptureParameters() { Zoom = .25 }, 5);
        }

        public void StopVideoRecording()
        {
            if (Config.Settings.reportSettings.videoRecordingOnError)
                testData.recorder.Stop();
        }


        private static Object locker = new object();
        public void LaunchBrowser()
        {
            lock (locker)
            {
                if (Config.Settings.runTimeSettings.launchBrowser)
                {
                    driver = new WebDriverBrowser().LaunchBrowser();
                    testData.FireEvent(Config.Settings.runTimeSettings.browser.ToString() + " Browser Launched");
                }
            }
        }

        public void SetDegreeOfParallelism()
        {
            Gallio.Framework.Pattern.TestAssemblyExecutionParameters.DegreeOfParallelism = Config.Settings.runTimeSettings.degreeOfParallelism;
        }
       

        [SetUp]
        public void SetUp()
        {
            testData.FireEvent(Common.GetCurrentTestName() + " started");
           // StartVideoRecording();
            LaunchBrowser();
            

        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                testData.FireEvent(Common.GetCurrentTestName() + " " + Common.GetTestOutcome().DisplayName);
                StopVideoRecording();
                LogScreenshotIfTestFailed();
                //LogVideoIfTestFailed();
                LogHtmlIfTestFailed();
                LogActions();
                QuitBrowser();
                AssertNoVerificationErrors();
            }
            catch (Exception)
            {
                QuitBrowser();
            }
                
        }

        [FixtureInitializer]
        public void Initializer()
        {
           
        }

        [FixtureSetUp]
        public void SuiteSetUp()
        {
            testDataCollection = new Dictionary<string, TestDataContainer>();
            //SetDegreeOfParallelism();
            //FireEvent("Suite Started");
        }

        [FixtureTearDown]
        public void SuiteTearDown()
        {
           // FireEvent("Suite Finished");
        }

    }
}
