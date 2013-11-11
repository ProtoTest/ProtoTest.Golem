using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

//This class is dedicated to the "Jobs" menu tab
namespace Golem.PageObjects.Emcon.FMX
{
    public class FmxJobsPage : BasePageObject
    {
        //These elements are found along the top of the jobs page
        Element cbo_QuickSearch = new Element("QuickSearch", By.Id("ct100_ContentPlaceHolder3_UcQuickSearch1_ddlSearchTerms"));
        Element btn_VendorSearch = new Element("VendorSearch", By.Id("ct100_ContentPlaceHolder2_lbtnVendorST"));
        Element btn_RtvIssues = new Element("RtvIssues", By.Id("ct100_ContentPlaceHolder2_lbtnRTV"));
        Element btn_HeaderSearch = new Element("HeaderSearch", By.Id("ct100_ContentPlaceHolder2_btnSearch"));
        Element btn_TopNewJob = new Element("TopNewJob", By.Id("ct100_lbtnNewRequest"));
        //These elements belong to the "Job Requests" sub-tab
        Element div_JobRequest = new Element("JobRequest", By.Id("ct100_ContentPlaceHolder1_ucJobMenu_lnkJobs")); 
        Element div_PendingJobs = new Element("PendingJobs", By.Id("ct100_ContentPlaceHolder1_ucJobMenu_lnkPending"));
        Element div_JobClosing = new Element("JobClosing", By.Id("ct100_ContentPlaceHolder1_ucJobMenu_lnkClosing"));
        Element div_JobPricing = new Element("JobPricing", By.Id("ct100_ContentPlaceHolder1_ucJobMenu_lnkPricing"));
        Element div_RtvDeficiencies = new Element("RtvDeficiencies", By.Id("ct100_ContentPlaceHolder1_ucJobMenu_lnkRTV"));
        Element btn_HideJobRequest = new Element("HideJobRequest", By.XPath("//div[@class='MainInfoCollapsibleHeader']/div[contains(@style,'Collapse_large')]"));
        Element btn_ShowJobRequest = new Element("ShowJobRequest", By.XPath("//div[@class='MainInfoCollapsibleHeader']/div[contains(@style, 'Expand_large')]"));
        Element btn_NewJobRequest = new Element("NewJobRequest", By.Id("ct100_ContentPlaceHolder1_fvRequest_lbtnNewRequest"));
        Element btn_TopSave = new Element("TopSave", By.Id("ct100_ContentPlaceHolder1_fvRequest_lbtnInsertRequest1"));
        Element btn_BottomSave = new Element("BottomSave", By.Id("ct100_ContentPlaceHolder1_fvRequest_lbtnInsertLocation2"));
        Element btn_TopCancel = new Element("TopCancel", By.Id("ct100_ContentPlaceHolder1_fvRequest_lbtnCancelRequest1"));
        Element btn_BottomCancel = new Element("BottomCancel", By.Id("ct100_ContentPlaceHolder1_fvRequest_lbtnCancelLocation2"));
        Element btn_CustSearch = new Element("CustSearch", By.Id("ct100_ContentPlaceHolder1_fvRequest_lbtnPopSearchCust"));
        Element cbo_CustId = new Element("CustId", By.Id("ct100_ContentPlaceHolder1_fvRequest_ddlCustomerID"));
        Element cbo_CommId = new Element("CommId", By.Id("ct100_ContentPlaceHolder1_fvRequest_ddlCommunicationID"));
        Element cbo_BusinessType = new Element("BusinessType", By.Id("ct100_ContentPlaceHolder1_fvRequest_ddlBusinessType"));
        Element cbo_Team = new Element("Team", By.Id("ct100_ContentPlaceHolder1_fvRequest_ddlTeam"));
        Element cbo_SourceRequest = new Element("SourceRequest", By.Id("ct100_ContentPlaceHolder1_fvRequest_ddlRequestSource"));
        Element txtRequesterName = new Element("RequesterName", By.Id("ct100_ContentPlaceHolder1_fvRequest_txtRequesterName"));
        Element txtRequesterTitle = new Element("RequesterTitle", By.Id("ct100_ContentPlaceHolder1_fvRequest_txtRequesterTitle"));
        Element txtRequestDesc = new Element("RequestDesc", By.Id("ct100_ContentPlaceHolder1_fvRequest_txtRequestDescription"));

        //This function clicks on the top vendor search button, which opens up a new vendor search window
        public VendorSearchPopup OpenTopVendorSearch()
        {
            btn_VendorSearch.Click();
            return new VendorSearchPopup();
        }

        //This function clicks on the top "RTV" button
        public FmxJobsPage OpenTopFmxPane()
        {
            btn_RtvIssues.Click();
            return new FmxJobsPage();
        }
        
        //This function clicks on the "Create New Request" link
        public FmxJobsPage OpenNewJobRequest()
        {
            btn_NewJobRequest.Click();
            return new FmxJobsPage();
        }

        //This function clicks on the "Pending Jobs" sub-tab
        public FmxJobsPage OpenPendingJobs()
        {
            div_PendingJobs.Click();
            return new FmxJobsPage();
        }

        //This is verifying that the sub-tab elements are present on the page
        public override void WaitForElements()
        {
            div_JobRequest.Verify.Present();
            div_PendingJobs.Verify.Present();
            div_JobClosing.Verify.Present();
            div_JobPricing.Verify.Present();
            div_RtvDeficiencies.Verify.Present();
        }
    }
}
