using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProtoTest.Golem.Purple;
using ProtoTest.Golem.Purple.PurpleElements;

namespace ProtoTest.Golem.Tests.PageObjects.MSPaint
{
    public class MSPaint_6 : BaseScreenObject
    {
        PurpleButton TextButton = new PurpleButton("MSPaintTextButton", "Untitled - Paint/Ribbon/Ribbon/!BLANK!/Ribbon/Lower Ribbon/!BLANK!/Home/Tools/Text");
        PurplePanel PaintArea = new PurplePanel("PaintArea", "Untitled - Paint/!BLANK!/!BLANK!");

        public void PaintText(string text)
        {
            TextButton.Click();
            PaintArea.MoveCursor(PaintArea.Bounds.TopLeft);
            PaintArea.LMB_Down();
            SendKeys.SendWait(text);
        }

        public static MSPaint_6 PaintWindow()
        {
            return new MSPaint_6();
        }
    }
}
