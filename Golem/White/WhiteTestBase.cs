using MbUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace Golem.White
{
    public class WhiteTestBase 
    {
        public static Application _app;
        public static Window window;

        [SetUp]
        public void LaunchApp()
        {
            _app = Application.Launch("C:\\Program Files\\Quest Integrity Group\\LifeQuest Pipeline\\LifeQuest.exe");
        }

        //[TearDown]
        //public void CloseApplication()
        //{
        //    if (_app != null) _app.Kill();
        //}

        

    }
}
