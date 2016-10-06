using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Golem.WebDriver
{
    /// <summary>
    ///     implements the GetScreenshot() method to su pport remote screenshots.
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

        /// <summary>
        ///     Gets the device representing the touch screen.
        /// </summary>
        public ITouchScreen TouchScreen { get; private set; }

        /// <summary>
        ///     Gets a <see cref="Screenshot" /> object representing the image of the page on the screen.
        /// </summary>
        /// <returns>A <see cref="Screenshot" /> object containing the image.</returns>
        public Screenshot GetScreenshot()
        {
            try
            {
                // Get the screenshot as base64. 
                var screenshotResponse = Execute(DriverCommand.Screenshot, null);
                var base64 = screenshotResponse.Value.ToString();

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