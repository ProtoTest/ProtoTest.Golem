using System;
using System.Collections.Generic;
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
                trace += stackFrame.GetMethod().ReflectedType.BaseType.ToString()+"."+stackFrame.GetMethod().Name.ToString() + "\r\n";
                if (stackFrame.GetMethod().ReflectedType.BaseType == typeof(BasePageObject))
                    return stackFrame.GetMethod().ReflectedType.Name.ToString() + "." + stackFrame.GetMethod().Name.ToString();
            }
           // Common.Log(trace);
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
            lock(locker)
            {
                return TestContext.CurrentContext.TestStep.FullName;
            }
   

        }

        public static string GetShortTestName(int length)
        {

            string name = TestContext.CurrentContext.TestStep.FullName;
            if (name.Length > length)
                name = name.Substring(name.Length - length, name.Length);
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
            if (TestBaseClass.testData.numVerificationErrors != 0)
                return TestOutcome.Failed;
            return TestContext.CurrentContext.Outcome;
        }
    }
}
