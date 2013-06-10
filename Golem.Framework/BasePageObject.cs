using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Diagnostics;


namespace Golem.Framework
{
    public abstract class BasePageObject 
    {
        private IWebDriver _driver;

        protected IWebDriver driver
        {
            get
            {
                return _driver;
            }
            set
            {
                _driver = value;
            }

        }
        public string url;

        public BasePageObject()
        {
            //TestBaseClass.testData.FireEvent(GetCurrentClassAndMethodName() + " Started");
            this.driver = TestBaseClass.driver;
            //string className = GetCurrentClassName();
           // string name = GetCurrentMethodName();
            WaitForElements();
            //TestBaseClass.testData.FireEvent(name + " Finished");
        }
        public abstract void WaitForElements();
        private static string GetCurrentClassAndMethodName()
        {
            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)
            string trace = "";
            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                trace += stackFrame.GetMethod().ReflectedType.BaseType.ToString() + "." + stackFrame.GetMethod().Name.ToString() + "." + stackFrame.GetMethod().DeclaringType.ToString()  + "\r\n";
                if (stackFrame.GetMethod().ReflectedType.BaseType == typeof(BasePageObject))
                    return stackFrame.GetMethod().ReflectedType.Name.ToString() + "." + stackFrame.GetMethod().Name.ToString();
            }
            //Common.Log(trace);
            return "";

        }

        private static string GetCurrentMethodName()
        {
            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                if (stackFrame.GetMethod().ReflectedType.BaseType == typeof(BasePageObject))
                    return stackFrame.GetMethod().Name.ToString();
            }
            return "";

        }
    }
}
