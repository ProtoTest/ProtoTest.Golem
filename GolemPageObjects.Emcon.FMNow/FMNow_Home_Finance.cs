using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace GolemPageObjects.Emcon.FMNow
{
    public class FMNow_Home_Finance : FMNow_HomePage
    {
        //The charts load out of order on this page, causing data to be enterred into the incorrect fields unless all element are loaded
        //These objects have to be verified visible before data can be entered
        Element wait_dd_SpendbyLocation_ByVisits = new Element("Chart1_dd1", By.Id("ContentPlaceHolder1_UcStoreSpend1_ddlStoreSpendChartType"));
        Element wait_txt_CallAvoided_Days = new Element("Chart2_ txt1", By.Id("ContentPlaceHolder1_UcCallAvoided1_txtCallsAvoidedChartViewNumOfDays"));
        Element wait_txt_InvoicedJobs = new Element("Chart3_txt1", By.Id("ContentPlaceHolder1_ucJobsInvoiced_txtJobsInvoicedChartViewNumOfDays"));
        Element wait_chk_All = new Element("Chart4_rd1", By.Id("ContentPlaceHolder1_UcAccrualAmount1_rblAccrualAmountType_0"));
        Element wait_chk_all = new Element("Chart5_chk1", By.Id("ContentPlaceHolder1_UcQuarterlyInvoice1_rblQtrInvClickType"));

        //for some reason still the chart above this chart looks to be getting the options set instead of the last chart --verified the IDs not sure how to handle this
        Element rd_TandM = new Element("T&MRadioButton", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_rblCustInvJobType_0"));
        Element rd_Quoted = new Element("QuotedRadioButton", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_rblCustInvJobType_1"));
        Element rd_AllInvoices = new Element("AllInvoicesRadioButton", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_rblCustInvClickType_0"));
        Element rd_ByTrade = new Element("ByTradeRadio", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_rblCustInvClickType_1"));

        Element dd_StartFiscalMonth = new Element("FiscalStartMonth", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_ddlCustInvFiscalStart"));
        Element dd_StartQuarter = new Element("StartQuarter", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_ddlCustInvQuarterFrom"));
        Element dd_EndQuarter = new Element("EndQuarter", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_ddlCustInvQuarterTo"));
        Element dd_StartYear = new Element("StartYear", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_ddlCustInvYearFrom"));
        Element dd_EndYear = new Element("EndYear", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_ddlCustInvYearTo"));
        Element btn_RefreshChart = new Element("RefreshChart", By.Id("ContentPlaceHolder1_UcInvoiceCompareAverage1_btnCustInvBasicRefresh"));

        //This still needs some work once data gets filled in
        Element InvoiceComparisonByAverageChart = new Element("InvoiceComparisonByAverageChart", By.XPath("//*[@id='invoiceCompareChartCustomer']/div"));

        public FMNow_Home_Finance getInvoiceComparisonByAverageChart(bool TandM, bool AllInvoices, string FiscalYearMonth,
                                                                     string StartQuarter, string endQuarter,
                                                                     string startYear, string endYear)
        {
            if (TandM)
            {
                rd_TandM.WaitUntil.Visible().Click();
            }
            else
            {
                rd_Quoted.WaitUntil.Visible().Click();
            }
            if (AllInvoices)
            {
                rd_AllInvoices.WaitUntil.Visible().Click();
            }
            else
            {
                rd_ByTrade.WaitUntil.Visible().Click();
            }
            dd_StartFiscalMonth.SelectOption(FiscalYearMonth);
            //dd_StartFiscalMonth.WaitUntil.Visible().FindElement(ByE.Text(FiscalYearMonth)).Click();
            dd_StartQuarter.SelectOption(StartQuarter);
            dd_EndQuarter.SelectOption(endQuarter);
            dd_StartYear.SelectOption(startYear);
            dd_EndYear.SelectOption(endYear);
            btn_RefreshChart.WaitUntil.Visible().Click();
            //Going to need to do something with the chart here
            return new FMNow_Home_Finance();
        }

        public override void WaitForElements()
        {
 	        base.WaitForElements();
            wait_dd_SpendbyLocation_ByVisits.WaitUntil.Visible();
            wait_txt_CallAvoided_Days.WaitUntil.Visible();
            wait_txt_InvoicedJobs.WaitUntil.Visible();
            wait_chk_All.WaitUntil.Visible();
            wait_chk_all.WaitUntil.Visible();
        }




    }
}
