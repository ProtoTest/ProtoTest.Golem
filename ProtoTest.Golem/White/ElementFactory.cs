using ProtoTest.Golem.Core;
using ProtoTest.Golem.White.Elements;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;

namespace ProtoTest.Golem.White
{
    public class ElementFactory 
    {
        private static string LogFormatString = "{0}Looking for item in '{1}' with '{2}'";
        public static T GetItem<T>(T item, SearchCriteria criteria, UIItem parent) where T : UIItem
        {
<<<<<<< HEAD

            if (item != null && !item.IsStale()) return item;
            string description = parent.GetType() == typeof(WhiteWindow) ? ((WhiteWindow)parent).description : ((IWhiteElement)parent).description;
            TestBase.LogEvent(string.Format(LogFormatString, WhiteTestBase.GetCurrentClassAndMethodName(), description, criteria));
            item = parent.Get<T>(criteria);
=======
            if (item == null || item.IsStale())
            {
                if (parent.GetType() == typeof (WhiteWindow))
                {
                    var window = (WhiteWindow)parent;
                    if (window == null) window = WhiteTestBase.window;
                    TestBase.LogEvent(string.Format(LogFormatString, WhiteTestBase.GetCurrentClassAndMethodName(), window.description, criteria));
                    item = window.Get<T>(criteria);
                }
                else
                {
                    TestBase.LogEvent(string.Format(LogFormatString, WhiteTestBase.GetCurrentClassAndMethodName(), ((IWhiteElement)parent).description, criteria));
                    item = parent.Get<T>(criteria);
                }            
            }
>>>>>>> d816fa3d4af27c6afe54189172cd0018c21756fb
            return item;
        }

    }
}
