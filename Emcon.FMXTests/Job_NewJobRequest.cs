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
    class Job_NewJobRequest : TestBaseClass
    {
        private string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test]
        public void JobRequestSearch()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .NewJobRequest()
                          .CustomerSearch("EMCON TEST")
                          .EnterRequestInfo("Repair and Maintenance", "Blue Team", "Store", "ProtoTest Automation",
                                            "webDriver",
                                            "Here is a short Description")
                          .AddNewLocation()
                          .AddNewJob("Quoted", "Standard", "PROTOTEST-0001",
                                     "This is a job created by an automated script");


        }
    }
}
