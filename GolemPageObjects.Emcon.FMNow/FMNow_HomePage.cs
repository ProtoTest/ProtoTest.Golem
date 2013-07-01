using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace GolemPageObjects.Emcon.FMNow
{
    public class FMNow_HomePage : BasePageObject
    {
        Element Home_MainTab = new Element("Home_MainTab", By.CssSelector("span.TopNavTabLink"));
        Element Request_MainTab = new Element("Request_MainTab", By.CssSelector("#lnkRequest > span.TopNavTabLink"));
        Element Search_MainTab = new Element("Search_MainTab", By.CssSelector("#lnkSearch > span.TopNavTabLink"));
        Element Preferances_MainTab = new Element("Preferances_MainTab", By.CssSelector("#lnkPrefs > span.TopNavTabLink"));

        //Under Home_MainTab
        Element RepairsTab = new Element("RepairsTab", By.CssSelector("span.SubNavTabLink"));
        Element FinanceTab = new Element("FinanceTab", By.CssSelector("#lnkFinance > span.SubNavTabLink"));
        Element VolumeTab = new Element("VolumeTab", By.CssSelector("#lnkVolume > span.SubNavTabLink"));

        //PleaseWait
        public Element pleaseWait = new Element("PleaseWait", By.XPath("//*[@id='loadingDiv']"));

        public FMNow_HomePage ClickHomeMainTab()
        {
            //Also brings up the Home_Repairs subtab
            Home_MainTab.WaitUntilVisible().Click();
            return new FMNow_HomePage();
        }

        public FMNow_Home_Finance ClickFinanceTab()
        {
            FinanceTab.WaitUntilVisible().Click();
            return new FMNow_Home_Finance();
        }

        //TODO add function for Home_Volume subtab


        public FMNow_Request ClickRequestMainTab()
        {
            Request_MainTab.WaitUntilVisible().Click();
            return new FMNow_Request();
        }

        

        public override void WaitForElements()
        {
            Request_MainTab.WaitUntilVisible();
            Search_MainTab.WaitUntilVisible();
            Preferances_MainTab.WaitUntilVisible();

        }
    }
}




