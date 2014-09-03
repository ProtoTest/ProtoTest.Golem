using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Appium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using MbUnit.Framework;

namespace ProtoTest.Golem.Tests
{
    class BrowserStackTest : WebDriverTestBase
    {
        [FixtureInitializer]
        public void Setup()
        {
            /* 
               Add the following to App.config to test this stuff
                <add key="RunOnRemoteHost" value="True" />
                <add key="LaunchBrowser" value="True" />
                <add key="BrowserStack_User" value="<your user>" />
                <add key="BrowserStack_Key" value="<your key>" />
                <add key="BrowserStack_OS" value="OS X" />
                <add key="BrowserStack_OS_Version" value="Mountain Lion" />
                <add key="HostIp" value="hub.browserstack.com" />
             */
        }
        [Test]
        public void Test()
        {
            driver.Navigate().GoToUrl("http://www.espn.com");
            var element = new Element("ESPN Element", By.XPath("//*[@class='espn-logo']//*[text()='ESPN']"));
            element.WaitUntil(10).Visible();
            Common.Log("Successfully navigated to " + driver.Title);
        }
    }
}
