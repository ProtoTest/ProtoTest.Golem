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
using TestStack.White.UIItems.Custom;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.Scrolling;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using Action = TestStack.White.UIItems.Actions.Action;
using Point = System.Windows.Point;

namespace ProtoTest.Golem.White.Elements
{
    public class WhiteCustom : UIItem, IWhiteElement
    {
        public string description { get; set; }
        public SearchCriteria criteria { get; set; }
        public UIItem parent { get; set; }
        private UIItem _item;
        public UIItem item
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

        public WhiteCustom(SearchCriteria criteria, UIItem parent = null, string description = null)
        {
            this.description = description ?? criteria.ToString();
            this.criteria = criteria;
            this.parent = parent ?? WhiteTestBase.window;
        }
        public virtual string Value
        {
            get
            {
                if (this.AutomationElement.Current.IsPassword)
                    throw new WhiteException("Text cannot be retrieved from textbox which has secret text (e.g. password) stored in it");
                ValuePattern valuePattern = this.Pattern(ValuePattern.Pattern) as ValuePattern;
                if (valuePattern != null)
                    return valuePattern.Current.Value;
                TextPattern textPattern = this.Pattern(TextPattern.Pattern) as TextPattern;
                if (textPattern != null)
                    return textPattern.DocumentRange.GetText(int.MaxValue);
                else
                    throw new WhiteException(string.Format("AutomationElement for {0} supports neither ValuePattern or TextPattern", (object)this.ToString()));
            }
            set
            {
                this.Enter(value);
            }
        }


        public override bool ValueOfEquals(AutomationProperty property, object compareTo)
        {
            return item.ValueOfEquals(property, compareTo);
        }

        public override void RightClickAt(Point point)
        {
            item.RightClickAt(point);
        }

        public override void RightClick()
        {
            item.RightClick();
        }

        public override void Focus()
        {
            item.Focus();
        }

        public override void Visit(WindowControlVisitor windowControlVisitor)
        {
            item.Visit(windowControlVisitor);
        }

        public override string ErrorProviderMessage(Window window)
        {
            return item.ErrorProviderMessage(window);
        }

        public override bool NameMatches(string text)
        {
            return item.NameMatches(text);
        }

        public override void Click()
        {
            item.Click();
        }

        public override void DoubleClick()
        {
            item.DoubleClick();
        }

        public override void KeyIn(KeyboardInput.SpecialKeys key)
        {
            item.KeyIn(key);
        }

        public override bool Equals(object obj)
        {
            return item.Equals(obj);
        }

        public override int GetHashCode()
        {
            return item.GetHashCode();
        }

        public override string ToString()
        {
            return item.ToString();
        }

        public override void UnHookEvents()
        {
            item.UnHookEvents();
        }

        public override void HookEvents(UIItemEventListener eventListener)
        {
            item.HookEvents(eventListener);
        }

        public override void SetValue(object value)
        {
            item.SetValue(value);
        }

        public override void ActionPerforming(UIItem uiItem)
        {
            item.ActionPerforming(uiItem);
        }

        public override void ActionPerformed(Action action)
        {
            item.ActionPerformed(action);
        }

        public override void LogStructure()
        {
            item.LogStructure();
        }

        public override AutomationElement GetElement(SearchCriteria searchCriteria)
        {
            return item.GetElement(searchCriteria);
        }

        public override void Enter(string value)
        {
            item.Enter(value);
        }

        public override void RaiseClickEvent()
        {
            item.RaiseClickEvent();
        }

        public override AutomationElement AutomationElement
        {
            get { return item.AutomationElement; }
        }

        public override bool Enabled
        {
            get { return item.Enabled; }
        }

        public override WindowsFramework Framework
        {
            get { return item.Framework; }
        }

        public override Point Location
        {
            get { return item.Location; }
        }

        public override Rect Bounds
        {
            get { return item.Bounds; }
        }

        public override string Name
        {
            get { return item.Name; }
        }

        public override Point ClickablePoint
        {
            get { return item.ClickablePoint; }
        }

        public override string AccessKey
        {
            get { return item.AccessKey; }
        }

        public override string Id
        {
            get { return item.Id; }
        }

        public override bool Visible
        {
            get { return item.Visible; }
        }

        public override string PrimaryIdentification
        {
            get { return item.PrimaryIdentification; }
        }

        public override ActionListener ActionListener
        {
            get { return item.ActionListener; }
        }

        public override IScrollBars ScrollBars
        {
            get { return item.ScrollBars; }
        }

        public override bool IsOffScreen
        {
            get { return item.IsOffScreen; }
        }

        public override bool IsFocussed
        {
            get { return item.IsFocussed; }
        }

        public override Color BorderColor
        {
            get { return item.BorderColor; }
        }

        public override Bitmap VisibleImage
        {
            get { return item.VisibleImage; }
        }

        public override string HelpText
        {
            get { return item.HelpText; }
        }

        string IWhiteElement.description
        {
            get { return description; }
            set { description = value; }
        }

        SearchCriteria IWhiteElement.criteria
        {
            get { return criteria; }
            set { criteria = value; }
        }

        UIItem IWhiteElement.parent
        {
            get { return parent; }
            set { parent = value; }
        }

        UIItem IWhiteElement.getItem()
        {
            return getItem();
        }

        ElementVerification IWhiteElement.Verify(int timeout)
        {
            return Verify(timeout);
        }

        ElementVerification IWhiteElement.WaitUntil(int timeout)
        {
            return WaitUntil(timeout);
        }
    }
}
