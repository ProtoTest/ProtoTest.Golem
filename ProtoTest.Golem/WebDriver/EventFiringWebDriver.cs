// Type: OpenQA.Selenium.Support.Events.EventFiringWebDriver
// Assembly: WebDriver.Support, Version=2.40.0.0, Culture=neutral
// MVID: 9FAA975A-389C-466A-AE2E-96ABC7996728
// Assembly location: C:\Users\Brian\Documents\GitHub\Golem\Golem\packages\Selenium.Support.2.40.0\lib\net40\WebDriver.Support.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.Events;

namespace Golem.WebDriver
{
    /// <summary>
    ///     A wrapper around an arbitrary WebDriver instance which supports registering for
    ///     events, e.g. for logging purposes.
    /// </summary>
    public class EventFiringWebDriver : IWebDriver, ISearchContext, IDisposable, IJavaScriptExecutor, ITakesScreenshot,
        IWrapsDriver
    {
        /// <summary>
        ///     Initializes a new instance of the EventFiringWebDriver class.
        /// </summary>
        /// <param name="parentDriver">The driver to register events for.</param>
        public EventFiringWebDriver(IWebDriver parentDriver)
        {
            WrappedDriver = parentDriver;
        }

        /// <summary>
        ///     Executes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to execute.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>
        ///     The value returned by the script.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The
        ///         <see cref="M:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ExecuteScript(System.String,System.Object[])" />
        ///         method executes JavaScript in the context of
        ///         the currently selected frame or window. This means that "document" will refer
        ///         to the current document. If the script has a return value, then the following
        ///         steps will be taken:
        ///     </para>
        ///     <para>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     For an HTML element, this method returns a <see cref="T:OpenQA.Selenium.IWebElement" />
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>For a number, a <see cref="T:System.Int64" /> is returned</description>
        ///             </item>
        ///             <item>
        ///                 <description>For a boolean, a <see cref="T:System.Boolean" /> is returned</description>
        ///             </item>
        ///             <item>
        ///                 <description>For all other cases a <see cref="T:System.String" /> is returned.</description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     For an array,we check the first element, and attempt to return a
        ///                     <see cref="T:System.Collections.Generic.List`1" /> of that type, following the rules above. Nested
        ///                     lists are not
        ///                     supported.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If the value is null or there is no return value,
        ///                     <see langword="null" /> is returned.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        ///     <para>
        ///         Arguments must be a number (which will be converted to a <see cref="T:System.Int64" />),
        ///         a <see cref="T:System.Boolean" />, a <see cref="T:System.String" /> or a
        ///         <see cref="T:OpenQA.Selenium.IWebElement" />.
        ///         An exception will be thrown if the arguments do not meet these criteria.
        ///         The arguments will be made available to the JavaScript via the "arguments" magic
        ///         variable, as if the function were called via "Function.apply"
        ///     </para>
        /// </remarks>
        public object ExecuteScript(string script, params object[] args)
        {
            var javaScriptExecutor = WrappedDriver as IJavaScriptExecutor;
            if (javaScriptExecutor == null)
                throw new NotSupportedException("Underlying driver instance does not support executing javascript");
            object obj;
            try
            {
                var objArray = UnwrapElementArguments(args);
                var e = new WebDriverScriptEventArgs(WrappedDriver, script);
                OnScriptExecuting(e);
                obj = javaScriptExecutor.ExecuteScript(script, objArray);
                OnScriptExecuted(e);
            }
            catch (Exception ex)
            {
                OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                throw;
            }
            return obj;
        }

        /// <summary>
        ///     Executes JavaScript asynchronously in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to execute.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>
        ///     The value returned by the script.
        /// </returns>
        public object ExecuteAsyncScript(string script, params object[] args)
        {
            var javaScriptExecutor = WrappedDriver as IJavaScriptExecutor;
            if (javaScriptExecutor == null)
                throw new NotSupportedException("Underlying driver instance does not support executing javascript");
            object obj;
            try
            {
                var objArray = UnwrapElementArguments(args);
                var e = new WebDriverScriptEventArgs(WrappedDriver, script);
                OnScriptExecuting(e);
                obj = javaScriptExecutor.ExecuteAsyncScript(script, objArray);
                OnScriptExecuted(e);
            }
            catch (Exception ex)
            {
                OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                throw;
            }
            return obj;
        }

        /// <summary>
        ///     Gets a <see cref="T:OpenQA.Selenium.Screenshot" /> object representing the image of the page on the screen.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:OpenQA.Selenium.Screenshot" /> object containing the image.
        /// </returns>
        public Screenshot GetScreenshot()
        {
            var takesScreenshot = WrappedDriver as ITakesScreenshot;
            if (WrappedDriver == null)
                throw new NotSupportedException("Underlying driver instance does not support taking screenshots");
            return takesScreenshot.GetScreenshot();
        }

        /// <summary>
        ///     Gets or sets the URL the browser is currently displaying.
        /// </summary>
        /// <remarks>
        ///     Setting the <see cref="P:OpenQA.Selenium.Support.Events.EventFiringWebDriver.Url" /> property will load a new web
        ///     page in the current browser window.
        ///     This is done using an HTTP GET operation, and the method will block until the
        ///     load is complete. This will follow redirects issued either by the server or
        ///     as a meta-redirect from within the returned HTML. Should a meta-redirect "rest"
        ///     for any duration of time, it is best to wait until this timeout is over, since
        ///     should the underlying page change while your test is executing the results of
        ///     future calls against this interface will be against the freshly loaded page.
        /// </remarks>
        /// <seealso cref="M:OpenQA.Selenium.INavigation.GoToUrl(System.String)" />
        /// <seealso cref="M:OpenQA.Selenium.INavigation.GoToUrl(System.Uri)" />
        public string Url
        {
            get
            {
                var str = string.Empty;
                try
                {
                    return WrappedDriver.Url;
                }
                catch (Exception ex)
                {
                    OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                    throw;
                }
            }
            set
            {
                try
                {
                    var e = new WebDriverNavigationEventArgs(WrappedDriver, value);
                    OnNavigating(e);
                    WrappedDriver.Url = value;
                    OnNavigated(e);
                }
                catch (Exception ex)
                {
                    OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                    throw;
                }
            }
        }

        /// <summary>
        ///     Gets the title of the current browser window.
        /// </summary>
        public string Title
        {
            get
            {
                var str = string.Empty;
                try
                {
                    return WrappedDriver.Title;
                }
                catch (Exception ex)
                {
                    OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                    throw;
                }
            }
        }

        /// <summary>
        ///     Gets the source of the page last loaded by the browser.
        /// </summary>
        /// <remarks>
        ///     If the page has been modified after loading (for example, by JavaScript)
        ///     there is no guarantee that the returned text is that of the modified page.
        ///     Please consult the documentation of the particular driver being used to
        ///     determine whether the returned text reflects the current state of the page
        ///     or the text last sent by the web server. The page source returned is a
        ///     representation of the underlying DOM: do not expect it to be formatted
        ///     or escaped in the same way as the response sent from the web server.
        /// </remarks>
        public string PageSource
        {
            get
            {
                var str = string.Empty;
                try
                {
                    return WrappedDriver.PageSource;
                }
                catch (Exception ex)
                {
                    OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                    throw;
                }
            }
        }

        /// <summary>
        ///     Gets the current window handle, which is an opaque handle to this
        ///     window that uniquely identifies it within this driver instance.
        /// </summary>
        public string CurrentWindowHandle
        {
            get
            {
                var str = string.Empty;
                try
                {
                    return WrappedDriver.CurrentWindowHandle;
                }
                catch (Exception ex)
                {
                    OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                    throw;
                }
            }
        }

        /// <summary>
        ///     Gets the window handles of open browser windows.
        /// </summary>
        public ReadOnlyCollection<string> WindowHandles
        {
            get
            {
                try
                {
                    return WrappedDriver.WindowHandles;
                }
                catch (Exception ex)
                {
                    OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                    throw;
                }
            }
        }

        /// <summary>
        ///     Close the current window, quitting the browser if it is the last window currently open.
        /// </summary>
        public void Close()
        {
            try
            {
                WrappedDriver.Close();
            }
            catch (Exception ex)
            {
                OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                throw;
            }
        }

        /// <summary>
        ///     Quits this driver, closing every associated window.
        /// </summary>
        public void Quit()
        {
            try
            {
                WrappedDriver.Quit();
            }
            catch (Exception ex)
            {
                OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                throw;
            }
        }

        /// <summary>
        ///     Instructs the driver to change its settings.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:OpenQA.Selenium.IOptions" /> object allowing the user to change
        ///     the settings of the driver.
        /// </returns>
        public IOptions Manage()
        {
            return new EventFiringOptions(this);
        }

        /// <summary>
        ///     Instructs the driver to navigate the browser to another location.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:OpenQA.Selenium.INavigation" /> object allowing the user to access
        ///     the browser's history and to navigate to a given URL.
        /// </returns>
        public INavigation Navigate()
        {
            return new EventFiringNavigation(this);
        }

        /// <summary>
        ///     Instructs the driver to send future commands to a different frame or window.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:OpenQA.Selenium.ITargetLocator" /> object which can be used to select
        ///     a frame or window.
        /// </returns>
        public ITargetLocator SwitchTo()
        {
            return new EventFiringTargetLocator(this);
        }

        /// <summary>
        ///     Find the first <see cref="T:OpenQA.Selenium.IWebElement" /> using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        ///     The first matching <see cref="T:OpenQA.Selenium.IWebElement" /> on the current context.
        /// </returns>
        /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
        public IWebElement FindElement(By by)
        {
            try
            {
                var e = new FindElementEventArgs(WrappedDriver, by);
                OnFindingElement(e);
                var element = WrappedDriver.FindElements(by).FirstOrDefault(x => x.Displayed);
                OnFindElementCompleted(e);
                var f = new FoundElementEventArgs(WrappedDriver, element, by);
                OnFoundElement(f);
                return WrapElement(element);
            }
            catch (Exception ex)
            {
                OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                throw;
            }
        }

        /// <summary>
        ///     Find all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context
        ///     using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        ///     A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of all
        ///     <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
        ///     matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            var list = new List<IWebElement>();
            try
            {
                var e = new FindElementEventArgs(WrappedDriver, by);
                OnFindingElement(e);
                var elements = WrappedDriver.FindElements(by).Where(x => x.Displayed);
                OnFindElementCompleted(e);
                foreach (var underlyingElement in elements)
                {
                    var webElement = WrapElement(underlyingElement);
                    list.Add(webElement);
                    var f = new FoundElementEventArgs(WrappedDriver, underlyingElement, by);
                    OnFoundElement(f);
                }
            }
            catch (Exception ex)
            {
                OnException(new WebDriverExceptionEventArgs(WrappedDriver, ex));
                throw;
            }
            return list.AsReadOnly();
        }

