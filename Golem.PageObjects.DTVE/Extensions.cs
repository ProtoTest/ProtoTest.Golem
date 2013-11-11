using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;

namespace Golem.PageObjects.DTVE
{
    public static class Extensions
    {
        public static By DataTestId(this ByE bye, string value)
        {
            return By.XPath(string.Format("//*[@data-test-id='{0}']", value));
        }
    }
}
