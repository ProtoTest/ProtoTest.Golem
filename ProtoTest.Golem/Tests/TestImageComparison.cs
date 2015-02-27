using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    [TestFixture]
    public class TestImageComparison : WebDriverTestBase
    {
        [Test]
        public static void TestImages()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").VerifyImages();
        }
    }
}