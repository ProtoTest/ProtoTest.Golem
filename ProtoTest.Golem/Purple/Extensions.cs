using System;
using System.Drawing;
using System.Windows;
using System.Windows.Automation;
using ProtoTest.Golem.Purple.PurpleCore;
using ProtoTest.Golem.Purple.PurpleElements;
using Image = System.Drawing.Image;

namespace ProtoTest.Golem.Purple
{
    public static class Extensions
    {
        //TODO this has to be refactored
        public static Image GetImage(this AutomationElement window)
        {
            return new ScreenCapture().CaptureScreenShot();
        }

        public static void SetCheckbox(this PurpleCheckBox box, bool isChecked)
        {
            if (box.Checked != isChecked)
                box.Click();
        }

        public static Image GetImage(this PurpleElementBase item)
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
            result.X = (int)value.X;
            result.Y = (int)value.Y;
            result.Width = (int)value.Width;
            result.Height = (int)value.Height;
            return result;
        }

        public static bool IsStale(this PurpleElementBase item)
        {
            try
            {
                var enabled = item.PurpleElement.Current.IsEnabled;
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static bool Present(this PurpleElementBase item)
        {
            return !item.IsStale() && item.PurpleElement.Current.IsEnabled;
        }

        public static void WaitForVisible(this PurpleElementBase item)
        {
            try
            {
                var enabled = item.PurpleElement.Current.IsEnabled;
            }
            catch (Exception)
            {

            }


        }
    }
}