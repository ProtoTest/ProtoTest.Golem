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
            NewProposal.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            SaveProposal.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            EditProposal.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            SelectWorkScope.WaitUntilVisible().Click();
            AddWorkScope.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            SaveEditProposal.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            SendSavePrintProposal.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            SendSavePrintPopup.WaitUntilVisible();
            EmailAddress.WaitUntilVisible().Text = emailToSend;
            SendEmail.WaitUntilVisible().Click();
            SendUSPS.WaitUntilVisible().Click();
            SendDocument.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            SendSavePrintPopup.WaitUntilNotVisible();
            SetCustomerSelections.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            WorkScopeCustomerSelection.WaitUntilVisible().Click();
            WorkScropCustomerSelectionSave.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            EditProposalAgain.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            ApprovedBy.WaitUntilVisible().Text = approvedByName;
            ApprovedByCalendar.WaitUntilVisible();
            int y = ApprovedByCalendar.Location.Y;
            driver.JavaWindowScroll(0, y-HeaderBarHeight);
            ApprovedByCalendar.Click();
            ApprovedByCalendarNextMonth.WaitUntilVisible().Click();
            ApprovedbyCalendarFirstDay.WaitUntilVisible().Click();
            EditProposalSave.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            JobCapButton.WaitUntilPresent().Click();
            pleaseWait.WaitUntilNotVisible();
            return new FMX_Jobs_JobRequest_Proposals();
        }

        public FMX_Jobs_JobRequest_WorkOrders ClickWorkOrdersTab()
        {
            WorkOrderTab.WaitUntilVisible();
            int y = WorkOrderTab.Location.Y;
            driver.JavaWindowScroll(0, y-HeaderBarHeight);
            WorkOrderTab.Click();
            pleaseWait.WaitUntilNotVisible();
            return new FMX_Jobs_JobRequest_WorkOrders();
        }

    }
}
