using System.Collections.Generic;
using System.IO;
using System.Linq;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Proxy.HAR;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Proxy
{
    class ProxyTests : WebDriverTestBase
    {
        
        public class TestObject
        {
            string filePath = Config.GetConfigValue("DataCsvPath", Common.GetCodeDirectory() + @"\Proxy\Data.csv");
            public int rowNumber;
            public string url;
            public List<QueryStringItem> queryStrings;
            public string ElementText;
            public string line;

            public TestObject(int rowNum)
            {
                this.line = GetLineByRow(rowNum);
                var fields = LineSplitter(line).ToArray();
                this.url = fields[0];
                this.ElementText = fields[1];
                for (var i = 2; i < fields.Length; i++)
                {
                    var qs = fields[i].Split('=');
                    if (qs[0] != "")
                    {
                        queryStrings.Add(new QueryStringItem() { Name = qs[0], Value = qs[1] });
                    }

                }
            }

            public string GetLineByRow(int rowNum)
            {
                if (!File.Exists(filePath))
                {
                    Assert.Fail("Could not run tests, " + filePath + " could not be found");
                }
                StreamReader file = new StreamReader(filePath);
                string line = "";
                for (var i = 1; i < rowNum; i++)
                {
                    line = file.ReadLine();
                }
                return line;
            }

            IEnumerable<string> LineSplitter(string line)
            {
                int fieldStart = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == ',')
                    {
                        string field = line.Substring(fieldStart, i - fieldStart).Replace("\"", "");
                        yield return field;
                        fieldStart = i + 1;
                    }
                    if (line[i] == '"')
                        for (i++; line[i] != '"'; i++)
                        {
                        }
                }
            }
        }
        string filePath = Config.GetConfigValue("DataCsvPath", Common.GetCodeDirectory() + @"\Proxy\Data.csv");
        public IEnumerable<int> GetRowNumber()
        {
            if (!File.Exists(filePath))
            {
                Assert.Fail("Could not run tests, " + filePath + " could not be found");
            }
            StreamReader file = new StreamReader(filePath);
            int rowNumber = 0;
            string line = file.ReadLine();
            while (line != null)
            {
                rowNumber++;
                yield return rowNumber;               
            }
            
        }

       

        //    if (!File.Exists(filePath))
        //    {
        //        Assert.Fail("Could not run tests, " + filePath + " could not be found");
        //    }
        //    StreamReader file = new StreamReader(filePath);
        //    string line = file.ReadLine();
        //    int numLines = 0;
        //    while (line != null)
        //    {
        //        numLines++;
        //        var fields = LineSplitter(line).ToArray();
        //        string url = fields[0];
        //        string linkText = fields[1];
        //        List<QueryStringItem> queryStrings = new List<QueryStringItem>();
        //        for (var i = 2; i < fields.Length; i++)
        //        {
        //            var qs = fields[i].Split('=');
        //            if (qs[0] != "")
        //            {
        //                queryStrings.Add(new QueryStringItem() { Name = qs[0], Value = qs[1] });
        //            }
                    
        //        }

        //        yield return new TestObject() {url = fields[0], queryStrings = queryStrings,ElementText = linkText};
        //        line = file.ReadLine();
        //    }
        //}

       

        [Test,Factory("GetRowNumber")]
        public void DDTTest(int RowNumber)
        {
            TestObject testObject = new TestObject(RowNumber);
            driver.Navigate().GoToUrl(testObject.url);
            if (testObject.ElementText != "")
            {
                driver.FindVisibleElement(By.XPath("//*[text()='" + testObject.ElementText + "']")).Click();
            }
            var lastRequest = proxy.GetLastEntryForUrl("om.healthgrades.com");

            foreach (var querystring in testObject.queryStrings)
            {
                proxy.VerifyQueryStringInEntry(querystring,lastRequest);
            }
        }
        [FixtureInitializer]
        public void init()
        {
            Config.Settings.httpProxy.startProxy = true;
            Config.Settings.httpProxy.useProxy = true;
        }

        [Test]
        public void Test()
        {
            driver.Navigate().GoToUrl("http://www.healthgrades.com/physician/dr-john-schultz-2324x");
            driver.Sleep(3000);
            proxy.VerifyRequestQueryString("om.healthgrades.com",new QueryStringItem(){Name = "c6",Value = "physician"});
        }
        
    }
}
