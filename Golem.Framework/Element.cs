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
        private By by;
        private IWebDriver driver;
        public string name;

        private IWebElement _element;
        private IWebElement element
        {
            get
            {
      
                    this._element = this.driver.FindElement(by);
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
                var _element = element;
                _element.Clear();
                _element.SendKeys(value);
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
            driver.WaitForPresent(this.by);
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
                if (driver.FindElements(this.by).Count != 0)
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
                if (driver.FindElements(this.by).Count != 0)
                {
                    if (driver.FindElement(this.by).Displayed)
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        return this;   
                    }
                }
                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") not visible after " + seconds + " seconds");
            return this;
        }

        public Element VerifyNotPresent(int seconds = 0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (driver.FindElements(this.by).Count == 0)
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
                if (driver.FindElements(this.by).Count != 0)
                {
                    if (!driver.FindElement(this.by).Displayed)
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        return this;
                    }
                }
                else
                    System.Threading.Thread.Sleep(1000);
            }
            Golem.Framework.TestBaseClass.AddVerificationError(Common.GetCurrentClassAndMethodName() + ": Element : " + this.name + " (" + this.by + ") still visible after " + seconds + " seconds");
            return this;
        }

    }
}
