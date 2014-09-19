using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using WindowsInput;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.PurpleCore;

namespace ProtoTest.Golem.Purple.PurpleElements
{

    public static class PurpleWindow
    {
        
        private static AutomationElement window;
        public static AutomationElement purpleWindow { get { return window; } }
         [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            uint uFlags);
        private static IntPtr handle;

        
        public static bool FindRunningProcess()
        {
            bool processRunning = false;
            Process[] processes = Process.GetProcessesByName(Config.Settings.purpleSettings.ProcessName);
            if (processes.Length == 0)
            {
                TestBase.Log(string.Format("Could not find process {0}. Attempting to start process...", Config.Settings.purpleSettings.ProcessName));
                var startProcess = new ProcessStartInfo(Config.Settings.purpleSettings.appPath);
                Process app = Process.Start(startProcess);
                waitForWindow();
                handle = app.MainWindowHandle;
                SetWindowPos(app.MainWindowHandle, new IntPtr(-1), 0, 0, 0, 0, 3); //This should bring the application to the front once it starts
                //SetForegroundWindow(handle);  //this didn't bring the app to the front
            }
            else
            {
                TestBase.Log(string.Format("Process Length is: {0}. Attempting to kill existing process and start it up again.", processes.Length));
                EndProcess();
                FindRunningProcess();
            }
            //the PurpleLib will always try to find an element the first window the with name = Purple_windowTitle;
            return processRunning;
        }

        private static void waitForWindow()
        {
            window = Locator.WaitForElementAvailable(Config.Settings.purpleSettings.Purple_Delimiter + Config.Settings.purpleSettings.Purple_windowTitle, Config.Settings.purpleSettings.Purple_windowTitle);

        }

        public static void EndProcess(String DontsaveProjectPath = "notused")
        {
            Process[] processes = Process.GetProcessesByName(Config.Settings.purpleSettings.ProcessName);
            foreach (Process process in processes)
            {
                process.Kill();
                Thread.Sleep(2000);
                //process.CloseMainWindow();
                //PurpleButton dontsave = new PurpleButton("Save Dialog: No", "/LifeQuest™ Pipeline/Save Project?/Save Project?/No");
                //dontsave.Invoke();
            }
        }

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
        public static void ReleaseKey(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyUp(key);
        }
        public static void PressKey(VirtualKeyCode key)
        {
            InputSimulator.SimulateKeyPress(key);
        }

    }
}
