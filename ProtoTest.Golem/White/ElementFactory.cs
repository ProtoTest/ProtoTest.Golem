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
        public static T GetItem<T>(T item, SearchCriteria criteria, UIItem parent) where T : UIItem
        {
        if (item == null || item.IsStale())
            {
                if (parent.GetType() == typeof (WhiteWindow))
                {
                    var window = (WhiteWindow)parent;
                    if (window == null) window = WhiteTestBase.window;
                    TestBase.LogEvent(string.Format("Looking for item in '{0}' with '{1}'", window.description, criteria));
                    item = window.Get<T>(criteria);
                }
                else
                {

                    TestBase.LogEvent(string.Format("Looking for item in '{0}' with '{1}'", parent.Name, criteria));
                    item = parent.Get<T>(criteria);
                }            
            }
            else
            {
                TestBase.LogEvent(string.Format("Item already exists : '{0}'", item.GetType().ToString()));
            }
            
            return item;
        }
    }
}
