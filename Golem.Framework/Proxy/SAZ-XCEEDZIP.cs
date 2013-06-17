#if SAZ_SUPPORT
#if USE_XCEED
// THIS CODE SAMPLE & SOFTWARE IS LICENSED "AS-IS." YOU BEAR THE RISK OF USING IT. Telerik GIVES NO EXPRESS WARRANTIES, GUARANTEES OR CONDITIONS. 
// Full license terms are contained in the file License.txt in the package. 
//
// This class allows your FiddlerCore program to save and load SAZ files (http://fiddler.wikidot.com/saz-files).
// This version of the class is based on the commercial XCEED ZIP for .NET library, version 4.2, available from http://xceed.com/. Version 5.0 should also be compatible.
//
// To use it:
//  1. Add this file to your project.
//  2. Add the XCEED.ZIP, XCEED.FileSystem, XCeed.Compression dlls to your Project's REFERENCES list.
//  3. Edit the Project Properties to set the conditional compilation symbols SAZ_SUPPORT; USE_XCEED
//  4. Set your private XCEED LICENSE KEY in the method provided below.
//
// TODO: Support opening of Password-Protected archives
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Fiddler;
using Xceed.FileSystem;
using Xceed.Zip;


[assembly: RequiredVersionAttribute("2.3.7.0")]
namespace Fiddler
{
    // This class MUST be public in order to allow it to be found by reflection
    [ProfferFormat("SAZ", "Fiddler's native session archive zip format")]
    public class SAZFormat: ISessionImporter, ISessionExporter
    {
        static SAZFormat()
        {
            // The XCEED Compression libraries REQUIRE that you obtain your own license key and set it before any compression
            // methods are called. You MUST enter your license key below.
            //
            Xceed.Zip.Licenser.LicenseKey = "ZIN50-XXXXX-XXXXX-XXXX"; 
        }
        /// <summary>
        /// Returns a string indicating the ZipLibrary version information
        /// </summary>
        /// <returns></returns>
        public static string GetZipLibraryInfo()
        {
            return "Xceed";
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

            ZipArchive oZip = null;
            List<Session> outSessions = new List<Session>();

            try
            {
                // Sniff for ZIP file.
                FileStream oSniff = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (oSniff.Length < 64 || oSniff.ReadByte() != 0x50 || oSniff.ReadByte() != 0x4B)
                {  // Sniff for 'PK'
                    FiddlerApplication.Log.LogString("SAZFormat> ReadSessionArchive Failed. " + sFilename + " is not a Fiddler-generated .SAZ archive of HTTP Sessions.");
                    oSniff.Close();
                    return null;
                }
                oSniff.Close();

                oZip = new ZipArchive(new DiskFile(sFilename));
                oZip.BeginUpdate();

                AbstractFolder oRaw = oZip.GetFolder("raw");
                if (!oRaw.Exists)
                {
                    FiddlerApplication.Log.LogString("SAZFormat> ReadSessionArchive Failed. The selected ZIP is not a Fiddler-generated .SAZ archive of HTTP Sessions.");
                    oZip.EndUpdate();
                    return null;
                }

                foreach (AbstractFile oRequestFile in oRaw.GetFiles(true, @"*_c.txt"))
                {
                    try
                    {
                        byte[] arrRequest = new byte[oRequestFile.Size];
                        Stream oFS;

                    RetryWithPassword:
                        try
                        {
                            oFS = oRequestFile.OpenRead(FileShare.Read);
                        }
                        catch (Xceed.Zip.InvalidDecryptionPasswordException)
                        {
                            DiagnosticLog.WriteLine("Password-Protected Session Archive.\nEnter the password to decrypt, or enter nothing to abort opening.\n>");
                            string sPassword = Console.ReadLine();
                            if (sPassword != String.Empty)
                            {
                                oZip.DefaultDecryptionPassword = sPassword;
                                goto RetryWithPassword;
                            }

                            return null;
                        }
                        int iRead = Utilities.ReadEntireStream(oFS, arrRequest);
                        oFS.Close();
                        Debug.Assert(iRead == arrRequest.Length, "Failed to read entire request.");

                        AbstractFile oResponseFile = oRaw.GetFile(oRequestFile.Name.Replace("_c.txt", "_s.txt"));
                        if (!oResponseFile.Exists)
                        {
                            FiddlerApplication.Log.LogString("Could not find a server response for: " + oResponseFile.FullName);
                            continue;
                        }

                        byte[] arrResponse = new byte[oResponseFile.Size];
                        oFS = oResponseFile.OpenRead();
                        iRead = Utilities.ReadEntireStream(oFS, arrResponse);
                        oFS.Close();
                        Debug.Assert(iRead == arrResponse.Length, "Failed to read entire response.");

                        oResponseFile = oRaw.GetFile(oRequestFile.Name.Replace("_c.txt", "_m.xml"));

                        Session oSession = new Session(arrRequest, arrResponse);

                        if (oResponseFile.Exists)
                        {
                            oSession.LoadMetadata(oResponseFile.OpenRead());    // Method closes the stream
                        }
                        oSession.oFlags["x-LoadedFrom"] = oRequestFile.Name.Replace("_c.txt", "_s.txt");
                        outSessions.Add(oSession);

                    }
                    catch (Exception eX)
                    {
                        FiddlerApplication.Log.LogString("SAZFormat> ReadSessionArchive incomplete. Invalid data was present in session: " + oRequestFile.FullName + ".\n\n\n" + eX.Message + "\n" + eX.StackTrace);
                    }
                }
            }
            catch (Exception eX)
            {
                FiddlerApplication.ReportException(eX, "ReadSessionArchive Error");
                return null;
            }

            if (null != oZip)
            {
                oZip.EndUpdate();
                oZip = null;
            }

            return outSessions.ToArray();

        }

