using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    class TestDDT : WebDriverTestBase
    {
        [XmlData("//Search",FilePath = ".\\Tests\\Data\\SearchData.xml")]
        [Test]
        public void TestXml([Bind("@term")]string search, [Bind("@result")]string result)
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(search).VerifyResult(result);
        }
        [CsvData(FilePath = ".\\Tests\\Data\\Data.csv", HasHeader = true)]
        [Test]
        public void TestCSV(string term, string result)
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(term).VerifyResult(result);
        }

        [Row("Selenium","Selenium - Web Browser Automation")]
        [Row("ProtoTest", "ProtoTest « Beyond Bugs")]
        [Test]
        public void TestRowData(string term, string result)
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(term).VerifyResult(result);
        }

        public class Search
        {
            public string term;
            public string result;
        }

        public IEnumerable<Search> GetDataEnumerable()
        {
            yield return new Search() {term = "Selenium", result = "Selenium - Web Browser Automation"};
            yield return new Search() { term = "ProtoTest", result = "ProtoTest « Beyond Bugs" };
        }

        public List<Search> GetDataList()
        {
            List<Search> searches = new List<Search>();
            searches.Add(new Search() { term = "WebDriver", result = "Selenium WebDriver" });
            searches.Add(new Search() { term = "Appium", result = "Appium: Mobile App Automation Made Awesome." });
            return searches;
        }

        [Test, Factory("GetDataEnumerable"),Factory("GetDataList")]
        public void TestFactory(Search search)
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(search.term).VerifyResult(search.result);
        }

    }
}
