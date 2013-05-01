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


namespace Golem
{
    public class WebDriverBrowser
    {
        public enum Browser { Firefox, Chrome, IE, Safari, Remote }
        public IWebDriver driver;
        public WebDriverBrowser() { }
        public IWebDriver LaunchBrowser()
        {
            var browser = Config.RuntimeSettings.browser;
            
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
                case Browser.Remote:
                    driver = StartRemoteBrowser();
                    break;
                default:
                    driver = LaunchFirefox();
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

        public IWebDriver StartRemoteBrowser()
        {
            return new FirefoxDriver();
        }

        public IWebDriver LaunchFirefox()
        {
            return new FirefoxDriver();
        }

       
    }
}
