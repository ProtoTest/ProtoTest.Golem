// Type: OpenQA.Selenium.Support.Events.EventFiringWebDriver
// Assembly: WebDriver.Support, Version=2.40.0.0, Culture=neutral
// MVID: 9FAA975A-389C-466A-AE2E-96ABC7996728
// Assembly location: C:\Users\Brian\Documents\GitHub\ProtoTest.Golem\ProtoTest.Golem\packages\Selenium.Support.2.40.0\lib\net40\WebDriver.Support.dll

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium.Support.Events;

namespace ProtoTest.Golem.WebDriver
{
  /// <summary>
  /// A wrapper around an arbitrary WebDriver instance which supports registering for
  ///             events, e.g. for logging purposes.
  /// 
  /// </summary>
  public class EventFiringWebDriver : IWebDriver, ISearchContext, IDisposable, IJavaScriptExecutor, ITakesScreenshot, IWrapsDriver
  {
    private IWebDriver driver;

    /// <summary>
    /// Gets the <see cref="T:OpenQA.Selenium.IWebDriver"/> wrapped by this EventsFiringWebDriver instance.
    /// 
    /// </summary>
    public IWebDriver WrappedDriver
    {
      get
      {
        return this.driver;
      }
    }

    /// <summary>
    /// Gets or sets the URL the browser is currently displaying.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// Setting the <see cref="P:OpenQA.Selenium.Support.Events.EventFiringWebDriver.Url"/> property will load a new web page in the current browser window.
    ///             This is done using an HTTP GET operation, and the method will block until the
    ///             load is complete. This will follow redirects issued either by the server or
    ///             as a meta-redirect from within the returned HTML. Should a meta-redirect "rest"
    ///             for any duration of time, it is best to wait until this timeout is over, since
    ///             should the underlying page change while your test is executing the results of
    ///             future calls against this interface will be against the freshly loaded page.
    /// 
    /// </remarks>
    /// <seealso cref="M:OpenQA.Selenium.INavigation.GoToUrl(System.String)"/><seealso cref="M:OpenQA.Selenium.INavigation.GoToUrl(System.Uri)"/>
    public string Url
    {
      get
      {
        string str = string.Empty;
        try
        {
          return this.driver.Url;
        }
        catch (Exception ex)
        {
          this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
          throw;
        }
      }
      set
      {
        try
        {
          WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs(this.driver, value);
          this.OnNavigating(e);
          this.driver.Url = value;
          this.OnNavigated(e);
        }
        catch (Exception ex)
        {
          this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
          throw;
        }
      }
    }

    /// <summary>
    /// Gets the title of the current browser window.
    /// 
    /// </summary>
    public string Title
    {
      get
      {
        string str = string.Empty;
        try
        {
          return this.driver.Title;
        }
        catch (Exception ex)
        {
          this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
          throw;
        }
      }
    }

    /// <summary>
    /// Gets the source of the page last loaded by the browser.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// If the page has been modified after loading (for example, by JavaScript)
    ///             there is no guarantee that the returned text is that of the modified page.
    ///             Please consult the documentation of the particular driver being used to
    ///             determine whether the returned text reflects the current state of the page
    ///             or the text last sent by the web server. The page source returned is a
    ///             representation of the underlying DOM: do not expect it to be formatted
    ///             or escaped in the same way as the response sent from the web server.
    /// 
    /// </remarks>
    public string PageSource
    {
      get
      {
        string str = string.Empty;
        try
        {
          return this.driver.PageSource;
        }
        catch (Exception ex)
        {
          this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
          throw;
        }
      }
    }

    /// <summary>
    /// Gets the current window handle, which is an opaque handle to this
    ///             window that uniquely identifies it within this driver instance.
    /// 
    /// </summary>
    public string CurrentWindowHandle
    {
      get
      {
        string str = string.Empty;
        try
        {
          return this.driver.CurrentWindowHandle;
        }
        catch (Exception ex)
        {
          this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
          throw;
        }
      }
    }

    /// <summary>
    /// Gets the window handles of open browser windows.
    /// 
    /// </summary>
    public ReadOnlyCollection<string> WindowHandles
    {
      get
      {
        try
        {
          return this.driver.WindowHandles;
        }
        catch (Exception ex)
        {
          this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
          throw;
        }
      }
    }

    /// <summary>
    /// Fires before the driver begins navigation.
    /// 
    /// </summary>
    public event EventHandler<WebDriverNavigationEventArgs> Navigating;

    /// <summary>
    /// Fires after the driver completes navigation
    /// 
    /// </summary>
    public event EventHandler<WebDriverNavigationEventArgs> Navigated;

    /// <summary>
    /// Fires before the driver begins navigation back one entry in the browser history list.
    /// 
    /// </summary>
    public event EventHandler<WebDriverNavigationEventArgs> NavigatingBack;

