using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FmxMenuBar : BasePageObject
    {
        public Element tab_Home = new Element("HomeTab", By.Id("ctl00_ucMainMenu_lnkHome"));
        public Element tab_Customers = new Element("CutomersTab", By.Id("ctl00_ucMainMenu_lnkCustomer"));
        public Element tab_Locations = new Element("LocationsTab", By.Id("ctl00_ucMainMenu_lnkLocation"));
        public Element tab_Jobs = new Element("JobsTab", By.Id("ctl00_ucMainMenu_lnkJobs"));
        public Element tab_Vendors = new Element("VendorsTab", By.Id("ctl00_ucMainMenu_lnkVendor"));
        public Element tab_VendorSearch = new Element("VendorSearchTab", By.Id("ctl00_ucMainMenu_lnkVendSearch"));
        public Element tab_BusinessTools = new Element("BusinessToolsTab", By.Id("ctl00_ucMainMenu_lnkBizTools"));
        public Element tab_Reports = new Element("ReportsTab", By.Id("ctl00_ucMainMenu_lnkReports"));
        public Element tab_Dashboard = new Element("DashboardTab", By.Id("ctl00_ucMainMenu_lnkCharts"));
        public Element btn_NewJobRequest = new Element("NewJobRequestBtn", By.Id("ctl00_lbtnNewRequest"));
        public Element btn_LogoutBtn = new Element("LogoutBtn", By.Id("ctl00_LoginStatus1"));
        //this overlay is used throughout the site
        public Element pleaseWait = new Element("PleaseWaitOverlay", By.XPath("//*[@id='ctl00_UpdateProg1']"));

        public bool notWaiting()
        {
            while (pleaseWait.GetAttribute("aria-hidden") == "false")
            {
                Thread.Sleep(100);
            }
            return true;
        }

        public override void WaitForElements()
        {
            tab_Home.VerifyVisible(10);
            tab_Jobs.VerifyVisible(10);
            tab_Dashboard.VerifyVisible(10);
            btn_NewJobRequest.VerifyVisible(10);
        }
        
    }
}
