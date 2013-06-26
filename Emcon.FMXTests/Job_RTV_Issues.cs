using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.PageObjects.Emcon.FMX;
using MbUnit.Framework;

namespace Emcon.FMXTests
{
    class Job_RTV_Issues : TestBaseClass
    {
        private static string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test]
        public void Test_RTV_Issues()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickJobs()
                          .JobRequestsSubTab()
                          .ClickJobWithCompletedStatus()
                          .ClickRTVIssuesButton()
                          .
                          
        }
    }
}
