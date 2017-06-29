using System;
using System.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Edge;
using Golem.Core;

namespace Golem.WebDriver
{
    /// <summary>
    ///     Contains all functionality relating to launching the webdriver browsers.
    /// </summary>
    public class WebDriverBrowser
    {
        public enum Browser
        {
            Firefox,
            Chrome,
            IE,
            Edge,
            Safari,
            PhantomJS,
            Android,
            IPhone,
            IPad,
            None
        }

        public IWebDriver driver;

        public static Browser getBrowserFromString(string name)
        {
            return (Browser) Enum.Parse(typeof(Browser), name);
        }

        public IWebDriver LaunchBrowser(Browser browser)
        {
            switch (browser)
            {
                case Browser.IE:
                    driver = StartIEBrowser();
                    break;
                case Browser.Edge:
                    driver = StartEdgeBrowser();
                    break;
                case Browser.Chrome:
                    driver = StartChromeBrowser();
                    break;
                case Browser.Safari:
                    driver = StartSafariBrowser();
                    break;
                case Browser.PhantomJS:
                    driver = StartPhantomJSBrowser();
                    break;
                default:
                    driver = StartFirefoxBrowser();
                    break;
            }

            // deleting cookies throws an exception when trying to delete cookies before site is loaded
            try
            {
                driver.Manage().Cookies.DeleteAllCookies();
            }
            catch (Exception e)
            {

                TestContext.WriteLine(e.Message);
            }

            SetBrowserSize();
            var eDriver = new EventedWebDriver(driver);

            return eDriver.driver;
        }

        private void SetBrowserSize()
        {
            var resolution = Config.settings.runTimeSettings.BrowserResolution;
            if (resolution.Contains("Default"))
            {
                Log.Message("Maximizing Browser");
                driver.Manage().Window.Maximize();
            }
            else
            {
                Log.Message($"Setting Browser Size to {resolution}");
                driver.Manage().Window.Size = Common.GetSizeFromResolution(resolution);
            }
        }

        public IWebDriver StartFirefoxBrowser()
        {
            // var capabilities = new DesiredCapabilities();
            FirefoxProfile profile = new FirefoxProfile();
            var proxy = new OpenQA.Selenium.Proxy();
            if (Config.settings.httpProxy.useProxy)
            {
                proxy.HttpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                // capabilities.SetCapability("proxy", proxy);
                profile.SetProxyPreferences(proxy);
            }

            // this is first solution from http://stackoverflow.com/questions/33937067/firefox-webdriver-opens-first-run-page-all-the-time
            // firefoxProfile.SetPreference("browser.startup.homepage", "about:blank");
            // firefoxProfile.SetPreference("startup.homepage_welcome_url", "about:blank");
            // firefoxProfile.SetPreference("startup.homepage_welcome_url.additional", "about:blank");
            // capabilities.SetCapability(FirefoxDriver.ProfileCapabilityName, firefoxProfile.ToBase64String());

            // this is second solution from http://stackoverflow.com/questions/33937067/firefox-webdriver-opens-first-run-page-all-the-time
            // firefoxProfile.SetPreference("xpinstall.signatures.required", false);
            // firefoxProfile.SetPreference("toolkit.telemetry.reportingpolicy.firstRun", false);

            // FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("browser.startup.homepage_override.mstone", "ignore");
            profile.SetPreference("startup.homepage_welcome_url", "about:blank");
            profile.SetPreference("startup.homepage_welcome_url.additional", "about:blank");
            profile.SetPreference("browser.startup.homepage", "about:blank");

            return new FirefoxDriver(profile);
        }

        public IWebDriver StartChromeBrowser()
        {
            var options = new ChromeOptions();

            // Add the WebDriver proxy capability.
            if (Config.settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.Proxy = proxy;
            }

            if (!string.IsNullOrEmpty(Config.settings.browserSettings.UserAgent))
            {
                options.AddArgument($"--user-agent={Config.settings.browserSettings.UserAgent}");
            }

            return new ChromeDriver(options);
        }

