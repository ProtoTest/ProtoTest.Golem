using OpenQA.Selenium;

namespace Golem.WebDriver.Elements.Types
{
    public class Panel : Element
    {
        public Panel(By bylocator) : base(bylocator)
        {
        }

        public Panel(string name, By bylocator) : base(name, bylocator)
        {
        }
    }
}