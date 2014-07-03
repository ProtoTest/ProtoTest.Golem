using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Gallio.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.Elements;
using ProtoTest.Golem.WebDriver.Elements.Images;
using Image = System.Drawing.Image;

namespace ProtoTest.Golem.Purple
{
    //TODO: this class is going to need to be re-factored!
    public class ElementImageComparer
    {
        public static bool UpdateImages = Config.Settings.imageCompareSettings.updateImages;
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
                return Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") + "\\ElementImages\\" + element.ElementName.Replace(" ", "") + ".bmp";
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
                Config.Settings.imageCompareSettings.fuzziness);
            differenceString = (difference * 100).ToString("0.##\\%");
            return difference < Config.Settings.imageCompareSettings.accuracy;
        }

        public Image GetMergedImage()
        {
            Image overlayImage = OverlayImages(liveImage, GetDifferenceImage());
            Image mergedImage = CombineImages(storedImage, liveImage, overlayImage);
            return mergedImage;
        }

        private Image CombineImages(Image image1, Image image2, Image image3)
        {
            int newWidth = image1.Width + image2.Width + image3.Width;
            int newHeight = image1.Height;
            var bmp = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(bmp))
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
            using (Graphics gr = Graphics.FromImage(img))
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
            using (Image image = GetLiveImage())
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
                Common.Log("Exception saving image : " + e.Message);
            }
        }

        private Image cropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public void VerifyImage()
        {

            if (ImagesMatch())
            {
                TestContext.CurrentContext.IncrementAssertCount();
                TestBase.LogEvent("Images match!");
            }

            else
            {
                TestBase.AddVerificationError("Images don't match", GetMergedImage());
            }
        }
    }
}