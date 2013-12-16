using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.PropertyGridItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.MenuItems;


using TestStack.White.UIA;
using Golem.TestStack.White;

namespace Golem.White.ScreenObjects.LQP
{
    public static class extension
    {
        public static void TryClick(this MenuBar menu)
        {
            
        }
    }

    public class MainScreen : BaseScreenObject
    {
        Window mainScreen = WhiteTestBase._app.GetWindow("LifeQuest™ Pipeline");
       
        Window OpenFile;

        public MainScreen()
        {
            
        }

       

        public MainScreen OpenProject(String project)
        {
            SearchCriteria FileMenu = SearchCriteria.ByText("File");
            try
            {
                mainScreen.LogStructure();
                
                
                //menuscreen.Get<Button>(SearchCriteria.ByText("Project...")).Click();
            }
            catch (Exception)
            {
                Button OpenProj = mainScreen.Get<Button>("Project...");
                OpenProj.Click();
 
            }
            
            

            return this;

        }

    }
}
