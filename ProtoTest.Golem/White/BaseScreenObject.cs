using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    public abstract class BaseScreenObject
    {
        //This class will act similarly to how the BasePageObject works on the webdriver side

        public BaseScreenObject()
        {
        }

        public BaseScreenObject(Window window)
        {
            this.window = window;
        }

        public Window window
        {
            get { return WhiteTestBase.window; }
            set { WhiteTestBase.window = value; }
        }
    }
}