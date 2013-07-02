using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_HomePage : FmxMenuBar
    {
        Element MessageBoard = new Element("MessageBoard", By.ClassName("MainInfoCollapsibleHeaderText"));
        

        public override void WaitForElements()
        {
            base.WaitForElements();
            MessageBoard.VerifyVisible(10);
        }

        public FMX_HomePage VerifyResult(string text)
        {
            var HomePageVerified = new Element("HomePageVerified", By.XPath("/html/body/form/div[4]/div[3]/div[2]/div[2]/div/div/div[2]/div"));
            HomePageVerified.VerifyVisible(10);
            return new FMX_HomePage();
        }
        
        //Jobs functions
        public FMX_JobsTab ClickJobs()
        {
            tab_Jobs.Click();
            return new FMX_JobsTab();
        }
        public FMX_Jobs_JobRequests NewJobRequest()
        {
            btn_NewJobRequest.Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_DashboardPage ClickDashboard()
        {
            tab_Dashboard.WaitUntilVisible().Click();
            return new FMX_DashboardPage();
        }

        
    }
}
