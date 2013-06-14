using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using Gallio.Framework;
using Gallio.Model;
using Gallio.Common.Media;
using Golem.Framework;

namespace Golem.Framework
{
    public class TestDataContainer
    {
        #region Events
        public event ActionEvent beforeTestEvent;
        public event ActionEvent afterTestEvent;
        public event ActionEvent pageObjectActionEvent;
        public event ActionEvent beforeCommandEvent;
        public event ActionEvent afterCommandEvent;
        public event ActionEvent beforeSuiteEvent;
        public event ActionEvent afterSuiteEvent;
        public event ActionEvent genericEvent;

        public delegate void ActionEvent(string name, EventArgs e);

        public void FireEvent(string name)
        {
            EventArgs e = null;
            genericEvent(name, e);
        }

        public class GolemEventArgs : EventArgs
        {
            public string name;
            public DateTime time;
        }
        #endregion

        public string testName;
        public VerificationError[] verificationErrors;
        public ScreenRecorder recorder;
        public List<VerificationError> VerificationErrors;
        public ActionList actions;
        private IWebDriver _driver;
        public IWebDriver driver
        {
            get
            {
                return _driver;
            }
            set
            {
                _driver = value;
            }

        }
        public int numVerificationErrors
        {
            get
            {
                return VerificationErrors.Count;
            }
        }

        public TestDataContainer(string name)
        {
            this.testName = name;
            actions = new ActionList();
            VerificationErrors = new List<VerificationError>();
            pageObjectActionEvent += new ActionEvent(LogAction);
            beforeTestEvent += new ActionEvent(WriteActionToLog);
            afterTestEvent += new ActionEvent(WriteActionToLog);
            genericEvent += new ActionEvent(WriteActionToLog);
        }

        private void WriteActionToLog(string name, EventArgs e)
        {
            Common.Log("(" + DateTime.Now.ToString("HH:mm:ss::ffff") + ") : " + name);
        }
        private void LogAction(string name, EventArgs e)
        {
            actions.addAction(name);
        }
  
    }
}
