using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael.Setup_Portfolio
{
    public class EditPortfolioPage : BasePageObject
    {
        public Element CourseTitle_Link = new Element("CourseTitle_Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_EditPortfolio_courseTitleHyperLink"));
        public Element ChangeCourseDetails_Link = new Element("ChangeCourseDetails_Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_EditPortfolio_changeCourseDetailsHyperLink"));
        public Element PreviewPortfolio_Link = new Element("PreviewPortfolio_Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_EditPortfolio_previewPortfolioHyperLink"));
        public Element School_Link = new Element("School_Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_EditPortfolio_schoolNameHyperLink"));
        public Element LearningOutcomes_Field = new Element("LearningOutcomes_Field", By.Id("learningOutcomes"));
        public Element LearningNarrative_FileChooser = new Element("LearningNarrative_FileChooser", By.Id("fileupload"));
        public Element AddAnotherDocument_Link = new Element("AddAnotherDocument_Link", By.LinkText("Add another document"));
        public Element Cancel_Link = new Element("Cancel_Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_EditPortfolio_cancelHyperLink"));
        public Element SaveChanges_Button = new Element("SaveChanges_Button", ByE.PartialText("Save Changes"));
        public Element SubmitPortfolio_Button = new Element("SubmitPortfolio_Button", By.Id("submitPortfolioButton"));

        public Element SupportLinkUrl_Field = new Element("LinkUrl Field",By.Name("link_url"));
        public Element SupportFileCaption_Field = new Element("Support File Caption Field", By.Name("course_description_link"));
        public Element SupportDescription_Field = new Element("Support Description Field", By.XPath("//textarea[@ng-model='supportingDocument.description']"));
        public Element SupportSave_Button = new Element("Support save button", By.XPath("//button[text()='Save']"));
        public Element FileUpload_FileChooser = new Element("File UPload file cho oser", By.Id("file"));

        public EditPortfolioPage AddSupportDocument()
        {
            AddAnotherDocument_Link.Click();
            return this;
        }

        public EditPortfolioPage EnterSupportDocument(string url, string caption, string descrtiption)
        {
            driver.FindVisibleElement(By.Name("link_url")).SendKeys(url);
            driver.FindVisibleElement(By.Name("course_description_link")).SendKeys(caption);
            driver.FindVisibleElement(By.XPath("//textarea[@ng-model='supportingDocument.description']")).SendKeys(descrtiption);
            //ByE.PartialAttribute("button", "text()", "Save");
            driver.FindVisibleElement(By.XPath("//button[contains(text(),'Save')]")).Click();
            return this;
        }

        public EditPortfolioPage EditOutcomesText(string text)
        {
            driver.SwitchTo().Frame(driver.WaitForVisible(By.XPath("//iframe")).FindElement(By.XPath("//iframe"))).FindElement(By.TagName("body")).SendKeys(text);
            driver.SwitchTo().DefaultContent();
            return this;
        }

        public EditPortfolioPage ChooseNarrativeFile(string filePath)
        {
            LearningNarrative_FileChooser.SendKeys(filePath);
            return this;
        }

        public SubmitPortfolioPage SubmitPortfolio()
        {
            SubmitPortfolio_Button.Click();
            return new SubmitPortfolioPage();
        }

        public EditPortfolioPage SaveChanges()
        {
            SaveChanges_Button.Click();
            return this;
        }

        public override void WaitForElements()
        {
            LearningNarrative_FileChooser.Verify.Present();
            LearningOutcomes_Field.Verify.Present();
            SaveChanges_Button.Verify.Present();
            //SubmitPortfolio_Button.Verify.Present();
        }
    }
}
