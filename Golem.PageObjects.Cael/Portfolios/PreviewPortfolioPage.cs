using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class PreviewPortfolioPage : BasePageObject
    {
        public Element CourseTitle_Link = new Element("Course Title Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_PreviewPortfolio_courseTitleHyperLink"));
        public Element CourseSchool_Link = new Element("Schol LInk", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_PreviewPortfolio_schoolNameHyperLink"));
        public Element LearningOutcomes_Text = new Element("Learning Outcomes Text", By.ClassName("credits-padding"));
        public Element LearningOutcomes_Link = new Element("Learning Outcomes Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_PreviewPortfolio_learningNarrativeHyperLink"));
        public Element Credits_Text = new Element("Credits Text", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_PreviewPortfolio_creditsLabel"));

        public override void WaitForElements()
        {
            CourseTitle_Link.Verify.Visible();
            CourseSchool_Link.Verify.Visible();
            LearningOutcomes_Link.Verify.Visible();
            LearningOutcomes_Text.Verify.Visible();
            Credits_Text.Verify.Visible();
        }
    }
}
