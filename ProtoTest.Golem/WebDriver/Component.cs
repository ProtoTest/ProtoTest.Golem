using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class Component : Element
    {
        private Element _root;

        public Component() : base()
        {
        }

        public Component(OpenQA.Selenium.By by) : base(by)  
        {
            this._root = new Element(by);
            this.@by = by;
        }

        public Component(Element root, OpenQA.Selenium.By by) : base(root, by)
        {
            this._root = root;
            this.@by = by;
        }

        public Component(Element element) : base(element)
        {
            this._root = element;
        }

        public void SetRoot(Element element)
        {
            _root = element;
        }

        public override IWebElement GetElement()
        {
            try
            {
                var text = _root.Text;
                var ele = _root.WaitForPresent(@by, timeoutSec);
                if (ele.GetType() == typeof(Element))
                {
                    Element element = (Element) ele;
                    ele = element.GetElement();
                }
                return ele;
            }
            catch (Exception e)
            {
                var message = string.Format("Could not find element '{0}' ({1}) after {2} seconds", name, @by,
                    timeoutSec);
                throw new NoSuchElementException(message);
            }
        }

    }
}