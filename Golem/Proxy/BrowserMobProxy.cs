using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Gallio.Framework;
using Gallio.Model.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Proxy.HAR;
using Ionic.Zip;
using RestSharp;

namespace ProtoTest.Golem.Proxy
{
    public class BrowserMobProxy
    {
        private static readonly string zipPath = Directory.GetCurrentDirectory() + @"\Proxy\browsermob-proxy-2.0-beta-8-bin.zip";

        private static readonly string batchPath = @"C:\BMP\browsermob-proxy-2.0-beta-8\bin\browsermob-proxy";

        private static readonly string extractPath = @"C:\BMP";

        private int proxyPort;
        private int serverPort;
        private Process serverProcess;

        private IRestClient client;
        private IRestRequest request;
        private IRestResponse response;


        public BrowserMobProxy()
        {
            client = new RestClient();
            request = new RestRequest();
            response = new RestResponse();
            UnzipProxy();
        }


        public void StartServer(int port = 0)
        {
            if (port == 0) port = 8081;
            serverPort = port;
            Common.Log("Starting BrowserMob server on port " + port);
            this.serverProcess = new Process();
            var StartInfo = new ProcessStartInfo();
            StartInfo.FileName = batchPath;
            StartInfo.Arguments = "-port " + this.serverPort;
            StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            StartInfo.CreateNoWindow = false;
            this.serverProcess.StartInfo = StartInfo;
            this.serverProcess.Start();
            client.BaseUrl = "http://localhost:" + serverPort;
            WaitForServerToStart();
        }

        public void KillOldProxy()
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                try
                {
                    if ((process.ProcessName == "java")&&(process.StartInfo.CreateNoWindow==false))
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
            Common.Log("Stopping BrowserMobProxy Server");
            this.serverProcess.CloseMainWindow();
            this.serverProcess.Kill();
            

        }

        public bool WaitForServerToStart(int timeout = 30)
        {
            for (var i = 0; i < timeout; i++)
            {
                request.Method = Method.GET;
                request.Resource = "/";
                response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return true;
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

        public void QuitProxy(int port = 0)
        {
            if (port == 0) port = Config.Settings.httpProxy.proxyPort;
            proxyPort = port;
            Common.Log("Quitting Proxy on Port " + port);
            request.Method = Method.DELETE;
            request.Resource = "/proxy/" + this.proxyPort;
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                TestLog.Warnings.WriteLine("Could not quit proxy on port : " + proxyPort);
            }
        }

        public void CreateProxy(int port = 0)
        {
            if (port == 0) port = Config.Settings.httpProxy.proxyPort;
            proxyPort = port;
            Common.Log("Creating Proxy on Port " + port);
            request.Method = Method.POST;
            request.Resource = "/proxy?port="+this.proxyPort;
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not start Proxy at port : " + port + " : " + response.StatusCode);
            }
        }

        public void CreateHar()
        {
            response = new RestResponse();
            request = new RestRequest();
            Common.Log("Creating a new Har");
            request.Method = Method.PUT;
            request.Resource = string.Format("/proxy/{0}/har", proxyPort);
            response = client.Execute(request);
            if  (response.ResponseStatus!=ResponseStatus.Completed)
            {
                throw new Exception("Could not create Har : " + response.StatusCode);
            }
        }

        public string GetPrettyHar()
        {
            return JsonConvert.SerializeObject(GetHarString(), Formatting.Indented);
        }

        public static string Indent = "    ";

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

        public void DeleteProxy(int port = 0)
        {

            if (port == 0) port = proxyPort;
            Common.Log("Deleting proxy on port " + port);
            request.Method = Method.DELETE;
            request.Resource = string.Format("/proxy/{0}", port);
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not delete proxy at port : " + port + " : " + response.StatusCode);
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
                    throw new Exception("Could not get har for proxy at port " + proxyPort + " : " + responseHar.StatusCode);
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
            return entry.Request.QueryString.Any(qs => (qs.Name.Contains(querysString.Name)) && (qs.Value.Contains(querysString.Value)));
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
                Common.Log(string.Format("!--Verification Passed. Request contains {0}={1}", queryString.Name, queryString.Value));
                return;
            }
            string message;
            string value = GetValueForQueryStringWithName(queryString.Name, entry);
            if (value != null)
            {
                message = string.Format("Expected {0}={1}. Actual {2}={3}", queryString.Name, queryString.Value, queryString.Name, value);
            }
            else
            {
                message = string.Format("No QueryString found with Name={0}", queryString.Name);
            }
            TestBase.AddVerificationError(string.Format("Request From {0} to {1} not correct. {2}",
                TestBase.testData.driver.Url, "om.healthgrades.com", message));
        }

        public void VerifyRequestMade(string url)
        {
            var entries = FilterEntries(url);
            if(entries.Count==0)
            TestBase.AddVerificationError("Did not find request with url " + url);
        }

        public void VerifyRequestQueryString(string url, QueryStringItem queryString)
        {
            if (IsQueryStringInEntry(queryString, GetLastEntryForUrl(url)))
            {
                Common.Log(string.Format("!--Verification Passed. Request contains {0}={1}",queryString.Name,queryString.Value));
                return;
            }
            string message;
            string value = GetValueForQueryStringWithName(queryString.Name, GetLastEntryForUrl(url));
            if (value != null)
            {
                message = string.Format("Expected {0}={1}. Actual {2}={3}",queryString.Name,queryString.Value,queryString.Name,value);
            }
            else
            {
                message = string.Format("No QueryString found with Name={0}",queryString.Name);
            }
            TestBase.AddVerificationError(string.Format("Request From {0} to {1} not correct. {2}",
                TestBase.testData.driver.Url, url, message));
        }

        public void Whitelist(string values, int statusCode=408)
        {
            request.Method = Method.PUT;
            request.Resource = string.Format("/proxy/{0}/whitelist", proxyPort);
            request.AddParameter("regex", values);
            request.AddParameter("status", statusCode);
            response = client.Execute(request);
        }

        public void Blacklist(string values, int statusCode=407)
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
            using (StreamWriter outfile = new StreamWriter(GetHarFilePath()))
            {
                outfile.Write(GetPrettyHar());
            }
        }



        public string GetHarFilePath()
        {
            string name = Common.GetShortTestName(80);
            string path = Directory.GetCurrentDirectory()
                          + Path.DirectorySeparatorChar
                          + "HTTP_Traffic_" + name + ".har";
            return path;
        }

    }
}