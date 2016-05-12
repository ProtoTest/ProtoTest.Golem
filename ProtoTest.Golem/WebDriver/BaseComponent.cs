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
        }

        public BaseComponent(By by) 
        {
            this.root = new Element(by);
            this.@by = by;
            this.name = TestBase.GetCurrentClassName();
        }

        public BaseComponent(Element element) 
        {
            this.root = element;
            this.name = TestBase.GetCurrentClassName();
        }

        public BaseComponent(By by, Element frame) 
        {
            this.root = new Element(by, frame);
            this.frame = frame;
            this.@by = by;
            this.name = TestBase.GetCurrentClassName();
        }

        public override IWebElement GetElement()
        {
            TestBase.testData.lastElement = this;
            var element2 = this.root.GetElement();
            return element2;
          
        }
    }
}