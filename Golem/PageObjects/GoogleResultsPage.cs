using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Golem
{
    public class GoogleResultsPage : BasePageObject
    {
        Element GoogleLogo = new Element("GoogleLogo", By.ClassName("gbqlca"));
        Element SearchField = new Element("SearchField", By.Name("q"));
        Element SearchButton = new Element("SearchButton", By.Name("btnK"));
        Element SignInButton = new Element("SignInButton", By.ClassName("gbit"));
        public Element SearchResult(string text)
        {
            return new Element("SearchResultLink", By.PartialLinkText(text));
        }

        private string searchString;

        public GoogleResultsPage(string searchString)
        {
            this.searchString = searchString;
        }
        
        public GoogleResultsPage SearchFor(string text)
        {
            SearchField.SendKeys(text);
            SearchField.Submit();
            return new GoogleResultsPage();
        }

        public override void WaitForElements()
        {
           // TestBaseClass.FireEvent("ResultsPage WaitForELements");
            SearchField.WaitForVisible();
            GoogleLogo.WaitForVisible();
            SearchButton.WaitForVisible();
            SignInButton.WaitForVisible();
        }

        public override void ValidateLoaded()
        {
            //TestBaseClass.FireEvent("ResultsPage validateLoaded");
        }


    }
}
