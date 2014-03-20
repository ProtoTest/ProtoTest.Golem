using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ProtoTest.Golem.Purple.PurpleElements
{
    public class PurplePanel : PurpleElementBase
    {
        public PurplePanel(string name, string locatorPath) : base(name, locatorPath)
        {
        }

        public void DragAndDrop(Point startPoint, Point endPoint, bool RMB = false)
        {
            if (!RMB)//Use the left mouse button as default
            {
                MoveCursor(startPoint);
                LMB_Down();
                MoveCursor(endPoint);
                LMB_Up();
            }
            else
            {
                MoveCursor(startPoint);
                RMB_Down();
                MoveCursor(endPoint);
                RMB_Up(); 
            }
            
        }


    }
}
