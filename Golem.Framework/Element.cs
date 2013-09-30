using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;

namespace Golem.Framework
{
    public class Element : IWebElement
    {
        protected By by;
        protected IWebDriver driver;
        public string name;
        protected IWebElement _element;
        protected IWebElement element
        {
            get
            {
                try
                {
                    if (this._element != null)
                        return _element;
                    this._element = GetElement();
                    if ((_element != null) && (Config.Settings.runTimeSettings.HighlightOnFind))
                    {
                        this._element.Highlight();
                    }
                    return this._element;
                }
                catch (Exception)
                {
                    this._element = GetElement();
                    return this._element;
                }
                
            }
            set
            {
                this._element = value;
            }
        }

        private IWebElement GetElement()
        {
            var elements = driver.FindElements(this.by);
            if (elements.Count == 0) return null;
            if (elements.Count > 1)
            {
                foreach (var ele in elements)
                {
                    if (ele.Displayed && ele.Enabled)
                        return ele;
                }
            }
            return elements[0];
        }

        public Element(){}

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
            set { 
                IWebElement textField = element;
                textField.Clear();
                textField.SendKeys(value);
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
            element.Clear();
        }

        public void Click()
        {
            WaitUntilPresent();
            element.Click();
        }
        public void Submit()
        {
            element.Submit();
        }
        public void SendKeys(string text)
        {
            WaitUntilPresent();
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
            if(element.Selected!=isChecked)
            {
                element.Click();
            }
            return this;
        }
        public Element WaitUntilPresent()
        {
            driver.WaitForPresent(this.by);
            return this;
        }
        public Element WaitUntilVisible()
        {
            driver.WaitForVisible(this.by);
            return this;
        }
        public Element WaitUntilNotPresent()
        {
            driver.WaitForNotPresent(this.by);
            element = null;
            return this;
        }
        public Element WaitUntilNotVisible()
        {
            driver.WaitForNotVisible(this.by);
            return this;
        }
        public Element VerifyPresent(int seconds=0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (element!=null)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    return this;
                }
                    
                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") not present after " + seconds + " seconds");
            return this;
        }

        public Element VerifyVisible(int seconds=0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (element!=null)
                {
                        if (this.Displayed)
                        {
                            TestContext.CurrentContext.IncrementAssertCount();
                            return this;
                        }  
                    
                }
                System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") not visible after " + seconds + " seconds");
            return this;
        }

        public Element VerifyNotPresent(int seconds = 0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (element==null)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    return this;
                }

                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") still present after " + seconds + " seconds");
            return this;
        }
        public Element VerifyNotVisible(int seconds = 0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                    if (!element.Displayed)
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        return this;
                    }

                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") still visible after " + seconds + " seconds");
            return this;
        }

        public Element VerifyText(string text, int seconds=0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (element!=null)
                {
                    if (element.Text.Contains(text))
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        return this;
                    }
                }
                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") did not have text : " + text + " after " + seconds + " seconds");
            return this;
        }

        public Element VerifyValue(string text, int seconds = 0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (element!=null)
                {
                    string value;

                    if ((value = element.GetAttribute("value")) != null)
                    {
                        if(value.Equals(text))
                        {
                            TestContext.CurrentContext.IncrementAssertCount();
                            return this;
                        }
                    }
                }
                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") did not have attribute value : " + text + " after " + seconds + " seconds");
            return this;
        }

        public Element VerifySelected(int seconds = 0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (element!=null)
                {
                    if (element.Selected)
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        return this;
                    }
                }
                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") was not selected after " + seconds + " seconds");
            return this;
        }

    }
}
