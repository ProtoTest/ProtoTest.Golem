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
using Fiddler;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

namespace Demo
{
    class Program
    {
        static bool bUpdateTitle = true;
        static Proxy oSecureEndpoint;
        static string sSecureEndpointHostname = "localhost";
        static int iSecureEndpointPort = 7777;

        public static void WriteCommandResponse(string s)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ForegroundColor = oldColor;
        }

        public static void DoQuit()
        {
            WriteCommandResponse("Shutting down...");
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
            Fiddler.FiddlerApplication.Shutdown();
            Thread.Sleep(500);
        }
        private static string Ellipsize(string s, int iLen)
        {
            if (s.Length <= iLen) return s;
            return s.Substring(0, iLen - 3) + "...";
        }

#if SAZ_SUPPORT
        private static void ReadSessions(List<Fiddler.Session> oAllSessions)
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

        private static void SaveSessionsToDesktop(List<Fiddler.Session> oAllSessions)
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
                        Console.WriteLine("Save failed because the SAZ Format Exporter was not available.");
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
                Console.WriteLine("Save failed: " + eX.Message);
            }
        }
#endif

        private static void WriteSessionList(List<Fiddler.Session> oAllSessions)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Session list contains...");
            try
            {
                Monitor.Enter(oAllSessions);
                foreach (Session oS in oAllSessions)
                {
                    Console.Write(String.Format("{0} {1} {2}\n{3} {4}\n\n", oS.id, oS.oRequest.headers.HTTPMethod, Ellipsize(oS.fullUrl, 60), oS.responseCode, oS.oResponse.MIMEType));
                }
            }
            finally
            {
                Monitor.Exit(oAllSessions);
            }
            Console.WriteLine();
            Console.ForegroundColor = oldColor;
        }

        static void Main(string[] args)
        {
            List<Fiddler.Session> oAllSessions = new List<Fiddler.Session>();
            #region AttachEventListeners
            //
            // It is important to understand that FiddlerCore calls event handlers on session-handling
            // background threads.  If you need to properly synchronize to the UI-thread (say, because
            // you're adding the sessions to a list view) you must call .Invoke on a delegate on the 
            // window handle.
            // 
            // If you are writing to a non-threadsafe data structure (e.g. List<t>) you must
            // use a Monitor or other mechanism to ensure safety.
            //

            // Simply echo notifications to the console.  Because Fiddler.CONFIG.QuietMode=true 
            // by default, we must handle notifying the user ourselves.
            Fiddler.FiddlerApplication.OnNotification += delegate(object sender, NotificationEventArgs oNEA) { Console.WriteLine("** NotifyUser: " + oNEA.NotifyString); };
            Fiddler.FiddlerApplication.Log.OnLogString += delegate(object sender, LogEventArgs oLEA) { Console.WriteLine("** LogString: " + oLEA.LogString); };

            Fiddler.FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oS)
            {
                // Console.WriteLine("Before request for:\t" + oS.fullUrl);
                // In order to enable response tampering, buffering mode MUST
                // be enabled; this allows FiddlerCore to permit modification of
                // the response in the BeforeResponse handler rather than streaming
                // the response to the client as the response comes in.
                oS.bBufferResponse = false;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);
                oS["X-AutoAuth"] = "(default)";
                /* If the request is going to our secure endpoint, we'll echo back the response.
                
                Note: This BeforeRequest is getting called for both our main proxy tunnel AND our secure endpoint, 
                so we have to look at which Fiddler port the client connected to (pipeClient.LocalPort) to determine whether this request 
                was sent to secure endpoint, or was merely sent to the main proxy tunnel (e.g. a CONNECT) in order to *reach* the secure endpoint.
                
                As a result of this, if you run the demo and visit https://localhost:7777 in your browser, you'll see
                
                Session list contains...
                 
                    1 CONNECT http://localhost:7777
                    200                                         <-- CONNECT tunnel sent to the main proxy tunnel, port 8877

                    2 GET https://localhost:7777/
                    200 text/html                               <-- GET request decrypted on the main proxy tunnel, port 8877

                    3 GET https://localhost:7777/               
                    200 text/html                               <-- GET request received by the secure endpoint, port 7777
                */

                if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.HTTPResponseStatus = "200 Ok";
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://"+sSecureEndpointHostname+ ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
            };

            /*
                // The following event allows you to examine every response buffer read by Fiddler. Note that this isn't useful for the vast majority of
                // applications because the raw buffer is nearly useless; it's not decompressed, it includes both headers and body bytes, etc.
                //
                // This event is only useful for a handful of applications which need access to a raw, unprocessed byte-stream
                Fiddler.FiddlerApplication.OnReadResponseBuffer += new EventHandler<RawReadEventArgs>(FiddlerApplication_OnReadResponseBuffer);
            */

            /*
            Fiddler.FiddlerApplication.BeforeResponse += delegate(Fiddler.Session oS) {
                // Console.WriteLine("{0}:HTTP {1} for {2}", oS.id, oS.responseCode, oS.fullUrl);
                
                // Uncomment the following two statements to decompress/unchunk the
                // HTTP response and subsequently modify any HTTP responses to replace 
                // instances of the word "Microsoft" with "Bayden". You MUST also
                // set bBufferResponse = true inside the beforeREQUEST method above.
                //
                //oS.utilDecodeResponse(); oS.utilReplaceInResponse("Microsoft", "Bayden");
            };*/

            Fiddler.FiddlerApplication.AfterSessionComplete += delegate(Fiddler.Session oS)
            {
                //Console.WriteLine("Finished session:\t" + oS.fullUrl); 
                if (bUpdateTitle)
                {
                    Console.Title = ("Session list contains: " + oAllSessions.Count.ToString() + " sessions");
                }
            };

            // Tell the system console to handle CTRL+C by calling our method that
            // gracefully shuts down the FiddlerCore.
            //
            // Note, this doesn't handle the case where the user closes the window with the close button.
            // See http://geekswithblogs.net/mrnat/archive/2004/09/23/11594.aspx for info on that...
            //
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            #endregion AttachEventListeners

            string sSAZInfo = "NoSAZ";