    /// <summary>
    /// Fires after the driver completes navigation back one entry in the browser history list.
    /// 
    /// </summary>
    public event EventHandler<WebDriverNavigationEventArgs> NavigatedBack;

    /// <summary>
    /// Fires before the driver begins navigation forward one entry in the browser history list.
    /// 
    /// </summary>
    public event EventHandler<WebDriverNavigationEventArgs> NavigatingForward;

    /// <summary>
    /// Fires after the driver completes navigation forward one entry in the browser history list.
    /// 
    /// </summary>
    public event EventHandler<WebDriverNavigationEventArgs> NavigatedForward;

    /// <summary>
    /// Fires before the driver clicks on an element.
    /// 
    /// </summary>
    public event EventHandler<WebElementEventArgs> ElementClicking;

    /// <summary>
    /// Fires after the driver has clicked on an element.
    /// 
    /// </summary>
    public event EventHandler<WebElementEventArgs> ElementClicked;

    /// <summary>
    /// Fires before the driver changes the value of an element via Clear(), SendKeys() or Toggle().
    /// 
    /// </summary>
    public event EventHandler<WebElementEventArgs> ElementValueChanging;

    /// <summary>
    /// Fires after the driver has changed the value of an element via Clear(), SendKeys() or Toggle().
    /// 
    /// </summary>
    public event EventHandler<WebElementEventArgs> ElementValueChanged;

    /// <summary>
    /// Fires before the driver starts to find an element.
    /// 
    /// </summary>
    public event EventHandler<FindElementEventArgs> FindingElement;

    /// <summary>
    /// Fires after the driver completes finding an element.
    /// 
    /// </summary>
    public event EventHandler<FindElementEventArgs> FindElementCompleted;

    /// <summary>
    /// Fires after the driver completes finding an element.
    /// 
    /// </summary>
    public event EventHandler<FoundElementEventArgs> FoundElement;

    /// <summary>
    /// Fires before a script is executed.
    /// 
    /// </summary>
    public event EventHandler<WebDriverScriptEventArgs> ScriptExecuting;

    /// <summary>
    /// Fires after a script is executed.
    /// 
    /// </summary>
    public event EventHandler<WebDriverScriptEventArgs> ScriptExecuted;

    /// <summary>
    /// Fires when an exception is thrown.
    /// 
    /// </summary>
    public event EventHandler<WebDriverExceptionEventArgs> ExceptionThrown;

    /// <summary>
    /// Initializes a new instance of the EventFiringWebDriver class.
    /// 
    /// </summary>
    /// <param name="parentDriver">The driver to register events for.</param>
    public EventFiringWebDriver(IWebDriver parentDriver)
    {
      this.driver = parentDriver;
    }

    /// <summary>
    /// Close the current window, quitting the browser if it is the last window currently open.
    /// 
    /// </summary>
    public void Close()
    {
      try
      {
        this.driver.Close();
      }
      catch (Exception ex)
      {
        this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
        throw;
      }
    }

    /// <summary>
    /// Quits this driver, closing every associated window.
    /// 
    /// </summary>
    public void Quit()
    {
      try
      {
        this.driver.Quit();
      }
      catch (Exception ex)
      {
        this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
        throw;
      }
    }

    /// <summary>
    /// Instructs the driver to change its settings.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:OpenQA.Selenium.IOptions"/> object allowing the user to change
    ///             the settings of the driver.
    /// </returns>
    public IOptions Manage()
    {
      return (IOptions) new EventFiringWebDriver.EventFiringOptions(this);
    }

    /// <summary>
    /// Instructs the driver to navigate the browser to another location.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:OpenQA.Selenium.INavigation"/> object allowing the user to access
    ///             the browser's history and to navigate to a given URL.
    /// </returns>
    public INavigation Navigate()
    {
      return (INavigation) new EventFiringWebDriver.EventFiringNavigation(this);
    }

    /// <summary>
    /// Instructs the driver to send future commands to a different _frame or window.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:OpenQA.Selenium.ITargetLocator"/> object which can be used to select
    ///             a _frame or window.
    /// </returns>
    public ITargetLocator SwitchTo()
    {
      return (ITargetLocator) new EventFiringWebDriver.EventFiringTargetLocator(this);
    }

    /// <summary>
    /// Find the first <see cref="T:OpenQA.Selenium.IWebElement"/> using the given method.
    /// 
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>
    /// The first matching <see cref="T:OpenQA.Selenium.IWebElement"/> on the current context.
    /// </returns>
    /// <exception cref="T:OpenQA.Selenium.NoSuchElementException">If no element matches the criteria.</exception>
    public IWebElement FindElement(By by)
    {
      try
      {
        FindElementEventArgs e = new FindElementEventArgs(this.driver, by);
        this.OnFindingElement(e);
        IWebElement element = this.driver.FindElement(by);
        this.OnFindElementCompleted(e);
        FoundElementEventArgs f= new FoundElementEventArgs(driver,element,by);
        this.OnFoundElement(f);
        return this.WrapElement(element);
      }
      catch (Exception ex)
      {
        this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
        throw;
      }
    }


