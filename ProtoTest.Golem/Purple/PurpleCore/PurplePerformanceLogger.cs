using System.Collections.Generic;

namespace Golem.Purple.PurpleCore
{
    public static class PurplePerformanceLogger
    {
        private static readonly List<LogEntries> LogEvents = new List<LogEntries>();

        public static void AddEntry(string name, string locator, int seconds, int miliseconds)
        {
            var newEntry = new LogEntries();
            newEntry.ElementName = name;
            newEntry.ElementLocator = locator;
            newEntry.SecondsToLocate = seconds;
            newEntry.MilisecondsToLocate = miliseconds;
            LogEvents.Add(newEntry);
        }

        public struct LogEntries
        {
            public string ElementLocator;
            public string ElementName;
            public int MilisecondsToLocate;
            public int SecondsToLocate;
        }
    }
}