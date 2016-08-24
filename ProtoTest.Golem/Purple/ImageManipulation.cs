using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Golem.Core;
using Golem.Purple.Elements;
using Golem.Purple.PurpleElements;
using Golem.WebDriver.Elements.Images;

namespace Golem.Purple
{
    public class ImageManipulation
    {
        public static bool UpdateImages = Config.settings.imageCompareSettings.updateImages;
        private readonly Image liveImage;
        private readonly string snapshotIndexFormat = "00";
        //private readonly Image storedImage;
        public float difference;
        public string differenceString;
        public string DirLocation;
        public IPurpleElement element;
        public string FileName;
        public int snapshotIndex;

        /// <summary>
        ///     Creates a new object with a reference to a PurpleElement object.
        ///     It erase all the previous snapshots that might have been taken.
        /// </summary>
        /// <param name="element">purple element</param>
        public ImageManipulation(IPurpleElement element)
        {
            this.element = element;
            CreateDirectory();
            liveImage = GetLiveImage();
            //storedImage = GetStoredImage();
            DirLocation = Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                          "\\ElementImages\\";
            FileName = element.ElementName.Replace(" ", "");
            DeleteAllSnapshots();
        }

        /// <summary>
        ///     Creates a new object with a reference to a PurpleElement object.
        ///     It keeps all the previous snapshots that might have been taken.
        /// </summary>
        /// <param name="element">purple element</param>
        /// <param name="keepOld">indicates that we like to keep previous snapshots</param>
        public ImageManipulation(IPurpleElement element, bool keepOld)
        {
            this.element = element;
            CreateDirectory();
            liveImage = GetLiveImage();
            //storedImage = GetStoredImage();
            DirLocation = Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                          "\\ElementImages\\";
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

        // Functions copied and modified from ElementImageComparer

        private string ImageLocation
        {
            get
            {
                return Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                       "\\ElementImages\\" + element.ElementName.Replace(" ", "") + ".bmp";
            }
        }

        /// <summary>
        ///     Takes a snapshot and save it to a file
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
        ///     Saves an image to disk
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
        ///     Returns the latest snapshot index from disk
        /// </summary>
        public int GetLastStoredSnapshotIndex()
        {
            var files = Directory.GetFiles(DirLocation, FileName + "-Snapshot*");
            var index = 0;
            if (files.Length > 0)
            {
                var sortedFiles = from s in files orderby s descending select s;
                var latestFile = sortedFiles.ElementAt(0);
                index = int.Parse(latestFile.Substring(latestFile.IndexOf("-Snapshot") + 9, 2));
            }
            return index;
        }

        /// <summary>
        ///     delete all the snapshots from disk
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
        ///     Get he image content based on the index of the snapshot taken
        /// </summary>
        /// <param name="snapshot">snapshot order</param>
        /// <returns>Image</returns>
        public Image GetStoredSnapshot(int snapshot)
        {
            var fName = FileName + "-Snapshot" + snapshot.ToString(snapshotIndexFormat);
            if (File.Exists(DirLocation + fName + ".bmp"))
                return Image.FromFile(DirLocation + fName + ".bmp");
            return null;
        }

        /// <summary>
        ///     Returns the image of the latest stored snapshot
        /// </summary>
        /// <returns>Image</returns>
        public Image GetLatestStoredSnapshot()
        {
            var fName = FileName + "-Snapshot" + snapshotIndex.ToString(snapshotIndexFormat);
            if (File.Exists(DirLocation + fName))
                return Image.FromFile(DirLocation + fName + ".bmp");
            return null;
        }

        /// <summary>
        ///     Gets an image stored on disk. Do not include file extension
        /// </summary>
        /// <param name="imageName">Name of the file without extension</param>
        /// <returns>Image</returns>
        public Image GetImageFromDisk(string imageName)
        {
            var fName = imageName + ".bmp";
            if (File.Exists(DirLocation + fName))
                return Image.FromFile(DirLocation + fName);
            return null;
        }

        /// <summary>
        ///     Retruns the image which is the result of the difference between 2 given images.
        ///     The resultimg image will have the size of image 2
        /// </summary>
        /// <param name="image1">Image 1</param>
        /// <param name="image2">Image 2</param>
        /// <returns>Image resulting from comparisson</returns>
        public Image GetDifferenceImage(Image image1, Image image2)
        {
            var bmp = new Bitmap(image2.Width, image2.Height);
            return liveImage.GetDifferenceOverlayImage(image1).Resize(image2.Width, image2.Height);
        }

        /// <summary>
        ///     Retruns the image which is the result of the difference between an image previoulsy captured and the current image
        ///     on screen.
        /// </summary>
        /// <param name="oldImage">the image we want to compare with</param>
        /// <returns>Image</returns>
        public Image GetDifferenceImage(Image oldImage)
        {
            var bmp = new Bitmap(liveImage.Width, liveImage.Height);
            return liveImage.GetDifferenceOverlayImage(oldImage).Resize(liveImage.Width, liveImage.Height);
        }

        /// <summary>
        ///     Retruns the image which is the result of the difference between a snapshot previoulsy captured and the current
        ///     image on screen.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <returns></returns>
        public Image GetDifferenceImageAgainstSnapshot(int snapshot)
        {
            var oldImage = GetStoredSnapshot(snapshot);
            var bmp = new Bitmap(liveImage.Width, liveImage.Height);
            return liveImage.GetDifferenceOverlayImage(oldImage).Resize(liveImage.Width, liveImage.Height);
        }

        /// <summary>
        ///     Compares 2 images and returns a boolean value indicating if the change is greater than an accuracy value.
        /// </summary>
        /// <param name="source1">Image 1</param>
        /// <param name="source2">Image 2</param>
        /// <returns>boolean</returns>
        public bool ImagesMatch(Image source1, Image source2)
        {
            difference = ImageComparer.ImageComparePercentage(source1, source2,
                Config.settings.imageCompareSettings.fuzziness);
            differenceString = (difference*100).ToString("0.##\\%");
            return difference < Config.settings.imageCompareSettings.accuracy;
        }

        /// <summary>
        ///     Compares 2 images, and returns a boolean value indicating if the change is greater than an accuracy value.
        /// </summary>
        /// <param name="source1">Image 1</param>
        /// <param name="source2">Image 2</param>
        /// <param name="accuracy">Accuracy of the comparisson</param>
        /// <returns>boolean</returns>
        public bool ImagesMatch(Image source1, Image source2, float accuracy)
        {
            difference = ImageComparer.ImageComparePercentage(source1, source2,
                Config.settings.imageCompareSettings.fuzziness);
            differenceString = (difference*100).ToString("0.##\\%");
            return difference < accuracy;
        }

        /// <summary>
        ///     Compares 2 images and return the difference expressed as a percentage
        /// </summary>
        /// <param name="source1">Image 1</param>
        /// <param name="source2">Image 2</param>
        /// <param name="fuzzines">Image 2</param>
        /// <returns>float value that indicates the difference expressed as percentage</returns>
        public string ImagesMatchReturnValue(Image source1, Image source2, int fuzzines = -1)
        {
            byte fuz;
            if (fuzzines == -1)
            {
                fuz = Config.settings.imageCompareSettings.fuzziness;
            }
            else
            {
                fuz = Byte.Parse(fuzzines.ToString());
            }
            difference = ImageComparer.ImageComparePercentage(source1, source2, fuz);
            differenceString = (difference*100).ToString("0.##");
            return differenceString;
        }

        /// <summary>
        ///     Compares an image captured on a previous snapshot and the current image, and returns a boolean value indicating if
        ///     the change is greater than an accuracy value.
        /// </summary>
        /// <param name="snapshot">number of snapshot</param>
        /// <returns>boolean</returns>
        public bool ImagesMatchCurrentAndSnapshot(int snapshot)
        {
            var current = GetLiveImage();
            var oldImage = GetStoredSnapshot(snapshot);
            return ImagesMatch(current, oldImage);
        }

        /// <summary>
        ///     Compares an image captured on a previous snapshot and the current image, and returns a boolean value indicating if
        ///     the change is greater than an accuracy value.
        /// </summary>
        /// <param name="snapshot">number of snapshot</param>
        /// <param name="accuracy">accuracy to campare against</param>
        /// <returns>boolean</returns>
        public bool ImagesMatchCurrentAndSnapshot(int snapshot, float accuracy)
        {
            var current = GetLiveImage();
            var oldImage = GetStoredSnapshot(snapshot);
            return ImagesMatch(current, oldImage, accuracy);
        }

        /// <summary>
        ///     Creates a combination of 2 images side by side
        /// </summary>
        /// <param name="image1">image 1</param>
        /// <param name="image2">image 2</param>
        /// <returns>Image result</returns>
        public Image CombineImages(Image image1, Image image2)
        {
            var newWidth = image1.Width + image2.Width;
            var newHeight = image1.Height;
            var bmp = new Bitmap(newWidth, newHeight);
            using (var gr = Graphics.FromImage(bmp))
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

        /// <summary>
        ///     Returns an image that represents the screen at this moment
        /// </summary>
        /// <returns>Image</returns>
        public Image GetLiveImage()
        {
            var img = element.UIAElement.GetImage();
            var cropArea = ((PurpleElementBase) element).Bounds.ToRectangle();
            return cropImage(img, cropArea);
        }

        /*
        public Image GetStoredImage()
        {
            if (File.Exists(ImageLocation))
                return Image.FromFile(ImageLocation);
            return GetLiveImage();
        }
        */

        public Image cropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }
    }
}