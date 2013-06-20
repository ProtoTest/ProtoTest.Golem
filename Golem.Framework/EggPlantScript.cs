using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CookComputing.XmlRpc;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;

namespace Golem.Framework
{
    public class EggPlantScript
    {
        public string suitePath;
        public string scriptName;
        public string host = "";
        public string port = "";
        public int timeout = 600000;
        public string description = "";


        private IEggPlantDriver driver;
        public EggPlantScript(string suitePath, string scriptName, string host, string port)
        {
            this.suitePath = suitePath;
            this.scriptName = scriptName;
            this.host = host;
            this.port = port;
            Init();
        }

        private void Init()
        {
            driver = (IEggPlantDriver)XmlRpcProxyGen.Create(typeof(IEggPlantDriver));
            driver.Timeout = timeout;
            StartSession();
            Connect(host);
            this.description = GetScriptDescription();
        }

        private void StartSession()
        {
            try
            {
                driver.StartSession(suitePath);

            }
            catch (Exception)
            {
            }
        }
        private void EndSession()
        {
            try
            {
                driver.EndSession();

            }
            catch (Exception)
            {
            }
        }

        private void Connect(string host)
        {
            try
            {
                driver.Execute("Connect (name:\"" + host + "\")");

            }
            catch (Exception)
            {
            }
        }
        

        public void ExecuteScript()
        {
            DiagnosticLog.WriteLine("Executing test : " + this.scriptName);
            TestLog.WriteLine(description);
            driver.Execute("RunWithNewResults("+scriptName+")");


        }

        public string GetLogFile()
        {
            return System.IO.File.ReadAllText(getResultDirectory() + "\\LogFile.txt");

        }

        private string getResultDirectory()
        {
            string path = "";
            path += suitePath;
            path += "\\Results\\" + scriptName + "\\";
            string[] directories = Directory.GetDirectories(path);
            string biggest = "0";
            foreach (string dir in directories)
            {
                string folder = dir.Replace(path, "");
                folder = folder.Replace("\\", "");
                //Common.Log("Folder : " + folder);
                if (folder.CompareTo(biggest) > 0)
                    biggest = folder;
            }
            return path + "\\" + biggest;

        }

        public string GetScriptDescription()
        {
            string path = "";
            path += suitePath + "\\Scripts\\" + scriptName + ".script";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line = "";
            string result = "";
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("--"))
                {
                    result += line += "\r\n";
                }
                
            }
            return result;

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
                    TestLog.Failures.EmbedImage(null, ScaleImage(Image.FromFile(getResultDirectory() + "\\Screen_Error.png")));
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

    }
}
