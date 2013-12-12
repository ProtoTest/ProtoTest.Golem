using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Gallio.Framework;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using ProtoTest.Golem.Core;

namespace Golem.TestStack.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application _app;
        public static Window window;

        [SetUp]
        public void LaunchApp()
        {
            _app = Application.Launch("C:\\Program Files\\Quest Integrity Group\\LifeQuest Pipeline\\LifeQuest.exe");
        }

        [TearDown]
        public void CloseApplication()
        {
            if (_app != null) _app.Kill();
        }

        

    }
}
