using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using Golem.Core;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
{
    internal class TestProxy : WebDriverTestBase
    {
        [Test]
        public void TestProxyNotNull()
        {
            Assert.IsNotNull(proxy);
        }

        [Test]
        public void TestProxyWorks()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/");
        }

        [Test]
        public void TestProxyFilter()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/");
            var entries = proxy.FilterEntries("www.google.com");
            Assert.AreEqual(10, entries.Count);
            Assert.AreEqual("http://www.google.com/", entries[0].Request.Url);
        }

        [Test]
        public void TestHTTPValidation()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/");
            proxy.VerifyRequestMade("http://www.google.com/");
        }

        [Test]
        public void TestResponseCodeValidation()
        {
            OpenPage<GoogleHomePage>("http://www.google.com/");
            proxy.VerifyNoErrorsCodes();
        }
    }
}