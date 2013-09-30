using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
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
            HowItWorks_link.VerifyVisible(30);
            CaelLogo_Link.VerifyVisible(30);
            EmailAddress_Link.VerifyVisible(30);
            SignIn_Link.VerifyVisible(30);
            Facebook_Link.VerifyVisible(30);
        }
    }
}
