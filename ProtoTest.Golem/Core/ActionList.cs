using System;
using System.Collections.Generic;
using Gallio.Framework;

namespace ProtoTest.Golem.Core
{
    /// <summary>
    /// Holds a list of Actions, with some functions to help print
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
            DateTime time = DateTime.Now;
            actions.Add(new Action(name, time));
        }

        public void PrintActions()
        {
            TestLog.BeginSection("Actions");
            foreach (Action a in actions)
            {
                TestLog.WriteLine(a.name + " : " + a._time.ToString("HH:mm:ss.ffff"));
            }
            TestLog.End();
        }

        public void PrintActionTimings()
        {
            if (actions.Count > 0)
            {
                TestLog.BeginSection("Test Action Timings:");
                DateTime start;
                DateTime end;
                TimeSpan difference;
                for (int i = 1; i < actions.Count; i++)
                {
                    start = actions[i - 1]._time;
                    end = actions[i]._time;
                    difference = end.Subtract(start);
                    TestLog.WriteLine(actions[i].name + " : " + difference.ToString("mm':'ss'.'ffff"));
                }
                start = actions[0]._time;
                end = actions[actions.Count - 1]._time;
                difference = end.Subtract(start);
                TestLog.WriteLine("All Actions : " + difference.ToString("mm':'ss'.'ffff"));
                TestLog.End();  
            }
            
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