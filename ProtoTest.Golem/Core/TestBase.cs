using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Gallio.Common.Markup;
using Gallio.Common.Media;
using Gallio.Framework;
using NUnit.Framework;
using ProtoTest.Golem.Proxy;
using ProtoTest.Golem.WebDriver;
using TestContext = NUnit.Framework.TestContext;
using TestStatus = NUnit.Framework.Interfaces.TestStatus;

namespace ProtoTest.Golem.Core
{
    public abstract class TestBase
    {
        public static CaptionOverlay overlay = new CaptionOverlay
        {
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom
        };

        protected static Object locker = new object();
        public static BrowserMobProxy proxy;
        private static IDictionary<string, TestDataContainer> _testDataCollection;

        public static IDictionary<string, TestDataContainer> testDataCollection
        {
            get { return _testDataCollection ?? (_testDataCollection = new Dictionary<string, TestDataContainer>()); }
            set { _testDataCollection = value; }
        }

        public static TestDataContainer testData
        {
            get
            {
                var name = Common.GetCurrentTestName();
                if (!testDataCollection.ContainsKey(name))
                {
                    lock (locker)
                    {
                        var container = new TestDataContainer(name);
                        testDataCollection.Add(name, container);
                        return container;
                    }
                }
                return testDataCollection[name];
            }
        }

        [SetUp]
        public virtual void SetUpTestBase()
        {
            Log.Message(Common.GetCurrentTestName() + " started");
            StartNewProxy();
            StartVideoRecording();
        }

        [TearDown]
        public virtual void TearDownTestBase()
        {
            Log.Message(Common.GetCurrentTestName() + " " + Common.GetTestOutcome());
            VerifyHttpTraffic();
            GetHarFile();
            QuitProxy();           
            LogVideoIfTestFailed();
            AssertNoVerificationErrors();
            DeleteTestData();
        }

        private void VerifyHttpTraffic()
        {
            if (Config.Settings.httpProxy.useProxy && Config.Settings.httpProxy.validateTraffic)
            {
                proxy.VerifyNoErrorsCodes();
            }
        }

        [TestFixtureSetUp]
        public virtual void SuiteSetUp()
        {
            CreateReportDirectory();
            SetTestExecutionSettings();
            StartProxyServer();
        }

        private void CreateReportDirectory()
        {
            string filePath = Path.GetFullPath(Config.Settings.reportSettings.reportPath);
            Directory.CreateDirectory(filePath);
            bool exists = Directory.Exists(filePath);
        }

        [TestFixtureTearDown]
        public virtual void SuiteTearDown()
        {
            StopVideoRecording();
            QuitProxyServer();
            Config.Settings = new ConfigSettings();
        }

        private void DeleteTestData()
        {
            var testName = Common.GetCurrentTestName();
            if (!testDataCollection.ContainsKey(testName))
            {
                testDataCollection.Remove(testName);
            }
        }
        public static void LogVerificationPassed(string successText)
        {
            Log.Message("--> VerificationError Passed: " + successText);
//            TestContext.CurrentContext.IncrementAssertCount();
        }

        public static void AddVerificationError(string errorText)
        {
            Log.Message("--> VerificationError Found: " + errorText);
            testData.VerificationErrors.Add(new VerificationError(errorText,
                Config.Settings.reportSettings.screenshotOnError));
//            TestContext.CurrentContext.IncrementAssertCount();
        }

        public static void AddVerificationError(string errorText, Image image)
        {
            Log.Message("--> VerificationError Found: " + errorText);
            testData.VerificationErrors.Add(new VerificationError(errorText, image));
//            TestContext.CurrentContext.IncrementAssertCount();
        }

        private void AssertNoVerificationErrors()
        {
            if (testData.VerificationErrors.Count == 0)
            {
                return;
            }

            var i = 1;
//            TestLog.BeginMarker(Marker.AssertionFailure);
            foreach (var error in testData.VerificationErrors)
            {
//                TestLog.Failures.BeginSection("ElementVerification Error " + i);
                Common.Log(error.errorText);
                if (Config.Settings.reportSettings.screenshotOnError && (error.screenshot != null))
                {
                    Log.Image(error.screenshot);
                }
                i++;
            }
            Assert.Fail("The test failed due to verification errors");
        }

        public void SetTestExecutionSettings()
        {
//            TestAssemblyExecutionParameters.DegreeOfParallelism = Config.Settings.runTimeSettings.DegreeOfParallelism;
//            TestAssemblyExecutionParameters.DefaultTestCaseTimeout =
//                TimeSpan.FromMinutes(Config.Settings.runTimeSettings.TestTimeoutMin);
        }

