using System;
using Gallio.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class EventedWebDriver
    {
        private const string errorMessage = "{0}: {1} '{2}' ({3}) {4}";
        public EventFiringWebDriver driver;

        public EventedWebDriver(IWebDriver driver)
        {
            this.driver = new EventFiringWebDriver(driver);
            RegisterEvents();
        }

        public IWebDriver RegisterEvents()
        {
            driver.ElementClicking += driver_ElementClicking;
            driver.ExceptionThrown += driver_ExceptionThrown;
            driver.FindingElement += driver_FindingElement;
            driver.Navigating += driver_Navigating;
            driver.ElementValueChanged += driver_ElementValueChanged;
            driver.FindElementCompleted += driver_FindElementCompleted;
            return driver;
        }


        private void driver_FindElementCompleted(object sender, FindElementEventArgs e)
        {
            TestContext.CurrentContext.IncrementAssertCount();
        }

        private void driver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            try
            {
                TestBase.LogEvent(GetLogMessage("Typing", e, e.Element.GetAttribute("value")));
            }
            catch (Exception)
            {
            }
        }


        private void driver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Common.Delay(Config.Settings.runTimeSettings.CommandDelayMs);
            TestBase.LogEvent(string.Format("Navigating to url {0}", e.Url));
        }


        private void driver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            // Do not want to flood the log with these element not found exceptions. 
            // Especially with the ability to wait for elements to display within a timeout period. 
            // Turn it on if you really want them there.

            //Common.Log("Exception: " + e.ThrownException.ToString());
        }

        private void driver_FindingElement(object sender, FindElementEventArgs e)
        {
            Common.Delay(Config.Settings.runTimeSettings.CommandDelayMs);
            TestBase.LogEvent(GetLogMessage("Finding", e));
        }

        private void driver_ElementClicking(object sender, WebElementEventArgs e)
        {
            Common.Delay(Config.Settings.runTimeSettings.CommandDelayMs);
            TestBase.LogEvent(GetLogMessage("Click", e));
            if (e.Element == null)
            {
                throw new NoSuchElementException(string.Format("Element '{0}' not present, cannot click on it",
                    e.Element));
            }
        }

        private string GetLogMessage(string command, WebElementEventArgs e = null, string param = "")
        {
            if (param != "") param = "'" + param + "'";

            if (TestBase.testData.lastElement.name != "")
            {
                return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command,
                    TestBase.testData.lastElement.name, TestBase.testData.lastElement.by, param);
            }

            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command, "Element", e.Element,
                param);
        }

        private string GetLogMessage(string command, FindElementEventArgs e = null, string param = "")
        {
            if (param != "") param = "'" + param + "'";

            if (TestBase.testData.lastElement.name != "")
            {
                return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command,
                    TestBase.testData.lastElement.name, TestBase.testData.lastElement.by, param);
            }

            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command, "Element",
                e.Element.Location,
                param);
        }
    }
}