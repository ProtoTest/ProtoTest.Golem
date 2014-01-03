using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements.Images;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// Provides a simplified API to the IWebELement.  Can be instantiated in a class header.  Will automatically find the IWebElement each time it is used, not when it is instantiated.  
    /// </summary>
    public class Element : IWebElement, IWrapsDriver, IWrapsElement
    {
        protected IWebElement _element;
        private ElementImages _images;
        public By by;
        public string name = "Element";

        protected IWebDriver driver
        {
            get { return TestBase.testData.driver; }
            set { TestBase.testData.driver = value; }
        }

        public Element()
        {
           // this.driver = WebDriverTestBase.driver;
        }

        public Element(IWebElement element)
        {
            this.element = element;
           // this.driver = ((IWrapsDriver) element).WrappedDriver;
        }

        public Element(string name, By locator)
        {
            this.name = name;
            this.by = locator;
            //this.driver = this.driver = WebDriverTestBase.driver;
        }

        public Element(string name, By locator, IWebDriver driver)
        {
          //  this.driver = driver;
            this.name = name;
            this.by = locator;
        }

        public Element(By locator)
        {
            by = locator;
           // this.driver = WebDriverTestBase.driver;
        }

        public Element(By locator, IWebDriver driver)
        {
            by = locator;
           // this.driver = driver;
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

        public bool Displayed
        {
            get
            {
                if (!Present) return false;
                return element.Displayed;
            }
        }

        public bool Enabled
        {
            get
            {
                if (!Present) return false;
                return element.Enabled;
            }
        }

        public Point Location
        {
            get { return element.Location; }
        }

        public bool Selected
        {
            get
            {
                if (!Present) return false;
                return element.Selected;
            }
        }

        public Size Size
        {
            get { return element.Size; }
        }

        public string TagName
        {
            get { return element.TagName; }
        }

        public string Text
        {
            get
            {
                if (!Present)
                    throw new NoSuchElementException(string.Format("No Such Element '{0}' ({1}) ", name, @by));
                if (Config.Settings.runTimeSettings.HighlightOnVerify)
                    element.Highlight();
                return element.Text;
            }
            set
            {
                if (!Present)
                    throw new NoSuchElementException(string.Format("No Such Element '{0}' ({1}) ", name, @by));
                element.Clear();
                element.SendKeys(value);
                if (Config.Settings.runTimeSettings.HighlightOnVerify)
                    element.Highlight();
            }
        }


        public IWebElement FindElement(By by)
        {
            return element.FindElement(by);
        }


        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return element.FindElements(by);
        }

        public void Clear()
        {
            if (!Present)
                throw new NoSuchElementException(string.Format("No Such Element '{0}' ({1}) ", name, @by));
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.Clear();
        }

        public void Click()
        {
            if (!Present)
                throw new NoSuchElementException(string.Format("No Such Element '{0}' ({1}) ", name, @by));
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.Click();
        }

        public void Submit()
        {
            if (!Present)
                throw new NoSuchElementException(string.Format("No Such Element '{0}' ({1}) ", name, @by));
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.Submit();
        }

        public void SendKeys(string text)
        {
            if (!Present)
                throw new NoSuchElementException(string.Format("No Such Element '{0}' ({1}) ", name, @by));
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.SendKeys(text);
        }

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

        public string GetCssValue(string text)
        {
            return element.GetCssValue(text);
        }

        public IWebDriver WrappedDriver
        {
            get { return driver; }
            private set { driver = value; }
        }

        public IWebElement WrappedElement
        {
            get { return _element; }
            private set { _element = value; }
        }

        public ElementVerification Verify(int timeoutSec = -1)
        {
            if (timeoutSec == -1) timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this, timeoutSec, false);
        }

        public ElementVerification WaitUntil(int timeoutSec = -1)
        {
            if (timeoutSec == -1) timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this, timeoutSec, true);
        }

        private IWebElement GetElement()
        {
            try
            {
                TestBase.testData.lastElement = this;
                if (_element.IsStale())
                    _element = driver.FindElement(@by);
                return _element;
            }
            catch (Exception)
            {
                string message = string.Format("Could not locate element '{0}' ({1})", name, @by);
                throw new NoSuchElementException(message);
            }
        }

        /// <summary>
        ///     Clear a checked element (radio or checkbox)
        /// </summary>
        public void ClearChecked()
        {
            if (!Present)
                throw new NoSuchElementException("No Such Element '{0}' ({1}) ");
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.ClearChecked();
        }

        public void Highlight()
        {
            if (!Present)
                throw new NoSuchElementException("No Such Element '{0}' ({1}) ");
            element.Highlight();
        }

        public Element SetCheckbox(bool isChecked)
        {
            if (element.Selected != isChecked)
            {
                element.Click();
            }
            return this;
        }

        public Element FindVisibleElement()
        {
            element = driver.FindVisibleElement(@by);
            return this;
        }

        public void MouseOver()
        {
            element.MouseOver();
        }
    }
}