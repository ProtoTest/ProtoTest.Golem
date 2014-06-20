using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements.Images;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// Provides a simplified API to the IWebELement.  Can be instantiated in a class header.  
    /// Will automatically find the IWebElement each time it is used, not when it is instantiated.  
    /// </summary>
    public class Element : IWebElement, IWrapsDriver, IWrapsElement
    {
        protected Element _frame;
        protected IWebElement _element;
        protected ElementImages _images;
        public By by;
        public string name = "Element";
        public string pageObjectName = "";
        public int timeoutSec;

        protected IWebDriver driver
        {
            get { return TestBase.testData.driver; }
            set { TestBase.testData.driver = value; }
        }

        public Element()
        {
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        /// Construct an element using an existing element
        /// </summary>
        /// <param name="element"></param>
        public Element(IWebElement element): base()
        {
            this.element = element;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
        }

 

        /// <summary>
        /// Construct an element
        /// </summary>
        /// <param name="name">Human readable name of the element</param>
        /// <param name="locator">By locator</param>
        public Element(string name, By locator)
        {
            this.name = name;
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        /// Construct an element
        /// </summary>
        /// <param name="locator">By locator</param>
        public Element(By locator): base()
        {
            this.name = "Element";
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        /// Construct an element within an iframe
        /// </summary>
        /// <param name="name">Human readable name of the element</param>
        /// <param name="locator">By locator</param>
        public Element(string name, By locator, Element frame): base()
        {
            this._frame = frame;
            this.name = name;
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        /// Construct an element
        /// </summary>
        /// <param name="locator">By locator</param>
        public Element(By locator, Element frame): base()
        {
            this._frame = frame;
            this.name = "Element";
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
        }

        public ElementImages Images
        {
            get { return _images ?? (_images = new ElementImages(this)); }
        }

        protected IWebElement element
        {
            get
            {
                _element = GetElement();
                return _element;
            }
            set { _element = value; }
        }

        /// <summary>
        /// Is the element present on the page, but not necesarily displayed and visible?
        /// </summary>
        public bool Present
        {
            get
            {
                try
                {
                    return element.Enabled;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Is the element present and displayed on the page?
        /// </summary>
        public bool Displayed
        {
            get
            {
                try
                {
                    if (!Present) return false;
                    return element.Displayed;
                }
                catch (Exception e)
                {
                    return false;
                }
                
            }
        }

        /// <summary>
        /// Is the element present on the page and able to be interacted with?
        /// </summary>
        public bool Enabled
        {
            get
            {
                if (!Present) return false;
                return element.Enabled;
            }
        }

        /// <summary>
        /// Get the upper-left (x,y) coordinates of the element relative to the upper-left corner of the page.
        /// </summary>
        public Point Location
        {
            get { return element.Location; }
        }

        /// <summary>
        /// Checks if the element is selected on the page.
        /// </summary>
        public bool Selected
        {
            get
            {
                if (!Present) return false;
                return element.Selected;
            }
        }

        /// <summary>
        /// Return an object containing the size of the element (height, width).
        /// </summary>
        public Size Size
        {
            get { return element.Size; }
        }

        /// <summary>
        /// Return the tag name of the element.
        /// </summary>
        public string TagName
        {
            get { return element.TagName; }
        }

        /// <summary>
        /// Property to get and set the Text for the element.
        /// </summary>
        public string Text
        {
            get
            {
                return element.Text;
            }
            set
            {
                element.Clear();
                element.SendKeys(value);
            }
        }

        /// <summary>
        /// Returns the first element found by the locator.
        /// </summary>
        /// <param name="by">The locator to use.</param>
        /// <returns>The IWebElement found.</returns>
        public IWebElement FindElement(By by)
        {
            return element.FindElement(by);
        }

        /// <summary>
        /// Return a collection of elements found by the locator.
        /// </summary>
        /// <param name="by">The locator to use.</param>
        /// <returns>Collection of IWebElements found.</returns>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return element.FindElements(by);
        }

        /// <summary>
        /// Clears the contents of the element\\.
        /// </summary>
        public void Clear()
        {
            element.Clear();
        }

        /// <summary>
        /// Click the element and optionally highlights the element if set in the application configuration settings.
        /// </summary>
        public void Click()
        {
            element.Click();
        }

        /// <summary>
        /// Submit this element to the web server and optionally highlights the element if set in the application configuration settings.
        /// </summary>
        public void Submit()
        {
            element.Submit();
        }

        /// <summary>
        /// Simulates typing text into the element and optionally highlights the element if set in the application configuration settings.
        /// </summary>
        /// <param name="text">Text to send</param>
        public void SendKeys(string text)
        {
            element.SendKeys(text);
        }

        /// <summary>
        /// Get the value of the requested attribute for the element
        /// </summary>
        /// <param name="attribute">The attribute name</param>
        /// <returns></returns>
        public string GetAttribute(string attribute)
        {
            try
            {
                return element.GetAttribute(attribute);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Get the value of a CSS property for the element
        /// </summary>
        /// <param name="propertyName">The CSS property name</param>
        /// <returns></returns>
        public string GetCssValue(string propertyName)
        {
            return element.GetCssValue(propertyName);
        }

        public IWebDriver WrappedDriver
        {
            get { return driver; }
            private set { driver = value; }
        }

        public IWebElement WrappedElement
        {
            get { return element; }
            private set { element = value; }
        }

        /// <summary>
        /// Create an element verification for some condition.  
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify()
        {
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this, this.timeoutSec, false);
        }

        /// <summary>
        /// Wait for some condition on the element
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil(int timeoutSec)
        {
            this.timeoutSec = timeoutSec;
            return new ElementVerification(this, this.timeoutSec, true);
        }

        /// <summary>
        /// Create an element verification for some condition
        /// </summary>
        /// <param name="timeoutSec"> timeout that overrides the default timeout set in the configuration settings class or App.config file</param>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify(int timeoutSec)
        {
            this.timeoutSec = timeoutSec;
            return new ElementVerification(this, this.timeoutSec, false);
        }

        /// <summary>
        /// Wait for some condition on the element
        /// </summary>
        /// <param name="timeoutSec">Optional timeout that overrides the default timeout set in the configuration settings class or App.config file</param>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil()
        {
            this.timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this, this.timeoutSec, true);
        }

        private IWebElement GetElement()
        {
            try
            {
                TestBase.testData.lastElement = this;
                if (_element.IsStale())
                {
                    if (this._frame != null)
                    {
                        driver.SwitchTo().Frame(_frame.WrappedElement);
                        TestBase.testData.lastElement = this;
                    }
                    else
                    {
                        driver.SwitchTo().DefaultContent();
                    }
                    return driver.WaitForPresent(@by,timeoutSec);
                }
                return _element;
            }
            catch (Exception e)
            {
                string message = string.Format("Could not find element '{0}' ({1}) after {2} seconds", name, @by,timeoutSec);
                throw new NoSuchElementException(message);
            }
        }

        /// <summary>
        ///  Clear a checked element (radio or checkbox)
        /// </summary>
        public void ClearChecked()
        {
            element.ClearChecked();
        }

        /// <summary>
        /// Highlight the element on the page
        /// </summary>
        public void Highlight()
        {
            element.Highlight();
        }

        /// <summary>
        /// Set the checkbox element
        /// </summary>
        /// <param name="isChecked">if true, check it; if false, uncheck it</param>
        /// <returns>The element reference</returns>
        public Element SetCheckbox(bool isChecked)
        {
            if (element.Selected != isChecked)
            {
                element.Click();
            }
            return this;
        }

        /// <summary>
        /// If there are multiple elements that can be found using the same locator,
        /// find one that is displayed and enabled. 
        /// </summary>
        /// <returns>The element found</returns>
        public Element GetVisibleElement()
        {
            element = driver.FindVisibleElement(@by);
            return this;
        }

        /// <summary>
        /// Move the mouse over the element
        /// </summary>
        public void MouseOver()
        {
            element.MouseOver();
        }
    }
}