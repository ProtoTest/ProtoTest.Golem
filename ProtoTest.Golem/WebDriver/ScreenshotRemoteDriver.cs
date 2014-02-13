using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Android;
using OpenQA.Selenium.Remote;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// implements the GetScreenshot() method to su pport remote screenshots.  
    /// </summary>
    public class ScreenshotRemoteWebDriver : RemoteWebDriver,
        ITakesScreenshot, IHasTouchScreen
    {
        public ScreenshotRemoteWebDriver(ICommandExecutor commandExecutor, ICapabilities desiredCapabilities)
            : base(commandExecutor, desiredCapabilities)
        {
        }

        public ScreenshotRemoteWebDriver(ICapabilities desiredCapabilities) : base(desiredCapabilities)
        {
        }

        public ScreenshotRemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities)
            : base(remoteAddress, desiredCapabilities)
        {
        }

        public ScreenshotRemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities, TimeSpan commandTimeout)
            : base(remoteAddress, desiredCapabilities, commandTimeout)
        {
        }
        private ITouchScreen touchScreen;

        /// <summary>
        /// Gets the device representing the touch screen.
        /// 
        /// </summary>
        public ITouchScreen TouchScreen
        {
            get
            {
                return this.touchScreen;
            }
        }

        /// <summary>
        ///     Gets a <see cref="Screenshot" /> object representing the image of the page on the screen.
        /// </summary>
        /// <returns>A <see cref="Screenshot" /> object containing the image.</returns>
        public Screenshot GetScreenshot()
        {
            try
            {
                // Get the screenshot as base64. 
                Response screenshotResponse = Execute(DriverCommand.Screenshot, null);
                string base64 = screenshotResponse.Value.ToString();

                // ... and convert it. 
                return new Screenshot(base64);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}