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
        public Dropdown(By bylocator)
        {
            this.name = "";
            this.by = bylocator;
        }
        public Dropdown(string name, By bylocator)
        {
            this.name = name;
            this.by = bylocator;
        }

        public Dropdown SelectOption(string option)
        {
            new SelectElement(element).SelectByText(option);
            return this;
        }

    }
}
