using System.Configuration;
using System.Drawing;
using System.Windows;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.Recording;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.Scrolling;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WPFUIItems;
using TestStack.White.WindowsAPI;
using Point = System.Windows.Point;

namespace ProtoTest.Golem.White.Elements
{
    public class WhiteButton : Button, IWhiteElement
    {
        public string description { get; set; }
        public SearchCriteria criteria { get; set; }
        public UIItem parent { get; set; }
        private Button _item;
        public Button item
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

        public WhiteButton(SearchCriteria criteria, UIItem parent = null, string description=null)
        {
            this.description = description ?? criteria.ToString();
            this.criteria = criteria;
            this.parent = parent ?? WhiteTestBase.window;
        }

        public override void HookEvents(UIItemEventListener eventListener)
        {
            item.HookEvents(eventListener);
        }

        public override void UnHookEvents()
        {
            item.UnHookEvents();
        }

        public override void Toggle()
        {
            item.Toggle();
        }

        public override ToggleState State
        {
            get
            {
                return item.State;
            }
            set
            {
                item.State = value;
            }
        }

        public override string Text
        {
            get { return item.Text; }
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

        public override void Visit(WindowControlVisitor WindowControlVisitor)
        {
            item.Visit(WindowControlVisitor);
        }

        public override string ErrorProviderMessage(Window Window)
        {
            return item.ErrorProviderMessage(Window);
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

        //protected override void HookClickEvent(UIItemEventListener eventListener)
        //{
        //    base.HookClickEvent(eventListener);
        //}

        //protected override void UnHookClickEvent()
        //{
        //    base.UnHookClickEvent();
        //}

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

        //protected override void EnterData(string value)
        //{
        //    base.EnterData(value);
        //}

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


        
    }
}