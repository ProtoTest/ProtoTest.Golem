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
        Element tab_Pending_Jobs = new Element("PendingJobsTab", By.Id("ctl00_ContentPlaceHolder1_ucJobMenu_lnkPending"));
        Element tab_RTV_Deficiencies = new Element("RTVDeficienciesTab", By.Id("ctl00_ContentPlaceHolder1_ucJobMenu_lnkRTV"));
        Element tab_Job_JobRequests = new Element("JobRequestsTab", By.Id("ctl00_ContentPlaceHolder1_ucJobMenu_lnkJobs"));

        public FMX_Jobs_JobRequests SearchJobs()
        {
            btn_JobSearch.VerifyVisible(5);
            btn_JobSearch.Click();
            return new FMX_Jobs_JobRequests();
        }
        

        public FMX_JobsTab JobRequestsSubTab()
        {
            tab_Job_JobRequests.Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobClosing ClickJobClosingTab()
        {
            tab_Job_Closing.Click();
            return new FMX_Jobs_JobClosing();
        }

        public FMX_Jobs_JobPricing ClickJobPricingTab()
        {
            tab_Job_Pricing.Click();
            return new FMX_Jobs_JobPricing();
        }
        
        public override void WaitForElements()
        {
            base.WaitForElements();
            tab_Job_JobRequests.VerifyVisible(5);
        }

       

    }
}

