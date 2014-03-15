using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Logging;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.PurpleElements;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.Purple.Elements;
using RestSharp.Extensions;


namespace ProtoTest.Golem.Purple
{
    public class PurpleTestBase : TestBase
    {
        
        [FixtureInitializer]
        public void WhiteSettings()
        {
            
        }


        [SetUp]
        public void SetUp()
        {
            PurpleWindow.FindRunningProcess();
        }

        

        [TearDown]
        public void TearDown()
        {
            
            //TestLog.WriteLine(CoreAppXmlConfiguration.Instance.WorkSessionLocation.ToString());
            LogScreenshotIfTestFailed();
            //app.Close();
            //app.ApplicationSession.Save();
        }

        public void LogScreenshotIfTestFailed()
        {
            if(TestContext.CurrentContext.Outcome!=TestOutcome.Passed)
                TestLog.EmbedImage(null, PurpleWindow.purpleWindow.GetImage());
        }
    }
}