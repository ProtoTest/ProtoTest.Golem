using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium;

namespace Golem
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

            return driver;
        }

        void driver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            Common.Log(Common.GetCurrentClassAndMethodName() + ": Typing : " + e.Element.Text);
        }

        void driver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            TestBaseClass.FireEvent("Navigating to url " + e.Url);
        }



        void driver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            //Common.Log("Exception: " + e.ThrownException.ToString());
        }

        void driver_FindingElement(object sender, FindElementEventArgs e)
        {
            Common.Log(Common.GetCurrentClassAndMethodName() + ": Finding Element : " + e.FindMethod);
        }



        void driver_ElementClicking(object sender, WebElementEventArgs e)
        {
            Common.Log(Common.GetCurrentClassAndMethodName() + ": Clicking Element");
        }

    }
}
