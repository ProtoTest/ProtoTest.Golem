using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;


namespace GolemPageObjects.Emcon.FMNow
{
    public class FMNow_Home_Finance : FMNow_HomePage
    {
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
                rd_TandM.WaitUntilVisible().Click();
            }
            else
            {
                rd_Quoted.WaitUntilVisible().Click();
            }
            if (AllInvoices)
            {
                rd_AllInvoices.WaitUntilVisible().Click();
            }
            else
            {
                rd_ByTrade.WaitUntilVisible().Click();
            }
            dd_StartFiscalMonth.FindElement(ByE.Text(FiscalYearMonth)).Click();
            dd_StartQuarter.FindElement(ByE.Text(StartQuarter)).Click();
            dd_EndQuarter.FindElement(ByE.Text(endQuarter)).Click();
            dd_StartYear.FindElement(ByE.Text(startYear)).Click();
            dd_EndYear.FindElement(ByE.Text(endYear)).Click();
            btn_RefreshChart.WaitUntilVisible().Click();
            //Going to need to do something with the chart here
            return new FMNow_Home_Finance();
        }




    }
}
