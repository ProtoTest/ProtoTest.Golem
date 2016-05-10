using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    ///     Contains By Extensions for the OpenQA.Selenium.By class
    /// </summary>
    public class ByE
    {
        public static OpenQA.Selenium.By Text(string text)
        {
            return OpenQA.Selenium.By.XPath("//*[text()='" + text + "']");
        }

        public static OpenQA.Selenium.By PartialText(string text)
        {
            return OpenQA.Selenium.By.XPath("//*[contains(text(),'" + text + "')]");
        }

        public static OpenQA.Selenium.By PartialAttribute(string tag, string attribute, string value)
        {
            string attr;

            // put a '@' in front of the attribute if the user did not
            if (!attribute.StartsWith("@"))
            {
                attr = string.Format("@{0}", attribute);
            }
            else
            {
                attr = attribute;
            }

            return OpenQA.Selenium.By.XPath(string.Format("//{0}[contains({1},'{2}')]", tag, attr, value));
        }
    }
}