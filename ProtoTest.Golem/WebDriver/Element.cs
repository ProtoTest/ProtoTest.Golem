using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements.Images;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    ///     Provides a simplified API to the IWebELement.  Can be instantiated in a class header.
    ///     Will automatically find the IWebElement each time it is used, not when it is instantiated.
    /// </summary>
    public class Element : IWrapsDriver, IWrapsElement, IEnumerable<Element>
    {
        protected IWebElement _element;
        protected IEnumerable<Element> _elements;
        protected internal Element root;
        protected internal Frame frame;
        protected ElementImages _images;
        public By by;
        public string name;
        public string pageObjectName = "";
        public int timeoutSec;
        private StackFrame[] stackFrames;
        public void GetName()
        {
            this.pageObjectName = TestBase.GetCurrentClassName();
            var stackTrace = new StackTrace(); // get call stack
            stackFrames = stackTrace.GetFrames(); // get method calls (frames)
            var t = this.GetType();

            foreach (var stackFrame in stackFrames)
            {
                var method = stackFrame.GetMethod();
                Type type = method.ReflectedType;
                if (method.Name.Contains("get_"))
                {
                    this.name = $"{ this.pageObjectName}." + method.Name.Replace("get_", "").Replace("()", "");
                    if (name.Contains("ClickOrdersLink"))
                    {
                        foreach(var stack in stackFrames)
                        {
                            Log.Message(stack.GetMethod().Name);
                        }
                    }
                    return;
                }

                if ((type.IsSubclassOf(typeof(Element)) &&
                     (!method.IsConstructor)))
                {
                    this.name = $"{ this.pageObjectName}." + method.Name;
                    if (name.Contains("ClickOrdersLick"))
                    {
                        foreach (var stack in stackFrames)
                        {
                            Log.Message(stack.GetMethod().Name);
                        }
                    }
                    return;
                }

                if ((type.IsSubclassOf(typeof(BaseComponent))))
                {
                    this.name = $"{ this.pageObjectName}." + type.Name;
                    if (name.Contains("ClickOrdersLink"))
                    {
                        foreach (var stack in stackFrames)
                        {
                            Log.Message(stack.GetMethod().Name);
                        }
                    }
                    return;
                }
            }
        }

        public Element()
        {
            GetName();
            pageObjectName = TestBase.GetCurrentClassName();
            timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }

        public Element(ReadOnlyCollection<IWebElement> elements)
        {
            GetName();
            var eles = new List<Element>();
            foreach (var ele in elements)
            {
                eles.Add(new Element(ele));
            }
            this.elements = eles;
        }

        /// <summary>
        ///     Construct an element using an existing element
        /// </summary>
        /// <param name="element"></param>
        public Element(IWebElement element)
        {
            GetName();
            this.element = element;

            pageObjectName = TestBase.GetCurrentClassName();
            timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        ///     Construct an element using an existing element
        /// </summary>
        /// <param name="element"></param>
        public Element(IWebElement element, By by)
        {
            GetName();
            this.element = element;
            this.by = by;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        ///     Construct an element
        /// </summary>
        /// <param name="name">Human readable name of the element</param>
        /// <param name="locator">By locator</param>
        public Element(string name, By locator)
        {
            GetName();
            this.name = name;
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        ///     Construct an element
        /// </summary>
        /// <param name="locator">By locator</param>
        public Element(By locator)
        {
            GetName();
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }

        /// <summary>
        ///     Construct an element within an iframe
        /// </summary>
        /// <param name="name">Human readable name of the element</param>
        /// <param name="locator">By locator</param>
        public Element(string name, By locator, Frame frame)
        {
            this.frame = frame;
            this.name = name;
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }



        /// <summary>
        ///     Construct an element
        /// </summary>
        /// <param name="locator">By locator</param>
        public Element(By locator, Frame frame)
        {
            GetName();
            this.frame = frame;
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }

        public Element(BaseComponent root, By locator, Frame frame=null)
        {
            GetName();
            this.root = root;
            this.frame = frame;
            this.by = locator;
            this.pageObjectName = TestBase.GetCurrentClassName();
            this.timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
        }


        protected IEnumerable<Element> elements
        {
            get
            {
                if (@by != null)
                {
                    if (frame != null)
                    {
                        Log.Message($"Looking in frame {frame.name} {frame.GetHtml()}");
                        WebDriverTestBase.driver.SwitchTo().Frame(frame.WrappedElement);
                    }
                    else
                    {
                        Log.Message("Looking in default frame");
                        WebDriverTestBase.driver.SwitchTo().DefaultContent();
                    }

                    var newList = new List<Element>();
                    Log.Message($"{TestBase.GetCurrentClassAndMethodName()}: Looking for Elements {this.pageObjectName}.{this.name} ({this.@by})");
                    var eles = root != null ? root.FindElements(@by) : driver.FindElements(@by);
                    foreach (var ele in eles)
                    {
                        var nele = new Element(ele);
                        newList.Add(nele);
                    }
                    _elements = newList;
                }

                return _elements;
            }
            set { _elements = value; }
        }

        public IEnumerator<Element> GetEnumerator()
        {
            foreach (var ele in elements)
            {
                yield return ele;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ForEach(Action<Element> action)
        {
            this.ToList().ForEach(action);
        }


        protected IWebDriver driver
        {
            get { return TestBase.testData.driver; }
            set { TestBase.testData.driver = value; }
        }

        public ElementImages Images
        {
            get { return _images ?? (_images = new ElementImages(this)); }
        }

        protected IWebElement element
        {
            get
            {
                _element = GetElement();              
                return _element;
            }
            set { _element = value; }
        }

        /// <summary>
        ///     Is the element present on the page, but not necesarily displayed and visible?
        /// </summary>
        public bool Present
        {
            get
            {
                try
                {
                    return element.Enabled;
                }
                catch (NoSuchElementException e)
                {
                    return false;
                }
                catch (StaleElementReferenceException e)
                {
                    return false;
                }
                catch (InvalidOperationException e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Is the element present and displayed on the page?
        /// </summary>
        public bool Displayed
        {
            get
            {
                try
                {
                    if (!Present) return false;
                    return element.Displayed;
                }
                catch (NoSuchElementException e)
                {
                    return false;
                }
                catch (StaleElementReferenceException e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Is the element present on the page and able to be interacted with?
        /// </summary>
        public bool Enabled
        {
            get
            {
                if (!Present) return false;
                return element.Enabled;
            }
        }

        /// <summary>
        ///     Get the upper-left (x,y) coordinates of the element relative to the upper-left corner of the page.
        /// </summary>
        public Point Location
        {
            get { return element.Location; }
        }

        public Frame Frame
        {
            get
            {
                return this.frame;
            }
            set { this.frame = value; }
        }

        /// <summary>
        ///     Checks if the element is selected on the page.
        /// </summary>
        public bool Selected
        {
            get
            {
                if (!Present) return false;
                return element.Selected;
            }
        }

        /// <summary>
        ///     Return an object containing the size of the element (height, width).
        /// </summary>
        public Size Size
        {
            get { return element.Size; }
        }

        /// <summary>
        ///     Return the tag name of the element.
        /// </summary>
        public string TagName
        {
            get { return element.TagName; }
        }

        /// <summary>
        ///     Property to get and set the Text for the element.
        /// </summary>
        public string Text
        {
            get
            {
                  var text = element.Text;
                return text;
            }
            set
            {
                element.Clear();
                element.SendKeys(value);
            }
        }

        /// <summary>
        ///     Returns the first element found by the locator.
        /// </summary>
        /// <param name="by">The locator to use.</param>
        /// <returns>The IWebElement found.</returns>
        public IWebElement FindElement(By by)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    Log.Message($"{TestBase.GetCurrentClassAndMethodName()}: Looking for Child ({by}) of Element {this.name} ({this.@by})");
                    var eles = element.FindElements(by);
                    if (eles.Count > 0)
                        return eles[0];
                    Common.Delay(1000);
                }
                catch (StaleElementReferenceException e)
                {
                }
            }
            throw new NoSuchElementException(
                $"Child ({by}) of Element {this.name} ({this.@by}) was not present after {timeoutSec} seconds");
        }

        /// <summary>
        ///     Return a collection of elements found by the locator.
        /// </summary>
        /// <param name="by">The locator to use.</param>
        /// <returns>Collection of IWebElements found.</returns>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {

            Log.Message($"{TestBase.GetCurrentClassAndMethodName()}: Looking for Children ({by}) of Element {this.name} ({this.@by})");
            
            var childelements = element.FindElements(by);
            return childelements;
        }

        /// <summary>
        ///     Clears the contents of the element\\.
        /// </summary>
        public Element Clear()
        {
            try
            {
                element.Clear();
                return this;
            }
            catch (Exception e)
            {
                Log.Warning("Could not clear " + e.Message);
                return this;
            }
            
        }

        /// <summary>
        ///     Click the element and optionally highlights the element if set in the application configuration settings.
        /// </summary>
        public Element Click()
        {
            try
            {
                element.Click();
            }
            catch (InvalidOperationException e)
            {
                element.ScrollIntoView().Click();
            }
            return this;
        }

        /// <summary>
        ///     Submit this element to the web server and optionally highlights the element if set in the application configuration
        ///     settings.
        /// </summary>
        public Element Submit()
        {
            element.Submit();
            return this;
        }

        /// <summary>
        ///     Simulates typing text into the element and optionally highlights the element if set in the application
        ///     configuration settings.
        /// </summary>
        /// <param name="text">Text to send</param>
        public Element SendKeys(string text)
        {
            try
            {
                element.SendKeys(text);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Cannot sendKeys that element, please verify it is an input or text field" + e.Message);
            }
            
            return this;
        }

        /// <summary>
        ///     Get the value of the requested attribute for the element
        /// </summary>
        /// <param name="attribute">The attribute name</param>
        /// <returns></returns>
        public string GetAttribute(string attribute)
        {
            try
            {
                return element.GetAttribute(attribute);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        ///     Get the value of a CSS property for the element
        /// </summary>
        /// <param name="propertyName">The CSS property name</param>
        /// <returns></returns>
        public string GetCssValue(string propertyName)
        {
            return element.GetCssValue(propertyName);
        }

        public IWebDriver WrappedDriver
        {
            get { return driver; }
            private set { driver = value; }
        }

        public IWebElement WrappedElement
        {
            get { return element; }
            private set { element = value; }
        }

        public bool IsPresent(int timeoutSec)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    var eles = driver.FindElements(by);
                    if (eles.Count > 0)
                        return true;
                    Common.Delay(1000);
                }
                catch (StaleElementReferenceException e)
                {
                }
            }
            return false;
        }

        public bool IsDisplayed(int timeoutSec)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    var eles = driver.FindElements(by).ToList();
                    if (eles.Any(ele => ele.Displayed))
                    {
                        return true;
                    }
                    Common.Delay(1000);
                }
                catch (StaleElementReferenceException e)
                {
                }
                catch (InvalidOperationException e)
                {
                    
                }
            }
            return false;
        }

        public Element SelectOption(string option)
        {
            new SelectElement(this.GetElement()).SelectByText(option);
            return this;
        }

        public Element SelectOptionByPartialText(string text)
        {
            var s_element = new SelectElement(this.GetElement());

            foreach (var option in s_element.Options.Where(option => option.Text.Contains(text)))
            {
                option.Click();
                break;
            }

            return this;
        }


        /// <summary>
        ///     Returns the first element found by the locator.
        /// </summary>
        /// <param name="by">The locator to use.</param>
        /// <returns>The IWebElement found.</returns>
        public IWebElement FindElement(Element childElement)
        {
            var then = DateTime.Now.AddSeconds(timeoutSec);
            for (var now = DateTime.Now; now < then; now = DateTime.Now)
            {
                try
                {
                    Log.Message($"{TestBase.GetCurrentClassAndMethodName()}: Looking for Child {childElement.name} ({childElement.@by}) of Element {this.name} ({this.@by})");
                    var eles = element.FindElements(childElement.by);
                    if (eles.Count > 0)
                        return eles[0];
                    Common.Delay(1000);
                }
                catch (StaleElementReferenceException e)
                {
                }
            }
            throw new NoSuchElementException($"Child Element {childElement.name} ({childElement.@by}) of {this.name} ({this.@by}) was not present after {timeoutSec} seconds");
        }

        public ReadOnlyCollection<IWebElement> FindElements(Element element)
        {
            return FindElements(element.by);
        }

        /// <summary>
        ///     Create an element verification for some condition.
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify()
        {
            timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this, timeoutSec, false);
        }

        /// <summary>
        ///     Wait for some condition on the element
        /// </summary>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil(int timeoutSec)
        {
            this.timeoutSec = timeoutSec;
            return new ElementVerification(this, this.timeoutSec, true);
        }

        /// <summary>
        ///     Create an element verification for some condition
        /// </summary>
        /// <param name="timeoutSec">
        ///     timeout that overrides the default timeout set in the configuration settings class or
        ///     App.config file
        /// </param>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification Verify(int timeoutSec)
        {
            this.timeoutSec = timeoutSec;
            return new ElementVerification(this, this.timeoutSec, false);
        }

        /// <summary>
        ///     Wait for some condition on the element
        /// </summary>
        /// <param name="timeoutSec">
        ///     Optional timeout that overrides the default timeout set in the configuration settings class or
        ///     App.config file
        /// </param>
        /// <returns>A new ElementVerification for the element</returns>
        public ElementVerification WaitUntil()
        {
            timeoutSec = Config.settings.runTimeSettings.ElementTimeoutSec;
            return new ElementVerification(this, timeoutSec, true);
        }

        public virtual IWebElement GetElement()
        {
            try
            {
                if (_element.IsStale())
                {
                    
                    Log.Message($"{TestBase.GetCurrentClassAndMethodName()}: Looking for Element {this.name} {this.@by}");
                    if (root != null)
                    {
                        var root_ele = root.GetElement();
                        if (frame != null)
                        {
                            Log.Message(
                                $"{TestBase.GetCurrentClassAndMethodName()}: Looking in frame : {frame.@by}:  {frame.GetHtml()}");
                            driver.SwitchTo().Frame(frame.GetElement());
                        }
                        _element = root_ele.WaitForPresent(@by, timeoutSec);
                    }
                    else
                    {
                        if (frame != null)
                        {
                            Log.Message(
                                $"{TestBase.GetCurrentClassAndMethodName()}: Looking in frame : {frame.@by}:  {frame.GetHtml()}");
                            driver.SwitchTo().Frame(frame.GetElement());
                        }
                        else
                        {
                            Log.Message($"{TestBase.GetCurrentClassAndMethodName()}: Looking in default frame");
                            driver.SwitchTo().DefaultContent();
                        }
                        _element = driver.WaitForPresent(@by, timeoutSec);
                    }
                }
                return _element;
            }
            catch (NoSuchElementException e)
            {
                var message = $"Could not find element '{name}' ({@by}) after {timeoutSec} seconds {GetFrameMessage()}";
                throw new NoSuchElementException(message);
            }
        }

        private string GetFrameMessage()
        {
            if (frame != null)
            {
                return " In frame " + frame.name;
            }
            return "";
        }

        /// <summary>
        ///     Clear a checked element (radio or checkbox)
        /// </summary>
        public Element ClearChecked()
        {
            element.ClearChecked();
            return this;
        }

        /// <summary>
        ///     Highlight the element on the page
        /// </summary>
        public Element Highlight(int ms = 30, string color = "yellow")
        {
            element.Highlight(ms, color);
            return this;
        }

        /// <summary>
        ///     Set the checkbox element
        /// </summary>
        /// <param name="isChecked">if true, check it; if false, uncheck it</param>
        /// <returns>The element reference</returns>
        public Element SetCheckbox(bool isChecked)
        {
            if (element.Selected != isChecked)
            {
                element.Click();
            }
            return this;
        }

        public bool IsStale()
        {
            return element.IsStale();
        }

        public Element SetText(string value)
        {
            Clear();
            SendKeys(value);
            var element_value = this.GetAttribute("value");
            if (value != element_value)
            {
                Clear();
                Thread.Sleep(1000);
                SendKeys(value);
            }
            return this;
        }

        public string GetHtml()
        {
            return element.GetHtml();
        }

        /// <summary>
        ///     If there are multiple elements that can be found using the same locator,
        ///     find one that is displayed and enabled.
        /// </summary>
        /// <returns>The element found</returns>
        public Element GetVisibleElement()
        {
            element = driver.FindVisibleElement(@by);
            return this;
        }

        /// <summary>
        ///     Move the mouse over the element
        /// </summary>
        public Element MouseOver()
        {
            element.MouseOver();
            return this;
        }

        public Element ScrollIntoView()
        {
            element.ScrollIntoView();
            return this;
        }

        /// <summary>
        ///     WithParam swaps out {0} in the locator with the value entered
        ///     This allows us to adjust for params with specific strings
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Element WithParam(string value)
        {
            if (by == null)
                throw new Exception("WithParams only works with Elements instantiated using a By locator");
            var oldBy = by.ToString();
            var toks = oldBy.Split(':');
            var type = toks[0];
            var locator = toks[1];
            var newlocator = locator.Replace("{0}", value);
            if (type.Contains("ClassName"))
            {
                by = By.ClassName(newlocator);
            }
            if (type.Contains("XPath"))
            {
                by = By.XPath(newlocator);
            }
            if (type.Contains("Id"))
            {
                by = By.Id(newlocator);
            }
            if (type.Contains("PartialLink"))
            {
                by = By.PartialLinkText(newlocator);
            }
            if (type.Contains("LinkText"))
            {
                by = By.LinkText(newlocator);
            }
            if (type.Contains("Name"))
            {
                by = By.Name(newlocator);
            }
            if (type.Contains("CssSelector"))
            {
                by = By.CssSelector(newlocator);
            }
            if (type.Contains("TagName"))
            {
                by = By.TagName(newlocator);
            }
            return this;
        }

        /// <summary>
        ///     Swaps out {0},{1},{2}..etc with the values in values array
        ///     //div[contains(text(),'{0}) and contains(@class,'{1})]
        ///     with values[] = {"textOfElement","classofElement"} becomes
        ///     //div[contains(text(),'textOfElement') and contains(@class,'classOfElement)]
        ///     will not work if element was instantiated with an existing IWebELement instead of a By locator.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public Element WithParams(string[] values)
        {
            if (by == null)
                throw new Exception("WithParams only works with Elements instantiated using a By locator");
            var oldBy = by.ToString();
            var toks = oldBy.Split(':');
            var type = toks[0];
            var locator = toks[1];
            for (var i = 0; i < values.Length; i++)
            {
                locator = locator.Replace("{" + i + "}", values[i]);
            }
            if (type.Contains("ClassName"))
            {
                by = By.ClassName(locator);
            }
            if (type.Contains("XPath"))
            {
                by = By.XPath(locator);
            }
            if (type.Contains("Id"))
            {
                by = By.Id(locator);
            }
            if (type.Contains("PartialLink"))
            {
                by = By.PartialLinkText(locator);
            }
            if (type.Contains("LinkText"))
            {
                by = By.LinkText(locator);
            }
            if (type.Contains("Name"))
            {
                by = By.Name(locator);
            }
            if (type.Contains("CssSelector"))
            {
                by = By.CssSelector(locator);
            }
            if (type.Contains("TagName"))
            {
                by = By.TagName(locator);
            }
            return this;
        }
    }
}