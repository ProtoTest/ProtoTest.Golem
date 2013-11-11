using System;
using System.Collections.Generic;
using Gallio.Framework;

namespace ProtoTest.Golem.Core
{
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
            Common.Log("PRINTING ACTIONS");
            foreach (Action a in actions)
            {
                Common.Log(a.name + " : " + a._time.ToString("HH:mm:ss.ffff"));
            }
        }

        public void PrintActionTimings()
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
                TestLog.WriteLine(actions[i].name + " : " + difference);
            }
            start = actions[0]._time;
            end = actions[actions.Count - 1]._time;
            difference = end.Subtract(start);
            TestLog.WriteLine("All Actions : " + difference);
            TestLog.End();
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