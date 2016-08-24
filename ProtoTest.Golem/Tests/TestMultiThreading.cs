using NUnit.Framework;
using Golem.Core;
using Golem.Tests.PageObjects.Google;
using Golem.WebDriver;

namespace Golem.Tests
{
    [Parallelizable]
    internal class TestMultiThreading : WebDriverTestBase
    {
        [Test]
        public void TestThreadedRepeat1()
        {
            OpenPage<GoogleHomePage>("http://www.google.com");
        }

        [Test, Parallelizable]
        public void TestThreadedRepeat2()
        {
            OpenPage<GoogleHomePage>("http://www.google.com");
        }

        [Test, Parallelizable]
        public void TestThreadedRepeat3()
        {
            OpenPage<GoogleHomePage>("http://www.google.com");
        }

        [Test, Parallelizable]
        public void TestThreadedRepea4t()
        {
            OpenPage<GoogleHomePage>("http://www.google.com");
        }

        [Test, Parallelizable]
        public void TestThreadedRepeat5()
        {
            OpenPage<GoogleHomePage>("http://www.google.com");
        }
    }
}