using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver.Elements.Types
{
    public class Button : Element
    {
        public Button(By bylocator) : base(bylocator)
        {
        }

        public Button(string name, By bylocator) : base(name, bylocator)
        {
        }
    }
}