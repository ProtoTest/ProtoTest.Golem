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
    class Dashboard_InvoiceComparisonbyAverageChart : TestBaseClass
    {
        private string envUrl = Config.GetConfigValue("EnvironmentUrl","http://demo.fmx.bz/");
        [Test(Order = 2)]
        [TestsOn("2 - Charts")]
        public void DashboardTestTwo()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickDashboard()
                          .SelectTeam("All Teams")
                          .SelectJobCounts("Specific Customer")
                          .SpecificCustomerName("ABERCROMBIE AND FITCH")
                          .RefreshChart()
                          .SelectHighChartItem()
                          .SelectSecondaryChart()
                          .VerifyJobCountListDisplayed();

        }
    }
}