        //commented out Proxy stuff because browserMobProxy is not implimented
        private void GetHarFile()
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    var name = Common.GetShortTestName(80);
                    proxy.SaveHarToFile();
                    Log.FilePath(proxy.GetHarFilePath());
                    proxy.CreateHar();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error caught getting BMP Har File : " + e.Message);
            }
        }

        private void StartNewProxy()
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    proxy.CreateProxy();
                    proxy.CreateHar();
                }
            }
            catch (Exception e)
            {
                Log.Failure("Failed to setup proxy: " + e.Message + ": Trying again...");
                proxy.CreateProxy();
                proxy.CreateHar();
            }
        }

        private void StartProxyServer()
        {
            try
            {
                proxy = new BrowserMobProxy();
                if (Config.Settings.httpProxy.startProxy)
                {
                    proxy.KillOldProxy();
                    proxy.StartServer();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error caught starting BMP Proxy Server : " + e.Message);
            }
        }

        private void QuitProxyServer()
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    proxy.QuitServer();
                }
            }
            catch (Exception)
            {
            }
        }

        private void QuitProxy()
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    proxy.QuitProxy();
                }
            }
            catch (Exception)
            {
            }
        }

        public static string GetCurrentClassName()
        {
            var stackTrace = new StackTrace(); // get call stack
            var stackFrames = stackTrace.GetFrames(); // get method calls (frames)

            foreach (var stackFrame in stackFrames)
            {
                if ((stackFrame.GetMethod().ReflectedType.BaseType == typeof (BasePageObject)) &&
                    (!stackFrame.GetMethod().IsConstructor))
                {
                    return stackFrame.GetMethod().ReflectedType.Name;
                }
            }
            foreach (var stackFrame in stackFrames)
            {
                if ((stackFrame.GetMethod().ReflectedType.BaseType == typeof (TestBase)) &&
                    (!stackFrame.GetMethod().IsConstructor))
                {
                    return stackFrame.GetMethod().ReflectedType.Name + "." + stackFrame.GetMethod().Name;
                }
            }

            return "";
        }

        public static string GetCurrentMethodName()
        {
            var stackTrace = new StackTrace(); // get call stack
            var stackFrames = stackTrace.GetFrames(); // get method calls (frames)

            // write call stack method names
            foreach (var stackFrame in stackFrames)
            {
                if ((stackFrame.GetMethod().ReflectedType.IsSubclassOf(typeof (BasePageObject))) &&
                    (!stackFrame.GetMethod().IsConstructor))
                {
                    return stackFrame.GetMethod().Name;
                }
            }
            foreach (var stackFrame in stackFrames)
            {
                if ((stackFrame.GetMethod().ReflectedType.IsSubclassOf(typeof (TestBase)) &&
                     (!stackFrame.GetMethod().IsConstructor)))
                {
                    return stackFrame.GetMethod().ReflectedType.Name + "." + stackFrame.GetMethod().Name;
                }
            }

            return "";
        }

        public static string GetCurrentClassAndMethodName()
        {
            var stackTrace = new StackTrace(); // get call stack
            var stackFrames = stackTrace.GetFrames(); // get method calls (frames)

            foreach (var stackFrame in stackFrames)
            {
                if ((stackFrame.GetMethod().ReflectedType.IsSubclassOf(typeof (BasePageObject)) &&
                     (!stackFrame.GetMethod().IsConstructor)))
                {
                    return stackFrame.GetMethod().ReflectedType.Name + "." + stackFrame.GetMethod().Name;
                }
            }

            foreach (var stackFrame in stackFrames)
            {
                var type = stackFrame.GetMethod().ReflectedType;
                if (type.IsSubclassOf(typeof (TestBase)) && (!stackFrame.GetMethod().IsConstructor))
                {
                    return stackFrame.GetMethod().ReflectedType.Name + "." + stackFrame.GetMethod().Name;
                }
            }

            return "";
        }

        public void LogVideoIfTestFailed()
        {
            if ((Config.Settings.reportSettings.videoRecordingOnError) &&
                (Common.GetTestOutcome() != TestStatus.Passed) && testData.recorder != null && testData.recorder.Video != null)
            {
                Log.Video(testData.recorder.Video);
            }
        }

        public void StartVideoRecording()
        {
            if (Config.Settings.reportSettings.videoRecordingOnError)
            {
                testData.recorder = Capture.StartRecording(new CaptureParameters {Zoom = .25}, 5);
                testData.recorder.OverlayManager.AddOverlay(overlay);
            }
        }

        public void StopVideoRecording()
        {
            try
            {
                if (Config.Settings.reportSettings.videoRecordingOnError && Config.Settings.runTimeSettings.DegreeOfParallelism == 1 && Config.Settings.runTimeSettings.RunOnRemoteHost == false)
                {
                    testData.recorder.Stop();
                }
            }
            catch (Exception e)
            {
                Log.Failure(e.Message);
            }
        }
    }
}