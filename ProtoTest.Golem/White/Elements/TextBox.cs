using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White.Elements
{
    public class TextBox : TestStack.White.UIItems.TextBox
    {
        public BaseItem<TestStack.White.UIItems.TextBox> item;

        public TextBox(string name, SearchCriteria criteria, Window window = null)
        {
            if (window == null) window = WhiteTestBase.window;
            item = new BaseItem<TestStack.White.UIItems.TextBox>(name, criteria, window);
        }
    }
}
