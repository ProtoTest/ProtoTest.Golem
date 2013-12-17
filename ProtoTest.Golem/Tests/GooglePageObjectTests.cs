using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    [TestFixture]
    public class GooglePageObjectTests : WebDriverTestBase
    {
        [Test]
        public static void TestImages()
        {
           GoogleHomePage.OpenGoogle().VerifyImages();
            
        }

        [Test]
        public void TestSearch()
        {
            var searchText = "Selenium";
            var searchResult = "Selenium - Web Browser Automation";

            GoogleHomePage.
                OpenGoogle().
                SearchFor(searchText).
                VerifyResult(searchResult).
                GoToResult(searchResult);
        }

        [Test]
        [Row("Selenium", "Selenium - Web Browser Automation")]
        [Row("ProtoTest", "ProtoTest - IT Staffing and Mobile App Testing Lab")]
        [Row("Soasta", "SOASTA - Wikipedia, the free encyclopedia")]
        public void TestSearch_WithRows(string searchText, string searchResult)
        {
            GoogleHomePage.
                OpenGoogle().
                SearchFor(searchText).
                VerifyResult(searchResult).
                GoToResult(searchResult);
        }
        
    }
}
