using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Cael.Setup_Portfolio;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class PortfoliosPage : BasePageObject
    {
        public LoggedInHeader Header = new LoggedInHeader();
        public Footer Footer = new Footer();

        public Element GetStarted_Link = new Element("Get Started Link",ByE.Text("Get Started"));
        public Element StartAnotherPortfolio_Button = new Element("Start Another portfolio button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_Portfolios_portfolioHyperLink"));
        public Element ConfirmButton = new Element("Confirm button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_Portfolio_enterPaymentButton"));
        public Element PortfolioTitle_Text = new Element("portfolio title", By.ClassName("top-page-title"));

        public PortfoliosPage VerifyPortfolioSubmitted(string courseName)
        {
            Element submittedIcon = new Element("Submitted Icon", By.XPath("//div[@class='portfolio-snipet-boxed clearfix' and .//a[text()='" + courseName + "']]//div[@class='dark-box' and contains(.,'Submitted')]"));
            submittedIcon.Verify.Visible();
            return this;
        }

        public EditPortfolioPage EditPortfolio(string title)
        {
            Element link = new Element("Course Title Link",By.XPath("//a[contains(@href,'edit-portfolio') and text()='"+title+"']"));
            link.Click();
            return new EditPortfolioPage();
        }

        public PreviewPortfolioPage PreviewPortfolio(string title)
        {
            Element link = new Element("Course Title Link", By.LinkText(title));
            link.Click();
            return new PreviewPortfolioPage();
        }

        public PaymentPage StartAnotherPortfolio()
        {
            StartAnotherPortfolio_Button.Click();
            ConfirmButton.Click();
            return new PaymentPage();
        }

        public SetupPortfolioPage GetStarted()
        {
            GetStarted_Link.Click();
            return new SetupPortfolioPage();
        }

        public override void WaitForElements()
        {
            PortfolioTitle_Text.Verify.Visible();
        }
    }
}
