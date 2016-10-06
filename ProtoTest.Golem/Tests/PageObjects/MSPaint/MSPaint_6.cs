using System.Windows.Forms;
using Golem.Purple;
using Golem.Purple.PurpleElements;

namespace Golem.Tests.PageObjects.MSPaint
{
    public class MSPaint_6 : BaseScreenObject
    {
        private readonly PurplePanel PaintArea = new PurplePanel("PaintArea", "Untitled - Paint/!BLANK!/!BLANK!");

        private readonly PurpleButton TextButton = new PurpleButton("MSPaintTextButton",
            "Untitled - Paint/Ribbon/Ribbon/!BLANK!/Ribbon/Lower Ribbon/!BLANK!/Home/Tools/Text");

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