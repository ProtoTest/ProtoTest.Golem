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
    public class FMX_Jobs_JobRequest_BidRequests : FMX_Jobs_JobRequests
    {
        Element NewBidRequest = new Element("NewBidRequests", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid_lbtnNew"));
        Element bidRequestPop = new Element("BidRequestPopUp", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_pnlPopupGenerateBid"));
        Element CalendarButton = new Element("BidDueDate_CalendarButton", By.XPath("//*[@id='ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_pnlPopupGenerateBidContent']/div/img"));
        Element NextMonth = new Element("NextMonthButton", By.XPath("//*[@id='ui-datepicker-div']/div/a[2]/span"));
        Element firstDay = new Element("FirstDay", By.XPath("//*[@id='ui-datepicker-div']/table/tbody/tr[2]/td[2]/a"));
        Element GenerateBid = new Element("GenerateBid", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_btnGenerate"));
        Element ViewprintSend = new Element("Viewprintsent",By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid_lbtnSend"));
        Element selectBidDocs = new Element("SelectBidDocsPopup",By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_pnlPopUpDocumentPicker"));
        Element SendPrintButton = new Element("SendPrintButton",By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_lbtnSend"));
        Element SendEmail = new Element("SendEmail", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_ucDocumentSend_rptEmail_ctl00_clstEmail_0"));
        Element SendUSPS = new Element("SendUSPS",By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_ucDocumentSend_chkPrint"));
        Element SendDocuments = new Element("SendDocuments", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_ucDocumentSend_btnSendDocument"));
        Element EditBidRequests = new Element("EditBidRequests", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid_lbtnEdit"));

        Element EditBid_Calendar = new Element("EditBid_Calendar",By.XPath("//*[@id='ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid']/tbody/tr/td/div/table/tbody/tr[2]/td[2]/img"));
        Element EditBid_Cal_NextMonth = new Element("EditBid_Cal_NextMonth", By.XPath("//*[@id='ui-datepicker-div']/div/a[2]/span"));
        Element EditBid_Cal_FirstDay = new Element("EditBid_firstDay", By.XPath("//*[@id='ui-datepicker-div']/table/tbody/tr[2]/td[2]/a"));
        Element SaveEdit = new Element("saveedit", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid_lnkSave"));

        Element EditPricing = new Element("EditPricing", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_lbtnEditPricing"));
        Element UnitValue = new Element("UnitValue",By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_gvPricing_ctl02_txtBuyQuantity"));
        Element UnitPrice = new Element("UnitPrice", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_gvPricing_ctl02_txtBuyUnitCost"));
        Element SaveEditPricing = new Element("SaveEditPrice", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_lbtnSavePricing"));

        Element ProposalsTab = new Element("ProposalsTab", By.Id("__tab_ctl00_ContentPlaceHolder1_JobTabs_TabProposals"));

        public FMX_Jobs_JobRequest_BidRequests CreateNewBidRequest(string numUnits, string numPrice)
        {
            NewBidRequest.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            bidRequestPop.WaitUntilVisible();
            CalendarButton.WaitUntilVisible().Click();
            NextMonth.WaitUntilVisible().Click();
            firstDay.WaitUntilVisible().Click();
            GenerateBid.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            bidRequestPop.WaitUntilNotVisible();
            ViewprintSend.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            selectBidDocs.WaitUntilVisible();
            SendPrintButton.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            SendEmail.WaitUntilVisible().Click();
            SendUSPS.WaitUntilVisible().Click();
            SendDocuments.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            selectBidDocs.WaitUntilNotVisible();
            EditBidRequests.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            EditBid_Calendar.WaitUntilVisible().Click();
            EditBid_Cal_NextMonth.WaitUntilVisible().Click();
            EditBid_Cal_FirstDay.WaitUntilVisible().Click();
            SaveEdit.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            EditPricing.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            UnitValue.Text = numUnits;
            UnitPrice.Text = numPrice;
            SaveEditPricing.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            return new FMX_Jobs_JobRequest_BidRequests();
        }

        public FMX_Jobs_JobRequest_Proposals ClickProposalTab()
        {
            ProposalsTab.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            return new FMX_Jobs_JobRequest_Proposals();
        }



    }
}
