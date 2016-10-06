using System;
using System.Diagnostics;
using System.Linq;

namespace Golem.Core
{
    public class CurrentProcessInfo
    {
        private readonly Type commandInterface;
        private readonly string formatString = "{0}.{1}()_{2}.{3}() : ";
        private readonly Type pageObjectType;
        public string className = "";
        public string commandName = "";
        public string elementName = "";
        public string methodName = "";

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
            var stackFrames = stackTrace.GetFrames();
            foreach (var stackFrame in stackFrames)
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