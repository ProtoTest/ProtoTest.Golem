using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Golem.White.ScreenObjects.LQP;
using Golem.TestStack.White;

namespace Golem.White.Tests.LQP
{
    public class Test1 : WhiteTestBase
    {
        
        [Test]
        public void PleaseWorkTest()
        {

            AboutScreen.clickOkButton();
        }
    }
}
