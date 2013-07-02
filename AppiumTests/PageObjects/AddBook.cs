using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace AppiumTests.PageObjects
{
    class AddBook : BasePageObject 
    {
        public override void WaitForElements()
        {
            throw new NotImplementedException();
        }
        public void AddBooks()
        {

            var buttons = driver.FindElements(By.TagName("ImageView"));
            buttons[3].Click();

            driver.FindElement(By.Name("Title")).SendKeys("Title text");
            driver.FindElement(By.Name("Subtitle")).SendKeys("Subtitle text");
            driver.FindElement(By.Name("ISBN 10")).SendKeys("ISBN text");
            driver.FindElement(By.Name("Author(s) (comma separated)")).SendKeys("Author text");
            driver.FindElement(By.Name("Subject")).SendKeys("Subject text");
            driver.FindElement(By.Name("Publisher")).SendKeys("Publisher text");

            // IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            // IDictionary<String, Double> swipeObject = new Dictionary<string, double>();
            // swipeObject.Add("endX", 0.5);
            // swipeObject.Add("endY", .1);
            // swipeObject.Add("touchCount", 2);
            ////js.ExecuteScript("mobile: flick", swipeObject);
            // RemoteTouchScreen touch = new RemoteTouchScreen(driver);
            // touch.Flick(10,10);

            driver.FindElement(By.Name("Save")).Click();
        }
    }
}
