using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Gallio.Framework;
using Ionic.Zip;
using Newtonsoft.Json;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Proxy.HAR;
using RestSharp;

namespace ProtoTest.Golem.Proxy
{
    /// <summary>
    /// This class acts as a wrapper around a BrowserMobProxy jar.  
    /// It contains methods to launch and stop the server, as well as to issue REST commands to start/stop/configure an individual proxy.  
    /// </summary>
    public class BrowserMobProxy
    {
        private static readonly string zipPath = Directory.GetCurrentDirectory() +
                                                 @"\Proxy\browsermob-proxy-2.0-beta-8-bin.zip";

        private static readonly string batchPath = @"C:\BMP\browsermob-proxy-2.0-beta-8\bin\browsermob-proxy";

        private static readonly string extractPath = @"C:\BMP";
        public static string Indent = "    ";

        private readonly IRestClient client;
        private readonly IDictionary<string, int> proxyPortsByTest;
        private readonly IRestRequest request;
        private readonly int serverPort;
        private IRestResponse response;
        private Process serverProcess;

        public BrowserMobProxy()
        {
            serverPort = Config.Settings.httpProxy.proxyServerPort;
            proxyPortsByTest = new Dictionary<string, int>();
            client = new RestClient();
            request = new RestRequest();
            response = new RestResponse();
            UnzipProxy();
        }

        public int proxyPort
        {
            get
            {
                if (!proxyPortsByTest.ContainsKey(TestContext.CurrentContext.TestStep.FullName))
                {
                    proxyPortsByTest.Add(TestContext.CurrentContext.TestStep.FullName,
                        Config.Settings.httpProxy.proxyPort);
                    Config.Settings.httpProxy.proxyPort++;
                }

                return proxyPortsByTest[TestContext.CurrentContext.TestStep.FullName];
            }
            set
            {
                if (!proxyPortsByTest.ContainsKey(TestContext.CurrentContext.TestStep.FullName))
                {
                    proxyPortsByTest.Add(TestContext.CurrentContext.TestStep.FullName,
                        Config.Settings.httpProxy.proxyPort);
                    Config.Settings.httpProxy.proxyPort++;
                }
                proxyPortsByTest[TestContext.CurrentContext.TestStep.FullName] = value;
            }
        }


        public void StartServer()
        {
            try
            {
                Common.Log("Starting BrowserMob server on port " + serverPort);
                serverProcess = new Process();
                var StartInfo = new ProcessStartInfo();
                StartInfo.FileName = batchPath;
                StartInfo.Arguments = "-port " + serverPort;
                StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                StartInfo.CreateNoWindow = false;
                serverProcess.StartInfo = StartInfo;
                serverProcess.Start();
                client.BaseUrl = "http://localhost:" + serverPort;
                WaitForServerToStart();
            }
            catch (Exception e)
            {
                Common.Log(e.Message);
            }
        }

        public void KillOldProxy()
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                try
                {
                    if ((process.ProcessName == "java") && (process.StartInfo.CreateNoWindow == false))
                    {
                        Common.Log("Killing old BMP Proxy");
                        process.Kill();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void QuitServer()
        {
            try
            {
                Common.Log("Stopping BrowserMobProxy Server");
                serverProcess.CloseMainWindow();
                serverProcess.Kill();
            }
            catch (Exception) { }
        }

        public bool WaitForServerToStart(int timeout = 30)
        {
            for (int i = 0; i < timeout; i++)
            {
                request.Method = Method.GET;
                request.Resource = "/";
                response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            throw new Exception("Could not start the BrowserMobProxy " + response.StatusCode);
        }

        public void UnzipProxy()
        {
            if (File.Exists(batchPath) == false)
            {
                Common.Log("BrowserMobProxy not found, unzipping");
                using (ZipFile zf = ZipFile.Read(zipPath))
                {
                    zf.ExtractAll(extractPath);
                }
            }
        }

        public void QuitProxy()
        {
            Common.Log("Quitting Proxy on Port " + proxyPort);
            request.Method = Method.DELETE;
            request.Resource = "/proxy/" + proxyPort;
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                TestLog.Warnings.WriteLine("Could not quit proxy on port : " + proxyPort);
            }
        }

        public void CreateProxy()
        {
            Common.Log("Creating Proxy on Port " + proxyPort);

            request.Method = Method.POST;
            request.Resource = "/proxy?port=" + proxyPort;
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Could not start Proxy at port : " + proxyPort + " : " + response.StatusCode);
                }
            }
        }

        public void CreateHar()
        {
            Common.Log("Creating a new Har");
            request.Method = Method.PUT;
            request.Resource = string.Format("/proxy/{0}/har", proxyPort);
            response = client.Execute(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Could not create Har on port " + proxyPort + ": " + response.StatusCode);
            }
        }

        public string GetPrettyHar()
        {
            return JsonConvert.SerializeObject(GetHarString(), Formatting.Indented);
        }

        public void CreatePage(string name)
        {
            Common.Log("Creating a new Proxy Page with name " + name);
            request.Method = Method.PUT;
            request.Resource = string.Format("/proxy/{0}/har/pageRef", proxyPort);
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not create Page : " + response.StatusCode);
            }
        }

