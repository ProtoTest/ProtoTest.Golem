using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// BasePageObject should be inherited by all page objects used in the framework.  It represents either a base component or page in an application.  You must implement void WaitForElements(), which should contain checks for ajax elements being present/not present.  It contains a static reference to the WebDriverTestBase.driver object
    /// </summary>
    public abstract class BasePageObject
    {
        public string className;
        public string currentMethod;
        public string url;

        public BasePageObject()
        {
            driver = WebDriverTestBase.driver;
            className = GetType().Name;
            if (Config.Settings.runTimeSettings.AutoWaitForElements)
            {
                WaitForElements();
            }
            TestBase.testData.actions.addAction(TestBase.GetCurrentClassAndMethodName());
        }

        public BasePageObject(IWebDriver driver)
        {
            this.driver = driver;
            className = GetType().Name;
            if (Config.Settings.runTimeSettings.AutoWaitForElements)
            {
                WaitForElements();
            }
            
            TestBase.testData.actions.addAction(TestBase.GetCurrentClassAndMethodName());
        }

        public IWebDriver driver { get; set; }


        public abstract void WaitForElements();
    }
}