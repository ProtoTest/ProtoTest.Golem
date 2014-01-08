using Gallio.Framework;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application app { get; set; }


        public static Window window { get; set; }

        [FixtureInitializer]
        public void WhiteSettings()
        {
            CoreAppXmlConfiguration.Instance.RawElementBasedSearch = false;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 4;
            

        }


        [SetUp]
        public void LaunchApp()
        {
            app = Application.Launch(Config.Settings.whiteSettings.appPath);
            app.ApplicationSession.WindowSession(InitializeOption.NoCache.AndIdentifiedBy("MainWindowX"));
            

        }

        [TearDown]
        public void CloseApplication()
        {
            TestLog.EmbedImage(null, app.GetImage());
<<<<<<< HEAD
            TestLog.WriteLine(CoreAppXmlConfiguration.Instance.WorkSessionLocation.ToString());
            app.Close();
            app.ApplicationSession.Save();
            

=======
            app.ApplicationSession.Save();
>>>>>>> cd086b2336a9813c7caa158f4c7a9836d1e6c738
        }
    }
}