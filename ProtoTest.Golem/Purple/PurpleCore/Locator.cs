using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Automation;
using MbUnit.Framework;
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

        public static AutomationElement WaitForElementAvailable(string purplePath)
        {
            elementTimeoutTimer.Start();
            AutomationElement getsomething = null;
            while (getsomething == null)
            {
                try
                {
                    getsomething = ByPurplePath.FindElement(purplePath);
                }
                catch (Exception e)
                {
                    Thread.Sleep(50);
                }
                if (notfound)
                {
                    Assert.Fail("Fail damnit");
                    
                }
            }
            elementTimeoutTimer.Stop();
            return getsomething;
        }

        private static void elementTimeout(object source, ElapsedEventArgs args)
        {
            TestBase.Log("Element took longer than " + Config.Settings.purpleSettings.Purple_ElementTimeoutWaitSeconds + " Seconds to respond.");
            notfound = true;
        }


    }
}
