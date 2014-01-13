using System.Diagnostics;
using System.Linq;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Logging;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.White.Elements;
using RestSharp.Extensions;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application app { get; set; }


        public static WhiteWindow window { get; set; }

        [FixtureInitializer]
        public void WhiteSettings()
        {

            //CoreAppXmlConfiguration.Instance.RawElementBasedSearch = false;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 4;
           // CoreAppXmlConfiguration.Instance.LoggerFactory.Create("WhiteDefaultLogger",LoggerLevel.Info);
        }


        [SetUp]
        public void SetUp()
        {
            app = Application.Launch(Config.Settings.whiteSettings.appPath);
            if(Config.Settings.whiteSettings.windowTitle!="NOT_SET")
                 window = new WhiteWindow(Config.Settings.whiteSettings.windowTitle);
        }

        [TearDown]
        public void TearDown()
        {
            
            //TestLog.WriteLine(CoreAppXmlConfiguration.Instance.WorkSessionLocation.ToString());
            LogScreenshotIfTestFailed();
            app.Close();
            //app.ApplicationSession.Save();
        }

        public void LogScreenshotIfTestFailed()
        {
            if(TestContext.CurrentContext.Outcome!=TestOutcome.Passed)
                TestLog.EmbedImage(null, app.GetImage());
        }
    }
}