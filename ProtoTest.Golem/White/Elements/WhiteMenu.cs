using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;

namespace ProtoTest.Golem.White.Elements
{
   public class WhiteMenu : Menu
    {
        public string description;
        public SearchCriteria criteria;
        private Menu _item;
        private UIItem parent;

        public Menu item
        {
            get
            {
                _item = ElementFactory.GetItem(_item, criteria, parent);
                return _item;
            }
            set
            {
                _item = value;
            }
        }

        public WhiteMenu(SearchCriteria Criteria, string Description=null , UIItem Parent=null)
        {
            this.description = Description ?? Criteria.ToString();
            this.criteria = Criteria;
            this.parent = Parent ?? WhiteTestBase.window;
        }
    }
}
