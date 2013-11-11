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
    public class Header : BasePageObject
    {
        public Link WatchDirecTV = new Link("Watch DirecTV link", By.LinkText("Watch DIRECTV"));
        public Link Movies = new Link("Movies Link",By.LinkText("Movies"));
        public Link TVShows = new Link("TV SHows link",By.LinkText("TV Shows"));
        public Link Sports = new Link("Sports Link",By.LinkText("Sports"));
        public Link Networks = new Link("Networks link",By.LinkText("Networks"));
        public Link Guide = new Link("Guide link",By.LinkText("Guide"));
        public Link Playlist = new Link("Playlist",By.LinkText("Playlist"));

        public Field SearchField = new Field("Search FIeld", By.Id("global_type_ahead_text"));
        public Link Logo = new Link("Logo link", By.Id("dtv_logo"));
        public Link GetDirecTV = new Link("Get DirecTV",By.LinkText("Get DIRECTV"));
        public Link Help = new Link("Help link",By.LinkText("Help"));
        public Link SignIn = new Link("Sign In",By.PartialLinkText("Sign In"));

        public override void WaitForElements()
        {
            WatchDirecTV.Verify().Visible();
            SearchField.Verify().Visible();
        }
    }
}
