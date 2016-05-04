using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ProtoTest.Golem.Core
{
    /// <summary>
    ///     Holds a list of Actions, with some functions to help print
    /// </summary>
    public class ActionList
    {
        public List<Action> actions;

        public ActionList()
        {
            actions = new List<Action>();
        }

        public void addAction(string name, DateTime time)
        {
            actions.Add(new Action(name, time));
        }

        public void addAction(string name)
        {
            var time = DateTime.Now;
            actions.Add(new Action(name, time));
        }

        public void PrintActions()
        {
            foreach (var a in actions)
            {
                Log.Message(a.name + " : " + a._time.ToString("HH:mm:ss.ffff"));
            }
        }

        public void PrintActionTimings()
        {
            DateTime start;
            DateTime end;
            TimeSpan difference;
            for (var i = 1; i < actions.Count; i++)
            {
                while (actions[i].name == actions[i - 1].name)
                {
                    i++;
                }
                start = actions[i - 1]._time;
                end = actions[i]._time;
                difference = end.Subtract(start);
                Log.Message(actions[i].name + " : " + difference);
            }
            start = actions[0]._time;
            end = actions[actions.Count - 1]._time;
            difference = end.Subtract(start);
            Log.Message("All Actions : " + difference);
        }

        public void RemoveDuplicateEntries()
        {
            var distinctItems = actions.GroupBy(x => x.name).Select(y => y.Last());
            actions = distinctItems.ToList();
        }

        public class Action
        {
            public DateTime _time;
            public string name;

            public Action(string name, DateTime time)
            {
                this.name = name;
                _time = time;
            }
        }
    }
}