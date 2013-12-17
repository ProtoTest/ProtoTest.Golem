using System;
using System.Collections.Generic;
using NHunspell;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Properties;

namespace ProtoTest.Golem.WebDriver
{
    internal class Spellchecker
    {
        private readonly Element AllText = new Element("AllText", By.XPath("//html"));
        private readonly List<string> Misspellings;
        private readonly Hunspell huunspell;

        public Spellchecker()
        {
            Misspellings = MispelledWords(AllText.Text);

            if (huunspell == null)
            {
                huunspell = new Hunspell(Resources.en_US, Resources.en_US_aff);
            }
        }

        public void VerifyNoMisspelledWords()
        {
            if (Misspellings.Count > 0)
            {
                string allwords = "";
                foreach (string miss in Misspellings)
                {
                    allwords += miss + ", ";
                }
                TestBase.AddVerificationError(Misspellings.Count + " Spelling Errors found on page : " + allwords);
            }
        }

        public void AddCustomWords(string word)
        {
            huunspell.Add(word);
        }

        public void AddCustomWords(List<string> words)
        {
            for (int i = 0; i < words.Count; i++)
            {
                huunspell.Add(words[i]);
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
                    if (!huunspell.Spell(ScrubbedText[i]))
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
    }
}