using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.Events;


namespace Golem.Framework
{
    public class WebDriverBrowser
    {
        public enum Browser { Firefox, Chrome, IE, Safari }
        public IWebDriver driver;
        public WebDriverBrowser() { }
        public static Browser getBrowserFromString(string name)
        {
            return (Browser)Enum.Parse(typeof(Browser), name);
        }
        public IWebDriver LaunchBrowser()
        {
              switch (Config.Settings.runTimeSettings.browser)
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
            var eDriver = new EventedWebDriver(driver);
            return eDriver.driver;

        }

        public static IWebDriver StartFirefoxBrowser()
        {
            return new FirefoxDriver();
        }

        public IWebDriver StartChromeBrowser()
        {
            return new ChromeDriver();
        }

        public IWebDriver StartIEBrowser()
        {
            return new InternetExplorerDriver();
        }

        public IWebDriver StartSafariBrowser()
        {
            return new SafariDriver();
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
                default:
                    return DesiredCapabilities.Firefox();
            }
        }

        public IWebDriver LaunchRemoteBrowser(Browser browser, string host)
        {
            DesiredCapabilities desiredCapabilities = DesiredCapabilities.Firefox();
            var remoteAddress = new Uri("http://"+ host +":4444/wd/hub");
            return new RemoteWebDriver(remoteAddress, desiredCapabilities);
        }
       
    }
}
