using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class ReviewAssessmentConfirmPage : BasePageObject
    {
        string noCreditText = "Credit Not Recommended";
        string creditRecommendedText = "Credit Recommended!";

        Element Page_Title = new Element("Page Title", By.XPath("//div[@id='container_content_inner']/div/h1"));
        Element GoBackAndEdit_Link = new Element("Go back and edit assessment link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_ReviewAssessment_editAssessmentHyperLink"));
        Element Submit_Button = new Element("Submit this assessment button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_ReviewAssessment_submitAssessmentButton"));
        Element CreditRecommendation_Label = new Element("Credit Recommended Label", By.XPath("//div[@class='totals']//div[@class='t-left']/div"));
        Element TotalScore_Label = new Element("Total Score Label", By.XPath("//div[@class='totals']//div[@class='t-right']/span[2]"));

        public AssessPortfolioPage GoBackAndEditAssessment()
        {
            GoBackAndEdit_Link.Click();
            return new AssessPortfolioPage();
        }

        public void SubmitAssessment()
        {
            Submit_Button.Click();
        }

        public ReviewAssessmentConfirmPage VerifyCreditRecommendation(string score_str, Boolean credit_recommended)
        {
            TotalScore_Label.Verify.Text(score_str);
            if (credit_recommended)
            {
                CreditRecommendation_Label.Verify.Text(creditRecommendedText.Trim());
            }
            else
            {
                CreditRecommendation_Label.Verify.Text(noCreditText);
            }
            return this;
        }

        public override void WaitForElements()
        {
            Page_Title.Verify.Visible().Verify.Text("Review Assessment");
            Submit_Button.Verify.Visible().Verify.Value("Submit This Assessment");
        }
    }
}