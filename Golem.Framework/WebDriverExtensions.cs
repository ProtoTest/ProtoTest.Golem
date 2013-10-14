using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA;
using System.Drawing;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;


namespace Golem.Framework
{
    public static class WebDriverExtensions
    {
        public static IWebElement Hide(this IWebElement element)
        {
            return (IWebElement)TestBaseClass.driver.ExecuteJavaScript("arguments[0].style.visibility='hidden';return;", element);
        }
        public static IWebElement Show(this IWebElement element)
        {
            return (IWebElement)TestBaseClass.driver.ExecuteJavaScript("arguments[0].style.visibility='visible';return;", element);
        }
        public static IWebElement FindInSiblings(this IWebElement element, By by)
        {
            return element.GetParent().FindElement(by);
        }
        public static IWebElement GetParent(this IWebElement element)
        {
            IWebDriver driver = TestBaseClass.driver;
            return (IWebElement)driver.ExecuteJavaScript(@"return arguments[0].parentNode;", element);
        }
        public static string GetHtml(this IWebElement element)
        {
            IWebDriver driver = TestBaseClass.driver;
            string html = (string)driver.ExecuteJavaScript("var f = document.createElement('div').appendChild(arguments[0].cloneNode(true)); return f.parentNode.innerHTML", element);
                return html;
        }

        public static bool IsStale(this IWebElement element)
        {
            try
            {
                var TagName = element.TagName;
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static void Highlight(this IWebElement element)
        {
            try
            {
                var jsDriver = ((IJavaScriptExecutor)TestBaseClass.driver);
                string originalElementBorder = (string)jsDriver.ExecuteScript("return arguments[0].style.border", element);
                jsDriver.ExecuteScript("arguments[0].style.border='3px solid red'; return;", element);
                Thread.Sleep(50);
                jsDriver.ExecuteScript("arguments[0].style.border='" + originalElementBorder + "'; return;", element);
            }
            catch (Exception)
            {
            }
            
        }

        /// <summary>
        ///     Clear a checked element (radio or checkbox)
        /// </summary>
        /// <param name="element"></param>
        public static void ClearChecked(this IWebElement element)
        {
            var jsDriver = ((IJavaScriptExecutor)TestBaseClass.driver);
            jsDriver.ExecuteScript("arguments[0].checked=false;", element);
        }

        public static void MouseOver(this IWebElement element)
        {
            IWebDriver driver = TestBaseClass.driver;
           Actions action = new Actions(driver);
            action.MoveToElement(element).Build().Perform();
        }

        public static IWebElement WaitForPresent(this IWebElement element, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            WebDriverWait wait = new WebDriverWait(TestBaseClass.driver, TimeSpan.FromSeconds(timeout));
            return wait.Until<IWebElement>(d => element.FindElement(@by));
        }

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
            wait.Until(d => ((d.FindElements(by).Count > 0) && (d.FindElement(by).Displayed == true)));
            return driver.FindElement(by);
        }

        public static IWebElement WaitForVisible(this IWebElement element, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            WebDriverWait wait = new WebDriverWait(TestBaseClass.driver, TimeSpan.FromSeconds(Config.Settings.runTimeSettings.ElementTimeoutSec));
            wait.Until(d => ((d.FindElements(by).Count > 0) && (d.FindElement(by).Displayed == true)));
            return element.FindElement(by);
        }
        public static void WaitForNotVisible(this IWebDriver driver, By by, int timeout=0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            wait.Until(d => ((d.FindElements(by).Count == 0) || (d.FindElement(by).Displayed == false)));
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
            new SelectElement(element).SelectByText(option);
            return element;
        }

        public static IWebElement FindVisibleElement(this IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            foreach (IWebElement ele in elements.Where(ele => (ele.Displayed)&&(ele.Enabled)))
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

        public static object ExecuteJavaScript(this IWebDriver driver, string script)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return js.ExecuteScript("return " + script);
        }

        public static object ExecuteJavaScript(this IWebDriver driver, string script, params object[] args)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return js.ExecuteScript("return " + script, args);
        }

        public static void JavaWindowScroll(this IWebDriver driver, int xCord, int yCord)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(" + xCord + "," + yCord + ");");

        }

        public static IWebElement ScrollIntoView(this IWebElement element)
        {

            IJavaScriptExecutor js = (IJavaScriptExecutor)TestBaseClass.driver;
            js.ExecuteScript("arguments[0].scrollIntoView(); return;");
            return element;
        }

        public static void SelectNewWindow(this IWebDriver driver)
        {
            try
            {
                var currentHanlde = driver.CurrentWindowHandle;
                foreach (var handle in (driver.WindowHandles))
                {
                    if (handle != currentHanlde)
                        driver.SwitchTo().Window(handle);
                }
            }
            catch (Exception)
            {
            }
            
        }

    }
}
