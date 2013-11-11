using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;

namespace ProtoTest.Golem.WebDriver
{
    public class ScreenshotRemoteWebDriver : RemoteWebDriver, 
ITakesScreenshot 
{
        public ScreenshotRemoteWebDriver(ICommandExecutor commandExecutor, ICapabilities desiredCapabilities) : base(commandExecutor, desiredCapabilities)
        {
        }

        public ScreenshotRemoteWebDriver(ICapabilities desiredCapabilities) : base(desiredCapabilities)
        {
        }

        public ScreenshotRemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities) : base(remoteAddress, desiredCapabilities)
        {
        }

        public ScreenshotRemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities, TimeSpan commandTimeout) : base(remoteAddress, desiredCapabilities, commandTimeout)
        {
        }

        /// <summary> 
        /// Gets a <see cref="Screenshot"/> object representing the image of the page on the screen. 
        /// </summary> 
        /// <returns>A <see cref="Screenshot"/> object containing the image.</returns> 
        public Screenshot GetScreenshot() 
        { 
            // Get the screenshot as base64. 
            Response screenshotResponse = this.Execute(DriverCommand.Screenshot, null); 
            string base64 = screenshotResponse.Value.ToString(); 

            // ... and convert it. 
            return new Screenshot(base64); 
        } 
} 
}
