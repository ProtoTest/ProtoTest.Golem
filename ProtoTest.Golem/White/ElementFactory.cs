using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using ProtoTest.Golem.Core;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;

namespace ProtoTest.Golem.White.Elements
{
    public class ElementFactory
    {
        private static string LogFormatString = "{0}Looking for item in '{1}' with '{2}'";
        public static T GetItem<T>(T item, SearchCriteria criteria, UIItem parent) where T : UIItem
        {
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
                    TestBase.LogEvent(string.Format(LogFormatString, ((IWhiteElement)parent).description, criteria));
                    item = parent.Get<T>(criteria);
                }            
            }
            return item;
        }
    }
}
