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
        private string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");
        [Test(Order = 3)]
        [TestsOn("2 - Charts")]
        public void DashboardTest()
        {
            FmXwelcomePage.OpenFMX(envUrl)
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
