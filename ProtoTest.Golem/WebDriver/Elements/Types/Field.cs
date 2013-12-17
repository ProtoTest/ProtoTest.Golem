using OpenQA.Selenium;

namespace ProtoTest.Golem.WebDriver.Elements.Types
{
    public class Field : Element
    {
        public Field(By bylocator) : base(bylocator)
        {
        }

        public Field(string name, By bylocator) : base(name, bylocator)
        {
        }
    }
}