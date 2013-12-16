using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using Gallio.Framework;
using TestStack.White.UIItems;
using Image = System.Drawing.Image;

namespace Golem.White
{
    public static class Extensions
    {

        public static Image VerifyImage(this UIItem item)
        {
        }
        public static Image GetImage(this UIItem item)
        {
            Image screenImage = Capture.Screenshot();
            var cropArea = item.Bounds.ToRectangle();
            var bmpImage = new Bitmap(screenImage);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public static System.Drawing.Rectangle
            ToRectangle(this Rect value)
        {
            System.Drawing.Rectangle result =
                new System.Drawing.Rectangle();
            result.X = (int)value.X;
            result.Y = (int)value.Y;
            result.Width = (int)value.Width;
            result.Height = (int)value.Height;
            return result;
        }
    }

}
