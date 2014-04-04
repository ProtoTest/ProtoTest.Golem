using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Safari;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.Tests.PageObjects.Google;

namespace ProtoTest.Golem.Tests
{
    class ElementTests : WebDriverTestBase
    {

        [Test]
        public void TestNoName()
        {
            var element = new Element(By.Id("Id"));
            Assert.AreEqual(element.name, "By.Id: Id");
        }
        [Test]
        public void TestWithName()
        {
            var element = new Element("NameOfElement",By.Id("Id"));
            Assert.AreEqual(element.name, "NameOfElement");
        }
        [Test]
        public void TestDSL()
        {
            driver.Navigate().GoToUrl("http://www.google.com/");
            var element = new Element("NameOfElement", By.Name("q"));
            element.WaitUntil(20).Visible().SetText("ProtoTest");
            element.Verify().Value("ProtoTest").Click();

            element.WaitUntil(20).Visible().Text = "Golem";
            element.Verify().Value("Golem");
        }

    }
}
