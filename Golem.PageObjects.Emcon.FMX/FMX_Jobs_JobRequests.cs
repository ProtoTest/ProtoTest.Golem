using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_Jobs_JobRequests : FMX_JobsTab
    {
        #region Web ELements
        //job Requests Sub-tab
        Element table_JobRequests_CustomerName = new Element("JobRequestsTable", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_fvRequest']/tbody/tr/td/table/tbody/tr[4]/td[2]"));
        Element btn_JobRequests_Request = new Element("RequestButton", By.Id("ctl00_ContentPlaceHolder1_lnkbRequestRequest"));
        Element btn_JobRequests_Location = new Element("LocationButton", By.Id("ctl00_ContentPlaceHolder1_lnkbRequestLocation"));
        Element txt_RequestNumber = new Element("txt_RequestNumber", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_fvRequest']/tbody/tr/td/table/tbody/tr[2]/td[2]"));

        //Dynamic Search PopUp - From Search Job button
        Element pop_DynamicSearch = new Element("DynamicSearch_JobSearch_Popup", By.Id("ctl00_ContentPlaceHolder1_pnlPopUpSearch"));
        Element txt_CustomerName_DynamicSearch_Pop = new Element("CustomerNameSearchField", By.Id("ctl00_ContentPlaceHolder1_ucDynamicSearch_rpSearchForm_ctl01_txtTextBoxStr"));
        Element btn_SearchButton = new Element("DyanicSearchButton", By.Id("ctl00_ContentPlaceHolder1_ucDynamicSearch_btnSearch1"));
        Element table_SearchResultsTable = new Element("DynamicSearchResultsTable", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucDynamicSearch_ucCustomerSearchResultsGrid_gvGrid']/tbody/tr[3]/td[2]"));
        Element btn_PopupClose = new Element("PopupCloseButton", By.Id("ctl00_ContentPlaceHolder1_LinkButton1"));
        Element drp_JobStatus = new Element("JobStatusDropdown", By.Id("ctl00_ContentPlaceHolder1_ucDynamicSearch_rpSearchForm_ctl04_pnlVocabListBox"));
        Element chk_JobStatus = new Element("JobStatusCheckbox", By.Id("ctl00_ContentPlaceHolder1_ucDynamicSearch_rpSearchForm_ctl04_chkIncludeField"));

        //Customer Search Popup from Job Request tab
        Element pop_CustomerSearch = new Element("CustomerSearch_Popup", By.Id("ctl00_ContentPlaceHolder1_pnlPopUpCust"));
        Element txt_CustomerName_CustomerSearch_pop = new Element("txt_CustomerSearchName_Pop", By.Id("ctl00_ContentPlaceHolder1_ucCustomerParentSearch_rpSearchForm_ctl01_txtTextBoxStr"));
        Element btn_CustomerSearch = new Element("CustomerSearchButton", By.Id("ctl00_ContentPlaceHolder1_ucCustomerParentSearch_btnSearch1"));
        Element table_CustomerSearchResults_grid = new Element("Table_CustomerSearchResultsgrid", By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucCustomerParentSearch_ucCustomerSearchResultsGrid_gvGrid']/tbody/tr[2]/td[2]"));

        //New Job-Request Request SubTab
        Element btn_JobRequests_CustomerSearch = new Element("CustomerSearchButton", By.Id("ctl00_ContentPlaceHolder1_fvRequest_lbPopSearchCust"));
        Element dd_BusinessType = new Element("dd_BuisnessType", By.Id("ctl00_ContentPlaceHolder1_fvRequest_ddlBusinessType"));
        Element dd_Team = new Element("dd_Team", By.Id("ctl00_ContentPlaceHolder1_fvRequest_ddlTeam"));
        Element dd_RequestSource = new Element("dd_RequestSource", By.Id("ctl00_ContentPlaceHolder1_fvRequest_ddlRequestSource"));  //has a default value
        Element txt_RequesterName = new Element("txt_RequesterName", By.Id("ctl00_ContentPlaceHolder1_fvRequest_txtRequesterName"));
        Element txt_RequesterTitle = new Element("txt_RequesterTitle", By.Id("ctl00_ContentPlaceHolder1_fvRequest_txtRequesterTitle"));
        Element txt_RequestDescription = new Element("txt_RequestDescription", By.Id("ctl00_ContentPlaceHolder1_fvRequest_txtRequestDescription"));
        Element btn_SaveJobRequest = new Element("btn_SaveJobRequest", By.Id("ctl00_ContentPlaceHolder1_fvRequest_lbtnInsertRequest1"));

        //New Job-Request Location SubTab
        Element btn_AddNewLocation = new Element("AddNewLocationButton", By.Id("ctl00_ContentPlaceHolder1_fvLocation_lbtnNewLocation"));
        Element btn_SaveLocation = new Element("SaveLocationButton", By.Id("ctl00_ContentPlaceHolder1_fvLocation_lbtnInsertLocation2"));
        //Might want to build out adding a new location

        //Job-Requests - Job Subtab
        Element btn_JobRequests_JobSubTab = new Element("JobRequest_NewJobSubTabButton", By.Id("ctl00_ContentPlaceHolder1_lnkbLocationJob"));
        Element btn_JobRequests_NewJobButton = new Element("JobRequest_NewJob_Button", By.Id("ctl00_ContentPlaceHolder1_fvJob_lbtnNewJob"));
        Element dd_JobRequests_NewJob_JobType = new Element("NewJobType_Dropdown", By.Id("ctl00_ContentPlaceHolder1_fvJob_ddlJobType"));
        Element dd_JobRequests_NewJob_JobPriority = new Element("NewJobPriority_dropdown", By.Id("ctl00_ContentPlaceHolder1_fvJob_ddlJobPriority"));
        Element txt_JobRequests_NewJob_JobPONumber = new Element("NewJob_PONUmber_textfield", By.Id("ctl00_ContentPlaceHolder1_fvJob_txtCustomerPONo"));
        Element txt_JobRequests_NewJob_Comment = new Element("NewJob_Comment_textfield", By.Id("ctl00_ContentPlaceHolder1_fvJob_txtEmconComment"));
        Element numtxt_JobRequests_NewJob_CustomerJobCap = new Element("NewJob_CustomerJobCap_numtxt", By.Id("ctl00_ContentPlaceHolder1_fvJob_txtJobCap"));
        Element btn_JobRequests_NewJob_SaveRequest = new Element("SaveNewJobRequest_button", By.Id("ctl00_ContentPlaceHolder1_fvJob_lbtnInsertJob2"));

        //Job-Requests - Workscope Subtab --Multiple WorkScopes can be added
        Element dd_JobRequests_WorkScope_Trade = new Element("JQ_WorkScope_Trade_DropDown", By.Id("ctl00_ContentPlaceHolder1_gvwWorkScopeTrade_ctl03_ddlTradeNew"));
        Element dd_JobRequests_WorkScope_SubTrade = new Element("JQ_WorkScope_SubTrade_DropDown", By.Id("ctl00_ContentPlaceHolder1_gvwWorkScopeTrade_ctl03_ddlSubTradeNew"));
        Element txt_JobRequests_WorkScope_WSDescription = new Element("JQ_WorkScope_Description_textBox", By.Id("ctl00_ContentPlaceHolder1_gvwWorkScopeTrade_ctl03_txtWorkScopeOptNew"));
        Element btn_JobRequests_WorkScope_Save = new Element("JQ_WorkScope_SaveButton", By.Id("ctl00_ContentPlaceHolder1_gvwWorkScopeTrade_ctl03_ibtnAddBus"));

        #endregion

        #region Finished Test Functions
        public FMX_Jobs_JobRequests CustomerSearch(string customerName)
        {
            //This search uses the specific customer search pop up
            pleaseWait.WaitUntilNotVisible();
            btn_JobRequests_CustomerSearch.VerifyVisible();
            btn_JobRequests_CustomerSearch.Click();
            pleaseWait.WaitUntilNotVisible();
            pop_CustomerSearch.VerifyVisible(5);
            txt_CustomerName_CustomerSearch_pop.VerifyVisible(5);
            txt_CustomerName_CustomerSearch_pop.Text = customerName;
            btn_CustomerSearch.VerifyVisible();
            btn_CustomerSearch.Click();
            pleaseWait.WaitUntilNotVisible();
            table_CustomerSearchResults_grid.Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobRequests DynamicSearch(string customerName)
        {   
            //This Search uses the dynamic search popup for a customer
            pop_DynamicSearch.VerifyVisible(5);
            txt_CustomerName_DynamicSearch_Pop.VerifyVisible(5);
            txt_CustomerName_DynamicSearch_Pop.Text = customerName;
            btn_SearchButton.Click();
            table_SearchResultsTable.VerifyVisible(5);
            //CustomerName = table_SearchResultsTable.Text;  //May need someway of tracking customer name stuff
            table_SearchResultsTable.Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobRequests EnterJobRequestInfo(string businessType, string businessTeam, string requestSource,
                                            string requesterName, string requesterTitle, string requestDescription)
        {
            pleaseWait.WaitUntilNotVisible();
            dd_BusinessType.FindElement(ByE.Text(businessType)).Click();
            pleaseWait.WaitUntilNotVisible();
            //there is a check here to see if team has available business types --this test assumes you know which team's go with which business types
            dd_Team.FindElement(ByE.Text(businessTeam)).Click();
            pleaseWait.WaitUntilNotVisible();
            dd_RequestSource.FindElement(ByE.Text(requestSource)).Click();
            txt_RequesterName.VerifyVisible(5);
            txt_RequesterName.Text = requesterName;
            txt_RequesterTitle.Text = requesterTitle;
            txt_RequestDescription.Text = requestDescription;
            btn_SaveJobRequest.Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobRequests AddNewLocation()
        {
            pleaseWait.WaitUntilNotVisible();
            btn_AddNewLocation.WaitUntilVisible().Click(); //Default Data is selected for EMCON TEST customer
            pleaseWait.WaitUntilNotVisible();
            btn_SaveLocation.Click();
            pleaseWait.WaitUntilNotPresent();
            btn_JobRequests_JobSubTab.WaitUntilVisible().Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobRequests AddNewJob(string jobType, string jobPriority, string POnumber, string JobComment, int jobCap = 9999)
        {
            btn_JobRequests_NewJobButton.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotPresent();
            dd_JobRequests_NewJob_JobType.WaitUntilVisible();
            dd_JobRequests_NewJob_JobType.FindElement(ByE.Text(jobType)).Click();
            pleaseWait.WaitUntilNotPresent();
            dd_JobRequests_NewJob_JobPriority.WaitUntilVisible();
            dd_JobRequests_NewJob_JobPriority.FindElement(ByE.Text(jobPriority)).Click();
            pleaseWait.WaitUntilNotVisible();
            txt_JobRequests_NewJob_JobPONumber.Text = POnumber;
            txt_JobRequests_NewJob_Comment.Text = JobComment;
            numtxt_JobRequests_NewJob_CustomerJobCap.Text = jobCap.ToString();
            btn_JobRequests_NewJob_SaveRequest.Click();
            pleaseWait.WaitUntilNotVisible();
            return new FMX_Jobs_JobRequests();
        }
        
        public FMX_Jobs_JobRequests SearchCustomer(string customerName)
        {
            pop_DynamicSearch.VerifyVisible(5);
            txt_CustomerName_DynamicSearch_Pop.VerifyVisible(5);
            txt_CustomerName_DynamicSearch_Pop.Text = customerName;
            btn_SearchButton.Click();
            //CustomerName = table_SearchResultsTable.Text;  //May need someway of tracking customer name stuff
            table_SearchResultsTable.WaitUntilPresent().Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobRequests CloseSearchPopUp()
        {
            pleaseWait.WaitUntilNotVisible();
            btn_PopupClose.Click();
            return new FMX_Jobs_JobRequests();
        }

        public FMX_Jobs_JobRequests SelectCompletedJob()
        {
            driver.WaitForPresent(ByE.PartialText("Job #")).Click();
            return new FMX_Jobs_JobRequests();
        }

  
        public FMX_Jobs_JobRequests VerifyRequestPresent(string customerName)
        {
            pleaseWait.WaitUntilNotVisible();
            table_JobRequests_CustomerName.FindElements(By.LinkText(customerName));
            return new FMX_Jobs_JobRequests();
        }
        #endregion

        public FMX_Jobs_JobRequests AddWorkScopes(string trade, string subTrade, string description)
        {
            pleaseWait.WaitUntilNotVisible();
            dd_JobRequests_WorkScope_Trade.VerifyVisible();
            dd_JobRequests_WorkScope_Trade.FindElement(ByE.Text(trade)).Click();
            pleaseWait.WaitUntilNotVisible();
            dd_JobRequests_WorkScope_SubTrade.VerifyVisible();
            dd_JobRequests_WorkScope_SubTrade.FindElement(ByE.Text(subTrade)).Click(); //Sub-Trade is dependant on options selected from Trade
            txt_JobRequests_WorkScope_WSDescription.Text += description;
            
            return new FMX_Jobs_JobRequests();
        }


        public FMX_Jobs_JobRequests SearchJobStatus(string statusText)
        {
            chk_JobStatus.Click();
            drp_JobStatus.Click();
            drp_JobStatus.FindElement(By.Id("ddcl-ctl00_ContentPlaceHolder1_ucDynamicSearch_rpSearchForm_ctl04_lbListBox-i4")).Click();
            btn_SearchButton.Click();
            table_SearchResultsTable.WaitUntilPresent().Click();
            return new FMX_Jobs_JobRequests();
        }
    }
}
