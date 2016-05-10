using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class ElementCollection : IEnumerable<Element>
    {
        private readonly OpenQA.Selenium.By locator;
        private IEnumerable<Element> _elements;

        public ElementCollection()
        {
        }

        public ElementCollection(IEnumerable<Element> elements)
        {
            this.elements = elements;
        }

        public ElementCollection(ReadOnlyCollection<Element> elements)
        {
            this.elements = elements;
        }

        public ElementCollection(OpenQA.Selenium.By locator)
        {
            this.locator = locator;
            elements = new List<Element>();
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

        public ElementCollection WaitForPresent(int timeoutSec)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    if (elements.ToList().Count > 0)
                        return this;
                    Common.Delay(1000);
                }
                catch (Exception e)
                {
                }
            }
            return this;
        }

        public ElementCollection WaitForNotPresent(int timeoutSec)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    if (elements.ToList().Count == 0)
                        return this;
                    Common.Delay(1000);
                }
                catch (Exception e)
                {
                }
            }
            return this;
        }

        public ElementCollection WaitForVisible(int timeoutSec)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    if (elements.Count(x=>x.Displayed)>0)
                        return this;
                    Common.Delay(1000);
                }
                catch (Exception e)
                {
                }
            }
            return this;
        }

        public ElementCollection WaitForNotVisible(int timeoutSec)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    if (elements.Count(x => x.Displayed) == 0)
                        return this;
                    Common.Delay(1000);
                }
                catch (Exception e)
                {
                }
            }
            return this;
        }

        protected IEnumerable<Element> elements
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

        public IEnumerator<Element> GetEnumerator()
        {
            foreach (var ele in elements)
            {
                yield return ele;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ForEach(Action<Element> action)
        {
            this.ToList().ForEach(action);
        }
    }
}