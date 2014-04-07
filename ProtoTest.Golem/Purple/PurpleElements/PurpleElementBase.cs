using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using WindowsInput;
using ProtoTest.Golem.Core;
using Purple.Core;
using PurpleLib;
using Point = System.Windows.Point;

namespace ProtoTest.Golem.Purple.PurpleElements
{
    public class PurpleElementBase
    {
        //These functions are used to set the cursor position and handle click events
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

        protected AutomationElement _UIAElement;
        private String _PurplePath;
        private String _elementName;
        private bool isOffScreen;

        //This is not built yet -- should be interesting we might want to do this on the screenobject base
        private UIA_ElementCacher elementCache;

        #region Accessor Methods
        public AutomationElement PurpleElement
        {
            get
            {
                //This will wait for specified ElementTimeout config before trying to interact with the element
                _UIAElement = PurpleCore.Locator.WaitForElementAvailable(_PurplePath, _elementName);
                return _UIAElement;
            }
        }
        
        public Rect Bounds
        {
            get { return PurpleElement.Current.BoundingRectangle; }
        }

        public String ElementName {get { return _elementName; }}
        #endregion

        public PurpleElementBase(string name, string locatorPath)
        {
            _elementName = name;
            _PurplePath = locatorPath;
        }

        public void Invoke()
        {
            //This function may need to go in a different class -- although a lot of elements use the Invoke Pattern
            ((InvokePattern)PurpleElement.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
        }

        public void SetFocus()
        {
            if (_UIAElement != null)
            {
                _UIAElement.SetFocus();
            }
            else
            {
                if (!PurpleElement.Current.IsOffscreen)
                {
                    SetFocus();
                }
            }
        }

        public bool IsEnabled()
        {
            bool enabled = false;
            if (_UIAElement == null)
            {
                enabled = PurpleElement.Current.IsEnabled;
            }
            else
            {
                enabled = _UIAElement.Current.IsEnabled;
            }
            return enabled;
        }
       
        #region MouseFunctions Functions for dealing with and simulating mouse input

        public void MoveCursor(Point position)
        {
            SetCursorPos((int) position.X, (int) position.Y);
        }

        public void LMB_Down()
        {
            mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        public void LMB_Up()
        {
            mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void RMB_Down()
        {
            mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTDOWN, 0,0,0,0);
        }

        public void RMB_Up()
        {
            mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        public void Click()
        {
            if (_UIAElement != null)
            {
                SetCursorPos((int)_UIAElement.GetClickablePoint().X, (int)_UIAElement.GetClickablePoint().Y);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            else
            {
                if (!PurpleElement.Current.IsOffscreen)
                {
                    if (_UIAElement.Current.IsEnabled)
                    {
                        Click();
                    }
                }
            }

        }

        public void DoubleLeftClick()
        {
            if (_UIAElement != null)
            {
                SetCursorPos((int) PurpleElement.GetClickablePoint().X, (int) PurpleElement.GetClickablePoint().Y);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            else
            {
                if (!PurpleElement.Current.IsOffscreen)
                {
                    if (_UIAElement.Current.IsEnabled)
                    {
                        DoubleLeftClick();
                    }
                }
            }
            
        }
        #endregion


        
    }
}
