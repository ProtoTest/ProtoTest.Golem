using ProtoTest.Golem.White.Elements;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    public class BaseScreenObject
    {
        private WhiteWindow _window;

        public WhiteWindow window
        {
            get
            {
                return _window ?? WhiteTestBase.window;
            }
            set
            {
                _window = value;
            }
        }

        public BaseScreenObject()
        {
        }

        public BaseScreenObject(WhiteWindow window)
        {
            this.window = window;
        }

        public void setWindow(WhiteWindow window)
        {
            this.window = window;
        }

        public WhiteWindow getWindow(WhiteWindow window)
        {
            return window;
        }
}

}