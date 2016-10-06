using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using Golem.WebDriver;

namespace Golem.Tests
{
    internal class TestElements : WebDriverTestBase
    {
        [Test]
        public void TestElementAPI()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            var elements = new Element(driver.FindElements(By.Name("q")));
            elements.First().Click();
        }

        [Test]
        public void TestLinq()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            var elements = new Element(driver.FindElements(By.XPath("//*")));
            elements.First(x => x.GetAttribute("name") == "q" && x.Displayed).Click();
        }

        [Test]
        public void testCount()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            var elements = new Element(driver.FindElements(By.Name("q")));
            driver.Navigate().GoToUrl("http://www.google.com");
            Assert.AreEqual(1, elements.Where(x => x.IsStale()).Count());
        }

        [Test]
        public void TestBy()
        {
            var elements = new Element(By.Name("q"));
            driver.Navigate().GoToUrl("http://www.google.com");
            elements.ForEach(x => x.Click());
            driver.Navigate().GoToUrl("http://www.google.com");
            elements.ForEach(x => x.SendKeys("test"));
        }
    }
}