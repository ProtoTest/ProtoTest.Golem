using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_DashboardPage : FmxMenuBar
    {
        Element Dashboard_Tab1 = new Element("Dashboard_Tab1", By.Id("ctl00_ContentPlaceHolder1_ucChartMenu_lnkChartTab1"));
        Element dd_TeamSelector = new Element("TeamSelectionDropDown", By.Id("ctl00_ContentPlaceHolder1_ucDailyReports_ddlDailyTeam"));
        //Element pleaseWait = new Element("PleaseWaitOverlay", By.XPath("//*[@id='ctl00_UpdateProg1']"));
        //Pie Chart stuff
        Element Piechart_PieChart = new Element("PieChart", By.Id("ctl00_ContentPlaceHolder1_ucDailyReports_upDailyRptChart"));  //This will always display even when loading
        Element Piechart_LoadingPieChart = new Element("LoadingPieChart", By.XPath("//*[@class='highChartPie']/*[@class='highcharts-container']/*[@class='highcharts-loading']"));
        Element PieChartItem9 = new Element("PieChartItem9", By.XPath("//*[name()='svg']/*[name()='g'][position()=20]/*[name()='g']/*[name()='g']/*[name()='g'][position()=9]/*[name()='text']/*[name()='tspan']"));
        Element dd_JobCountFilter = new Element("JobCountFilterDropDown", By.Id("ctl00_ContentPlaceHolder1_ucJobCountChart_ddlJobCountFilterType"));

        Element btn_RefreshChart = new Element("RefreshChartButton", By.Id("ctl00_ContentPlaceHolder1_ucJobCountChart_btnJobCountRefresh"));
        Element HighChart_Month9 = new Element("HighChart_Month9", By.XPath("//*[@id='jobCountChart']/*[@class='highcharts-container']/*[name()='svg']/*[name()='g'][position()=11]/*[name()='g']/*[name()='rect'][position()=9]"));
        Element HighChart_JobCountChart = new Element("HighChart_JobCountChart", By.Id("jobCountChart"));
        Element HighChart_MonthJobCountTotal = new Element("HighChart_MonthJobCountTotal", By.XPath("//*[@id='highcharts-86']/*[name()='svg']/*[name()='g'][position()=12]/*[name()='g']/*[name()='rect'][position()=2]"));
        Element HighChart_MonthJobCountTotal_Item1 = new Element("HighChart_MonthJobCountTotal_Item1", By.XPath("//*[@id='jobCountChart']/*[@class='highcharts-container']/*[name()='svg']/*[name()='g'][position()=12]/*[name()='g']/*[name()='rect']"));
        Element CustomerNameField = new Element("CustomerNameField", By.Id("ctl00_ContentPlaceHolder1_ucJobCountChart_txtJobCountClientLookup"));
        Element JobCountsPageSelector = new Element("JobCountsPageSelector", By.Id("ctl00_ContentPlaceHolder1_ucJobCountChart_gucJobs_ddlPageSize"));


        //public bool notWaiting()
        //{
        //    while (pleaseWait.GetAttribute("aria-hidden") == "false")
        //    {
        //        Thread.Sleep(100);
        //    }
        //    return true;
        //}

        public FMX_DashboardPage SelectTeam(string dropdownItem)
        {
            PieChartItem9.Wait();
            dd_TeamSelector.Wait();
            if (notWaiting())
            {
                dd_TeamSelector.FindElement(ByE.Text(dropdownItem)).Click();
            }
            return new FMX_DashboardPage();
        }

        public FMX_DashboardPage SelectJobCounts(string dropdownItem)
        {
            PieChartItem9.Wait();
            dd_JobCountFilter.Wait();
            if (notWaiting())
            {
                dd_JobCountFilter.FindElement(ByE.Text(dropdownItem)).Click();
            }
            return new FMX_DashboardPage();
        }

        public FMX_DashboardPage RefreshChart()
        {
            PieChartItem9.Wait();
            btn_RefreshChart.Wait();
            if (notWaiting())
            {
                btn_RefreshChart.Click();
            }
            return new FMX_DashboardPage();
        }

        public FMX_DashboardPage SelectPieChartItem()
        {
            //always selecting the last item --this item is always filled in
            PieChartItem9.Wait();
            PieChartItem9.VerifyVisible(20);
            if (notWaiting())
            {
                PieChartItem9.Click();
            }
            return new FMX_DashboardPage();
        }

        public FMX_DashboardPage SelectHighChartItem()
        {

            PieChartItem9.Wait();
            HighChart_JobCountChart.VerifyVisible(10);
            HighChart_Month9.VerifyVisible(30);
            if(notWaiting())
            {
                HighChart_Month9.Click();
            }
            return new FMX_DashboardPage();
        }

        public FMX_DashboardPage VerifyHighChartDisplayed()
        {
            HighChart_JobCountChart.VerifyVisible(10);
            HighChart_MonthJobCountTotal.VerifyVisible(10);
            return new FMX_DashboardPage();
        }

        public FMX_DashboardPage SpecificCustomerName(string customerName)
        {
            PieChartItem9.Wait();
            CustomerNameField.Wait();
            if (notWaiting())
            {
                CustomerNameField.Text = customerName;
                CustomerNameField.SendKeys(Keys.Return);
            }
            return new FMX_DashboardPage();
        }
       
        public FMX_DashboardPage SelectSecondaryChart()
        {
            PieChartItem9.Wait();
            HighChart_MonthJobCountTotal.VerifyVisible(5);
            if (notWaiting())
            {
                HighChart_MonthJobCountTotal_Item1.Click();
            }
            return new FMX_DashboardPage();
        }

        public FMX_DashboardPage VerifyJobCountListDisplayed()
        {
            PieChartItem9.Wait();
            if (notWaiting())
            {
                JobCountsPageSelector.VerifyVisible(10);
            }
            return new FMX_DashboardPage();
        }

        public new void WaitForElements()
        {
            base.WaitForElements();
            Dashboard_Tab1.VerifyVisible(10);
            Piechart_PieChart.VerifyVisible(10);
            PieChartItem9.VerifyVisible(5);
        }
        
    }
}
