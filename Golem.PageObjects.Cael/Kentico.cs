using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Golem.PageObjects.Cael;
using MbUnit.Framework;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.WebDriver.Elements.Types;
using ProtoTest.Golem.Core;

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

        public Link LearningCounts_Link = new Link("Learning Counts Tab link", By.XPath("//span[text()='LearningCounts']"));

        public static Kentico Login(string username, string password)
        {
            string site_addr = Config.GetConfigValue("EnvUrl", null);

            Assert.IsNotNull(site_addr);

            WebDriverTestBase.driver.Navigate().GoToUrl(string.Format("{0}/CMSPages/logon.aspx?ReturnUrl=%2fcmsdesk", site_addr));
            WebDriverTestBase.driver.FindElement(By.Id("Login1_UserName")).SendKeys(username);
            WebDriverTestBase.driver.FindElement(By.Id("Login1_Password")).SendKeys(password);
            WebDriverTestBase.driver.FindElement(By.Id("Login1_LoginButton")).Click();
            return new Kentico();
        }

        public Kentico CreateAssessor(string username, string[] departments, string[] subjects)
        {
            LearningCounts_Link.Verify().Visible().Click();

            // This link is within an iframe, and then another frame
            WebDriverTestBase.driver.SwitchTo().Frame("m_c_cmsdesktop").SwitchTo().Frame("toolscontent").WaitForPresent(ByE.PartialText("new Assessor")).Click();

            Element username_TextField = new Element("New Assessor User Name text field", By.Id("m_c_ucUserName_txtUserName"));
            Element firstname_TextField = new Element("New Assessor First Name text field", By.Id("m_c_TextBoxFirstName"));
            Element lastname_TextField = new Element("New Assessor Last Name text field", By.Id("m_c_TextBoxLastName"));
            Element email_TextField = new Element("New assessor email", By.Id("m_c_TextBoxEmail"));
            Element department_MultiSelect = new Element("New assessor Department multi select", By.Id("m_c_lstDepartments"));
            Element subjects_MultiSelect = new Element("New assessor subjects multi select", By.Id("m_c_subcategoryDDList"));
            Button save_Button = new Button("New assessor save button", By.Id("m_actionsElem_editMenuElem_menu_menu_HA_00"));
            Element status_Label = new Element("New assessor created success label", By.Id("m_c_LabelMessage"));

            username_TextField.Text = username;
            firstname_TextField.Text = "ProtoTest";
            lastname_TextField.Text = "Assessor";
            email_TextField.Text = username;

            for (int i = 0; i < departments.Count(); i++)
            {
                department_MultiSelect.Verify().Text(departments[i]);
                department_MultiSelect.SelectOption(departments[i]);
                
            }

            // TODO: create an element verification to verify options in a multi-select
            Common.Delay(500);

            for (int i = 0; i < subjects.Count(); i++)
            {
                subjects_MultiSelect.SelectOption(subjects[i]);
            }

            save_Button.Click();
            status_Label.Verify().Text("A notification has sent to the assessor sucessfully");

            return new Kentico();
        }

         /// <summary>
        ///     Clicks the edit row for the requested string text
        /// </summary>
        /// <param name="text">The ID of the database column we are searching for</param>
        /// <param name="title">The title of the button to find (i.e. 'Edit', 'Find Assessor') </param>
        /// <returns></returns>
        public static void ClickEditRowButtonByText(string text)
        {
            WebDriverTestBase.driver.FindElementWithText(text).FindInSiblings(By.ClassName("UnigridActionButton")).Click();
        }

        private Boolean FindAndClickPortfolioId(string portfolioID)
        {
            LearningCounts_Link.Verify().Visible().Click();

            // This link is within an iframe, and then another frame
            WebDriverTestBase.driver.SwitchTo().Frame("m_c_cmsdesktop").SwitchTo().Frame("toolscontent").FindElement(ByE.PartialText("Assign Portfolios")).Click();
            try
            {
                ClickEditRowButtonByText(portfolioID);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public Kentico AssignPortfolioToAssessor(string portfolioID, string assessor_email)
        {
            // The portfolio IDs submitted may take a bit to display on the 'Assign Portfolio' page before we can assign them to an Assessor
            Retry.Repeat(5).WithPolling(10000).Until(() => FindAndClickPortfolioId(portfolioID));

            Element Assessor_Dropdown = new Element("Assign Assessor to Portfolio: Assessor Dropdown", By.Id("m_c_ddlAssessors"));
            Field AssignDate_Field = new Field("Assign Assessor to Portfolio: Assigning Date Text Field", By.Id("m_c_dtPickup_txtDateTime"));
            Field DueDate_Field = new Field("Assign Assessor to Portfolio: Due Date Text Field", By.Id("m_c_dtDueDate_txtDateTime"));
            Button Assign_Btn = new Button("Assign Assessor to Portfolio: Assign Button", By.Id("m_c_Button1"));

            Assessor_Dropdown.Verify().Visible().SelectOptionByPartialText(assessor_email);

            DateTime dateTimeNow = DateTime.Now;
            AssignDate_Field.SetText(dateTimeNow.ToString());
            DueDate_Field.SetText(dateTimeNow.AddDays(5).ToString());

            Assign_Btn.Click();

            return new Kentico();
        }

        public static void SelectEmail(string email)
        {
            try
            {
                // Kentico interface uses frames; need to default the webdriver back to main content,
                // then switch through the frames again to select email addresses
                WebDriverTestBase.driver.SwitchTo().DefaultContent().
                    SwitchTo().Frame("m_c_cmsdesktop").
                    SwitchTo().Frame("frameMain").
                    SwitchTo().Frame("content").
                    FindElementWithText(email).FindInSiblings(By.TagName("input")).Click();
            }
            catch (Exception)
            {
                // do nothing as webdriver failed to find this email address in the queue
            }
        }

        public Kentico ForceSendEmail(string[] emails)
        {
            Element ResendSelected_Button = new Element("Resend Selected Button", By.XPath("//span[text()='Resend selected']"));

            SiteManager.Verify().Visible().Click();
            Administration.Verify().Visible().Click();

            Common.Delay(3000);

            // The Email Queue link is within an iframe, and then another frame
            WebDriverTestBase.driver.SwitchTo().Frame("m_c_cmsdesktop").SwitchTo().Frame("admintree").FindElement(By.XPath("//span[text()='E-mail queue']")).Click();

            Common.Delay(3000);

            foreach (string email in emails)
            {
                SelectEmail(email);
            }

             ResendSelected_Button.Click();

            // Handle the alert popup to select OK
            WebDriverTestBase.driver.SwitchTo().Alert().Accept();

            return new Kentico();
        }

        public void Logout()
        {
            // Get out of whatever frame we are currently in
            WebDriverTestBase.driver.SwitchTo().DefaultContent();

            new Element("Sign out button", By.XPath("//span[text()='Sign Out']")).Click();
        }

        public override void WaitForElements()
        {
        }

    }
}
