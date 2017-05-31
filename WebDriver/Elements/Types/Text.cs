using OpenQA.Selenium;

namespace Golem.WebDriver.Elements.Types
{
    public class Text : Element
    {
        public Text(By bylocator) : base(bylocator)
        {
        }

        public Text(string name, By bylocator) : base(name, bylocator)
        {
        }
    }
}