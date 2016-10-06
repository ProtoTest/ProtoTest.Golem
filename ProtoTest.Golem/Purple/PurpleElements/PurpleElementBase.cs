using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using Golem.Purple.Elements;
using Golem.Purple.PurpleCore;
using Purple.Core;
using PurpleLib;

namespace Golem.Purple.PurpleElements
{
    public class PurpleElementBase : IPurpleElement
    {
        private Locator _locator;
        protected AutomationElement _UIAElement;
        //This is not built yet -- should be interesting we might want to do this on the screenobject base
        private UIA_ElementCacher elementCache;

        public PurpleElementBase(string name, string locatorPath)
        {
            ElementName = name;
            PurplePath = locatorPath;
        }

        public PurpleElementBase(string name, AutomationElement element)
        {
            ElementName = name;
            _UIAElement = element;
            PurplePath = aLocator.FindPurplePath(element);
        }

        /// <summary>
        ///     This can be used to find a PurplePath for an automation element
        /// </summary>
        public Locator aLocator
        {
            get { return new Locator(); }
        }

        //These functions are used to set the cursor position and handle click events
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

        public void Invoke()
        {
            //This function may need to go in a different class -- although a lot of elements use the Invoke Pattern
            ((InvokePattern) PurpleElement.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
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
            var enabled = false;
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

        public bool IsOnScreen()
        {
            bool isVisible;
            if (_UIAElement != null)
            {
                if (_UIAElement.Current.IsOffscreen)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            else
            {
                if (PurpleElement.Current.IsOffscreen)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            return isVisible;
        }

        #region Accessor Methods

        public AutomationElement PurpleElement
        {
            get
            {
                //This will wait for specified ElementTimeout config before trying to interact with the element
                _UIAElement = aLocator.WaitForElementAvailable(PurplePath, ElementName);
                return _UIAElement;
            }
        }

        public Rect Bounds
        {
            get { return PurpleElement.Current.BoundingRectangle; }
        }

        //used with Interface
        public String ElementName { get; private set; }
        public String PurplePath { get; private set; }

        public AutomationElement UIAElement
        {
            get { return _UIAElement; }
        }

        public bool HasChildren
        {
            get { return aLocator.HasChildren(PurplePath, ElementName); }
        }

        public List<AutomationElement> GetChildren()
        {
            return aLocator.GetChildren(PurpleElement);
        }

        #endregion

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
            mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
        }

        public void RMB_Up()
        {
            mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public void Click()
        {
            if (_UIAElement != null)
            {
                var point = _UIAElement.GetClickablePoint();
                SetCursorPos((int) point.X, (int) point.Y);
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
                var point = PurpleElement.GetClickablePoint();
                SetCursorPos((int) point.X, (int) point.Y);
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

        public void RightClick()
        {
            if (_UIAElement != null)
            {
                var point = _UIAElement.GetClickablePoint();
                SetCursorPos((int) point.X, (int) point.Y);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
            else
            {
                if (!PurpleElement.Current.IsOffscreen)
                {
                    if (_UIAElement.Current.IsEnabled)
                    {
                        RightClick();
                    }
                }
            }
        }

        #endregion
    }
}