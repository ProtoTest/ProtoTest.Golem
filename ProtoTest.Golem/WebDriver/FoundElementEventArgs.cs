// Type: OpenQA.Selenium.Support.Events.FindElementEventArgs
// Assembly: WebDriver.Support, Version=2.40.0.0, Culture=neutral
// MVID: 9FAA975A-389C-466A-AE2E-96ABC7996728
// Assembly location: C:\Users\Brian\Documents\GitHub\ProtoTest.Golem\ProtoTest.Golem\packages\Selenium.Support.2.40.0\lib\net40\WebDriver.Support.dll

using OpenQA.Selenium;
using System;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// Provides data for events related to finding elements.
    /// 
    /// </summary>
    public class FoundElementEventArgs : EventArgs
    {
        private IWebDriver driver;
        private IWebElement element;
        private By method;

        /// <summary>
        /// Gets the WebDriver instance used in finding elements.
        /// 
        /// </summary>
        public IWebDriver Driver
        {
            get
            {
                return this.driver;
            }
        }

        /// <summary>
        /// Gets the element that was found
        /// 
        /// </summary>
        public IWebElement Element
        {
            get
            {
                return this.element;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:OpenQA.Selenium.By"/> object containing the method used to find elements.
        /// 
        /// </summary>
        public By FindMethod
        {
            get
            {
                return this.method;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.Support.Events.FindElementEventArgs"/> class.
        /// 
        /// </summary>
        /// <param name="driver">The WebDriver instance used in finding elements.</param><param name="element">The element that was found</param><param name="method">The <see cref="T:OpenQA.Selenium.By"/> object containing the method used to find elements.</param>
        public FoundElementEventArgs(IWebDriver driver, IWebElement element, By method)
        {
            this.driver = driver;
            this.element = element;
            this.method = method;
        }
    }
}
