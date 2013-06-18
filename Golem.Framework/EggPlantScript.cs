using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;

namespace Golem.Framework
{
    public class EggPlantScript
    {

        public EggPlantScript(string runScriptPath, string suitePath, string scriptName, string host, string port)
        {

            this.runScriptPath = runScriptPath;
            this.suitePath = suitePath;
            this.scriptName = scriptName;
            this.host = host;
            this.port = port;
        }

        public string suitePath;
        public string scriptName;
        public string globalResultsFolder = Directory.GetCurrentDirectory();
        public string[] parameters;
        public bool commandLineOutput = true;
        public string host = "";
        public string colorDepth;
        public string password;
        public string port = "";
        public string username;
        public int repeat = 0;
        public string defaultDocDirectory;
        public string runScriptPath;
        public string output;
        public bool reportFailures = true;

        public void Execute()
        {
            //Create process
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

            //strCommand is path and file name of command to run
            pProcess.StartInfo.FileName = "cmd.exe";

            //strCommandParameters are parameters to pass to program
            pProcess.StartInfo.Arguments = GetCommandScript();

            pProcess.StartInfo.UseShellExecute = false;

            //Set output of program to be written to process output stream
            pProcess.StartInfo.RedirectStandardOutput = true;

            //Start the process
            pProcess.Start();

            //Get program output
            output = pProcess.StandardOutput.ReadToEnd();

            //Wait for process to finish
            pProcess.WaitForExit();

            pProcess.Close();

        }

        public void ExecuteScript()
        {
            DiagnosticLog.WriteLine("Executing test : " + this.scriptName);
            string tempGETCMD = null;
            Process CMDprocess = new Process();
            System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
            StartInfo.FileName = "cmd"; //starts cmd window
            StartInfo.Arguments = "/c \"" + GetCommandScript() + "\"";
            StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            StartInfo.CreateNoWindow = true;
            StartInfo.RedirectStandardInput = true;
            StartInfo.RedirectStandardOutput = true;
            StartInfo.UseShellExecute = false; //required to redirect
            CMDprocess.StartInfo = StartInfo;
            CMDprocess.Start();
            System.IO.StreamReader SR = CMDprocess.StandardOutput;
            System.IO.StreamWriter SW = CMDprocess.StandardInput;
            CMDprocess.Start();
            DiagnosticLog.WriteLine("Executing Command : " + GetCommandScript());
            //SW.WriteLine(GetCommandScript());
            output = SR.ReadToEnd(); //returns results of the command window
            DiagnosticLog.WriteLine(output);
            // SW.WriteLine("exit"); //exits command prompt window
            SW.Close();
            SR.Close();
            DiagnosticLog.WriteLine("Finished test : " + this.scriptName);
        }

        public string GetLogFile()
        {
            return System.IO.File.ReadAllText(getResultDirectory() + "\\LogFile.txt");

        }

        public string GetCommandScript()
        {
            string command = "";
            command += "\"" + runScriptPath + "\" ";
            command += "\"" + suitePath + scriptName + "\" ";
            // command += (repeat != 0 ? "-repeat " + repeat + " " : "");
            command += (host != "" ? "-host " + host + " " : "");
            command += (port != "" ? "-port " + port + " " : "");
            command += (reportFailures == true ? "-ReportFailures YES " : "-ReportFailures NO ");
            command += (username != null ? "-username " + username + " " : "");
            command += (password != null ? "-password " + password + " " : "");
            // command += (commandLineOutput == true ? "-CommandLineOutput YES " : "-CommandLineOutput NO ");
            command += (globalResultsFolder != null ? "-GlobalResultsFolder \"" + globalResultsFolder + "\" " : "");
            return command;
        }

        private string getResultDirectory()
        {
            string path = "";
            path += globalResultsFolder;
            path += "\\" + scriptName + "\\";
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
                    TestLog.Failures.EmbedImage(null, Image.FromFile(getResultDirectory() + "\\Screen_Error.png"));
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
