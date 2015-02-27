using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;
//using Castle.Core.Logging;

namespace ProtoTest.Golem.Tests
{
    internal class TestWebDriverTestBase : WebDriverTestBase
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