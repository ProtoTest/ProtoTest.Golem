using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Gallio.Common.Markup;
using Gallio.Framework;
using Gallio.Framework.Pattern;
using Gallio.Model;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Proxy;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Core
{
    public abstract class TestBase
    {
        #region Events

#pragma warning disable 67
        public static event ActionEvent BeforeTestEvent;
        public static event ActionEvent AfterTestEvent;
        public static event ActionEvent PageObjectActionEvent;
        public static event ActionEvent BeforeCommandEvent;
        public static event ActionEvent AfterCommandEvent;
        public static event ActionEvent BeforeSuiteEvent;
        public static event ActionEvent AfterSuiteEvent;
        public static event ActionEvent GenericEvent;
#pragma warning restore 67
        private void WriteActionToLog(string name, EventArgs e)
        {
            if (Config.Settings.reportSettings.commandLogging)
                Common.Log("(" + DateTime.Now.ToString("HH:mm:ss::ffff") + ") : " + name);
        }

        private void AddAction(string name, EventArgs e)
        {
            testData.actions.addAction(name);
        }

        public delegate void ActionEvent(string name, EventArgs e);

        public static void LogEvent(string name)
        {
            GenericEvent(name, null);
        }

        public class GolemEventArgs : EventArgs
        {
            public IWebDriver driver;
            public IWebElement element;
            public string name;
            public DateTime time;
        }

        #endregion
        protected static Object locker = new object();
        protected static BrowserMobProxy proxy;

        [SetUp]
        public void SetUp()
        {
            LogEvent(Common.GetCurrentTestName() + " started");
        }

        [TearDown]
        public void TearDown()
        {
            LogEvent(Common.GetCurrentTestName() + " " + Common.GetTestOutcome().DisplayName);
            GetHarFile();
            AssertNoVerificationErrors();
            DeleteTestData();
        }



        [FixtureSetUp]
        public void SuiteSetUp()
        {
            SetupEvents();
            testDataCollection = new Dictionary<string, TestDataContainer>();
            SetTestExecutionSettings();
            StartProxy();
        }


        [FixtureTearDown]
        public void SuiteTearDown()
        {
            QuitProxy();
            RemoveEvents();
        }

        private void DeleteTestData()
        {
            string testName = Common.GetCurrentTestName();
            if (!testDataCollection.ContainsKey(testName))
            {
                testDataCollection.Remove(testName);
            }
        }

        public static IDictionary<string, TestDataContainer> testDataCollection;

        public static TestDataContainer testData
        {
            get
            {
                string name = Common.GetCurrentTestName();
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


        public static void AddVerificationError(string errorText)
        {
            LogEvent("--> VerificationError Found: " + errorText);
            testData.VerificationErrors.Add(new VerificationError(errorText));
        }

        public static void AddVerificationError(string errorText, Image image)
        {
            LogEvent("--> VerificationError Found: " + errorText);
            testData.VerificationErrors.Add(new VerificationError(errorText, image));
        }

        private void AssertNoVerificationErrors()
        {
            if (testData.VerificationErrors.Count == 0)
                return;
            int i = 1;
            TestLog.BeginMarker(Marker.AssertionFailure);
            foreach (VerificationError error in testData.VerificationErrors)
            {
                TestLog.Failures.BeginSection("ElementVerification Error " + i);
                TestLog.Failures.WriteLine(error.errorText);
                if (error.screenshot != null)
                    TestLog.Failures.EmbedImage(null, error.screenshot);
                TestLog.Failures.End();
                i++;
            }
            TestLog.End();
            Assert.TerminateSilently(TestOutcome.Failed);
        }


        public void SetTestExecutionSettings()
        {
            TestAssemblyExecutionParameters.DegreeOfParallelism = Config.Settings.runTimeSettings.DegreeOfParallelism;
            TestAssemblyExecutionParameters.DefaultTestCaseTimeout =
                TimeSpan.FromMinutes(Config.Settings.runTimeSettings.TestTimeoutMin);
        }

        private void SetupEvents()
        {
            PageObjectActionEvent += AddAction;
            BeforeTestEvent += WriteActionToLog;
            AfterTestEvent += WriteActionToLog;
            GenericEvent += WriteActionToLog;
        }

        private void RemoveEvents()
        {
            PageObjectActionEvent -= AddAction;
            BeforeTestEvent -= WriteActionToLog;
            AfterTestEvent -= WriteActionToLog;
            GenericEvent -= WriteActionToLog;
        }


        private void LogHttpTrafficMetrics()
        {
            if (Config.Settings.httpProxy.useProxy)
            {
                //TestBase.proxy.GetSessionMetrics();
                //TestLog.BeginSection("HTTP Metrics");
                //TestLog.WriteLine("Number of Requests : " + TestBase.proxy.numSessions);
                //TestLog.WriteLine("Min Response Time : " + TestBase.proxy.minResponseTime);
                //TestLog.WriteLine("Max Response Time : " + TestBase.proxy.maxResponseTime);
                //TestLog.WriteLine("Avg Response Time : " + TestBase.proxy.avgResponseTime);
                //TestLog.End();
            }
        }

        private void GetHarFile()
        {
            if (Config.Settings.httpProxy.useProxy)
            {
                string name = Common.GetShortTestName(80);
                proxy.SaveHarToFile();
                TestLog.Attach(new BinaryAttachment("HTTP_Traffic_" + name + ".har",
                    "application/json", File.ReadAllBytes(TestBase.proxy.GetHarFilePath())));
                LogHttpTrafficMetrics();
                proxy.CreateHar();
            }
        }

        private void StartProxy()
        {
            try
            {
                if (Config.Settings.httpProxy.startProxy)
                {
                    Common.KillProcess("java");
                    proxy = new BrowserMobProxy();
                    proxy.StartServer(Config.Settings.httpProxy.proxyServerPort);
                    proxy.CreateProxy(Config.Settings.httpProxy.proxyPort);
                    proxy.CreateHar();

                }
            }
            catch (Exception e)
            {
            }
        }

        private void QuitProxy()
        {
            if (Config.Settings.httpProxy.startProxy)
            {
                TestBase.proxy.QuitServer();
            }
        }
        public static string GetCurrentClassName()
        {
            var stackTrace = new StackTrace(); // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames(); // get method calls (frames)

            foreach (StackFrame stackFrame in stackFrames)
            {
                // trace += stackFrame.GetMethod().ReflectedType.FullName;
                if (stackFrame.GetMethod().ReflectedType.FullName.Contains("PageObject"))
                {
                    string name = stackFrame.GetMethod().ReflectedType.Name;
                    return name;
                }
            }
            // DiagnosticLog.WriteLine(stackTrace.ToString());
            return "";
        }

        public static string GetCurrentMethodName()
        {
            var stackTrace = new StackTrace(); // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames(); // get method calls (frames)

            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                if (stackFrame.GetMethod().ReflectedType.BaseType == typeof(BasePageObject))
                    return stackFrame.GetMethod().Name;
            }
            return "";
        }

        public static string GetCurrentClassAndMethodName()
        {
            var stackTrace = new StackTrace(); // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames(); // get method calls (frames)
            // string trace = "";
            foreach (StackFrame stackFrame in stackFrames)
            {
                // trace += stackFrame.GetMethod().ReflectedType.FullName;
                if ((stackFrame.GetMethod().ReflectedType.FullName.Contains("PageObject")) &&
                    (!stackFrame.GetMethod().IsConstructor))
                {
                    string name = stackFrame.GetMethod().ReflectedType.Name + "." + stackFrame.GetMethod().Name;
                    return name;
                }
            }
            // DiagnosticLog.WriteLine(stackTrace.ToString());
            return "";
        }


    }
}