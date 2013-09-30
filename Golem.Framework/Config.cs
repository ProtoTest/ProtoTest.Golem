using System.Collections.Generic;
using System.Configuration;
using OpenQA.Selenium;

namespace Golem.Framework
{
    public class Config
    {
        public static ConfigSettings _settings;
        public static ConfigSettings Settings
        {
            get { return _settings ?? (_settings = new ConfigSettings()); }
            set { _settings = value; }
        }

        public static string GetConfigValue(string key, string defaultValue)
        {

            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
                return defaultValue;
            else
                return setting;
        }

    }

    public class ConfigSettings
    {
        public RuntimeSettings runTimeSettings;
        public ReportSettings reportSettings;
        public HttpProxy httpProxy;
        public LocalProxy localProxy;  //added to use local proxy testing for ebags
        public AppiumSettings appiumSettings;

        public ConfigSettings()
        {
            runTimeSettings = new RuntimeSettings();
            reportSettings = new ReportSettings();
            httpProxy = new HttpProxy();
            localProxy = new LocalProxy(); //added by seth
            appiumSettings = new AppiumSettings();
        }

        public class RuntimeSettings
        {
            public List<WebDriverBrowser.Browser> Browsers = new List<WebDriverBrowser.Browser>(); 
            public bool LaunchBrowser;
            public int TestTimeoutMin;
            public int ElementTimeoutSec;
            public int PageTimeoutSec;
            public string EnvironmentUrl;
            public int DegreeOfParallelism;
            public int CommandDelayMs;
            public bool RunOnRemoteHost;
            public string HostIp;
            public bool HighlightOnFind;
            

            public RuntimeSettings()
            {
            Browsers = GetBrowserList();

            LaunchBrowser = Common.IsTruthy(Config.GetConfigValue("LaunchBrowser", "True"));
            TestTimeoutMin = int.Parse(Config.GetConfigValue("TestTimeoutMin","5"));
            ElementTimeoutSec =int.Parse(Config.GetConfigValue("ElementTimeoutSec","20"));
            PageTimeoutSec = int.Parse(Config.GetConfigValue("PageTimeoutSec","30"));
            EnvironmentUrl = Config.GetConfigValue("EnvironmentUrl","");
            DegreeOfParallelism = int.Parse(Config.GetConfigValue("DegreeOfParallelism", "5"));
            CommandDelayMs = int.Parse(Config.GetConfigValue("CommandDelayMs", "0"));
            RunOnRemoteHost = Common.IsTruthy(Config.GetConfigValue("RunOnRemoteHost", "False"));
            HostIp = Config.GetConfigValue("HostIp", "localhost");
            HighlightOnFind = Common.IsTruthy(Config.GetConfigValue("HighlightOnFind", "False"));
            }

            

            private List<WebDriverBrowser.Browser> GetBrowserList()
            {
                List<WebDriverBrowser.Browser> browsers = new List<WebDriverBrowser.Browser>();
                string browser = Config.GetConfigValue("Browser", "null");
                if (browser != "null")
                    browsers.Add(WebDriverBrowser.getBrowserFromString(browser));
                for (var i = 1; i < 5; i++)
                {
                    browser = Config.GetConfigValue("Browser" + i, "null");
                    if (browser != "null")
                        browsers.Add(WebDriverBrowser.getBrowserFromString(browser));
                }
                if(browsers.Count==0)
                    browsers.Add(WebDriverBrowser.Browser.Firefox);
                return browsers;
            }

            public int GetTimeoutSettings()
            {
                return ElementTimeoutSec;
            }
        }
        public class ReportSettings
        {

            public bool htmlOnError;
            public bool screenshotOnError;
            public bool videoRecordingOnError;
            public bool commandLogging;
            public bool actionLogging;
            public bool spellChecking;

            public ReportSettings()
            {
                htmlOnError = Common.IsTruthy(Config.GetConfigValue("HtmlOnError", "True"));
                screenshotOnError = Common.IsTruthy(Config.GetConfigValue("ScreenshotOnError","True"));
                videoRecordingOnError = Common.IsTruthy(Config.GetConfigValue("VideoRecordingOnError", "True"));
                commandLogging = Common.IsTruthy(Config.GetConfigValue("CommandLogging","True"));
                actionLogging= Common.IsTruthy(Config.GetConfigValue("ActionLogging","True"));
                spellChecking = Common.IsTruthy(Config.GetConfigValue("SpellChecking", "False"));
            }
            
        }
        public class HttpProxy
        {
            public bool startProxy;
            public int proxyPort;
            public int sslProxyPort;
            public HttpProxy()
            {
                startProxy = Common.IsTruthy(Config.GetConfigValue("StartFiddlerProxy", "True"));
                proxyPort = int.Parse(Config.GetConfigValue("ProxyPort", "8876"));
                sslProxyPort = int.Parse(Config.GetConfigValue("SslProxyPort", "7777"));
            }

          }

        public class LocalProxy
        {
            public bool localProxy;
            public int localPort;
            public string localHost;
            public LocalProxy()
            {
                localProxy = Common.IsTruthy(Config.GetConfigValue("UseLocalProxy", "False"));
                localPort = int.Parse(Config.GetConfigValue("ProxyPort", "8888"));
                localHost = Config.GetConfigValue("HostIP", "localhost");
            }
        }
        public class AppiumSettings
        {
            public bool launchApp = false;
            public string appPath;
            public string package;
            public string activity;
            public string appOs;
            public AppiumSettings()
            {
                launchApp = Common.IsTruthy(Config.GetConfigValue("LaunchApp", "False"));
                appPath = Config.GetConfigValue("AppPath", "");
                package = Config.GetConfigValue("AppPackage", "");
                activity = Config.GetConfigValue("AppActivity", "");
                appOs = Config.GetConfigValue("AppOs", "Android");
            }
        }

        
      
    }
}
