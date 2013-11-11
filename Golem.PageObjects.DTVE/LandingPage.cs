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
    public class LandingPage : BasePageObject
    {
        public Panel FiltersMenu = new Panel("Filters Menu", By.ClassName("filters-menu"));
        public Button ToggleFilterButton = new Button("ToggleFilter", ByE.PartialAttribute("button", "@data-test-id", "button_toggle_filters"));
        public Header Header = new Header();
        public Footer Footer = new Footer();

        public override void WaitForElements()
        {
            ToggleFilterButton.Verify().Visible();
        }

        public LandingPage ToggleFilter()
        {
            ToggleFilterButton.Click();
            return new LandingPage();
        }

        public LandingPage VerifyFiltersShown()
        {

            FiltersMenu.Verify().Visible();
            return this;
        }

        public LandingPage VerifyFiltersHidden()
        {
             FiltersMenu.Verify().Not().Visible();
            return this;
        }

        public MovieDetails ClickMoviePoster(string id)
        {
            Image movie = new Image("Movie",ByE.PartialAttribute("div","@id",id));
            movie.WaitUntil().Visible().ScrollIntoView().Click();
            return new MovieDetails(id);
        }

    }
}
