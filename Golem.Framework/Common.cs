using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public static void ExecuteDosCommand(string command)
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
            //SW.WriteLine(GetCommandScript());
            string output = SR.ReadToEnd(); //returns results of the command window
            DiagnosticLog.WriteLine(output);
            // SW.WriteLine("exit"); //exits command prompt window
            SW.Close();
            SR.Close();
            DiagnosticLog.WriteLine("Finished executing DOS Command");
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
            string trace = "";
            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                if ((stackFrame.GetMethod().ReflectedType.BaseType == typeof(BasePageObject)) && (!stackFrame.GetMethod().IsConstructor))
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

        public static Image ResizeImage(Image imgToResize, float percent)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            int destWidth = (int)(sourceWidth * percent);
            int destHeight = (int)(sourceHeight * percent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}
