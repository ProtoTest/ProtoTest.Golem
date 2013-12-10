using System;
using System.Collections.Generic;
using System.Reflection;
using Golem.Properties;
using Golem.WebDriver;
using NHunspell;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements;

namespace ProtoTest.Golem.WebDriver
{
    public abstract class BasePageObject
    {


        public string className;
        public string currentMethod;
        public string url;

        public BasePageObject()
        {
            driver = WebDriverTestBase.driver;
            className = GetType().Name;
            WaitForElements();
            //TestBase.testData.LogEvent(TestBase.GetCurrentClassAndMethodName() + " Finished");
            TestBase.testData.actions.addAction(TestBase.GetCurrentClassAndMethodName());

            //Check for misspellings feature
            if (Config.Settings.reportSettings.spellChecking)
            {
                new Spellchecker().VerifyNoMisspelledWords();
            }
        }

        public IWebDriver driver { get; set; }

      
        public abstract void WaitForElements();
    }
}