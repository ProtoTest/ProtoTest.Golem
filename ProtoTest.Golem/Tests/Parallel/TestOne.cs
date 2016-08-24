using System.Configuration;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using Golem.Core;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
{
    [Parallelizable]
    internal class ParallelTestOne : WebDriverTestBase
    {
  
        [Test]
        public void TestOne()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            //Assert.Fail("Failing");
        }

        [Test]
        public void TestTwo()
        {
            driver.Navigate().GoToUrl("http://www.gmail.com");
        }
        
    }
}