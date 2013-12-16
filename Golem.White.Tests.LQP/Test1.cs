using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Golem.White.ScreenObjects.LQP;


namespace Golem.White.Tests.LQP
{
    public class Test1 : WhiteTestBase
    {

    
        [Test]
        public void PleaseWorkTest()
        {

            AboutScreen.StartScreen().clickOkButton().CloseSplashScreen().OpenProject("TestDataImport.prj");
        }
    }
}
