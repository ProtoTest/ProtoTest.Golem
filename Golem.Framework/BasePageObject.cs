using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using System.Diagnostics;
using NHunspell;

namespace Golem.Framework
{
    public abstract class BasePageObject 
    {
        private IWebDriver _driver;
        private Element AllText;
        private Hunspell SpellChecker;
        private List<string> Misspellings;
        

        protected IWebDriver driver
        {
            get
            {
                return _driver;
            }
            set
            {
                _driver = value;
            }

        }

        
        public string url;
        public string className;
        public string currentMethod;
        public BasePageObject()
        {
            driver = TestBaseClass.driver;
            className = this.GetType().Name;
            WaitForElements();
            //TestBaseClass.testData.LogEvent(Common.GetCurrentClassAndMethodName() + " Finished");
            TestBaseClass.testData.actions.addAction(Common.GetCurrentClassAndMethodName());
            
            //Check for misspellings feature
            if (Config.Settings.reportSettings.spellChecking)
            {
                AllText = new Element("AllText", By.XPath("//html"));
                InitSpellChecker()
                Misspellings = MispelledWords(AllText.Text);
            }
        }

        public void InitSpellChecker()
        {
            if (SpellChecker == null)
            {
                SpellChecker = new Hunspell(Golem.Framework.Properties.Resources.en_US_aff, Golem.Framework.Properties.Resources.en_US);
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
            List<string> mispelledWords = new List<string>();

            string[] ScrubbedText = unscrubbedText.Split(new string[] { "\n", "\r\n", "\\", " ", ",", ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
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
