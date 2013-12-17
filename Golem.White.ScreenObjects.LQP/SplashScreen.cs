using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using Golem.White.Elements;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.PropertyGridItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;

using TestStack.White.UIA;

namespace Golem.White.ScreenObjects.LQP
{
    public class SplashScreen : BaseScreenObject
    {
        //Element<Button> CloseSplash = new Element<Button>();
       static private Elements.Button button = new Elements.Button("Name", SearchCriteria.ByText("slkdfj"));
        public Button CloseSplash = new Button("CloseSplash",SearchCriteria.ByText("Close"));

        public SplashScreen()
        {
            window = WhiteTestBase.app.GetWindow("Splash");

        }

        public MainScreen CloseSplashScreen()
        {
            
            CloseSplash.Click();
            return new MainScreen();
        }





    }
}
