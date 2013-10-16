using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Image : Element
    {
        public Image(By bylocator) : base(bylocator)
        {
        }
        public Image(string name, By bylocator): base(name,bylocator)
        {
        }

    }
}
