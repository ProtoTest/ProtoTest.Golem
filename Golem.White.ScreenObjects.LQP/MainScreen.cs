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
using TestStack.White.UIItems.WPFUIItems;

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
            var DockTop = mainScreen.Get<GroupBox>(SearchCriteria.ByText("Dock Top"));
            var menuBar = DockTop.GetMenuBar(SearchCriteria.ByText("Main Menu"));
            var File = menuBar.Get<Menu>(SearchCriteria.ByText("File"));
            var Open = menuBar.Get<Menu>(SearchCriteria.ByText("Open"));
            var Project = Open.Get<Button>(SearchCriteria.ByText("Project..."));
            File.Click();
            Open.Click();
            Project.Click();

            
            

            return this;

        }

    }
}
