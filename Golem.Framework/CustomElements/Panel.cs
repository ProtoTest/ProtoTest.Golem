using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem.Framework.CustomElements
{
    public class Panel : Element
    {
        public Panel(By bylocator): base(bylocator)
        {
        }
        public Panel(string name, By bylocator): base(name,bylocator)
        {
        }
    }
}
