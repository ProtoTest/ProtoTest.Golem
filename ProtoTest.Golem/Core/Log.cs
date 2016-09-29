using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Gallio.Common.Media;
using NUnit.Framework;

namespace Golem.Core
{
    public class Log
    {
        public static void Message(string text, ActionList.Action.ActionType type=ActionList.Action.ActionType.Other)
        {
            try
            {
                TestBase.overlay.Text = text;
                Debug.WriteLine(string.Format("DEBUG : ({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), text));
//              TestContext.WriteLine(string.Format("({0}) : {1}", DateTime.Now.ToString("HH:mm:ss::ffff"), text));
                TestBase.testData.LogEvent(text, type);
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
                Message("FAILURE: " + text, ActionList.Action.ActionType.Error);
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        public static string Image(Image image)
        {
            try
            {
                String path = Path.GetFullPath(Path.Combine(Config.settings.reportSettings.reportPath, "screenshot_" + DateTime.Now.ToString("HHms") + ".png"));
                image.Save(path, ImageFormat.Png);
                Message("file:///" + path, ActionList.Action.ActionType.Image);
                return path;
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
                return "";
            }
        }

        public static string Video(Video video)
        {
            try
            {
                String path = Path.GetFullPath(Path.Combine(Config.settings.reportSettings.reportPath, "Video_" + Common.GetShortTestName(90) + DateTime.Now.ToString("HHms") + ".flv"));
                FileStream fs = new FileStream(path, FileMode.Create);
                video.Save(fs);
                Message(path, ActionList.Action.ActionType.Video);
                return path;
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
                return "";
            }
        }

        public static void FilePath(string file)
        {
            Message("file:///" + file, ActionList.Action.ActionType.Link);
        }

        public static void Html(string name, string html)
        {
            try
            {
                String path = Path.GetFullPath(Path.Combine(Config.settings.reportSettings.reportPath, "html" + DateTime.Now.ToString("HHms") + ".html"));
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, html);
                }
                Message("file:///" + path, ActionList.Action.ActionType.Link);
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
                Message("WARNING: " + message, ActionList.Action.ActionType.Warning);
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
                Message("ERROR: " + message, ActionList.Action.ActionType.Error);
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
