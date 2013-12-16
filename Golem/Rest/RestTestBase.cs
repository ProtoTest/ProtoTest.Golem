using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Gallio.Common.Markup;
using Gallio.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Proxy;
using MbUnit.Framework;

namespace ProtoTest.Golem.Rest
{
    public class RestTestBase : TestBase
    {
        protected IDictionary<string, string> Tokens;

        protected Given Given
        {
            get
            {
                WebProxy proxy = null;
                if (Config.Settings.httpProxy.startProxy)
                {
                    proxy = new WebProxy("localhost:" + TestBase.proxy.proxyPort);
                }
                return new Given(proxy);
            }
        }

        private void LogHttpTrafficMetrics()
        {
            //if (Config.Settings.httpProxy.startProxy)
            //{
            //    TestBase.proxy.GetSessionMetrics();
            //    TestLog.BeginSection("HTTP Metrics");
            //    TestLog.WriteLine("Number of Requests : " + TestBase.proxy.numSessions);
            //    TestLog.WriteLine("Min Response Time : " + TestBase.proxy.minResponseTime);
            //    TestLog.WriteLine("Max Response Time : " + TestBase.proxy.maxResponseTime);
            //    TestLog.WriteLine("Avg Response Time : " + TestBase.proxy.avgResponseTime);
            //    TestLog.End();
            //}
        }

        private void GetHTTPTrafficInfo()
        {
            //if (Config.Settings.httpProxy.startProxy)
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
                if (Config.Settings.httpProxy.startProxy)
                {
                    TestBase.proxy = new BrowserMobProxy();
                    TestBase.proxy.StartServer();
                    TestBase.proxy.CreateProxy();
                    TestBase.proxy.CreateHar();
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