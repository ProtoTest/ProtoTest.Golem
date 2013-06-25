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
        [Test]
        public static void DashboardTestTwo()
        {
            FmXwelcomePage.OpenFMX(false)
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
