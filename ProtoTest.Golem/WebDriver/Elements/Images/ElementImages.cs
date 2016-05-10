using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver.Elements.Images
{
    public class ElementImages
    {
        public static bool UpdateImages = Config.settings.imageCompareSettings.updateImages;
        private readonly Image liveImage;
        private readonly Image storedImage;
        public float difference;
        public string differenceString;
        public Element element;

        public ElementImages(Element element)
        {
            this.element = element;
            CreateDirectory();
            liveImage = GetImage();
            storedImage = GetStoredImage();
        }

        private string ImageLocation
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                       "\\ElementImages\\" + element.pageObjectName + "_" +
                       element.name.Replace(" ", "") + ".bmp";
            }
        }

        public Image GetDifferenceImage()
        {
            var bmp = new Bitmap(liveImage.Width, liveImage.Height);

            return liveImage.GetDifferenceOverlayImage(storedImage)
                .Resize(storedImage.Width, storedImage.Height);
        }

        public bool ImagesMatch()
        {
            if ((!File.Exists(ImageLocation)) || (UpdateImages))
            {
                UpdateImage();
            }
            difference = ImageComparer.ImageComparePercentage(storedImage, liveImage,
                Config.settings.imageCompareSettings.fuzziness);
            differenceString = (difference*100).ToString("0.##\\%");

            return difference < Config.settings.imageCompareSettings.accuracy;
        }

        public Image GetMergedImage()
        {
            var overlayImage = OverlayImages(liveImage, GetDifferenceImage());
            var mergedImage = CombineImages(storedImage, liveImage, overlayImage);

            return mergedImage;
        }

        private Image CombineImages(Image image1, Image image2, Image image3)
        {
            var newWidth = image1.Width + image2.Width + image3.Width;
            var newHeight = image1.Height;
            var bmp = new Bitmap(newWidth, newHeight);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(image1, new Point(0, 0));
                gr.DrawImage(image2, new Point(image1.Width, 0));
                gr.DrawImage(image3, new Point(image2.Width + image1.Width, 0));
            }

            return bmp;
        }

        public Image GetStoredImage()
        {
            if (File.Exists(ImageLocation))
            {
                return Image.FromFile(ImageLocation);
            }

            return GetImage();
        }

        public Image OverlayImages(Image imageBackground, Image imageOverlay)
        {
            imageOverlay = imageOverlay.Resize(imageBackground.Width, imageBackground.Height);
            Image img = new Bitmap(imageBackground.Width, imageBackground.Height);
            using (var gr = Graphics.FromImage(img))
            {
                gr.DrawImage(imageBackground, new Point(0, 0));
                gr.DrawImage(imageOverlay, new Point(0, 0));
            }

            return img;
        }

        public void CreateDirectory()
        {
            if (!Directory.Exists(Common.GetCodeDirectory() + "\\ElementImages"))
            {
                Directory.CreateDirectory(Common.GetCodeDirectory() + "\\ElementImages");
            }
        }

        public void DeleteOldImage()
        {
            if (File.Exists(ImageLocation))
            {
                File.Delete(ImageLocation);
            }
        }

        public void UpdateImage()
        {
            using (var image = GetImage())
            {
                SaveImage(image);
            }
        }

        private void SaveImage(Image image)
        {
            try
            {
                DeleteOldImage();
                using (var tempImage = new Bitmap(image))
                {
                    tempImage.Save(ImageLocation, ImageFormat.Bmp);
                }
            }
            catch (Exception e)
            {
                Log.Message("Exception saving image : " + e.Message);
            }
        }

        public Image GetImage()
        {
            var size = new Size(element.Size.Width, element.Size.Height);
            if (element.Displayed == false)
            {
                throw new BadImageFormatException(string.Format(
                    "Could not create image for element {0} as it is hidden", element.name));
            }
            var cropRect = new Rectangle(element.Location, size);
            var screenShot = TestBase.testData.driver.GetScreenshot();

            // Trim the crop to not extend off the screenshot, preventing OutOfMemoryException.
            if (cropRect.X < 0)
            {
                cropRect.X = 0;
            }
            if (cropRect.Y < 0)
            {
                cropRect.Y = 0;
            }
            if (cropRect.X + cropRect.Width > screenShot.Width)
            {
                cropRect.Width = screenShot.Width - cropRect.X;
            }
            if (cropRect.Y + cropRect.Height > screenShot.Height)
            {
                cropRect.Height = screenShot.Height - cropRect.Y;
            }

            return cropImage(screenShot, cropRect);
        }

        private Image cropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

            return bmpCrop;
        }

        public void AttachImage()
        {
            Log.Image(GetImage());
        }
    }
}