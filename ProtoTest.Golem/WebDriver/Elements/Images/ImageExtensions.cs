using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

// Created in 2012 by Jakob Krarup (www.xnafan.net).
// Use, alter and redistribute this code freely,
// but please leave this comment :)

namespace Golem.WebDriver.Elements.Images
{
    /// <summary>
    ///     A class with extensionmethods for comparing images
    /// </summary>
    public static class ImageTool
    {
        //the font to use for the DifferenceImages
        private static readonly Font DefaultFont = new Font("Arial", 8);
        //the brushes to use for the DifferenceImages
        private static readonly Brush[] brushes = new Brush[256];
        //the colormatrix needed to grayscale an image
        //http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        private static readonly ColorMatrix ColorMatrix = new ColorMatrix(new[]
        {
            new[] {.3f, .3f, .3f, 0, 0},
            new[] {.59f, .59f, .59f, 0, 0},
            new[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
        });

        //Create the brushes in varying intensities
        static ImageTool()
        {
            for (var i = 0; i < 256; i++)
            {
                brushes[i] = new SolidBrush(Color.FromArgb(i/2, 255, 2, 2));
            }
        }

        /// <summary>
        ///     Gets the difference between two images as a percentage
        /// </summary>
        /// <param name="img1">The first image</param>
        /// <param name="img2">The image to compare to</param>
        /// <param name="threshold">How big a difference (out of 255) will be ignored - the default is 3.</param>
        /// <returns>The difference between the two images as a percentage</returns>
        public static float PercentageDifference(this Image img1, Image img2, byte threshold = 3)
        {
            var differences = img1.GetDifferences(img2);

            var diffPixels = 0;

            foreach (byte b in differences)
            {
                if (b > threshold)
                {
                    diffPixels++;
                }
            }

            return diffPixels/256f;
        }

        /// <summary>
        ///     Gets an image which displays the differences between two images
        /// </summary>
        /// <param name="img1">The first image</param>
        /// <param name="img2">The image to compare with</param>
        /// <param name="adjustColorSchemeToMaxDifferenceFound">
        ///     Whether to adjust the color indicating maximum difference (usually 255) to the maximum difference found in this
        ///     case.
        ///     E.g. if the maximum difference found is 12, then a true value in adjustColorSchemeToMaxDifferenceFound would result
        ///     in 0 being black, 6 being dark pink, and 12 being bright pink.
        ///     A false value would still have differences of 255 as bright pink resulting in the 12 difference still being very
        ///     dark.
        /// </param>
        /// <param name="percentages">Whether to write percentages in each of the 255 squares (true) or the absolute value (false)</param>
        /// <returns>an image which displays the differences between two images</returns>
        public static Bitmap GetDifferenceImage(this Image img1, Image img2,
            bool adjustColorSchemeToMaxDifferenceFound = false, bool absoluteText = false)
        {
            //create a 16x16 tiles image with information about how much the two images differ
            var cellsize = 16; //each tile is 16 pixels wide and high
            var bmp = new Bitmap(16*cellsize + 1, 16*cellsize + 1);
            //16 blocks * 16 pixels + a borderpixel at left/bottom

            var g = Graphics.FromImage(bmp);
            //g.FillRectangle(Brushes, 0, 0, bmp.Width, bmp.Height);
            byte[,] differences = img1.GetDifferences(img2);
            byte maxDifference = 255;

            //if wanted - adjust the color scheme, by finding the new maximum difference
            if (adjustColorSchemeToMaxDifferenceFound)
            {
                maxDifference = 0;
                foreach (var b in differences)
                {
                    if (b > maxDifference)
                    {
                        maxDifference = b;
                    }
                }

                if (maxDifference == 0)
                {
                    maxDifference = 1;
                }
            }

            for (var y = 0; y < differences.GetLength(1); y++)
            {
                for (var x = 0; x < differences.GetLength(0); x++)
                {
                    var cellValue = differences[x, y];
                    string cellText = null;

                    if (absoluteText)
                    {
                        cellText = cellValue.ToString();
                    }
                    else
                    {
                        cellText = string.Format("{0}%", (int) cellValue);
                    }

                    var percentageDifference = (float) differences[x, y]/maxDifference;
                    var colorIndex = (int) (255*percentageDifference);

                    g.FillRectangle(brushes[colorIndex], x*cellsize, y*cellsize, cellsize, cellsize);
                    g.DrawRectangle(Pens.Blue, x*cellsize, y*cellsize, cellsize, cellsize);
                    var size = g.MeasureString(cellText, DefaultFont);
                    g.DrawString(cellText, DefaultFont, Brushes.Black, x*cellsize + cellsize/2 - size.Width/2 + 1,
                        y*cellsize + cellsize/2 - size.Height/2 + 1);
                    g.DrawString(cellText, DefaultFont, Brushes.White, x*cellsize + cellsize/2 - size.Width/2,
                        y*cellsize + cellsize/2 - size.Height/2);
                }
            }

            return bmp;
        }

