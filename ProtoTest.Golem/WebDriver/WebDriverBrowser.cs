using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    /// Contains all functionality relating to launching the webdriver browsers.
    /// </summary>
    public class WebDriverBrowser
    {
        public enum Browser
        {
            Firefox,
            Chrome,
            IE,
            Safari,
            Android,
            IPhone,
            IPad
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
                case Browser.IE:
                    driver = StartIEBrowser();
                    break;
                case Browser.Chrome:
                    driver = StartChromeBrowser();
                    break;
                case Browser.Safari:
                    driver = StartSafariBrowser();
                    break;
                case Browser.Firefox:
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
            if (resolution.Contains("Default"))
            {
                driver.Manage().Window.Maximize();
            }
            else
            {
                driver.Manage().Window.Size = Common.GetSizeFromResolution(resolution);
            }
        }

        public IWebDriver StartFirefoxBrowser()
        {
            var capabilities = new DesiredCapabilities();
            var proxy = new OpenQA.Selenium.Proxy();
            if (Config.Settings.httpProxy.useProxy)
            {
                proxy.HttpProxy = "localhost:" + TestBase.proxy.proxyPort;
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
                proxy.HttpProxy = "localhost:" + TestBase.proxy.proxyPort;
                options.Proxy = proxy;
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
                proxy.HttpProxy = "localhost:" + TestBase.proxy.proxyPort;
                options.Proxy = proxy;
                options.UsePerProcessProxy = true;
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
                proxy.HttpProxy = "localhost:" + TestBase.proxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }

            return new SafariDriver(options);
        }

        public DesiredCapabilities GetCapabilitiesForBrowser(Browser browser)
        {
            switch (browser)
            {
                case Browser.IE:
                    return DesiredCapabilities.InternetExplorer();
                case Browser.Chrome:
                    return DesiredCapabilities.Chrome();
                case Browser.Safari:
                    return DesiredCapabilities.Safari();
                case Browser.Android:
                    return DesiredCapabilities.Android();
                case Browser.IPhone:
                    var capabilities = DesiredCapabilities.IPhone();
                    capabilities.SetCapability("device", "iphone");
                    capabilities.SetCapability("app", "safari");
                    return capabilities;
                case Browser.IPad:
                    return DesiredCapabilities.IPad();
                case Browser.Firefox:
                default:
                    return DesiredCapabilities.Firefox();
            }
        }

        public IWebDriver LaunchRemoteBrowser(Browser browser, string host)
        {
            String URIStr = null;
            DesiredCapabilities desiredCapabilities = GetCapabilitiesForBrowser(browser);
            desiredCapabilities.SetCapability(CapabilityType.Platform,Config.Settings.runTimeSettings.Platform);
            desiredCapabilities.SetCapability(CapabilityType.Version, Config.Settings.runTimeSettings.Version);
            
            if (host.Equals(Config.Settings.browserStackSettings.BrowserStackRemoteURL))
            {
                String user = Config.Settings.browserStackSettings.BrowserStack_User;
                String key = Config.Settings.browserStackSettings.BrowserStack_Key;
                String os = Config.Settings.browserStackSettings.BrowserStack_OS;
                String os_version = Config.Settings.browserStackSettings.BrowserStack_OS_Version;

                if (user == null) { throw new ConfigurationErrorsException("Framework configured to use BrowserStack, however 'BrowserStack_User' is not defined in App.config"); }
                if (key == null) { throw new ConfigurationErrorsException("Framework configured to use BrowserStack, however 'BrowserStack_Key' is not defined in App.config"); }
                if (os == null) { throw new ConfigurationErrorsException("Framework configured to use BrowserStack, however 'BrowserStack_OS' is not defined in App.config"); }
                if (os_version == null) { throw new ConfigurationErrorsException("Framework configured to use BrowserStack, however 'BrowserStack_OS_Version' is not defined in App.config"); }

                // Browser stack does not require a remote host port
                desiredCapabilities.SetCapability("browserstack.user", user);
                desiredCapabilities.SetCapability("browserstack.key", key);
                desiredCapabilities.SetCapability("os", os);
                desiredCapabilities.SetCapability("os_version", os_version);

                URIStr = string.Format("http://{0}/wd/hub", host);
                Common.Log(string.Format("Starting {0} browser on host : {1}", browser, host));
            }
            else
            {
                URIStr = string.Format("http://{0}:{1}/wd/hub", host, Config.Settings.runTimeSettings.RemoteHostPort);
                Common.Log(string.Format("Starting {0} browser on host : {1}:{2}", browser, host, Config.Settings.runTimeSettings.RemoteHostPort));
            }
           
            var remoteAddress = new Uri(URIStr);
            return new EventedWebDriver(new ScreenshotRemoteWebDriver(remoteAddress, desiredCapabilities)).driver;
        }

        public IWebDriver LaunchAppDriver(string appPath, string package, string activity)
        {
            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, "");
            capabilities.SetCapability("device", "Android");
            capabilities.SetCapability("app", appPath);
            capabilities.SetCapability("appPackage", package);
            capabilities.SetCapability("appActivity", activity);

            var eDriver = new EventedWebDriver(driver);

            return eDriver.driver;
        }
    }
}