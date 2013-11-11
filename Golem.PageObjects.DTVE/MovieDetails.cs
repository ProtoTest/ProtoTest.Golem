using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.WebDriver.Elements.Types;
using OpenQA.Selenium;

namespace Golem.PageObjects.DTVE
{
    public class MovieDetails : BasePageObject
    {
        public string id;
        public MovieDetails(string id)
        {
            this.id = id;
        }

        public Header Header = new Header();
        public Footer Footer = new Footer();
        public Panel Title = new Panel("Title", By.ClassName("details-title"));
        public Image Poster = new Image("Poster", ByE.PartialAttribute("div", "@class", "details-poster-primary"));
        public Button Watch = new Button("Watch", By.LinkText("Watch"));
        public Button Record = new Button("Record", By.LinkText("Record"));
        public Button Close = new Button("Close", ByE.PartialAttribute("div", "@class", "details-overlay-close"));
        public Button Share = new Button("Share", By.LinkText("Share"));
        public Button Playlist = new Button("Playlist", By.LinkText("Playlist"));
        public Button Trailer = new Button("Trailer", By.LinkText("Trailer"));

        public Panel Duration = new Panel("Duration", By.XPath("//div[contains(@class,'details-meta')]//dl[1]/dd[2]"));
        public Panel Rating = new Panel("Rating", By.XPath("//div[contains(@class,'details-meta')]//dl[1]/dd[4]"));
        public Panel Year = new Panel("Duration", By.XPath("//div[contains(@class,'details-meta')]//dl[1]/dd[6]"));
        public Panel Genre = new Panel("Duration", By.XPath("//div[contains(@class,'details-meta')]//dl[1]/dd[8]"));

        public Panel Descripton = new Panel("Description", By.XPath("//div[contains(@class,'details-meta')]//p"));
        public Link ReadMore = new Link("Read More", By.LinkText("Read More"));
        public Link ReadLess = new Link("Read Less", By.LinkText("Read Less"));
        public Image Actor = new Image("Actor",By.ClassName("poster"));

        public CelebrityDetails ClickCelebrity(string id)
        {
            Link celebrity = new Link("Celebrity",ByE.PartialAttribute("a","@personid",id));
            celebrity.Click();
            return new CelebrityDetails(id);
        }

        public override void WaitForElements()
        {
            Poster.Verify().Visible();
            Share.Verify().Visible();
            Playlist.Verify().Visible();
            Trailer.Verify().Visible();
            Watch.Verify().Visible();
            Record.Verify().Visible();
            Title.Verify().Visible();
            Descripton.Verify().Visible();
            Actor.Verify().Visible();

        }

        public void VerifyPageLoaded()
        {
            
        }
    }
}
