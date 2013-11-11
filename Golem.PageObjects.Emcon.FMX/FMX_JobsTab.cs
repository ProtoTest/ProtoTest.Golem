using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Golem.Framework;
using OpenQA.Selenium;


namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_JobsTab : FmxMenuBar
    {
        //Above Menu Bar Objects
        Element btn_JobSearch = new Element("JobSearchButton", By.Id("ctl00_ContentPlaceHolder2_btnSearch"));

        //job sub-tabs
        Element tab_Job_Closing = new Element("JobClosingTab", By.Id("ctl00_ContentPlaceHolder1_ucJobMenu_lnkClosing"));
        Element tab_Job_Pricing = new Element("JobPricingTab", By.Id("ctl00_ContentPlaceHolder1_ucJobMenu_lnkPricing"));
        Element tab_Job_JobRequests = new Element("JobRequestsTab", By.Id("ctl00_ContentPlaceHolder1_ucJobMenu_lnkJobs"));

        Element btn_RTVIssues = new Element("btn_RTVIssues", By.Id("ctl00_ContentPlaceHolder2_lbtnRTV"));

        public FMX_Jobs_JobRequests SearchJobs()
        {
            btn_JobSearch.WaitUntil.Present().Click();
            return new FMX_Jobs_JobRequests();
        }
        

        public FMX_JobsTab JobRequestsSubTab()
        {
            tab_Job_JobRequests.Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobClosing ClickJobClosingTab()
        {
            tab_Job_Closing.WaitUntil.Visible().Click();
            return new FMX_Jobs_JobClosing();
        }

        public FMX_Jobs_JobPricing ClickJobPricingTab()
        {
            tab_Job_Pricing.WaitUntil.Visible().Click();
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_RTVIssues ClickRTVIssuesButton()
        {
            btn_RTVIssues.WaitUntil.Visible().Click();
            return new FMX_Jobs_RTVIssues();
        }

        
        public override void WaitForElements()
        {
            base.WaitForElements();
            tab_Job_JobRequests.Verify.Visible();
        }

       

    }
}

