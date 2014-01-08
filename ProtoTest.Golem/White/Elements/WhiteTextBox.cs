using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White.Elements
{
    public class WhiteTextBox : TestStack.White.UIItems.TextBox
    {
        public SearchCriteria criteria;
        public string name;
        public Window window;
        private UIItem _item;
        public UIItem item
        {
            get { return TryGet(); }
        }

        public WhiteTextBox(string name, SearchCriteria criteria, Window window = null)
        {
            if (window == null) this.window = WhiteTestBase.window;
            this.name = name;
            this.criteria = criteria;
        }

        public UIItem TryGet()
        {
            try
            {
                _item = window.Get<WhiteTextBox>(criteria);
                return _item;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
