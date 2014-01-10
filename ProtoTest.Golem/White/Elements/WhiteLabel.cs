using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Recording;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.Scrolling;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using Action = TestStack.White.UIItems.Actions.Action;
using Point = System.Windows.Point;

namespace ProtoTest.Golem.White.Elements
{
    public class WhiteLabel : Label, IWhiteElement
    {
        public string description { get; set; }
        public SearchCriteria criteria { get; set; }
        public UIItem parent { get; set; }
        private Label _item;
        public Label item
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

        public WhiteLabel(SearchCriteria criteria, UIItem parent = null, string description=null)
        {
            this.description = description ?? criteria.ToString();
            this.criteria = criteria;
            this.parent = parent ?? WhiteTestBase.window;
        }

        public override string Text
        {
            get { return item.Text; }
        }

        public override string AccessKey
        {
            get { return item.AccessKey; }
        }

        public override ActionListener ActionListener
        {
            get { return item.ActionListener; }
        }

        public override void ActionPerformed(Action action)
        {
            item.ActionPerformed(action);
        }

        public override void ActionPerforming(UIItem uiItem)
        {
            item.ActionPerforming(uiItem);
        }

        public override AutomationElement AutomationElement
        {
            get { return item.AutomationElement; }
        }

        public override Color BorderColor
        {
            get { return item.BorderColor; }
        }

        public override Rect Bounds
        {
            get { return item.Bounds; }
        }

        public override void Click()
        {
            item.Click();
        }

        public override Point ClickablePoint
        {
            get { return item.ClickablePoint; }
        }

        public override void DoubleClick()
        {
            item.DoubleClick();
        }

        public override bool Enabled
        {
            get { return item.Enabled; }
        }

        public override void Enter(string value)
        {
            item.Enter(value);
        }

        public override bool Equals(object obj)
        {
            return item.Equals(obj);
        }

        public override string ErrorProviderMessage(Window window)
        {
            return item.ErrorProviderMessage(window);
        }

        public override void Focus()
        {
            item.Focus();
        }

        public override WindowsFramework Framework
        {
            get { return item.Framework; }
        }

        public override AutomationElement GetElement(SearchCriteria searchCriteria)
        {
            return item.GetElement(searchCriteria);
        }

        public override int GetHashCode()
        {
            return item.GetHashCode();
        }

        public override string HelpText
        {
            get { return item.HelpText; }
        }

        public override void HookEvents(UIItemEventListener eventListener)
        {
            item.HookEvents(eventListener);
        }

        public override string Id
        {
            get { return item.Id; }
        }

        public override bool IsFocussed
        {
            get { return item.IsFocussed; }
        }

        public override bool IsOffScreen
        {
            get { return item.IsOffScreen; }
        }

        public override void KeyIn(KeyboardInput.SpecialKeys key)
        {
            item.KeyIn(key);
        }

        public override Point Location
        {
            get { return item.Location; }
        }

        public override void LogStructure()
        {
            item.LogStructure();
        }

        public override string Name
        {
            get { return item.Name; }
        }

        public override bool NameMatches(string text)
        {
            return item.NameMatches(text);
        }

        public override string PrimaryIdentification
        {
            get { return item.PrimaryIdentification; }
        }

        public override void RaiseClickEvent()
        {
            item.RaiseClickEvent();
        }

        public override void RightClick()
        {
            item.RightClick();
        }

        public override void RightClickAt(Point point)
        {
            item.RightClickAt(point);
        }

        public override IScrollBars ScrollBars
        {
            get { return item.ScrollBars; }
        }

        public override void SetValue(object value)
        {
            item.SetValue(value);
        }

        public override string ToString()
        {
            return item.ToString();
        }

        public override void UnHookEvents()
        {
            item.UnHookEvents();
        }

        public override bool ValueOfEquals(AutomationProperty property, object compareTo)
        {
            return item.ValueOfEquals(property, compareTo);
        }

        public override bool Visible
        {
            get { return item.Visible; }
        }

        public override Bitmap VisibleImage
        {
            get { return item.VisibleImage; }
        }

        public override void Visit(WindowControlVisitor windowControlVisitor)
        {
            item.Visit(windowControlVisitor);
        }



    }
}
