using NUnit.Framework;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
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