using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Field : Element
    {
        public Field(By bylocator)
        {
            this.name = "";
            this.by = bylocator;
        }
        public Field(string name, By bylocator)
        {
            this.name = name;
            this.by = bylocator;
        }
    }
}
