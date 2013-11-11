using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using GolemPageObjects.Emcon.FMNow;
using MbUnit.Framework;

namespace Emcon.FMNowTests
{
    public class Charts : TestBaseClass
    {
        private static string envURL = Config.GetConfigValue("EnvironmentUrl", "http://customerdemo.fmx.bz");
        [Test(Order = 2)]
        [TestsOn("2 - Charts")]
        public void TestCharts()
        {
            FMNow_WelcomePage.OpenFMNow(envURL)
                             .Login("bkitchener@prototest.com", "!Test1234")
                             .ClickFinanceTab()
                             .getInvoiceComparisonByAverageChart(false, false, "Feb", "1", "4", "2010", "2013");


        }
    }
}
