using System.ComponentModel;
using System.Drawing;
using System.Threading;
using Gallio.Common.Media;
using ProtoTest.Golem.Core;
using Timer = System.Timers.Timer;

namespace ProtoTest.Golem.WebDriver
{
    public class WebDriverRecorder
    {
        public int fps;
        public int frameDelayMs;
        public Bitmap lastImage;
        public BackgroundWorker screenshotGetter;
        public Size screensize;
        public int ticks;
        public Timer timer;
        public Video video;
        public BackgroundWorker videoBuilder;

        public WebDriverRecorder(int fps)
        {
            this.fps = fps;
            if (Config.Settings.runTimeSettings.Browser == WebDriverBrowser.Browser.Android)
            {
                screensize = new Size(300, 500);
            }
            else
            {
                screensize = new Size(1024, 768);
            }

            frameDelayMs = 1000/fps;
            //Log.Message("The current dimensions are : " + screensize.Width + " x " + screensize.Height);
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
                var bitmap = new Bitmap(TestBase.testData.driver.GetScreenshot());
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