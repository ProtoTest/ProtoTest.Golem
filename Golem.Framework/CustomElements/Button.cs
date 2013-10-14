using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Button : Element
    {
        public Button(By bylocator)
        {
            this.name = "";
            this.by = bylocator;
        }
        public Button(string name, By bylocator)
        {
            this.name = name;
            this.by = bylocator;
        }

    }
}
