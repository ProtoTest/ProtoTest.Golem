using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Automation;

namespace ProtoTest.Golem.White.PurpleElements
{
    public class PurpleMenu : PurpleElementBase
    {
        private string _pathAfterMenu;
        private string[] _pathtoMenuSelection;

        public PurpleMenu(string name, string locatorPath, string targetPath) : base(name, locatorPath)
        {
            _pathAfterMenu = targetPath;
            FindMenuPath();
        }

        private void FindMenuPath()
        {
            _pathtoMenuSelection = _pathAfterMenu.Split(getDelimiter().ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public new void Click()
        {
            int menuNums = _pathtoMenuSelection.Count();
            AutomationElement menu = null;
            for(int x = 0; x < menuNums - 1; x++)
            {
                menu = PurpleElement.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, _pathtoMenuSelection[x]));
                ((ExpandCollapsePattern)menu.GetCurrentPattern(ExpandCollapsePattern.Pattern)).Expand();
                Thread.Sleep(50);
            }
            if (menu != null)
            {
                AutomationElement MenuItem = menu.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, _pathtoMenuSelection[menuNums - 1]));
                ((InvokePattern)MenuItem.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
            }
        }


    }
}
