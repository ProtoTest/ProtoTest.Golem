using System.Drawing;
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
using Point = System.Windows.Point;

namespace ProtoTest.Golem.White.Elements
{
    public class BaseItem<T> : IUIItem where T : UIItem
    {
        private UIItem _item;

        public SearchCriteria criteria;
        public string name;
        public Window window;

        public BaseItem(string name, SearchCriteria criteria, Window window)
        {
            this.name = name;
            this.criteria = criteria;
            this.window = window;
        }

        public UIItem item
        {
            get
            {
                _item = window.Get<T>(criteria);
                return _item;
            }
            set { _item = item; }
        }

        public AutomationElement AutomationElement { get; private set; }

        public bool ValueOfEquals(AutomationProperty property, object compareTo)
        {
            return item.ValueOfEquals(property, compareTo);
        }

        public void RightClickAt(Point point)
        {
            item.RightClickAt(point);
        }

        public void RightClick()
        {
            item.RightClick();
        }

        public void Focus()
        {
            item.Focus();
        }

        public void Visit(WindowControlVisitor windowControlVisitor)
        {
            item.Visit(windowControlVisitor);
        }

        public string ErrorProviderMessage(Window window)
        {
            return item.ErrorProviderMessage(window);
        }

        public bool NameMatches(string text)
        {
            return NameMatches(text);
        }

        public void Click()
        {
            item.Click();
        }

        public void DoubleClick()
        {
            item.DoubleClick();
        }

        public void KeyIn(KeyboardInput.SpecialKeys key)
        {
            item.KeyIn(key);
        }

        public void UnHookEvents()
        {
            item.UnHookEvents();
        }

        public void HookEvents(UIItemEventListener eventListener)
        {
            item.HookEvents(eventListener);
        }

        public void SetValue(object value)
        {
            item.SetValue(value);
        }

        public void LogStructure()
        {
            item.LogStructure();
        }

        AutomationElement IUIItem.GetElement(SearchCriteria searchCriteria)
        {
            return item.GetElement(searchCriteria);
        }

        public void Enter(string value)
        {
            item.Enter(value);
        }

        AutomationElement IUIItem.AutomationElement
        {
            get { return item.AutomationElement; }
        }

        public bool Enabled { get; private set; }
        public WindowsFramework Framework { get; private set; }
        public Point Location { get; private set; }
        public Rect Bounds { get; private set; }
        public string Name { get; private set; }
        public Point ClickablePoint { get; private set; }
        public string AccessKey { get; private set; }
        public string Id { get; private set; }
        public bool Visible { get; private set; }
        public string PrimaryIdentification { get; private set; }
        public ActionListener ActionListener { get; private set; }
        public IScrollBars ScrollBars { get; private set; }
        public bool IsOffScreen { get; private set; }
        public bool IsFocussed { get; private set; }
        public Color BorderColor { get; private set; }
        public Bitmap VisibleImage { get; private set; }

        public void ActionPerforming(UIItem uiItem)
        {
            item.ActionPerforming(uiItem);
        }

        public void ActionPerformed(Action action)
        {
            item.ActionPerformed(action);
        }

        public AutomationElement GetElement(SearchCriteria searchCriteria)
        {
            return item.GetElement(searchCriteria);
        }
    }
}