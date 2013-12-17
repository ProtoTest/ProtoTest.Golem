using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    public class TestBasePageObject : WebDriverTestBase
    {

        [Test]
        public void TestWaitForElements()
        {
            var page = OpenPage<GoogleHomePage>("http://www.google.com");
        }

        [Test]
        public void TestPageObjectHasDriver()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            var page = new GoogleHomePage();
            Assert.IsNotNull(page.driver);
        }
    }
}
