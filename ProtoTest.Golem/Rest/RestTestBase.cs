using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Proxy;

namespace ProtoTest.Golem.Rest
{
    /// <summary>
    ///     Test Base class to be inherited by all test fixtures.  Will automatically instantiate an object named Given
    /// </summary>
    public class RestTestBase : TestBase
    {
        protected IDictionary<string, string> Tokens;

        protected Given Given
        {
            get
            {
                WebProxy proxy = null;
                if (Config.settings.httpProxy.startProxy)
                {
                    proxy = new WebProxy("localhost:" + TestBase.proxy.proxyPort);
                }
                return new Given(proxy);
            }
        }

        private void LogHttpTrafficMetrics()
        {
            //if (Config.settings.httpProxy.startProxy)
            //{
            //    TestBase.proxy.GetSessionMetrics();
            //    TestLog.BeginSection("HTTP Metrics");
            //    Log.Message("Number of Requests : " + TestBase.proxy.numSessions);
            //    Log.Message("Min Response Time : " + TestBase.proxy.minResponseTime);
            //    Log.Message("Max Response Time : " + TestBase.proxy.maxResponseTime);
            //    Log.Message("Avg Response Time : " + TestBase.proxy.avgResponseTime);
            //    TestLog.End();
            //}
        }

        private void GetHTTPTrafficInfo()
        {
            //if (Config.settings.httpProxy.startProxy)
            //{
            //    string name = Common.GetShortTestName(80);
            //    TestBase.proxy.SaveSessionsToFile();
            //    TestLog.Attach(new BinaryAttachment("HTTP_Traffic_" + name + ".saz",
            //        "application/x-fiddler-session-archive", File.ReadAllBytes(TestBase.proxy.GetSazFilePath())));

            //    LogHttpTrafficMetrics();

            //    TestBase.proxy.ClearSessionList();
            //}
        }

        private void StartProxy()
        {
            try
            {
                if (Config.settings.httpProxy.startProxy)
                {
                    proxy = new BrowserMobProxy();
                    proxy.StartServer();
                    proxy.CreateProxy();
                    proxy.CreateHar();
                }
            }
            catch (Exception)
            {
            }
        }

        private void QuitProxy()
        {
            if (Config.settings.httpProxy.startProxy)
            {
                proxy.QuitServer();
            }
        }

        [SetUp]
        public void SetUp()
        {
            Tokens = new Dictionary<string, string>();
            StartProxy();
        }

        [TearDown]
        public void TearDown()
        {
            QuitProxy();
            GetHTTPTrafficInfo();
        }
    }
}