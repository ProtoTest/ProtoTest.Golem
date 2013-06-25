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
    
    class JobRequestSearch : TestBaseClass
    {
        [Test]
        public static void JobSearch()
        {
            FmXwelcomePage.OpenFMX(true)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickJobs()
                          .SearchJobs()
                          .SearchCustomer("Emcon")
                          .CloseSearchPopUp()
                          .VerifyRequestPresent("Emcon");
        }
    }
}
