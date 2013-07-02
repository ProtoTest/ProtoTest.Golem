using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_Jobs_JobClosing : FmxMenuBar
    {
        Element JobsTable = new Element("JobsTable", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_gvJobs"));
        Element drp_Job_Outcome = new Element("JobOutcomeDropdown", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_ddCallOutcome"));
        Element chk_SatisfactoryOutcome = new Element("SatisfactoryOutcomeDropdown", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_rblSatisfactory_0"));
        Element chk_UnSatisfactoryOutcome = new Element("UnSatisfactoryOutcomeDropdown", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_rblSatisfactory_1"));

        Element txt_ManagerFirst = new Element("ManagerFirstNameText", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_txtFirstName"));
        Element txt_ManagerLast = new Element("ManagerLastNameText", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_txtLastName"));
        Element txt_Notes = new Element("NotesTextField", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_txtNotes"));
        private Element btn_OpenCalendar = new Element("OpenCalendarButton", By.ClassName("ui-datepicker-trigger"));
        Element txt_JobCompletedDate = new Element("JobCompletedTextField", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_txtJobCompletedDate"));
        Element txt_JobCompletedDateHidden = new Element("JobCompletedHiddenTextField", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_vceJobCompletedDate_ClientState"));

        private Element tbl_DatePickerCalendar = new Element("DatePickerCalendarTable",
                                                             By.ClassName("ui-datepicker-calendar"));

        
        Element btn_SaveOutcomeChanges = new Element("SaveOutcomeChangesButton", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_lbtnInsertOutcome2"));
        Element btn_CancelOutcomeChanges = new Element("CancelOutcomeChangesButton", By.Id("ctl00_ContentPlaceHolder1_ucCloserTeam_fvCloserTeam_lbtnCancelOutcome2"));

        //Notes popup
        Element btn_Close_NotesPopup = new Element("CLoseNotesPopup", By.Id("ctl00_ContentPlaceHolder1_ucNotes_lnkClose"));

        public IWebElement JobByIndex(int index)
        {
            return JobsTable.FindElement(By.XPath("//tbody/tr[" + (index + 1) + "]"));
        }

        public FMX_Jobs_JobClosing SelectJobByIndex(int index)
        {
            JobByIndex(index).Click();
            return new FMX_Jobs_JobClosing();
        }

        public FMX_Jobs_JobClosing CloseNotesPopup()
        {
            btn_Close_NotesPopup.WaitUntilPresent().Click();
            return new FMX_Jobs_JobClosing();
        }

        public FMX_Jobs_JobClosing SelectJobOutcome(string outcome)
        {
            drp_Job_Outcome.FindElement(ByE.Text(outcome)).Click();
            return new FMX_Jobs_JobClosing();
        }
        public FMX_Jobs_JobClosing SelectIfSatisfactory(bool satisfactory)
        {
            if (satisfactory)
            {
                chk_SatisfactoryOutcome.WaitUntilVisible().Click();
            }
            else
            {
                chk_UnSatisfactoryOutcome.WaitUntilVisible().Click();
            }
            return new FMX_Jobs_JobClosing();
        }
        public FMX_Jobs_JobClosing EnterOutcomeInfo(string managerFirst, string managerLast, string notes)
        {
            txt_ManagerFirst.WaitUntilPresent().Text = managerFirst;
            txt_ManagerLast.Text = managerLast;
            txt_Notes.Text = notes;
            btn_OpenCalendar.Click();
            tbl_DatePickerCalendar.FindElement(By.ClassName("ui-state-highlight")).Click();
            return new FMX_Jobs_JobClosing();
        }

        public FMX_Jobs_JobClosing SaveOutcomeChanges()
        {
            btn_SaveOutcomeChanges.Click();
            driver.SwitchTo().Alert().Accept();
            return new FMX_Jobs_JobClosing();
        }

        public FMX_Jobs_JobClosing CancelOutcomeChanges()
        {
            btn_CancelOutcomeChanges.Click();
            return new FMX_Jobs_JobClosing();
        }

        public override void WaitForElements()
        {
            base.WaitForElements();
            JobsTable.VerifyPresent(10);
        }
    }
}
