using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace Golem.Framework
{
    public class Element : IWebElement, IWrapsDriver, IWrapsElement 
    {
        public By by;
        public string name = "Element";
        public Verification Verify;
        public Verification WaitUntil;
        protected IWebDriver driver
        {
            get
            {
                return TestBaseClass.driver;
            }
            set
            {
                TestBaseClass.driver = value;
            }
        }

        protected IWebElement _element;
        protected IWebElement element
        {
            get
            {
                this._element = GetElement();
                return this._element;
            }
            set
            {
                this._element = value;
            }
        }

        private IWebElement GetElement()
        {
            try
            {
                TestBaseClass.testData.lastElement = this;
                if(this._element.IsStale())
                    this._element = driver.FindElement(this.by);
                return this._element;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public Element(){}

        public Element(IWebElement element)
        {
            this.element = element;
        }

        public Element(string name, By locator)
        {
            this.name = name;
            this.by = locator;
            Verify = new Verification(this,Config.Settings.runTimeSettings.ElementTimeoutSec,false);
            WaitUntil = new Verification(this,Config.Settings.runTimeSettings.ElementTimeoutSec,true);
            
        }

        public Element(By locator)
        {
            this.by = locator;
            Verify = new Verification(this, Config.Settings.runTimeSettings.ElementTimeoutSec, false);
            WaitUntil = new Verification(this, Config.Settings.runTimeSettings.ElementTimeoutSec, true);
        }

        public bool Displayed
        {
            get
            {
                if (element == null) return false;
                return element.Displayed;
            }
        }

        public bool Enabled
        {
            get
            {
                if (element == null) return false;
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
                if (element == null) return false;
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
                if (Config.Settings.runTimeSettings.HighlightOnVerify)
                    element.Highlight();
                return element.Text;
            }
            set { 
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
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.Clear();
        }

        /// <summary>
        ///     Clear a checked element (radio or checkbox)
        /// </summary>
        public void ClearChecked()
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.ClearChecked();
        }

        public void Highlight()
        {
            element.Highlight();
        }

        public void Click()
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.Click();
        }
        public void Submit()
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                element.Highlight();
            element.Submit();
        }
        public void SendKeys(string text)
        {
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
            catch (Exception e)
            {
                return "";
            }            
            
        }
        public string GetCssValue(string text)
        {
            return element.GetCssValue(text);
        }

        public Element SetCheckbox(bool isChecked)
        {
            if (element.Selected != isChecked)
            {
                element.Click();
            }
            return this;
        }

        public IWebDriver WrappedDriver
        {
            get
            {
                return this.driver;
            }
            private set
            {
                this.driver = value;
            }
        }

        public IWebElement WrappedElement
        {
            get
            {
                return this._element;
            }
            private set
            {
                this._element = value;
            }
        }

        public Element ScrollIntoView()
        {
            element.ScrollIntoView();
            return this;
        }
       
   }
}
