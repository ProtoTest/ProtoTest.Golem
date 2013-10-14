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
    public class FMX_Jobs_JobRequest_Proposals : FMX_Jobs_JobRequests
    {
        Element NewProposal = new Element("NewProposal", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_fvProposal_lbtnNewRequest2"));
        Element SaveProposal = new Element("SaveProposal", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_fvProposal_lbtnUpdateRequest1"));
        Element EditProposal = new Element("EditProposal", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_lbtnEditProposal"));
        Element SelectWorkScope = new Element("SelectWorkScope", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_dlOptionItems_ctl00_chkSelected"));
        Element AddWorkScope = new Element("AddWorkScope", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_lbtnAddToOption"));
        Element SaveEditProposal = new Element("SaveEditProposal", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_lbtnSave"));
        Element SendSavePrintProposal = new Element("SendSavePrintProposal", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_fvProposal_lbtnSend2"));
        Element SendSavePrintPopup = new Element("SendSavePrintPop", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_ucDocumentSend_pnlPopupDocumentSend"));
        Element EmailAddress = new Element("EmailAddress", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_ucDocumentSend_txtNewEmail"));
        Element SendEmail = new Element("SendEmail", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_ucDocumentSend_clstNewEmail_0"));
        Element SendUSPS = new Element("SendUSPS", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_ucDocumentSend_chkPrint"));
        Element SendDocument = new Element("SendDocument", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_ucDocumentSend_btnSendDocument"));
        
        Element SetCustomerSelections = new Element("SetCustomerSelctions", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_lbtnSelectOptions"));
        Element WorkScopeCustomerSelection = new Element("WorkScopeCustomerSelection", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_dlSelections_ctl00_chkOption"));
        Element WorkScropCustomerSelectionSave = new Element("WOrkScropSaveButton", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_lbtnSaveSelections"));
        Element EditProposalAgain = new Element("EditProposalTwoButton", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_fvProposal_lbtnEditRequest2"));

        Element ApprovedBy = new Element("ApprovedBy", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_fvProposal_txtApprovedBy"));
        Element ApprovedByCalendar = new Element("ApprovedByCalendar", By.XPath("//*[@id='ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_fvProposal']/tbody/tr/td/table/tbody/tr[8]/td/img"));
        Element ApprovedByCalendarNextMonth = new Element("ApprovedNextMonth", By.XPath("//*[@id='ui-datepicker-div']/div/a[2]/span"));
        Element ApprovedbyCalendarFirstDay = new Element("FirstDayWeek2", By.XPath("//*[@id='ui-datepicker-div']/table/tbody/tr[2]/td[2]/a"));

        Element EditProposalSave = new Element("EditProposalSave", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_fvProposal_lbtnUpdateRequest2"));
        Element JobCapButton = new Element("JobCapPopUp", By.Id("ctl00_ContentPlaceHolder1_JobTabs_TabProposals_ucProposalEditor_btnOkJobCapUpdate"));

        Element WorkOrderTab = new Element("WorkOrdersTab", By.XPath("//*[@id='ctl00_ContentPlaceHolder1_upnlLowerTabs']/div/div/span[4]/span/span/span"));

        public FMX_Jobs_JobRequest_Proposals CreateNewProposal(string emailToSend, string approvedByName)
        {
            NewProposal.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            SaveProposal.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            EditProposal.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            SelectWorkScope.WaitUntil.Visible().Click();
            AddWorkScope.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            SaveEditProposal.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            SendSavePrintProposal.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            SendSavePrintPopup.WaitUntil.Visible();
            EmailAddress.WaitUntil.Visible().Text = emailToSend;
            SendEmail.WaitUntil.Visible().Click();
            SendUSPS.WaitUntil.Visible().Click();
            SendDocument.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            SendSavePrintPopup.WaitUntil.NotVisible();
            SetCustomerSelections.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            WorkScopeCustomerSelection.WaitUntil.Visible().Click();
            WorkScropCustomerSelectionSave.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            EditProposalAgain.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            ApprovedBy.WaitUntil.Visible().Text = approvedByName;
            ApprovedByCalendar.WaitUntil.Visible();
            int y = ApprovedByCalendar.Location.Y;
            driver.JavaWindowScroll(0, y-HeaderBarHeight);
            ApprovedByCalendar.Click();
            ApprovedByCalendarNextMonth.WaitUntil.Visible().Click();
            ApprovedbyCalendarFirstDay.WaitUntil.Visible().Click();
            EditProposalSave.WaitUntil.Visible().Click();
            pleaseWait.WaitUntil.NotVisible();
            JobCapButton.WaitUntil.Present().Click();
            pleaseWait.WaitUntil.NotVisible();
            return new FMX_Jobs_JobRequest_Proposals();
        }

        public FMX_Jobs_JobRequest_WorkOrders ClickWorkOrdersTab()
        {
            WorkOrderTab.WaitUntil.Visible();
            int y = WorkOrderTab.Location.Y;
            driver.JavaWindowScroll(0, y-HeaderBarHeight);
            WorkOrderTab.Click();
            pleaseWait.WaitUntil.NotVisible();
            return new FMX_Jobs_JobRequest_WorkOrders();
        }

    }
}
