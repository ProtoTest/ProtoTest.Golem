using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Golem.White.ScreenObjects.LQP;
using ProtoTest.Golem.Core;


namespace Golem.White.Tests.LQP
{
    public class Test1 : WhiteTestBase
    {
        [FixtureInitializer]
        public void Setup()
        {
            Config.Settings.whiteSettings.appPath =
                "C:\\Program Files\\Quest Integrity Group\\LifeQuest Pipeline\\LifeQuest.exe";
        }
    
        [Test]
        public void PleaseWorkTest()
        {

            AboutScreen.StartScreen().clickOkButton().CloseSplashScreen().OpenProject("TestDataImport.prj");
        }
    }
}
