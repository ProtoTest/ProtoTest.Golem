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

        public FMX_Jobs_JobRequest_Proposals CreateNewProposal(string emailToSend)
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

            return new FMX_Jobs_JobRequest_Proposals();
        }

    }
}