        private static bool WriteSessionArchive(string sFilename, Session[] arrSessions, string sPassword, bool bDisplayErrorMessages)
        {
            if ((null == arrSessions || (arrSessions.Length < 1)))
            {
                if (bDisplayErrorMessages)
                {
                    FiddlerApplication.Log.LogString("WriteSessionArchive - No Input. No sessions were provided to save to the archive.");
                }
                return false;
            }

            try
            {
                if (File.Exists(sFilename))
                {
                    File.Delete(sFilename);
                }

                DiskFile odfZip = new DiskFile(sFilename);
                ZipArchive oZip = new ZipArchive(odfZip);
                oZip.TempFolder = new MemoryFolder();

                oZip.BeginUpdate();
                ZippedFolder oZipRawFolder = (ZippedFolder)oZip.CreateFolder("raw");

#region PasswordProtectIfNeeded
                if (!String.IsNullOrEmpty(sPassword))
                {
                    if (CONFIG.bUseAESForSAZ)
                    {
                        oZip.DefaultEncryptionMethod = EncryptionMethod.WinZipAes;  // Use 256bit AES
                    }
                    oZip.DefaultEncryptionPassword = sPassword;
                }
#endregion PasswordProtectIfNeeded

                oZip.Comment = Fiddler.CONFIG.FiddlerVersionInfo + " " + GetZipLibraryInfo() + " Session Archive. See http://www.fiddler2.com";

#region ProcessEachSession
                int iFileNumber = 1;
                // Our format string must pad all session ids with leading 0s for proper sorting.
                string sFileNumberFormatter = ("D" + arrSessions.Length.ToString().Length);
               
                foreach (Session oSession in arrSessions)
                {
                    WriteSessionToSAZ(oSession, odfZip, iFileNumber, sFileNumberFormatter, null, bDisplayErrorMessages);
                    iFileNumber++;
                }
#endregion ProcessEachSession

                oZip.EndUpdate();
                return true;
            }
            catch (Exception eX)
            {
                // TODO: Should close any open handles here. 
                if (bDisplayErrorMessages)
                {
                    FiddlerApplication.Log.LogString("Failed to save Session Archive.\n\n" + eX.Message);
                }
                return false;
            }
        }

        // This is a refactored helper function which writes a single session to an open SAZ file.
        internal static void WriteSessionToSAZ(Session oSession, DiskFile odfZip, int iFileNumber, string sFileNumberFormatter, StringBuilder sbHTML, bool bDisplayErrorMessages)
        {
            string sBaseFilename = @"raw\" + iFileNumber.ToString(sFileNumberFormatter);
            string sRequestFilename = sBaseFilename + "_c.txt";
            string sResponseFilename = sBaseFilename + "_s.txt";
            string sMetadataFilename = sBaseFilename + "_m.xml";

            // Write the Request to the Archive
            try
            {
                ZippedFile o = new ZippedFile(odfZip, sRequestFilename);
                using (Stream oS = o.CreateWrite(FileShare.None))
                {
                    oSession.WriteRequestToStream(false, true, oS);
                }
            }
            catch (Exception eX)
            {
                if (bDisplayErrorMessages)
                {
                    FiddlerApplication.Log.LogString("Archive Failure: Unable to add " + sRequestFilename + "\n\n" + eX.Message);
                }
            }

            // Write the Response to the Archive
            try
            {
                ZippedFile o = new ZippedFile(odfZip, sResponseFilename);
                using (Stream oS = o.CreateWrite(FileShare.None))
                {
                    oSession.WriteResponseToStream(oS, false);
                }
            }
            catch (Exception eX)
            {
                if (bDisplayErrorMessages)
                {
                    FiddlerApplication.Log.LogString("Archive Failure: Unable to add " + sResponseFilename + "\n\n" + eX.Message);
                }
            }

            // Write the MetaData to the Archive
            try
            {
                ZippedFile o = new ZippedFile(odfZip, sMetadataFilename);
                using (Stream oS = o.CreateWrite(FileShare.None))
                {
                    oSession.WriteMetadataToStream(oS);
                }
            }
            catch (Exception eX)
            {
                if (bDisplayErrorMessages)
                {
                    FiddlerApplication.Log.LogString("Archive Failure: Unable to add " + sMetadataFilename + "\n\n" + eX.Message);
                }
            }

#region AddIndexHTMLEntry
            if (null != sbHTML)
            {
                sbHTML.Append("<tr>");
                sbHTML.Append("<TD><a href='" + sRequestFilename + "'>C</a>&nbsp;");
                sbHTML.Append("<a href='" + sResponseFilename + "'>S</a>&nbsp;");
                sbHTML.Append("<a href='" + sMetadataFilename + "'>M</a></TD>");
                sbHTML.Append("</tr>");
            }
#endregion AddIndexHTMLEntry
        }

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

#endregion

        public void Dispose()
        {
            // nothing to do here.
        }

#endregion PublicInterface
    }
}
#endif
#endif