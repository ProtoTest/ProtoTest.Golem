using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    class FMX_DashboardPage : FmxMenuBar
    {
        Element dd_TeamSelector = new Element("TeamSelectionDropDown", By.Id("ctl00_ContentPlaceHolder1_ucDailyReports_ddlDailyTeam"));

    }
}
