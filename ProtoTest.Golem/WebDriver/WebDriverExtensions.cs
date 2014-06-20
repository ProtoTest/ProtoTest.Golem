using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Gallio.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// Extension methods added to the IWebDriver and IWebElement API's
    /// </summary>
    public static class WebDriverExtensions
    {
        public static ElementVerification Verify(this IWebElement element)
        {
            return new ElementVerification(new Element(element), Config.Settings.runTimeSettings.ElementTimeoutSec,
                false);
        }

        public static ElementVerification WaitUntil(this IWebElement element)
        {
            return new ElementVerification(new Element(element), Config.Settings.runTimeSettings.ElementTimeoutSec, true);
        }

        public static IWebElement Hide(this IWebElement element)
        {
            return
                (IWebElement)
                    WebDriverTestBase.driver.ExecuteJavaScript("arguments[0].style.visibility='hidden';return;", element);
        }

        public static IWebElement Show(this IWebElement element)
        {
            return
                (IWebElement)
                    WebDriverTestBase.driver.ExecuteJavaScript("arguments[0].style.visibility='visible';return;",
                        element);
        }

        public static IWebElement FindInSiblings(this IWebElement element, By by)
        {
            return element.GetParent().FindElement(by);
        }

        public static IWebElement FindInChildren(this IWebElement element, By by)
        {
            return element.FindElement(by);
        }

        public static IWebElement GetParent(this IWebElement element)
        {
            IWebDriver driver = WebDriverTestBase.driver;
            return (IWebElement) element.FindElement(By.XPath(".."));
        }

        public static string GetHtml(this IWebElement element)
        {
            return element.GetAttribute("outerHTML");
        }

        public static string GetHtml(this IWebElement element, int length)
        {
            try
            {
                string html = element.GetAttribute("outerHTML").Replace("\r\n","");
                if (html.Length <= length)
                    return html;
                int halfLength = length/2;
                string start = html.Substring(0, halfLength);
                string end = html.Substring((html.Length - halfLength), halfLength);
                return string.Format("{0}...{1}", start, end);
            }
            catch (Exception e)
            {
                return "HTML Not found " + e.Message;
            }
            
        }

        public static bool IsStale(this IWebElement element)
        {
            try
            {
                bool enabled = element.Enabled;
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
                var jsDriver = ((IJavaScriptExecutor) WebDriverTestBase.driver);
                var originalElementBorder = (string) jsDriver.ExecuteScript("return arguments[0].style.border", element);
                jsDriver.ExecuteScript("arguments[0].style.border='3px solid red'; return;", element);
                Thread.Sleep(20);
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
            var jsDriver = ((IJavaScriptExecutor) WebDriverTestBase.driver);
            jsDriver.ExecuteScript("arguments[0].checked=false;", element);
        }

        public static void MouseOver(this IWebElement element)
        {
            IWebDriver driver = WebDriverTestBase.driver;
            var action = new Actions(driver).MoveToElement(element);
            Thread.Sleep(2000);
            action.Build().Perform();
        }

        public static IWebElement WaitForPresent(this IWebElement element, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = element.FindElements(by);
                if (eles.Count > 0)
                    return eles[0];
                Common.Delay(1000);
            }
            throw new NoSuchElementException(string.Format("Element ({0}) was not present after {1} seconds",
                by.ToString(), timeout));
        }

        public static IWebElement WaitForPresent(this IWebDriver driver, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by);
                if (eles.Count > 0)
                    return eles[0];
                Common.Delay(1000);
            }
            throw new NoSuchElementException(string.Format("Element ({0}) was not present after {1} seconds",
                by.ToString(), timeout));
        }

        public static void WaitForNotPresent(this IWebDriver driver, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by);
                if (eles.Count > 0)
                    return;
                Common.Delay(1000);
            }
            throw new InvalidElementStateException(string.Format("Element ({0}) was still present after {1} seconds",
                by.ToString(), timeout));
        }

        public static IWebElement WaitForVisible(this IWebDriver driver, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by);
                if (eles.Count > 0 && eles[0].Displayed)
                    return eles[0];
            }
            throw new ElementNotVisibleException(string.Format("Element ({0}) was not visible after {1} seconds",
                by.ToString(), timeout));
        }

        public static void WaitForNotVisible(this IWebDriver driver, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by);
                if (eles.Count > 0 && !eles[0].Displayed)
                    return;
                Common.Delay(1000);
            }
            throw new ElementNotVisibleException(string.Format("Element ({0}) was still visible after {1} seconds",
                by.ToString(), timeout));
        }

        public static IWebElement FindElementWithText(this IWebDriver driver, string text)
        {
            return driver.FindElement(By.XPath("//*[text()='" + text + "']"));
        }

        public static IWebElement WaitForElementWithText(this IWebDriver driver, string text, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            return driver.WaitForPresent(By.XPath("//*[text()='" + text + "']"));
        }

        public static void VerifyElementPresent(this IWebDriver driver, By by, bool isPresent = true)
        {
            int count = driver.FindElements(by).Count;
            Verify(isPresent && count == 0, "VerifyElementPresent Failed : Element : " + @by +
                                            (isPresent ? " found" : " not found"));
        }

        public static void Verify(bool condition, string message)
        {
            if (!condition)
            {
                TestBase.AddVerificationError(message);
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

        public static IWebElement SelectOptionByPartialText(this IWebElement element, string text)
        {
            var s_element = new SelectElement(element);

            foreach (IWebElement option in s_element.Options.Where(option => option.Text.Contains(text)))
            {
                option.Click();
                break;
            }

            return element;
        }

        public static IWebElement FindVisibleElement(this IWebDriver driver, By by)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(by);
            foreach (IWebElement ele in elements.Where(ele => (ele.Displayed) && (ele.Enabled)))
            {
                return ele;
            }
            throw new ElementNotVisibleException("No element visible for : " + @by);
        }

        public static void VerifyElementVisible(this IWebDriver driver, By by, bool isVisible = true)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(by);
            int count = elements.Count;
            bool visible = false;
            if (isVisible && count != 0)
            {
                foreach (IWebElement element in elements)
                {
                    if (element.Displayed)
                    {
                        visible = true;
                    }
                }
            }
            Verify(isVisible != visible,
                "VerifyElementVisible Failed : Element : " + @by +
                (isVisible ? " visible" : " not visible"));
        }

        public static void VerifyElementText(this IWebDriver driver, By by, string expectedText)
        {
            string actualText = driver.FindElement(by).Text;
            Verify(actualText != expectedText,
                "VerifyElementText Failed : Expected : " + @by + " Expected text : '" + expectedText +
                "' + Actual '" + actualText);
        }

        public static Rectangle GetRect(this IWebElement element)
        {
            try
            {
                var jsDriver = ((IJavaScriptExecutor) WebDriverTestBase.driver);
                var originalElementBorder = (string) jsDriver.ExecuteScript("return arguments[0].style.border", element);
                return (Rectangle) jsDriver.ExecuteScript("return arguments[0].getBoundingClientRect();", element);
            }
            catch (Exception)
            {
                return new Rectangle();
            }
        }

        public static Image GetScreenshot(this IWebDriver driver)
        {
            Image screen_shot = null;

            try
            {
                if (driver == null) return screen_shot;
                Screenshot ss = ((ITakesScreenshot) driver).GetScreenshot();
                    var ms = new MemoryStream(ss.AsByteArray);
                    screen_shot = Image.FromStream(ms); 
                    ms.Dispose();
   
            }
            catch (Exception e)
            {
                TestLog.Failures.WriteLine("Failed to take screenshot: " + e.Message);
            }

            return screen_shot;
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
            try
            {
                var js = (IJavaScriptExecutor) driver;
                return js.ExecuteScript("return " + script);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static object ExecuteJavaScript(this IWebDriver driver, string script, params object[] args)
        {
            var js = (IJavaScriptExecutor) driver;
            return js.ExecuteScript(script, args);
        }

        public static void JavaWindowScroll(this IWebDriver driver, int xCord, int yCord)
        {
            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("window.scrollBy(" + xCord + "," + yCord + ");");
        }

        public static IWebElement ScrollIntoView(this IWebElement element)
        {
            var js = (IJavaScriptExecutor) WebDriverTestBase.driver;
            js.ExecuteScript("arguments[0].scrollIntoView(); return;", element);
            return element;
        }

        public static void SelectNewWindow(this IWebDriver driver, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.Settings.runTimeSettings.OpenWindowTimeoutSec;

            try
            {
                string currentHandle = driver.CurrentWindowHandle;
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => (driver.WindowHandles.Count() > 1));

                foreach (string handle in (driver.WindowHandles))
                {
                    if (handle != currentHandle)
                    {
                        driver.SwitchTo().Window(handle);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static void SetOrientation(this IWebDriver driver, ScreenOrientation orientation)
        {
            ((IRotatable) driver).Orientation = orientation;
        }

        public static void SetBrowserSize(this IWebDriver driver, Size size)
        {
            driver.Manage().Window.Size = size;
        }

        public static IWebDriver GetWrappedDriver(this IWebElement element)
        {
            return ((IWrapsDriver) element).WrappedDriver;
        }

        public static IWebElement DragToOffset(this IWebElement element, int x, int y)
        {
            var action = new Actions(WebDriverTestBase.driver);
            action.MoveToElement(element, x, y).Click().Build().Perform();
            return element;
        }

        public static Image GetImage(this IWebElement element)
        {
            var size = new Size(element.Size.Width, element.Size.Height);
            if (element.Displayed == false || element.Location.X<0 || element.Location.Y <0)
            {
                throw new BadImageFormatException(string.Format(
                    "Could not create image for element as it is hidden"));
            }
            var cropRect = new Rectangle(element.Location, size);
            using (Image screenShot = TestBase.testData.driver.GetScreenshot())
            {
                if (cropRect.X < 0)
                {
                    cropRect.X = 0;

                }
                if (cropRect.Y < 0)
                {
                    cropRect.Y = 0;

                }
                if (cropRect.X + cropRect.Width > screenShot.Width)
                {
                    cropRect.Width = screenShot.Width - cropRect.X;
                }
                if (cropRect.Y + cropRect.Height > screenShot.Height)
                {
                    cropRect.Height = screenShot.Height - cropRect.Y;
                }

                try
                {
                    var bmpImage = new Bitmap(screenShot);
                    Bitmap bmpCrop = bmpImage.Clone(cropRect, bmpImage.PixelFormat);
                    return bmpCrop;

                }
                catch (Exception e)
                {
                    return null;
                }
                
            }

        }
    }
}