using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.Framework.CustomElements;
using OpenQA.Selenium;
using Golem.PageObjects.Cael;

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
        
        /// <summary>
        ///     Clicks the edit row for the requested string text
        /// </summary>
        /// <param name="text">The ID of the database column we are searching for</param>
        /// <param name="title">The title of the button to find (i.e. 'Edit', 'Find Assessor') </param>
        /// <returns></returns>
        public static void ClickEditRowButtonByText(string text)
        {
            TestBaseClass.driver.FindElementWithText(text).FindInSiblings(By.Id("m_c_grdListPortfolio_v_ctl18_aedit")).Click();
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
            for (int i = 0; i < subjects.Count(); i++)
            {
                subjects_MultiSelect.SelectOption(subjects[i]);
            }

            save_Button.Click();

            return new Kentico();
        }

        public Kentico AssignPortfolioToAssessor(string portfolioId, string assessor_email)
        {
            LearningCounts_Link.Verify.Visible().Click();

            // This link is within an iframe, and then another frame
            TestBaseClass.driver.SwitchTo().Frame("m_c_cmsdesktop").SwitchTo().Frame("toolscontent").FindElement(ByE.PartialText("Assign Portfolios")).Click();
            ClickEditRowButtonByText(portfolioId);

            Element Assessor_Dropdown = new Element("Assign Assessor to Portfolio: Assessor Dropdown", By.Id("m_c_ddlAssessors"));
            Field AssignDate_Field = new Field("Assign Assessor to Portfolio: Assigning Date Text Field", By.Id("m_c_dtPickup_txtDateTime"));
            Field DueDate_Field = new Field("Assign Assessor to Portfolio: Due Date Text Field", By.Id("m_c_dtDueDate_txtDateTime"));
            Button Assign_Btn = new Button("Assign Assessor to Portfolio: Assign Button", By.Id("m_c_Button1"));

            Assessor_Dropdown.Verify.Visible().SelectOptionByPartialText(assessor_email);

            DateTime dateTimeNow = DateTime.Now;
            AssignDate_Field.SetText(dateTimeNow.ToString());
            DueDate_Field.SetText(dateTimeNow.AddDays(5).ToString());

            Assign_Btn.Click();

            return new Kentico();
        }

        public override void WaitForElements()
        {
        }
    }
}
