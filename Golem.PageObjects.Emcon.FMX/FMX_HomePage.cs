using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Emcon.FMX
{
    public class FMX_HomePage : FmxMenuBar
    {
        Element MessageBoard = new Element("MessageBoard", By.ClassName("MainInfoCollapsibleHeaderText"));

        public new void WaitForElements()
        {
            base.WaitForElements();
            MessageBoard.VerifyVisible(10);
        }

        public FMX_HomePage VerifyResult(string text)
        {
            var HomePageVerified = new Element("HomePageVerified", By.PartialLinkText(text));
            HomePageVerified.VerifyPresent();
            return new FMX_HomePage();
        }

        
    }
}
