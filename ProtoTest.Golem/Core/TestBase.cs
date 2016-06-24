using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Web.UI;
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
            testData.ClassName = TestContext.CurrentContext.Test.ClassName;
            testData.MethodName = TestContext.CurrentContext.Test.MethodName;
            Config.settings = new ConfigSettings();
            Config.settings.reportSettings.reportPath = reportPath;
            StartNewProxy();
            StartVideoRecording();
        }

        [TearDown]
        public virtual void TearDownTestBase()
        {
            try
            {
                LogTestOutcome();
                VerifyHttpTraffic();
                GetHarFile();
                QuitProxy();
                LogVideo();
                AssertNoVerificationErrors();
            }
            catch (Exception e)
            {
                CreateHtmlReport();
                DeleteTestData();
                throw e;
            }
            
        }

        private void LogTestOutcome()
        {
            Log.Message(Common.GetCurrentTestName() + " " + Common.GetTestOutcome());
            
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                Core.Log.Error(TestContext.CurrentContext.Result.Message);
                Core.Log.Error(TestContext.CurrentContext.Result.StackTrace);
                testData.ExceptionMessage = TestContext.CurrentContext.Result.Message;
                
            }
            testData.Status = TestContext.CurrentContext.Result.Outcome.Status.ToString();
            testData.Result = TestContext.CurrentContext.Result;
        }

        private void VerifyHttpTraffic()
        {
            if (Config.settings.httpProxy.useProxy && Config.settings.httpProxy.validateTraffic)
            {
                proxy.VerifyNoErrorsCodes();
            }
        }

        [OneTimeSetUp]
        public virtual void SuiteSetUp()
        {
            var context = TestContext.CurrentContext;
            CreateReportDirectory();
            SetTestExecutionSettings();
            StartProxyServer();
        }

        private string reportPath;
        private void CreateReportDirectory()
        {
            string filePath = Path.GetFullPath(Config.settings.reportSettings.reportPath);
            reportPath = Path.Combine(filePath, DateTime.Now.ToString("yyMMdd_HHMM"));
            Config.settings.reportSettings.reportPath = reportPath;
            Directory.CreateDirectory(reportPath);
        }
        
        
        [OneTimeTearDown]
        public virtual void SuiteTearDown()
        {
            StopVideoRecording();
            QuitProxyServer();
            CreateHtmlIndex();
            Config.settings = new ConfigSettings();
        }

        private void CreateHtmlIndex()
        {
            HtmlReportGenerator gen = new HtmlReportGenerator();
            gen.GenerateStartTags();
            gen.GenerateIndexHead();
            foreach (var item in testDataCollection)
            {
                if (item.Value.MethodName != null)
                {
                    gen.GenerateIndexRow(item.Key, item.Value.ReportPath, item.Value.Status);
                }
               
            }
            gen.GenerateLogEnd();
            gen.GenerateEndTags();
            gen.WriteToIndexFile();
        }

        private void CreateHtmlReport()
        {
    
            HtmlReportGenerator gen = new HtmlReportGenerator();
            gen.GenerateStartTags();
            gen.GenerateLogHeader();
            gen.GenerateLogStatus(testData.Result.Outcome.Status.ToString(),testData.Result.Message,testData.Result.StackTrace, testData.ScreenshotPath,testData.VideoPath);
            foreach (var item in testData.actions.actions)
            {
                switch (item.type)
                {
                    case ActionList.Action.ActionType.Link:
                        gen.GenerateLogLink(item.time.ToLongTimeString(), item.name);
                        break;
                    case ActionList.Action.ActionType.Video:
                        gen.GenerateLogVideo(item.time.ToLongTimeString(), item.name);
                        break;
                    case ActionList.Action.ActionType.Image:
                        gen.GenerateLogImage(item.time.ToLongTimeString(), item.name);
                        break;
                    case ActionList.Action.ActionType.Error:
                        gen.GenerateLogError(item.time.ToLongTimeString(), item.name);
                        break;
                    case ActionList.Action.ActionType.Warning:
                        gen.GenerateLogWarning(item.time.ToLongTimeString(), item.name);
                        break;
                    case ActionList.Action.ActionType.Message:
                        gen.GenerateLogMessage(item.time.ToLongTimeString(), item.name);
                        break;

                    default:
                        gen.GenerateLogRow(item.time.ToLongTimeString(), item.name);
                        break;
                }
               
            }
            gen.GenerateLogEnd();
            gen.GenerateEndTags();
            gen.WriteToFile();

           
         
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
            Log.Error("--> VerificationError Found: " + errorText);
            var error = new VerificationError(errorText,
                Config.settings.reportSettings.screenshotOnError);
            testData.VerificationErrors.Add(error);
            if (error.screenshot != null)
            {
               Log.Image(error.screenshot);
            }
           
//            TestContext.CurrentContext.IncrementAssertCount();
        }

        public static void AddVerificationError(string errorText, Image image)
        {
            Log.Error("--> VerificationError Found: " + errorText, image);
            testData.VerificationErrors.Add(new VerificationError(errorText, image));
//            TestContext.CurrentContext.IncrementAssertCount();
        }

        private void AssertNoVerificationErrors()
        {
            if (testData.VerificationErrors.Count >= 1)
            {
                Assert.Fail("The test failed due to verification errors");
            }
        }

        public void SetTestExecutionSettings()
        {
//            TestAssemblyExecutionParameters.DegreeOfParallelism = Config.settings.runTimeSettings.DegreeOfParallelism;
//            TestAssemblyExecutionParameters.DefaultTestCaseTimeout =
//                TimeSpan.FromMinutes(Config.settings.runTimeSettings.TestTimeoutMin);
        }

        private void GetHarFile()
        {
            try
            {
                if (Config.settings.httpProxy.startProxy)
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

        internal void StartNewProxy()
        {
            try
            {
                if (Config.settings.httpProxy.startProxy)
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

        internal void StartProxyServer()
        {
            try
            {
                proxy = new BrowserMobProxy();
                if (Config.settings.httpProxy.startProxy)
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
                if (Config.settings.httpProxy.startProxy)
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
                if (Config.settings.httpProxy.startProxy)
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
                if ((stackFrame.GetMethod().ReflectedType.BaseType == typeof (BasePageObject) || stackFrame.GetMethod().ReflectedType.BaseType == typeof(BaseComponent)) &&
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
                if ((stackFrame.GetMethod().ReflectedType.IsSubclassOf(typeof (BasePageObject)) || stackFrame.GetMethod().ReflectedType.IsSubclassOf(typeof(BaseComponent))) &&
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
                var type = stackFrame.GetMethod().ReflectedType;
                if (((type.IsSubclassOf(typeof (BasePageObject)) || type.IsSubclassOf(typeof(BaseComponent))) &&
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

        public void LogVideo()
        {
            if ((Config.settings.reportSettings.videoRecordingOnError) &&
                testData.recorder != null && testData.recorder.Video != null)
            {
                var path = Log.Video(testData.recorder.Video);
                TestBase.testData.VideoPath = path;
            }
        }

        public void StartVideoRecording()
        {
            if (Config.settings.reportSettings.videoRecordingOnError)
            {
                testData.recorder = Capture.StartRecording(new CaptureParameters {Zoom = .25}, 5);
                testData.recorder.OverlayManager.AddOverlay(overlay);
            }
        }

        public void StopVideoRecording()
        {
            try
            {
                if (Config.settings.reportSettings.videoRecordingOnError && Config.settings.runTimeSettings.DegreeOfParallelism == 1 && Config.settings.runTimeSettings.RunOnRemoteHost == false)
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