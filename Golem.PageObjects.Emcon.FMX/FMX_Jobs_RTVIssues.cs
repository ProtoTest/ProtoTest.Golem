using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_Jobs_RTVIssues : FmxMenuBar
    {
        public Element btn_CreateRTVIssue = new Element("CreateRTVIssueButton", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_lbtnNew"));
        public Element chk_InvoiceReceived = new Element("InvoiceReceivedCheckbox", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_chkInvoiceReceived"));
        public Element txt_InvoiceUpload_Master = new Element("InvoiceUploadMasterTextField", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_fuInvoice"));
        public Element txt_SignoffUpload_MarkedUp = new Element("InvoiceUploadMasterTextField", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_fuSignoffMU"));
        public Element chk_SignoffReceived = new Element("SignoffReceivedCheckbox", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_chkSignoffReceivedFromStore"));
        public Element chk_PaperworkApproved = new Element("PaperworkApprovedCheckbox", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_chkApproved"));
        public Element btn_Save = new Element("SaveButton", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_lnkSave"));
        public Element btn_Cancel = new Element("SaveButton", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_lnkCancel"));
        public Element tbl_Results = new Element("ResultsTable", By.Id("ctl00_ContentPlaceHolder1_ucRTV_gvWorkOrderHistory_gvGrid"));

        public FMX_Jobs_RTVIssues SelectWorkOrder()
        {
            driver.FindVisibleElement(By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucRTV_gvWorkOrderHistory_gvGrid']//tr[2]")).Click();
            return new FMX_Jobs_RTVIssues();
        }

        public FMX_Jobs_RTVIssues ClickNewRTVIssueButton()
        {
            btn_CreateRTVIssue.Click();
            return new FMX_Jobs_RTVIssues();
        }

        public FMX_Jobs_RTVIssues SubmitIssueInfo(bool invoiceReceived, string pathToMasterForInvoice, bool signoffReceived, string pathToMarkedUpForSignoff, bool paperworkApproved)
        {
            chk_InvoiceReceived.SetCheckbox(invoiceReceived);
            txt_InvoiceUpload_Master.SendKeys(pathToMasterForInvoice);
            chk_SignoffReceived.SetCheckbox(signoffReceived);
            txt_SignoffUpload_MarkedUp.SendKeys(pathToMarkedUpForSignoff);
            chk_PaperworkApproved.SetCheckbox(paperworkApproved);
            btn_Save.Click();
            return new FMX_Jobs_RTVIssues();
        }


    }
}