      /// <summary>
    /// Find all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context
    ///             using the given mechanism.
    /// 
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>
    /// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1"/> of all <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
    ///             matching the current criteria, or an empty list if nothing matches.
    /// </returns>
    public ReadOnlyCollection<IWebElement> FindElements(By by)
    {
      List<IWebElement> list = new List<IWebElement>();
      try
      {
        FindElementEventArgs e = new FindElementEventArgs(this.driver, by);
        this.OnFindingElement(e);
        ReadOnlyCollection<IWebElement> elements = this.driver.FindElements(by);
        this.OnFindElementCompleted(e);
        foreach (IWebElement underlyingElement in elements)
        {
          IWebElement webElement = this.WrapElement(underlyingElement);
          list.Add(webElement);
          FoundElementEventArgs f = new FoundElementEventArgs(driver, underlyingElement, by);
          this.OnFoundElement(f);
        }
      }
      catch (Exception ex)
      {
        this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
        throw;
      }
      return list.AsReadOnly();
    }

    /// <summary>
    /// Frees all managed and unmanaged resources used by this instance.
    /// 
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Executes JavaScript in the context of the currently selected _frame or window.
    /// 
    /// </summary>
    /// <param name="script">The JavaScript code to execute.</param><param name="args">The arguments to the script.</param>
    /// <returns>
    /// The value returned by the script.
    /// </returns>
    /// 
    /// <remarks>
    /// 
    /// <para>
    /// The <see cref="M:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ExecuteScript(System.String,System.Object[])"/>method executes JavaScript in the context of
    ///             the currently selected _frame or window. This means that "document" will refer
    ///             to the current document. If the script has a return value, then the following
    ///             steps will be taken:
    /// 
    /// </para>
    /// 
    /// <para>
    /// 
    /// <list type="bullet">
    /// 
    /// <item>
    /// <description>For an HTML element, this method returns a <see cref="T:OpenQA.Selenium.IWebElement"/></description>
    /// </item>
    /// 
    /// <item>
    /// <description>For a number, a <see cref="T:System.Int64"/> is returned</description>
    /// </item>
    /// 
    /// <item>
    /// <description>For a boolean, a <see cref="T:System.Boolean"/> is returned</description>
    /// </item>
    /// 
    /// <item>
    /// <description>For all other cases a <see cref="T:System.String"/> is returned.</description>
    /// </item>
    /// 
    /// <item>
    /// <description>For an array,we check the first element, and attempt to return a
    ///             <see cref="T:System.Collections.Generic.List`1"/> of that type, following the rules above. Nested lists are not
    ///             supported.</description>
    /// </item>
    /// 
    /// <item>
    /// <description>If the value is null or there is no return value,
    ///             <see langword="null"/> is returned.</description>
    /// </item>
    /// 
    /// </list>
    /// 
    /// </para>
    /// 
    /// <para>
    /// Arguments must be a number (which will be converted to a <see cref="T:System.Int64"/>),
    ///             a <see cref="T:System.Boolean"/>, a <see cref="T:System.String"/> or a <see cref="T:OpenQA.Selenium.IWebElement"/>.
    ///             An exception will be thrown if the arguments do not meet these criteria.
    ///             The arguments will be made available to the JavaScript via the "arguments" magic
    ///             variable, as if the function were called via "Function.apply"
    /// 
    /// </para>
    /// 
    /// </remarks>
    public object ExecuteScript(string script, params object[] args)
    {
      IJavaScriptExecutor javaScriptExecutor = this.driver as IJavaScriptExecutor;
      if (javaScriptExecutor == null)
        throw new NotSupportedException("Underlying driver instance does not support executing javascript");
      object obj;
      try
      {
        object[] objArray = EventFiringWebDriver.UnwrapElementArguments(args);
        WebDriverScriptEventArgs e = new WebDriverScriptEventArgs(this.driver, script);
        this.OnScriptExecuting(e);
        obj = javaScriptExecutor.ExecuteScript(script, objArray);
        this.OnScriptExecuted(e);
      }
      catch (Exception ex)
      {
        this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
        throw;
      }
      return obj;
    }

