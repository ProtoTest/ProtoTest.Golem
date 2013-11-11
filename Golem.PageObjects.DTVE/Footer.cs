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
    public class Footer: BasePageObject
    {
        public Link ExploreDirecTV = new Link("ExploreDirecTV link",By.LinkText("Explore DIRECTV"));  
        public Link WereSocial = new Link(By.LinkText("We're Social"));
        public override void WaitForElements()
        {
            ExploreDirecTV.Verify().Visible();
        }
    }
}
