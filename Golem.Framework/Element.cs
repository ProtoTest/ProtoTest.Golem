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
            set { 
                IWebElement textField = element;
                textField.Clear();
                textField.SendKeys(value);
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
            WaitUntilPresent();
            element.Click();
        }
        public void Submit()
        {
            element.Submit();
        }
        public void SendKeys(string text)
        {
            WaitUntilVisible();
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
                var eles = driver.FindElements(this.by);
                if (eles.Count != 0)
                {
                    foreach (var ele in eles)
                    {
                        if (ele.Displayed)
                        {
                            TestContext.CurrentContext.IncrementAssertCount();
                            return this;
                        }  
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

        public Element VerifyText(string text, int seconds=0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (driver.FindElements(this.by).Count != 0)
                {
                    if (driver.FindElement(this.by).Text.Contains(text))
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
                if (driver.FindElements(this.by).Count != 0)
                {
                    string value;

                    if ((value = driver.FindElement(this.by).GetAttribute("value")) != null)
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
                if (driver.FindElements(this.by).Count != 0)
                {
                    if (driver.FindElement(this.by).Selected)
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
