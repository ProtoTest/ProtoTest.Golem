using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.PageObjects.Emcon.FMX;
using MbUnit.Framework;

namespace Emcon.FMXTests
{
    public class Job_Closing : TestBaseClass
    {
        private static string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test(Order = 8)]
        [TestsOn("8 - Job Closing")]
        public void CloseJob()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickJobs()
                          .ClickJobClosingTab()
                          //should add a check for if the record is locked before clicking it
                          .SelectJobByIndex(3) //TheFirst Job may already be set to closed changed to 3
                          .CloseNotesPopup()
                          .SelectJobOutcome("Job Done")
                          .SelectIfSatisfactory(true)
                          .EnterOutcomeInfo("TestFirstName", "TestLastName", "Test notes")
                          .SaveOutcomeChanges();

        }
    }
}
