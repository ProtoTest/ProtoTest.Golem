using OpenQA.Selenium;

namespace Golem.WebDriver.Elements.Types
{
    public class Image : Element
    {
        public Image(By bylocator) : base(bylocator)
        {
        }

        public Image(string name, By bylocator) : base(name, bylocator)
        {
        }
    }
}