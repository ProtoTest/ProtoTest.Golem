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
        [Test]
        [TestsOn("8 - Job Closing")]
        public void CloseJob()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickJobs()
                          .ClickJobClosingTab()
                          .SelectJobByIndex(1)
                          .CloseNotesPopup()
                          .SelectJobOutcome("Job Done")
                          .SelectIfSatisfactory(true)
                          .EnterOutcomeInfo("TestFirstName", "TestLastName", "Test notes")
                          .SaveOutcomeChanges();

        }
    }
}
