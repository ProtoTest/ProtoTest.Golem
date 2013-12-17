using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
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
    public class MainScreen : BaseScreenObject
    {
        private static Window mainScreen = WhiteTestBase._app.GetWindow("LifeQuest™ Pipeline");
        
        public MainScreen()
        {
            base.setWindow(mainScreen);
        }

        Component<Menu> FileMenu = new Component<Menu>("LifeQuest™ Pipeline", "FileMenu", SearchCriteria.ByText(("File")));
        static Component<Menu> Open = new Component<Menu>("LifeQuest™ Pipeline", "Open", SearchCriteria.ByText("Open"));
        Component<Button> OpenProj = new Component<Button>(Open, "OpenProject", SearchCriteria.ByText("Project..."));
       

        public MainScreen OpenProject(String project)
        {
            FileMenu.Click();
            Open.Click();
            OpenProj.Click();

            OpenFile_Dialog OpenFile = new OpenFile_Dialog("LifeQuest™ Pipeline");
            OpenFile.OpenaFile(project);



            

            return this;

        }


      

    }
}
