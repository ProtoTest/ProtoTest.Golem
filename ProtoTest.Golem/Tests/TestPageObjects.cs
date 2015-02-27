using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;

namespace ProtoTest.Golem.Tests
{
    internal class TestPageObjects : TestWebDriverTestBase
    {
        [Test]
        public void Test()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").
                SearchFor("Selenium").
                VerifyResult("Web Browser Automation").
                SearchFor("ProtoTest").
                VerifyResult("ProtoTest: Home");
        }
    }
}