        public IWebDriver StartIEBrowser()
        {
            var options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            options.IgnoreZoomLevel = true;
            if (Config.settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.Proxy = proxy;
                options.UsePerProcessProxy = true;
            }

            return new InternetExplorerDriver(options);
        }

        public IWebDriver StartEdgeBrowser()
        {
            var options = new EdgeOptions();
            return new EdgeDriver(options);
        }

        public IWebDriver StartPhantomJSBrowser()
        {
            var options = new PhantomJSOptions();

            if (Config.settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }

            return new PhantomJSDriver(options);
        }

        public IWebDriver StartSafariBrowser()
        {
            var options = new SafariOptions();

            // Add the WebDriver proxy capability.
            if (Config.settings.httpProxy.useProxy)
            {
                var proxy = new OpenQA.Selenium.Proxy();
                proxy.HttpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.SslProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                proxy.FtpProxy = Config.settings.httpProxy.proxyUrl + ":" + TestBase.proxy.proxyPort;
                options.AddAdditionalCapability("proxy", proxy);
            }

            return new SafariDriver(options);
        }

        public DesiredCapabilities GetCapabilitiesForBrowser(Browser browser)
        {
            switch (browser)
            {
                case Browser.PhantomJS:
                    return DesiredCapabilities.PhantomJS();
                case Browser.IE:
                    return DesiredCapabilities.InternetExplorer();
                case Browser.Edge:
                    return DesiredCapabilities.Edge();
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

            if (Config.settings.sauceLabsSettings.UseSauceLabs)
            {
                
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("username", Config.settings.sauceLabsSettings.SauceLabsUsername);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("accessKey", Config.settings.sauceLabsSettings.SauceLabsAPIKey);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("name", TestContext.CurrentContext.Test.FullName);
                Log.Message(string.Format("Starting {0}:{1} browser on SauceLabs : {2}", browser,
                    WebDriverTestBase.testData.browserInfo.capabilities,
                    Config.settings.sauceLabsSettings.SauceLabsUrl));
                var sauceLabs = new Uri(Config.settings.sauceLabsSettings.SauceLabsUrl);
                driver = new RemoteWebDriver(sauceLabs, WebDriverTestBase.testData.browserInfo.capabilities);
                 driver.Manage().Cookies.DeleteAllCookies();
                SetBrowserSize();
                return new EventedWebDriver(driver).driver;
            }

            if (host.Equals(Config.settings.browserStackSettings.BrowserStackRemoteURL))
            {
                var user = Config.settings.browserStackSettings.BrowserStack_User;
                var key = Config.settings.browserStackSettings.BrowserStack_Key;
                var os = Config.settings.browserStackSettings.BrowserStack_OS;
                var os_version = Config.settings.browserStackSettings.BrowserStack_OS_Version;

                if (user == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_User' is not defined in App.config");
                }
                if (key == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_Key' is not defined in App.config");
                }
                if (os == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_OS' is not defined in App.config");
                }
                if (os_version == null)
                {
                    throw new ConfigurationErrorsException(
                        "Framework configured to use BrowserStack, however 'BrowserStack_OS_Version' is not defined in App.config");
                }

                // Browser stack does not require a remote host port
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("browserstack.user", user);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("browserstack.key", key);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("os", os);
                WebDriverTestBase.testData.browserInfo.capabilities.SetCapability("os_version", os_version);

                URIStr = string.Format("http://{0}/wd/hub", host);
                Log.Message(string.Format("Starting {0} browser on host : {1}", browser, host));
            }
            else
            {
                URIStr = string.Format("http://{0}:{1}/wd/hub", host, Config.settings.runTimeSettings.RemoteHostPort);
                Log.Message(string.Format("Starting {0} browser on host : {1}:{2}", browser, host,
                    Config.settings.runTimeSettings.RemoteHostPort));
            }

            var remoteAddress = new Uri(URIStr);
            driver = new ScreenshotRemoteWebDriver(remoteAddress,
                WebDriverTestBase.testData.browserInfo.capabilities);
            driver.Manage().Cookies.DeleteAllCookies();
            SetBrowserSize();
            return new EventedWebDriver(driver).driver;
        }
    }
}