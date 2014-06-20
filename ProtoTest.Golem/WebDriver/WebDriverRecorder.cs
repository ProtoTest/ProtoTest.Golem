using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Gallio.Common.Media;
using Gallio.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using System.Timers;

namespace ProtoTest.Golem.WebDriver
{
    public class WebDriverRecorder
    {
        public BackgroundWorker videoBuilder;
        public BackgroundWorker screenshotGetter;
        public Video video;
        public Size screensize;
        public int fps;
        public int frameDelayMs;
        public int ticks;
        public System.Timers.Timer timer;
        public Bitmap lastImage;

        public WebDriverRecorder(int fps) 
        {
            this.fps = fps;
            if (WebDriverTestBase.browser == WebDriverBrowser.Browser.Android)
            {
                this.screensize = new Size(300, 500);
            }
            else
            {
                this.screensize = new Size(1024,768);
            }
            
            this.frameDelayMs = 1000/fps;
            //Common.Log("The current dimensions are : " + screensize.Width + " x " + screensize.Height);
            video = new FlashScreenVideo(new FlashScreenVideoParameters(screensize.Width, screensize.Height, fps));

            screenshotGetter = new BackgroundWorker();
            screenshotGetter.DoWork += screenshotGetter_DoWork;
            screenshotGetter.WorkerSupportsCancellation = true;
            screenshotGetter.RunWorkerAsync();

            videoBuilder = new BackgroundWorker();
            videoBuilder.DoWork += videoBuilder_DoWork;
            videoBuilder.WorkerSupportsCancellation = true;
            videoBuilder.RunWorkerAsync();

        }

        private void screenshotGetter_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!screenshotGetter.CancellationPending)
            {
                var bitmap = new Bitmap(WebDriverTestBase.testData.driver.GetScreenshot());
                lastImage = new Bitmap(Common.ResizeImage(bitmap, screensize.Width, screensize.Height));
            }
        }

        private void videoBuilder_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!videoBuilder.CancellationPending)
            {
                if (lastImage != null)
                    video.AddFrame(new BitmapVideoFrame(lastImage));
                Thread.Sleep(frameDelayMs);
            }
        }

        public void Stop()
        {
            screenshotGetter.CancelAsync();
            videoBuilder.CancelAsync();
        }

    }
}
