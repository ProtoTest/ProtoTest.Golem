using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using GolemPageObjects.Emcon.FMNow;
using MbUnit.Framework;

namespace Emcon.FMNowTests
{
    public class Login : TestBaseClass
    {
        private static string envURL = Config.GetConfigValue("EnvironmentUrl", "http://customerdemo.fmx.bz");
        [Test(Order = 1)]
        [TestsOn("1 - Login")]
        public void TestLogin()
        {
            FMNow_WelcomePage.OpenFMNow(envURL)
               .Login("bkitchener@prototest.com", "!Test1234");
        }
    }
}
