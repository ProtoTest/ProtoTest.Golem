using Gallio.Framework;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.White.Elements;
using TestStack.White;

namespace ProtoTest.Golem.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application app { get; set; }
        public static WhiteWindow _window;

        public static WhiteWindow window
        {
            get
            {
                return _window;
            }

            set
            {
                _window = value;
            }
        }

        [SetUp]
        public void LaunchApp()
        {
            app = Application.Launch(Config.Settings.whiteSettings.appPath);
        }

        [TearDown]
        public void CloseApplication()
        {
            TestLog.EmbedImage(null, app.GetImage());
  
            app.Close();
  
        }
    }
}