        public void DeleteProxy()
        {
            Common.Log("Deleting proxy on port " + proxyPort);
            request.Method = Method.DELETE;
            request.Resource = string.Format("/proxy/{0}", proxyPort);
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not delete proxy at port : " + proxyPort + " : " + response.StatusCode);
            }
        }

        public string GetHarString()
        {
            request.Method = Method.GET;
            request.Resource = string.Format("/proxy/{0}/har ", proxyPort);
            response = client.Execute(request);
            return response.Content;
        }

        public HarResult GetHar()
        {
            try
            {
                request.RequestFormat = DataFormat.Json;
                request.Method = Method.GET;
                request.Resource = string.Format("/proxy/{0}/har ", proxyPort);
                IRestResponse<HarResult> responseHar = client.Execute<HarResult>(request);
                if (responseHar.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Could not get har for proxy at port " + proxyPort + " : " +
                                        responseHar.StatusCode);
                }
                return JsonConvert.DeserializeObject<HarResult>(responseHar.Content);
            }
            catch (Exception)
            {
                return new HarResult();
            }
        }

        public List<Entry> FilterEntries(string url)
        {
            try
            {
                return GetHar().Log.Entries.Where(entry => entry.Request.Url.Contains(url)).ToList();
            }
            catch (Exception)
            {
                return new List<Entry>();
            }
        }

        public bool IsQueryStringInEntry(QueryStringItem querysString, Entry entry)
        {
            return
                entry.Request.QueryString.Any(
                    qs => (qs.Name.Contains(querysString.Name)) && (qs.Value.Contains(querysString.Value)));
        }

        public string GetValueForQueryStringWithName(string name, Entry entry)
        {
            try
            {
                return entry.Request.QueryString.First(qs => qs.Name.Contains(name)).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Entry GetLastEntryForUrl(string url)
        {
            return GetHar().Log.Entries.Last(entry => entry.Request.Url.Contains(url));
        }

        public void VerifyQueryStringInEntry(QueryStringItem queryString, Entry entry)
        {
            if (IsQueryStringInEntry(queryString, entry))
            {
                Common.Log(string.Format("!--Verification Passed. Request contains {0}={1}", queryString.Name,
                    queryString.Value));
                return;
            }

            string message;
            string value = GetValueForQueryStringWithName(queryString.Name, entry);
            if (value != null)
            {
                message = string.Format("Expected {0}={1}. Actual {2}={3}", queryString.Name, queryString.Value,
                    queryString.Name, value);
            }
            else
            {
                message = string.Format("No QueryString found with Description={0}", queryString.Name);
            }
            TestBase.AddVerificationError(string.Format("Request From {0} to {1} not correct. {2}",
                TestBase.testData.driver.Url, entry.Request.Url, message));
        }

        public void VerifyRequestMade(string url)
        {
            List<Entry> entries = FilterEntries(url);
            if (entries.Count == 0)
            {
                TestBase.AddVerificationError("Did not find request with url " + url);
            }
        }

        public void VerifyRequestQueryString(string url, QueryStringItem queryString)
        {
            if (IsQueryStringInEntry(queryString, GetLastEntryForUrl(url)))
            {
                Common.Log(string.Format("!--Verification Passed. Request contains {0}={1}", queryString.Name,
                    queryString.Value));
                return;
            }

            string message;
            string value = GetValueForQueryStringWithName(queryString.Name, GetLastEntryForUrl(url));
            if (value != null)
            {
                message = string.Format("Expected {0}={1}. Actual {2}={3}", queryString.Name, queryString.Value,
                    queryString.Name, value);
            }
            else
            {
                message = string.Format("No QueryString found with Description={0}", queryString.Name);
            }
            TestBase.AddVerificationError(string.Format("Request From {0} to {1} not correct. {2}",
                TestBase.testData.driver.Url, url, message));
        }

        public void Whitelist(string values, int statusCode = 408)
        {
            request.Method = Method.PUT;
            request.Resource = string.Format("/proxy/{0}/whitelist", proxyPort);
            request.AddParameter("regex", values);
            request.AddParameter("status", statusCode);
            response = client.Execute(request);
        }

        public void Blacklist(string values, int statusCode = 407)
        {
            request.Method = Method.PUT;
            request.Resource = string.Format("/proxy/{0}/blacklist", proxyPort);
            request.AddParameter("regex", values);
            request.AddParameter("status", statusCode);
            response = client.Execute(request);
        }

        public void ClearWhitelist()
        {
            request.Method = Method.DELETE;
            request.Resource = string.Format("/proxy/{0}/whitelist", proxyPort);
            response = client.Execute(request);
        }

        public void ClearBlacklist()
        {
            request.Method = Method.DELETE;
            request.Resource = string.Format("/proxy/{0}/blacklist", proxyPort);
            response = client.Execute(request);
        }

        public void SetBandwidthLimit(string kbps)
        {
            request.Method = Method.PUT;
            request.Resource = string.Format("/proxy/{0}/limit", proxyPort);
            request.AddParameter("downstreamKbps", kbps);
            request.AddParameter("upstreamKbps", kbps);
            request.AddParameter("enable", "true");
            response = client.Execute(request);
        }

        public void SaveHarToFile()
        {
            using (var outfile = new StreamWriter(GetHarFilePath()))
            {
                outfile.Write(GetPrettyHar());
            }
        }


        public string GetHarFilePath()
        {
            return Directory.GetCurrentDirectory()
                          + Path.DirectorySeparatorChar
                          + "HTTP_Traffic_" + Common.GetShortTestName(80) + ".har";
        }
    }
}