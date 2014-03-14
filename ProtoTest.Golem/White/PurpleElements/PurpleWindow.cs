using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WindowsInput;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.White.PurpleCore;
using PurpleLib;

namespace ProtoTest.Golem.White.PurpleElements
{
    public static class PurpleWindow
    {
        private static string windowTitle = Config.Settings.whiteSettings.Purple_windowTitle;
        private static AutomationElement window = new PurplePath().FindElement(Config.Settings.whiteSettings.Purple_Delimiter + Config.Settings.whiteSettings.Purple_windowTitle);

        private static WindowPattern GetWindowPattern(AutomationElement targetControl)
        {
            WindowPattern windowPattern = null;

            try
            {
                windowPattern = targetControl.GetCurrentPattern(WindowPattern.Pattern)
                    as WindowPattern;
            }
            catch (InvalidOperationException)
            {
                // object doesn't support the WindowPattern control pattern 
                return null;
            }
            // Make sure the element is usable. 
            if (false == windowPattern.WaitForInputIdle(10000))
            {
                // Object not responding in a timely manner 
                return null;
            }
            return windowPattern;
        }

        public static void SetVisualState(Window_VisualStyle visualState)
        {
            WindowPattern windowPattern = GetWindowPattern(window);
            try
            {
                if (windowPattern.Current.WindowInteractionState == WindowInteractionState.ReadyForUserInteraction)
                {
                    switch (visualState)
                    {
                        case Window_VisualStyle.Maximized:
                            // Confirm that the element can be maximized 
                            if ((windowPattern.Current.CanMaximize) && !(windowPattern.Current.IsModal))
                            {
                                windowPattern.SetWindowVisualState(WindowVisualState.Maximized);
                            }
                            break;
                        case Window_VisualStyle.Minimized:
                            // Confirm that the element can be minimized 
                            if ((windowPattern.Current.CanMinimize) &&!(windowPattern.Current.IsModal))
                            {
                                windowPattern.SetWindowVisualState(WindowVisualState.Minimized);
                            }
                            break;
                        case Window_VisualStyle.Normal:
                            windowPattern.SetWindowVisualState(WindowVisualState.Normal);
                            break;
                        default:
                            windowPattern.SetWindowVisualState(WindowVisualState.Normal);
                            break;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // object is not able to perform the requested action 
                return;
            }
        }

        //These are duplicated on the PurpleElement we should pick a place
        public static void HoldKey(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyDown(key);
        }
        public static void LeaveKey(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyUp(key);
        }
        public static void PressSpecialKey(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyPress(key);
        }

    }
}
