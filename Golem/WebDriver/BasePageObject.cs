using System;
using System.Collections.Generic;
using System.Reflection;
using Golem.Properties;
using NHunspell;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements;

namespace ProtoTest.Golem.WebDriver
{
    public abstract class BasePageObject
    {
        private Element AllText;
        private List<string> Misspellings;
        private Hunspell SpellChecker;


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
                AllText = new Element("AllText", By.XPath("//html"));
                InitSpellChecker();
                Misspellings = MispelledWords(AllText.Text);
            }
        }

        protected IWebDriver driver { get; set; }

        public void InitSpellChecker()
        {
            if (SpellChecker == null)
            {

                SpellChecker = new Hunspell(Resources.en_US, Resources.en_US_aff);
            }
        }

        public void AddCustomWords(string word)
        {
            InitSpellChecker();
            SpellChecker.Add(word);
        }

        public void AddCustomWords(List<string> words)
        {
            InitSpellChecker();
            for (int i = 0; i < words.Count; i++)
            {
                SpellChecker.Add(words[i]);
            }
        }

        public List<string> MispelledWords(string unscrubbedText)
        {
            var mispelledWords = new List<string>();

            string[] ScrubbedText = unscrubbedText.Split(new[] {"\n", "\r\n", "\\", " ", ",", ".", "!", "?"},
                StringSplitOptions.RemoveEmptyEntries);
            if (ScrubbedText != null)
            {
                for (int i = 0; i < ScrubbedText.Length; i++)
                {
                    if (!SpellChecker.Spell(ScrubbedText[i]))
                    {
                        if (ScrubbedText[i].Length > 1)
                        {
                            mispelledWords.Add(ScrubbedText[i]);
                        }
                    }
                }
            }

            return mispelledWords;
        }


        public abstract void WaitForElements();
    }
}