    /// <summary>
    /// Executes JavaScript asynchronously in the context of the currently selected _frame or window.
    /// 
    /// </summary>
    /// <param name="script">The JavaScript code to execute.</param><param name="args">The arguments to the script.</param>
    /// <returns>
    /// The value returned by the script.
    /// </returns>
    public object ExecuteAsyncScript(string script, params object[] args)
    {
      IJavaScriptExecutor javaScriptExecutor = this.driver as IJavaScriptExecutor;
      if (javaScriptExecutor == null)
        throw new NotSupportedException("Underlying driver instance does not support executing javascript");
      object obj;
      try
      {
        object[] objArray = EventFiringWebDriver.UnwrapElementArguments(args);
        WebDriverScriptEventArgs e = new WebDriverScriptEventArgs(this.driver, script);
        this.OnScriptExecuting(e);
        obj = javaScriptExecutor.ExecuteAsyncScript(script, objArray);
        this.OnScriptExecuted(e);
      }
      catch (Exception ex)
      {
        this.OnException(new WebDriverExceptionEventArgs(this.driver, ex));
        throw;
      }
      return obj;
    }

    /// <summary>
    /// Gets a <see cref="T:OpenQA.Selenium.Screenshot"/> object representing the image of the page on the screen.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:OpenQA.Selenium.Screenshot"/> object containing the image.
    /// </returns>
    public Screenshot GetScreenshot()
    {
      ITakesScreenshot takesScreenshot = this.driver as ITakesScreenshot;
      if (this.driver == null)
        throw new NotSupportedException("Underlying driver instance does not support taking screenshots");
      return takesScreenshot.GetScreenshot();
    }

