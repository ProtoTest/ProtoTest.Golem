using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class Footer : BasePageObject
    {
        public Element HowItWorks_link = new Element("How it works link", By.LinkText("How it Works"));
        public Element CaelLogo_Link = new Element("Cael logo link", By.LinkText("CAEL"));
        public Element EmailAddress_Link = new Element("Email address link",By.LinkText("moreinfo@learningcounts.org"));
        public Element SignIn_Link = new Element("Sign In Link",By.LinkText("Sign In"));
        public Element Facebook_Link = new Element("Facebook link",By.LinkText("f"));

        public override void WaitForElements()
        {
            HowItWorks_link.Verify().Visible();
            CaelLogo_Link.Verify().Visible();
            EmailAddress_Link.Verify().Visible();
            SignIn_Link.Verify().Visible();
            Facebook_Link.Verify().Visible();
        }
    }
}
