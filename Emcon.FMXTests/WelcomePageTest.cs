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
        [Test]
        public static void LoginTest()
        {
            FmXwelcomePage.OpenFMX(true).Login("PROTOTEST", "!TEST1234").VerifyResult("EmconMessageBoard");
        }
    }
}
