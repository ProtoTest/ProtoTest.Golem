using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class Components<T> : Component, IEnumerable<T> where T : BaseComponent, new()
    {
        private ElementCollection RootElements;
        public Components(OpenQA.Selenium.By @by) : base(by)
        {
            RootElements = new ElementCollection(by);
        }

       public IEnumerator<T> GetEnumerator()
        {
            foreach (var ele in RootElements)
            {
                var el = new T();
                el.SetRoot(ele);
                yield return el;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

//        public override IWebElement GetElement()
//        {
//            try
//            {
//                TestBase.testData.lastElement = this;
//                if (_element.IsStale())
//                {
//                    if (_frame != null)
//                    {
//                        driver.SwitchTo().Frame(_frame.WrappedElement);
//                        TestBase.testData.lastElement = this;
//                    }
//                    else
//                    {
//                        driver.SwitchTo().DefaultContent();
//                    }
//                    _element = RootElement.WaitForPresent(@by, timeoutSec);
//                }
//                return _element;
//            }
//            catch (NoSuchElementException e)
//            {
//                var message = string.Format("Could not find element '{0}' ({1}) after {2} seconds", name, @by,
//                    timeoutSec);
//                throw new NoSuchElementException(message);
//            }
//        }
    }
}
