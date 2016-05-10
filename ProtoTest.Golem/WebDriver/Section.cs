using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver
{
    public abstract class Section : BasePageObject
    {
        public static Element Root;

        private By by;

        protected Section(By by) : base()
        {
            this.by = by;
            Root = new Element(by);
        }

        protected Section()
        {
        }

    }
}
