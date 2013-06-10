using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using System.Diagnostics;

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

    }
}
