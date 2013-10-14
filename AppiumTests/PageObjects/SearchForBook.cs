using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Appium.Bookworm;
using OpenQA.Selenium;

namespace AppiumTests.PageObjects
{
    public class SearchForBook : BasePageObject
    {
        public Element txt_SearchField = new Element("SearchFIeld", By.TagName("EditText"));
        public Element btn_Search = new Element("SearchButton",By.Name("Search"));
        public Element btn_AddBook = new Element("AddBookButton",By.Name("Add book"));

        public override void WaitForElements()
        {
            txt_SearchField.Verify.Present();
        }

        public SearchForBook SearchFor(string title)
        {
            txt_SearchField.SendKeys(title);
            btn_Search.Click();
            return new SearchForBook();
        }

        public SearchForBook AddBookByTitle(string title)
        {
            driver.FindElement(By.Name(title)).Click();
            btn_AddBook.Click();
            return new SearchForBook();
        }

        public MenuPage GoBack()
        {
            driver.Navigate().Back();
            return new MenuPage();
        }

    }
}
