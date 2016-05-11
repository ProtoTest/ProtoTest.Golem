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

        public BaseComponent() : base()
        {
        }

        public BaseComponent(By by) : base(by)
        {
            this.root = new Element(by);
            this.@by = by;
        }

        public BaseComponent(Element element) : base(element)
        {
            this.root = element;
        }

        public BaseComponent(By by, Element frame) : base(by, frame)
        {
            this.root = new Element(by, frame);
            this.frame = frame;
            this.@by = by;
        }

        public override IWebElement GetElement()
        {
            var element2 = this.root.GetElement();
            return element2;
          
        }
    }
}