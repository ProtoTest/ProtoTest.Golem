using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gallio.Framework.Pattern;
using Golem.PageObjects.Google;
using MbUnit.Framework;
using OpenQA.Selenium.Firefox;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace Golem.Tests
{
    class TestMultiThreading : WebDriverTestBase
    {
        [FixtureInitializer]
        public void setup()
        {
            Config.Settings.runTimeSettings.DegreeOfParallelism = 10;
        }
      [Test, Parallelizable,ThreadedRepeat(10)]
        public void TestThreadedRepeat()
        {
          OpenPage<GoogleHomePage>("http://www.google.com");
          Assert.AreEqual(testDataCollection.Count,10);
        }
   
    }
}
