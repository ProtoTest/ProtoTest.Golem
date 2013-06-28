// THIS CODE SAMPLE & SOFTWARE IS LICENSED "AS-IS." YOU BEAR THE RISK OF USING IT. Telerik GIVES NO EXPRESS WARRANTIES, GUARANTEES OR CONDITIONS. 
// Full license terms are contained in the file License.txt in the package. 
//
// This class allows your FiddlerCore program to save and load SAZ files (http://fiddler.wikidot.com/saz-files).
//
// This version of the class is based on the free DotNetZIP class library, available from http://dotnetzip.codeplex.com
//
// To use it:
//  1. Add this file to your project.
//  2. Download the DotNetZip library from http://dotnetzip.codeplex.com/releases/view/27890. It's licensed under the MS Public License.
//  3. Add Ionic.Zip.Reduced to your Project's REFERENCES list. 
//  4. Edit the Project Properties to set the conditional compilation symbols SAZ_SUPPORT; USE_DOTNETZIP
//
// This version of SAZFormat was built using DotNetZip version 1.9.1.5
using System;
using Ionic.Zip;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Fiddler;

[assembly: RequiredVersionAttribute("2.3.7.0")]
namespace Fiddler
{
    // This class MUST be public in order to allow it to be found by reflection
    [ProfferFormat("SAZ", "Fiddler's native session archive zip format")]
    public class SAZFormat: ISessionImporter, ISessionExporter
    {
#region Internal-Implementation-Details
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

            ZipFile oZip = null;
            string sPassword = String.Empty;
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

                oZip = new ZipFile(sFilename);

                if (!oZip.EntryFileNames.Contains("raw/"))
                {
                    FiddlerApplication.Log.LogString("SAZFormat> ReadSessionArchive Failed. The selected ZIP is not a Fiddler-generated .SAZ archive of HTTP Sessions.");
                    return null;
                }

                foreach (ZipEntry oZE in oZip)
                {
                    // Not a request. Skip it.
                    if (!oZE.FileName.EndsWith("_c.txt") || !oZE.FileName.StartsWith("raw/")) continue;

                GetPassword:
                    if (oZE.Encryption != EncryptionAlgorithm.None && (String.Empty == sPassword)) 
                    {
                        Console.Write("Password-Protected Session Archive.\nEnter the password to decrypt, or enter nothing to abort opening.\n>");
                        sPassword = Console.ReadLine();
                        if (sPassword != String.Empty)
                        {
                            oZip.Password = sPassword;
                        }
                        else
                        {
                            return null;
                        }
                    }

                    try
                    {
                        byte[] arrRequest = new byte[oZE.UncompressedSize];
                        Stream oFS;
                        try
                        {
                            oFS = oZE.OpenReader();
                        }
                        catch (Ionic.Zip.BadPasswordException)
                        {
                            Console.WriteLine("Incorrect password.");
                            sPassword = String.Empty;
                            goto GetPassword;
                        }
                        int iRead = Utilities.ReadEntireStream(oFS, arrRequest);
                        oFS.Close();
                        Debug.Assert(iRead == arrRequest.Length, "Failed to read entire request.");

                        ZipEntry oZEResponse = oZip[oZE.FileName.Replace("_c.txt", "_s.txt")];

                        if (null == oZEResponse)
                        {
                            FiddlerApplication.Log.LogString("Could not find a server response for: " + oZE.FileName);
                            continue;
                        }

                        byte[] arrResponse = new byte[oZEResponse.UncompressedSize];
                        oFS = oZEResponse.OpenReader();
                        iRead = Utilities.ReadEntireStream(oFS, arrResponse);
                        oFS.Close();
                        Debug.Assert(iRead == arrResponse.Length, "Failed to read entire response.");

                        Session oSession = new Session(arrRequest, arrResponse);

                        ZipEntry oZEMetadata = oZip[oZE.FileName.Replace("_c.txt", "_m.xml")];

                        if (null != oZEMetadata)
                        {
                            oSession.LoadMetadata(oZEMetadata.OpenReader());    // Method closes the stream
                        }
                        oSession.oFlags["x-LoadedFrom"] = oZE.FileName.Replace("_c.txt", "_s.txt");
                        outSessions.Add(oSession);
                    }
                    catch (Exception eX)
                    {
                        FiddlerApplication.Log.LogString("SAZFormat> ReadSessionArchive incomplete. Invalid data was present in session: " + oZE.FileName + ".\n\n\n" + eX.Message + "\n" + eX.StackTrace);
                    }
                }
                      /*    RetryWithPassword:
                          try
                          {
                              oFS = oRequestFile.OpenRead(FileShare.Read);
                          }
                          catch (Xceed.Zip.InvalidDecryptionPasswordException)
                          {
                            Console.WriteLine("Password-Protected Session Archive.\nEnter the password to decrypt, or enter nothing to abort opening.\n>");
                            string sPassword = Console.ReadLine();
                            if (sPassword != String.Empty)
                            {
                                oZip.DefaultDecryptionPassword = sPassword;
                                goto RetryWithPassword;
                            }

                            return null;
                          }
                 */
            }
            catch (Exception eX)
            {
                FiddlerApplication.ReportException(eX, "ReadSessionArchive Error");
                return null;
            }

