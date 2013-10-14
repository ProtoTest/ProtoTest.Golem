using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Link : Element
    {
        public Link(string name, By bylocator)
        {
            this.name = name;
            this.by = bylocator;
        }
        public Link(By bylocator)
        {
            this.name = "";
            this.by = bylocator;
        }
    }
}
