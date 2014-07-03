using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
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
