using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    [Parallelizable]
    internal class ParallelTestThree : WebDriverTestBase
    {

        [Test]
        public void TestOne()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
        }

        [Test]
        public void TestTwo()
        {
            driver.Navigate().GoToUrl("http://www.gmail.com");
        }

    }
}