using System.Diagnostics;
using Castle.Core.Logging;
using Gallio.Framework;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.White.Elements;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
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
<<<<<<< HEAD
            //CoreAppXmlConfiguration.Instance.RawElementBasedSearch = false;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 4;
            CoreAppXmlConfiguration.Instance.LoggerFactory = new WhiteDefaultLoggerFactory(LoggerLevel.Debug);
            
=======
            get
            {
                return _window;
            }

            set
            {
                _window = value;
            }
>>>>>>> c9941934e531306f953c2413d2ba260e60c3c414
        }


        [SetUp]
        public void LaunchApp()
        {
            app = Application.Launch(Config.Settings.whiteSettings.appPath);
            //app.ApplicationSession.WindowSession(InitializeOption.NoCache.AndIdentifiedBy("MainWindowX"));
        }

        [TearDown]
        public void CloseApplication()
        {
            TestLog.EmbedImage(null, app.GetImage());
            TestLog.WriteLine(CoreAppXmlConfiguration.Instance.WorkSessionLocation.ToString());
            app.Close();
            app.ApplicationSession.Save();
        }
    }
}