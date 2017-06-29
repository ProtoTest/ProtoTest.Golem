using NUnit.Framework;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
{
    [TestFixture]
    internal class TestPageObjects : WebDriverTestBase
    {
        [Test]
        public void TestPageObject1()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").
                SearchFor("Something").
                VerifyResult("Something");
        }
        [Test]
        public void TestPageObject2()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").
                SearchFor("ProtoTest").
                VerifyResult("ProtoTest");
        }
        [Test]
        public void TestPageObject3()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").
                SearchFor("Selenium").
                VerifyResult("Web Browser Automation");
        }
    }
}