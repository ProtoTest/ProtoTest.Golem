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

        //Element EditBid_Calendar = new Element("EditBid_Calendar",By.XPath("//*[@id='ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid']/tbody/tr/td/div/table/tbody/tr[2]/td[2]/img"));
        Element EditBid_Calendar = new Element("EditBid_Calendar", By.XPath("//*[@id='ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid']/tbody/tr/td/div/table/tbody/tr[2]/td[2]/img"));
        Element EditBid_Cal_NextMonth = new Element("EditBid_Cal_NextMonth", By.ClassName("ui-datepicker-next"));
        Element EditBid_Cal_Today = new Element("EditBid_firstDay", By.ClassName("ui-state-highlight"));
        Element SaveEdit = new Element("saveedit", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_fvBid_lnkSave"));

        Element EditPricing = new Element("EditPricing", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_lbtnEditPricing"));
        Element UnitValue = new Element("UnitValue",By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_gvPricing_ctl02_txtBuyQuantity"));
        Element UnitPrice = new Element("UnitPrice", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_gvPricing_ctl02_txtBuyUnitCost"));
        Element SaveEditPricing = new Element("SaveEditPrice", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabPricingBidRequests_ucJobBid1_lbtnSavePricing"));

        Element ProposalsTab = new Element("ProposalsTab", By.Id("__tab_ctl00_ContentPlaceHolder1_JobTabs_TabProposals"));
        Element WorkOrdersTab = new Element("WorkOrdersTab", By.Id("__tab_ctl00_ContentPlaceHolder1_JobTabs_TabWO"));

        public FMX_Jobs_JobRequest_BidRequests CreateNewBidRequest(string numUnits, string numPrice)
        {
            NewBidRequest.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            bidRequestPop.WaitUntil.Visible();
            //select bid duedate
            CalendarButton.WaitUntil.Visible().Click();
            NextMonth.WaitUntil.Visible().Click();
            firstDay.WaitUntil.Visible().Click();
            GenerateBid.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            bidRequestPop.WaitUntil.NotVisible();

            EditPricing.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            UnitValue.Text = numUnits;
            UnitPrice.Text = numPrice;
            SaveEditPricing.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();

            //Send the bid
            ViewprintSend.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            selectBidDocs.WaitUntil.Visible();
            SendPrintButton.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            SendEmail.WaitUntil.Visible().Click();
            //SendUSPS.WaitUntil.Visible().Click();
            SendDocuments.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            selectBidDocs.WaitUntil.NotVisible();

            EditBidRequests.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            EditBid_Calendar.WaitUntil.Visible();
            int y = EditBid_Calendar.Location.Y;
            driver.JavaWindowScroll(0, y-HeaderBarHeight);
            EditBid_Calendar.WaitUntil.Visible().Click();
            //EditBid_Cal_NextMonth.WaitUntil.Visible().Click();
            EditBid_Cal_Today.WaitUntil.Visible().Click();
            SaveEdit.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();


            return new FMX_Jobs_JobRequest_BidRequests();
        }

        public FMX_Jobs_JobRequest_Proposals ClickProposalsTab()
        {
            ProposalsTab.WaitUntil.Visible();
            int y = ProposalsTab.Location.Y;
            driver.JavaWindowScroll(0, y-HeaderBarHeight);
            ProposalsTab.Click();
            pleaseWait.WaitUntil.NotVisible();
            return new FMX_Jobs_JobRequest_Proposals();
        }

        //public FMX_Jobs_JobRequest_WorkOrders ClickWorkOrdersTab()
        //{
        //    WorkOrdersTab.WaitUntil.Visible().Click();
        //    pleaseWait.WaitUntil.NotVisible();
        //    return new FMX_Jobs_JobRequest_WorkOrders();
        //}



    }
}
