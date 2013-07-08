using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Android;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support;


namespace Golem.Framework
{
    public class WebDriverBrowser
    {
        public enum Browser { Firefox, Chrome, IE, Safari, Android, IPhone }
        public IWebDriver driver;
        public WebDriverBrowser() { }
        public static Browser getBrowserFromString(string name)
        {
            return (Browser)Enum.Parse(typeof(Browser), name);
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
            var eDriver = new EventedWebDriver(driver);
            return eDriver.driver;
            return driver;

        }

        public static IWebDriver StartFirefoxBrowser()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            if (Config.Settings.httpProxy.startProxy)
            {
                Proxy proxy = new Proxy();
                proxy.SslProxy = "localhost:" + Config.Settings.httpProxy.sslProxyPort;
                proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
                capabilities.SetCapability("proxy", proxy);
            }
            
            return new FirefoxDriver(capabilities);
        }

        public IWebDriver StartChromeBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            // Add the WebDriver proxy capability.
            if (Config.Settings.httpProxy.startProxy)
            {
                Proxy proxy = new Proxy();
                proxy.SslProxy = "localhost:" + Config.Settings.httpProxy.sslProxyPort;
                proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
                options.AddAdditionalCapability("proxy",proxy);
            } 
            return new ChromeDriver(options);
        }

        public IWebDriver StartIEBrowser()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            options.IgnoreZoomLevel = true;
            if (Config.Settings.httpProxy.startProxy)
            {
            Proxy proxy = new Proxy();
            proxy.SslProxy = "localhost:" + Config.Settings.httpProxy.sslProxyPort;
            proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
            options.AddAdditionalCapability("proxy",proxy);    
            }
            
            return new InternetExplorerDriver(options);
        }


        public IWebDriver StartSafariBrowser()
        {
            SafariOptions options = new SafariOptions();
            // Add the WebDriver proxy capability.
            if (Config.Settings.httpProxy.startProxy)
            {
                Proxy proxy = new Proxy();
                proxy.SslProxy = @"localhost:" + Config.Settings.httpProxy.sslProxyPort;
                proxy.HttpProxy = "localhost:" + Config.Settings.httpProxy.proxyPort;
                options.AddAdditionalCapability("proxy",proxy);
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
            var remoteAddress = new Uri("http://"+ host +":4444/wd/hub");
            return new EventedWebDriver(new RemoteWebDriver(remoteAddress, desiredCapabilities)).driver;
        }

        public IWebDriver LaunchAppDriver(string appPath, string package, string activity)
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
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
