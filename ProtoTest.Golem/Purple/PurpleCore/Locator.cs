using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Automation;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using Timer = System.Timers.Timer;

namespace ProtoTest.Golem.Purple.PurpleCore
{
    public static class Locator
    {
        private static PurpleLib.PurplePath _purplePath = new PurpleLib.PurplePath();
        private static Timer elementTimeoutTimer = new Timer(Config.Settings.purpleSettings.Purple_ElementTimeoutWaitSeconds * 1000);
        private static bool notfound = false;

        public static PurpleLib.PurplePath ByPurplePath{ get { return _purplePath; }}

        static Locator()
        {
            _purplePath.Delimiter = Config.Settings.purpleSettings.Purple_Delimiter;
            _purplePath.BlankValue = Config.Settings.purpleSettings.Purple_blankValue;
            _purplePath.DefaultWindowName = Config.Settings.purpleSettings.Purple_windowTitle;
            _purplePath.ValueDelimiterStart = Config.Settings.purpleSettings.Purple_ValueDelimiterStart;
            _purplePath.ValueDelimiterEnd = Config.Settings.purpleSettings.Purple_ValueDelimiterEnd;
            elementTimeoutTimer.Elapsed += elementTimeout;
        }

        public static AutomationElement WaitForElementAvailable(string purplePath, string name)
        {
            elementTimeoutTimer.Start();
            AutomationElement elementAvailable = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TestBase.Log(string.Format("Locating Element: {0}", name));
            while (elementAvailable == null)
            {
                try
                {
                    elementAvailable = ByPurplePath.FindElement(purplePath);
                }
                catch (Exception e)
                {
                    Thread.Sleep(50);
                }
                if (notfound)
                {
                    Assert.Fail(string.Format("Element: {0} with Path: {1} Failed to respond in alloted time.", name, purplePath));
                    
                }
            }
            elementTimeoutTimer.Stop();
            stopWatch.Stop();
            if (!notfound)
            {
                TimeSpan time = stopWatch.Elapsed;
                TestBase.Log(string.Format("Element: {2} found in {0}.{1} seconds.", time.Seconds, time.Milliseconds, name));
            }
            return elementAvailable;
        }

        private static void elementTimeout(object source, ElapsedEventArgs args)
        {
            TestBase.Log("Element took longer than " + Config.Settings.purpleSettings.Purple_ElementTimeoutWaitSeconds + " Seconds to respond.");
            notfound = true;
        }


    }
}
