using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class Component<T> where T : BaseComponent, new()
    {
       private Element RootElement;

        public Component(OpenQA.Selenium.By by) 
        {
            this.RootElement = new Element(by);
        }
    }
}
