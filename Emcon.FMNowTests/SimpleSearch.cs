using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using GolemPageObjects.Emcon.FMNow;
using MbUnit.Framework;

namespace Emcon.FMNowTests
{
    class SimpleSearch : TestBaseClass
    {
        private static string envURL = Config.GetConfigValue("EnvironmentUrl", "http://customerdemo.fmx.bz");
        [Test(Order = 4)]
        [TestsOn("4 - SimpleSearch")]
        public void TestSimpleSearch()
        {
            FMNow_WelcomePage.OpenFMNow(envURL)
                             .Login("bkitchener@prototest.com", "!Test1234")
                             .ClickSearchMainTab()
                             .doSimpleSearch("Job Status", "On-Hold"); 


        }
    }
}