            if (null != oZip)
            {
                oZip.Dispose();
                oZip = null;
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
                if (File.Exists(sFilename))
                {
                    File.Delete(sFilename);
                }

                ZipFile oZip = new ZipFile();
                // oZip.TempFolder = new MemoryFolder();
                oZip.AddDirectoryByName("raw");

#region PasswordProtectIfNeeded
                if (!String.IsNullOrEmpty(sPassword))
                {
                    if (CONFIG.bUseAESForSAZ)
                    {
                        oZip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    }
                    oZip.Password = sPassword;
                }
#endregion PasswordProtectIfNeeded

                oZip.Comment = Fiddler.CONFIG.FiddlerVersionInfo + " " + GetZipLibraryInfo() + " Session Archive. See http://www.fiddler2.com";
                oZip.ZipError += new EventHandler<ZipErrorEventArgs>(oZip_ZipError);
#region ProcessEachSession
                int iFileNumber = 1;
                // Our format string must pad all session ids with leading 0s for proper sorting.
                string sFileNumberFormatter = ("D" + arrSessions.Length.ToString().Length);
               
                foreach (Session oSession in arrSessions)
                {
                    // If you don't make a copy of the session within the delegate's scope,
                    // you get a behavior different than what you'd expect.
                    // See http://blogs.msdn.com/brada/archive/2004/08/03/207164.aspx
                    // and http://blogs.msdn.com/oldnewthing/archive/2006/08/02/686456.aspx
                    // and http://blogs.msdn.com/oldnewthing/archive/2006/08/04/688527.aspx
                    Session delegatesCopyOfSession = oSession;

                    string sBaseFilename = @"raw\" + iFileNumber.ToString(sFileNumberFormatter);
                    string sRequestFilename = sBaseFilename + "_c.txt";
                    string sResponseFilename = sBaseFilename + "_s.txt";
                    string sMetadataFilename = sBaseFilename + "_m.xml";

                    oZip.AddEntry(sRequestFilename, new WriteDelegate(delegate(string sn, Stream strmToWrite)
                        {
                            delegatesCopyOfSession.WriteRequestToStream(false, true, strmToWrite);
                        })
                    );

                    oZip.AddEntry(sResponseFilename,
                        new WriteDelegate(delegate(string sn, Stream strmToWrite)
                        {
                            delegatesCopyOfSession.WriteResponseToStream(strmToWrite, false);
                        })
                    );

                    oZip.AddEntry(sMetadataFilename,
                        new WriteDelegate(delegate(string sn, Stream strmToWrite)
                        {
                            delegatesCopyOfSession.WriteMetadataToStream(strmToWrite);
                        })
                    );

                    iFileNumber++;
                }
#endregion ProcessEachSession
                oZip.Save(sFilename);

                return true;
            }
            catch (Exception eX)
            {
                FiddlerApplication.Log.LogString("WriteSessionArchive failed to save Session Archive. " + eX.Message);
                return false;
            }
        }
        static void oZip_ZipError(object sender, ZipErrorEventArgs e)
        {
            FiddlerApplication.Log.LogFormat("WriteSessionArchive skipped writing {0} to {1} because {2};\n{3}...",
                e.CurrentEntry.FileName,
                e.ArchiveName,
                e.Exception.Message,
                e.Exception.StackTrace);

            e.CurrentEntry.ZipErrorAction = ZipErrorAction.Skip;
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

#endregion

        public void Dispose()
        {
            // nothing to do here.
        }

        /// <summary>
        /// Returns a string indicating the ZipLibrary version information
        /// </summary>
        /// <returns></returns>
        public static string GetZipLibraryInfo()
        {
            return "DotNetZip v" + Ionic.Zip.ZipFile.LibraryVersion.ToString();
        }

#endregion
    }
}