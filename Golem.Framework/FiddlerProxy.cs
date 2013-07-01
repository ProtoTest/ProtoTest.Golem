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
using System.IO;
using Fiddler;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Gallio.Framework;
using Golem.Framework;

namespace Golem.Framework
{
    public class FiddlerProxy
    {
        private static bool bUpdateTitle = true;
        private static Fiddler.Proxy oSecureEndpoint;
        private static string sSecureEndpointHostname = "localhost";
        private static int iSecureEndpointPort = 7777;
        private int proxyPort;
        private bool systemProxy;
        private bool decryptSSL;

        private List<Fiddler.Session> oAllSessions;

        public FiddlerProxy()
        {
            oAllSessions = new List<Fiddler.Session>();
            AttachEventListeners();
            this.proxyPort = 8877;
        }

        public FiddlerProxy(int port, bool systemProxy=false)
        {
            oAllSessions = new List<Fiddler.Session>();
            AttachEventListeners();
            this.proxyPort = port;
            this.systemProxy = systemProxy;
            this.decryptSSL = true;
        }

        public static void WriteCommandResponse(string s)
        {
           // Common.Log(s);
        }

        public void QuitFiddler()
        {

            WriteCommandResponse("Shutting down...");
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
            while (FiddlerApplication.IsStarted())
            {
                Common.Log("Stopping Fiddler");
                Fiddler.FiddlerApplication.Shutdown();
                Thread.Sleep(500);
            }
        }

        private string Ellipsize(string s, int iLen)
        {
            if (s.Length <= iLen) return s;
            return s.Substring(0, iLen - 3) + "...";
        }

        private void AttachEventListeners()
        {

            Fiddler.FiddlerApplication.OnNotification +=
                delegate(object sender, NotificationEventArgs oNEA)
                    { WriteCommandResponse("** NotifyUser: " + oNEA.NotifyString); };
            Fiddler.FiddlerApplication.Log.OnLogString +=
                delegate(object sender, LogEventArgs oLEA)
                { WriteCommandResponse("** LogString: " + oLEA.LogString); };

            Fiddler.FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oS)
                {
                    oS.bBufferResponse = false;
                    Monitor.Enter(oAllSessions);
                    oAllSessions.Add(oS);
                    Monitor.Exit(oAllSessions);
                    oS["X-AutoAuth"] = "(default)";

                    //if it's a https request
                    if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) &&
                        (oS.hostname == sSecureEndpointHostname))
                    {
                        oS.utilCreateResponseAndBypassServer();
                        oS.oResponse.headers.HTTPResponseStatus = "200 Ok";
                        oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                        oS.oResponse["Cache-Control"] = "private, max-age=0";
                        oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" +
                                               iSecureEndpointPort.ToString() +
                                               " received. Your request was:<br /><plaintext>" +
                                               oS.oRequest.headers.ToString());
                    }
                };

        }
        public void ReadSessions()
        {
            TranscoderTuple oImporter = FiddlerApplication.oTranscoders.GetImporter("SAZ");
            if (null != oImporter)
            {
                Dictionary<string, object> dictOptions = new Dictionary<string, object>();
                dictOptions.Add("Filename", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ToLoad.saz");

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
                            + System.IO.Path.DirectorySeparatorChar
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
                        Dictionary<string, object> dictOptions = new Dictionary<string, object>();
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
                            + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("hh-mm-ss") + ".saz";
            try
            {
                try
                {
                    Monitor.Enter(oAllSessions);
                    TranscoderTuple oExporter = FiddlerApplication.oTranscoders.GetExporter("SAZ");

                    if (null != oExporter)
                    {
                        Dictionary<string, object> dictOptions = new Dictionary<string, object>();
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

                WriteCommandResponse( bSuccess ? ("Wrote: " + sFilename) : ("Failed to save: " + sFilename) );
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

            string sSAZInfo = "NoSAZ";

            if (!FiddlerApplication.oTranscoders.ImportTranscoders(Assembly.GetExecutingAssembly()))
            {
                DiagnosticLog.WriteLine("This assembly was not compiled with a SAZ-exporter");
            }
            else
            {
                sSAZInfo = Fiddler.SAZFormat.GetZipLibraryInfo();
            }
            Fiddler.CONFIG.IgnoreServerCertErrors = true;
            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

         
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;

            Fiddler.FiddlerApplication.Startup(proxyPort,systemProxy,decryptSSL);

            oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
            if (null != oSecureEndpoint)
            {
                FiddlerApplication.Log.LogFormat(
                    "Created secure end point listening on port {0}, using a HTTPS certificate for '{1}'",
                    iSecureEndpointPort, sSecureEndpointHostname);
            }


        }
    }
}

