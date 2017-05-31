using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Gallio.Framework;
using Golem.Core;
using Golem.Purple.Elements;
using Golem.WebDriver.Elements.Images;

namespace Golem.Purple
{
    //TODO: this class is going to need to be re-factored!
    public class ElementImageComparer
    {
        public static bool UpdateImages = Config.settings.imageCompareSettings.updateImages;
        private readonly Image liveImage;
        private readonly Image storedImage;
        public float difference;
        public string differenceString;
        public IPurpleElement element;

        public ElementImageComparer(IPurpleElement element)
        {
            this.element = element;
            CreateDirectory();
            liveImage = GetLiveImage();
            storedImage = GetStoredImage();
        }

        private string ImageLocation
        {
            get
            {
                return Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                       "\\ElementImages\\" + element.ElementName.Replace(" ", "") + ".bmp";
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

        public Image GetLiveImage()
        {
            return element.UIAElement.GetImage();
        }

        public Image GetStoredImage()
        {
            if (File.Exists(ImageLocation))
                return Image.FromFile(ImageLocation);
            return GetLiveImage();
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
                Directory.CreateDirectory(Common.GetCodeDirectory() + "\\ElementImages");
        }

        public void DeleteOldImage()
        {
            if (File.Exists(ImageLocation))
                File.Delete(ImageLocation);
        }

        public void UpdateImage()
        {
            using (var image = GetLiveImage())
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

        private Image cropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public void VerifyImage()
        {
            if (ImagesMatch())
            {
                TestContext.CurrentContext.IncrementAssertCount();
                Log.Message("Images match!");
            }

            else
            {
                TestBase.AddVerificationError("Images don't match", GetMergedImage());
            }
        }
    }
}