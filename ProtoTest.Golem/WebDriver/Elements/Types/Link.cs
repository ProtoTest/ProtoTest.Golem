using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver.Elements.Types
{
    public class Link : Element
    {
        public Link(string name, By bylocator) : base(name, bylocator)
        {
        }

        public Link(By bylocator) : base(bylocator)
        {
        }
    }
}