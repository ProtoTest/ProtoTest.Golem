using System;
using System.Collections.Generic;
using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.PurpleElements;

namespace ProtoTest.Golem.Purple
{
    public class PurpleTestBase : TestBase
    {
        public static List<string> TestOutcomes = new List<string>();
        private string ProjectFile;
        private string TestFileLoc;
        //Used for logging how long it takes elements to appear.
        //Has to be set in the constructor for the testclass
        public static bool PerfLogging { get; set; }

        public string TestFileLocation
        {
            get
            {
                if (TestFileLoc == null)
                {
                    SetFileInfo();
                }
                return TestFileLoc;
            }
        }

        public string ProjectFileName
        {
            get
            {
                if (ProjectFile == null)
                {
                    SetFileInfo();
                }
                return ProjectFile;
            }
        }

        private void SetFileInfo()
        {
            //TODO This is fugly - need a better solution
            TestFileLoc = @"C:\";
            ProjectFile = "NOT CONFIGURED";
            var machineName = Environment.MachineName;
            if (machineName == Config.settings.purpleSettings.Machine1)
            {
                TestFileLoc = Config.settings.purpleSettings.DataSetPath1;
                ProjectFile = Config.settings.purpleSettings.ProjectName1;
            }
            if (machineName == Config.settings.purpleSettings.Machine2)
            {
                TestFileLoc = Config.settings.purpleSettings.DataSetPath2;
                ProjectFile = Config.settings.purpleSettings.ProjectName2;
            }
            if (machineName == Config.settings.purpleSettings.Machine3)
            {
                TestFileLoc = Config.settings.purpleSettings.DataSetPath3;
                ProjectFile = Config.settings.purpleSettings.ProjectName3;
            }
            if (machineName == Config.settings.purpleSettings.Machine4)
            {
                TestFileLoc = Config.settings.purpleSettings.DataSetPath4;
                ProjectFile = Config.settings.purpleSettings.ProjectName4;
            }
        }

        public void TestSettings()
        {
        }

        [SetUp]
        public void SetUp()
        {
            PurpleWindow.FindRunningProcess();
        }

        [TearDown]
        public override void TearDownTestBase()
        {
            Log.Message(Common.GetCurrentTestName() + " " + TestContext.CurrentContext.Result.Outcome);
            PurpleWindow.EndProcess();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
        }

        public void LogScreenshotIfTestFailed()
        {
            //Will need to rework logscreenshot if failed for NUnit
            //if(TestContext.CurrentContext.Outcome!=TestOutcome.Passed)
            //    TestLog.EmbedImage(null, PurpleWindow.purpleWindow.GetImage());
        }
    }
}