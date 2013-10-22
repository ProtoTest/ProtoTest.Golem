using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;
using Golem.PageObjects.Cael;

namespace Golem.PageObjects.Cael
{
    public class DashBoardAssessmentPage : BasePageObject
    {
        public LoggedInHeader Header = new LoggedInHeader();

        public Element ACCEPT_Button = new Element("Accept Assessment Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_AssessorDashboard_rptOpportunities_ctl00_btnAcceptOpportunity"));
        public Element Decline_Link = new Element("Decline Assessment Link", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_AssessorDashboard_rptOpportunities_ctl00_lbCancelOpportunity"));
        public Element LeftColumn_Container = new Element("Left column container", By.Id("left_col"));
        public Element RightColumn_Container = new Element("Right column container", By.Id("right_col"));
        public Element Assessment_Link = new Element("Assessment Link", By.LinkText("English"));

        public DashBoardAssessmentPage AcceptAssessment()
        {
            ACCEPT_Button.Click();
            return new DashBoardAssessmentPage();
        }

        public DeclineAssessmentPopup DeclineAssessment()
        {
            Decline_Link.Click();
            return new DeclineAssessmentPopup();

            // need to verify there are no assessments in the screen!
        }

        public AssessPortfolioPage SelectPendingAssessment()
        {
            Assessment_Link.Click();
            return new AssessPortfolioPage();
        }

        public override void WaitForElements()
        {
            //LeftColumn_Container.Verify.Visible();
            //   RightColumn_Container.Verify.Visible();
        }
    }
}


