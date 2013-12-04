using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Tests.HG.Pages;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace Golem.Tests.HG
{
    public class Class1 : WebDriverTestBase
    {
        [Test]
        public void test()
        {
            PatientRatings.OpenPage()
                .TakeTheSurvey()
                .SetRowSlider("Total wait time");
        }
    }
}
