using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Common.Media;
using Gallio.Framework;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Core
{
    public class BackgroundVideoRecorder
    {
        public BackgroundWorker bgWorker;
        public Video video;
        public Size screensize;
        public int fps;
        public int frameDelayMs;
        public BackgroundVideoRecorder(int fps)
        {
            this.fps = fps;
            this.frameDelayMs = 1000 / fps;
            screensize = WebDriverTestBase.testData.driver.GetScreenshot().Size;
            Common.Log("The current dimensions are : " + screensize.Width + " x " + screensize.Height);
            video = new FlashScreenVideo(new FlashScreenVideoParameters(screensize.Width, screensize.Height, fps));
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.RunWorkerAsync();
        }

        void Stop()
        {
            bgWorker.CancelAsync();
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bgWorker.CancellationPending)
            {
                
                var bitmap = new Bitmap(WebDriverTestBase.testData.driver.GetScreenshot());
                Common.Log("Bitmap dimensions are : " + bitmap.Width + " x " + bitmap.Height);
                if(bitmap!=null)
                    video.AddFrame(new BitmapVideoFrame(bitmap));

                Thread.Sleep(frameDelayMs);
            }

        }
    }
}
