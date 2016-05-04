using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

[TestFixture(typeof(ChromeDriver))]
[TestFixture(typeof(InternetExplorerDriver))]
public class BrowserTests<TWebDriver> where TWebDriver : IWebDriver, new()
{
    private IWebDriver _webDriver;

    [SetUp]
    public void SetUp()
    {
        string driversPath = Environment.CurrentDirectory + @"\..\..\..\WebDrivers\";

        _webDriver = Activator.CreateInstance(typeof(TWebDriver), new object[] { driversPath }) as IWebDriver;
    }

    [TearDown]
    public void TearDown()
    {
        _webDriver.Dispose(); // Actively dispose it, doesn't seem to do so itself
    }

    [Test]
    public void Tests()
    {
        //TestCode
    }
}