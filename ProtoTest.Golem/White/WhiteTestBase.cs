using Gallio.Framework;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application app { get; set; }


        public static Window window { get; set; }

        [SetUp]
        public void LaunchApp()
        {
            app = Application.Launch(Config.Settings.whiteSettings.appPath);
        }

        [TearDown]
        public void CloseApplication()
        {
            TestLog.EmbedImage(null, app.GetImage());
            app.ApplicationSession.Save();
        }
    }
}