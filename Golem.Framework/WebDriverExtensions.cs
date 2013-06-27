using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA;
using System.Drawing;
using System.Collections.ObjectModel;


namespace Golem.Framework
{
    public static class WebDriverExtensions
    {
        public static IWebElement WaitForPresent(this IWebDriver driver, By by, int timeout=0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until<IWebElement>((d) => d.FindElement(@by));
        }

        public static void WaitForNotPresent(this IWebDriver driver, By by, int timeout=0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            wait.Until(d => d.FindElements(by).Count == 0);
        }

        public static IWebElement WaitForVisible(this IWebDriver driver, By by, int timeout=0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Config.Settings.runTimeSettings.ElementTimeoutSec));
            wait.Until(d => ((d.FindElements(by).Count>0)&&(d.FindElement(by).Displayed == true)));
            return driver.FindElement(by);
        }
        public static void WaitForNotVisible(this IWebDriver driver, By by, int timeout=0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            wait.Until(d=>((d.FindElements(by).Count==0)||(d.FindElement(by).Displayed==false)));
        }
        public static IWebElement FindElementWithText(this IWebDriver driver, string text)
        {
            return driver.FindElement(By.XPath("//*[text()='" + text + "']"));
        }
        public static IWebElement WaitForElementWithText(this IWebDriver driver, string text, int timeout=0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            return driver.WaitForPresent(By.XPath("//*[text()='" + text + "']"));
        }
        public static void VerifyElementPresent(this IWebDriver driver, By by, bool isPresent=true)
        {
            int count = driver.FindElements(by).Count;
            Verify(isPresent && count == 0,"VerifyElementPresent Failed : Element : " + by.ToString() +
                                          (isPresent == true ? " found" : " not found"));
        }

        public static void Verify(bool condition, string message)
        {
            if (!condition)
            {
                TestBaseClass.AddVerificationError(message);
            }
            else
            {
                TestContext.CurrentContext.IncrementAssertCount();

            }
        }

        public static IWebElement SelectOption(this IWebElement element, string option)
        {
            element.FindElement(By.XPath("//option[contains(text(),'"+option+"')]")).Click();
            return element;
        }

        public static IWebElement FindVisibleElement(this IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            foreach (IWebElement ele in elements.Where(ele => ele.Displayed))
            {
                return ele;
            }
            throw new ElementNotVisibleException("No element visible for : " + by.ToString());
        }

        public static void VerifyElementVisible(this IWebDriver driver, By by, bool isVisible = true)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(by);
            int count = elements.Count;
            bool visible = false;
            if (isVisible && count != 0)
            {
                foreach(IWebElement element in elements)
                {
                    if (element.Displayed)
                        visible = true;
                }
            }
            Verify(isVisible != visible,
                   "VerifyElementVisible Failed : Element : " + by.ToString() +
                   (isVisible == true ? " visible" : " not visible"));

        }

        public static void VerifyElementText(this IWebDriver driver, By by, string expectedText)
        {
            string actualText = driver.FindElement(by).Text;
            Verify(actualText != expectedText,
                   "VerifyElementText Failed : Expected : " + by.ToString() + " Expected text : '" + expectedText +
                   "' + Actual '" + actualText);
        }
        
        public static Image GetScreenshot(this IWebDriver driver)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(ss.AsByteArray);
            return System.Drawing.Image.FromStream(ms);
        }

        public static void SetText(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
            if (element.GetAttribute("value") != text)
            {
                element.Clear();
                element.SendKeys(text);
            }
        }

        public static void ExecuteJavaScript(this IWebDriver driver, string script)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("return " + script);
        }

    }
}
