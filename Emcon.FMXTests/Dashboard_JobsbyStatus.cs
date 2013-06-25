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
    class Dashboard_JobsbyStatus : TestBaseClass
    {
        [Test]
        public static void DashboardTest()
        {
            FmXwelcomePage.OpenFMX(false)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickDashboard()
                          .SelectTeam("Blue Team")
                          .SelectJobCounts("Business Type")
                          .RefreshChart()
                          .SelectPieChartItem()
                          .SelectHighChartItem()
                          .VerifyHighChartDisplayed();
        }
    }
}
