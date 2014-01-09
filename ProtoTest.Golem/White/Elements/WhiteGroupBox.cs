using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.Recording;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.Scrolling;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.WindowsAPI;
using Action = TestStack.White.UIItems.Actions.Action;
using Point = System.Windows.Point;

namespace ProtoTest.Golem.White.Elements
{
    public class WhiteGroupBox : GroupBox
    {
        public string description;
        public SearchCriteria criteria;
        private GroupBox _item;
        private UIItem parent;

        public GroupBox item
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

        public WhiteGroupBox(SearchCriteria Criteria, UIItem Parent = null, string Description=null)
        {
            this.description = Description ?? Criteria.ToString();
            this.criteria = Criteria;
            this.parent = Parent ?? WhiteTestBase.window;
        }

        public override T Get<T>()
        {
            return ((UIItemContainer) this).Get<T>();
        }

        public override T Get<T>(string primaryIdentification)
        {
            return ((UIItemContainer) this).Get<T>(primaryIdentification);
        }

        public override T Get<T>(SearchCriteria searchCriteria)
        {
            return ((UIItemContainer) this).Get<T>(searchCriteria);
        }

        public override IUIItem Get(SearchCriteria searchCriteria)
        {
            return item.Get(searchCriteria);
        }

        public override IUIItem Get(SearchCriteria searchCriteria, TimeSpan timeout)
        {
            return item.Get(searchCriteria, timeout);
        }

        public override void ReInitialize(InitializeOption option)
        {
            item.ReInitialize(option);
        }

        public override IUIItem[] GetMultiple(SearchCriteria criteria)
        {
            return item.GetMultiple(criteria);
        }

        public override void ActionPerforming(UIItem uiItem)
        {
            item.ActionPerforming(uiItem);
        }

        public override MenuBar GetMenuBar(SearchCriteria searchCriteria)
        {
            return item.GetMenuBar(searchCriteria);
        }

        public override ToolTip GetToolTipOn(UIItem uiItem)
        {
            return item.GetToolTipOn(uiItem);
        }

        public override ToolStrip GetToolStrip(string primaryIdentification)
        {
            return item.GetToolStrip(primaryIdentification);
        }

        public override List<UIItem> ItemsWithin(UIItem containingItem)
        {
            return item.ItemsWithin(containingItem);
        }

        public override UIItemCollection Items
        {
            get { return item.Items; }
        }

        public override AttachedKeyboard Keyboard
        {
            get { return item.Keyboard; }
        }

        public override AttachedMouse Mouse
        {
            get { return item.Mouse; }
        }

        public override VerticalSpan VerticalSpan
        {
            get { return item.VerticalSpan; }
        }

        public override MenuBar MenuBar
        {
            get { return item.MenuBar; }
        }

        public override List<MenuBar> MenuBars
        {
            get { return item.MenuBars; }
        }

        public override ToolTip ToolTip
        {
            get { return item.ToolTip; }
        }

        public override ToolStrip ToolStrip
        {
            get { return item.ToolStrip; }
        }

        public override List<Tab> Tabs
        {
            get { return item.Tabs; }
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
    }
}
