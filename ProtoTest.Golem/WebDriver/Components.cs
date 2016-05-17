using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class Components<T> : IEnumerable<T> where T : Element, new()
    {
       private Element RootElements;

        public Components()
        {
        }

        public Components(By by) 
        {
            this.RootElements = new Element(by);
        }

       public IEnumerator<T> GetEnumerator()
        {
            foreach (var ele in RootElements)
            {
                var el = new T();
                el.root = ele;
                yield return el;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Create an element verification for some condition.
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify()
        {
           var  timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this.RootElements, timeoutSec, false);
        }

        /// <summary>
        ///     Wait for some condition on the element
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil()
        {
            var timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this.RootElements, timeoutSec, true);
        }

        /// <summary>
        ///     Create an element verification for some condition.
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify(int timeoutSec)
        {
            return new ElementVerification(this.RootElements, timeoutSec, false);
        }

        /// <summary>
        ///     Wait for some condition on the element
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil(int timeoutSec)
        {
            return new ElementVerification(this.RootElements, timeoutSec, true);
        }
    }
}
