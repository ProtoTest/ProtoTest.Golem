using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements.Images;

namespace ProtoTest.Golem.WebDriver
{
  
    public class Component : Element
    {
        public Element RootElement;
        public By by;
        public Component(Element RootElement, By by) : base(by)
        {
            this.RootElement = RootElement;
        }
        public Component(Element RootElement, string name, By by) : base(name,by)
        {
            this.RootElement = RootElement;
        }
        protected override IWebElement GetElement()
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
                    return RootElement.WaitForPresent(@by,timeoutSec);
                }
                return _element;
            }
            catch (Exception e)
            {
                string message = string.Format("Could not find element '{0}' ({1}) after {2} seconds", name, @by,timeoutSec);
                throw new NoSuchElementException(message);
            }
        }
    }
}