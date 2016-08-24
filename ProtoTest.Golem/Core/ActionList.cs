using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Golem.Core
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

        public void addAction(string name, Action.ActionType type=Action.ActionType.Other)
        {
            var time = DateTime.Now;
            actions.Add(new Action(name, time, type));
        }

        public void PrintActions()
        {
            foreach (var a in actions)
            {
                Log.Message(a.name + " : " + a.time.ToString("HH:mm:ss.ffff"));
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
                start = actions[i - 1].time;
                end = actions[i].time;
                difference = end.Subtract(start);
                Log.Message(actions[i].name + " : " + difference);
            }
            start = actions[0].time;
            end = actions[actions.Count - 1].time;
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
            public DateTime time;
            public string name;
            public ActionType type;

            public Action(string name, DateTime time, ActionType type=ActionType.Other)
            {
                this.name = name;
                this.time = time;
                this.type = type;
            }

            public enum ActionType
            {
                Message = 0,
                Warning = 1,
                Error = 2,
                Video = 3, 
                Image = 4,
                Link = 5,
                Other = 6
            }
        }
    }

  
}