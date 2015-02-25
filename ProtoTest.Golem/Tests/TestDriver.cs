using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using Golem.TestProject.PageObjects;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    class TestDriver : WebDriverTestBase 
    {
        [Test]
        public void TestWebElementExtensions()
        {
          
            driver.Navigate().GoToUrl("http://google.com");
            driver.WaitForPresent(By.Name("q"),20).Highlight();
            driver.WaitForNotVisible(By.Name("zas"));
        }

        [Test]
        public void TestWebDriverExtensions()
        {
            driver.Navigate().GoToUrl("http://google.com");
            driver.WaitForElementWithText("Google Search").Highlight();
            driver.FindElementWithText("Google Search").Highlight();
        }

        [Test]
        public void test()
        {
            OpenPage<LoginPanel>("http://www.radiofreeamerica.com");
        }

    }
}
