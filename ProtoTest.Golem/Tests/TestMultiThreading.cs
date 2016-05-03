using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    [Parallelizable]
    internal class TestMultiThreading : WebDriverTestBase
    {
        [Test, Parallelizable]
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