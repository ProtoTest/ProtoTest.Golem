using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Golem.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_Jobs_JobRequest_Vendors : FMX_Jobs_JobRequests
    {
        Element Trade = new Element("TradeSubTradeDropDown", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabVendors_ucVendortradeJobs_ddlVendorTrade"));
        Element VendorSearch = new Element("VendorSearch", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabVendors_ucVendortradeJobs_lbPopRadiuSearch"));
        Element PopUpVendorSearch = new Element("VendorSearchPopup", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabVendors_ucVendortradeJobs_pnlPopUpVendorSearch"));
        Element FirstVendor = new Element("FirstVendor", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabVendors_ucVendortradeJobs_ucRadiusSearch_gucVendorRadiusSearch_ctl03_chkSelectVendor"));
        Element AcceptVendor = new Element("AcceptVendorButton", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabVendors_ucVendortradeJobs_ucRadiusSearch_btnSelectVendor2"));

        Element BidRequestsSubTab = new Element("BidRequestsSubTab",By.Id("__tab_ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests"));

        public FMX_Jobs_JobRequest_Vendors AddVendor(string vendorType)
        {
            Trade.WaitUntilVisible().FindElement(ByE.PartialText(vendorType));
            pleaseWait.WaitUntilNotVisible();
            VendorSearch.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            PopUpVendorSearch.WaitUntilVisible();
            FirstVendor.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            //Common.Delay(1000);
            AcceptVendor.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            PopUpVendorSearch.WaitUntilNotVisible();
            return new FMX_Jobs_JobRequest_Vendors();
        }

        public FMX_Jobs_JobRequest_BidRequests ClickBidRequests()
        {

            BidRequestsSubTab.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            return new FMX_Jobs_JobRequest_BidRequests();
        }








    }
}
