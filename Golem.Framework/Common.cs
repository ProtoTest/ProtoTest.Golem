using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using Gallio.Framework;
using System.Diagnostics;
using Gallio.Model;

namespace Golem.Framework
{
    public class Common
    {
        public static string GetRandomString()
        {
            return DateTime.Now.ToString("ddHHmmss");
        }

        public static void KillProcess(string name)
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                if (process.ProcessName == name)
                {
                    Common.Log("Killing Process : "+ name);
                    process.Kill();
                }
            }
        }

        public static Process ExecuteBatchFile(string filePath)
        {
            if(!File.Exists(filePath))
                    throw new SilentTestException(TestOutcome.Failed,"Could not find batch file : " + filePath);
            return System.Diagnostics.Process.Start(filePath);
        }
        public static Process ExecuteDosCommand(string command, bool waitToFinish=true)
        {
            DiagnosticLog.WriteLine("Executing DOS Command: " + command);
            string tempGETCMD = null;
            Process CMDprocess = new Process();
            System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
            StartInfo.FileName = "cmd"; //starts cmd window
            StartInfo.Arguments = "/c \"" + command + "\"";
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
            string line = "";
            
            while((line!=null)&&(waitToFinish))
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

        public static string GetCurrentMethodName()
        {
            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                if (stackFrame.GetMethod().ReflectedType.BaseType==typeof(BasePageObject))
                    return stackFrame.GetMethod().Name.ToString();
            }
            return "";
         
        }
        public static string GetCurrentClassAndMethodName()
        {
            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)
           // string trace = "";
            foreach (StackFrame stackFrame in stackFrames)
            {
               // trace += stackFrame.GetMethod().ReflectedType.FullName;
                if ((stackFrame.GetMethod().ReflectedType.FullName.Contains("PageObject")) && (!stackFrame.GetMethod().IsConstructor))
                {
                     string name = stackFrame.GetMethod().ReflectedType.Name + "." + stackFrame.GetMethod().Name;
                    return name;
                }
                   
            }
           // DiagnosticLog.WriteLine(stackTrace.ToString());
            return "";

        }

        public static bool IsTruthy(string truth)
        {
            switch (truth)
            {
                case "1":
                case "true":
                case "True" :
                    return true;
                case "0":
                case "false":
                case "False":
                    return false;
                default:
                    return false;

            }

        }

        private static Object locker = new Object();
        public static string GetCurrentTestName()
        {
             return TestContext.CurrentContext.TestStep.FullName;
        }

        public static string GetShortTestName(int length)
        {
            string name = TestContext.CurrentContext.TestStep.Name; 
            name = name.Replace("/", "_");
            name = name.Replace("\\", "_");
            name = name.Replace("\"", "");
            name = name.Replace(" ", "");
            if (name.Length > length)
                name = name.Substring((name.Length - length), length);

            return name;

        }

        public static string GetCallStack()
        {
           StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)
            string name = "";
            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                name += stackFrame.GetMethod().Name.ToString() + ".";
            }
            return name;
        }

        public string GetValueFromXmlFile(string filepath, string xpath)
        {
            XmlDocument configFile = new XmlDocument();
            configFile.Load(filepath);
            return configFile.SelectSingleNode(xpath).Value ?? "";
        }

        public string GetConfigValue(string fileName, string xpath)
        {
            XmlDocument configFile = new XmlDocument();
            configFile.Load(Directory.GetCurrentDirectory() + fileName);
            return configFile.SelectSingleNode(xpath).Value ?? ""; 
        }

        public static TestOutcome GetTestOutcome()
        {
            if (TestBaseClass.testData.VerificationErrors.Count != 0)
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
                var objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync))
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

        private static Image ScaleImage(Image image, double scale = .5)
        {
            var newWidth = (int)(image.Width * scale);
            var newHeight = (int)(image.Height * scale);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static void Delay(int delayMs)
        {
            if (delayMs > 0)
            {
                TestBaseClass.LogEvent(Common.GetCurrentClassAndMethodName() + ": Delay : " + delayMs);
                Thread.Sleep(delayMs); 
            }
            
        }

        public static void UpdateConfigFile(string key, string value)
        {
            XmlDocument doc = new XmlDocument();
            string path = System.Reflection.Assembly.GetCallingAssembly().Location + ".config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='"+key+"']").Attributes["value"].Value = value;
            doc.Save(path);

            path = Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                   "\\App.config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='" + key + "']").Attributes["value"].Value = value;
            doc.Save(path);

        }

    }
}
