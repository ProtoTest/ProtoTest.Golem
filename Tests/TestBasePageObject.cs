using NUnit.Framework;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
{
    public class TestBasePageObject : WebDriverTestBase
    {
        [Test]
        public void TestWaitForElements()
        {
            var page = OpenPage<GoogleHomePage>("http://www.google.com");
        }

        [Test]
        public void TestPageObjectHasDriver()
        {
            OpenPage<GoogleHomePage>("http://www.google.com");
            var page = new GoogleHomePage();
            Assert.IsNotNull(page.driver);
        }
    }
}