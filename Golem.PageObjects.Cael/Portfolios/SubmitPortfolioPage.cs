
using Golem.PageObjects.Cael.Portfolios;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.WebDriver.Elements;

namespace Golem.PageObjects.Cael
{
    public class SubmitPortfolioPage : BasePageObject
    {
        public Element Generic_chk = new Element("Checkbox",By.XPath("//input[@type='checkbox']"));
        public Element EditPortfolioLink = new Element("Edit portfolio Link",By.LinkText("edit your portfolio"));
        public Element SubmitPortfolio_Button = new Element("Submit portfolio button", By.Id("submitPortfolioButton"));

        public SubmitPortfolioSuccessPage ConfirmAndSubmit()
        {
            var eles = driver.FindElements(By.XPath("//input[@type='checkbox']"));
            foreach (var ele in eles)
            {
                ele.Click();
            }
            SubmitPortfolio_Button.Click();
            return new SubmitPortfolioSuccessPage();
        }

        public override void WaitForElements()
        {
            SubmitPortfolio_Button.Verify().Visible();
            Generic_chk.Verify().Present();
        }
    }
}
