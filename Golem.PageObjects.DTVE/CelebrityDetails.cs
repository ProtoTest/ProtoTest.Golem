using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace Golem.PageObjects.DTVE
{
    public class CelebrityDetails : BasePageObject
    {
        #region List of All Page Elements
        public Element CelebrityName = new Element("CelebrityName",
            By.XPath("//div[@class='col-sm-12 col-lg-10']/h2[contains(@class,'details-title')]"));
        public Element CelebrityBirthDate = new Element("CelebrityBirthDate",
            By.XPath("//div[@class='details-meta row']//dd[1]"));
        public Element CelebrityBirthPlace = new Element("CelebrityBirthPlace",
            By.XPath("//div[@class='details-meta row']//dd[2]"));
        public Element CelebrityDeathDate = new Element("CelebrityDeathDate",
            By.XPath("//div[@class='details-meta row']//dd[3]"));
        public Element CelebrityImage = new Element("CelebrityImage",
            By.XPath("//div[@class='details-poster-primary col-sm-12 col-lg-12']"));
        public Element CelebrityShowsTab = new Element("CelebrityShowsTab",
            By.Id("shows-tab"));
        public Element CelebrityAwardsTab = new Element("CelebrityAwardsTab",
            By.Id("awards-tab"));
        public Element CelebrityShowsList = new Element("CelebrityShowsList",
            ByE.PartialAttribute("span", "@class", "text-overflow ng-binding"));
        public Element ShowsListFirstItem = new Element("ShowsListFirstItem",
            By.XPath("//div[@class='panel-group']/div[1]//div[@class='item-title-left']"));
        //expanded list locators
        #endregion

        //ACTIONS

        //Opens Environment URL
        public static CelebrityDetails OpenPage(string url)
        {
            WebDriverTestBase.driver.Navigate().GoToUrl(url);
            WebDriverTestBase.driver.ExecuteJavaScript("document.getElementById(\"topnav-mobile-account-menu\").style.visibility='hidden';return;");
            return new CelebrityDetails();
        }
        
        public CelebrityDetails ClickAwardsTab()
        {
            CelebrityAwardsTab.Click();
            return new CelebrityDetails();
        }

        public CelebrityDetails ClickShowsTab()
        {
            CelebrityShowsTab.Click();
            return new CelebrityDetails();
        }

 

    //desktop
        public CelebrityDetails ExpandShowsListFirstItem()
        {
            ShowsListFirstItem.Click();
            return new CelebrityDetails();
        }

        public CelebrityDetails VerifyExpandedListData()
        {
            //expanded list locators
            return this;
        }

        //mobile
        public MoviesDetails TapShowsListFirstItem()
        {
            ShowsListFirstItem.Click();
            return new MoviesDetails();
        }

        //VERIFICATIONS

        //Verifications on page for every page load
        public override void WaitForElements()
        {
            CelebrityName.Verify().Present();
            CelebrityImage.Verify().Present();
            CelebrityShowsTab.Verify().Present();
        }

        //Verifications for Celebrity API data (currently hardcoded)
        public CelebrityDetails VerifyCelebrityDetails(
            string celebrityNameData = "James Gandolfini",
            string celebrityBirthDateData = "September 18, 1961",
            string celebrityBirthPlaceData = "Westwood, New Jersey",
            string celebrityDeathDateData = "June 19, 2013")
        {
            CelebrityName.Verify().Text(celebrityNameData);
            CelebrityBirthDate.Verify().Text(celebrityBirthDateData);
            CelebrityBirthPlace.Verify().Text(celebrityBirthPlaceData);
            CelebrityDeathDate.Verify().Text(celebrityDeathDateData);
            return this;
        }

        //Verify Celebrity Image (API integration in progress)
        public CelebrityDetails VerifyCelebrityImage(string celebrityFullNameData)
        {
            CelebrityImage = new Element("CelebrityImage for" + celebrityFullNameData,
                By.XPath("//div[@class='details-poster-primary col-sm-12 col-lg-12']"));
            CelebrityImage.Verify().Image();
            return this;
        }
        
        //Verify Celebrity Shows List
        public CelebrityDetails VerifyCelebrityShowsList(List<string> celebrityShowsListData)
        {
            foreach (var Show in celebrityShowsListData)
            {
                Element CelebrityShowsText = new Element("CelebrityShowsText", By.XPath("//span[@class='text-overflow ng-binding' and contains(.,'"+Show+"')]"));
                CelebrityShowsText.Verify().Visible();
            }
            return this;
        }

        //Verify Celebrity Awards List
        public CelebrityDetails VerifyCelebrityAwardsList(List<string> celebrityAwardsListData)
        {
            foreach (var Award in celebrityAwardsListData)
            {
                Element CelebrityAwardsText = new Element("CelebrityAwardsText", By.XPath("//div[@id='celebrity-awards']//li[contains(.,'"+Award+"')]"));
                CelebrityAwardsText.Verify().Visible();
            }
            return this;
        }
    }
}