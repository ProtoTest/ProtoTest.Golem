using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace Golem.Framework
{

    public class ActionList
    {
        public class Action
        {
            public string name;
            public DateTime _time;
            public Action(string name, DateTime time)
            {
                this.name = name;
                this._time = time;
            }
        }

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
            for(int i=1;i<actions.Count;i++)
            {
                start = actions[i-1]._time;
                end = actions[i]._time;
                difference = end.Subtract(start);
                TestLog.WriteLine(actions[i].name + " : " + difference.ToString());
            }
            start = actions[0]._time;
            end = actions[actions.Count-1]._time;
            difference = end.Subtract(start);
            TestLog.WriteLine("All Actions : " + difference.ToString());
            TestLog.End();
        }
    }

}