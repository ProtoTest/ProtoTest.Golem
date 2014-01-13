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
    public class WhiteComboBox : ComboBox, IWhiteElement
    {
        public string description { get; set; }
        public SearchCriteria criteria { get; set; }
        public UIItem parent { get; set; }
        private ComboBox _item;
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
        public UIItem getItem()
        {
            return item;
        }
        public ElementVerification Verify(int timeout = 0)
        {
            return new ElementVerification(this, timeout, false, true);
        }

        public ElementVerification WaitUntil(int timeout = 0)
        {
            return new ElementVerification(this, timeout, true, true);
        }

        public WhiteComboBox(SearchCriteria criteria, UIItem parent = null, string description = null)
        {
            this.description = description ?? criteria.ToString();
            this.criteria = criteria;
            this.parent = parent ?? WhiteTestBase.window;
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
