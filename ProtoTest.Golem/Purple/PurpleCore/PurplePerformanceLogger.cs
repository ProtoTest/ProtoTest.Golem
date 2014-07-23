using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoTest.Golem.Purple.PurpleCore
{
    public static class PurplePerformanceLogger
    {
        public struct LogEntries
        {
            public string ElementName;
            public string ElementLocator;
            public int SecondsToLocate;
            public int MilisecondsToLocate;
        }

        private static List<LogEntries> LogEvents = new List<LogEntries>();

        public static void AddEntry(string name, string locator, int seconds, int miliseconds)
        {
            LogEntries newEntry = new LogEntries();
            newEntry.ElementName = name;
            newEntry.ElementLocator = locator;
            newEntry.SecondsToLocate = seconds;
            newEntry.MilisecondsToLocate = miliseconds;
            LogEvents.Add(newEntry);
        }


    }
}
