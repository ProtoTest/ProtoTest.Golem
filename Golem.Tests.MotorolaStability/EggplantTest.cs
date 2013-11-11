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

namespace ProtoTest.TestRunner.Eggplant
{
    public class EggplantTest
    {
        private Process cmdProcess;
        public string suitePath;
        public string scriptName;
        public string host = "";
        public string port = "";
        public int timeoutMin;
        public string description = "";
        public string scriptPath = "";


        private EggplantDriver driver;
        public EggplantTest(EggplantDriver driver, string suitePath, string scriptName, string host, string port, int timeoutMin)
        {

            this.driver = driver;
            this.suitePath = suitePath;
            this.scriptName = scriptName;
            this.host = host;
            this.port = port;
            this.timeoutMin = timeoutMin;
            this.scriptPath += suitePath + "\\Scripts\\" + scriptName + ".script";
            this.description = GetCommentsFromScript();
            VerifyScriptExists();
            driver.Connect(host);
        }

        private void VerifyScriptExists()
        {
            Assert.IsTrue(File.Exists(scriptPath),"Test aborted, could not find file : " + scriptPath);
        }


        public TestOutcome ExecuteTest(string testName)
        {
            Gallio.Common.Action executeTest = new Gallio.Common.Action(delegate
            {
                driver.ExecuteScript(scriptPath,description);
                VerifySuccess();
                AttachTestFiles();
            });


            return TestStep.RunStep(testName, executeTest, new TimeSpan(0, 0, timeoutMin, 0), true, null).Outcome;
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

        private string GetCommentsFromScript()
        {
            if(!File.Exists(scriptPath))
                throw new FileNotFoundException("Could not find script file at path : " + scriptPath);
            string path = scriptPath;
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
                    TestLog.Failures.EmbedImage(null, Common.ScaleImage(Image.FromFile(getResultDirectory() + "\\Screen_Error.png")));
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


       


  

    }
}
