using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems.WindowItems;

namespace Golem.TestStack.White
{
    public class BaseScreenObject
    {
        //This class will act similarly to how the BasePageObject works on the webdriver side
        private Window _window;

        public BaseScreenObject()
        {
            
        }

        public Window getWindow()
        {
            return _window;
        }

    }
}
