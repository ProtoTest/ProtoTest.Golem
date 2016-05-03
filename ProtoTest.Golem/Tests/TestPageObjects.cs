using NUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    internal class TestPageObjects : WebDriverTestBase
    {
        [Test]
        public void TestPageObject()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").
                SearchFor("Selenium").
                VerifyResult("Web Browser Automation").
                SearchFor("ProtoTest").
                VerifyResult("ProtoTest");
        }
    }
}