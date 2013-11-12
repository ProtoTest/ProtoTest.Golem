using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using MbUnit.Framework;
using Gallio.Framework;
using OpenQA.Selenium;
using Golem.PageObjects.DTVE;

namespace Golem.Tests.DTVE
{
    public class DTVE_Demo : WebDriverTestBase
    {
        [Test]
        public void TestFilters()
        {

            OpenPage<LandingPage>("http://int-dtve.directv.com/browse").
                ClickMoviePoster("posterTMSMV004267570000");
            //ClickCelebrity("29109");
            //CelebrityDetails().


        }
    }
}
