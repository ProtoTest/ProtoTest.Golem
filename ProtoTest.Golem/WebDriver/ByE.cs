using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// Contains By Extensions for the OpenQA.Selenium.By class
    /// </summary>
    public class ByE
    {
        public static By Text(string text)
        {
            return By.XPath("//*[text()='" + text + "']");
        }

        public static By PartialText(string text)
        {
            return By.XPath("//*[contains(text(),'" + text + "')]");
        }

        public static By PartialAttribute(string tag, string attribute, string value)
        {
            string attr;

            // put a '@' in front of the attribute if the user did not
            if(!attribute.StartsWith("@"))
            {
                attr = string.Format("@{0}", attribute);
            }
            else
            {
                attr = attribute;
            }

            return By.XPath(string.Format("//{0}[contains({1},'{2}')]", tag, attr, value));
        }
    }
}