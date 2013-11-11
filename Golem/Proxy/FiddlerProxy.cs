/*
* This demo program shows how to use the FiddlerCore library.
* By default, it is compiled without support for the SAZ File format.
* If you want to add SAZ support, define the token SAZ_SUPPORT in the list of
* Conditional Compilation symbols on the project's BUILD tab.
* 
* You will need to add either SAZ-DOTNETZIP.cs or SAZXCEEDZIP.cs to your project,
* depending on which ZIP library you want to use. You must also ensure to set the 
* Fiddler.RequiredVersionAttribute on your assembly, or your transcoders will be 
* ignored.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Fiddler;
using Gallio.Framework;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.Proxy
{
    public class FiddlerProxy
    {
        private static bool bUpdateTitle = true;
        private static Fiddler.Proxy oSecureEndpoint;
        private static string sSecureEndpointHostname = "localhost";
        private readonly int iSecureEndpointPort;
        private readonly List<Session> oAllSessions;
        private readonly int proxyPort;
        public TimeSpan avgResponseTime = TimeSpan.FromSeconds(0);
        public TimeSpan currentSum = TimeSpan.FromSeconds(0);
        private bool decryptSSL;
        public TimeSpan fiveToNinetyFire = TimeSpan.FromSeconds(0);
        public TimeSpan maxResponseTime = TimeSpan.FromSeconds(0);
        public TimeSpan minResponseTime = TimeSpan.FromSeconds(30);
        public int numSessions = 0;
        private bool systemProxy;

        public FiddlerProxy()
        {
            oAllSessions = new List<Session>();
            AttachEventListeners();
            iSecureEndpointPort = Config.Settings.httpProxy.proxyPort + 1;
            proxyPort = Config.Settings.httpProxy.proxyPort;
        }

        public static void WriteCommandResponse(string s)
        {
            // Common.Log(s);
        }

        public void QuitFiddler()
        {
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
            while (FiddlerApplication.IsStarted())
            {
                TestBase.LogEvent("Stopping Fiddler Proxy on port " + proxyPort);
                FiddlerApplication.Shutdown();
                Common.Delay(500);
            }
        }

        private string Ellipsize(string s, int iLen)
        {
            if (s.Length <= iLen) return s;
            return s.Substring(0, iLen - 3) + "...";
        }

        public void GetSessionMetrics()
        {
            Monitor.Enter(oAllSessions);
            numSessions = oAllSessions.Count;
            foreach (Session oS in oAllSessions)
            {
                TimeSpan duration = oS.Timers.ClientDoneResponse.Subtract(oS.Timers.ClientConnected);
                currentSum += duration;
                if (duration < minResponseTime)
                    minResponseTime = duration;
                if (duration > maxResponseTime)
                    maxResponseTime = duration;
            }
            avgResponseTime = TimeSpan.FromMilliseconds(currentSum.TotalMilliseconds/numSessions);
            Monitor.Exit(oAllSessions);
        }

        private void AttachEventListeners()
        {
            FiddlerApplication.OnNotification +=
                delegate(object sender, NotificationEventArgs oNEA)
                {
                    WriteCommandResponse("** NotifyUser: " + oNEA.NotifyString);
                };
            FiddlerApplication.Log.OnLogString +=
                delegate(object sender, LogEventArgs oLEA) { WriteCommandResponse("** LogString: " + oLEA.LogString); };

            FiddlerApplication.BeforeRequest += delegate(Session oS)
            {
                oS.bBufferResponse = true;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);
                oS["X-AutoAuth"] = "(default)";

                if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) &&
                    (oS.hostname == sSecureEndpointHostname))
                {
                    oS.bBufferResponse = true;
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.HTTPResponseStatus = "200 Ok";
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" +
                                           iSecureEndpointPort +
                                           " received. Your request was:<br /><plaintext>" +
                                           oS.oRequest.headers);
                }
            };
        }

        public void ReadSessions()
        {
            TranscoderTuple oImporter = FiddlerApplication.oTranscoders.GetImporter("SAZ");
            if (null != oImporter)
            {
                var dictOptions = new Dictionary<string, object>();
                dictOptions.Add("Filename",
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ToLoad.saz");

                Session[] oLoaded = FiddlerApplication.DoImport("SAZ", false, dictOptions, null);

                if ((oLoaded != null) && (oLoaded.Length > 0))
                {
                    oAllSessions.AddRange(oLoaded);
                    WriteCommandResponse("Loaded: " + oLoaded.Length + " sessions.");
                }
            }
        }

        public string GetSazFilePath()
        {
            string name = Common.GetShortTestName(80);
            string path = Directory.GetCurrentDirectory()
                          + Path.DirectorySeparatorChar
                          + "HTTP_Traffic_" + name + ".saz";
            return path;
        }

        public void SaveSessionsToFile()
        {
            bool bSuccess = false;
            string sFilename = GetSazFilePath();
            try
            {
                try
                {
                    Monitor.Enter(oAllSessions);
                    TranscoderTuple oExporter = FiddlerApplication.oTranscoders.GetExporter("SAZ");

                    if (null != oExporter)
                    {
                        var dictOptions = new Dictionary<string, object>();
                        dictOptions.Add("Filename", sFilename);
                        bSuccess = FiddlerApplication.DoExport("SAZ", oAllSessions.ToArray(), dictOptions, null);
                    }
                    else
                    {
                        WriteCommandResponse("Save failed because the SAZ Format Exporter was not available.");
                    }
                }
                finally
                {
                    Monitor.Exit(oAllSessions);
                }

                WriteCommandResponse(bSuccess ? ("Wrote: " + sFilename) : ("Failed to save: " + sFilename));
            }
            catch (Exception eX)
            {
                WriteCommandResponse("Save failed: " + eX.Message);
            }
        }

        public void SaveSessionsToDesktop()
        {
            bool bSuccess = false;
            string sFilename = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                               + Path.DirectorySeparatorChar + DateTime.Now.ToString("hh-mm-ss") + ".saz";
            try
            {
                try
                {
                    Monitor.Enter(oAllSessions);
                    TranscoderTuple oExporter = FiddlerApplication.oTranscoders.GetExporter("SAZ");

                    if (null != oExporter)
                    {
                        var dictOptions = new Dictionary<string, object>();
                        dictOptions.Add("Filename", sFilename);
                        // dictOptions.Add("Password", "pencil");

                        bSuccess = FiddlerApplication.DoExport("SAZ", oAllSessions.ToArray(), dictOptions, null);
                    }
                    else
                    {
                        WriteCommandResponse("Save failed because the SAZ Format Exporter was not available.");
                    }
                }
                finally
                {
                    Monitor.Exit(oAllSessions);
                }

                WriteCommandResponse(bSuccess ? ("Wrote: " + sFilename) : ("Failed to save: " + sFilename));
            }
            catch (Exception eX)
            {
                WriteCommandResponse("Save failed: " + eX.Message);
            }
        }

        public void WriteSessionList()
        {
            Common.Log("Session list contains...");
            try
            {
                Monitor.Enter(oAllSessions);
                foreach (Session oS in oAllSessions)
                {
                    Common.Log(String.Format("{0} {1} {2}\n{3} {4}\n\n", oS.id,
                        oS.oRequest.headers.HTTPMethod, Ellipsize(oS.fullUrl, 60),
                        oS.responseCode, oS.oResponse.MIMEType));
                }
            }
            finally
            {
                Monitor.Exit(oAllSessions);
            }
        }

        public void ClearSessionList()
        {
            Monitor.Enter(oAllSessions);
            oAllSessions.Clear();
            Monitor.Exit(oAllSessions);
            WriteCommandResponse("Clear...");
            FiddlerApplication.Log.LogString("Cleared session list.");
        }

        public void ForgetfulStreaming()
        {
            bool bForgetful =
                !FiddlerApplication.Prefs.GetBoolPref("fiddler.network.streaming.ForgetStreamedData", false);
            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.ForgetStreamedData", bForgetful);
            DiagnosticLog.WriteLine(bForgetful
                ? "FiddlerCore will immediately dump streaming response data."
                : "FiddlerCore will keep a copy of streamed response data.");
        }

        public void StartFiddler()
        {
            Common.Log("Starting Fiddler Proxy on port " + proxyPort);
            string sSAZInfo = "NoSAZ";

            if (!FiddlerApplication.oTranscoders.ImportTranscoders(Assembly.GetExecutingAssembly()))
            {
                DiagnosticLog.WriteLine("This assembly was not compiled with a SAZ-exporter");
            }
            else
            {
                sSAZInfo = SAZFormat.GetZipLibraryInfo();
            }
            CONFIG.IgnoreServerCertErrors = true;
            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);


            FiddlerCoreStartupFlags oFCSF = (FiddlerCoreStartupFlags.AllowRemoteClients |
                                             FiddlerCoreStartupFlags.DecryptSSL |
                                             FiddlerCoreStartupFlags.MonitorAllConnections |
                                             //            FiddlerCoreStartupFlags.RegisterAsSystemProxy |
                                             FiddlerCoreStartupFlags.ChainToUpstreamGateway |
                                             FiddlerCoreStartupFlags.CaptureLocalhostTraffic);


            FiddlerApplication.Startup(proxyPort, false, true, true);
            oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
        }
    }
}