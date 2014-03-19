Golem Overview
====================

##Object Oriented C# Automated Testing Framework

Golem was created to simplify the process of creating enterprise-scale automated testing suites. It wraps around Gallio/MbUnit and a number of automation tools (such as Selenium-WebDriver) to provide the tester with a simple process for creating automated tests no matter what the tool. The inclusion of advanced features, diagnostic information, easy configuration, and enhanced API's helps Golem make automating in code-based automation tools much more practical. A standard test structure is enforced throughout the Golem framework to make the code readable and easy to reuse. 

The Golem repository contains several modules that can be used to test desktop or mobile browsers (Golem.WebDriver), Android and iOS applications (Golem.Appium), Windows applications (Golem.White), or HTTP requests and web services (Golem.Rest). 

requests (Golem.Rest). 

For all documentation, visit the [Golem Wiki](https://github.com/ProtoTest/ProtoTest.Golem/wiki).

##Example
Tests are written using a human readable DSL via Page Objects.  
```C#
    class TestExample : WebDriverTestBase
    {
        [Test]
        public void TestNormal()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor("Selenium").VerifyResult("Selenium - Web Browser Automation");
        }
```
Page Objects are simple, and stable.  Configurable automatic waiting and page validations.  
```C#
    public class GoogleHomePage : BasePageObject
    {
        Element searchField = new Element("SearchField", By.Name("q"));
        Element googleLogo = new Element("GoogleLogo", By.Id("hplogo"));
        Element searchButton = new Element("SearchButton", By.Name("btnK"));
        Element feelingLuckyButton = new Element("ImFeelingLuckyButton", By.Name("btnI"));

        public GoogleResultsPage SearchFor(string text)
        {
            searchField.Text = text;
            searchField.Submit();
            return new GoogleResultsPage();
        }

        public override void WaitForElements()
        {
            searchField.WaitUntil().Present();
            googleLogo.WaitUntil().Present();
            searchButton.WaitUntil().Present();
            feelingLuckyButton.WaitUntil().Present();
        }
    }
```

Test reports include robust diagnostic information.  A command log, source html, screenshots/video, and HTTP traffic configurable through code or an App.config.  

![ScreenShot](https://raw.github.com/ProtoTest/ProtoTest.Golem/master/ProtoTest.Golem/Tests/SampleReport/Report.jpg)

##How to Get Started

1) Install the ProtoTest.Golem Nuget package into a C# project. This sets up Selenium and WebDriver along with the Golem framework.		

2) Take a look at the [Golem.Core documention](https://github.com/ProtoTest/ProtoTest.Golem/wiki/Golem.Core,-Setup) for an overview of the framework.

3) Golem includes modules for testing web sites, REST api's and desktop applications. Take a look at the respective documentation to get started with each.
* For web sites, use [Golem.WebDriver](https://github.com/ProtoTest/ProtoTest.Golem/wiki/Golem.WebDriver,-Getting-Started). 
* For RESTful services, use [Golem.Rest](https://github.com/ProtoTest/ProtoTest.Golem/wiki/Golem.Rest,-Getting-Started). 
* For desktop applications, use [Golem.White](https://github.com/ProtoTest/ProtoTest.Golem/wiki/Golem.White,-Getting-Started).



