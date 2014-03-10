using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.Recording;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.Scrolling;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.WindowsAPI;
using Action = TestStack.White.UIItems.Actions.Action;
using Point = System.Windows.Point;

namespace ProtoTest.Golem.White.Elements
{
    public class WhiteWindow : Window, IWhiteElement
    {
        //This is obviously a UIItemCOntainer
        public string description { get; set; }
        public SearchCriteria criteria { get; set; }
        public UIItem parent { get; set; }

        public UIItem getItem()
        {
            return window;
        }

        public string title { get; set; }
        private Window _window;

        public Window window
        {
            get
            {
                _window = getWindow();
                return _window;
            }
            set
            {
                _window = value;
            }
        }

        private void foo()
        {
            WindowSession.Register(this);
        }

        public ElementVerification Verify(int timeout=0)
        {
            return new ElementVerification(this,timeout,false,true);
        }

        public ElementVerification WaitUntil(int timeout=0)
        {
            return new ElementVerification(this,timeout,true,true);
        }

        public WhiteWindow(string title, Window parent = null, string description = null)
        {
            this.description = description ?? title;
            this.title = title;
            this.parent = parent;
        }
        //SU - Changed this to require description  will use for caching name
        public WhiteWindow(SearchCriteria criteria, string description, Window parent = null)
        {
            this.description = description;
            this.criteria = criteria;
            this.parent = parent;
        }

        private Window FindWindow()
        {
            var windows = WhiteTestBase.app.GetWindows();
            foreach (var win in windows)
            {
                if (win.Title.Contains(title))
                    return win;
            }
            return null;
        }

        

        private Window getWindow()
        {
            if (_window == null)
            {
                    var parentWindow = (Window)parent;
                    return criteria != null
                        ? (parent == null
                            ? WhiteTestBase.app.GetWindow(criteria, InitializeOption.NoCache.AndIdentifiedBy(this.description))
                            : parentWindow.ModalWindow(criteria, InitializeOption.NoCache.AndIdentifiedBy(this.description)))
                        : (parent == null
                            //? FindWindow()  //SU - Reverted this change, causing timing issue in QI tests
                            ? WhiteTestBase.app.GetWindow(title, InitializeOption.NoCache.AndIdentifiedBy(this.description))
                            : parentWindow.ModalWindow(title, InitializeOption.NoCache.AndIdentifiedBy(this.description)));  

            }
            return _window;
        }

        public override void Close()
        {
            window.Close();
        }

        public override StatusStrip StatusBar(InitializeOption initializeOption)
        {
            return window.StatusBar(initializeOption);
        }

        public override void WaitWhileBusy()
        {
            window.WaitWhileBusy();
        }


        public override void ActionPerformed(Action action)
        {
            window.ActionPerformed(action);
        }

        public override Window ModalWindow(string title)
        {
            return window.ModalWindow(title);
        }

        public override Window ModalWindow(SearchCriteria searchCriteria)
        {
            return window.ModalWindow(searchCriteria);
        }

        public override void Visit(WindowControlVisitor windowControlVisitor)
        {
            window.Visit(windowControlVisitor);
        }

        public override void WaitTill(WaitTillDelegate waitTillDelegate)
        {
            window.WaitTill(waitTillDelegate);
        }

        public override void WaitTill(WaitTillDelegate waitTillDelegate, TimeSpan timeout)
        {
            window.WaitTill(waitTillDelegate, timeout);
        }

        public override void ReloadIfCached()
        {
            window.ReloadIfCached();
        }

        public override void HookEvents(UIItemEventListener eventListener)
        {
            window.HookEvents(eventListener);
        }

        public override void UnHookEvents()
        {
            window.UnHookEvents();
        }

        public override string ToString()
        {
            return window.ToString();
        }

        public override void Dispose()
        {
            window.Dispose();
        }

        public override Window MessageBox(string title)
        {
            return window.MessageBox(title);
        }

        public override void Focus(DisplayState displayState)
        {
            window.Focus(displayState);
        }

        public override UIItemContainer MdiChild(SearchCriteria searchCriteria)
        {
            return window.MdiChild(searchCriteria);
        }

        public override List<Window> ModalWindows()
        {
            return window.ModalWindows();
        }

        public override Menu PopupMenu(params string[] path)
        {
            return window.PopupMenu(path);
        }

        public override bool HasPopup()
        {
            return window.HasPopup();
        }

        public override string Title
        {
            get { return window.Title; }
        }

        public override bool IsClosed
        {
            get { return window.IsClosed; }
        }

        public override DisplayState DisplayState
        {
            get
            {
                return window.DisplayState;
            }
            set
            {
                window.DisplayState = value;
            }
        }

        public override ActionListener ActionListener
        {
            get { return window.ActionListener; }
        }

        public override TitleBar TitleBar
        {
            get { return window.TitleBar; }
        }

        public override bool IsModal
        {
            get { return window.IsModal; }
        }

        public override VerticalSpan VerticalSpan
        {
            get { return window.VerticalSpan; }
        }

        public override bool IsCurrentlyActive
        {
            get { return window.IsCurrentlyActive; }
        }

        public override T Get<T>()
        {
            return ((UIItemContainer) window).Get<T>();
        }

