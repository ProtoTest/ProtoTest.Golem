using System.Collections.Generic;
using NUnit.Framework;
using Golem.Core;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
{
    internal class TestDDT : WebDriverTestBase
    {
//        [XmlData("//Search", FilePath = ".\\Tests\\Data\\SearchData.xml")]
//        [Test]
//        public void TestXml([Bind("@term")] string search, [Bind("@result")] string result)
//        {
//            Config.settings.runTimeSettings.FindHiddenElements = true;
//            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(search).VerifyResult(result);
//        }
//
//        [CsvData(FilePath = ".\\Tests\\Data\\Data.csv", HasHeader = true)]
//        [Test]
//        public void TestCSV(string term, string result)
//        {
//            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(term).VerifyResult(result);
//        }

        [Test]
        public void TestValues(
        [Values("Selenium")] string term,
        [Values("Selenium - Web Browser Automation", "Selenium (software) - Wikipedia, the free encyclopedia")] string result)
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(term).VerifyResult(result);
        }


//        public IEnumerable<Search> GetDataEnumerable()
//        {
//            yield return new Search {term = "Selenium", result = "Selenium - Web Browser Automation"};
//            yield return new Search {term = "ProtoTest", result = "ProtoTest"};
//        }
//
        public static List<Search> GetDataList()
        {
            var searches = new List<Search>();
            searches.Add(new Search {term = "WebDriver", result = "Selenium WebDriver"});
            searches.Add(new Search {term = "Appium", result = "Appium: Mobile App Automation Made Awesome."});
            return searches;
        }

        [Test, TestCaseSource("GetDataList")]
        public void TestSource(Search search)
        {
            OpenPage<GoogleHomePage>("http://www.google.com/").SearchFor(search.term).VerifyResult(search.result);
        }

        public class Search
        {
            public string result;
            public string term;
        }
    }
}