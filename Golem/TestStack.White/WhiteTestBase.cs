using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Gallio.Framework;
using TestStack.White;
using ProtoTest.Golem.Core;

namespace Golem.TestStack.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application _app;

        [SetUp]
        public void LaunchApp()
        {
            _app = Application.Launch("C:\\Program Files\\Quest Integrity Group\\LifeQuest Pipeline\\LifeQuest.exe");
        }

        

    }
}