    /// <summary>
    /// Frees all managed and, optionally, unmanaged resources used by this instance.
    /// 
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to dispose of only managed resources;
    ///             <see langword="false"/> to dispose of managed and unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.driver.Dispose();
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.Navigating"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs"/> that contains the event data.</param>
    protected virtual void OnNavigating(WebDriverNavigationEventArgs e)
    {
      if (this.Navigating == null)
        return;
      this.Navigating((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.Navigated"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs"/> that contains the event data.</param>
    protected virtual void OnNavigated(WebDriverNavigationEventArgs e)
    {
      if (this.Navigated == null)
        return;
      this.Navigated((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatingBack"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs"/> that contains the event data.</param>
    protected virtual void OnNavigatingBack(WebDriverNavigationEventArgs e)
    {
      if (this.NavigatingBack == null)
        return;
      this.NavigatingBack((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatedBack"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs"/> that contains the event data.</param>
    protected virtual void OnNavigatedBack(WebDriverNavigationEventArgs e)
    {
      if (this.NavigatedBack == null)
        return;
      this.NavigatedBack((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatingForward"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs"/> that contains the event data.</param>
    protected virtual void OnNavigatingForward(WebDriverNavigationEventArgs e)
    {
      if (this.NavigatingForward == null)
        return;
      this.NavigatingForward((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.NavigatedForward"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverNavigationEventArgs"/> that contains the event data.</param>
    protected virtual void OnNavigatedForward(WebDriverNavigationEventArgs e)
    {
      if (this.NavigatedForward == null)
        return;
      this.NavigatedForward((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementClicking"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs"/> that contains the event data.</param>
    protected virtual void OnElementClicking(WebElementEventArgs e)
    {
      if (this.ElementClicking == null)
        return;
      this.ElementClicking((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementClicked"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs"/> that contains the event data.</param>
    protected virtual void OnElementClicked(WebElementEventArgs e)
    {
      if (this.ElementClicked == null)
        return;
      this.ElementClicked((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementValueChanging"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs"/> that contains the event data.</param>
    protected virtual void OnElementValueChanging(WebElementEventArgs e)
    {
      if (this.ElementValueChanging == null)
        return;
      this.ElementValueChanging((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ElementValueChanged"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebElementEventArgs"/> that contains the event data.</param>
    protected virtual void OnElementValueChanged(WebElementEventArgs e)
    {
      if (this.ElementValueChanged == null)
        return;
      this.ElementValueChanged((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.FindingElement"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.FindElementEventArgs"/> that contains the event data.</param>
    protected virtual void OnFindingElement(FindElementEventArgs e)
    {
      if (this.FindingElement == null)
        return;
      this.FindingElement((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.FindElementCompleted"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.FindElementEventArgs"/> that contains the event data.</param>
    protected virtual void OnFindElementCompleted(FindElementEventArgs e)
    {
      if (this.FindElementCompleted == null)
        return;
      this.FindElementCompleted((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.FindElementCompleted"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.FindElementEventArgs"/> that contains the event data.</param>
    protected virtual void OnFoundElement(FoundElementEventArgs e)
    {
        if (this.FoundElement == null)
            return;
        this.FoundElement((object)this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ScriptExecuting"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverScriptEventArgs"/> that contains the event data.</param>
    protected virtual void OnScriptExecuting(WebDriverScriptEventArgs e)
    {
      if (this.ScriptExecuting == null)
        return;
      this.ScriptExecuting((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ScriptExecuted"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverScriptEventArgs"/> that contains the event data.</param>
    protected virtual void OnScriptExecuted(WebDriverScriptEventArgs e)
    {
      if (this.ScriptExecuted == null)
        return;
      this.ScriptExecuted((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:OpenQA.Selenium.Support.Events.EventFiringWebDriver.ExceptionThrown"/> event.
    /// 
    /// </summary>
    /// <param name="e">A <see cref="T:OpenQA.Selenium.Support.Events.WebDriverExceptionEventArgs"/> that contains the event data.</param>
    protected virtual void OnException(WebDriverExceptionEventArgs e)
    {
      if (this.ExceptionThrown == null)
        return;
      this.ExceptionThrown((object) this, e);
    }

    private static object[] UnwrapElementArguments(object[] args)
    {
      List<object> list = new List<object>();
      foreach (object obj in args)
      {
        EventFiringWebDriver.EventFiringWebElement firingWebElement = obj as EventFiringWebDriver.EventFiringWebElement;
        if (firingWebElement != null)
          list.Add((object) firingWebElement.WrappedElement);
        else
          list.Add(obj);
      }
      return list.ToArray();
    }

    private IWebElement WrapElement(IWebElement underlyingElement)
    {
      return (IWebElement) new EventFiringWebDriver.EventFiringWebElement(this, underlyingElement);
    }

    /// <summary>
    /// Provides a mechanism for Navigating with the driver.
    /// 
    /// </summary>
    private class EventFiringNavigation : INavigation
    {
      private EventFiringWebDriver parentDriver;
      private INavigation wrappedNavigation;

      /// <summary>
      /// Initializes a new instance of the EventFiringNavigation class
      /// 
      /// </summary>
      /// <param name="driver">Driver in use</param>
      public EventFiringNavigation(EventFiringWebDriver driver)
      {
        this.parentDriver = driver;
        this.wrappedNavigation = this.parentDriver.WrappedDriver.Navigate();
      }

      /// <summary>
      /// Move the browser back
      /// 
      /// </summary>
      public void Back()
      {
        try
        {
          WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs((IWebDriver) this.parentDriver);
          this.parentDriver.OnNavigatingBack(e);
          this.wrappedNavigation.Back();
          this.parentDriver.OnNavigatedBack(e);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Move the browser forward
      /// 
      /// </summary>
      public void Forward()
      {
        try
        {
          WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs((IWebDriver) this.parentDriver);
          this.parentDriver.OnNavigatingForward(e);
          this.wrappedNavigation.Forward();
          this.parentDriver.OnNavigatedForward(e);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Navigate to a url for your test
      /// 
      /// </summary>
      /// <param name="url">String of where you want the browser to go to</param>
      public void GoToUrl(string url)
      {
        try
        {
          WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs((IWebDriver) this.parentDriver, url);
          this.parentDriver.OnNavigating(e);
          this.wrappedNavigation.GoToUrl(url);
          this.parentDriver.OnNavigated(e);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Navigate to a url for your test
      /// 
      /// </summary>
      /// <param name="url">Uri object of where you want the browser to go to</param>
      public void GoToUrl(Uri url)
      {
        if (url == (Uri) null)
          throw new ArgumentNullException("url", "url cannot be null");
        try
        {
          WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs((IWebDriver) this.parentDriver, url.ToString());
          this.parentDriver.OnNavigating(e);
          this.wrappedNavigation.GoToUrl(url);
          this.parentDriver.OnNavigated(e);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Refresh the browser
      /// 
      /// </summary>
      public void Refresh()
      {
        try
        {
          this.wrappedNavigation.Refresh();
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }
    }

    /// <summary>
    /// Provides a mechanism for setting options needed for the driver during the test.
    /// 
    /// </summary>
    private class EventFiringOptions : IOptions
    {
      private IOptions wrappedOptions;

      /// <summary>
      /// Gets an object allowing the user to manipulate cookies on the page.
      /// 
      /// </summary>
      public ICookieJar Cookies
      {
        get
        {
          return this.wrappedOptions.Cookies;
        }
      }

      /// <summary>
      /// Gets an object allowing the user to manipulate the currently-focused browser window.
      /// 
      /// </summary>
      /// 
      /// <remarks>
      /// "Currently-focused" is defined as the browser window having the window handle
      ///             returned when IWebDriver.CurrentWindowHandle is called.
      /// </remarks>
      public IWindow Window
      {
        get
        {
          return this.wrappedOptions.Window;
        }
      }

      /// <summary>
      /// Initializes a new instance of the EventFiringOptions class
      /// 
      /// </summary>
      /// <param name="driver">Instance of the driver currently in use</param>
      public EventFiringOptions(EventFiringWebDriver driver)
      {
        this.wrappedOptions = driver.WrappedDriver.Manage();
      }

      /// <summary>
      /// Provides access to the timeouts defined for this driver.
      /// 
      /// </summary>
      /// 
      /// <returns>
      /// An object implementing the <see cref="T:OpenQA.Selenium.ITimeouts"/> interface.
      /// </returns>
      public ITimeouts Timeouts()
      {
        return (ITimeouts) new EventFiringWebDriver.EventFiringTimeouts(this.wrappedOptions);
      }
    }

    /// <summary>
    /// Provides a mechanism for finding elements on the page with locators.
    /// 
    /// </summary>
    private class EventFiringTargetLocator : ITargetLocator
    {
      private ITargetLocator wrappedLocator;
      private EventFiringWebDriver parentDriver;

      /// <summary>
      /// Initializes a new instance of the EventFiringTargetLocator class
      /// 
      /// </summary>
      /// <param name="driver">The driver that is currently in use</param>
      public EventFiringTargetLocator(EventFiringWebDriver driver)
      {
        this.parentDriver = driver;
        this.wrappedLocator = this.parentDriver.WrappedDriver.SwitchTo();
      }

      /// <summary>
      /// Move to a different _frame using its index
      /// 
      /// </summary>
      /// <param name="frameIndex">The index of the </param>
      /// <returns>
      /// A WebDriver instance that is currently in use
      /// </returns>
      public IWebDriver Frame(int frameIndex)
      {
        try
        {
          return this.wrappedLocator.Frame(frameIndex);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Move to different _frame using its name
      /// 
      /// </summary>
      /// <param name="frameName">name of the _frame</param>
      /// <returns>
      /// A WebDriver instance that is currently in use
      /// </returns>
      public IWebDriver Frame(string frameName)
      {
        try
        {
          return this.wrappedLocator.Frame(frameName);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Move to a _frame element.
      /// 
      /// </summary>
      /// <param name="frameElement">a previously found FRAME or IFRAME element.</param>
      /// <returns>
      /// A WebDriver instance that is currently in use.
      /// </returns>
      public IWebDriver Frame(IWebElement frameElement)
      {
        try
        {
          return this.wrappedLocator.Frame((frameElement as IWrapsElement).WrappedElement);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Change to the Window by passing in the name
      /// 
      /// </summary>
      /// <param name="windowName">name of the window that you wish to move to</param>
      /// <returns>
      /// A WebDriver instance that is currently in use
      /// </returns>
      public IWebDriver Window(string windowName)
      {
        try
        {
          return this.wrappedLocator.Window(windowName);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Change the active _frame to the default
      /// 
      /// </summary>
      /// 
      /// <returns>
      /// Element of the default
      /// </returns>
      public IWebDriver DefaultContent()
      {
        try
        {
          return this.wrappedLocator.DefaultContent();
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Finds the active element on the page and returns it
      /// 
      /// </summary>
      /// 
      /// <returns>
      /// Element that is active
      /// </returns>
      public IWebElement ActiveElement()
      {
        try
        {
          return this.wrappedLocator.ActiveElement();
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Switches to the currently active modal dialog for this particular driver instance.
      /// 
      /// </summary>
      /// 
      /// <returns>
      /// A handle to the dialog.
      /// </returns>
      public IAlert Alert()
      {
        try
        {
          return this.wrappedLocator.Alert();
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }
    }

    /// <summary>
    /// Defines the interface through which the user can define timeouts.
    /// 
    /// </summary>
    private class EventFiringTimeouts : ITimeouts
    {
      private ITimeouts wrappedTimeouts;

      /// <summary>
      /// Initializes a new instance of the EventFiringTimeouts class
      /// 
      /// </summary>
      /// <param name="options">The <see cref="T:OpenQA.Selenium.IOptions"/> object to wrap.</param>
      public EventFiringTimeouts(IOptions options)
      {
        this.wrappedTimeouts = options.Timeouts();
      }

      /// <summary>
      /// Specifies the amount of time the driver should wait when searching for an
      ///             element if it is not immediately present.
      /// 
      /// </summary>
      /// <param name="timeToWait">A <see cref="T:System.TimeSpan"/> structure defining the amount of time to wait.</param>
      /// <returns>
      /// A self reference
      /// </returns>
      /// 
      /// <remarks>
      /// When searching for a single element, the driver should poll the page
      ///             until the element has been found, or this timeout expires before throwing
      ///             a <see cref="T:OpenQA.Selenium.NoSuchElementException"/>. When searching for multiple elements,
      ///             the driver should poll the page until at least one element has been found
      ///             or this timeout has expired.
      /// 
      /// <para>
      /// Increasing the implicit wait timeout should be used judiciously as it
      ///             will have an adverse effect on test run time, especially when used with
      ///             slower location strategies like XPath.
      /// 
      /// </para>
      /// 
      /// </remarks>
      public ITimeouts ImplicitlyWait(TimeSpan timeToWait)
      {
        return this.wrappedTimeouts.ImplicitlyWait(timeToWait);
      }

      /// <summary>
      /// Specifies the amount of time the driver should wait when executing JavaScript asynchronously.
      /// 
      /// </summary>
      /// <param name="timeToWait">A <see cref="T:System.TimeSpan"/> structure defining the amount of time to wait.</param>
      /// <returns>
      /// A self reference
      /// </returns>
      public ITimeouts SetScriptTimeout(TimeSpan timeToWait)
      {
        return this.wrappedTimeouts.SetScriptTimeout(timeToWait);
      }

      /// <summary>
      /// Specifies the amount of time the driver should wait for a page to load when setting the <see cref="P:OpenQA.Selenium.IWebDriver.Url"/> property.
      /// 
      /// </summary>
      /// <param name="timeToWait">A <see cref="T:System.TimeSpan"/> structure defining the amount of time to wait.</param>
      /// <returns>
      /// A self reference
      /// </returns>
      public ITimeouts SetPageLoadTimeout(TimeSpan timeToWait)
      {
        this.wrappedTimeouts.SetPageLoadTimeout(timeToWait);
        return (ITimeouts) this;
      }
    }

    /// <summary>
    /// EventFiringWebElement allows you to have access to specific items that are found on the page
    /// 
    /// </summary>
    private class EventFiringWebElement : IWebElement, ISearchContext, IWrapsElement
    {
      private IWebElement underlyingElement;
      private EventFiringWebDriver parentDriver;

      /// <summary>
      /// Gets the underlying wrapped <see cref="T:OpenQA.Selenium.IWebElement"/>.
      /// 
      /// </summary>
      public IWebElement WrappedElement
      {
        get
        {
          return this.underlyingElement;
        }
      }

      /// <summary>
      /// Gets the DOM Tag of element
      /// 
      /// </summary>
      public string TagName
      {
        get
        {
          string str = string.Empty;
          try
          {
            return this.underlyingElement.TagName;
          }
          catch (Exception ex)
          {
            this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
            throw;
          }
        }
      }

      /// <summary>
      /// Gets the text from the element
      /// 
      /// </summary>
      public string Text
      {
        get
        {
          string str = string.Empty;
          try
          {
            return this.underlyingElement.Text;
          }
          catch (Exception ex)
          {
            this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
            throw;
          }
        }
      }

      /// <summary>
      /// Gets a value indicating whether an element is currently enabled
      /// 
      /// </summary>
      public bool Enabled
      {
        get
        {
          try
          {
            return this.underlyingElement.Enabled;
          }
          catch (Exception ex)
          {
            this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
            throw;
          }
        }
      }

      /// <summary>
      /// Gets a value indicating whether this element is selected or not. This operation only applies to input elements such as checkboxes, options in a select and radio buttons.
      /// 
      /// </summary>
      public bool Selected
      {
        get
        {
          try
          {
            return this.underlyingElement.Selected;
          }
          catch (Exception ex)
          {
            this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
            throw;
          }
        }
      }

      /// <summary>
      /// Gets the Location of an element and returns a Point object
      /// 
      /// </summary>
      public Point Location
      {
        get
        {
          Point point = new Point();
          try
          {
            return this.underlyingElement.Location;
          }
          catch (Exception ex)
          {
            this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
            throw;
          }
        }
      }

      /// <summary>
      /// Gets the <see cref="P:OpenQA.Selenium.Support.Events.EventFiringWebDriver.EventFiringWebElement.Size"/> of the element on the page
      /// 
      /// </summary>
      public Size Size
      {
        get
        {
          Size size = new Size();
          try
          {
            return this.underlyingElement.Size;
          }
          catch (Exception ex)
          {
            this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
            throw;
          }
        }
      }

      /// <summary>
      /// Gets a value indicating whether the element is currently being displayed
      /// 
      /// </summary>
      public bool Displayed
      {
        get
        {
          try
          {
            return this.underlyingElement.Displayed;
          }
          catch (Exception ex)
          {
            this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
            throw;
          }
        }
      }

      /// <summary>
      /// Gets the underlying EventFiringWebDriver for this element.
      /// 
      /// </summary>
      protected EventFiringWebDriver ParentDriver
      {
        get
        {
          return this.parentDriver;
        }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.Support.Events.EventFiringWebDriver.EventFiringWebElement"/> class.
      /// 
      /// </summary>
      /// <param name="driver">The <see cref="T:OpenQA.Selenium.Support.Events.EventFiringWebDriver"/> instance hosting this element.</param><param name="element">The <see cref="T:OpenQA.Selenium.IWebElement"/> to wrap for event firing.</param>
      public EventFiringWebElement(EventFiringWebDriver driver, IWebElement element)
      {
        this.underlyingElement = element;
        this.parentDriver = driver;
      }

      /// <summary>
      /// Method to clear the text out of an Input element
      /// 
      /// </summary>
      public void Clear()
      {
        try
        {
          WebElementEventArgs e = new WebElementEventArgs(this.parentDriver.WrappedDriver, this.underlyingElement);
          this.parentDriver.OnElementValueChanging(e);
          this.underlyingElement.Clear();
          this.parentDriver.OnElementValueChanged(e);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Method for sending native key strokes to the browser
      /// 
      /// </summary>
      /// <param name="text">String containing what you would like to type onto the screen</param>
      public void SendKeys(string text)
      {
        try
        {
          WebElementEventArgs e = new WebElementEventArgs(this.parentDriver.WrappedDriver, this.underlyingElement);
          this.parentDriver.OnElementValueChanging(e);
          this.underlyingElement.SendKeys(text);
          this.parentDriver.OnElementValueChanged(e);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// If this current element is a form, or an element within a form, then this will be submitted to the remote server.
      ///             If this causes the current page to change, then this method will block until the new page is loaded.
      /// 
      /// </summary>
      public void Submit()
      {
        try
        {
          this.underlyingElement.Submit();
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Click this element. If this causes a new page to load, this method will block until
      ///             the page has loaded. At this point, you should discard all references to this element
      ///             and any further operations performed on this element will have undefined behavior unless
      ///             you know that the element and the page will still be present. If this element is not
      ///             clickable, then this operation is a no-op since it's pretty common for someone to
      ///             accidentally miss  the target when clicking in Real Life
      /// 
      /// </summary>
      public void Click()
      {
        try
        {
          WebElementEventArgs e = new WebElementEventArgs(this.parentDriver.WrappedDriver, this.underlyingElement);
          this.parentDriver.OnElementClicking(e);
          this.underlyingElement.Click();
          this.parentDriver.OnElementClicked(e);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// If this current element is a form, or an element within a form, then this will be submitted to the remote server. If this causes the current page to change, then this method will block until the new page is loaded.
      /// 
      /// </summary>
      /// <param name="attributeName">Attribute you wish to get details of</param>
      /// <returns>
      /// The attribute's current value or null if the value is not set.
      /// </returns>
      public string GetAttribute(string attributeName)
      {
        string str = string.Empty;
        try
        {
          return this.underlyingElement.GetAttribute(attributeName);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Method to return the value of a CSS Property
      /// 
      /// </summary>
      /// <param name="propertyName">CSS property key</param>
      /// <returns>
      /// string value of the CSS property
      /// </returns>
      public string GetCssValue(string propertyName)
      {
        string str = string.Empty;
        try
        {
          return this.underlyingElement.GetCssValue(propertyName);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Finds the first element in the page that matches the <see cref="T:OpenQA.Selenium.By"/> object
      /// 
      /// </summary>
      /// <param name="by">By mechanism to find the element</param>
      /// <returns>
      /// IWebElement object so that you can interaction that object
      /// </returns>
      public IWebElement FindElement(By by)
      {
        try
        {
          FindElementEventArgs e = new FindElementEventArgs(this.parentDriver.WrappedDriver, this.underlyingElement, by);
          this.parentDriver.OnFindingElement(e);
          IWebElement element = this.underlyingElement.FindElement(by);
          this.parentDriver.OnFindElementCompleted(e);
          FoundElementEventArgs f = new FoundElementEventArgs(this.parentDriver.WrappedDriver, element, by);
          this.parentDriver.OnFoundElement(f);
          return this.parentDriver.WrapElement(element);
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
      }

      /// <summary>
      /// Finds the elements on the page by using the <see cref="T:OpenQA.Selenium.By"/> object and returns a ReadOnlyCollection of the Elements on the page
      /// 
      /// </summary>
      /// <param name="by">By mechanism to find the element</param>
      /// <returns>
      /// ReadOnlyCollection of IWebElement
      /// </returns>
      public ReadOnlyCollection<IWebElement> FindElements(By by)
      {
        List<IWebElement> list = new List<IWebElement>();
        try
        {
          FindElementEventArgs e = new FindElementEventArgs(this.parentDriver.WrappedDriver, this.underlyingElement, by);
          this.parentDriver.OnFindingElement(e);
          ReadOnlyCollection<IWebElement> elements = this.underlyingElement.FindElements(by);
          this.parentDriver.OnFindElementCompleted(e);
          foreach (IWebElement underlyingElement in elements)
          {
            IWebElement webElement = this.parentDriver.WrapElement(underlyingElement);
            list.Add(webElement);
            FoundElementEventArgs f = new FoundElementEventArgs(this.parentDriver.WrappedDriver, webElement, by);
            this.parentDriver.OnFoundElement(f);
          }
        }
        catch (Exception ex)
        {
          this.parentDriver.OnException(new WebDriverExceptionEventArgs((IWebDriver) this.parentDriver, ex));
          throw;
        }
        return list.AsReadOnly();
      }
    }
  }
}
