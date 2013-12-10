using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.TestStack.White;

namespace Golem.White.ScreenObjects.LQP
{
    public class AboutScreen : BaseScreenObject
    {
        public static Component OkButton = new Component("About", "OK", "Button");

       

        public static void clickOkButton()
        {
            OkButton.Click();
        }

    }
}
