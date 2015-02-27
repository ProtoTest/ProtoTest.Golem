using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Windows.Automation;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using PurpleLib;
using Timer = System.Timers.Timer;

namespace ProtoTest.Golem.Purple.PurpleCore
{
    public class Locator
    {
        private static readonly PurplePath _purplePath = new PurplePath();

        private readonly Timer elementTimeoutTimer =
            new Timer(Config.Settings.purpleSettings.Purple_ElementTimeoutWaitSeconds*1000);

        private bool notfound;

        public Locator()
        {
            _purplePath.Delimiter = Config.Settings.purpleSettings.Purple_Delimiter;
            _purplePath.BlankValue = Config.Settings.purpleSettings.Purple_blankValue;
            _purplePath.DefaultWindowName = Config.Settings.purpleSettings.Purple_windowTitle;
            _purplePath.ValueDelimiterStart = Config.Settings.purpleSettings.Purple_ValueDelimiterStart;
            _purplePath.ValueDelimiterEnd = Config.Settings.purpleSettings.Purple_ValueDelimiterEnd;

            elementTimeoutTimer.Elapsed += elementTimeout;
        }

        public PurplePath ByPurplePath
        {
            get { return _purplePath; }
        }

        //TODO need to figure out how and why this kills subsequent tests - prossible due to unhandled exception
        //handling it though causes the test to hang instead of moving onto the next test in the suite
        public AutomationElement WaitForElementAvailable(string purplePath, string name)
        {
            elementTimeoutTimer.Start();
            AutomationElement elementAvailable = null;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            TestBase.Log(string.Format("Locating Element: {0} ", name));
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
                    if (PurpleTestBase.PerfLogging)
                    {
                        PurplePerformanceLogger.AddEntry(name, purplePath,
                            Config.Settings.purpleSettings.Purple_ElementTimeoutWaitSeconds, 0);
                    }
                    break;
                }
            }
            elementTimeoutTimer.Stop();
            stopWatch.Stop();
            if (!notfound)
            {
                var time = stopWatch.Elapsed;
                TestBase.Log(string.Format("Element: {2} found in {0}.{1} seconds.", time.Seconds, time.Milliseconds,
                    name));
                if (PurpleTestBase.PerfLogging)
                {
                    PurplePerformanceLogger.AddEntry(name, purplePath, time.Seconds, time.Milliseconds);
                }
            }
            if (elementAvailable == null)
            {
                Assert.Fail("Element: {0} with Path: {1} Failed to respond in alloted time.", name, purplePath);
            }
            return elementAvailable;
        }

        private void elementTimeout(object source, ElapsedEventArgs args)
        {
            TestBase.Log("Element took longer than " + Config.Settings.purpleSettings.Purple_ElementTimeoutWaitSeconds +
                         " Seconds to respond.");
            notfound = true;
        }

        public bool HasChildren(string purplePath, string name)
        {
            var presumedParent = WaitForElementAvailable(purplePath, name);
            return ByPurplePath.HasChildren(presumedParent);
        }

        public List<AutomationElement> GetChildren(AutomationElement presumedParent)
        {
            return ByPurplePath.GetChildren(presumedParent);
        }

        public string FindPurplePath(AutomationElement element)
        {
            return ByPurplePath.getPurplePath(element);
        }
    }
}