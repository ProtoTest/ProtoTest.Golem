using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.Framework.CustomElements;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class Kentico : BasePageObject
    {
        public Link SiteManager = new Link("Site Manager link", By.LinkText("Site Manager"));
        public Link Development = new Link("Development Link",By.LinkText("Development"));
        public Link Administration = new Link("Administration Link", By.Id("TabItem_1"));
        public Link CustomTables = new Link("Custom Tables", By.Id("node_developmentcustomtables"));
        public Button Data = new Button("Data Link",By.XPath("//span[text()='Data']"));
        public Button Save = new Button("Save Button",By.XPath("//span[text()='Save']"));

        public Link LearningCounts_Link = new Link("Learning Counts Tab link", By.Id("TabItem_6"));
        
        public static Button EditRowButtonByText(string text)
        {
            return new Button("EditRowButton",By.XPath("//tr[//td[text()='"+text+"']]//input[@title='Edit']"));
        }

        public static Kentico Login(string username, string password)
        {
            TestBaseClass.driver.Navigate().GoToUrl(@"http://lcdev.bluemodus.com/CMSPages/logon.aspx?ReturnUrl=%2fcmsdesk");
            TestBaseClass.driver.FindElement(By.Id("Login1_UserName")).SendKeys(username);
            TestBaseClass.driver.FindElement(By.Id("Login1_Password")).SendKeys(password);
            TestBaseClass.driver.FindElement(By.Id("Login1_LoginButton")).Click();
            return new Kentico();
        }

        public Kentico CreateAssessor(string username, string password, string department, string[] subjects)
        {
            LearningCounts_Link.Verify.Visible().Click();

            // This link is within an iframe, and then another frame
            TestBaseClass.driver.SwitchTo().Frame("m_c_cmsdesktop").SwitchTo().Frame("toolscontent").FindElement(ByE.PartialText("new Assessor")).Click();

            Element username_TextField = new Element("New Assessor User Name text field", By.Id("m_c_ucUserName_txtUserName"));
            Element fullname_TextField = new Element("New Assessor Full Name text field", By.Id("m_c_TextBoxFullName"));
            Element email_TextField = new Element("New assessor email", By.Id("m_c_TextBoxEmail"));
            Element department_DropDown = new Element("New assessor Department Dropdown", By.Id("m_c_categoryDDList"));
            Element subjects_MultiSelect = new Element("New assessor subjects multi select", By.Id("m_c_subcategoryDDList"));
            Element password_TextField = new Element("New assessor password", By.Id("m_c_passStrength_txtPassword"));
            Element password_confirm_TextField = new Element("New assesor password confirm", By.Id("m_c_TextBoxConfirmPassword"));
            Button save_Button = new Button("New assessor save button", By.Id("m_actionsElem_editMenuElem_menu_menu_HA_00"));

            username_TextField.Text = username;
            fullname_TextField.Text = username;
            email_TextField.Text = username;
            password_TextField.Text = password;
            password_confirm_TextField.Text = password;
            department_DropDown.SelectOption(department);
            subjects_MultiSelect.SelectOption(subjects[1]);
            save_Button.Click();

            return new Kentico();
        }

        public Kentico AssignPortfolioToAssessor(string portfolioId, string assessorId)
        {
            //these fields show up on the Edit Item Page 
            Field StudentIdField = new Field("STudentIDField",
By.Id("m_c_customTableForm_customTableForm_ctl00_StudentID_textbox"));

            Field AssesorIdField = new Field("AssessorId",
            By.Id("m_c_customTableForm_customTableForm_ctl00_AssessorID_textbox"));

            Field AssignedByField = new Field("AssignedByField", By.Id("m_c_customTableForm_customTableForm_ctl00_AssignedBy_textbox"));
            Link DateAssignedNow = new Link("DateAssignedNowLink", By.Id("m_c_customTableForm_customTableForm_ctl00_DateAssigned_timePicker_btnNow"));
            Field StatusId = new Field("AssessmentStatusID", By.Id("m_c_customTableForm_customTableForm_ctl00_AssessmentStatusID_textbox"));
            //first we update the portfolio table
            SiteManager.Click();
            Development.Click();
            CustomTables.Click();
            EditRowButtonByText("Portfolio").Click();
            Data.Click();
            EditRowButtonByText(portfolioId).Click();


            string studentId = StudentIdField.GetAttribute("value");
            AssesorIdField.Text = assessorId;
            AssignedByField.Text = studentId;
            DateAssignedNow.Click();
            Save.Click();

            //now update assessment table
            CustomTables.Click();
            EditRowButtonByText("Assessment").Click();
            Data.Click();
            EditRowButtonByText(portfolioId).Click();
            AssesorIdField.Text = assessorId;
            DateAssignedNow.Click();
            
            StatusId.Text = "6";
            Save.Click();
            return new Kentico();
        }

        public override void WaitForElements()
        {
        }
    }
}
