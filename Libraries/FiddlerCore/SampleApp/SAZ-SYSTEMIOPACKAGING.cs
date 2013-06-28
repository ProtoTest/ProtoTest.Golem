#if SAZ_SUPPORT
#if USE_SYSTEMIOPACKAGING
// THIS CODE SAMPLE & SOFTWARE IS LICENSED "AS-IS." YOU BEAR THE RISK OF USING IT. Telerik GIVES NO EXPRESS WARRANTIES, GUARANTEES OR CONDITIONS. 
// Full license terms are contained in the file License.txt in the package. 
//
// This class allows your FiddlerCore program to save and load SAZ files (http://fiddler.wikidot.com/saz-files).
//
// This version of the class is based on the free System.IO.Packaging assembly found in the .NET Framework version 3.0
// It can load SAZ files saved by Fiddler version 2.4.0.9/4.4.0.9 and later, as these have the required Packaging-compatible format.
//
// Note: Password-Protection of SAZ files is not supported by this class.
//
// To use it:
//  1. Add this file to your .NET3.0-targeted Project.
//  2. Ensure your Project's References list includes WindowsBase.dll
//  3. Edit the Project Properties to set the conditional compilation symbols SAZ_SUPPORT; USE_SYSTEMIOPACKAGING
//
using System;
using System.IO;
using System.IO.Packaging;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Fiddler;

[assembly: RequiredVersionAttribute("2.3.7.5")]
namespace Fiddler
{
    // This class MUST be public in order to allow it to be found by reflection
    [ProfferFormat("SAZ", "Fiddler's native session archive zip format")]
    public class SAZFormat: ISessionImporter, ISessionExporter
    {
#region Internal-Implementation-Details

        /// <summary>
        /// Reads all bytes from a stream and returns the content as a byte[]
        /// Different than the Utilities method of this name in that it does
        /// not need to know the size of the target array ahead of time
        /// </summary>
        /// <param name="oS"></param>
        /// <returns></returns>
        private static byte[] ReadEntireStream(Stream oS)
        {
            MemoryStream oMS = new MemoryStream();
            byte[] buffer = new byte[32768];
            int bytesRead = 0;
            while ((bytesRead = oS.Read(buffer, 0, buffer.Length)) > 0)
            {
                oMS.Write(buffer, 0, bytesRead);
            }
            return oMS.ToArray();
        }

        /// <summary>
        /// Reads a session archive zip file into an array of Session objects
        /// </summary>
        /// <param name="sFilename">Filename to load</param>
        /// <param name="bVerboseDialogs"></param>
        /// <returns>Loaded array of sessions or null, in case of failure</returns>        
        private static Session[] ReadSessionArchive(string sFilename, bool bVerboseDialogs)
        {     
            /*  Okay, given the zip, we gotta:
            *		Unzip
            *		Find all matching pairs of request, response
            *		Create new Session object for each pair
            */
           
            if (!File.Exists(sFilename))
            {
                FiddlerApplication.Log.LogString("SAZFormat> ReadSessionArchive Failed. File " + sFilename + " does not exist.");
                return null;
            }

            List<Session> outSessions = new List<Session>();

            try
            {
                // Sniff for ZIP file.
                using (FileStream oSniff = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (oSniff.Length < 64 || oSniff.ReadByte() != 0x50 || oSniff.ReadByte() != 0x4B)
                    {  // Sniff for 'PK'
                        FiddlerApplication.Log.LogString("SAZFormat> ReadSessionArchive Failed. " + sFilename + " is not a Fiddler-generated .SAZ archive of HTTP Sessions.");
                        return null;
                    }
                }

                using (Package oZip = Package.Open(sFilename, FileMode.Open))
                {
                    PackagePartCollection oAllParts = oZip.GetParts();
                    foreach (PackagePart oPPC in oAllParts)
                    {
                        string sURI = oPPC.Uri.ToString();
                        if (!sURI.EndsWith("_c.txt")) continue;

                        byte[] arrRequest = null;
                        byte[] arrResponse = null;

                        // Okay, we now have a Request. Let's read it.
                        using (Stream oData = oPPC.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            arrRequest = ReadEntireStream(oData);
                        }

                        try
                        {
                            sURI = sURI.Replace("_c.txt", "_s.txt");

                            // Get the response
                            PackagePart oResponse = oZip.GetPart(new Uri(sURI, UriKind.Relative));
                            using (Stream oData = oResponse.GetStream(FileMode.Open, FileAccess.Read))
                            {
                                arrResponse = ReadEntireStream(oData);
                            }
                        }
                        catch (Exception eX)
                        {
                            FiddlerApplication.Log.LogString("Could not load Server Response: " + sURI + "\n" + eX.Message);
                            // Fatal error. Skip this session
                            continue;
                        }

                        Session oSession = new Session(arrRequest, arrResponse);
                        oSession.oFlags["x-LoadedFrom"] = sURI;

                        sURI = sURI.Replace("_s.txt", "_m.xml");

                        try
                        {
                            // Get the Metadata
                            PackagePart oMetadata = oZip.GetPart(new Uri(sURI, UriKind.Relative));
                            Stream oData = oMetadata.GetStream(FileMode.Open, FileAccess.Read);
                            oSession.LoadMetadata(oData);  // Note: Closes the stream automatically
                        }
                        catch
                        {
                            FiddlerApplication.Log.LogString("Could not load Metadata: " + sURI);
                            // Missing metadata is not-fatal.
                        }
                        outSessions.Add(oSession);
                    }
                }
            }
            catch (Exception eX)
            {
                FiddlerApplication.ReportException(eX, "ReadSessionArchive Error");
                return null;
            }

            return outSessions.ToArray();
        }

