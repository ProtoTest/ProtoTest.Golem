using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Security;
using Gallio.Common.Text;
using Gallio.Framework;
using Gallio.Model.Tree;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver.Elements.Images
{
    public class ElementImages
    {
        public static bool UpdateImages = Config.Settings.imageCompareSettings.updateImages;
        private readonly Image liveImage;
        private readonly Image storedImage;
        public Element element;
        public float difference;

        public ElementImages(Element element)
        {
            
            this.element = element;
            CreateDirectory();
            liveImage = GetLiveImage();
            storedImage = GetStoredImage();
        }

        private string ImageLocation
        {
            get { return Common.GetCodeDirectory() + "\\ElementImages\\" + TestBase.GetCurrentClassName() + "." + element.name.Replace(" ", "") + ".bmp"; }
        }

        public Image GetDifferenceImage()
        {
            Bitmap bmp = new Bitmap(liveImage.Width,liveImage.Height);
            return liveImage.GetDifferenceOverlayImage(storedImage, true, true).Resize(storedImage.Width, storedImage.Height);
        }

        public bool ImagesMatch()
        {
            if ((!File.Exists(ImageLocation)) || (UpdateImages))
            {
                UpdateImage();
            }
            //TestLog.EmbedImage(this.element.name+"original" + Common.GetRandomString(),this.storedImage);
            TestLog.EmbedImage(this.element.name + "combined" + Common.GetRandomString(), GetMergedImage());
            this.difference = ImageComparer.ImageComparePercentage(this.storedImage, this.liveImage, Config.Settings.imageCompareSettings.fuzziness);
            //Common.Log(string.Format("{0} difference is {1}",this.element.name,this.difference));
            return difference < Config.Settings.imageCompareSettings.accuracy;
        }

        public Image GetMergedImage()
        {
            var overlayImage = OverlayImages(this.liveImage,GetDifferenceImage());
            var mergedImage = CombineImages(this.storedImage, this.liveImage, overlayImage);
            return mergedImage;
        }

        private Image CombineImages(Image image1, Image image2, Image image3)
        {
            //Common.LogImage(image1);
            //Common.LogImage(image2);
            //Common.LogImage(image3);
            int newWidth = image1.Width + image2.Width + image3.Width;
            int newHeight = image1.Height;
            Bitmap bmp = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(image1, new Point(0, 0));
                gr.DrawImage(image2, new Point(image1.Width, 0));
                gr.DrawImage(image3, new Point(image2.Width+image1.Width, 0));
            }
            return bmp;
        }

        public Image GetLiveImage()
        {
            return GetImage();
        }

        public Image GetStoredImage()
        {
            if (File.Exists(ImageLocation))
                return Image.FromFile(ImageLocation);
            else
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
                Common.Log("Exception saving : " + e.Message);
            }
        }

        public Image GetImage()
        {
            var size = new Size(element.Size.Width, element.Size.Height);
            if (element.Displayed == false)
                throw new BadImageFormatException(string.Format("Could not create image for element {0} as it is hidden", element.name));
            var cropRect = new Rectangle(element.Location, size);
           // Common.Log("Rect is " + cropRect);

            return cropImage(TestBase.testData.driver.GetScreenshot(), cropRect);
        }

        private Image cropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public void AttachImage()
        {
            TestLog.AttachImage(element.name, GetImage());
        }
    }
}