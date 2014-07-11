using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;

namespace ProtoTest.Golem.Tests
{
    class TestPageObjects : TestWebDriverTestBase
    {
        [Test]
        public void Test()
        {
        
            OpenPage<GoogleHomePage>("http://www.google.com/").
                SearchFor("Selenium").
                VerifyResult("Web Browser Automation").
                SearchFor("ProtoTest").
                VerifyResult("ProtoTest « Beyond Bugs");
        }
    }
}
