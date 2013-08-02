Golem
=====

Event driven WebDriver C# Framework

Golem is an event driven framework for C#.  It was originally built for webdriver, but has become more tool agnostic.  It is built upon MbUnit and Gallio.  

Overview:
- Golem Tests follow the unit testing pattern.  You will have a .cs file that contains a class (suite of tests).  
- The class will contain one or more methods marked with the [Test] attribute.  
- Each Test becomes executable separately in Gallio Icarus.  
- By default, the framework will automatically launch Firefox on your local browser before each test, and instantiates a webdriver instance.  
- All settings, such as which browsers to use, can be modified via the Config class, or an App.Config file. 
- Golem includes a variety of "features" that get started automatically.  For example, having each webdriver command written to the test log.  
- Golem is event-driven, and has a variety of events that get triggered automatically.  In your test class you can define methods that are called when these events are fired.  

Things to know : 
- C# and .Net 4.0
- Built upon Gallio and MbUnit. (gallio.org)  Build the project into a dll, open dll in Gallio Icarus to execute.  
- Multi-threaded support (Parallel test execution)
- Selenium GRID/SauceLabs supported.
- Extended WebDriver API to add additional commands.
- Using EventDrivenWebDriver to log commands.
- Fully configurable programatically or with an App.Config.  Config class reads the App.config and stores all values as properties.  Each value should have a default defined.
- Built on the Page Object Design Pattern.  Page Objects should inherit BasePageObject
- Test class should inherit TestBaseClass, and each test function should be marked with the [Test] attribute.
- Optional Element class wraps and hides webdriver API
- WebDriver driver is instantiated automatically for each test, and stored in a static IDictionary in the TestBaseClass. This is for multi-threaded support. Each thread automatically knows which driver belongs to it.  Access it in any test or page object like "driver.FindElement".  
- Page Objects need to define a WaitForElements() function.  This is called automaticalled when the PO is instantiated. You should put waits or validations in here to wait or validate that a page object is fully loaded and ready to be used.  
- Page Objects don't need a constructor.  The base constructor instantiates everything, and the driver object is stored statically in the TestBaseClass
- You can use MbUnit.Framework.Assert to make assertions.  These will stop the test on failure.
- Created a variety of Verifications in the WebDriver API.  These will not stop the test if they fail.  '
- Supports data driven testing through MbUnit attributes.  

Configurable Features : 
- Multi Threaded Execution.  Mark test with [Parallelizable] attribute. Set number of threads with "DegreeOfParallelism", "1"
- Automatically Launch Browser - LaunchBrowser", "True"
- Specify up to five browsers - "Browser1" value="Firefox"
- Add a delay between commands - "CommandDelayMs" value="0"
- Run on local or remote computer - "RunOnRemoteHost" value="false", "HostIp" value="localhost"
- Capture screenshot on error - "ScreenshotOnError" value="True"
- Capture page html source on error - "HtmlOnError" value="True" 
- Capture screen video recording on error - "VideoRecordingOnError", "True"
- Write all webdriver commands to the log - "CommandLogging" value="True"
- Write all page object functions to the log - "ActionLogging" value="True"
- Launch a proxy to capture http traffic - "StartFiddlerProxy" value="True", "ProxyPort" value="8876"
- Appium support - "LaunchApp", "False" - "AppPath", "" - "AppPackage", "" - "AppActivity", "" - "AppOs", "Android"
- Configurable Test timeout - "TestTimeoutMin","5"
- Configurable element Timeout - "ElementTimeoutSec","20"
- Configurable environment Url - "EnvironmentUrl",""
- Automatically check spelling on each page - 




