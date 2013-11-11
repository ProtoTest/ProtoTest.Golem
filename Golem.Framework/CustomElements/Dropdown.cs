using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Golem.Framework.CustomElements
{
    public class Dropdown : Element
    {
        public Dropdown(By bylocator) : base(bylocator)
        {
        }
        public Dropdown(string name, By bylocator) : base(name,bylocator)
        {
        }

        public Dropdown SelectOption(string option)
        {
            new SelectElement(element).SelectByText(option);
            return this;
        }

    }
}
