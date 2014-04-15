using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using OpenQA.Selenium.Firefox;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    class TestWebDriverTestBase : WebDriverTestBase
    {
        [Test]
        public void TestPageObjectCreater()
        {
            var page = OpenPage<GoogleHomePage>("http://www.google.com");
            Assert.IsInstanceOfType<GoogleHomePage>(page);
        }
        [Test]
        public void TestDriverNotNull()
        {
            Assert.IsNotNull(driver);
        }
        [Test]
        public void TestDefaultBrowser()
        {
            Assert.AreEqual(browser, WebDriverBrowser.Browser.Chrome);
        }
        [Test]
        public void TestEventFiringDriverLaunches()
        {
            Assert.IsInstanceOfType<EventFiringWebDriver>(driver);
        }

    }
}
