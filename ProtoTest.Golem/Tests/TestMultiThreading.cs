using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Tests.PageObjects.Google;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    internal class TestMultiThreading : WebDriverTestBase
    {
        [FixtureInitializer]
        public void setup()
        {
            Config.Settings.runTimeSettings.DegreeOfParallelism = 5;
        }

        [Test, Parallelizable, ThreadedRepeat(5)]
        public void TestThreadedRepeat()
        {
            OpenPage<GoogleHomePage>("http://www.google.com");
            //    Assert.AreEqual(testDataCollection.Count,5);
        }
    }
}