using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Gallio.Common.Media;
using NUnit.Framework;

namespace ProtoTest.Golem.Core
{
    class Log
    {
        public static void Message(string text)
        {
            try
            {
                TestContext.WriteLine(string.Format("({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), text));
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static void Failure(string text)
        {
            try
            {
                TestContext.WriteLine(string.Format("({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), text));
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static void Image(Image image)
        {
            try
            {
                String path = Path.GetFullPath(Path.Combine(Config.Settings.reportSettings.reportPath, "screenshot_" + DateTime.Now.ToString("HHms") + ".png"));
                image.Save(path, ImageFormat.Png);
                Message("file:///" + path);
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static void Video(Video video)
        {
            try
            {
                String path = Path.GetFullPath(Path.Combine(Config.Settings.reportSettings.reportPath, "Video_" + Common.GetShortTestName(90) + DateTime.Now.ToString("HHms") + ".flv"));
                FileStream fs = new FileStream(path, FileMode.Create);
                video.Save(fs);
                Message("file:///" + path);
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static void FilePath(string file)
        {
            Message("file:///" + file);
        }

        public static void Html(string name, string html)
        {
            try
            {
                String path = Path.GetFullPath(Path.Combine(Config.Settings.reportSettings.reportPath, "html" + DateTime.Now.ToString("HHms") + ".html"));
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, html);
                }
                Message("file:///" + path);
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static void Warning(string message)
        {
            try
            {
                TestContext.WriteLine(string.Format("WARNING: ({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), message));
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static void Error(string message)
        {
            try
            {
                TestContext.WriteLine(string.Format("ERROR: ({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), message));
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static void Error(string message, Image image)
        {
            Error(message);
            Image(image);
        }
    }
}
