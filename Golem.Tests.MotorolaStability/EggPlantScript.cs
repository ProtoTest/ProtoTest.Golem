using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Xml;
using Gallio.Common.Markup;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using System.Threading;

namespace Golem.Framework
{
    public class EggPlantScript
    {
        private Process cmdProcess;
        public string suitePath;
        public string scriptName;
        public string host = "";
        public string port = "";
        public int timeout = 600000;
        public string description = "";


        private IEggPlantDriver driver;
        public EggPlantScript(IEggPlantDriver driver, string suitePath, string scriptName, string host, string port)
        {
            this.driver = driver;
            this.suitePath = suitePath;
            this.scriptName = scriptName;
            this.host = host;
            this.port = port;
            this.description = GetScriptDescription();
            Connect(host);
        }

        private void Connect(string host)
        {
            try
            {
                driver.Execute("Connect (name:\"" + host + "\")");

            }
            catch (Exception e)
            {
                DiagnosticLog.WriteLine("Error caught connecting to host : " + e.Message);
            }
        }
        

        private void ExecuteScript()
        {
            DiagnosticLog.WriteLine("Executing test : " + this.scriptName);
            TestLog.WriteLine(description);
            driver.Execute("RunWithNewResults("+scriptName+")");
        }

        public TestOutcome ExecuteTest(string testName)
        {

            Gallio.Common.Action executeTest = new Gallio.Common.Action(delegate
            {

                ExecuteScript();
                VerifySuccess();
                AttachTestFiles();
            });


            return TestStep.RunStep(testName, executeTest, new TimeSpan(0, 0, 10, 0), true, null).Outcome;
        }

        private void AttachTestFiles()
        {
            TestLog.Attach(new TextAttachment("LogFile.txt", "text", GetLogFile()));
            TestLog.Attach(new TextAttachment("LogFile.xml", "text", GetLogXmlFile()));
        }

        private string GetLogFile()
        {
            return System.IO.File.ReadAllText(getResultDirectory() + "\\LogFile.txt");

        }

        private string GetLogXmlFile()
        {
            return System.IO.File.ReadAllText(getResultDirectory() + "\\LogFile.xml");

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
                if (folder.CompareTo(biggest) > 0)
                    biggest = folder;
            }
            return path + "\\" + biggest;

        }

        private string GetScriptDescription()
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

        private void VerifySuccess()
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
