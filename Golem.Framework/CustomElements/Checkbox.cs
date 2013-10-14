using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Checkbox : Element
    {
        public Checkbox(string name, By bylocator)
        {
            this.name = name;
            this.by = bylocator;
        }
    }
}
