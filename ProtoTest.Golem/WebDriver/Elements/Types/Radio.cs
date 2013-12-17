using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver.Elements.Types
{
    public class Radio : Element
    {
        public Radio(By bylocator) : base(bylocator)
        {
        }

        public Radio(string name, By bylocator) : base(name, bylocator)
        {
        }

        public Radio SetValue(string value)
        {
            element.FindElement(By.XPath("//*[@value='" + value + "']")).Click();
            return this;
        }
    }
}