using System;
using System.Collections.Generic;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.PurpleElements;


namespace ProtoTest.Golem.Purple
{
    public class PurpleTestBase : TestBase
    {
        //Used for logging how long it takes elements to appear.
        //Has to be set in the constructor for the testclass
        public static bool PerfLogging { get; set; }
        public static List<string> TestOutcomes = new List<string>();

        public string TestFileLocation
        {
            get { return GetTestFile(); }
        }

        private string GetTestFile()
        {
            string TestFileLoc = @"C:\";
            var machineName = Environment.MachineName;
            if (machineName == Config.Settings.purpleSettings.Machine1)
            {
                TestFileLoc = Config.Settings.purpleSettings.DataSetPath1;
            }
            if (machineName == Config.Settings.purpleSettings.Machine2)
            {
                TestFileLoc = Config.Settings.purpleSettings.DataSetPath2;
            }
            if (machineName == Config.Settings.purpleSettings.Machine3)
            {
                TestFileLoc = Config.Settings.purpleSettings.DataSetPath3;
            }
            if (machineName == Config.Settings.purpleSettings.Machine4)
            {
                TestFileLoc = Config.Settings.purpleSettings.DataSetPath4;
            }
            return TestFileLoc;
        }

        public string ProjectFileName
        {
            get
            {
                return "RLF2013TestFile.qig";
            }
        }

        public void TestSettings()
        {
            
        }

        [NUnit.Framework.SetUp]
        [MbUnit.Framework.SetUp]
        public void SetUp()
        {
            PurpleWindow.FindRunningProcess();
        }


        [NUnit.Framework.TearDown]
        [MbUnit.Framework.TearDown]
        public void TearDown()
        {
            //LogScreenshotIfTestFailed();
            var testoutcome = TestContext.CurrentContext.Test.Name + " Result: " + TestContext.CurrentContext.Result.State;
            TestOutcomes.Add(testoutcome);
            PurpleWindow.EndProcess();
            if (PerfLogging)
            {
                //write perflog file
            }
        }
        [NUnit.Framework.TestFixtureTearDown]
        public void FixtureTearDown()
        {
            foreach (var testOutcome in TestOutcomes)
            {
                TestBase.Log(testOutcome);
            }
        }

        public void LogScreenshotIfTestFailed()
        {
            //Will need to rework logscreenshot if failed for NUnit
            //if(TestContext.CurrentContext.Outcome!=TestOutcome.Passed)
            //    TestLog.EmbedImage(null, PurpleWindow.purpleWindow.GetImage());
        }
    }
}