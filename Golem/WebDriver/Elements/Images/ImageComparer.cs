using System;
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
            //for (byte i = 0; i < 50; i++)
            //{
            //    Common.Log("Diff of image : " + TestBase.testData.lastElement.name + " at level : " + i + " : " + firstImage.PercentageDifference(secondImage, i));
            //}
            return firstImage.PercentageDifference(secondImage, fuzzyness);
            
        }
    }
}