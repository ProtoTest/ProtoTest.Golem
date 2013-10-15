using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.Framework.CustomElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Golem.PageObjects.Cael
{
    public class Kentico : BasePageObject
    {
        public Link SiteManager = new Link("Site Manager link", By.LinkText("Site Manager"));
        public Link Development = new Link("Development Link",By.LinkText("Development"));
        public Link CustomTables = new Link("Custom Tables", By.Id("node_developmentcustomtables"));
        public Button Data = new Button("Data Link",By.XPath("//span[text()='Data']"));
        public Button Save = new Button("Save Button",By.XPath("//span[text()='Save']"));
        
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
