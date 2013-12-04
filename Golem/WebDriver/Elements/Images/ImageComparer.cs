using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver.Elements.Images
{
    public class ImageComparer
    {
        public static bool ImageCompareString(System.Drawing.Image firstImage, System.Drawing.Image secondImage)
        {
            var ms1 = new MemoryStream();
            var ms2 = new MemoryStream();

            firstImage.Save(ms1, ImageFormat.Bmp);
            String firstBitmap = Convert.ToBase64String(ms1.ToArray());

            secondImage.Save(ms2, ImageFormat.Bmp);
            String secondBitmap = Convert.ToBase64String(ms2.ToArray());

            ms1.Dispose();
            ms2.Dispose();

            if (firstBitmap.Equals(secondBitmap))
            {
                return true;
            }
            return false;
        }

        public static bool ImageCompareArray(System.Drawing.Image firstImage, System.Drawing.Image secondImage)
        {
            bool flag = true;
            string firstPixel;
            string secondPixel;
            firstImage = Common.ScaleImage(firstImage);
            secondImage = Common.ScaleImage(firstImage);

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
                for (int i = 0; i < firstBmp.Width; i++)
                {
                    for (int j = 0; j < firstBmp.Height; j++)
                    {
                        firstPixel = firstBmp.GetPixel(i, j).ToString();
                        secondPixel = secondBmp.GetPixel(i, j).ToString();
                        if (firstPixel != secondPixel)
                        {
                            flag = false;
                            break;
                        }
                    }
                }

                if (flag == false)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static float ImageComparePercentage(System.Drawing.Image firstImage, System.Drawing.Image secondImage, byte fuzzyness=10)
        {
            //for (byte i = 0; i < 100; i++)
            //{
            //    Common.Log("Diff of image : " + TestBase.testData.lastElement.name + " at level : " + i + " : " + firstImage.PercentageDifference(secondImage, i));
            //}
            return firstImage.PercentageDifference(secondImage, fuzzyness);
            
        }

        public static float ImageCompareAverageHash(Image firstImage, Image secondImage)
        {
            int diff = 0;
            var first =  GetHash(firstImage).ToCharArray();
            var second = GetHash(secondImage).ToCharArray();
            for (var i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                {
                    diff++;
                }
            }
            Common.Log("The Hamming Distance is " + diff);
            Common.Log("The Hamming Difference is " + ((float)diff/first.Length));
            return ((float)diff/first.Length);
        }

        public static string GetHash(Image image)
        {
            string hashString = "";
            Bitmap newImage = new Bitmap(image.Resize(8, 8).GetGrayScaleVersion());

            int average = GetAverageColor(newImage).ToArgb();
            for (int x = 0; x < newImage.Width; x++)
            {
                for (int y = 0; y < newImage.Height; y++)
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
            //Common.Log(hashString);
            return hashString;

        }

        public static Image GetHashImage(Image image)
        {
            Bitmap newImage = new Bitmap(image.Resize(8, 8).GetGrayScaleVersion());

            int average = GetAverageColor(newImage).ToArgb();
            Bitmap bmp = new Bitmap(8,8);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    if (newImage.GetPixel(x, y).ToArgb() < average)
                    {
                        bmp.SetPixel(x,y,Color.Black);
                    }
                    else
                    {
                        bmp.SetPixel(x,y,Color.White);
                    }
                }
            }

            return bmp;

        }

        public static long GetHashCodeInt64(string input)
        {
            var s1 = input.Substring(0, input.Length / 2);
            var s2 = input.Substring(input.Length / 2);

            var x = ((long)s1.GetHashCode()) << 0x20 | s2.GetHashCode();

            return x;
        }

        public static Color GetAverageColor(Bitmap bmp)
        {

            //Used for tally
            int r = 0;
            int g = 0;
            int b = 0;

            int total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);

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