        private static bool WriteSessionArchive(string sFilename, Session[] arrSessions, string sPassword, bool bVerboseDialogs)
        {
            if ((null == arrSessions || (arrSessions.Length < 1)))
            {
                if (bVerboseDialogs)
                {
                    FiddlerApplication.Log.LogString("WriteSessionArchive - No Input. No sessions were provided to save to the archive.");
                }
                return false;
            }

            try
            {
                using (Package oZip = Package.Open(sFilename, FileMode.Create))
                {
                    // Note: No obvious way to set comments with System.IO.Packaging...

                    #region ProcessEachSession
                    int iFileNumber = 1;
                    // Our format string must pad all session ids with leading 0s for proper sorting.
                    string sFileNumberFormatter = ("D" + arrSessions.Length.ToString().Length);

                    foreach (Session oSession in arrSessions)
                    {
                        #region GenerateIn-PackageURIs
                        string sBaseFilename = @"/raw/" + iFileNumber.ToString(sFileNumberFormatter);
                        Uri uriRequest = PackUriHelper.CreatePartUri(new Uri(sBaseFilename + "_c.txt", UriKind.Relative)); 
                        Uri uriResponse = PackUriHelper.CreatePartUri(new Uri(sBaseFilename + "_s.txt", UriKind.Relative)); 
                        Uri uriMetadata = PackUriHelper.CreatePartUri(new Uri(sBaseFilename + "_m.xml", UriKind.Relative));
                        #endregion

                        #region WriteRequest
                        PackagePart oPart = oZip.CreatePart(uriRequest, "text/plain", CompressionOption.Normal);
                        using (Stream strmWriteTo = oPart.GetStream())
                        {
                            oSession.WriteRequestToStream(false, true, strmWriteTo);
                        }
                        #endregion

                        #region WriteResponse
                        oPart = oZip.CreatePart(uriResponse, "text/plain", CompressionOption.Normal);
                        using (Stream strmWriteTo = oPart.GetStream())
                        {
                            oSession.WriteResponseToStream(strmWriteTo, false);
                        }
                        #endregion

                        #region WriteMetadata
                        oPart = oZip.CreatePart(uriMetadata, "application/xml", CompressionOption.Normal);
                        using (Stream strmWriteTo = oPart.GetStream())
                        {
                            oSession.WriteMetadataToStream(strmWriteTo);
                        }
                        #endregion

                        iFileNumber++;
                    }
                    #endregion ProcessEachSession
                }

                return true;
            }
            catch (Exception eX)
            {
                FiddlerApplication.Log.LogString("WriteSessionArchive failed to save Session Archive. " + eX.Message);
                return false;
            }
        }
#endregion Internal-Implementation-Details

#region PublicInterface

#region ISessionExporter Members

        public bool ExportSessions(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if ((sExportFormat != "SAZ")) return false;
            string sFilename = null;
            string sPassword = null;
            if (null != dictOptions && dictOptions.ContainsKey("Filename"))
            {
                sFilename = dictOptions["Filename"] as string;
            }
            if (string.IsNullOrEmpty(sFilename)) sFilename = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) 
                                                            + System.IO.Path.DirectorySeparatorChar + DateTime.Now.ToString("hh-mm-ss") + ".saz";
            if (null != dictOptions && dictOptions.ContainsKey("Password"))
            {
                sPassword = dictOptions["Password"] as string;
            }
            
            return WriteSessionArchive(sFilename, oSessions, sPassword, false);
        }

#endregion
#region ISessionImporter Members

        public Session[] ImportSessions(string sImportFormat, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            if ((sImportFormat != "SAZ")) return null;

            string sFilename = null;
            if (null != dictOptions && dictOptions.ContainsKey("Filename"))
            {
                sFilename = dictOptions["Filename"] as string;
            }
            if (string.IsNullOrEmpty(sFilename)) return null;

            return ReadSessionArchive(sFilename, true);
        }

        public static string GetZipLibraryInfo()
        {
            return "Using .NET3.0's System.IO.Packaging for SAZ Support";
        }
#endregion

        public void Dispose()
        {
            // nothing to do here.
        }

#endregion
    }
}
#endif
#endif