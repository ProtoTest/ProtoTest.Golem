using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Radio : Element
    {

        public Radio(By bylocator) : base(bylocator)
        {
        }
        public Radio(string name, By bylocator) : base(name,bylocator)
        {
        }

        public Radio SetValue(string value)
        {
            element.FindElement(By.XPath("//*[@value='" + value + "']")).Click();
            return this;
        }
    }
}
