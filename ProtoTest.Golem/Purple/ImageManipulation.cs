using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Gallio.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.Elements;
using ProtoTest.Golem.WebDriver.Elements.Images;
using Image = System.Drawing.Image;

namespace ProtoTest.Golem.Purple
{
    public class ImageManipulation
    {
        public static bool UpdateImages = Config.Settings.imageCompareSettings.updateImages;
        private readonly Image liveImage;
        private readonly Image storedImage;
        public float difference;
        public string differenceString;
        public IPurpleElement element;
        public string DirLocation;
        public string FileName;
        public int snapshotIndex;
        string snapshotIndexFormat = "00";

        /// <summary>
        /// Creates a new object with a reference to a PurpleElement object.
        /// It erase all the previous snapshots that might have been taken.
        /// </summary>
        /// <param name="element">purple element</param>
        public ImageManipulation(IPurpleElement element)
        {
            this.element = element;
            CreateDirectory();
            liveImage = GetLiveImage();
            storedImage = GetStoredImage();
            DirLocation = Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") + "\\ElementImages\\";
            FileName = element.ElementName.Replace(" ", "");
            DeleteAllSnapshots();
        }

        /// <summary>
        /// Creates a new object with a reference to a PurpleElement object.
        /// It keeps all the previous snapshots that might have been taken.
        /// </summary>
        /// <param name="element">purple element</param>
        /// <param name="keepOld">indicates that we like to keep previous snapshots</param>
        public ImageManipulation(IPurpleElement element, bool keepOld)
        {
            this.element = element;
            CreateDirectory();
            liveImage = GetLiveImage();
            storedImage = GetStoredImage();
            DirLocation = Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") + "\\ElementImages\\";
            FileName = element.ElementName.Replace(" ", "");
            if (keepOld)
            {
                snapshotIndex = GetLastStoredSnapshotIndex();
            }
            else
            {
                DeleteAllSnapshots();
            }
        }

        /// <summary>
        /// Takes a snapshot and save it to a file
        /// </summary>
        /// <returns></returns>
        public int TakeSnapshot()
        {
            snapshotIndex++;
            var currentImage = GetLiveImage();
            Save(currentImage, FileName + "-Snapshot" + snapshotIndex.ToString(snapshotIndexFormat));
            return snapshotIndex;
        }


        /// <summary>
        /// Saves an image to disk
        /// </summary>
        /// <param name="image">Image content</param>
        /// <param name="imageName">Image filename</param>
        public void Save(Image image, string imageName)
        {
            using (var tempImage = new Bitmap(image))
            {
                tempImage.Save(DirLocation + imageName + ".bmp", ImageFormat.Bmp);
            }
        }

        public int GetCurrentSnapshotIndex()
        {
            return snapshotIndex;
        }

        /// <summary>
        /// Returns the latest snapshot index from disk 
        /// </summary>
        public int GetLastStoredSnapshotIndex()
        {
            var files = Directory.GetFiles(DirLocation, FileName + "-Snapshot*");
            int index = 0;
            if (files.Length > 0)
            {
                var sortedFiles = from s in files orderby s descending select s;
                string latestFile = sortedFiles.ElementAt(0);
                index = int.Parse(latestFile.Substring(latestFile.IndexOf("-Snapshot") + 9, 2));
            }
            return index;
        }

        /// <summary>
        /// delete all the snapshots from disk
        /// </summary>
        public void DeleteAllSnapshots()
        {
            var files2 = Directory.GetFiles(DirLocation, FileName + "-Snapshot*");
            foreach (var currentFile in files2)
            {
                File.Delete(currentFile);
            }
            snapshotIndex = 0;
        }

        /// <summary>
        /// Get he image content based on the index of the snapshot taken
        /// </summary>
        /// <param name="snapshot">snapshot order</param>
        /// <returns>Image</returns>
        public Image GetStoredSnapshot(int snapshot)
        {
            string fName = FileName + "-Snapshot" + snapshot.ToString(snapshotIndexFormat);
            if (File.Exists(DirLocation+fName+".bmp"))
                return Image.FromFile(DirLocation + fName + ".bmp");
            return null;
        }