#if SAZ_SUPPORT
            // If this demo was compiled with a SAZ-Transcoder, then the following lines will load the
            // Transcoders into the available transcoders. You can load other types of Transcoders from
            // a different assembly if you'd like, using the ImportTranscoders(string AssemblyPath) overload.
            // See https://www.fiddler2.com/dl/FiddlerCore-BasicFormats.zip for an example.
            //
            if (!FiddlerApplication.oTranscoders.ImportTranscoders(Assembly.GetExecutingAssembly()))
            {
                Console.WriteLine("This assembly was not compiled with a SAZ-exporter");
            }
            else
            {
                sSAZInfo = SAZFormat.GetZipLibraryInfo();
            }
#endif

            Console.WriteLine(String.Format("Starting {0} ({1})...", Fiddler.FiddlerApplication.GetVersionString(), sSAZInfo));

            // For the purposes of this demo, we'll forbid connections to HTTPS 
            // sites that use invalid certificates. Change this from the default only
            // if you know EXACTLY what that implies.
            Fiddler.CONFIG.IgnoreServerCertErrors = false;

            // ... but you can allow a specific (even invalid) certificate by implementing and assigning a callback...
            // FiddlerApplication.OnValidateServerCertificate += new System.EventHandler<ValidateServerCertificateEventArgs>(CheckCert);

            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

            // For forward-compatibility with updated FiddlerCore libraries, it is strongly recommended that you 
            // start with the DEFAULT options and manually disable specific unwanted options.
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;

            // E.g. If you want to add a flag, start with the Defaults and "or" it in:
            // oFCSF = (oFCSF | FiddlerCoreStartupFlags.CaptureFTP);

            // ... or if you don't want a flag in the defaults, "and not" it out:
            // Uncomment the next line if you don't want FiddlerCore to act as the system proxy
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.RegisterAsSystemProxy);
            // or uncomment the next line if you don't want to decrypt SSL traffic.
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.DecryptSSL);
            //
            // NOTE: Unless you disable the option to decrypt HTTPS traffic, makecert.exe
            // must be present in this executable's folder.

            // NOTE: In the next line, you can pass 0 for the port (instead of 8877) to have FiddlerCore auto-select an available port
            Fiddler.FiddlerApplication.Startup(8877, oFCSF);

            FiddlerApplication.Log.LogFormat("Starting with settings: [{0}]", oFCSF);
            FiddlerApplication.Log.LogFormat("Gateway: {0}", CONFIG.UpstreamGateway.ToString());

            Console.WriteLine("Hit CTRL+C to end session.");

            // We'll also create a HTTPS listener, useful for when FiddlerCore is masquerading as a HTTPS server
            // instead of acting as a normal CERN-style proxy server.
            oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
            if (null != oSecureEndpoint)
            {
                FiddlerApplication.Log.LogFormat("Created secure end point listening on port {0}, using a HTTPS certificate for '{1}'", iSecureEndpointPort , sSecureEndpointHostname);
            }
            
            bool bDone = false;
            do
            {
                Console.WriteLine("\nEnter a command [C=Clear; L=List; G=Collect Garbage; W=write SAZ; R=read SAZ;\n\tS=Toggle Forgetful Streaming; T=Toggle Title Counter; Q=Quit]:");
                Console.Write(">");
                ConsoleKeyInfo cki = Console.ReadKey();
                Console.WriteLine();
                switch (cki.KeyChar)
                {
                    case 'c':
                        Monitor.Enter(oAllSessions);
                        oAllSessions.Clear();
                        Monitor.Exit(oAllSessions);
                        WriteCommandResponse("Clear...");
                        FiddlerApplication.Log.LogString("Cleared session list.");
                        break;

                    case 'd':
                        FiddlerApplication.Log.LogString("FiddlerApplication::Shutdown.");
                        FiddlerApplication.Shutdown();
                        break;

                    case 'l':
                        WriteSessionList(oAllSessions);
                        break;

                    case 'g':
                        Console.WriteLine("Working Set:\t" + Environment.WorkingSet.ToString("n0"));
                        Console.WriteLine("Begin GC...");
                        GC.Collect();
                        Console.WriteLine("GC Done.\nWorking Set:\t" + Environment.WorkingSet.ToString("n0"));
                        break;

                    case 'q':
                        bDone = true;
                        DoQuit();
                        break;

                    case 'r':
#if SAZ_SUPPORT
                        ReadSessions(oAllSessions);
#else
                        WriteCommandResponse("This demo was compiled without SAZ_SUPPORT defined");
#endif
                        break;

                    case 'w':
#if SAZ_SUPPORT
                        if (oAllSessions.Count > 0)
                        {
                            SaveSessionsToDesktop(oAllSessions);
                        }
                        else
                        {
                            WriteCommandResponse("No sessions have been captured");
                        }
#else
                        WriteCommandResponse("This demo was compiled without SAZ_SUPPORT defined");
#endif
                        break;

                    case 't':
                        bUpdateTitle = !bUpdateTitle;
                        Console.Title = (bUpdateTitle) ? "Title bar will update with request count..." :
                            "Title bar update suppressed...";
                        break;

                    // Forgetful streaming
                    case 's':
                        bool bForgetful = !FiddlerApplication.Prefs.GetBoolPref("fiddler.network.streaming.ForgetStreamedData", false);
                        FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.ForgetStreamedData", bForgetful);
                        Console.WriteLine( bForgetful ? "FiddlerCore will immediately dump streaming response data." : "FiddlerCore will keep a copy of streamed response data.");
                        break;

                }
            } while (!bDone);
        }

        /*
        /// <summary>
        /// This callback allows your code to evaluate the certificate for a site and optionally override default validation behavior for that certificate.
        /// You should not implement this method unless you understand why it is a security risk.
        /// </summary>
        static void CheckCert(object sender, ValidateServerCertificateEventArgs e)
        {
            if (null != e.ServerCertificate)
            {
                Console.WriteLine("Certificate for " + e.ExpectedCN + " was for site " + e.ServerCertificate.Subject + " and errors were " + e.CertificatePolicyErrors.ToString());

                if (e.ServerCertificate.Subject.Contains("fiddler2.com"))
                {
                    Console.WriteLine("Got a certificate for fiddler2.com. We'll say this is also good for any other site, like https://fiddlertool.com.");
                    e.ValidityState = CertificateValidity.ForceValid;
                }
            }
        }
        */

        /*
        // This event handler is called on every socket read for the HTTP Response. You almost certainly don't want
        // to add a handler for this event, but the code below shows how you can use it to mess up your HTTP traffic.
        static void FiddlerApplication_OnReadResponseBuffer(object sender, RawReadEventArgs e)
        {
            // NOTE: arrDataBuffer is a fixed-size array. Only bytes 0 to iCountOfBytes should be read/manipulated.
            //
            // Just for kicks, lowercase every byte. Note that this will obviously break any binary content.
            for (int i = 0; i < e.iCountOfBytes; i++)
            {
                if ((e.arrDataBuffer[i] > 0x40) && (e.arrDataBuffer[i] < 0x5b))
                {
                    e.arrDataBuffer[i] = (byte)(e.arrDataBuffer[i] + (byte)0x20);
                }
            }
            Console.WriteLine(String.Format("Read {0} response bytes for session {1}", e.iCountOfBytes, e.sessionOwner.id));
        }
        */

        /// <summary>
        /// When the user hits CTRL+C, this event fires.  We use this to shut down and unregister our FiddlerCore.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            DoQuit();
        }
    }
}

