using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoTest.Golem.White.PurpleElements
{
    public class PurpleMenu : PurpleElementBase
    {
        private string _pathAfterMenu;
        private string[] _pathtoMenuSelection;

        public PurpleMenu(string name, string locatorPath, string targetPath) : base(name, locatorPath)
        {
            _pathAfterMenu = targetPath;
        }

        private void FindMenuPath()
        {
            _pathtoMenuSelection = _pathAfterMenu.Split(Convert.ToChar(getDelimiter()));
        }


    }
}
