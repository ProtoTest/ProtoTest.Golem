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

        public AutomationElement PurpleElement
        {
            get
            {
                _UIAElement = PurpleCore.Locator.WaitForElementAvailable(_PurplePath);
                return _UIAElement;
            }
        }
        
        public Rect Bounds
        {
            get { return PurpleElement.Current.BoundingRectangle; }
        }

        public String ElementName {get { return _elementName; }}

        public PurpleElementBase(string name, string locatorPath)
        {
            _elementName = name;
            _PurplePath = locatorPath;
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
            if (!PurpleElement.Current.IsOffscreen)
            {
                SetCursorPos((int) PurpleElement.GetClickablePoint().X, (int) PurpleElement.GetClickablePoint().Y);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        public void DoubleLeftClick()
        {
            
            if (!PurpleElement.Current.IsOffscreen)
            {
                SetCursorPos((int) PurpleElement.GetClickablePoint().X, (int) PurpleElement.GetClickablePoint().Y);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(50);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            
        }
        #endregion

        //This function may need to go in a different class
        public void Invoke()
        {
            ((InvokePattern)PurpleElement.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
        }
        #region KeyboardInput Functions for simulating keyboard input
        public void HoldShift()
        {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
        }

        public void ReleaseShift()
        {
            InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
        }

        public void PressKey(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyPress(key);
        }
        #endregion

        



    }
}
