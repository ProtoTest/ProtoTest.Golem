using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace GolemPageObjects.Emcon.FMNow
{
    class FMNow_Search : FMNow_HomePage
    {
        Element SimpleSearchTab = new Element("SimpleSearchTab", By.Id("__tab_ContentPlaceHolder1_tabsSearch_tabSimple"));
        Element AdvancedSearch = new Element("AdvancedSearchTab", By.Id("__tab_ContentPlaceHolder1_tabsSearch_tabAdvanced"));

        //simple search
        Element dd_Findrequest_Simple = new Element("FindRequestsBy_Simple",By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_ddlSearchParameter"));
        Element txt_SearchTerms_Simple = new Element("SearchTerms_Simple", By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_txtSearchValue"));
        Element btn_InitSearch_Simple = new Element("InitSearch_Simple", By.Id("ContentPlaceHolder1_tabsSearch_tabSimple_UcSimpleSearchForWorkOrders1_cmdSearch"));

        //advanced Serach
        Element dd_JobType = new Element("Jobtype", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlJobType"));
        Element btn_SearchLocations = new Element("SearchLocations", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_cmdLocationSearch"));
        Element txt_LocationNumber = new Element("LocationNum", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtStoreNumber"));
        Element txt_ClientRefNum = new Element("ClientRefNum", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtPONumber"));
        Element dd_JobStatus = new Element("jobStatus", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlJobStatus"));
        Element dd_CallPriority = new Element("CallPriority", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlCallPriority"));
        Element dd_TypeofWork = new Element("TypeofWork", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlJobTrade"));
        Element dd_SubTypeofWOrk = new Element("SubTypeofWOrk", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlSubTypeOfWork"));
        Element txt_ProposalApprovedBy = new Element("ProprosalApprovedBy", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtApprovedBy"));
        Element txt_OpenedBy = new Element("OpenedBy", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtOpenedBy"));
        Element txt_City = new Element("City", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtCity"));
        Element dd_State = new Element("State", By.ClassName("ui-dropdownchecklist")); //I know :(
        Element txt_zipCode = new Element("ZipCode", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtZipCode"));
        Element txt_Radius = new Element("Radius", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtRadius"));
        Element dd_Country = new Element("Country", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_ddlCountryCode"));
        Element txt_Region = new Element("Region", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtRegion"));
        Element cal_StartDate = new Element("StartDate", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtStartDate"));//hopefully no weird formatting issues
        Element cal_EndDate = new Element("EndDate", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_txtEndDate"));
        Element btn_InitSearch_adv = new Element("InitSearch_Adv", By.Id("ContentPlaceHolder1_tabsSearch_tabAdvanced_UcSearchForWorkOrders1_cmdSearchWorkOrders"));


    }
}
