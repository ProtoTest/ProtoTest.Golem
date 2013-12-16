using MbUnit.Framework;
using ProtoTest.Golem.Core;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace Golem.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application _app;
        public static Window window;

        [SetUp]
        public void LaunchApp()
        {
            _app = Application.Launch(Config.Settings.whiteSettings.appPath);
        }

        //[TearDown]
        //public void CloseApplication()
        //{
        //    if (_app != null) _app.Kill();
        //}

        

    }
}
