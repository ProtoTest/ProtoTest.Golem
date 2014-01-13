using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ProtoTest.Golem.Proxy.HAR;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.White;
using ProtoTest.Golem.White.Elements;
using RestSharp.Extensions;

namespace ProtoTest.Golem.Core
{
    public class CurrentProcessInfo
    {
        private string formatString = "{0}.{1}()_{2}.{3}() : ";
        public string className = "";
        public string methodName = "";
        public string commandName = "";
        public string elementName = "";
        private Type pageObjectType;
        private Type commandInterface;

        public CurrentProcessInfo(Type pageObjectType, Type commandInterface)
        {
            this.pageObjectType = pageObjectType;
            this.commandInterface = commandInterface;
            Init();
        }

        public string GetString()
        {
            return string.Format(formatString, className, methodName, elementName, commandName);
        }


        private void Init()
        {
            var stackTrace = new StackTrace(); 
            StackFrame[] stackFrames = stackTrace.GetFrames(); 
            foreach (StackFrame stackFrame in stackFrames)
            {
                var method = stackFrame.GetMethod();
                var type = stackFrame.GetMethod().ReflectedType;
                if ((type.BaseType == pageObjectType) && (!stackFrame.GetMethod().IsConstructor))
                {
                    className = type.Name;
                    methodName = stackFrame.GetMethod().Name;
                }
                if (type.GetInterfaces().Contains(commandInterface))
                {
                    commandName = stackFrame.GetMethod().Name;
                    elementName = type.Name;
                }
            }
        }
    }
}
