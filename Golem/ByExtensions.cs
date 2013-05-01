using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem
{
    public static class ByE
    {
        public static By Text(string text)
        {
            return By.XPath("//*[text()='" + text + "']");
        }
        public static By PartialText(string text)
        {
            return By.XPath("//*[contains(text(),'"+text+"')]");
        }
    }
}
