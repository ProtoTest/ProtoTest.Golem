using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using TestStack.White;
using TestStack.White.UIItems;
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
        Window SplishSplashScreen = WhiteTestBase._app.GetWindow("Splash");

        Button CloseSplash;
        

        public SplashScreen()
        {
            //SplishSplashScreen = MainWindow.ModalWindow("Splash");
            SearchCriteria Close = SearchCriteria.ByText("Close");
            CloseSplash = SplishSplashScreen.Get<Button>(Close);  
            
        }

        public MainScreen CloseSplashScreen()
        {
            
            CloseSplash.Click();
            return new MainScreen();
        }





    }
}
