using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using TestStack.White;
using TestStack.White.UIItemEvents;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using Image = System.Drawing.Image;

namespace ProtoTest.Golem.White
{
    public static class Extensions
    {
        public static Image GetImage(this Application window)
        {
            return new ScreenCapture().CaptureScreenShot();
        }

        public static void SetCheckbox(this CheckBox box, bool isChecked)
        {
            if(box.Checked!=isChecked)
                box.Click();
        }

        public static Image GetImage(this UIItem item)
        {
            Image screenImage = new ScreenCapture().CaptureScreenShot();
            Rectangle cropArea = item.Bounds.ToRectangle();
            var bmpImage = new Bitmap(screenImage);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public static Rectangle
            ToRectangle(this Rect value)
        {
            var result =
                new Rectangle();
            result.X = (int) value.X;
            result.Y = (int) value.Y;
            result.Width = (int) value.Width;
            result.Height = (int) value.Height;
            return result;
        }

        public static bool IsStale(this UIItem item)
        {
            try
            {
                return item.Enabled;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void WaitForVisible(this IUIItem item)
        {

            try
            {

                var enabled = item.Enabled;
            }
            catch (Exception)
            {
                
            }
            
            
        }
    }
}