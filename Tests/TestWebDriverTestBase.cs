using NUnit.Framework;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;
//using Castle.Core.Logging;

namespace Golem.Tests
{
    internal class TestWebDriverTestBase : WebDriverTestBase
    {
        [Test]
        public void TestPageObjectCreater()
        {
            var page = OpenPage<GoogleHomePage>("http://www.google.com");
            Assert.IsInstanceOf<GoogleHomePage>(page);
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
            Assert.IsInstanceOf<EventFiringWebDriver>(driver);
        }
    }
}