using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;


namespace GolemPageObjects.Emcon.FMNow
{
    public class FMNow_Request : FMNow_HomePage
    {
        Element rd_ServiceRequest = new Element("rd_ServiceRequest", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_rblJobTypes_0"));
        Element rd_BidRequest = new Element("rd_BidRequest", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_rblJobTypes_1"));
        Element rd_DispatchRequest = new Element("rd_DispatchRequest", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_rblJobTypes_2"));
        Element btn_SearchLocation = new Element("SearchLocation", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_btnSearchLocation"));
        Element txt_SearchFor = new Element("SearchFor", By.Id("ContentPlaceHolder1_txtSearchFor"));
        Element btn_StartSearchLocation = new Element("SearchLocation", By.Id("ContentPlaceHolder1_cmdSearch"));
        //Results
        Element table_FirstResult = new Element("FirstResult_table", By.XPath("//*[@id='ContentPlaceHolder1_dgSearchResults_gvGrid']/tbody/tr[2]/td[2]"));
        Element dd_TypeOfWork = new Element("TypeOfWordRequired", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_ddlTypeOfHelp"));
        Element dd_SubTypeOfWork = new Element("SubTypeOfWorkRequired", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_ddlSubTrade"));
        Element txt_ClientRefNum = new Element("ClientRefNum", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_txtPONumber"));
        Element txt_DescriptionOfService = new Element("DescriptionOfService", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_txtDescription"));
        Element btn_Next = new Element("NextButton", By.Id("ContentPlaceHolder1_CreateServiceRequestStep1_cmdNext"));
        Element btn_SubmitRequest = new Element("SubmitRequestButton", By.Id("ContentPlaceHolder1_cmdSubmit"));
        Element txt_RequestReferanceNumber = new Element("RequestReferanceNumber", By.XPath("//*[@id='ContentPlaceHolder1_lblConfirmationText']/font"));

        private static List<string> RequestIDCreated = new List<string>();

        public FMNow_Request SelectRequestType(int type)
        {
            Element RequestType = new Element("RequestType", By.XPath("//*[@id='ContentPlaceHolder1_CreateServiceRequestStep1_rblJobTypes']/tbody/tr[" +type.ToString()+"]/td/input"));
            RequestType.WaitUntilVisible().Click();
            return new FMNow_Request();
        }

        public FMNow_Request SearchLocation(string searchTerms)
        {
            btn_SearchLocation.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            txt_SearchFor.WaitUntilVisible().Text = searchTerms;
            btn_StartSearchLocation.WaitUntilVisible().Click();
             
            return new FMNow_Request();
        }

        public FMNow_Request SelectFirstResult(string description, string clientRefNumber, string typeOfwork, string subTypeOfwork = "[Not Sure]")
        {
            table_FirstResult.Click();
            pleaseWait.WaitUntilNotVisible();
            dd_TypeOfWork.SelectOption(typeOfwork);
            pleaseWait.WaitUntilNotVisible();
            dd_SubTypeOfWork.SelectOption(subTypeOfwork);
            txt_ClientRefNum.Text = "PROTOTEST-" + clientRefNumber;
            txt_DescriptionOfService.Text = "This was created by a PROTOTEST automated script " + description;
            btn_Next.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            btn_SubmitRequest.WaitUntilVisible().Click();
            pleaseWait.WaitUntilNotVisible();
            txt_RequestReferanceNumber.WaitUntilVisible();
            RequestIDCreated.Add(txt_RequestReferanceNumber.Text);
            return new FMNow_Request();
        }

        public List<string> GetRequestIDs()
        {
            return RequestIDCreated;
        }


    }
}
