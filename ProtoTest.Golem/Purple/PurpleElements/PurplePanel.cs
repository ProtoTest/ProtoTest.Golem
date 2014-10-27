using System.Windows;
using System.Windows.Automation;

namespace ProtoTest.Golem.Purple.PurpleElements
{
    public class PurplePanel : PurpleElementBase
    {
        public PurplePanel(string name, string locatorPath) : base(name, locatorPath)
        {
        }

        public PurplePanel(string name, AutomationElement element) : base(name, element)
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
