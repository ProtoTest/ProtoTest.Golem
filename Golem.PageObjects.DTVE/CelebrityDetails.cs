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
        #region Page Elements
        public Element CelebrityName = new Element("CelebrityName",
            By.XPath("//div[@class='col-sm-12 col-lg-10']/h2[contains(@class,'details-title')]"));
        public Element CelebrityBirthDate = new Element("CelebrityBirthDate",
            By.XPath("//div[@class='details-meta row']//dd[1]"));
        public Element CelebrityBirthPlace = new Element("CelebrityBirthPlace",
            By.XPath("//div[@class='details-meta row']//dd[1]"));
        public Element CelebrityDeathDate = new Element("CelebrityDeathDate",
            By.XPath("//div[@class='col-sm-12 col-lg-10']/h2[contains(@class,'details-title')]"));
        public Element CelebrityImage = new Element("CelebrityImage",
            By.XPath("//div[@class='details-poster-primary col-sm-12 col-lg-12']"));
        public Element CelebrityShowsTab = new Element("CelebrityShowsTab",
            By.Id("shows-tab"));
        public Element CelebrityAwardsTab = new Element("CelebrityAwardsTab",
            By.Id("awards-tab"));
        #endregion

        public override void WaitForElements()
        {
            CelebrityName.Verify().Visible();
            CelebrityImage.Verify().Visible();
            CelebrityShowsTab.Verify().Visible();
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

        public CelebrityDetails VerifyCelebrityDetails(
            string celebrityNameData,
            string celebrityBirthDateData,
            string celebrityBirthPlaceData,
            string celebrityDeathDateData)
        {
            CelebrityName.Verify().Text(celebrityNameData);
            CelebrityBirthDate.Verify().Text(celebrityBirthDateData);
            CelebrityBirthPlace.Verify().Text(celebrityBirthPlaceData);
            CelebrityDeathDate.Verify().Text(celebrityDeathDateData);
            return new CelebrityDetails();
        }

        public CelebrityDetails VerifyCelebrityImage(string celebrityFullNameData)
        {
            CelebrityImage = new Element("CelebrityImage for" + celebrityFullNameData,
                By.XPath("//div[@class='details-poster-primary col-sm-12 col-lg-12']"));
            CelebrityImage.Verify().Image();
            return new CelebrityDetails();
        }

        public static CelebrityDetails OpenPage(string url)
        {
            WebDriverTestBase.driver.Navigate().GoToUrl(url);
            return new CelebrityDetails();
        }
    }
}