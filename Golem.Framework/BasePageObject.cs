using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using System.Diagnostics;


namespace Golem.Framework
{
    public abstract class BasePageObject 
    {
        private IWebDriver _driver;

        protected IWebDriver driver
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
        public string url;
        public string className;
        public string currentMethod;
        public BasePageObject()
        {
            driver = TestBaseClass.driver;
            className = this.GetType().Name;
            WaitForElements();
            //TestBaseClass.testData.LogEvent(Common.GetCurrentClassAndMethodName() + " Finished");
            TestBaseClass.testData.actions.addAction(Common.GetCurrentClassAndMethodName());
        }
        public abstract void WaitForElements();
     
    }
}
