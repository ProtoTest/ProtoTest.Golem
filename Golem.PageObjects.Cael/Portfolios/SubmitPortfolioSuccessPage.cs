using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael.Portfolios
{
    public class SubmitPortfolioSuccessPage : BasePageObject
    {
        public Element ReviewPortfolio_Link = new Element("Review portfolio link",By.LinkText("Review your portfolio"));
        public Element ReturnToDashboard_Link = new Element("Return to dashboard link", By.LinkText("Return to your Dashboard"));
        public Element CreatePortfolio_Link = new Element("Create portfolio link", By.LinkText("Create a new portfolio"));

        public DashboardPage ReturnToDashboard()
        {
            ReturnToDashboard_Link.Click();
            return new DashboardPage();
        }

        public PreviewPortfolioPage ReviewYourPortfolio()
        {
            ReviewPortfolio_Link.Click();
            return new PreviewPortfolioPage();
        }

        public PaymentPage CreateNewPortfolio()
        {
            CreatePortfolio_Link.Click();
            return new PaymentPage();
        }

        public override void WaitForElements()
        {
            
        }
    }
}
