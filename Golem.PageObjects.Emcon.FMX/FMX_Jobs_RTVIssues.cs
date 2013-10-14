using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using Golem.Framework.CustomElements;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_Jobs_RTVIssues : FmxMenuBar
    {
        public Element btn_CreateRTVIssue = new Element("CreateRTVIssueButton", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_lbtnNew"));
        public Checkbox chk_InvoiceReceived = new Checkbox("InvoiceReceivedCheckbox", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_chkInvoiceReceived"));
        public Element txt_InvoiceUpload_Master = new Element("InvoiceUploadMasterTextField", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_fuInvoice"));
        public Element txt_SignoffUpload_MarkedUp = new Element("InvoiceUploadMasterTextField", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_fuSignoffMU"));
        public Checkbox chk_SignoffReceived = new Checkbox("SignoffReceivedCheckbox", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_chkSignoffReceivedFromStore"));
        public Checkbox chk_PaperworkApproved = new Checkbox("PaperworkApprovedCheckbox", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_chkApproved"));
        public Element btn_Save = new Element("SaveButton", By.XPath("//div[@id='ctl00_ContentPlaceHolder1_pnlPopUpRTVContent']//a[@id='ctl00_ContentPlaceHolder1_ucRTV_fvRTV_lnkSave']"));
        public Element btn_Cancel = new Element("SaveButton", By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_lnkCancel"));
        public Element tbl_Results = new Element("ResultsTable", By.Id("ctl00_ContentPlaceHolder1_ucRTV_gvWorkOrderHistory_gvGrid"));

        public FMX_Jobs_RTVIssues SelectWorkOrder()
        {
            driver.FindVisibleElement(By.XPath("//table[@id='ctl00_ContentPlaceHolder1_ucRTV_gvWorkOrderHistory_gvGrid']//tr[2]")).Click();
            return new FMX_Jobs_RTVIssues();
        }

        public FMX_Jobs_RTVIssues ClickNewRTVIssueButton()
        {
            btn_CreateRTVIssue.WaitUntil.Visible().Click();
            return new FMX_Jobs_RTVIssues();
        }

        public FMX_Jobs_RTVIssues SubmitIssueInfo(bool invoiceReceived, string pathToMasterForInvoice, bool signoffReceived, string pathToMarkedUpForSignoff, bool paperworkApproved)
        {
            chk_InvoiceReceived.SetCheckbox(invoiceReceived);
            txt_InvoiceUpload_Master.SendKeys(pathToMasterForInvoice);
            chk_SignoffReceived.WaitUntil.Visible().SetCheckbox(signoffReceived);
            txt_SignoffUpload_MarkedUp.SendKeys(pathToMarkedUpForSignoff);
            //.chk_PaperworkApproved.SetCheckbox(paperworkApproved);
            driver.WaitForVisible(By.Id("ctl00_ContentPlaceHolder1_ucRTV_fvRTV_lnkSaveAndUpload")).Click();
            return new FMX_Jobs_RTVIssues();
        }


    }
}
