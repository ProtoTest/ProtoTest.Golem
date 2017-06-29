using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Golem.Core;

namespace Golem.WebDriver
{
    public class Components<T> : IEnumerable<T> where T : BaseComponent, new()
    {
       private Element root;

        public Components()
        {
        }

        public Components(By by) 
        {
            this.root = new Element(by);
        }

        public Components(By by, Frame frame)
        {
            this.root = new Element(by, frame);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var ele in this.root)
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
            return new ElementVerification(this.root, timeoutSec, false);
        }

        /// <summary>
        ///     Wait for some condition on the element
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil()
        {
            var timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this.root, timeoutSec, true);
        }

        /// <summary>
        ///     Create an element verification for some condition.
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify(int timeoutSec)
        {
            return new ElementVerification(this.root, timeoutSec, false);
        }

        /// <summary>
        ///     Wait for some condition on the element
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil(int timeoutSec)
        {
            return new ElementVerification(this.root, timeoutSec, true);
        }
    }
}
