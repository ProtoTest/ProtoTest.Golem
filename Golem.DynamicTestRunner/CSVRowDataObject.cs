using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.DynamicTestRunner
{

    public class CSVRowDataObject
    {
        public enum RowFormat
        {
            Verb =0,
            Description=1,
            Domain=2,
            Url=3,
            ElementsToClick=4,
            Validations=5
        }
        private string filePath = Config.GetConfigValue("DataCsvPath",
            Directory.GetCurrentDirectory() + @"\Data.csv");

        public int rowNumber;
        public string url;
        public List<Element> ElementsToClick = new List<Element>();
        public List<Element> ElementsToValidate = new List<Element>();

        public string[] fieldsForRow;
        public string elementsField;
        public string validationsField;
        public string lineString;
        public string domain;
        public string description;
        public string verb;

        public CSVRowDataObject(int rowNum)
        {
            try
            {

            this.rowNumber = rowNum;
            this.lineString = GetLineByRow(rowNum);
            this.fieldsForRow = LineSplitter(lineString).ToArray();
            this.verb = fieldsForRow[(int) RowFormat.Verb];
            this.description = fieldsForRow[(int) RowFormat.Description];
            this.domain = fieldsForRow[(int) RowFormat.Domain];
            this.url = fieldsForRow[(int) RowFormat.Url];
            this.elementsField = fieldsForRow[(int) RowFormat.ElementsToClick];
            this.validationsField = fieldsForRow[(int)RowFormat.Validations];

            foreach (var ele in elementsField.Split(','))
            {
                if (ele != "")
                {
                    if (ele.Contains("//"))
                        ElementsToClick.Add(new Element(By.XPath(ele)));
                    else
                        ElementsToClick.Add(new Element(By.XPath("//*[text()='" + ele + "']")));
                }
            }

            foreach (var ele in validationsField.Split(','))
            {
                if (ele != "")
                {
                    if (ele.Contains("//"))
                        ElementsToValidate.Add(new Element(By.XPath(ele)));
                    else
                        ElementsToValidate.Add(new Element(By.XPath("//*[text()='" + ele + "']")));
                }
            }

            }
            catch (Exception e)
            {
                throw new FormatException("The CSV file is not in the correct format, please check it and try again." + e.Message);
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
            for (var i = 0; i < rowNum; i++)
            {
                line = file.ReadLine();
            }
            return line;
        }

        private IEnumerable<string> LineSplitter(string line)
        {
            int fieldStart = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',')
                {
                    string field = line.Substring(fieldStart, i - fieldStart).Replace("\"", "");
                    //add each column to our list
                    yield return field;
                    fieldStart = i + 1;
                }
                if (line[i] == '"')
                    for (i++; line[i] != '"'; i++)
                    {
                    }
            }
            //add the field after the last comma
            if (fieldStart != line.Length - 1)
                yield return line.Substring(fieldStart, line.Length - fieldStart).Replace("\"", "");

        }
    }

}
