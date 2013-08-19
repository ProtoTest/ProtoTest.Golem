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
        public void Test()
        {
            OpenPage<LandingPage>("http://int-dtve.directv.com/browse").
                ShowAllFilters().
                SelectFilter("Comedy").
                VerifyNumberOfResult("1,984 Results");
            
        }


    }
}
