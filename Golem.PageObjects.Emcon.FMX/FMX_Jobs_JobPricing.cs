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

        public Element lnk_Vendor_Name = new Element("VendorLink", By.Id("ctl00_ContentPlaceHolder1_ucPricerPage_tvVendort2"));
        public Element txt_InvoiceNumber = new Element("InvoiceNumberTextField", By.Id("ctl00_ContentPlaceHolder1_ucPricerPage_dlAP_ctl00_txtVendorInv"));
        public Element txt_subtotalCrossCheck = new Element("SubtotalCrossCheckTextField", By.Id("ctl00_ContentPlaceHolder1_ucPricerPage_dlAP_ctl00_txtCrossCheck"));
        public Element drp_setVendorPricing = new Element("SetVendorPricingButton", By.Id("ctl00_ContentPlaceHolder1_ucPricerPage_dlAP_ctl00_ddlVendorStatus"));
        public Element btn_SetAccountsReceived = new Element("AccountsReceivedButton", By.Id("ctl00_ContentPlaceHolder1_ucPricerPage_dlAR_ctl00_lbtnSetAR"));

        public FMX_Jobs_JobPricing Select_AR_AP_NoPricing()
        {
            drp_APStatus.WaitUntil.Visible().Click();
            drpOpt_APStatus_NoPricing.WaitUntil.Visible().Click();
            drp_ARStatus.WaitUntil.Visible().Click();
            drpOpt_ArStatus_NoPricing.WaitUntil.Visible().Click();
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_JobPricing ClickSearchButton()
        {
            btn_Search.WaitUntil.Visible().Click();
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_JobPricing SelectJobByIndex(int index)
        {
            driver.WaitForVisible(By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucPricerPage_gvRequests_gvGrid']/tbody/tr[" + (index + 2) + "]/td")).Click();
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_JobPricing ClickVendorLink()
        {
            lnk_Vendor_Name.WaitUntil.Visible().Click();
            return new FMX_Jobs_JobPricing();
        }

        public override void WaitForElements()
        {
            base.WaitForElements();
        }

        public FMX_Jobs_JobPricing SubmitPricingInfo(string invoiceNumber, string subtotalCrossCheck)
        {
            txt_InvoiceNumber.Text = invoiceNumber;
            txt_subtotalCrossCheck.Text = subtotalCrossCheck;
            drp_setVendorPricing.SelectOption("Pay Vendor");
            return new FMX_Jobs_JobPricing();
        }

        public FMX_Jobs_JobPricing ClickSetAccountsReceivable()
        {
            btn_SetAccountsReceived.WaitUntil.Visible().Click();
            return  new FMX_Jobs_JobPricing();
        }
    }
}
