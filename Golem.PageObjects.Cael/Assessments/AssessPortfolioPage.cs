using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace Golem.PageObjects.Cael
{
    public class AssessPortfolioPage : BasePageObject
    {
        string noCreditText = "Credit Not Recommended";
        string creditRecommendedText = "Credit Recommended";

        Element Page_Title = new Element("Assess Portfolio Title", By.XPath("//div[@id='container_content_inner']/div/h1"));
        Element Detailed_Feedback = new Element("Detailed Feedback", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_AssessPortfolio_txtComments"));
        Element CancelChanges_Link = new Element("Cancel Changes Link", By.LinkText("Cancel Changes"));
        Element SaveChanges_Button = new Element("Save Changes Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_AssessPortfolio_btnSave"));
        Element ReviewAssessment_Button = new Element("Review Assessment Button", By.Id("btnReviewAssessment"));

        Element PointsRewarded_Label = new Element("x/28 Points Rewarded Label", By.Id("divPoints"));
        Element CreditRecommded_Label = new Element("Credit Recommended Label", By.XPath("//div[@class='slider-box-total clearfix']/div/i"));

        Element CourseOutcomes_Slider = new Element("Course Outcomes Slider", By.XPath("//div[@id='slider1']/a"));
        Element LearningFromExp_Slider = new Element("Learning From Experience Slider", By.XPath("//div[@id='slider2']/a"));
        Element UnderstandingTP_Slider = new Element("Understanding of Theory & Practice Slider", By.XPath("//div[@id='slider3']/a"));
        Element Reflection_Slider = new Element("Reflection Slider", By.XPath("//div[@id='slider4']/a"));
        Element LearningApplication_Slider = new Element("Learning Application Slider", By.XPath("//div[@id='slider5']/a"));
        Element Communication_Slider = new Element("Communication Slider", By.XPath("//div[@id='slider6']/a"));
        Element SupportingDocs_Slider = new Element("Supporting Documentation Slider", By.XPath("//div[@id='slider7']/a"));

        public void ReviewAssessment()
        {
            ReviewAssessment_Button.Click();
            // return new ReviewAssessmentConfirmPopup();
        }

        /* 
         * Recommend credit when 20/28 points have been achieved 
         */

        public void VerifyRecommendationSliderValues()
        {
            

            VerifySliderMinValue();
            VerifySliderMaxValue();

            // Set the totals to 19
            SetCourseOutcomesSlider(4);
            SetLearningFromExpSlider(4);
            SetUnderstandingTPSlider(4);
            SetReflectionSlider(4);
            SetLearningApplicationSlider(3);
            SetCommunication_SliderSlider(0);
            SetSupportingDocsSlider(0);

            // verify text with slider updates
            PointsRewarded_Label.VerifyText("19/28 Points");
            CreditRecommded_Label.VerifyText(noCreditText);

            // Set the totals to 20
            SetCourseOutcomesSlider(0);
            SetLearningFromExpSlider(4);
            SetUnderstandingTPSlider(4);
            SetReflectionSlider(0);
            SetLearningApplicationSlider(4);
            SetCommunication_SliderSlider(4);
            SetSupportingDocsSlider(4);

            // verify text with slider updates
            PointsRewarded_Label.VerifyText("20/28 Points");
            CreditRecommded_Label.VerifyText(creditRecommendedText);
        }

        private void VerifySliderMaxValue()
        {
            // Set the totals to 28 (all maxed)
            SetCourseOutcomesSlider(4);
            SetLearningFromExpSlider(4);
            SetUnderstandingTPSlider(4);
            SetReflectionSlider(4);
            SetLearningApplicationSlider(4);
            SetCommunication_SliderSlider(4);
            SetSupportingDocsSlider(4);

            // verify text with slider updates
            PointsRewarded_Label.VerifyText("28/28 Points");
            CreditRecommded_Label.VerifyText(creditRecommendedText);

            // Try to set the sliders beyond the max value
            SetCourseOutcomesSlider(5);
            SetLearningFromExpSlider(5);
            SetUnderstandingTPSlider(5);
            SetReflectionSlider(5);
            SetLearningApplicationSlider(5);
            SetCommunication_SliderSlider(5);
            SetSupportingDocsSlider(5);

            // verify text with slider updates
            PointsRewarded_Label.VerifyText("28/28 Points");
            CreditRecommded_Label.VerifyText(creditRecommendedText);
        }

        private void VerifySliderMinValue()
        {
            // Initially set all sliders to zero
            SetCourseOutcomesSlider(0);
            SetLearningFromExpSlider(0);
            SetUnderstandingTPSlider(0);
            SetReflectionSlider(0);
            SetLearningApplicationSlider(0);
            SetCommunication_SliderSlider(0);
            SetSupportingDocsSlider(0);

            // Verify labels
            PointsRewarded_Label.VerifyText("0/28 Points");
            CreditRecommded_Label.VerifyText(noCreditText);

            // Try to move the sliders beyond min value
            SetCourseOutcomesSlider(-1);
            SetLearningFromExpSlider(-1);
            SetUnderstandingTPSlider(-1);
            SetReflectionSlider(-1);
            SetLearningApplicationSlider(-1);
            SetCommunication_SliderSlider(-1);
            SetSupportingDocsSlider(-1);

            // Verify labels
            PointsRewarded_Label.VerifyText("0/28 Points");
            CreditRecommded_Label.VerifyText(noCreditText);
        }




        public override void WaitForElements()
        {
            Page_Title.VerifyVisible(30).VerifyText("Assess Portfolio");
        }

 
        /// <summary>
        ///     Sets the slider on the page to requested value.
        ///     NOTE: Assumes the slider is in the default position of 0
        /// </summary>
        /// <param name="slider">Slider Element</param>
        /// <param name="newPosition">Value to set</param>
        private void setSlider(Element slider, int newPosition)
        {
            int i = 0;
            int sliderPositionByStylePercentage = 0;
            int curSliderPosition = -1;
            string key = Keys.ArrowUp;
            
            // The style attribute to set the slider position is a percentage
            // i.e. "style="left: 100%;"
            // Match the pecentage to determine the position
            Match m = Regex.Match(slider.GetAttribute("style"), @"\d+");

            if (m.Success)
            {
                sliderPositionByStylePercentage = Convert.ToUInt16(m.Value);
            }

            switch (sliderPositionByStylePercentage)
            {
                case 0:
                    curSliderPosition = 0;
                    break;
                case 25:
                    curSliderPosition = 1;
                    break;
                case 50:
                    curSliderPosition = 2;
                    break;
                case 75:
                    curSliderPosition = 3;
                    break;
                case 100:
                    curSliderPosition = 4;
                    break;
                default:
                    Common.Log("AssessPortfolioPage::setSlider(): Invalid slider style percentage found by regex");
                    break;
            }

            if ((newPosition - curSliderPosition) < 0)
            {
                key = Keys.ArrowDown;
            }

            // need to click the slider before you can start issuing key presses to move the slider
            slider.Click();

            for (i = 0; i < Math.Abs(newPosition - curSliderPosition); ++i)
            {
                slider.SendKeys(key);
            }
        }

        /// Setting slider apis
        public void SetCourseOutcomesSlider(int value)
        {
            setSlider(CourseOutcomes_Slider, value);
        }

        private void SetLearningFromExpSlider(int value)
        {
            setSlider(LearningFromExp_Slider, value);
        }

        private void SetUnderstandingTPSlider(int value)
        {
            setSlider(UnderstandingTP_Slider, value);
        }

        private void SetReflectionSlider(int value)
        {
            setSlider(Reflection_Slider, value);
        }

        private void SetLearningApplicationSlider(int value)
        {
            setSlider(LearningApplication_Slider, value);
        }

        private void SetCommunication_SliderSlider(int value)
        {
            setSlider(Communication_Slider, value);
        }

        private void SetSupportingDocsSlider(int value)
        {
            setSlider(SupportingDocs_Slider, value);
        }
    }
}
