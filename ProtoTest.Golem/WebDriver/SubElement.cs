using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class SubElement : Element
    {
        protected Element root;

        public SubElement() : base()
        {
        }

        public SubElement(OpenQA.Selenium.By by) : base(by)
        {
            this.@by = by;
        }

        public SubElement(BaseComponent parent, OpenQA.Selenium.By by) : base(by)
        {
            this.root = parent.root;
            this.@by = by;
        }

        public SubElement(Element element) : base(element)
        {
            this.root = element;
        }

 
        public override IWebElement GetElement()
        {
            try
            {
                var ele = this.root.WaitForPresent(@by, timeoutSec);
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