        /// <summary>
        ///     Frees all managed and unmanaged resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets the <see cref="T:OpenQA.Selenium.IWebDriver" /> wrapped by this EventsFiringWebDriver instance.
        /// </summary>
        public IWebDriver WrappedDriver { get; private set; }

        /// <summary>
        ///     Fires before the driver begins navigation.
        /// </summary>
        public event EventHandler<WebDriverNavigationEventArgs> Navigating;

        /// <summary>
        ///     Fires after the driver completes navigation
        /// </summary>
        public event EventHandler<WebDriverNavigationEventArgs> Navigated;

        /// <summary>
        ///     Fires before the driver begins navigation back one entry in the browser history list.
        /// </summary>
        public event EventHandler<WebDriverNavigationEventArgs> NavigatingBack;

        /// <summary>
        ///     Fires after the driver completes navigation back one entry in the browser history list.
        /// </summary>
        public event EventHandler<WebDriverNavigationEventArgs> NavigatedBack;

        /// <summary>
        ///     Fires before the driver begins navigation forward one entry in the browser history list.
        /// </summary>
        public event EventHandler<WebDriverNavigationEventArgs> NavigatingForward;

        /// <summary>
        ///     Fires after the driver completes navigation forward one entry in the browser history list.
        /// </summary>
        public event EventHandler<WebDriverNavigationEventArgs> NavigatedForward;