        /// <summary>
        /// Returns the image of the latest stored snapshot
        /// </summary>
        /// <returns>Image</returns>
        public Image GetLatestStoredSnapshot()
        {
            string fName = FileName + "-Snapshot" + snapshotIndex.ToString(snapshotIndexFormat);
            if (File.Exists(DirLocation + fName))
                return Image.FromFile(DirLocation + fName + ".bmp");
            return null;
        }
        
        /// <summary>
        /// Retruns the image which is the result of the difference between an image previoulsy captured and the current image on screen.
        /// </summary>
        /// <param name="oldImage">the image we want to compare with</param>
        /// <returns>Image</returns>
        public Image GetDifferenceImage(Image oldImage)
        {
            var bmp = new Bitmap(liveImage.Width, liveImage.Height);
            return liveImage.GetDifferenceOverlayImage(oldImage).Resize(storedImage.Width, storedImage.Height);
        }

        /// <summary>
        /// Retruns the image which is the result of the difference between a snapshot previoulsy captured and the current image on screen.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <returns></returns>
        public Image GetDifferenceImageAgainstSnapshot(int snapshot)
        {
            var oldImage = GetStoredSnapshot(snapshot);
            var bmp = new Bitmap(liveImage.Width, liveImage.Height);
            return liveImage.GetDifferenceOverlayImage(oldImage).Resize(storedImage.Width, storedImage.Height);
        }

        /// <summary>
        /// Compares 2 images 
        /// </summary>
        /// <param name="source1">Image 1</param>
        /// <param name="source2">Image 2</param>
        /// <returns>boolean</returns>
        public bool ImagesMatch(Image source1, Image source2)
        {
            difference = ImageComparer.ImageComparePercentage(source1, source2, Config.Settings.imageCompareSettings.fuzziness);
            differenceString = (difference * 100).ToString("0.##\\%");
            return difference < Config.Settings.imageCompareSettings.accuracy;
        }

        /// <summary>
        /// Compares an image captured on a previous snapshot and the current image
        /// </summary>
        /// <param name="snapshot">number of snapshot</param>
        /// <returns>boolean</returns>
        public bool ImageMatchCurrentAndSnapshot(int snapshot)
        {
            Image current = GetLiveImage();
            Image oldImage = GetStoredSnapshot(snapshot);
            return ImagesMatch(current, oldImage);
        }

        /// <summary>
        /// Creates a combination of 2 images side by side
        /// </summary>
        /// <param name="image1">image 1</param>
        /// <param name="image2">image 2</param>
        /// <returns>Image result</returns>
        public Image CombineImages(Image image1, Image image2)
        {
            int newWidth = image1.Width + image2.Width;
            int newHeight = image1.Height;
            var bmp = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(image1, new Point(0, 0));
                gr.DrawImage(image2, new Point(image1.Width, 0));
            }
            return bmp;
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



        // Functions copied from ElementImageComparer

        private string ImageLocation
        {
            get
            {
                return Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") + "\\ElementImages\\" + element.ElementName.Replace(" ", "") + ".bmp";
            }
        }

        public void CreateDirectory()
        {
            if (!Directory.Exists(Common.GetCodeDirectory() + "\\ElementImages"))
                Directory.CreateDirectory(Common.GetCodeDirectory() + "\\ElementImages");
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

        /*



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





        public Image GetStoredImageByName(string name)
        {
            if (File.Exists(ImageLocation))
                return Image.FromFile(DirLocation + name + ".bmp");
            return null;
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

        public void Save(Image image, string imageName)
        {
            using (var tempImage = new Bitmap(image))
            {
                tempImage.Save(DirLocation+imageName+".bmp", ImageFormat.Bmp);
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
        */
    }
}
