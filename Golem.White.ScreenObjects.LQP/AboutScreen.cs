using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using Golem.TestStack.White;

namespace Golem.White.ScreenObjects.LQP
{
    public class AboutScreen : BaseScreenObject
    {
        public Window wind = WhiteTestBase._app.GetWindow("About");
        
        public Button okButt;

        public AboutScreen()
        {
            SearchCriteria buttonname = SearchCriteria.ByText("OK");
            okButt = wind.Get<Button>(buttonname);            
        }

        public static AboutScreen StartScreen()
        {
            return new AboutScreen();
        }

        public SplashScreen clickOkButton()
        {
            okButt.Click();
            return new SplashScreen();
        }

    }
}
