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
            TestBaseClass.testData.FireEvent(Common.GetCurrentClassAndMethodName() + ": Typing : " + e.Element.GetAttribute("value"));
        }

        void driver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Thread.Sleep(Config.Settings.runTimeSettings.commandDelayMs);
            TestBaseClass.testData.FireEvent(Common.GetCurrentClassAndMethodName() + ": Navigating to url " + e.Url);
        }



        void driver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            //Common.Log("Exception: " + e.ThrownException.ToString());
        }

        void driver_FindingElement(object sender, FindElementEventArgs e)
        {
            Thread.Sleep(Config.Settings.runTimeSettings.commandDelayMs);
            TestBaseClass.testData.FireEvent(Common.GetCurrentClassAndMethodName() + ": Looking for Element : " + e.FindMethod);
        }

        void driver_ElementClicking(object sender, WebElementEventArgs e)
        {
            Thread.Sleep(Config.Settings.runTimeSettings.commandDelayMs);
            TestBaseClass.testData.FireEvent(Common.GetCurrentClassAndMethodName() + ": Clicking Element");
        }

    }
}