        /// <summary>
        ///     Fires before the driver clicks on an element.
        /// </summary>
        public event EventHandler<WebElementEventArgs> ElementClicking;

        /// <summary>
        ///     Fires after the driver has clicked on an element.
        /// </summary>
        public event EventHandler<WebElementEventArgs> ElementClicked;

        /// <summary>
        ///     Fires before the driver changes the value of an element via Clear(), SendKeys() or Toggle().
        /// </summary>
        public event EventHandler<WebElementEventArgs> ElementValueChanging;

        /// <summary>
        ///     Fires after the driver has changed the value of an element via Clear(), SendKeys() or Toggle().
        /// </summary>
        public event EventHandler<WebElementEventArgs> ElementValueChanged;

        /// <summary>
        ///     Fires before the driver starts to find an element.
        /// </summary>
        public event EventHandler<FindElementEventArgs> FindingElement;

        /// <summary>
        ///     Fires after the driver completes finding an element.
        /// </summary>
        public event EventHandler<FindElementEventArgs> FindElementCompleted;

        /// <summary>
        ///     Fires after the driver completes finding an element.
        /// </summary>
        public event EventHandler<FoundElementEventArgs> FoundElement;

        /// <summary>
        ///     Fires before a script is executed.
        /// </summary>
        public event EventHandler<WebDriverScriptEventArgs> ScriptExecuting;

