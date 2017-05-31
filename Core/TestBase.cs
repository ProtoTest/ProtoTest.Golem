using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using Gallio.Common.Media;
using Gallio.Framework;
using NUnit.Framework;
using Golem.Proxy;
using Golem.WebDriver;
using Ionic.Zip;
using TestContext = NUnit.Framework.TestContext;
using TestStatus = NUnit.Framework.Interfaces.TestStatus;

namespace Golem.Core
{
    public abstract class TestBase
    {
        public static CaptionOverlay overlay = new CaptionOverlay
        {
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom
        };
//        public static string reportPath;
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
                var exists = testDataCollection.ContainsKey(name);
                if (!exists)
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
            finally 
            {
                CreateHtmlReport();
                DeleteTestData();
            }
            
        }

        private void LogTestOutcome()
        {
            Log.Message(Common.GetCurrentTestName() + " " + Common.GetTestOutcome());
            
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                Log.Error(TestContext.CurrentContext.Result.Message);
                Log.Error(TestContext.CurrentContext.Result.StackTrace);
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

        private void DeleteOldReports()
        {
            try
            {
                var dirs = Directory.GetDirectories(Config.settings.reportSettings.reportRoot);
                int count = dirs.Length;
                for (int i = Config.settings.reportSettings.numReports; i < count; i++)
                {
                    string dir = dirs[i - Config.settings.reportSettings.numReports];
                    Directory.Delete(dir, true);
                }

            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
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

       
        private void CreateReportDirectory()
        {
            lock (locker)
            {
                
                 DeleteOldReports();
                Directory.CreateDirectory(Config.settings.reportSettings.reportPath);
                var path = $"{Config.settings.reportSettings.reportPath}\\{Common.GetCurrentTestName()}.html";
                TestContext.WriteLine(path);
                Debug.WriteLine(path);
            }
        }
        
        
        [OneTimeTearDown]
        public virtual void SuiteTearDown()
        {
            StopVideoRecording();
            QuitProxyServer();
            CreateHtmlIndex();
            CompressToZipFile();
            Config.settings = new ConfigSettings();
        }

        private void CreateHtmlIndex()
        {
            HtmlReportGenerator gen = new HtmlReportGenerator();
            gen.GenerateStartTags();
            gen.GenerateIndexHead();
            gen.GenerateIndexSummary();
            var results = testDataCollection.OrderBy(x => x.Key).ToList();
            string classname = "";
            bool first = true;
            foreach (var item in results)
            {
                if (item.Value.ClassName != classname)
                {
                    if (first == true)
                    {
                        first = false;
                    }
                    else
                    {
                        gen.GenerateSuiteFooter();
                    }
                    gen.GenerateSuiteHeader(item.Value.ClassName);
                    classname = item.Value.ClassName;
                }
                
                if (item.Value.MethodName != null)
                {
                    gen.GenerateIndexRow(item.Key, item.Value.ReportPath, item.Value.Status, item.Value.ExceptionMessage);
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

        private void CompressToZipFile()
        {
            if (!Config.settings.reportSettings.compressToZip)
            {
                return;
            }

            string path = $"{Config.settings.reportSettings.reportPath}";

            var files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return;
            }

            using (ZipFile zip = new ZipFile())
            {
                foreach (string src in files)
                {
                    zip.AddFile(src, string.Empty);
                }
                zip.Save($"{path}/report.zip");
            }
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
            try
            {
                if (Config.settings.reportSettings.videoRecordingOnError)
                {
                    testData.recorder = Capture.StartRecording(new CaptureParameters { Zoom = .25 }, 5);
                    testData.recorder.OverlayManager.AddOverlay(overlay);
                }
            }
            catch (Exception e)
            {
                Log.Failure("Exception caught while trying to start video recording : " + e.Message);
            }
            
        }

        public void StopVideoRecording()
        {
            try
            {
                if (Config.settings.reportSettings.videoRecordingOnError && Config.settings.runTimeSettings.RunOnRemoteHost == false)
                {
                    testData.recorder.Stop();
                }
            }
            catch (Exception e)
            {
                Log.Failure("Exception caught while trying to stop video recording : " + e.Message);
            }
        }
    }
}