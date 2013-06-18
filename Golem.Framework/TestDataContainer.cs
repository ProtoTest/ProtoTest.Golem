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

        public string testName;
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

        public TestDataContainer(string name)
        {
            this.testName = name;
            actions = new ActionList();
            VerificationErrors = new List<VerificationError>();
        }

    }
}
