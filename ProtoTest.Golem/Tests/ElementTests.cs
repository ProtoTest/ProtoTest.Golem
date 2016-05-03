using NUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    internal class ElementTests : WebDriverTestBase
    {
        [Test]
        public void TestNoName()
        {
            var element = new Element(By.Id("Id"));
            Assert.AreEqual(element.name, "Element");
        }

        [Test]
        public void TestWithName()
        {
            var element = new Element("NameOfElement", By.Id("Id"));
            Assert.AreEqual(element.name, "NameOfElement");
        }

        [Test]
        public void TestDSL()
        {
            driver.Navigate().GoToUrl("http://www.google.com/");
            var element = new Element("NameOfElement", By.Name("q"));
            element.SetText("ProtoTest");
            element.Verify().Value("ProtoTest").Submit();
            element.WaitUntil(20).Visible().Text = "Golem";
            element.Verify(20).Value("Golem");
        }

        [Test]
        public void TestNotPresent()
        {
            driver.Navigate().GoToUrl("http://www.google.com/");
            var SearchField = new Element("SearchField", By.Name("q"));
            var element = new Element("NotPresentElement", By.Name("z"));
            SearchField.WaitUntil().Present();
            element.Verify().Not().Present();
        }
    }
}