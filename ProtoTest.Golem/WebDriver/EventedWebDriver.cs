using System;
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
            driver.FoundElement += driver_FoundElement;
            return driver;
        }

        private void driver_FoundElement(object sender, FoundElementEventArgs e)
        {
            if (Config.settings.runTimeSettings.HighlightFoundElements)
            {
                e.Element.Highlight();
            }
        }

        private void driver_FindElementCompleted(object sender, FindElementEventArgs e)
        {
//            TestContext.CurrentContext.IncrementAssertCount();
        }

        private void driver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            try
            {
                if (Config.settings.reportSettings.commandLogging)
                {
                    Log.Message(GetLogMessage("Typing", e, e.Element.GetAttribute("value")));
                }
                e.Element.Highlight(30, "red");
            }
            catch (Exception)
            {
            }
        }

        private void driver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Common.Delay(Config.settings.runTimeSettings.CommandDelayMs);
            if (Config.settings.reportSettings.commandLogging)
            {
                Log.Message(string.Format("Navigating to url {0}", e.Url));
            }
        }

        private void driver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
        }

        private void driver_FindingElement(object sender, FindElementEventArgs e)
        {
            Common.Delay(Config.settings.runTimeSettings.CommandDelayMs);
            if (Config.settings.reportSettings.commandLogging)
            {
                Log.Message(GetLogMessage("Finding", e));
            }
        }

        private void driver_ElementClicking(object sender, WebElementEventArgs e)
        {
            Common.Delay(Config.settings.runTimeSettings.CommandDelayMs);
            if (Config.settings.reportSettings.commandLogging)
            {
                Log.Message(GetLogMessage("Click", e));
            }
            
            if (e.Element == null)
            {
                throw new NoSuchElementException(string.Format("Element '{0}' not present, cannot click on it",
                    e.Element));
            }
            e.Element.Highlight(30, "red");
        }

        private string GetLogMessage(string command, WebElementEventArgs e = null, string param = "")
        {
            if (param != "") param = "'" + param + "'";

            if (TestBase.testData.lastElement.name != "Element")
            {
                return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command,
                    TestBase.testData.lastElement.name, TestBase.testData.lastElement.GetHtml(), param);
            }

            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command, "Element",
                TestBase.testData.lastElement.by, param);
        }

        private string GetLogMessage(string command, FindElementEventArgs e = null, string param = "")
        {
            if (param != "") param = "'" + param + "'";
            var name = Common.GetCurrentElementName();
            Log.Message(name);
            if (TestBase.testData.lastElement!=null && TestBase.testData.lastElement.name != "Element")
            {
                return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command,
                    TestBase.testData.lastElement.name, e.FindMethod, param);
            }

            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), command, "Element", e.FindMethod,
                param);
        }
    }
}