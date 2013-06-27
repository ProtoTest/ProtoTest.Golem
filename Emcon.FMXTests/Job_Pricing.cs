using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.PageObjects.Emcon.FMX;
using MbUnit.Framework;

namespace Emcon.FMXTests
{
    public class Job_Pricing : TestBaseClass
    {
        private static string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test(Order = 2)]
        [TestsOn("9 - Job Pricing")]
        public void Test_Job_Pricing()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickJobs()
                          .ClickJobPricingTab()
                          .Select_AR_AP_NoPricing()
                          .ClickSearchButton()
                          .SelectJobByIndex(1)
                          .ClickVendorLink()
                          .ClickSetAccountsReceivable();


        }
    }
}
