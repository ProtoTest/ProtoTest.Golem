using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;
using Golem.PageObjects.Cael;

namespace Golem.PageObjects.Cael
{
    public class DashBoardAssessmentPage : BasePageObject
    {
        public LoggedInHeader Header = new LoggedInHeader();
        
        public Element LeftColumn_Container = new Element("Left column container", By.Id("left_col"));
        public Element RightColumn_Container = new Element("Right column container", By.Id("right_col"));
        public Element Assessment_Link = new Element("Assessment Link", By.LinkText("English"));


        /// <summary>
        ///     Accept an assessment based on the course category string (i.e. History, English)
        /// </summary>
        /// <param name="courseCategory">Course category string</param>
        /// <returns></returns>
        public DashBoardAssessmentPage AcceptAssessment(string courseCategory)
        {
            driver.FindElementWithText(courseCategory).FindInSiblings(By.XPath("//input[@type='submit']")).Click();
            return new DashBoardAssessmentPage();
        }

        /// <summary>
        ///     Decline an assessment based on the course category string (i.e. History, English)
        /// </summary>
        /// <param name="courseCategory">Course category string</param>
        /// <returns></returns>
        public DeclineAssessmentPopup DeclineAssessment(string courseCategory)
        {
            driver.FindElementWithText(courseCategory).GetParent().FindInSiblings(By.PartialLinkText("Decline")).Click();
            
            return new DeclineAssessmentPopup();
        }

        /// <summary>
        ///     Selects the "English" pending assessment
        /// </summary>
        /// <returns></returns>
        public AssessPortfolioPage SelectPendingAssessment()
        {
            Assessment_Link.Click();
            return new AssessPortfolioPage();
        }

        public override void WaitForElements()
        {
            //LeftColumn_Container.Verify().Visible();
            //   RightColumn_Container.Verify().Visible();
        }
    }
}


