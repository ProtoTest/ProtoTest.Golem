using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public abstract class BaseComponent : Element
    {
        public Element root { get; set; }

        public BaseComponent()
        {
            this.name = TestBase.GetCurrentClassName();
            this.WaitForElements();
        }

        public BaseComponent(By by) 
        {
            this.root = new Element(by);
            this.@by = by;
            this.name = TestBase.GetCurrentClassName();
            this.WaitForElements();
        }

        public BaseComponent(Element element) 
        {
            this.root = element;
            this.name = TestBase.GetCurrentClassName();
            this.WaitForElements();
        }

        public BaseComponent(By by, Element frame) 
        {
            this.root = new Element(by, frame);
            this.frame = frame;
            this.@by = by;
            this.name = TestBase.GetCurrentClassName();
            this.WaitForElements();
        }

        public BaseComponent(BaseComponent component, By by, Element frame)
        {
            this.root = new Element(component, by, frame);
            this.frame = frame;
            this.@by = by;
            this.name = TestBase.GetCurrentClassName();
            this.WaitForElements();
        }

        public BaseComponent(BaseComponent component, By by)
        {
            this.root = new Element(component, by);
            this.@by = by;
            this.name = TestBase.GetCurrentClassName();
            this.WaitForElements();
        }

        public override IWebElement GetElement()
        {
            IWebElement element2;
            TestBase.testData.lastElement = this;
            if (this.root == null)
            {
                element2 = base.GetElement();
            }
            else
            {
                element2 = this.root.GetElement();
            }
            
            return element2;
          
        }

        /// <summary>
        ///     Create an element verification for some condition.
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify()
        {
            timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this, timeoutSec, false);
        }

        /// <summary>
        ///     Wait for some condition on the element
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil(int timeoutSec)
        {
            this.timeoutSec = timeoutSec;
            return new ElementVerification(this, this.timeoutSec, true);
        }

        public virtual void WaitForElements()
        {
            
        }

    }
}