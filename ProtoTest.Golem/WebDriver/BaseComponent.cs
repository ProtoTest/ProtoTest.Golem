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
        }

        public BaseComponent(OpenQA.Selenium.By by)
        {
            this.root = new Element(by);
            this.@by = by;
        }

        public BaseComponent(Element element)
        {
            this.root = element;
        }
  

        public override IWebElement GetElement()
        {
            return this.root.GetElement();
        }
    }
}