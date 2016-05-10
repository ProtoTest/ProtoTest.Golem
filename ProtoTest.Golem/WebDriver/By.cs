using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver
{
    public class By : OpenQA.Selenium.By
    {
        public static OpenQA.Selenium.By Id(string id)
        {
            return OpenQA.Selenium.By.Id(id);
        }
        public static OpenQA.Selenium.By Name(string id)
        {
            return OpenQA.Selenium.By.Name(id);
        }
        public static OpenQA.Selenium.By TagName(string id)
        {
            return OpenQA.Selenium.By.TagName(id);
        }
        public static OpenQA.Selenium.By ClassName(string id)
        {
            return OpenQA.Selenium.By.ClassName(id);
        }
        public static OpenQA.Selenium.By CssSelector(string id)
        {
            return OpenQA.Selenium.By.CssSelector(id);
        }
        public static OpenQA.Selenium.By LinkText(string id)
        {
            return OpenQA.Selenium.By.LinkText(id);
        }
        public static OpenQA.Selenium.By PartialLinkText(string id)
        {
            return OpenQA.Selenium.By.PartialLinkText(id);
        }
        public static OpenQA.Selenium.By XPath(string id)
        {
            return OpenQA.Selenium.By.XPath(id);
        }

        public static OpenQA.Selenium.By Text(string text)
        {
            return OpenQA.Selenium.By.XPath("//*[text()='" + text + "']");
        }

        public static OpenQA.Selenium.By PartialText(string text)
        {
            return OpenQA.Selenium.By.XPath("//*[contains(text(),'" + text + "')]");
        }

        public static OpenQA.Selenium.By PartialAttribute(string tag, string attribute, string value)
        {
            string attr;

            // put a '@' in front of the attribute if the user did not
            if (!attribute.StartsWith("@"))
            {
                attr = string.Format("@{0}", attribute);
            }
            else
            {
                attr = attribute;
            }

            return OpenQA.Selenium.By.XPath(string.Format("//{0}[contains({1},'{2}')]", tag, attr, value));
        }

    }
}
