using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;


namespace Golem
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

        public BasePageObject()
        {
            TestBaseClass.FireEvent(Common.GetCurrentClassAndMethodName());
            this.driver = TestBaseClass.driver;
            string name = Common.GetCurrentMethodName();
            WaitForElements();
            ValidateLoaded();
            TestBaseClass.FireEvent(name + " Finished");
        }

        public abstract void ValidateLoaded();

        public abstract void WaitForElements();

    }
}
