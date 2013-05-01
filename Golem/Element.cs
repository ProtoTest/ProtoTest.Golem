using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;

namespace Golem
{
    public class Element : IWebElement
    {
        private By by;
        private IWebDriver driver;
        public string name;

        private IWebElement _element;
        private IWebElement element
        {
            get
            {
                this._element = driver.FindElement(by);
                return this._element;
            }
            set
            {
                this._element = value;
            }
        }

        public Element(string name, By locator)
        {
            this.name = name;
            this.driver = TestBaseClass.driver;
            this.by = locator;
        }

        public bool Displayed
        {
            get
            {
                return element.Displayed;
            }
        }

        public bool Enabled
        {
            get
            {
                return element.Enabled;
            }
        }

        public System.Drawing.Point Location
        {
            get
            {
                return element.Location;
            }
        }
        public bool Selected
        {
            get
            {
                return element.Selected;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return element.Size;
            }
        }
        public string TagName
        {
            get
            {
                return element.TagName;
            }
        }
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
            element.Clear();
        }

        public void Click()
        {
            element.Click();
        }
        public void Submit()
        {
            element.Submit();
        }
        public void SendKeys(string text)
        {
            element.SendKeys(text);
        }
        public string GetAttribute(string attribute)
        {
            return element.GetAttribute(attribute);
        }
        public string GetCssValue(string text)
        {
            return element.GetCssValue(text);
        }

        public IWebElement WaitForVisible()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(by));
            return _element;
        }
        public IWebElement WaitForPresent()
        {
            return driver.WaitForElement(by);
        }



    }
}
