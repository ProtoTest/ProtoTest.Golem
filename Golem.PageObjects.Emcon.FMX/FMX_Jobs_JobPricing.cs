using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_Jobs_JobPricing : FmxMenuBar
    {
        public Element btn_Search = new Element("SearchButton", By.Name("ctl00$ContentPlaceHolder1$ucPricerPage$btnSearchJobs"));
        public Element btn_ClearSearch = new Element("ClearSearchButton", By.Name("ctl00$ContentPlaceHolder1$ucPricerPage$btnClear"));
        public Element tbl_SearchResults = new Element("SearchResultsTable", By.Id("ctl00_ContentPlaceHolder1_ucPricerPage_gvRequests_gvGrid"));

        public Element drp_APStatus = new Element("APStatusdropdown", By.Id("ddcl-ctl00_ContentPlaceHolder1_ucPricerPage_lbAPStatus"));
        public Element drp_ARStatus = new Element("ARStatusdropdown", By.Id("ddcl-ctl00_ContentPlaceHolder1_ucPricerPage_lbARStatus"));
        public Element drpOpt_APStatus_NoPricing = new Element("APStatus_NoPricing_Option", By.Id("ddcl-ctl00_ContentPlaceHolder1_ucPricerPage_lbAPStatus-i1"));
        public Element drpOpt_ArStatus_NoPricing = new Element("ARStatus_NoPricing_Option", By.Id("ddcl-ctl00_ContentPlaceHolder1_ucPricerPage_lbARStatus-i1"));

        public Element lnk_Vendor_Name = new Element("VendorLink", By.ClassName("RequestTreeInfoLink"));

        public FMX_Jobs_JobPricing Select_AR_AP_NoPricing()
        {
            drp_APStatus.Click();
            drpOpt_APStatus_NoPricing.Click();
            drp_ARStatus.Click();
            drpOpt_ArStatus_NoPricing.Click();
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_JobPricing ClickSearchButton()
        {
            btn_Search.Click();
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_JobPricing SelectJobByIndex(int index)
        {
            driver.WaitForVisible(By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucPricerPage_gvRequests_gvGrid']/tbody/tr[" + (index + 2) + "]/td")).Click();
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_JobPricing ClickVendorLink()
        {
            lnk_Vendor_Name.Click();
            return new FMX_Jobs_JobPricing();
        }

        public override void WaitForElements()
        {
            base.WaitForElements();
        }
    }
}
