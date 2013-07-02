using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using GolemPageObjects.Emcon.FMNow;
using MbUnit.Framework;

namespace Emcon.FMNowTests
{
    class Request : TestBaseClass
    {
        private static string envURL = Config.GetConfigValue("EnvironmentUrl", "http://customerdemo.fmx.bz");
        [Test(Order = 3)]
        [TestsOn("3 - Request")]
        public void TestRequest()
        {
            FMNow_WelcomePage.OpenFMNow(envURL)
                             .Login("bkitchener@prototest.com", "!Test1234")
                             .ClickRequestMainTab()
                             .SearchLocation("NJ"); //BLOCKED at this point no search returns data
        }

    }
}
