using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    ///     Extension methods added to the IWebDriver and IWebElement API's
    /// </summary>
    public static class WebDriverExtensions
    {
        public static ElementVerification Verify(this IWebElement element, int timeout = -1)
        {
            try
            {
                if (timeout == -1)
                    timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
                var GolemElement = (Element) element;
                return new ElementVerification(GolemElement, timeout, false);
            }
            catch (Exception)
            {
                return new ElementVerification(new Element(element), Config.settings.runTimeSettings.ElementTimeoutSec,
                    false);
            }
        }

        public static ElementVerification WaitUntil(this IWebElement element, int timeout = -1)
        {
            try
            {
                if (timeout == -1)
                    timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
                var GolemElement = (Element) element;
                return new ElementVerification(GolemElement, timeout, true);
            }
            catch (Exception)
            {
                return new ElementVerification(new Element(element), Config.settings.runTimeSettings.ElementTimeoutSec,
                    false);
            }
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
            var driver = WebDriverTestBase.driver;
            return element.FindElement(By.XPath(".."));
        }

        public static string GetHtml(this IWebElement element)
        {
            try
            {
                return element.GetAttribute("outerHTML");
            }
            catch
            {
                return "";
            }
        }

        public static string GetHtml(this IWebElement element, int length)
        {
            try
            {
                var html = element.GetAttribute("innerHTML").Replace("\r\n", "");
                if (html.Length <= length)
                    return html;
                var halfLength = length/2;
                var start = html.Substring(0, halfLength);
                var end = html.Substring((html.Length - halfLength), halfLength);
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
                if (element == null)
                    return true;
                var enabled = element.Enabled;
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }

        public static void Highlight(this IWebElement element, int ms = 30, string color="yellow")
        {
            try
            {
                var jsDriver = ((IJavaScriptExecutor) ((IWrapsDriver) element).WrappedDriver);
                var originalElementBorder = (string) jsDriver.ExecuteScript("return arguments[0].style.background", element);
                jsDriver.ExecuteScript(string.Format("arguments[0].style.background='{0}'; return;", color), element);
                var bw = new BackgroundWorker();
                if (ms >= 0)
                {
                    bw.DoWork += (obj, e) => Unhighlight(element, originalElementBorder, ms);
                    bw.RunWorkerAsync();  
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }
        }

        private static void Unhighlight(IWebElement element, string border, int timeMs)
        {
            try
            {
                var jsDriver = ((IJavaScriptExecutor) ((IWrapsDriver) element).WrappedDriver);
                Thread.Sleep(timeMs);
                jsDriver.ExecuteScript("arguments[0].style.background='" + border + "'; return;", element);
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        ///     Clear a checked element (radio or checkbox)
        /// </summary>
        /// <param name="element"></param>
        public static void ClearChecked(this IWebElement element)
        {
            var jsDriver = ((IJavaScriptExecutor) ((IWrapsDriver) element));
            jsDriver.ExecuteScript("arguments[0].checked=false;", element);
        }

        public static void MouseOver(this IWebElement element)
        {
            var driver = WebDriverTestBase.driver;
            var action = new Actions(driver).MoveToElement(element);
            Thread.Sleep(2000);
            action.Build().Perform();
        }

        public static IWebElement WaitForPresent(this IWebElement element, OpenQA.Selenium.By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = element.FindElements(by);
                if (eles.Count > 0)
                    return eles.FirstOrDefault(x => x.Displayed);
                Common.Delay(1000);
            }
            throw new NoSuchElementException(string.Format("Element ({0}) was not present after {1} seconds",
                @by, timeout));
        }

        public static IWebElement WaitForPresent(this IWebDriver driver, OpenQA.Selenium.By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by);
                if (eles.Count > 0)
                    return eles.FirstOrDefault(x => x.Displayed);
                Common.Delay(1000);
            }
            throw new NoSuchElementException(string.Format("Element ({0}) was not present after {1} seconds",
                @by, timeout));
        }

        public static void WaitForNotPresent(this IWebDriver driver, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by);
                if (eles.Count == 0)
                    return;
                Common.Delay(1000);
            }
            throw new InvalidElementStateException(string.Format("Element ({0}) was still present after {1} seconds",
                @by, timeout));
        }

        public static IWebElement WaitForVisible(this IWebDriver driver, By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by).Where(x => x.Displayed).ToList();
                if (eles.Count > 0)
                    return eles.FirstOrDefault(x => x.Displayed);
                Common.Delay(1000);
            }
            throw new ElementNotVisibleException(string.Format("Element ({0}) was not visible after {1} seconds",
                @by, timeout));
        }

        public static void WaitForNotVisible(this IWebDriver driver, OpenQA.Selenium.By by, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
            var then = DateTime.Now.AddSeconds(timeout);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                var eles = driver.FindElements(by).Where(x => x.Displayed).ToList();
                if (eles.Count == 0)
                    return;
                Common.Delay(1000);
            }
            throw new ElementNotVisibleException(string.Format("Element ({0}) was still visible after {1} seconds",
                @by, timeout));
        }

        public static IWebElement FindElementWithText(this IWebDriver driver, string text)
        {
            return driver.FindElement(By.XPath("//*[text()=\"" + text + "\"]"));
        }

        public static IWebElement WaitForElementWithText(this IWebDriver driver, string text, int timeout = 0)
        {
            if (timeout == 0) timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
            return driver.WaitForPresent(By.XPath("//*[text()=\"" + text + "\"]"));
        }

        public static void VerifyElementPresent(this IWebDriver driver, OpenQA.Selenium.By by, bool isPresent = true)
        {
            var count = driver.FindElements(by).Count;
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
//                TestContext.CurrentContext.IncrementAssertCount();
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

            foreach (var option in s_element.Options.Where(option => option.Text.Contains(text)))
            {
                option.Click();
                break;
            }

            return element;
        }

        public static IWebElement FindVisibleElement(this IWebDriver driver, OpenQA.Selenium.By by)
        {
            var elements = driver.FindElements(by);
            foreach (var ele in elements.Where(ele => (ele.Displayed) && (ele.Enabled)))
            {
                return ele;
            }
            throw new ElementNotVisibleException("No element visible for : " + @by);
        }

        public static void VerifyElementVisible(this IWebDriver driver, OpenQA.Selenium.By by, bool isVisible = true)
        {
            var elements = driver.FindElements(by);
            var count = elements.Count;
            var visible = false;
            if (isVisible && count != 0)
            {
                foreach (var element in elements)
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

        public static void VerifyElementText(this IWebDriver driver, OpenQA.Selenium.By by, string expectedText)
        {
            var actualText = driver.FindElement(by).Text;
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

        public static string GetScreenshot(this IWebDriver driver, string path)
        {
            driver.TakeScreenshot().SaveAsFile(path, ImageFormat.Png);
            return path;
        }

        public static Image GetScreenshot(this IWebDriver driver)
        {
            Image screen_shot = null;

            try
            {
                if (driver == null) return screen_shot;
                var ss = ((ITakesScreenshot) driver).GetScreenshot();
                var ms = new MemoryStream(ss.AsByteArray);
                screen_shot = Image.FromStream(ms);
                ms.Dispose();
            }
            catch (Exception e)
            {
                Log.Error("Failed to take screenshot: " + e.Message);
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
            if (timeout == 0) timeout = Config.settings.runTimeSettings.OpenWindowTimeoutSec;

            try
            {
                var currentHandle = driver.CurrentWindowHandle;
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d => (driver.WindowHandles.Count() > 1));

                foreach (var handle in (driver.WindowHandles))
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
            action.DragAndDropToOffset(element, x, y).Build().Perform();
            return element;
        }

        public static Image GetImage(this IWebElement element)
        {
            var size = new Size(element.Size.Width, element.Size.Height);
            if (element.Displayed == false || element.Location.X < 0 || element.Location.Y < 0)
            {
                throw new BadImageFormatException(string.Format(
                    "Could not create image for element as it is hidden"));
            }
            var cropRect = new Rectangle(element.Location, size);
            using (var screenShot = TestBase.testData.driver.GetScreenshot())
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
                    using (var bmpImage = new Bitmap(screenShot))
                    {
                        using (var bmpCrop = bmpImage.Clone(cropRect, bmpImage.PixelFormat))
                        {
                            return bmpCrop;
                        }
                        
                    }
                    
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static IWebElement ClickWithOffset(this IWebElement element, int x, int y)
        {
            var action = new Actions(WebDriverTestBase.driver);
            action.MoveToElement(element, x, y).Click().Build().Perform();
            return element;
        }

        public static void Sleep(this IWebDriver driver, int timeoutMs)
        {
            Thread.Sleep(timeoutMs);
        }
    }
}