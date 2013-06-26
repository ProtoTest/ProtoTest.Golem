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
        public static List<string> requestNumbers = new List<string>();  //using this to store requests created during automation for easy access
        //Above Menu Bar Objects
        Element btn_JobSearch = new Element("JobSearchButton", By.Id("ctl00_ContentPlaceHolder2_btnSearch"));

        //job Requests Sub-tab
        Element tab_Job_JobRequests = new Element("JobRequestsTab", By.Id("ctl00_ContentPlaceHolder1_ucJobMenu_lnkJobs"));
        Element table_JobRequests_CustomerName = new Element("JobRequestsTable", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_fvRequest']/tbody/tr/td/table/tbody/tr[4]/td[2]"));
        Element btn_JobRequests_Request = new Element("RequestButton", By.Id("ctl00_ContentPlaceHolder1_lnkbRequestRequest"));
        Element btn_JobRequests_Location = new Element("LocationButton", By.Id("ctl00_ContentPlaceHolder1_lnkbRequestLocation"));
        Element txt_RequestNumber = new Element("txt_RequestNumber", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_fvRequest']/tbody/tr/td/table/tbody/tr[2]/td[2]"));
        //New Job-Request Specific
        Element btn_JobRequests_CustomerSearch = new Element("CustomerSearchButton", By.Id("ctl00_ContentPlaceHolder1_fvRequest_lbPopSearchCust"));
        Element dd_BusinessType = new Element("dd_BuisnessType", By.Id("ctl00_ContentPlaceHolder1_fvRequest_ddlBusinessType"));
        Element dd_Team = new Element("dd_Team", By.Id("ctl00_ContentPlaceHolder1_fvRequest_ddlTeam"));
        Element dd_RequestSource = new Element("dd_RequestSource", By.Id("ctl00_ContentPlaceHolder1_fvRequest_ddlRequestSource"));  //has a default value
        Element txt_RequesterName = new Element("txt_RequesterName", By.Id("ctl00_ContentPlaceHolder1_fvRequest_txtRequesterName"));
        Element txt_RequesterTitle = new Element("txt_RequesterTitle", By.Id("ctl00_ContentPlaceHolder1_fvRequest_txtRequesterTitle"));
        Element txt_RequestDescription = new Element("txt_RequestDescription", By.Id("ctl00_ContentPlaceHolder1_fvRequest_txtRequestDescription"));
        Element btn_SaveJobRequest = new Element("btn_SaveJobRequest", By.Id("ctl00_ContentPlaceHolder1_fvRequest_lbtnInsertRequest1"));

        //New Location
        Element btn_AddNewLocation = new Element("AddNewLocationButton", By.Id("ctl00_ContentPlaceHolder1_fvLocation_lbtnNewLocation"));
        Element btn_SaveLocation = new Element("SaveLocationButton", By.Id("ctl00_ContentPlaceHolder1_fvLocation_lbtnInsertLocation2"));

        //Job-Requests - Job Subtab
        Element btn_JobRequests_JobSubTab = new Element("JobRequest_NewJobSubTabButton", By.Id("ctl00_ContentPlaceHolder1_lnkbLocationJob"));
        Element btn_JobRequests_NewJobButton = new Element("JobRequest_NewJob_Button", By.Id("ctl00_ContentPlaceHolder1_fvJob_lbtnNewJob"));
        Element dd_JobRequests_NewJob_JobType = new Element("NewJobType_Dropdown", By.Id("ctl00_ContentPlaceHolder1_fvJob_ddlJobType"));
        Element dd_JobRequests_NewJob_JobPriority = new Element("NewJobPriority_dropdown", By.Id("ctl00_ContentPlaceHolder1_fvJob_ddlJobPriority"));
        Element txt_JobRequests_NewJob_JobPONumber = new Element("NewJob_PONUmber_textfield", By.Id("ctl00_ContentPlaceHolder1_fvJob_txtCustomerPONo"));
        Element txt_JobRequests_NewJob_Comment = new Element("NewJob_Comment_textfield", By.Id("ctl00_ContentPlaceHolder1_fvJob_txtEmconComment"));
        Element numtxt_JobRequests_NewJob_CustomerJobCap = new Element("NewJob_CustomerJobCap_numtxt", By.Id("ctl00_ContentPlaceHolder1_fvJob_txtJobCap"));
        Element btn_JobRequests_NewJob_SaveRequest = new Element("SaveNewJobRequest_button", By.Id("ctl00_ContentPlaceHolder1_fvJob_lbtnInsertJob2"));

        //Dynamic Search PopUp
        Element pop_DynamicSearch = new Element("DynamicSearch_JobSearch_Popup", By.Id("ctl00_ContentPlaceHolder1_pnlPopUpSearch"));
        Element txt_CustomerName_DynamicSearch_Pop = new Element("CustomerNameSearchField", By.Id("ctl00_ContentPlaceHolder1_ucDynamicSearch_rpSearchForm_ctl01_txtTextBoxStr"));
        Element btn_SearchButton = new Element("DyanicSearchButton", By.Id("ctl00_ContentPlaceHolder1_ucDynamicSearch_btnSearch1"));
        Element table_SearchResultsTable = new Element("DynamicSearchResultsTable", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucDynamicSearch_ucCustomerSearchResultsGrid_gvGrid']/tbody/tr[3]/td[2]"));
        Element btn_PopupClose = new Element("PopupCloseButton", By.Id("ctl00_ContentPlaceHolder1_LinkButton1"));

        //Customer Search Popup
        Element pop_CustomerSearch = new Element("CustomerSearch_Popup", By.Id("ctl00_ContentPlaceHolder1_pnlPopUpCust"));
        Element txt_CustomerName_CustomerSearch_pop = new Element("txt_CustomerSearchName_Pop", By.Id("ctl00_ContentPlaceHolder1_ucCustomerParentSearch_rpSearchForm_ctl01_txtTextBoxStr"));
        Element btn_CustomerSearch = new Element("CustomerSearchButton", By.Id("ctl00_ContentPlaceHolder1_ucCustomerParentSearch_btnSearch1"));
        Element table_CustomerSearchResults_grid = new Element("Table_CustomerSearchResultsgrid", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucCustomerParentSearch_ucCustomerSearchResultsGrid_gvGrid']/tbody/tr[2]/td[2]"));

        //this should be set by the search field
        private static string CustomerName;
        
        public FMX_JobsTab SearchJobs()
        {
            btn_JobSearch.VerifyVisible(5);
            btn_JobSearch.Click();
            return new FMX_JobsTab();
        }

        public FMX_JobsTab CustomerSearch(string customerName)
        {
            pleaseWait.WaitUntilNotPresent();
            btn_JobRequests_CustomerSearch.VerifyVisible();
            btn_JobRequests_CustomerSearch.Click();
            pleaseWait.WaitUntilNotPresent();
            pop_CustomerSearch.VerifyVisible(5);
            txt_CustomerName_CustomerSearch_pop.VerifyVisible(5);
            txt_CustomerName_CustomerSearch_pop.Text = customerName;
            btn_CustomerSearch.VerifyVisible();
            btn_CustomerSearch.Click();
            pleaseWait.WaitUntilNotPresent();
            table_CustomerSearchResults_grid.Click();
            return new FMX_JobsTab();
        }

        public FMX_JobsTab EnterRequestInfo(string businessType, string businessTeam, string requestSource,
                                            string requesterName, string requesterTitle, string requestDescription)
        {
            pleaseWait.WaitUntilNotPresent();
            dd_BusinessType.FindElement(ByE.Text(businessType)).Click();
            pleaseWait.WaitUntilNotPresent();
            //there is a check here to see if team has available business types --this test assumes you know which team's go with which business types
            dd_Team.FindElement(ByE.Text(businessTeam)).Click();
            pleaseWait.WaitUntilNotPresent();
            dd_RequestSource.FindElement(ByE.Text(requestSource)).Click();
            txt_RequesterName.VerifyVisible(5);
            txt_RequesterName.Text = requesterName;
            txt_RequesterTitle.Text = requesterTitle;
            txt_RequestDescription.Text = requestDescription;
            btn_SaveJobRequest.Click();
            return new FMX_JobsTab();
        }

        public FMX_JobsTab AddNewLocation()
        {
            pleaseWait.WaitUntilNotPresent();
            btn_AddNewLocation.WaitUntilVisible().Click(); //Default Data is selected for EMCON TEST customer
            pleaseWait.WaitUntilNotPresent();
            btn_SaveLocation.Click();
            pleaseWait.WaitUntilNotPresent();
            btn_JobRequests_JobSubTab.WaitUntilVisible().Click();
            return new FMX_JobsTab();
        }

        public FMX_JobsTab AddNewJob(string jobType, string jobPriority, string POnumber, string JobComment, int jobCap = 9999)
        {
            btn_JobRequests_NewJobButton.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotPresent();
            dd_JobRequests_NewJob_JobType.FindElement(ByE.Text(jobType));
            pleaseWait.WaitUntilNotPresent();
            dd_JobRequests_NewJob_JobPriority.FindElement(ByE.Text(jobPriority));
            txt_JobRequests_NewJob_JobPONumber.Text = POnumber;
            txt_JobRequests_NewJob_Comment.Text = JobComment;
            numtxt_JobRequests_NewJob_CustomerJobCap.Text = jobCap.ToString();
            btn_JobRequests_NewJob_SaveRequest.Click();
            pleaseWait.WaitUntilNotVisible();
            return new FMX_JobsTab();
        }


        public FMX_JobsTab SearchCustomer(string customerName)
        {
            pop_DynamicSearch.VerifyVisible(5);
            txt_CustomerName_DynamicSearch_Pop.VerifyVisible(5);
            txt_CustomerName_DynamicSearch_Pop.Text = customerName;
            btn_SearchButton.Click();
            table_SearchResultsTable.VerifyVisible(5);
            CustomerName = table_SearchResultsTable.Text;
            table_SearchResultsTable.Click();
            return new FMX_JobsTab();
        }

        public FMX_JobsTab CloseSearchPopUp()
        {
            pleaseWait.WaitUntilNotPresent();
            btn_PopupClose.Click();
            return new FMX_JobsTab();
        }

        public FMX_JobsTab VerifyRequestPresent(string customerName)
        {
           pleaseWait.WaitUntilNotPresent();
           table_JobRequests_CustomerName.FindElements(By.LinkText(customerName));
           return new FMX_JobsTab();
        }

        public FMX_JobsTab JobRequestsSubTab()
        {
            tab_Job_JobRequests.Click();
            return new FMX_JobsTab();
        }

        

        public override void WaitForElements()
        {
            base.WaitForElements();
            pleaseWait.WaitUntilNotPresent();
            tab_Job_JobRequests.VerifyVisible(5);
        }

       

    }
}

