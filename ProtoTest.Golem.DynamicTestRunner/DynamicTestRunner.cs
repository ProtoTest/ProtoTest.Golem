using System.Collections.Generic;
using System.IO;
using Gallio.Framework;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.DynamicTestRunner
{
    internal class Omniture_Test_Runner : WebDriverTestBase
    {



        private string filePath = Config.GetConfigValue("DataCsvPath", Directory.GetCurrentDirectory() + @"\Data.csv");

        //return one test per row found in the file.
        public IEnumerable<int> GetRowNumber()
        {
            if (!File.Exists(filePath))
            {
                Assert.Fail("Could not run tests, " + filePath + " could not be found");
            }
            StreamReader file = new StreamReader(filePath);
            int rowNumber = 0;
            string verb = "Run";
            string line = file.ReadLine();
            while (line != null)
            {
                if (line.StartsWith("Only"))
                {
                    verb = "Only";
                    break;
                }

                line = file.ReadLine();
            }
            file.BaseStream.Position = 0;
            file.DiscardBufferedData();
            line = file.ReadLine();
            while (line != null)
            {
                if (line.StartsWith(verb))
                {
                    yield return rowNumber + 1;
                }
                rowNumber++;
                line = file.ReadLine();

            }

        }

        //This will run one test per row found in our Data.csv file.  
        [Timeout(0)]
        [Test, Factory("GetRowNumber")]
        public void RunTestFromCSV(int RowNumber)
        {
            //Get an object that contains the appropriate row's data
            CSVRowDataObject rowData = new CSVRowDataObject(RowNumber);
            if (rowData.description != "")
                Common.Log("Running test : " + rowData.description);

            //open the url
            driver.Navigate().GoToUrl(rowData.domain + rowData.url);

            //click each element 
            if (rowData.ElementsToClick.Count > 0)
            {
                foreach (var ele in rowData.ElementsToClick)
                {
                    ele.WaitUntil().Present().Click();
                }

            }


            TestLog.BeginSection("Verifications");
            //Verify each element 
            if (rowData.ElementsToValidate.Count > 0)
            {
                foreach (var ele in rowData.ElementsToValidate)
                {
                    ele.Verify().Present();
                }

            }
            TestLog.End();
        }

    }
}
