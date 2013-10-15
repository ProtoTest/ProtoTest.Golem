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
    public class SetupPortfolioPage : BasePageObject
    {

        public Element CourseTitle_Field = new Element("Course Title Field",By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_titleTextBox"));
        public Element CourseNumber_Field = new Element("CourseNumber_Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_numberTextBox"));
        public Element CourseDescription_Field = new Element("CourseDescription_Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_descriptionTextBox"));
        public Element CourseCredits_Dropdown = new Element("CourseCredits_Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_creditsDDList"));
        public Element CourseLink_Field = new Element("CourseLink_Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_descriptionLinkTextBox"));
        public Element School_Dropdown = new Element("School_Dropdown", By.Id("schoolDDList"));
        public Element URL_Field = new Element("URL_Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_schoolUrlTextBox"));
        public Element Region_Dropdown = new Element("Region_Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_regionDDList"));
        public Element Category_Dropdown = new Element("Category_Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_categoryDDList"));
        public Element SubCategory_Dropdown = new Element("SubCategory_Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_subcategoryDDList"));
        public Element Cancel_Link = new Element("Cancel_Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_SetupPortfolio_cancelHyperLink"));
        public Element CreatePortfolio_Button = new Element("CreatePortfolio_Button", By.Id("savePortfolioButton"));

        public EditPortfolioPage CreatePortfolio(string title, string number, string credits, string desc, string link, string school,
            string schoolUrl, string region, string courseCategory, string subCategory)
        {
            CourseTitle_Field.Text = title;
            CourseNumber_Field.Text = number;
            CourseCredits_Dropdown.SelectOption(credits);
            CourseDescription_Field.Text = desc;
            CourseLink_Field.Text = link;
            School_Dropdown.SelectOption(school);
            URL_Field.Text = schoolUrl;
            Region_Dropdown.SelectOption(region);
            Category_Dropdown.SelectOption(courseCategory);
            SubCategory_Dropdown.WaitForPresent(ByE.Text(subCategory),30).Click();
            CreatePortfolio_Button.Click();
            return new EditPortfolioPage();
        }

        public override void WaitForElements()
        {

            CourseTitle_Field.Verify.Visible();
            CreatePortfolio_Button.Verify.Visible();
        }
    }
}
