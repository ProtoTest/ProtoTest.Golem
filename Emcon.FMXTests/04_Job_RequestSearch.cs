using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.PageObjects.Emcon.FMX;
using MbUnit.Framework;
using OpenQA.Selenium;

namespace Emcon.FMXTests
{
    
    class Job_RequestSearch : TestBaseClass
    {
        private string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test(Order = 4)]
        [TestsOn("4 - Charts")]
        public void JobSearch()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickJobs()
                          .SearchJobs()
                          .DynamicSearch("Emcon")
                          .CloseSearchPopUp()
                          .VerifyRequestPresent("Emcon");
        }
    }
}
