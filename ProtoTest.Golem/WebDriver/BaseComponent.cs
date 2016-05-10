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
        private Element _root;

        public BaseComponent()
        {
        }

        public BaseComponent(OpenQA.Selenium.By by)
        {
            this._root = new Element(by);
            this.@by = by;
        }

        public BaseComponent(Element element)
        {
            this._root = element;
        }

        public void SetRoot(Element element){
           this._root = element;
        }

        public override IWebElement GetElement()
        {
            _element = _root.GetElement();
            return _element;
        }
    }
}