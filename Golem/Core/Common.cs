using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using Gallio.Framework;
using Gallio.Model;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Core
{
    public class Common
    {
        private static string lastMessage;
        private static Object locker = new Object();

        public static string GetRandomString()
        {
            return DateTime.Now.ToString("ddHHmmss");
        }

        public static void KillProcess(string name)
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                try
                {
                    if (process.ProcessName == name)
                    {
                        Log("Killing Process : " + name);
                        process.Kill();
                    }
                }
                catch (Exception)
                {
 
                }
                
            }
           
        }

        public static Process ExecuteBatchFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new SilentTestException(TestOutcome.Failed, "Could not find batch file : " + filePath);
            return Process.Start(filePath);
        }

        public static Process ExecuteDosCommand(string command, bool waitToFinish = true)
        {
            DiagnosticLog.WriteLine("Executing DOS Command: " + command);
            string tempGETCMD = null;
            var CMDprocess = new Process();
            var StartInfo = new ProcessStartInfo();
            StartInfo.FileName = "cmd"; //starts cmd window
            StartInfo.Arguments = "/c \"" + command + "\"";
            StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            StartInfo.CreateNoWindow = true;
            StartInfo.RedirectStandardInput = true;
            StartInfo.RedirectStandardOutput = true;
            StartInfo.UseShellExecute = false; //required to redirect
            CMDprocess.StartInfo = StartInfo;
            CMDprocess.Start();
            StreamReader SR = CMDprocess.StandardOutput;
            StreamWriter SW = CMDprocess.StandardInput;
            CMDprocess.Start();
            string line = "";

            while ((line != null) && (waitToFinish))
            {
                line = SR.ReadLine();
                DiagnosticLog.WriteLine(line);
            }

            SW.Close();
            SR.Close();
            return CMDprocess;
        }

        public static void Log(string msg)
        {
            DiagnosticLog.WriteLine(msg);
            TestLog.WriteLine(msg);
        }

        public static bool IsTruthy(string truth)
        {
            switch (truth)
            {
                case "1":
                case "true":
                case "True":
                    return true;
                case "0":
                case "false":
                case "False":
                    return false;
                default:
                    return false;
            }
        }

        public static string GetCurrentTestName()
        {
            return TestContext.CurrentContext.TestStep.FullName;
        }

        public static string GetShortTestName(int length)
        {
            string name = TestContext.CurrentContext.TestStep.Name;
            name = name.Replace("/", "_");
            name = name.Replace(":", "_");
            name = name.Replace("\\", "_");
            name = name.Replace("\"", "");
            name = name.Replace(" ", "");
            if (name.Length > length)
                name = name.Substring((name.Length - length), length);

            return name;
        }



        public string GetValueFromXmlFile(string filepath, string xpath)
        {
            var configFile = new XmlDocument();
            configFile.Load(filepath);
            return configFile.SelectSingleNode(xpath).Value ?? "";
        }

        public string GetConfigValue(string fileName, string xpath)
        {
            var configFile = new XmlDocument();
            configFile.Load(Directory.GetCurrentDirectory() + fileName);
            return configFile.SelectSingleNode(xpath).Value ?? "";
        }

        public static TestOutcome GetTestOutcome()
        {
            if (TestBase.testData.VerificationErrors.Count != 0)
                return TestOutcome.Failed;
            return TestContext.CurrentContext.Outcome;
        }


        public static void ExecuteCommandSync(object command)
        {
            try
            {
                //// create the ProcessStartInfo using "cmd" as the program to be run,
                //// and "/c " as the parameters.
                //// Incidentally, /c tells cmd that we want it to execute the command that follows,
                //// and then exit.
                //System.Diagnostics.ProcessStartInfo procStartInfo =
                //    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                //// The following commands are needed to redirect the standard output.
                //// This means that it will be redirected to the Process.StandardOutput StreamReader.
                //procStartInfo.RedirectStandardOutput = true;
                //procStartInfo.UseShellExecute = false;
                //// Do not create the black window.
                //procStartInfo.CreateNoWindow = true;
                //// Now we create a process, assign its ProcessStartInfo and start it
                //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                //proc.StartInfo = procStartInfo;
                //proc.Start();
                //// Get the output into a string
                //string result = proc.StandardOutput.ReadToEnd();
                //// Display the command output.
                //Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                // Log the exception
            }
        }

        public static Thread ExecuteCommandAsync(string command)
        {
            try
            {
                //Asynchronously start the Thread to process the Execute command request.
                var objThread = new Thread(ExecuteCommandSync)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.AboveNormal
                };

                //Make the thread as background thread.
                //Set the Priority of the thread.
                //Start the thread.
                objThread.Start(command);
                return objThread;
            }
            catch (ThreadStartException objException)
            {
                // Log the exception
            }
            catch (ThreadAbortException objException)
            {
                // Log the exception
            }
            catch (Exception objException)
            {
                // Log the exception
            }
            return null;
        }

        public static Image ScaleImage(Image image, double scale = .5)
        {
            var newWidth = (int) (image.Width*scale);
            var newHeight = (int) (image.Height*scale);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static Image ResizeImage(Image image, int newWidth, int newHeight)
        {
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static void Delay(int delayMs)
        {
            if (delayMs > 0)
            {
                //TestBase.LogEvent(TestBase.GetCurrentClassAndMethodName() + ": Delay : " + delayMs);
                Thread.Sleep(delayMs);
            }
        }

        public static void UpdateConfigFile(string key, string value)
        {
            var doc = new XmlDocument();
            string path = Assembly.GetCallingAssembly().Location + ".config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='" + key + "']").Attributes["value"].Value = value;
            doc.Save(path);

            path = Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                   "\\App.config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='" + key + "']").Attributes["value"].Value = value;
            doc.Save(path);
        }

        /// <summary>
        ///     Create a dummy file with some ASCII
        /// </summary>
        /// <param name="filepath">File path and name to create</param>
        public static void CreateDummyFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                using (FileStream fs = File.Create(filepath))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }

                    fs.Close();
                }
            }
        }

        public static string GetCodeDirectory()
        {
  
            return Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "");
        }

        public static void LogImage(Image image)
        {
            TestLog.EmbedImage(null, image);
        }

        public static Size GetSizeFromResolution(string resolution)
        {
            var dimensions = resolution.Split('x');
            return new Size(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
        }
    }
}