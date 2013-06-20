using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CookComputing.XmlRpc;
using Gallio.Common.Markup;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;

namespace Golem.Framework
{
    public class EggPlantScript
    {
        private string suitePath;
        private string scriptName;
        private string host;
        private string port;
        private int timeout;
        private IEggPlantDriver rpcClient;
        private string description = "";

        public EggPlantScript(string suitePath, string scriptName, string host, string port, int timeout)
        {
            this.suitePath = suitePath;
            this.scriptName = scriptName;
            this.host = host;
            this.port = port;
            this.timeout = timeout * 60000;
            Init();
        }

        private void Init()
        {
            rpcClient = XmlRpcProxyGen.Create<IEggPlantDriver>();
            rpcClient.Timeout = timeout;
            GetScriptDescription();
            StartSession();
            Connect();
        }

        private void GetScriptDescription()
        {
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(GetScriptFilePath());
                string line = "";
                    while (line != null)
                    {
                        line = file.ReadLine();
                        if (line.Contains("--"))
                            this.description += line += "\r\n";
                    }
            }
            catch (Exception)
            {

            }
        }

        private void Connect()
        {
            try
            {
                rpcClient.Execute("Connect {name:\"" + host + "\"}");
            }
            catch (Exception e)
            {
               
            }
           
        }


        public void StartSession()
        {
            try
            {
                rpcClient.StartSession(suitePath);
            }
            catch (Exception e)
            {
               
            }
        }

        public void EndSession()
        {
            try
            {
                rpcClient.EndSession();
            }
            catch (Exception e)
            {
                Common.Log("Exception caught " + e.Message);
            }
            
        }

        public void ExecuteCommand(string command)
        {
            rpcClient.Execute(command);
        }

        public void ExecuteScript()
        {
            try
            {
                Common.Log(this.description);
                rpcClient.Execute("RunWithNewResults (" + scriptName + ")");
                
            }
            catch (Exception e)
            {
                Common.Log("Exception caught " + e.Message);
            }
            
        }

        public string GetLogFile()
        {
            return System.IO.File.ReadAllText(getResultDirectory() + "\\LogFile.txt");

        }
        public string GetLogXMLFile()
        {
            return System.IO.File.ReadAllText(getResultDirectory() + "\\LogFile.xml");

        }
        public string GetResultFile()
        {
            string failurePath = getResultDirectory() + "\\.failure";
            string successPath = getResultDirectory() + "\\.success";
            if (File.Exists(failurePath))
            {
                return failurePath;
            }
            if (File.Exists(successPath))
            {
                return successPath;
            }
            else
            {
                return "";
            }

        }

        private string getResultDirectory()
        {
            string path = "";
            path += suitePath + "\\Results" + "\\" + scriptName + "\\";
            string[] directories = Directory.GetDirectories(path);
            string biggest = "0";
            foreach (string dir in directories)
            {
                string folder = dir.Replace(path, "");
                folder = folder.Replace("\\", "");
               if (folder.CompareTo(biggest) > 0)
                    biggest = folder;
            }
            return path + "\\" + biggest;

        }

        public void AttachResultsFilesToLog()
        {
            TestLog.Attach(new BinaryAttachment("Log_File_Text_" + Common.GetCurrentTestName() + ".text", "application/text ", File.ReadAllBytes(GetLogFile())));
            TestLog.Attach(new BinaryAttachment("Log_File_XML_" + Common.GetCurrentTestName() + ".text", "application/xml ", File.ReadAllBytes(GetLogXMLFile())));
            TestLog.Attach(new BinaryAttachment("Results_" + Common.GetCurrentTestName(), "application/text ", File.ReadAllBytes(GetResultFile())));
        }

        public void VerifySuccess()
        {
            DiagnosticLog.WriteLine("Verifying Test : " + this.scriptName);
            XmlDocument resultsFile = new XmlDocument();
            string file = getResultDirectory() + "\\LogFile.xml";
            DiagnosticLog.WriteLine("Checking results file : " + file);
            resultsFile.Load(file);
            var result = resultsFile.SelectSingleNode("//property[@name='Status']/@value").Value;
            if (result != "Success")
            {
                DiagnosticLog.WriteLine("Test Failure Detected : " + this.scriptName);
                TestContext.CurrentContext.IncrementAssertCount();
                TestLog.Failures.BeginSection("EggPlant Error");
                TestLog.Failures.WriteLine(GetFailureMessage());
                if (File.Exists(getResultDirectory() + "\\Screen_Error.png"))
                {
                    TestLog.Failures.EmbedImage(null, Common.ResizeImage(Image.FromFile(getResultDirectory() + "\\Screen_Error.png"),1/2));
                }
                TestLog.Failures.End();
                Assert.TerminateSilently(TestOutcome.Failed);
            }
        }

        public string GetFailureMessage()
        {
            string file = getResultDirectory() + "\\LogFile.txt";
            DiagnosticLog.WriteLine("Getting failure reason from file : " + file);
            string line;
            string previous = "";
            string errorMessage = "";
            using (StreamReader reader = new StreamReader(file))
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("Error"))
                    {
                        StringBuilder error = new StringBuilder();
                        error.AppendLine(previous);
                        error.AppendLine(line);
                        error.AppendLine(reader.ReadToEnd());
                        return error.ToString();
                    }
                    else
                    {
                        previous = line;
                        line = reader.ReadLine();
                    }

                }

                return "";
            }


        }


        private static Image ScaleImage(Image image, double scale = .5)
        {
            var newWidth = (int)(image.Width * scale);
            var newHeight = (int)(image.Height * scale);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        private string GetScriptFilePath()
        {
            string path = "";
            path += suitePath + "\\Scripts\\" + scriptName + ".script";
            return path;

        }

    }
}
