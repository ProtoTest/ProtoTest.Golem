using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using ProtoTest.Golem.Core;
using PurpleLib;
using Point = System.Windows.Point;

namespace ProtoTest.Golem.White.PurpleElements
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

        //Need to read locator setup functions from App.config
        //Only want to go out to the disk filesystem once per test suite
        private static PurplePath purplePathLocator = new PurplePath();
        private static bool purpleInit = false;
        
        //This is not built yet -- should be interesting we might want to do this on the screenobject base
        private UIA_ElementCacher elementCache;

        public AutomationElement PurpleElement
        {
            get
            {
                _UIAElement = purplePathLocator.FindElement(_PurplePath);
                if (_UIAElement == null)
                {
                    Common.Log(string.Format("Unable to find element with PurplePath Specified: {0}\n", _PurplePath));
                }
                return _UIAElement;
            }
        }

        public String ElementName {get { return _elementName; }}

        public PurpleElementBase(string name, string locatorPath)
        {
            _elementName = name;
            _PurplePath = locatorPath;
            InitPurpleLocator();
        }

        private void InitPurpleLocator()
        {
            if (!purpleInit)
            {
                purplePathLocator.Delimiter = Config.Settings.whiteSettings.Purple_Delimiter;
                purplePathLocator.BlankValue = Config.Settings.whiteSettings.Purple_blankValue;
                purplePathLocator.DefaultWindowName = Config.Settings.whiteSettings.Purple_windowTitle;
                purplePathLocator.ValueDelimiterStart = Config.Settings.whiteSettings.Purple_ValueDelimiterStart;
                purplePathLocator.ValueDelimiterEnd = Config.Settings.whiteSettings.Purple_ValueDelimiterEnd;
                purpleInit = true;
            }
            
        }
        
        public void Click()
        {
            if (!PurpleElement.Current.IsOffscreen)
            {
                SetCursorPos((int) PurpleElement.GetClickablePoint().X, (int) PurpleElement.GetClickablePoint().Y);
                mouse_event(Purple.Core.WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(Purple.Core.WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }

        public void Invoke()
        {
            ((InvokePattern)PurpleElement.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
        }




    }
}