        /// <summary>
        ///     Gets an image which displays the differences between two images
        /// </summary>
        /// <param name="img1">The first image</param>
        /// <param name="img2">The image to compare with</param>
        /// <param name="adjustColorSchemeToMaxDifferenceFound">
        ///     Whether to adjust the color indicating maximum difference (usually 255) to the maximum difference found in this
        ///     case.
        ///     E.g. if the maximum difference found is 12, then a true value in adjustColorSchemeToMaxDifferenceFound would result
        ///     in 0 being black, 6 being dark pink, and 12 being bright pink.
        ///     A false value would still have differences of 255 as bright pink resulting in the 12 difference still being very
        ///     dark.
        /// </param>
        /// <param name="percentages">Whether to write percentages in each of the 255 squares (true) or the absolute value (false)</param>
        /// <returns>an image which displays the differences between two images</returns>
        public static Bitmap GetDifferenceOverlayImage(this Image img1, Image img2)
        {
            var bmp = new Bitmap(16*16 + 1, 16*16 + 1);
            var g = Graphics.FromImage(bmp);
            byte[,] differences = img1.GetDifferences(img2);
            byte maxDifference = 1;
            foreach (var b in differences)
            {
                if (b > maxDifference)
                    maxDifference = b;
            }
            for (var y = 0; y < differences.GetLength(1); y++)
            {
                for (var x = 0; x < differences.GetLength(0); x++)
                {
                    var percentageDifference = (float) differences[x, y]/maxDifference;
                    var colorIndex = (int) (255*percentageDifference);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(colorIndex, 255, 2, 2)), x*16, y*16, 16, 16);
                }
            }
            return bmp;
        }

        /// <summary>
        ///     Finds the differences between two images and returns them in a doublearray
        /// </summary>
        /// <param name="img1">The first image</param>
        /// <param name="img2">The image to compare with</param>
        /// <returns>the differences between the two images as a doublearray</returns>
        public static byte[,] GetDifferences(this Image img1, Image img2)
        {
            var thisOne = (Bitmap) img1.Resize(16, 16).GetGrayScaleVersion();
            var theOtherOne = (Bitmap) img2.Resize(16, 16).GetGrayScaleVersion();
            var differencesRed = new byte[16, 16];
            Console.WriteLine();

            for (var y = 0; y < 16; y++)
            {
                for (var x = 0; x < 16; x++)
                {
                    differencesRed[x, y] = (byte) Math.Abs(thisOne.GetPixel(x, y).R - theOtherOne.GetPixel(x, y).R);
                }
            }
            return differencesRed;
        }

        /// <summary>
        ///     Converts an image to grayscale
        /// </summary>
        /// <param name="original">The image to grayscale</param>
        /// <returns>A grayscale version of the image</returns>
        public static Image GetGrayScaleVersion(this Image original)
        {
            //http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
            //create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            var g = Graphics.FromImage(newBitmap);

            //create some image attributes
            var attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(ColorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();

            return newBitmap;
        }

        public static Image GetAverageColorValue(this Image original)
        {
            var newBitmap = new Bitmap(original.Width, original.Height);

            return newBitmap;
        }

        /// <summary>
        ///     Resizes an image
        /// </summary>
        /// <param name="originalImage">The image to resize</param>
        /// <param name="newWidth">The new width in pixels</param>
        /// <param name="newHeight">The new height in pixels</param>
        /// <returns>A resized version of the original image</returns>
        public static Image Resize(this Image originalImage, int newWidth, int newHeight)
        {
            Image smallVersion = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(smallVersion))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return smallVersion;
        }

        /// <summary>
        ///     Helpermethod to print a doublearray of
        /// </summary>
        /// <typeparam name="T">The type of doublearray</typeparam>
        /// <param name="doubleArray">The doublearray to print</param>
        public static void ToConsole<T>(this T[,] doubleArray)
        {
            for (var y = 0; y < doubleArray.GetLength(0); y++)
            {
                Console.Write("[");
                for (var x = 0; x < doubleArray.GetLength(1); x++)
                {
                    Console.Write("{0,3},", doubleArray[x, y]);
                }
                Console.WriteLine("]");
            }
        }

        /// <summary>
        ///     Gets a bitmap with the RGB histograms of a bitmap
        /// </summary>
        /// <param name="bmp">The bitmap to get the histogram for</param>
        /// <returns>A bitmap with the histogram for R, G and B values</returns>
        public static Bitmap GetRgbHistogramBitmap(this Bitmap bmp)
        {
            return new Histogram(bmp).Visualize();
        }

        /// <summary>
        ///     Get a histogram for a bitmap
        /// </summary>
        /// <param name="bmp">The bitmap to get the histogram for</param>
        /// <returns>A histogram for the bitmap</returns>
        public static Histogram GetRgbHistogram(this Bitmap bmp)
        {
            return new Histogram(bmp);
        }

        /// <summary>
        ///     Gets the difference between two images as a percentage
        /// </summary>
        /// <returns>The difference between the two images as a percentage</returns>
        /// <param name="image1Path">The path to the first image</param>
        /// <param name="image2Path">The path to the second image</param>
        /// <param name="threshold">How big a difference (out of 255) will be ignored - the default is 3.</param>
        /// <returns>The difference between the two images as a percentage</returns>
        public static float GetPercentageDifference(string image1Path, string image2Path, byte threshold = 3)
        {
            if (CheckFile(image1Path) && CheckFile(image2Path))
            {
                var img1 = Image.FromFile(image1Path);
                var img2 = Image.FromFile(image2Path);

                return img1.PercentageDifference(img2, threshold);
            }
            return -1;
        }

        private static bool CheckFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File '" + filePath + "' not found!");
            }
            return true;
        }
    }
}