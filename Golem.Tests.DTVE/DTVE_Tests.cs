using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using MbUnit.Framework;
using Gallio.Framework;
using OpenQA.Selenium;
using Golem.PageObjects.DTVE;

namespace Golem.Tests.DTVE
{
    public class DTVE_Tests : TestBaseClass
    {
        [Test]
        public void TestFilters()
        {

            OpenPage<LandingPage>("http://int-dtve.directv.com/browse").
                VerifyFiltersHidden().
                ToggleFilter().
                VerifyFiltersShown().
                ToggleFilter().
                VerifyFiltersHidden();

        }

        [Test]
        public void TestNavigation()
        {
            OpenPage<LandingPage>("http://int-dtve.directv.com/browse").
                ClickMoviePoster("MV004267570000").
                ClickCelebrity("290995");
        }
    }
}
