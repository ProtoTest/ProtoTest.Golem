using System;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.White.Elements;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
//using TestStack.White.UIItems.WPFUIItems;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    public class ElementFactory 
    {
        private static string LogFormatString = "{0}.{1}() : in '{4}' with '{5}' : {2}.{3}()";
        public static T GetItem<T>(T item, SearchCriteria criteria, UIItem parent, UIItemContainer parentContainer = null) where T : UIItem
        {
            try
            {
                string description = parent.GetType() == typeof(WhiteWindow) ? ((WhiteWindow)parent).description : ((IWhiteElement)parent).description;
                var procInfo = new CurrentProcessInfo(typeof(BaseScreenObject), typeof(IWhiteElement));
                string logInfo = string.Format(LogFormatString, procInfo.className, procInfo.methodName, procInfo.elementName, procInfo.commandName, description, criteria);
                TestBase.LogEvent(logInfo);
                if (item != null && !item.IsStale()) return item;
                //item = parent.Get<T>(criteria);
                if (parentContainer == null)
                {
                    parentContainer = WhiteTestBase.window;
                }
                item = parentContainer.Get<T>(criteria);
                return item;
            }
            catch (Exception)
            {
                return null;
            }
      
        }

        public static UIItem GetItem(UIItem item, SearchCriteria criteria, UIItem parent, UIItemContainer parentContainer = null)
        {
            try
            {
                string description = parent.GetType() == typeof(WhiteWindow) ? ((WhiteWindow)parent).description : ((IWhiteElement)parent).description;
                var procInfo = new CurrentProcessInfo(typeof(BaseScreenObject), typeof(IWhiteElement));
                string logInfo = string.Format(LogFormatString, procInfo.className, procInfo.methodName, procInfo.elementName, procInfo.commandName, description, criteria);
                TestBase.LogEvent(logInfo);
                if (item != null && !item.IsStale()) return item;
                //item =  (UIItem) parent.Get(criteria);if (parentContainer == null)
                if (parentContainer == null)
                {
                    parentContainer = WhiteTestBase.window;
                }
                item = (UIItem) parentContainer.Get(criteria);
                return item;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
