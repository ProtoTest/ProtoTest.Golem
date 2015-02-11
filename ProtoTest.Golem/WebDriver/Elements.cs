using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace ProtoTest.Golem.WebDriver
{
    public class ElementCollection : IEnumerable<Element>
    {
        private By locator;

        private IEnumerable<Element> _elements;
        private IEnumerable<Element> elements
        {
            get
            {
                if (locator != null)
                {
                    var newList = new List<Element>();
                    var eles = WebDriverTestBase.driver.FindElements(locator);
                    foreach (var ele in eles)
                    {
                        newList.Add(new Element(ele));
                    }
                    _elements = newList;
                }
               
                return _elements;
            }
            set { _elements = value; }
        }

        public ElementCollection(IEnumerable<Element> elements)
        {
            this.elements = elements;
        }

        public ElementCollection(ReadOnlyCollection<Element> elements)
        {
            this.elements = elements;
        }

        public ElementCollection(By locator)
        {
            this.locator = locator;
            this.elements = new List<Element>();
        }

        public ElementCollection(ReadOnlyCollection<IWebElement> elements)
        {
            var eles = new List<Element>();
            foreach (var ele in elements)
            {
                eles.Add(new Element(ele));
            }
            this.elements = eles;
        }

        public IEnumerator<Element> GetEnumerator()
        {
            foreach (Element ele in this.elements)
            {
                yield return ele;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void ForEach(Action<Element> action)
        {
            this.ToList().ForEach(action);
        }
       
    }
}
