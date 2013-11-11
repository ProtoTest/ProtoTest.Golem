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
    public class WelcomePageTest : TestBaseClass
    {
        //adding a comment to test github client sync fix - surban 06/25/13
        private string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test(Order = 1)]
        [TestsOn("1 - Login")]
        public void LoginTest()
        {
            var delay = Config.Settings.runTimeSettings.CommandDelayMs;
            FmXwelcomePage.OpenFMX(envUrl).Login("PROTOTEST", "!TEST1234").VerifyResult("EmconMessageBoard");
        }
    }
}
