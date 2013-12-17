using System.Drawing;
using System.Windows;
using TestStack.White;
using TestStack.White.UIItems;
using Image = System.Drawing.Image;

namespace ProtoTest.Golem.White
{
    public static class Extensions
    {
        public static Image GetImage(this Application window)
        {
            return new ScreenCapture().CaptureScreenShot();
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
    }
}