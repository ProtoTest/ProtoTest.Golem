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

            if (item != null && !item.IsStale()) return item;
            string description = parent.GetType() == typeof(WhiteWindow) ? ((WhiteWindow)parent).description : ((IWhiteElement)parent).description;
            TestBase.LogEvent(string.Format(LogFormatString, WhiteTestBase.GetCurrentClassAndMethodName(), description, criteria));
            item = parent.Get<T>(criteria);
            return item;
        }

    }
}
