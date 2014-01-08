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
        public static T GetItem<T>(T item, SearchCriteria criteria, Window window, UIItem parent) where T : UIItem
        {
            if (item == null || item.IsStale())
            {
                string location = "";
                if (parent != null)
                {
                    location = "Parent " + parent.GetType().ToString();
                    TestBase.LogEvent(string.Format("Looking for item in '{0}' with '{1}'", location, criteria));
                    item = parent.Get<T>(criteria);
                }

                else
                {
                    location = "Window " + window.Name;
                    TestBase.LogEvent(string.Format("Looking for item in '{0}' with '{1}'",location, criteria));
                    item = window.Get<T>(criteria);
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
