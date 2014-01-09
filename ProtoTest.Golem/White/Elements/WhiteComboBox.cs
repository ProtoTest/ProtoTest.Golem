using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White;
using TestStack.White.Recording;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.Scrolling;

namespace ProtoTest.Golem.White.Elements
{
    public class WhiteComboBox : ComboBox
    {
        public string description;
        public SearchCriteria criteria;
        private ComboBox _item;
        private UIItem parent;
        public ComboBox item
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

        public WhiteComboBox(SearchCriteria Criteria, UIItem Parent = null, string Description = null)
        {
            this.description = Description ?? Criteria.ToString();
            this.criteria = Criteria;
            this.parent = Parent ?? WhiteTestBase.window;
        }

        public override ListItem Item(string itemText)
        {
            return item.Item(itemText);
        }

        public override ListItem Item(int index)
        {
            return item.Item(index);
        }

        public override void Select(string itemText)
        {
            item.Select(itemText);
        }

        public override void Select(int index)
        {
            item.Select(index);
        }

        public override string SelectedItemText
        {
            get { return item.SelectedItemText; }
        }

        public override ListItem SelectedItem
        {
            get { return item.SelectedItem; }
        }

        public override void SetValue(object value)
        {
            item.SetValue(value);
        }

        public override void ActionPerforming(UIItem uiItem)
        {
            item.ActionPerforming(uiItem);
        }

        public override ListItems Items
        {
            get { return item.Items; }
        }

        public override void HookEvents(UIItemEventListener eventListener)
        {
            item.HookEvents(eventListener);
        }

        public override void UnHookEvents()
        {
            item.UnHookEvents();
        }

        public override VerticalSpan VerticalSpan
        {
            get { return item.VerticalSpan; }
        }

        public override IScrollBars ScrollBars
        {
            get { return item.ScrollBars; }
        }

        public override string EditableText
        {
            get
            {
                return item.EditableText;
            }
            set
            {
                item.EditableText = value;
            }
        }

     

        public override bool IsEditable
        {
            get { return item.IsEditable; }
        }
    }
}
