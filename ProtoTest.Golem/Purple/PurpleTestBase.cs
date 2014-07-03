using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.PurpleElements;


namespace ProtoTest.Golem.Purple
{
    public class PurpleTestBase : TestBase
    {
        
        
        public void TestSettings()
        {
            
        }

        [NUnit.Framework.SetUp]
        [MbUnit.Framework.SetUp]
        public void SetUp()
        {
            PurpleWindow.FindRunningProcess();
        }


        [NUnit.Framework.TearDown]
        [MbUnit.Framework.TearDown]
        public void TearDown()
        {
            LogScreenshotIfTestFailed();
            //PurpleWindow.EndProcess();
        }

        public void LogScreenshotIfTestFailed()
        {
            //Will need to rework logscreenshot if failed for NUnit
            //if(TestContext.CurrentContext.Outcome!=TestOutcome.Passed)
            //    TestLog.EmbedImage(null, PurpleWindow.purpleWindow.GetImage());
        }
    }
}