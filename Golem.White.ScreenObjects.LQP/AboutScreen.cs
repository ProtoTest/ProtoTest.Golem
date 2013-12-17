using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Golem.White.Elements;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using Button = TestStack.White.UIItems.Button;

namespace Golem.White.ScreenObjects.LQP
{
    public class AboutScreen : BaseScreenObject
    {
        public static Window wind = WhiteTestBase._app.GetWindow("About");

        public Button okButt = new Elements.Button("OkButton", SearchCriteria.ByText("OK"));

        public AboutScreen()
        {
            //SearchCriteria buttonname = SearchCriteria.ByText("OK");
            //okButt = wind.Get<Button>(buttonname);            
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
