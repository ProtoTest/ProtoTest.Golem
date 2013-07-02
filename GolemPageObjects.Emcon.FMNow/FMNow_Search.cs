using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace GolemPageObjects.Emcon.FMNow
{
    public class FMNow_Search : FMNow_HomePage
    {
        Element SimpleSearchTab = new Element("SimpleSearchTab", By.Id("__tab_ContentPlaceHolder1_tabsSearch_tabSimple"));
        Element AdvancedSearch = new Element("AdvancedSearchTab", By.Id("__tab_ContentPlaceHolder1_tabsSearch_tabAdvanced"));

        //simple search
        Element dd_Findrequest_Simple = new Element("FindRequestsBy_Simple",By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_ddlSearchParameter"));
        Element txt_SearchTerms_Simple = new Element("SearchTerms_Simple", By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_txtSearchValue"));
        Element btn_InitSearch_Simple = new Element("InitSearch_Simple", By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_cmdSearch"));
        Element dd_SearchTermsOne_Simple = new Element("SearchTermsDDOne_Simple", By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_ddlSearchValue"));
        Element dd_SearchTermsTwo_Simple = new Element("SearchTermsDDTwo_Simple", By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_ddlSearchValue2"));
        Element dd_SearchState_Simple = new Element("SearchTermsState", By.ClassName("ui-dropdownchecklist"));

        private static List<string> ValuesWithSecondDD;
            
        //advanced Serach
        Element AdvancedSearchTab = new Element("AdvancedSearchTab", By.Id("__tab_ContentPlaceHolder1_tabsSearch_tabAdvanced"));
        Element dd_JobType = new Element("Jobtype", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlJobType"));
        Element btn_SearchLocations = new Element("SearchLocations", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_cmdLocationSearch"));
        Element txt_LocationNumber = new Element("LocationNum", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtStoreNumber"));
        Element txt_ClientRefNum = new Element("ClientRefNum", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtPONumber"));
        Element dd_JobStatus = new Element("jobStatus", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlJobStatus"));
        Element dd_CallPriority = new Element("CallPriority", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlCallPriority"));
        Element dd_TypeofWork = new Element("TypeofWork", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlJobTrade"));
        Element dd_SubTypeofWOrk = new Element("SubTypeofWOrk", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlSubTypeOfWork"));
        //Element txt_ProposalApprovedBy = new Element("ProprosalApprovedBy", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtApprovedBy"));
        //Element txt_OpenedBy = new Element("OpenedBy", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtOpenedBy"));
        //Element txt_City = new Element("City", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtCity"));
        //Element dd_State = new Element("State", By.ClassName("ui-dropdownchecklist")); //I know :(
        //Element txt_zipCode = new Element("ZipCode", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtZipCode"));
        //Element txt_Radius = new Element("Radius", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtRadius"));
        //Element dd_Country = new Element("Country", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlCountryCode"));
        //Element txt_Region = new Element("Region", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtRegion"));
        //Element cal_StartDate = new Element("StartDate", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtStartDate"));//hopefully no weird formatting issues
        //Element cal_EndDate = new Element("EndDate", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtEndDate"));
        Element btn_InitSearch_adv = new Element("InitSearch_Adv", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_cmdSearchWorkOrders"));

        public void InitValuesWithSecondDD()
        {
            ValuesWithSecondDD = new List<string>();
            ValuesWithSecondDD.Add("Job Status");
            ValuesWithSecondDD.Add("Job Type");
            ValuesWithSecondDD.Add("Call Priority");
            ValuesWithSecondDD.Add("Country");
        }

        public FMNow_Search doSimpleSearch(string option, string searchTerms, string secondSearchTerm = "[Not Sure]")
        {
            InitValuesWithSecondDD();
            if (ValuesWithSecondDD.Contains(option))
            {
                dd_Findrequest_Simple.WaitUntilVisible().SelectOption(option);
                pleaseWait.WaitUntilNotVisible();
                dd_SearchTermsOne_Simple.WaitUntilVisible().SelectOption(searchTerms);
            }
            if (option == "State")
            {
                dd_Findrequest_Simple.WaitUntilVisible().SelectOption(option);
                pleaseWait.WaitUntilNotVisible();
                dd_SearchState_Simple.WaitUntilVisible().FindElement(ByE.Text(searchTerms)).Click();
            }
            if (option == "Type of Work")
            {
                dd_Findrequest_Simple.WaitUntilVisible().SelectOption(option);
                pleaseWait.WaitUntilNotVisible();
                dd_SearchTermsOne_Simple.WaitUntilVisible().SelectOption(searchTerms);
                pleaseWait.WaitUntilNotVisible();
                dd_SearchTermsTwo_Simple.WaitUntilVisible().SelectOption(secondSearchTerm);
            }
            else
            {
                dd_Findrequest_Simple.WaitUntilVisible().SelectOption(option);
                txt_SearchTerms_Simple.WaitUntilVisible().Text = searchTerms;
            }
            pleaseWait.WaitUntilNotVisible();
            btn_InitSearch_Simple.WaitUntilVisible().Click();
            return new FMNow_Search();
        }

        public FMNow_Search doAdvancedSearch(string jobType, string Location, string jobStatus, string callpriority, string typeofWOrk, string subtypeofwork)
        {
            AdvancedSearchTab.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            dd_JobType.WaitUntilVisible().SelectOption(jobType);
            txt_LocationNumber.WaitUntilVisible().Text = Location;
            //txt_ClientRefNum.WaitUntilVisible().Text = ClientRefNumber;
            dd_JobStatus.WaitUntilVisible().SelectOption(jobStatus);
            dd_CallPriority.WaitUntilVisible().SelectOption(callpriority);
            dd_TypeofWork.WaitUntilVisible().SelectOption(typeofWOrk);
            dd_SubTypeofWOrk.WaitUntilVisible().SelectOption(subtypeofwork);
            btn_InitSearch_adv.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();

            return new FMNow_Search();
        }



    }
}
