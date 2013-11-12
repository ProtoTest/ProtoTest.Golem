using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Android;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class WebDriverBrowser
    {
        public enum Browser
        {
            Firefox,
            Chrome,
            IE,
            Safari,
            Android,
            IPhone
        }

        public IWebDriver driver;

        public static Browser getBrowserFromString(string name)
        {
            return (Browser) Enum.Parse(typeof (Browser), name);
        }

        public IWebDriver LaunchBrowser(Browser browser)
        {
            switch (browser)
            {
                case Browser.Firefox:
                    driver = StartFirefoxBrowser();
                    break;
                case Browser.IE:
                    driver = StartIEBrowser();
                    break;
                case Browser.Chrome:
                    driver = StartChromeBrowser();
                    break;
                case Browser.Safari:
                    driver = StartSafariBrowser();
                    break;
                default:
                    driver = StartFirefoxBrowser();
                    break;
            }
            driver.Manage().Cookies.DeleteAllCookies();
            SetBrowserSize();
            var eDriver = new EventedWebDriver(driver);
            return eDriver.driver;
            }

        private void SetBrowserSize()
        {
            string resolution = Config.Settings.runTimeSettings.BrowserResolution;
            if(resolution.Contains("Default")) 
                driver.Manage().Window.Maximize();
            else
                driver.Manage().Window.Size = Common.GetSizeFromResolution(resolution);
        }

        public static IWebDriver StartFirefoxBrowser()
        {
            var capabilities = new DesiredCapabilities();
            var proxy = new OpenQA.Selenium.Proxy();
            if (Config.Settings.httpProxy.useProxy)
            {
                proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
                capabilities.SetCapability("proxy", proxy);
            }
            return new FirefoxDriver(capabilities);
        }

        public IWebDriver StartChromeBrowser()
        {
            var options = new ChromeOptions();
            // Add the WebDriver proxy capability.
            if (Config.Settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }
            return new ChromeDriver(options);
        }

        public IWebDriver StartIEBrowser()
        {
            var options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            options.IgnoreZoomLevel = true;
            if (Config.Settings.httpProxy.startProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }

            return new InternetExplorerDriver(options);
        }


        public IWebDriver StartSafariBrowser()
        {
            var options = new SafariOptions();
            // Add the WebDriver proxy capability.
            if (Config.Settings.httpProxy.startProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }
            return new SafariDriver(options);
        }

        public DesiredCapabilities GetCapabilitiesForBrowser(Browser browser)
        {
            switch (browser)
            {
                case Browser.Firefox:
                    return DesiredCapabilities.Firefox();
                case Browser.IE:
                    return DesiredCapabilities.InternetExplorer();
                case Browser.Chrome:
                    return DesiredCapabilities.Chrome();
                case Browser.Safari:
                    return DesiredCapabilities.Safari();
                case Browser.Android:
                    return DesiredCapabilities.Android();
                default:
                    return DesiredCapabilities.Firefox();
            }
        }

        public IWebDriver LaunchRemoteBrowser(Browser browser, string host)
        {
            DesiredCapabilities desiredCapabilities = GetCapabilitiesForBrowser(browser);
            string port = "4444";
            if (browser == Browser.Android)
            {
                port = "8080";
                //var driver= new AndroidDriver(new Uri(string.Format("http://{0}:{1}/wd/hub",Config.Settings.runTimeSettings.HostIp,port)));
                //driver.Orientation=ScreenOrientation.Landscape;
               // return driver;
            }
                
            var remoteAddress = new Uri(string.Format("http://{0}:{1}/wd/hub",Config.Settings.runTimeSettings.HostIp,port));
           
           return new EventedWebDriver(new ScreenshotRemoteWebDriver(remoteAddress, desiredCapabilities)).driver;
        }

        public IWebDriver LaunchAppDriver(string appPath, string package, string activity)
        {
            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, "");
            capabilities.SetCapability("device", "Android");
            capabilities.SetCapability("app", appPath);
            capabilities.SetCapability("app-package", package);
            capabilities.SetCapability("app-activity", activity);

            var eDriver = new EventedWebDriver(driver);
            return eDriver.driver;
        }
    }
}