        /// <summary>
        ///     Fires after a script is executed.
        /// </summary>
        public event EventHandler<WebDriverScriptEventArgs> ScriptExecuted;

        /// <summary>
        ///     Fires when an exception is thrown.
        /// </summary>
        public event EventHandler<WebDriverExceptionEventArgs> ExceptionThrown;

        /// <summary>
        ///     Frees all managed and, optionally, unmanaged resources used by this instance.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to dispose of only managed resources;
        ///     <see langword="false" /> to dispose of managed and unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            WrappedDriver.Dispose();
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.Navigating" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnNavigating(WebDriverNavigationEventArgs e)
        {
            if (Navigating == null)
                return;
            Navigating(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.Navigated" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnNavigated(WebDriverNavigationEventArgs e)
        {
            if (Navigated == null)
                return;
            Navigated(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatingBack" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnNavigatingBack(WebDriverNavigationEventArgs e)
        {
            if (NavigatingBack == null)
                return;
            NavigatingBack(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatedBack" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnNavigatedBack(WebDriverNavigationEventArgs e)
        {
            if (NavigatedBack == null)
                return;
            NavigatedBack(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatingForward" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnNavigatingForward(WebDriverNavigationEventArgs e)
        {
            if (NavigatingForward == null)
                return;
            NavigatingForward(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatedForward" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnNavigatedForward(WebDriverNavigationEventArgs e)
        {
            if (NavigatedForward == null)
                return;
            NavigatedForward(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementClicking" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs" /> that contains the event data.</param>
        protected virtual void OnElementClicking(WebElementEventArgs e)
        {
            if (ElementClicking == null)
                return;
            ElementClicking(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementClicked" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs" /> that contains the event data.</param>
        protected virtual void OnElementClicked(WebElementEventArgs e)
        {
            if (ElementClicked == null)
                return;
            ElementClicked(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementValueChanging" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs" /> that contains the event data.</param>
        protected virtual void OnElementValueChanging(WebElementEventArgs e)
        {
            if (ElementValueChanging == null)
                return;
            ElementValueChanging(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementValueChanged" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs" /> that contains the event data.</param>
        protected virtual void OnElementValueChanged(WebElementEventArgs e)
        {
            if (ElementValueChanged == null)
                return;
            ElementValueChanged(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.FindingElement" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.FindElementEventArgs" /> that contains the event data.</param>
        protected virtual void OnFindingElement(FindElementEventArgs e)
        {
            if (FindingElement == null)
                return;
            FindingElement(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.FindElementCompleted" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.FindElementEventArgs" /> that contains the event data.</param>
        protected virtual void OnFindElementCompleted(FindElementEventArgs e)
        {
            if (FindElementCompleted == null)
                return;
            FindElementCompleted(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.FindElementCompleted" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.FindElementEventArgs" /> that contains the event data.</param>
        protected virtual void OnFoundElement(FoundElementEventArgs e)
        {
            if (FoundElement == null)
                return;
            FoundElement(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ScriptExecuting" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverScriptEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnScriptExecuting(WebDriverScriptEventArgs e)
        {
            if (ScriptExecuting == null)
                return;
            ScriptExecuting(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ScriptExecuted" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverScriptEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnScriptExecuted(WebDriverScriptEventArgs e)
        {
            if (ScriptExecuted == null)
                return;
            ScriptExecuted(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ExceptionThrown" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverExceptionEventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnException(WebDriverExceptionEventArgs e)
        {
            if (ExceptionThrown == null)
                return;
            ExceptionThrown(this, e);
        }

        private static object[] UnwrapElementArguments(object[] args)
        {
            var list = new List<object>();
            foreach (var obj in args)
            {
                var firingWebElement = obj as EventFiringWebElement;
                if (firingWebElement != null)
                    list.Add(firingWebElement.WrappedElement);
                else
                    list.Add(obj);
            }
            return list.ToArray();
        }

        private IWebElement WrapElement(IWebElement underlyingElement)
        {
            return new EventFiringWebElement(this, underlyingElement);
        }

        /// <summary>
        ///     Provides a mechanism for Navigating with the driver.
        /// </summary>
        private class EventFiringNavigation : INavigation
        {
            private readonly EventFiringWebDriver parentDriver;
            private readonly INavigation wrappedNavigation;

            /// <summary>
            ///     Initializes a new instance of the EventFiringNavigation class
            /// </summary>
            /// <param name="driver">Driver in use</param>
            public EventFiringNavigation(EventFiringWebDriver driver)
            {
                parentDriver = driver;
                wrappedNavigation = parentDriver.WrappedDriver.Navigate();
            }

            /// <summary>
            ///     Move the browser back
            /// </summary>
            public void Back()
            {
                try
                {
                    var e = new WebDriverNavigationEventArgs(parentDriver);
                    parentDriver.OnNavigatingBack(e);
                    wrappedNavigation.Back();
                    parentDriver.OnNavigatedBack(e);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Move the browser forward
            /// </summary>
            public void Forward()
            {
                try
                {
                    var e = new WebDriverNavigationEventArgs(parentDriver);
                    parentDriver.OnNavigatingForward(e);
                    wrappedNavigation.Forward();
                    parentDriver.OnNavigatedForward(e);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Navigate to a url for your test
            /// </summary>
            /// <param name="url">String of where you want the browser to go to</param>
            public void GoToUrl(string url)
            {
                try
                {
                    var e = new WebDriverNavigationEventArgs(parentDriver, url);
                    parentDriver.OnNavigating(e);
                    wrappedNavigation.GoToUrl(url);
                    parentDriver.OnNavigated(e);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Navigate to a url for your test
            /// </summary>
            /// <param name="url">Uri object of where you want the browser to go to</param>
            public void GoToUrl(Uri url)
            {
                if (url == null)
                    throw new ArgumentNullException("url", "url cannot be null");
                try
                {
                    var e = new WebDriverNavigationEventArgs(parentDriver, url.ToString());
                    parentDriver.OnNavigating(e);
                    wrappedNavigation.GoToUrl(url);
                    parentDriver.OnNavigated(e);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Refresh the browser
            /// </summary>
            public void Refresh()
            {
                try
                {
                    wrappedNavigation.Refresh();
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }
        }

        /// <summary>
        ///     Provides a mechanism for setting options needed for the driver during the test.
        /// </summary>
        private class EventFiringOptions : IOptions
        {
            private readonly IOptions wrappedOptions;

            public ILogs Logs
            {
                get { return wrappedOptions.Logs; }
            }

            /// <summary>
            ///     Initializes a new instance of the EventFiringOptions class
            /// </summary>
            /// <param name="driver">Instance of the driver currently in use</param>
            public EventFiringOptions(EventFiringWebDriver driver)
            {
                wrappedOptions = driver.WrappedDriver.Manage();
            }

            /// <summary>
            ///     Gets an object allowing the user to manipulate cookies on the page.
            /// </summary>
            public ICookieJar Cookies
            {
                get { return wrappedOptions.Cookies; }
            }

            /// <summary>
            ///     Gets an object allowing the user to manipulate the currently-focused browser window.
            /// </summary>
            /// <remarks>
            ///     "Currently-focused" is defined as the browser window having the window handle
            ///     returned when IWebDriver.CurrentWindowHandle is called.
            /// </remarks>
            public IWindow Window
            {
                get { return wrappedOptions.Window; }
            }

            /// <summary>
            ///     Provides access to the timeouts defined for this driver.
            /// </summary>
            /// <returns>
            ///     An object implementing the <see cref="T:OpenQA.Selenium.ITimeouts" /> interface.
            /// </returns>
            public ITimeouts Timeouts()
            {
                return new EventFiringTimeouts(wrappedOptions);
            }
        }

        /// <summary>
        ///     Provides a mechanism for finding elements on the page with locators.
        /// </summary>
        private class EventFiringTargetLocator : ITargetLocator
        {
            private readonly EventFiringWebDriver parentDriver;
            private readonly ITargetLocator wrappedLocator;

            /// <summary>
            ///     Initializes a new instance of the EventFiringTargetLocator class
            /// </summary>
            /// <param name="driver">The driver that is currently in use</param>
            public EventFiringTargetLocator(EventFiringWebDriver driver)
            {
                parentDriver = driver;
                wrappedLocator = parentDriver.WrappedDriver.SwitchTo();
            }

            /// <summary>
            ///     Move to a different frame using its index
            /// </summary>
            /// <param name="frameIndex">The index of the </param>
            /// <returns>
            ///     A WebDriver instance that is currently in use
            /// </returns>
            public IWebDriver Frame(int frameIndex)
            {
                try
                {
                    return wrappedLocator.Frame(frameIndex);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Move to different frame using its name
            /// </summary>
            /// <param name="frameName">name of the frame</param>
            /// <returns>
            ///     A WebDriver instance that is currently in use
            /// </returns>
            public IWebDriver Frame(string frameName)
            {
                try
                {
                    return wrappedLocator.Frame(frameName);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Move to a frame element.
            /// </summary>
            /// <param name="frameElement">a previously found FRAME or IFRAME element.</param>
            /// <returns>
            ///     A WebDriver instance that is currently in use.
            /// </returns>
            public IWebDriver Frame(IWebElement frameElement)
            {
                try
                {
                    return wrappedLocator.Frame((frameElement as IWrapsElement).WrappedElement);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Change to the Window by passing in the name
            /// </summary>
            /// <param name="windowName">name of the window that you wish to move to</param>
            /// <returns>
            ///     A WebDriver instance that is currently in use
            /// </returns>
            public IWebDriver Window(string windowName)
            {
                try
                {
                    return wrappedLocator.Window(windowName);
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Change the active frame to the default
            /// </summary>
            /// <returns>
            ///     Element of the default
            /// </returns>
            public IWebDriver DefaultContent()
            {
                try
                {
                    return wrappedLocator.DefaultContent();
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Finds the active element on the page and returns it
            /// </summary>
            /// <returns>
            ///     Element that is active
            /// </returns>
            public IWebElement ActiveElement()
            {
                try
                {
                    return wrappedLocator.ActiveElement();
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Switches to the currently active modal dialog for this particular driver instance.
            /// </summary>
            /// <returns>
            ///     A handle to the dialog.
            /// </returns>
            public IAlert Alert()
            {
                try
                {
                    return wrappedLocator.Alert();
                }
                catch (Exception ex)
                {
                    parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, ex));
                    throw;
                }
            }

            public IWebDriver ParentFrame()
            {
                return wrappedLocator.DefaultContent();
            }
        }

        /// <summary>
        ///     Defines the interface through which the user can define timeouts.
        /// </summary>
        private class EventFiringTimeouts : ITimeouts
        {
            private readonly ITimeouts wrappedTimeouts;

            /// <summary>
            ///     Initializes a new instance of the EventFiringTimeouts class
            /// </summary>
            /// <param name="options">The <see cref="T:OpenQA.Selenium.IOptions" /> object to wrap.</param>
            public EventFiringTimeouts(IOptions options)
            {
                wrappedTimeouts = options.Timeouts();
            }

            /// <summary>
            ///     Specifies the amount of time the driver should wait when searching for an
            ///     element if it is not immediately present.
            /// </summary>
            /// <param name="timeToWait">A <see cref="T:System.TimeSpan" /> structure defining the amount of time to wait.</param>
            /// <returns>
            ///     A self reference
            /// </returns>
            /// <remarks>
            ///     When searching for a single element, the driver should poll the page
            ///     until the element has been found, or this timeout expires before throwing
            ///     a <see cref="T:OpenQA.Selenium.NoSuchElementException" />. When searching for multiple elements,
            ///     the driver should poll the page until at least one element has been found
            ///     or this timeout has expired.
            ///     <para>
            ///         Increasing the implicit wait timeout should be used judiciously as it
            ///         will have an adverse effect on test run time, especially when used with
            ///         slower location strategies like XPath.
            ///     </para>
            /// </remarks>
            public ITimeouts ImplicitlyWait(TimeSpan timeToWait)
            {
                return wrappedTimeouts.ImplicitlyWait(timeToWait);
            }

            /// <summary>
            ///     Specifies the amount of time the driver should wait when executing JavaScript asynchronously.
            /// </summary>
            /// <param name="timeToWait">A <see cref="T:System.TimeSpan" /> structure defining the amount of time to wait.</param>
            /// <returns>
            ///     A self reference
            /// </returns>
            public ITimeouts SetScriptTimeout(TimeSpan timeToWait)
            {
                return wrappedTimeouts.SetScriptTimeout(timeToWait);
            }

            /// <summary>
            ///     Specifies the amount of time the driver should wait for a page to load when setting the
            ///     <see cref="P:OpenQA.Selenium.IWebDriver.Url" /> property.
            /// </summary>
            /// <param name="timeToWait">A <see cref="T:System.TimeSpan" /> structure defining the amount of time to wait.</param>
            /// <returns>
            ///     A self reference
            /// </returns>
            public ITimeouts SetPageLoadTimeout(TimeSpan timeToWait)
            {
                wrappedTimeouts.SetPageLoadTimeout(timeToWait);
                return this;
            }
        }

        /// <summary>
        ///     EventFiringWebElement allows you to have access to specific items that are found on the page
        /// </summary>
        private class EventFiringWebElement : IWebElement, ISearchContext, IWrapsElement, IWrapsDriver
        {
            /// <summary>
            ///     Initializes a new instance of the
            ///     <see cref="T:OpenQA.Selenium.Support.Events.EventFiringWebDriver.EventFiringWebElement" /> class.
            /// </summary>
            /// <param name="driver">
            ///     The <see cref="T:OpenQA.Selenium.Support.Events.EventFiringWebDriver" /> instance hosting this
            ///     element.
            /// </param>
            /// <param name="element">The <see cref="T:OpenQA.Selenium.IWebElement" /> to wrap for event firing.</param>
            public EventFiringWebElement(EventFiringWebDriver driver, IWebElement element)
            {
                WrappedElement = element;
                ParentDriver = driver;
            }

            /// <summary>
            ///     Gets the underlying EventFiringWebDriver for this element.
            /// </summary>
            protected EventFiringWebDriver ParentDriver { get; private set; }

            /// <summary>
            ///     Gets the DOM Tag of element
            /// </summary>
            public string TagName
            {
                get
                {
                    var str = string.Empty;
                    try
                    {
                        return WrappedElement.TagName;
                    }
                    catch (Exception ex)
                    {
                        ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                        throw;
                    }
                }
            }

            /// <summary>
            ///     Gets the text from the element
            /// </summary>
            public string Text
            {
                get
                {
                    var str = string.Empty;
                    try
                    {
                        return WrappedElement.Text;
                    }
                    catch (Exception ex)
                    {
                        ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                        throw;
                    }
                }
            }

            /// <summary>
            ///     Gets a value indicating whether an element is currently enabled
            /// </summary>
            public bool Enabled
            {
                get
                {
                    try
                    {
                        return WrappedElement.Enabled;
                    }
                    catch (Exception ex)
                    {
                        ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                        throw;
                    }
                }
            }

            /// <summary>
            ///     Gets a value indicating whether this element is selected or not. This operation only applies to input elements such
            ///     as checkboxes, options in a select and radio buttons.
            /// </summary>
            public bool Selected
            {
                get
                {
                    try
                    {
                        return WrappedElement.Selected;
                    }
                    catch (Exception ex)
                    {
                        ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                        throw;
                    }
                }
            }

            /// <summary>
            ///     Gets the Location of an element and returns a Point object
            /// </summary>
            public Point Location
            {
                get
                {
                    var point = new Point();
                    try
                    {
                        return WrappedElement.Location;
                    }
                    catch (Exception ex)
                    {
                        ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                        throw;
                    }
                }
            }

            /// <summary>
            ///     Gets the <see cref="P:OpenQA.Selenium.Support.Events.EventFiringWebDriver.EventFiringWebElement.Size" /> of the
            ///     element on the page
            /// </summary>
            public Size Size
            {
                get
                {
                    var size = new Size();
                    try
                    {
                        return WrappedElement.Size;
                    }
                    catch (Exception ex)
                    {
                        ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                        throw;
                    }
                }
            }

            /// <summary>
            ///     Gets a value indicating whether the element is currently being displayed
            /// </summary>
            public bool Displayed
            {
                get
                {
                    try
                    {
                        return WrappedElement.Displayed;
                    }
                    catch (Exception ex)
                    {
                        ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                        throw;
                    }
                }
            }

            /// <summary>
            ///     Method to clear the text out of an Input element
            /// </summary>
            public void Clear()
            {
                try
                {
                    var e = new WebElementEventArgs(ParentDriver.WrappedDriver, WrappedElement);
                    ParentDriver.OnElementValueChanging(e);
                    WrappedElement.Clear();
                    ParentDriver.OnElementValueChanged(e);
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Method for sending native key strokes to the browser
            /// </summary>
            /// <param name="text">String containing what you would like to type onto the screen</param>
            public void SendKeys(string text)
            {
                try
                {
                    var e = new WebElementEventArgs(ParentDriver.WrappedDriver, WrappedElement);
                    ParentDriver.OnElementValueChanging(e);
                    WrappedElement.SendKeys(text);
                    ParentDriver.OnElementValueChanged(e);
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     If this current element is a form, or an element within a form, then this will be submitted to the remote server.
            ///     If this causes the current page to change, then this method will block until the new page is loaded.
            /// </summary>
            public void Submit()
            {
                try
                {
                    WrappedElement.Submit();
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Click this element. If this causes a new page to load, this method will block until
            ///     the page has loaded. At this point, you should discard all references to this element
            ///     and any further operations performed on this element will have undefined behavior unless
            ///     you know that the element and the page will still be present. If this element is not
            ///     clickable, then this operation is a no-op since it's pretty common for someone to
            ///     accidentally miss  the target when clicking in Real Life
            /// </summary>
            public void Click()
            {
                try
                {
                    var e = new WebElementEventArgs(ParentDriver.WrappedDriver, WrappedElement);
                    ParentDriver.OnElementClicking(e);
                    WrappedElement.Click();
                    ParentDriver.OnElementClicked(e);
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     If this current element is a form, or an element within a form, then this will be submitted to the remote server.
            ///     If this causes the current page to change, then this method will block until the new page is loaded.
            /// </summary>
            /// <param name="attributeName">Attribute you wish to get details of</param>
            /// <returns>
            ///     The attribute's current value or null if the value is not set.
            /// </returns>
            public string GetAttribute(string attributeName)
            {
                var str = string.Empty;
                try
                {
                    return WrappedElement.GetAttribute(attributeName);
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Method to return the value of a CSS Property
            /// </summary>
            /// <param name="propertyName">CSS property key</param>
            /// <returns>
            ///     string value of the CSS property
            /// </returns>
            public string GetCssValue(string propertyName)
            {
                var str = string.Empty;
                try
                {
                    return WrappedElement.GetCssValue(propertyName);
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Finds the first element in the page that matches the <see cref="T:OpenQA.Selenium.By" /> object
            /// </summary>
            /// <param name="by">By mechanism to find the element</param>
            /// <returns>
            ///     IWebElement object so that you can interaction that object
            /// </returns>
            public IWebElement FindElement(By by)
            {
                try
                {
                    var e = new FindElementEventArgs(ParentDriver.WrappedDriver, WrappedElement, by);
                    ParentDriver.OnFindingElement(e);
                    WrappedElement.Highlight(100,"green");
                    var element = WrappedElement.FindElement(by);
                    ParentDriver.OnFindElementCompleted(e);
                    var f = new FoundElementEventArgs(ParentDriver.WrappedDriver, element, by);
                    ParentDriver.OnFoundElement(f);
                    return ParentDriver.WrapElement(element);
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
            }

            /// <summary>
            ///     Finds the elements on the page by using the <see cref="T:OpenQA.Selenium.By" /> object and returns a
            ///     ReadOnlyCollection of the Elements on the page
            /// </summary>
            /// <param name="by">By mechanism to find the element</param>
            /// <returns>
            ///     ReadOnlyCollection of IWebElement
            /// </returns>
            public ReadOnlyCollection<IWebElement> FindElements(By by)
            {
                var list = new List<IWebElement>();
                try
                {
                    var e = new FindElementEventArgs(ParentDriver.WrappedDriver, WrappedElement, by);
                    ParentDriver.OnFindingElement(e);
                    WrappedElement.Highlight(100, "green");
                    var elements = WrappedElement.FindElements(by);
                    ParentDriver.OnFindElementCompleted(e);
                    foreach (var underlyingElement in elements)
                    {
                        var webElement = ParentDriver.WrapElement(underlyingElement);
                        list.Add(webElement);
                        var f = new FoundElementEventArgs(ParentDriver.WrappedDriver, webElement, by);
                        ParentDriver.OnFoundElement(f);
                    }
                }
                catch (Exception ex)
                {
                    ParentDriver.OnException(new WebDriverExceptionEventArgs(ParentDriver, ex));
                    throw;
                }
                return list.AsReadOnly();
            }

            public IWebDriver WrappedDriver
            {
                get { return ParentDriver; }
            }

            /// <summary>
            ///     Gets the underlying wrapped <see cref="T:OpenQA.Selenium.IWebElement" />.
            /// </summary>
            public IWebElement WrappedElement { get; private set; }
        }
    }
}