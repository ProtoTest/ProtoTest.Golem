using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Framework;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium;

namespace Golem.Framework
{
    public class EventedWebDriver
    {
        private const string errorMessage = "{0}: {1} '{2}' ({3}) {4}";//function: command name(by) params
        public EventFiringWebDriver driver;
        public EventedWebDriver(IWebDriver driver)
        {
            this.driver = new EventFiringWebDriver(driver);
            RegisterEvents();
        }

        public IWebDriver RegisterEvents()
        {
            driver.ElementClicking += new EventHandler<WebElementEventArgs>(driver_ElementClicking);
            driver.ExceptionThrown += new EventHandler<WebDriverExceptionEventArgs>(driver_ExceptionThrown);
            driver.FindingElement += new EventHandler<FindElementEventArgs>(driver_FindingElement);
            driver.Navigating += new EventHandler<WebDriverNavigationEventArgs>(driver_Navigating);
            driver.ElementValueChanged += new EventHandler<WebElementEventArgs>(driver_ElementValueChanged);
            driver.FindElementCompleted += new EventHandler<FindElementEventArgs>(driver_FindElementCompleted);
            return driver;
        }

        void driver_FindElementCompleted(object sender, FindElementEventArgs e)
        {
            TestContext.CurrentContext.IncrementAssertCount();


        }

        void driver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            try
            {
                TestBaseClass.LogEvent(GetLogMessage("Typing", e.Element.GetAttribute("value")));
            }
            catch (Exception)
            {
     
            }
            
        }

        

        void driver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Common.Delay(Config.Settings.runTimeSettings.CommandDelayMs);
            TestBaseClass.LogEvent(string.Format("Navigating to url {0}",e.Url));
        }



        void driver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            //Common.Log("Exception: " + e.ThrownException.ToString());
        }

        void driver_FindingElement(object sender, FindElementEventArgs e)
        {
            Common.Delay(Config.Settings.runTimeSettings.CommandDelayMs);
            TestBaseClass.LogEvent(GetLogMessage("Finding"));
        }

        void driver_ElementClicking(object sender, WebElementEventArgs e)
        {
            Common.Delay(Config.Settings.runTimeSettings.CommandDelayMs);
            TestBaseClass.LogEvent(GetLogMessage("Click"));
        }
        private string GetLogMessage(string command, string param = "")
        {
            if (param != "") param = "'" + param + "'";
            return string.Format(errorMessage, Common.GetCurrentClassAndMethodName(), command,
                TestBaseClass.testData.lastElement.name, TestBaseClass.testData.lastElement.by, param);
        }
    }
}
