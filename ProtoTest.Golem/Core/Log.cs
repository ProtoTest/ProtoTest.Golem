using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Gallio.Common.Media;
using NUnit.Framework;

namespace ProtoTest.Golem.Core
{
    class Log
    {
        public static void Message(string text)
        {
            
            TestContext.WriteLine(string.Format("({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), text));
        }

        public static void Failure(string text)
        {
            TestContext.WriteLine(string.Format("({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), text));
        }

        public static void Image(Image image)
        {
            String path = Path.GetFullPath(Path.Combine(Config.Settings.reportSettings.reportPath, "screenshot_" + DateTime.Now.ToString("HHms") + ".png"));
            image.Save(path,ImageFormat.Png);
            Message(path);
        }

        public static void Video(Video video)
        {
            String path = Path.GetFullPath(Path.Combine(Config.Settings.reportSettings.reportPath, "Video_" + Common.GetShortTestName(90) + DateTime.Now.ToString("HHms") + ".flv"));
            FileStream fs = new FileStream(path, FileMode.Create);
            video.Save(fs);
            Message(path);
        }

        public static void FilePath(string file)
        {
            Message(file);
        }

        public static void Html(string name, string html)
        {
            String path = Path.GetFullPath(Path.Combine(Config.Settings.reportSettings.reportPath, "html" + DateTime.Now.ToString("HHms") + ".html"));
            if (!File.Exists(path))
            {
                File.WriteAllText(path, html);
            }
            Message(path);
        }

        public static void Warning(string message)
        {
            Message("WARNING: " + message);
        }

        public static void Error(string message)
        {
            Message("ERROR: " + message);
        }
    }
}
