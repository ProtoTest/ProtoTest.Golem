using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using GolemPageObjects.Emcon.FMNow;
using MbUnit.Framework;

namespace Emcon.FMNowTests
{
    class AdvancedSearch : TestBaseClass
    {
        private static string envURL = Config.GetConfigValue("EnvironmentUrl", "http://customerdemo.fmx.bz");
        [Test(Order = 5)]
        [TestsOn("5 - AdvancedSearch")]
        public void TestAdvancedSearch()
        {
            FMNow_WelcomePage.OpenFMNow(envURL)
                             .Login("bkitchener@prototest.com", "!Test1234")
                             .ClickSearchMainTab()
                             .doAdvancedSearch("Bid Requests", "NJ", "On-Hold", "Standard", "Drywall",
                                               "Drywall Installation"); //BLOCKED Searchs do not return results
        }
    }
}
