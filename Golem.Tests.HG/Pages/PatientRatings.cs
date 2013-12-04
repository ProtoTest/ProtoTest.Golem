using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace Golem.Tests.HG.Pages
{
    public class PatientRatings : WebDriverTestBase
    {
        public Element TakeTheSurveyButton = new Element("TakeTheSurveyButton",By.LinkText("Take the Survey"));
        public static PatientRatings OpenPage()
        {
            WebDriverTestBase.driver.Navigate().GoToUrl(@"http://www.healthgrades.com/physician/dr-michael-porter-y8b4m/patient-ratings");
            return new PatientRatings();
        }

        public TakeSurveyPage TakeTheSurvey()
        {
            TakeTheSurveyButton.Click();
            return new TakeSurveyPage();
        }
    }
}
