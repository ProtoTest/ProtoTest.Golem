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
            //BLOCKED at this point 
            return new FMNow_Request();
        }
    }
}
