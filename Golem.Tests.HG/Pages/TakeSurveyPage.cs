using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using ProtoTest.Golem.WebDriver;

namespace Golem.Tests.HG.Pages
{
    public class TakeSurveyPage : WebDriverTestBase
    {
        
   
        public Element GetElementForRowWithText(string text)
        {
            return new Element("ElementForRowWithText", By.XPath("//tr[./td[contains(@class,'surveyRow') and contains(text(),'" + text + "')]]"));
        }

        public TakeSurveyPage SetRadioValueForElement(Element parent, string value)
        {
            parent.FindElement(By.XPath("//*[@title='"+value+"']")).Click();
            return this;
        }

        public TakeSurveyPage SetStarRatingForRowWithText(string text, int starRating)
        {
            By starLocator;
            switch (starRating)
            {
                case 1 :
                    starLocator = By.XPath("//span[@class='one starHit']");
                    break;
                case 2:
                    starLocator = By.XPath("//span[@class='two starHit']");
                    break;
                case 3:
                    starLocator = By.XPath("//span[@class='three starHit']");
                    break;
                case 4:
                    starLocator = By.XPath("//span[@class='four starHit']");
                    break;
                case 5:
                    starLocator = By.XPath("//span[@class='five starHit']");
                    break;
                default:
                    starLocator = By.XPath("//span[@class='one starHit']");
                    break;
            }
            GetElementForRowWithText(text).WaitUntil().Present().FindElement(starLocator).Click();
            return this;
        }

        public TakeSurveyPage  SetRowSlider(string text)
        {
            var container = driver.FindElement(By.XPath("//div[contains(@data-question-id,'226')]/div"));
            var handle = driver.FindElement(By.XPath("//div[contains(@data-question-id,'226')]/a"));

            driver.ExecuteJavaScript("arguments[0].style.width='40%';return;", container);
            driver.ExecuteJavaScript("arguments[0].style.width='40%';return;", handle);
            handle.Click();
            return this;
        }


    }
}
