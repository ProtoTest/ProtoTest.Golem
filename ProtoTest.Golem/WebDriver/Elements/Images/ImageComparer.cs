using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver.Elements.Images
{
    public class ImageComparer
    {
        public static bool ImageCompareString(Image firstImage, Image secondImage, float failurePercent)
        {
            var ms1 = new MemoryStream();
            var ms2 = new MemoryStream();

            firstImage.Save(ms1, ImageFormat.Bmp);
            var firstBitmap = Convert.ToBase64String(ms1.ToArray());

            secondImage.Save(ms2, ImageFormat.Bmp);
            var secondBitmap = Convert.ToBase64String(ms2.ToArray());

            ms1.Dispose();
            ms2.Dispose();

            var total = 0;
            for (var i = 0; i < firstBitmap.Length; i++)
            {
                if (firstBitmap[i] != secondBitmap[i])
                {
                    total++;
                }
            }
            var failure = total/(float) firstBitmap.Length;
            return failure < failurePercent;
        }

        public static bool ImageCompareArray(Image firstImage, Image secondImage)
        {
            var images_match = false;
            string firstPixel;
            string secondPixel;
            firstImage = Common.ScaleImage(firstImage);
            secondImage = Common.ScaleImage(secondImage);

            if (firstImage.Size.Width > secondImage.Size.Width)
            {
                firstImage = Common.ResizeImage(firstImage, secondImage.Size.Width, secondImage.Size.Height);
            }
            if (secondImage.Size.Width > firstImage.Size.Width)
            {
                secondImage = Common.ResizeImage(secondImage, firstImage.Size.Width, firstImage.Size.Height);
            }

            var firstBmp = new Bitmap(firstImage);
            var secondBmp = new Bitmap(secondImage);

            if (firstBmp.Width == secondBmp.Width
                && firstBmp.Height == secondBmp.Height)
            {
                for (var i = 0; i < firstBmp.Width; i++)
                {
                    for (var j = 0; j < firstBmp.Height; j++)
                    {
                        firstPixel = firstBmp.GetPixel(i, j).ToString();
                        secondPixel = secondBmp.GetPixel(i, j).ToString();
                        if (firstPixel != secondPixel)
                        {
                            // pixels do not match, bail...
                            return false;
                        }
                    }
                }

                // all pixels match
                images_match = true;
            }

            return images_match;
        }

        public static float ImageComparePercentage(Image firstImage, Image secondImage, byte fuzzyness = 10)
        {
            return firstImage.PercentageDifference(secondImage, fuzzyness);
        }

        public static float ImageCompareAverageHash(Image firstImage, Image secondImage)
        {
            var diff = 0;
            var first = GetHash(firstImage).ToCharArray();
            var second = GetHash(secondImage).ToCharArray();
            for (var i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                {
                    diff++;
                }
            }
            Common.Log("The Hamming Distance is " + diff);
            Common.Log("The Hamming Difference is " + ((float) diff/first.Length));

            return ((float) diff/first.Length);
        }

        public static string GetHash(Image image)
        {
            var hashString = "";
            var newImage = new Bitmap(image.Resize(8, 8).GetGrayScaleVersion());

            var average = GetAverageColor(newImage).ToArgb();
            for (var x = 0; x < newImage.Width; x++)
            {
                for (var y = 0; y < newImage.Height; y++)
                {
                    if (newImage.GetPixel(x, y).ToArgb() < average)
                    {
                        hashString += "0";
                    }
                    else
                    {
                        hashString += "1";
                    }
                }
            }

            return hashString;
        }

        public static Image GetHashImage(Image image)
        {
            var newImage = new Bitmap(image.Resize(8, 8).GetGrayScaleVersion());

            var average = GetAverageColor(newImage).ToArgb();
            var bmp = new Bitmap(8, 8);
            for (var x = 0; x < bmp.Width; x++)
            {
                for (var y = 0; y < bmp.Height; y++)
                {
                    if (newImage.GetPixel(x, y).ToArgb() < average)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                }
            }

            return bmp;
        }

        public static long GetHashCodeInt64(string input)
        {
            var s1 = input.Substring(0, input.Length/2);
            var s2 = input.Substring(input.Length/2);

            var x = ((long) s1.GetHashCode()) << 0x20 | s2.GetHashCode();

            return x;
        }

        public static Color GetAverageColor(Bitmap bmp)
        {
            //Used for tally
            var r = 0;
            var g = 0;
            var b = 0;

            var total = 0;

            for (var x = 0; x < bmp.Width; x++)
            {
                for (var y = 0; y < bmp.Height; y++)
                {
                    var clr = bmp.GetPixel(x, y);

                    r += clr.R;
                    g += clr.G;
                    b += clr.B;

                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(r, g, b);
        }
    }
}