        public override T Get<T>(string primaryIdentification)
        {
            return ((UIItemContainer)window).Get<T>(primaryIdentification);
        }

        public override T Get<T>(SearchCriteria searchCriteria)
        {
            return ((UIItemContainer) window).Get<T>(searchCriteria);
        }

        public override IUIItem Get(SearchCriteria searchCriteria)
        {
            return window.Get(searchCriteria);
        }

        public override IUIItem Get(SearchCriteria searchCriteria, TimeSpan timeout)
        {
            return window.Get(searchCriteria, timeout);
        }

        public override void ReInitialize(InitializeOption option)
        {
            window.ReInitialize(option);
        }

        public override IUIItem[] GetMultiple(SearchCriteria criteria)
        {
            return window.GetMultiple(criteria);
        }

        public override void ActionPerforming(UIItem uiItem)
        {
            window.ActionPerforming(uiItem);
        }

        public override MenuBar GetMenuBar(SearchCriteria searchCriteria)
        {
            return window.GetMenuBar(searchCriteria);
        }

        public override ToolTip GetToolTipOn(UIItem uiItem)
        {
            return window.GetToolTipOn(uiItem);
        }

        public override ToolStrip GetToolStrip(string primaryIdentification)
        {
            return window.GetToolStrip(primaryIdentification);
        }

        public override List<UIItem> ItemsWithin(UIItem containingItem)
        {
            return window.ItemsWithin(containingItem);
        }

   

        public override UIItemCollection Items
        {
            get { return window.Items; }
        }

        public override AttachedKeyboard Keyboard
        {
            get { return window.Keyboard; }
        }

        public override AttachedMouse Mouse
        {
            get { return window.Mouse; }
        }

        public override MenuBar MenuBar
        {
            get { return window.MenuBar; }
        }

        public override List<MenuBar> MenuBars
        {
            get { return window.MenuBars; }
        }

        public override ToolTip ToolTip
        {
            get { return window.ToolTip; }
        }

        public override ToolStrip ToolStrip
        {
            get { return window.ToolStrip; }
        }

        public override List<Tab> Tabs
        {
            get { return window.Tabs; }
        }

     
        public override bool ValueOfEquals(AutomationProperty property, object compareTo)
        {
            return window.ValueOfEquals(property, compareTo);
        }

      
        public override void RightClickAt(Point point)
        {
            window.RightClickAt(point);
        }

        public override void RightClick()
        {
            window.RightClick();
        }

        public override void Focus()
        {
            window.Focus();
        }

        public override string ErrorProviderMessage(Window window)
        {
            return window.ErrorProviderMessage(window);
        }

        public override bool NameMatches(string text)
        {
            return window.NameMatches(text);
        }

        public override void Click()
        {
            window.Click();
        }

        public override void DoubleClick()
        {
            window.DoubleClick();
        }

        public override void KeyIn(KeyboardInput.SpecialKeys key)
        {
            window.KeyIn(key);
        }

        public override bool Equals(object obj)
        {
            return window.Equals(obj);
        }

        public override int GetHashCode()
        {
            return window.GetHashCode();
        }

       

        public override void SetValue(object value)
        {
            window.SetValue(value);
        }

        public override void LogStructure()
        {
            window.LogStructure();
        }

        public override AutomationElement GetElement(SearchCriteria searchCriteria)
        {
            return window.GetElement(searchCriteria);
        }

        public override void Enter(string value)
        {
            window.Enter(value);
        }

   
        public override void RaiseClickEvent()
        {
            window.RaiseClickEvent();
        }

        public override AutomationElement AutomationElement
        {
            get { return window.AutomationElement; }
        }

        public override bool Enabled
        {
            get { return window.Enabled; }
        }

        public override WindowsFramework Framework
        {
            get { return window.Framework; }
        }

        public override Point Location
        {
            get { return window.Location; }
        }

        public override Rect Bounds
        {
            get { return window.Bounds; }
        }

        public override string Name
        {
            get { return window.Name; }
        }

        public override Point ClickablePoint
        {
            get { return window.ClickablePoint; }
        }

        public override string AccessKey
        {
            get { return window.AccessKey; }
        }

        public override string Id
        {
            get { return window.Id; }
        }

        public override bool Visible
        {
            get { return window.Visible; }
        }

        public override string PrimaryIdentification
        {
            get { return window.PrimaryIdentification; }
        }

        public override IScrollBars ScrollBars
        {
            get { return window.ScrollBars; }
        }

        public override bool IsOffScreen
        {
            get { return window.IsOffScreen; }
        }

        public override bool IsFocussed
        {
            get { return window.IsFocussed; }
        }

        public override Color BorderColor
        {
            get { return window.BorderColor; }
        }

        public override Bitmap VisibleImage
        {
            get { return window.VisibleImage; }
        }

        public override string HelpText
        {
            get { return window.HelpText; }
        }


        public override Window ModalWindow(string title, InitializeOption option)
        {
            return window.ModalWindow(title, option);
        }

        public override Window ModalWindow(SearchCriteria searchCriteria, InitializeOption option)
        {
            return window.ModalWindow(searchCriteria, option);
        }

        public override PopUpMenu Popup
        {
            get { return window.Popup; }
        }
}
}
