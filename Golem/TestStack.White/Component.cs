using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;


namespace Golem.TestStack.White
{
    public class Component
    {
        //The Component class acts like an element from the webdriver side
        private Window _window;
        private SearchCriteria _SearchCrit;
        private Application _application;
        private Button Somebutton;
        
        public Component()
        {
            _application = WhiteTestBase._app;
  
        }

        public Component(String wind, String search, String UItype)
        {
            _application = WhiteTestBase._app;
            _window = _application.GetWindow(wind);
            _SearchCrit = SearchCriteria.ByText(search);

            Somebutton = _window.Get<Button>(_SearchCrit);
        }

        public void Click()
        {
            Somebutton.Click();
        }



    }
}
