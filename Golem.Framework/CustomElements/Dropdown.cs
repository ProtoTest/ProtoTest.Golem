using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Dropdown : Element
    {
        public Dropdown(string name, By bylocator)
        {
            this.name = name;
            this.by = bylocator;
        }
    }
}
