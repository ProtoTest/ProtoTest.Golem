using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA;

namespace Golem
{
    public static class WebDriverExtensions
    {
        public static IWebElement WaitForElement(this IWebDriver driver, By by)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            return wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(by);
            });
        }

        public static IWebElement FindElementWithText(this IWebDriver driver, string text)
        {
            return driver.FindElement(By.XPath("//*[text()='" + text + "']"));
        }
        public static IWebElement WaitForElementWithText(this IWebDriver driver, string text)
        {
            return driver.WaitForElement(By.XPath("//*[text()='" + text + "']"));
        }
        public static IWebElement VerifyElement(this IWebDriver driver, By by, int timeout=10)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until<IWebElement>((d) =>
            {
                return d.FindElement(by);
            });
        } 


    }
}
