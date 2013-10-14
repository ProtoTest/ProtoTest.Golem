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

        public string name;
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
            //if the element isn't stale, we can use our old reference
            if(!this._element.IsStale())
                return this._element;
            //if its stale, lets find all the elements that match
            var elements = driver.FindElements(this.by);
            //if we can't find any elements return null
            if (elements.Count == 0) return null;
            //if there are more than one element lets find the best one
            if (elements.Count > 1)
            {
                foreach (var ele in elements)
                {
                    if (ele.Displayed && ele.Enabled)
                        return ele;
                }
            }
            // return something at least
            return elements[0];

        }

        public Element(){}

        public Element(string name, By locator)
        {
            this.name = name;
            this.by = locator;
            Verify = new Verification(this,Config.Settings.runTimeSettings.ElementTimeoutSec,false);
            WaitUntil = new Verification(this,Config.Settings.runTimeSettings.ElementTimeoutSec,true);
        }

        public Element(By locator)
        {
            this.name = "";
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
                if (Config.Settings.runTimeSettings.HighlightOnVerify)
                    Highlight();
                return element.Text;
            }
            set { 
                IWebElement textField = element;
                textField.Clear();
                textField.SendKeys(value);
                if (Config.Settings.runTimeSettings.HighlightOnVerify)
                    Highlight();
            }
        }


        public IWebElement FindElement(By by)
        {
            element = GetElement();
            return element.FindElement(by);
        }

        
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return element.FindElements(by);
        }

        public void Clear()
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                Highlight();
            element.Clear();
        }

        /// <summary>
        ///     Clear a checked element (radio or checkbox)
        /// </summary>
        public void ClearChecked()
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                Highlight();
            element.ClearChecked();
        }

        public void Highlight()
        {
            element.Highlight();
        }

        public void Click()
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                Highlight();
            element.Click();
        }
        public void Submit()
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                Highlight();
            element.Submit();
        }
        public void SendKeys(string text)
        {
            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                Highlight();
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
            _element.ScrollIntoView();
            return this;
        }
       
   }
}
