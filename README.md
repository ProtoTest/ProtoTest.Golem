Golem
=====

###Event driven WebDriver C# Framework

Golem is an event-driven test framework written in C# for Selenium-WebDriver and Gallio/MbUnit.  It builds on gallio's excellent reporting and web driver's automation library, adding a framework for more flexible testing using the Page Object pattern.  Golem encapsulates the code and best practices most companies need for wrangling enterprise-scale test suites.

Golem test suites are structured as typical unit tests.  Each test is marked with the MbUnit unit [Test] attribute--each Test becomes executable separately by Gallio Icarus or testdriven.net.  By default, the framework will automatically launch Firefox on your local browser before each test, and instantiates a webdriver instance.  All settings, such as which browsers to use, can be modified via the Config class, or an App.Config file. 

Golem includes a variety of "features" that get started automatically.  For example, having each webdriver command written to the test log.  Further, Golem is event-driven, and has a variety of events that get triggered automatically.  In your test class you can define methods that are called when these events are fired.

Golem has three goals: 
-	Simplify the design and coding costs of automation test suites by providing an enhanced Webdriver API and an intelligently designed library
-	Define and enforce a standard test structure which facilitates code readability and re-use
-	Provide features that enhance webdriver functionality, such as diagnostic information (screenshots, HTTP requests).

####Included Features:
-	Standard app.config file can be used to turn on/off or configure all features.  
-	Page Object design pattern makes tests a BDD style function chain : ```GoToWebPage().LogInWithUser(“GSmith”,”Password”).GoToProfilePage().EditName(“George Smith”).LogOut();```
-	Parallel test execution – Can execute up to 5 instances of a browser on each machine
-	Can run tests locally or on a remote machine
-	Multiple Browser support – Define multiple browsers and each test will run once per browser.
-	Logging of all WebDriver commands 
-	Screenshots on test failure
-	Video Recording on test failure
-	HTTP Traffic recording, metrics, and validations
-	Automatic spell check each page 
-	Log/Track all UI actions (like logging in) including AJAX waiting
-	Appium Support (mobile app automation)
-	Automatically validate pages loaded by defining a list of elements to check for  
-	Event-driven architecture makes customizations easy

###Getting Started With ProtoTest.Golem: 
####Requirements: 
- Visual Studio 2012 for C#
- NuGet 3.7 or later (Comes with VS 2012)
- TestDriven.net Visual Studio plugin 
- GallioBundle 3.4 (https://code.google.com/p/mb-unit/downloads/list)

#### Creating a Test
- Create a new Class Library File -> New -> Project -> Class Library.
- Right click on references -> Manage NuGet Packages
- Add Package “ProtoTest.Golem”. 
- WebDriver is available via "driver" property.
- Paste the following code into your class file, save the project/solution, and build:

```
using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    class Test : WebDriverTestBase
    {
        [Test]
        public void TestGoogle()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor("Selenium").VerifyResult("Selenium – Web Browser Automation");
        }
	}
}
```

#### Executing a Test: 
##### From Within Visual Studio
- TestDriven.net plugin is required
- Right Click on Test - > Run Test (Should open browser and close it)
- Right Click on Class or project to run all tests within.

##### From Gallio Icarus 
- Open Gallio Icarus
- Click "Add" and navigate to our ProjectName.dll file located in the /bin/release or /bin/debug/ directory.
- Select tests to execute.
- Click Run.


#### Configuring Tests

##### Through an App.Config :
- Add an "Application Configuration File".  
- Edit the App.config -> Set browser using key “Browser1” value=”Firefox”

```
<?xml version="1.0"?>
<configuration>
  <appSettings>
    <add key="Browser1" value="Firefox"/>
   </appSettings>
</configuration>
```
- Execute Test - > Should open Firefox instead of Chrome.  
- Supports Firefox, Chrome, IE, Safari, HtmlUnit, and Android (remote only)

##### Through code : 
```
[FixtureInitializer]
        public void init()
        {
            Config.Settings.httpProxy.startProxy = false;
            Config.Settings.httpProxy.useProxy = false;

        }

```
####Building a new Page Object:
- Look in the Tests/PageObjects directory in the Golem repository for examples
- Page Objects Inherit BasePage
- Instances of Element represent elements in the web page under test
- The Element API provides convenient methods for locating elements
- WebDriver API available through 'driver' property;
- WaitForElements method is called when page object is instantiated.  Use it to wait for dynamic elements.

```
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests.PageObjects.Google
{
    public class GoogleHomePage : BasePageObject
    {
        Element searchField = new Element("SearchField", By.Name("q"));
        Element googleLogo = new Element("GoogleLogo", By.Id("hplogo"));
        Element searchButton = new Element("SearchButton", By.Name("btnK"));
        Element feelingLuckyButton = new Element("ImFeelingLuckyButton", By.Name("btnI"));
        Element signInButton = new Element("SignInButon", By.LinkText("Sign in"));

        public GoogleResultsPage SearchFor(string text)
        {
            searchField.Text = text;
            searchField.Submit();
            return new GoogleResultsPage();
        }

        public override void WaitForElements()
        {
            searchField.Verify().Present();
            googleLogo.Verify().Present();
            searchButton.Verify().Present();
            feelingLuckyButton.Verify().Present();
            signInButton.Verify().Present();
        }
    }
}

```

####Executing on a Remote Computer
- Modify App Config, set host to IP or Hostname of remote computer 

```
<?xml version="1.0"?>
<configuration>
  <appSettings>
    <add key="Browser1" value="Chrome"/>
    <add key="RunOnRemoteHost" value="True"/>
    <add key="HostIp" value="10.10.1.151"/>
   </appSettings>
</configuration>
```

####Configurable Features : 
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

###Things to know : 
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


